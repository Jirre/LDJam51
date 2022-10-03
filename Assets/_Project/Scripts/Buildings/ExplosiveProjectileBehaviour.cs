using Project.Enemies;
using UnityEngine;

namespace Project.Buildings
{
    public class ExplosiveProjectileBehaviour : ProjectileBehaviour
    {
        [SerializeField] private float _BlastRadius;
        [SerializeField] private LayerMask _LayerMask;
        [SerializeField] private ParticleSystem _System;
        
        protected override void OnImpact()
        {
            Collider[] cols = Physics.OverlapSphere(TargetPos, _BlastRadius, _LayerMask);
            foreach (Collider col in cols)
            {
                EnemyBehaviour behaviour = col.GetComponent<EnemyBehaviour>();
                if (behaviour == null || behaviour.IsDead)
                    continue;
                
                behaviour.Damage(Damage);
            }
            GameObject obj = Instantiate(_System.gameObject, TargetPos, Quaternion.identity);
            Destroy(obj, 1.5f);
        }
    }
}
