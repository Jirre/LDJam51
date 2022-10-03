using System;
using JvLib.Pooling.Objects;
using JvLib.Pooling.Particles;
using JvLib.Services;
using Project.Enemies;
using UnityEngine;

namespace Project.Buildings
{
    public class ProjectileBehaviour : PooledObject
    {
        [SerializeField] private AnimationCurve _HeightCurve;
        [SerializeField] private float _Duration;
        private ParticlePool _particlePool;

        private Vector3 _startPosition;
        protected Transform TargetTransform;
        protected Vector3 TargetPos;
        protected float Damage;
        private float _lerp;

        private const float TARGET_OFFSET = 0.3f;

        public void Setup(Vector3 pStart, Transform pTarget, float pDamage)
        {
            _lerp = 0;
            _startPosition = pStart;
            TargetTransform = pTarget;
            TargetPos = pTarget.position;
            transform.LookAt(pTarget);
            Damage = pDamage;
        }

        private void Update()
        {
            _lerp = Mathf.Clamp01(_lerp + Time.deltaTime / _Duration);

            if (_lerp >= 1)
            {
                OnImpact();
                Deactivate();
                return;
            }
            
            if (TargetTransform != null)
                TargetPos = TargetTransform.position;
            
            float height = TARGET_OFFSET + (_startPosition.y - TargetPos.y) * _HeightCurve.Evaluate01(_lerp);
            
            transform.position = Vector3.Lerp(
                new Vector3(_startPosition.x, 0f, _startPosition.z), 
                new Vector3(TargetPos.x, 0f, TargetPos.z), _lerp) + Vector3.up * height;
        }

        protected virtual void OnImpact()
        {
            if (TargetTransform != null) TargetTransform.GetComponent<EnemyBehaviour>().Damage(Damage);
            
        }
    }
}
