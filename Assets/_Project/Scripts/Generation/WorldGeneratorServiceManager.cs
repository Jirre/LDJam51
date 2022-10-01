using System;
using System.Collections.Generic;
using System.Linq;
using JvLib.Events;
using JvLib.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Generation
{
    [ServiceInterface(Name = "Generator")]
    public class WorldGeneratorServiceManager : MonoBehaviour, IService
    {
        [SerializeField] private WorldGeneratorConfig _Config;
        private float _maxCells;
        
        public bool IsServiceReady { get; private set; }

        private List<WorldGenerator> _generators;
        private Dictionary<Vector2, WorldCell> _cells;

        private List<Vector2Int> _endPoints;

        public Vector2Int MinCoordinate { get; private set; }
        public Vector2Int MaxCoordinate { get; private set; }

        private SafeEvent _onBuildFinish = new SafeEvent();
        public event Action OnBuildFinish
        {
            add => _onBuildFinish += value;
            remove => _onBuildFinish -= value;
        }

        private enum EStates
        {
            Init,
            GenerateGround,
            Pathing,
            RenderGround,
            ResourcePlacing,
            Complete,
        }

        private EventStateMachine<EStates> _stateMachine;

        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
            IsServiceReady = true;

            _stateMachine = new EventStateMachine<EStates>(nameof(WorldGeneratorServiceManager));
            _stateMachine.Add(EStates.Init, InitState);
            _stateMachine.Add(EStates.GenerateGround, GenerateGroundState);
            _stateMachine.Add(EStates.Pathing, PathingState);
            _stateMachine.Add(EStates.RenderGround, RenderGroundState);
            _stateMachine.Add(EStates.Complete, CompleteState);

            _stateMachine.GotoState(EStates.Init);
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void InitState(EventState<EStates> pState)
        {
            Random.InitState((int)DateTime.Now.Ticks);

            _cells = new Dictionary<Vector2, WorldCell>();
            _endPoints = new List<Vector2Int>();
            
            _generators = new List<WorldGenerator>();
            _generators.Add(new WorldGenerator(Vector2Int.zero, 0));
            _cells.Add(Vector2Int.zero, new WorldCell(Vector2Int.zero, transform, 0));

            _maxCells = _Config.MaxFloors;

            Debug.Log(DateTime.Now.ToString("HH:mm:ss.ffffff") + ": Starting build process!");
            pState.GotoState(EStates.GenerateGround);
        }

        private void GenerateGroundState(EventState<EStates> pState)
        {
            int count = _generators.Count;
            for(int i = 0; i < count; i++)
            {
                _generators[i].Move(_Config.GetDirection());

                Vector2Int[] grid = _Config.GetGrid();
                foreach (Vector2Int pos in grid)
                {
                    int cost = (pos.x == 0 ? 0 : 1) + (pos.y == 0 ? 0 : 1);
                    Vector2Int rPos = pos + _generators[i].Position;
                    if (!_cells.ContainsKey(rPos))
                        _cells.Add(
                            rPos, 
                            new WorldCell(rPos, 
                            transform, 
                            _generators[i].StepCount + cost));

                    MinCoordinate = new Vector2Int(
                        Mathf.Min(MinCoordinate.x, rPos.x),
                        Mathf.Min(MinCoordinate.y, rPos.y));
                    MaxCoordinate = new Vector2Int(
                        Mathf.Max(MaxCoordinate.x, rPos.x),
                        Mathf.Max(MaxCoordinate.y, rPos.y));
                }
                
                if (_Config.ShouldSpawnGenerator(_generators.Count))
                {
                    //Add new Generator but dont increase count this iteration
                    //New Diggers should wait one iteration before acting
                    _generators.Add(new WorldGenerator(_generators[i].Position, _generators[i].StepCount));
                }

                if (_Config.ShouldRemoveGenerator(_generators.Count))
                {
                    //Remove current digger from list, as well as reduce count by 1 to prevent an out of bound error
                    count--;
                    _endPoints.Add(_generators[i].Position);
                    _generators.RemoveAt(i--);
                }
            }

            if (_cells.Count >= _maxCells)
            {
                foreach (WorldGenerator generator in _generators)
                {
                    _endPoints.Add(generator.Position);
                }
                _generators.Clear();
            }

            if (_generators.Count != 0) return;
            
            Debug.Log(DateTime.Now.ToString("HH:mm:ss.ffffff") + ": Generate complete!");
            pState.GotoState(EStates.Pathing);
        }

        private void PathingState(EventState<EStates> pState)
        {
            int paths = Random.Range(1, Mathf.Max(1, _Config.MaxRoads + 1));
            List<Vector2Int> spawnPoints = _endPoints.OrderByDescending(x =>
                _cells[x].Cost).ToList();

            for (int i = 0; i < Mathf.Min(spawnPoints.Count, paths); i++)
            {
                Vector2Int pos = spawnPoints[i];
                _cells[pos].SetContent(EWorldCellContent.Spawn);
                
                while (pos != Vector2Int.zero)
                {
                    Vector2Int previousPos = pos;
                    IEnumerable<WorldCell> neighbors = GetNeighbors(pos);
                    float cost = _cells[pos].Cost;
                    foreach (WorldCell worldCell in neighbors)
                    {
                        if (worldCell.Cost > cost || 
                            worldCell.Content == EWorldCellContent.Road) continue;
                        
                        pos = worldCell.Position;
                        cost = worldCell.Cost;
                    }
                    _cells[pos].SetContent(EWorldCellContent.Road);
                    if (pos == previousPos)
                        break;
                }
            }

            pState.GotoState(EStates.RenderGround);
        }

        private void RenderGroundState(EventState<EStates> pState)
        {
            foreach (KeyValuePair<Vector2, WorldCell> cell in _cells)
            {
                GroundConfig config = GroundConfigs.GetConfig(cell.Value.Content);
                byte context = 0;
                if (_cells.ContainsKey(cell.Key + Vector2Int.right) && config.DoesConnect(_cells[cell.Key + Vector2Int.right].Content)) context += 1;
                if (_cells.ContainsKey(cell.Key + Vector2Int.up) && config.DoesConnect(_cells[cell.Key + Vector2Int.up].Content)) context += 2;
                if (_cells.ContainsKey(cell.Key + Vector2Int.left) && config.DoesConnect(_cells[cell.Key + Vector2Int.left].Content)) context += 4;
                if (_cells.ContainsKey(cell.Key + Vector2Int.down) && config.DoesConnect(_cells[cell.Key + Vector2Int.down].Content)) context += 8;
                
                cell.Value.SetGround(config, context);
            }

            _onBuildFinish.Dispatch();
            pState.GotoState(EStates.Complete);
        }

        private void CompleteState(EventState<EStates> pState)
        {
            //FlagState
        }

        private IEnumerable<WorldCell> GetNeighbors(Vector2Int pPosition)
        {
            List<WorldCell> cells = new();
            if (_cells.ContainsKey(pPosition + Vector2Int.right)) cells.Add(_cells[pPosition + Vector2Int.right]);
            if (_cells.ContainsKey(pPosition + Vector2Int.up)) cells.Add(_cells[pPosition + Vector2Int.up]);
            if (_cells.ContainsKey(pPosition + Vector2Int.left)) cells.Add(_cells[pPosition + Vector2Int.left]);
            if (_cells.ContainsKey(pPosition + Vector2Int.down)) cells.Add(_cells[pPosition + Vector2Int.down]);

            return cells.ToArray();
        }
    }
}
