using JvLib.Data;
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

        #region --- Noise Generation ---

        [Space]
        [Tooltip("Frequency of the Noise Generator")] 
        [SerializeField] [BoxGroup("Noise Generation")] private float _Frequency = 4;
        public float NoiseFrequency => _Frequency;
        [Tooltip("Octaves of the Noise Generator")] 
        [SerializeField] [BoxGroup("Noise Generation"), Range(1, 8)] private int _Octaves = 1;
        public int NoiseOctaves => _Octaves;
        [Tooltip("Lacunarity of the Noise Generator")] 
        [SerializeField] [BoxGroup("Noise Generation"), Range(1f, 4f)] private float _Lacunarity = 2f;
        public float NoiseLacunarity => _Lacunarity;
        [Tooltip("Persistence of the Noise Generator")] 
        [SerializeField] [BoxGroup("Noise Generation"), Range(0f, 1f)] private float _Persistence = 0.5f;
        public float NoisePersistence => _Persistence;
        
        [Space]
        [Tooltip("The Chance of forests to be placed")] 
        [SerializeField] [BoxGroup("Noise Generation"), Range(0, 100)] private float _ForestChance = 25f;
        public float ForestChance => _ForestChance;
        [Tooltip("The Chance of Mountains to be placed")] 
        [SerializeField] [BoxGroup("Noise Generation"), Range(0, 100)] private float _MountainThreshold = 25f;
        public float MountainThreshold => _MountainThreshold;
        [Tooltip("The Chance of Crystals to be placed within the mountains")] 
        [SerializeField] [BoxGroup("Noise Generation"), Range(0, 100), Indent] private float _CrystalChance = 5f;
        public float CrystalChance => _CrystalChance;
        [Tooltip("The Chance of another Crystals to be placed is affected by every crystal placed")]
        [SerializeField] [BoxGroup("Noise Generation"), Range(-100, 100), Indent] private float _CrystalPerCrystalChance = 5f;
        public float CrystalPerCrystalChance => _CrystalPerCrystalChance;
        [Tooltip("The Chance of another Crystals to be placed is affected by every stone placed")]
        [SerializeField] [BoxGroup("Noise Generation"), Range(-100, 100), Indent] private float _CrystalPerStoneChance = 5f;
        public float CrystalPerStoneChance => _CrystalPerStoneChance;

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
        public int MinRoadLength => _MinRoadLength;
    }
}
