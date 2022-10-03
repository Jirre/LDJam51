using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JvLib.Events;
using JvLib.Generation.Noise;
using JvLib.Services;
using Project.Buildings;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Generation
{
    [ServiceInterface(Name = "World")]
    public partial class WorldServiceManager : MonoBehaviour, IService
    {
        [SerializeField] private WorldGeneratorConfig _Config;
        private float _maxCells;

        private NoiseHash _woodHash;
        private SimplexNoise2D _woodNoiseGenerator;
        private NoiseHash _mountainHash;
        private SimplexNoise2D _mountainNoiseGenerator;
        
        public bool IsServiceReady { get; private set; }

        private List<WorldGenerator> _generators;
        private Dictionary<Vector2Int, WorldCell> _cells;
        public bool TryGetCell(Vector2Int pCoordinate, out WorldCell pCell)
        {
            pCell = null;
            if (!_cells.ContainsKey(pCoordinate)) return false;
            
            pCell = _cells[pCoordinate];
            return true;

        }

        private List<Vector2Int> _endPoints;

        public Vector2Int MinCoordinate { get; private set; }
        public Vector2Int MaxCoordinate { get; private set; }


        private enum EStates
        {
            Init,
            GenerateGround,
            Pathing,
            ResourcePlacing,
            Rendering,
            
            Complete,
        }

        private EventStateMachine<EStates> _stateMachine;

        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
            IsServiceReady = true;

            _stateMachine = new EventStateMachine<EStates>(nameof(WorldServiceManager));
            _stateMachine.Add(EStates.Init, InitState);
            _stateMachine.Add(EStates.GenerateGround, GenerateGroundState);
            _stateMachine.Add(EStates.Pathing, PathingState);
            _stateMachine.Add(EStates.ResourcePlacing, GenerateResourcesState);
            _stateMachine.Add(EStates.Rendering, RenderingState);
            _stateMachine.Add(EStates.Complete, CompleteState);

            _stateMachine.GotoState(EStates.Init);
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void OnDestroy()
        {
            _woodHash.Dispose();
            _mountainHash.Dispose();
        }

        private void InitState(EventState<EStates> pState)
        {
            Random.InitState((int)DateTime.Now.Ticks);

            DOTween.SetTweensCapacity(250, 500);
            
            _isFreshWorld = true;
            _cells = new Dictionary<Vector2Int, WorldCell>();
            _maxCells = _Config.MaxFloors;

            _woodHash = NoiseHash.Generate((int)DateTime.Now.Ticks, 128);
            _woodNoiseGenerator = new SimplexNoise2D(
                _woodHash,
                _Config.NoiseFrequency,
                _Config.NoiseOctaves,
                _Config.NoiseLacunarity,
                _Config.NoisePersistence,
                0f, 100f);
            
            _mountainHash = NoiseHash.Generate(int.MaxValue - (int)DateTime.Now.Ticks, 128);
            _mountainNoiseGenerator = new SimplexNoise2D(
                _mountainHash,
                _Config.NoiseFrequency,
                _Config.NoiseOctaves,
                _Config.NoiseLacunarity,
                _Config.NoisePersistence,
                0f, 100f);
            
            Generate(Vector2Int.zero, 0);
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
