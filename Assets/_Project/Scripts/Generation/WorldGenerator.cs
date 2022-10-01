using UnityEngine;

using EDirection = Project.Generation.WorldGeneratorConfig.EDirection;

namespace Project.Generation
{
    public class WorldGenerator
    {
        public Vector2Int Position { get; private set; }
        private float _direction;
        public int StepCount { get; private set; }
        
        public WorldGenerator(Vector2Int pPosition, int pStepCount)
        {
            Position = pPosition;
            _direction = Mathf.RoundToInt(Random.value * 4f) * 90f;
            StepCount = pStepCount;
        }

        public void Move(EDirection pDirection)
        {
            _direction = pDirection switch
            {
                EDirection.Right => (_direction + 90) % 360,
                EDirection.Reverse => (_direction + 180) % 360,
                EDirection.Left => (_direction + 270) % 360,
                _ => _direction
            };

            Position = new Vector2Int(
                Position.x + Mathf.RoundToInt(Mathf.Cos(_direction * Mathf.Deg2Rad)),
                Position.y + Mathf.RoundToInt(Mathf.Sin(_direction * Mathf.Deg2Rad)));

            StepCount++;
        }
    }
}
