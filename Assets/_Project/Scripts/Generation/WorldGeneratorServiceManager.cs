using System;
using System.Collections.Generic;
using System.Linq;
using JvLib.Data;
using JvLib.Events;
using JvLib.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Generation
{
    [ServiceInterface(Name = "Generator")]
    public class WorldGeneratorServiceManager : MonoBehaviour, IService
    {
        [SerializeField] private WorldGeneratorConfig _config;
        private float _maxCells;
        
        public bool IsServiceReady { get; private set; }

        private List<WorldGenerator> _generators;
        private Dictionary<Vector2, WorldCell> _cells;

        private List<Vector2Int> _endPoints;

        

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
            Random.InitState((int)System.DateTime.Now.Ticks);

            _generators = new List<WorldGenerator>();
            _generators.Add(new WorldGenerator(Vector2Int.zero, 0));

            _maxCells = _config.MaxFloors;
            
            _cells = new Dictionary<Vector2, WorldCell>();
            _endPoints = new List<Vector2Int>();
            
            Debug.Log(System.DateTime.Now.ToString("HH:mm:ss.ffffff") + ": Starting build process!");
            pState.GotoState(EStates.GenerateGround);
        }

        private void GenerateGroundState(EventState<EStates> pState)
        {
            int count = _generators.Count;
            for(int i = 0; i < count; i++)
            {
                _generators[i].Move(_config.GetDirection());

                Vector2Int[] grid = _config.GetGrid();
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
                }
                
                if (_config.ShouldSpawnGenerator(_generators.Count))
                {
                    //Add new Generator but dont increase count this iteration
                    //New Diggers should wait one iteration before acting
                    _generators.Add(new WorldGenerator(_generators[i].Position, _generators[i].StepCount));
                }

                if (_config.ShouldRemoveGenerator(_generators.Count))
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
            
            Debug.Log(System.DateTime.Now.ToString("HH:mm:ss.ffffff") + ": Generate complete!");
            pState.GotoState(EStates.RenderGround);
        }

        private void PathingState(EventState<EStates> pState)
        {
            int paths = Random.Range(1, Mathf.Max(1, _config.MaxRoads + 1));
            Vector2Int[] spawnPoints = _endPoints.Where(x =>
                _cells[x].Cost >= _config.MinRoadLength).ToArray();

            if (spawnPoints.Length <= 0)
                _endPoints.CopyTo(spawnPoints);
        }

        private void RenderGroundState(EventState<EStates> pState)
        {
            foreach (KeyValuePair<Vector2, WorldCell> cell in _cells)
            {
                EWorldCellContent content = cell.Value.Content;
                byte context = 0;
                if (_cells.ContainsKey(cell.Key + Vector2Int.right) && _cells[cell.Key + Vector2Int.right]?.Content == content) context += 1;
                if (_cells.ContainsKey(cell.Key + Vector2Int.up) && _cells[cell.Key + Vector2Int.up].Content == content) context += 2;
                if (_cells.ContainsKey(cell.Key + Vector2Int.left) && _cells[cell.Key + Vector2Int.left]?.Content == content) context += 4;
                if (_cells.ContainsKey(cell.Key + Vector2Int.down) && _cells[cell.Key + Vector2Int.down]?.Content == content) context += 8;
                
                cell.Value.SetGround(GroundConfigs.GetConfig(content), context);
            }

            pState.GotoState(EStates.Complete);
        }

        private void CompleteState(EventState<EStates> pState)
        {
            //FlagState
        }
    }
}
