using Project.Enemies;
using UnityEngine;

namespace Project.Buildings
{
    public class ExplosiveProjectileBehaviour : ProjectileBehaviour
    {
        [SerializeField] private LayerMask _LayerMask;

        protected override void OnImpact()
        {
            Collider[] cols = Physics.OverlapSphere(TargetPos, BlastRadius, _LayerMask);
            foreach (Collider col in cols)
            {
                EnemyBehaviour behaviour = col.GetComponent<EnemyBehaviour>();
                if (behaviour == null || behaviour.IsDead)
                    continue;
                
                behaviour.Damage(Damage);
            }
        }
    }
}
