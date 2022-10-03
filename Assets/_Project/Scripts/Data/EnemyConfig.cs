using JvLib.Data;
using UnityEngine;

namespace Project.Enemies
{
    public class EnemyConfig : DataEntry
    {
        [SerializeField] private EnemyBehaviour _Prefab;
        public EnemyBehaviour Prefab => _Prefab;
        [SerializeField] private float _Speed;
        public float Speed => _Speed;

        [SerializeField] private float _BaseHealth = 1f;
        [SerializeField, Range(1f, 2f)] private float _HealthMult = 1.1f;
        [SerializeField, Range(1f, 2f)] private float _HealthMaxMult = 1.1f;

        public float GetHealth(int pWave)
        {
            return _BaseHealth + Mathf.Max(Mathf.Pow(_HealthMult, pWave), _HealthMaxMult * pWave);
        }

        [Space]
        [SerializeField] private float _Damage;
        public float Damage => _Damage;
    }
}
