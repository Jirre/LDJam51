using System.Collections.Generic;
using JvLib.Pooling.Objects;
using JvLib.Services;
using JvLib.Utilities;
using Project.Enemies;
using UnityEngine;

namespace Project.Buildings
{
    [RequireComponent(typeof(LineRenderer))]
    public class TurretBehaviour : BuildingBehaviour
    {
        [SerializeField] private LayerMask _TargetMask;
        
        private LineRenderer _lineRenderer;
        private const int LINE_SEGMENT_COUNT = 12;
        private const float LINE_HEIGHT = 0.25f;
        
        private float _timer;
        private ObjectPool _pool;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (!((_timer -= Time.deltaTime) <= 0)) 
                return;
            
            if (_Config is not TurretConfig tConfig)
                return;
                
            _timer = tConfig.Cooldown;
            Collider[] hits = Physics.OverlapSphere(transform.position, tConfig.Range, _TargetMask.value);

            Fire(hits);
        }

        private void Fire(IReadOnlyCollection<Collider> pHits)
        {
            if (_Config is not TurretConfig tConfig)
                return;
            
            if (pHits.Count <= 0)
                return;
            EnemyBehaviour target = null;
            foreach (Collider h in pHits)
            {
                EnemyBehaviour hit = h.GetComponent<EnemyBehaviour>();
                if (hit.IsDead) continue;
                
                if (target == null)
                {
                    target = h.GetComponent<EnemyBehaviour>();
                    continue;
                }
                if (target.TargetIndex < hit.TargetIndex)
                    target = hit;
            }
            if (target == null)
                return;

            Transform tTransform = target.transform;

            TurretTransform.eulerAngles = Vector3.up * MathUtility.DegPointDirection(
                new Vector2(TurretPosition.x, TurretPosition.z),
                new Vector2(tTransform.position.x, tTransform.position.z));
            
            GameObject obj = _pool.Activate(TurretPosition, TurretTransform.rotation);
            obj.GetComponent<ProjectileBehaviour>().Setup(TurretPosition, tTransform, tConfig.Damage);
        }

        public override void SetConfig(BuildingConfig pConfig)
        {
            base.SetConfig(pConfig);

            if (_Config is not TurretConfig tConfig)
                return;

            _lineRenderer.positionCount = LINE_SEGMENT_COUNT;
            _lineRenderer.SetPositions(GetPositions(tConfig));

            _timer = tConfig.Cooldown;
            _pool = Svc.ObjectPools.GetPool(tConfig.ProjectileID);
        }

        private Vector3[] GetPositions(TurretConfig pConfig)
        {
            Vector3[] results = new Vector3[LINE_SEGMENT_COUNT];
            
            for (int i = 0; i < LINE_SEGMENT_COUNT; i++)
            {
                float rad = Mathf.PI * 2f * ((float)i / LINE_SEGMENT_COUNT);
                results[i] = transform.position + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * pConfig.Range + Vector3.up * LINE_HEIGHT;
            }

            return results;
        }
    }
}
