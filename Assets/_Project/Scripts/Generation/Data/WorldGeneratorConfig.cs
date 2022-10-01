using JvLib.Data;
using JvLib.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Generation
{
    public class WorldGeneratorConfig : DataEntry
    {
        #region --- Direction ---

        [Tooltip("Chance that a generator continues its trajectory")]
        [SerializeField] [Range(0, 100), BoxGroup("Direction")] private float _ForwardChance = 50;
        [Tooltip("Chance that a generator turns 90 degrees to the left")]
        [SerializeField] [Range(0, 100), BoxGroup("Direction")] private float _TurnLeftChance = 50;
        [Tooltip("Chance that a generator turns 90 degrees to the right")]
        [SerializeField] [Range(0, 100), BoxGroup("Direction")] private float _TurnRightChance = 50;
        [Tooltip("Chance that a generator turns 180 degrees backwards")]
        [SerializeField] [Range(0, 100), BoxGroup("Direction")] private float _ReverseChance = 50;
        
        private float DirectionTotal => _ForwardChance + _TurnLeftChance + _TurnRightChance + _ReverseChance;

        public enum EDirection
        {
            Forward,
            Left,
            Right,
            Reverse
        }

        /// <summary>
        /// Calculate a new direction corresponding with the given ranges
        /// </summary>
        /// <returns>The new Direction to move in</returns>
        public EDirection GetDirection()
        {
            float value = Random.value;
            if (value <= _ForwardChance / DirectionTotal) 
                return EDirection.Forward;
            value -= (_ForwardChance / DirectionTotal);

            if (value <= _TurnLeftChance / DirectionTotal) 
                return EDirection.Left;
            value -= (_TurnLeftChance / DirectionTotal);

            return value <= _TurnRightChance / DirectionTotal ? EDirection.Right : EDirection.Reverse;
        }

        #endregion

        #region --- Creation ---

        [Space]
        [Tooltip("Chance that a generator spawns a 1x1 grid of ground every step")]
        [SerializeField] [Range(0, 100), BoxGroup("Creation")] private float _1X1Chance = 50;
        [Tooltip("Chance that a generator spawns a 2x2 grid of ground every step")]
        [SerializeField] [Range(0, 100), BoxGroup("Creation")] private float _2X2Chance = 50;
        [Tooltip("Chance that a generator spawns a 3x3 grid of ground every step")]
        [SerializeField] [Range(0, 100), BoxGroup("Creation")] private float _3X3Chance = 50;
        
        private float CreationTotal => _1X1Chance + _2X2Chance + _3X3Chance;
        
        /// <summary>
        /// Calculate a new direction corresponding with the given ranges
        /// </summary>
        /// <returns>The new Direction to move in</returns>
        public Vector2Int[] GetGrid()
        {
            float value = Random.value;
            if (value <= _1X1Chance / CreationTotal) 
                return new [] {Vector2Int.zero };
            
            value -= (_1X1Chance / CreationTotal);
            if (value <= _2X2Chance / CreationTotal)
            {
                int x = Mathf.RoundToInt(Mathf.Sign(Random.value - .5f));
                int y = Mathf.RoundToInt(Mathf.Sign(Random.value - .5f));
                
                return new []
                {
                    new Vector2Int(0, 0), new Vector2Int(x, 0),
                    new Vector2Int(0, y), new Vector2Int(x, y),
                };
            }

            return new[]
            {
                new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1),
                new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0),
                new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1),
            };
        }

        #endregion

        #region --- Life-Cycle ---

        [Space]
        [Tooltip("Base chance that a generator spawns a new spawner")]
        [SerializeField] [Range(-100, 100), BoxGroup("Life-Cycle")] private float _BaseSpawnChance = 0;
        [Tooltip("Added Chance that a generator spawns a new spawner based on the number of active generators")]
        [SerializeField] [Range(-100, 100), BoxGroup("Life-Cycle")] private float _AddedSpawnChance = 0;

        public bool ShouldSpawnGenerator(int pActiveGenerators)
        {
            return Random.Range(0, 100) < _BaseSpawnChance + _AddedSpawnChance * pActiveGenerators;
        }
        
        [Space]
        [Tooltip("Base chance that a generator is removed")]
        [SerializeField] [Range(-100, 100), BoxGroup("Life-Cycle")] private float _BaseRemoveChance = 0;
        [Tooltip("Added Chance that a generator is removed based on the number of active generators")]
        [SerializeField] [Range(-100, 100), BoxGroup("Life-Cycle")] private float _AddedRemoveChance = 0;

        public bool ShouldRemoveGenerator(int pActiveGenerators)
        {
            return Random.Range(0, 100) < _BaseRemoveChance + _AddedRemoveChance * pActiveGenerators;
        }
        
        #endregion
        
        [Space]
        [Tooltip("Maximum number of tiles to spawn during creation")]
        [SerializeField] private int _MaxFloors = 100;
        public int MaxFloors => _MaxFloors;
        
        [Tooltip("Maximum number of roads spawned during creation")]
        [SerializeField] private int _MaxRoads;
        public int MaxRoads => _MaxRoads;

        [Tooltip("Minimum road length")]
        [SerializeField] private int _MinRoadLength;
        public int MinRoadLength;
    }
}
