using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JvLib.Events;
using JvLib.Services;
using Project.Buildings;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Generation
{
    public partial class WorldServiceManager //Generation
    {
        private SafeEvent _onBuildFinish = new SafeEvent();
        public event Action OnBuildFinish
        {
            add => _onBuildFinish += value;
            remove => _onBuildFinish -= value;
        }

        private bool _isFreshWorld;

        public void Generate(Vector2Int pPosition, int pAddedMaxCells)
        {
            if (!_stateMachine.IsCurrentState(EStates.Init) &&
                !_stateMachine.IsCurrentState(EStates.Complete))
                throw new Exception("Can't start construction of a new world while another build is in progress");

            _maxCells += pAddedMaxCells;
            _endPoints = new List<Vector2Int>();
            
            _generators = new List<WorldGenerator> {new WorldGenerator(pPosition, 0)};
            if (!_cells.ContainsKey(pPosition))
                _cells.Add(Vector2Int.zero, new WorldCell(pPosition, transform));
            _stateMachine.GotoState(EStates.GenerateGround);
        }

        public void Clear()
        {
            if (!_stateMachine.IsCurrentState(EStates.Init) &&
                !_stateMachine.IsCurrentState(EStates.Complete))
                throw new Exception("Can't start clearing of a world while another build is in progress");

            _maxCells = _Config.MaxFloors;
            foreach (KeyValuePair<Vector2Int, WorldCell> kv in _cells)
            {
                kv.Value.Dispose(Mathf.Ceil(Vector2Int.Distance(kv.Key, Vector2Int.zero)));
            }
            
            _cells.Clear();
            MinCoordinate = Vector2Int.zero;
            MaxCoordinate = Vector2Int.zero;
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
                            transform));

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
            pState.GotoState(EStates.ResourcePlacing);
        }
        
        private void GenerateResourcesState(EventState<EStates> pState)
        {
            float[,] woods = _woodNoiseGenerator.CalcArray(
                new float2(MinCoordinate.x, MinCoordinate.y),
                new Vector2Int(
                    Mathf.Abs(MinCoordinate.x) + MaxCoordinate.x + 1,
                    Mathf.Abs(MinCoordinate.y) + MaxCoordinate.y + 1));
            
            float[,] mountains = _mountainNoiseGenerator.CalcArray(
                new float2(MinCoordinate.x, MinCoordinate.y),
                new Vector2Int(
                    Mathf.Abs(MinCoordinate.x) + MaxCoordinate.x + 1,
                    Mathf.Abs(MinCoordinate.y) + MaxCoordinate.y + 1));

            float crystalChance = _Config.CrystalChance;
            
            foreach (KeyValuePair<Vector2Int, WorldCell> cell in _cells)
            {
                if (cell.Value.Content != EWorldCellContent.Unassigned)
                    continue;
                
                int aX = Mathf.Abs(MinCoordinate.x) + cell.Key.x;
                int aY = Mathf.Abs(MinCoordinate.y) + cell.Key.y;
                float mV = mountains[aX, aY];
                
                if (mV >= 100f - _Config.MountainThreshold)
                {
                    if (Random.Range(0, 100) < crystalChance)
                    {
                        cell.Value.SetContent(EWorldCellContent.Crystals);
                        crystalChance += _Config.CrystalPerCrystalChance;
                        continue;
                    }
                    cell.Value.SetContent(EWorldCellContent.Stones);
                    crystalChance += _Config.CrystalPerStoneChance;
                    continue;
                }
                
                float fV = woods[aX, aY];
                if (fV >= 100 - _Config.ForestChance)
                {
                    cell.Value.SetContent(EWorldCellContent.Trees);
                    continue;
                }
                
                cell.Value.SetContent(EWorldCellContent.Empty);
            }
            pState.GotoState(EStates.Pathing);
        }
        
        private void PathingState(EventState<EStates> pState)
        {
            ResetPathingCost();
            SetPathingCost(Vector2Int.zero, 0);
            
            int paths = Random.Range(1, Mathf.Max(1, _Config.MaxRoads + 1));
            List<Vector2Int> spawnPoints = _endPoints.OrderByDescending(x =>
                _cells[x].Cost).ToList();

            for (int i = 0; i < Mathf.Min(spawnPoints.Count, paths); i++)
            {
                Vector2Int pos = spawnPoints[i];
                if (_cells[pos].Cost < _Config.MinRoadLength)
                {
                    paths++;
                    continue;
                }
                
                _cells[pos].SetContent(EWorldCellContent.Spawn);
                List<Vector2Int> steps = new() {pos};
                while (pos != Vector2Int.zero)
                {
                    Vector2Int previousPos = pos;
                    IEnumerable<WorldCell> neighbors = GetNeighbors(pos);
                    float cost = _cells[pos].Cost;
                    foreach (WorldCell worldCell in neighbors)
                    {
                        if (worldCell.Cost > cost) continue;
                        
                        pos = worldCell.Position;
                        cost = worldCell.Cost;
                    }
                    _cells[pos].SetContent(EWorldCellContent.Road);
                    if (pos == previousPos)
                        break;
                    
                    steps.Add(pos);
                }
                
                Svc.Gameplay.AddPath(steps.ToArray());
            }

            pState.GotoState(EStates.Rendering);
        }

        private void ResetPathingCost()
        {
            foreach (WorldCell c in _cells.Values)
            {
                c.SetCost(int.MaxValue);
            }
        }
        
        private void SetPathingCost(Vector2Int pPos, int pCost)
        {
            List<WorldCell> cells = GetNeighbors(pPos).ToList();
            _cells[pPos].SetCost(pCost);
            foreach (WorldCell c in cells)
            {
                int nCost = c.Content switch
                {
                    EWorldCellContent.Base => 0,
                    EWorldCellContent.Empty => pCost + 2,
                    EWorldCellContent.Road => pCost + 1,
                    EWorldCellContent.Spawn => int.MaxValue,
                    EWorldCellContent.Building => int.MaxValue,
                    _ => pCost + 100
                };
                if (c.Cost > nCost)
                {
                    SetPathingCost(c.Position, nCost);
                }
            }
        }

        private void RenderingState(EventState<EStates> pState)
        {
            foreach (KeyValuePair<Vector2Int, WorldCell> cell in _cells)
            {
                if (cell.Value.Content == EWorldCellContent.Building)
                    continue;
                
                GroundConfig config = GroundConfigs.GetConfig(cell.Value.Content);
                byte context = 0;
                if (_cells.ContainsKey(cell.Key + Vector2Int.right) && config.DoesConnect(_cells[cell.Key + Vector2Int.right].Content)) context += 1;
                if (_cells.ContainsKey(cell.Key + Vector2Int.up) && config.DoesConnect(_cells[cell.Key + Vector2Int.up].Content)) context += 2;
                if (_cells.ContainsKey(cell.Key + Vector2Int.left) && config.DoesConnect(_cells[cell.Key + Vector2Int.left].Content)) context += 4;
                if (_cells.ContainsKey(cell.Key + Vector2Int.down) && config.DoesConnect(_cells[cell.Key + Vector2Int.down].Content)) context += 8;
                
                cell.Value.SetGround(config, context);
            }

            _onBuildFinish.Dispatch();

            if (_isFreshWorld)
            {
                _isFreshWorld = false;
                
                GameObject obj = new("Base")
                {
                    transform =
                    {
                        position = Vector3.zero
                    }
                };
                obj.AddComponent<BuildingBehaviour>().SetConfig(BuildingConfig.Base_01);
            }
            pState.GotoState(EStates.Complete);
        }
    }
}
