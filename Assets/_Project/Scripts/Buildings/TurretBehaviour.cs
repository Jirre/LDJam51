using System.Collections.Generic;
using JvLib.Pooling.Objects;
using JvLib.Services;
using JvLib.Utilities;
using Project.Enemies;
using Project.Gameplay;
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

        private bool _isPaused;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _isPaused = !Svc.Gameplay.IsCurrentGameState(EGameStates.Gameplay);
            Svc.Gameplay.OnGameStateChange += OnStateChange;
        }

        private void OnDestroy()
        {
            Svc.Gameplay.OnGameStateChange -= OnStateChange;
        }

        private void OnStateChange(EGameStates pState)
        {
            _isPaused = pState != EGameStates.Gameplay;
        }

        private void Update()
        {
            if (_isPaused)
                return;
            
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

            Quaternion rot = Quaternion.Euler(Vector3.up * -MathUtility.DegPointDirection(
                new Vector2(TurretPosition.x, TurretPosition.z),
                new Vector2(tTransform.position.x, tTransform.position.z)));
            
            if (TurretTransform != null) TurretTransform.rotation = rot;
            GameObject obj = _pool.Activate(TurretPosition, rot);
            
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

        public override void OnDemolish()
        {
            if (_lineRenderer != null) _lineRenderer.positionCount = 0;
            base.OnDemolish();
        }
    }
}
