using JvLib.Pooling.Data.Objects;
using UnityEngine;

namespace Project.Buildings
{
    public class TurretConfig : BuildingConfig
    {
        [SerializeField] private float _Cooldown;
        public float Cooldown => _Cooldown;
        [SerializeField] private PooledObjectConfig _Projectile;
        public PooledObjectConfig Projectile => _Projectile;

        [SerializeField] private float _Damage;
        public float Damage => _Damage;
        [SerializeField] private float _Range;
        public float Range => _Range;
    }
}
