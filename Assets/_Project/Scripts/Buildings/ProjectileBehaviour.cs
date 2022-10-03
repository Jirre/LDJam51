using JvLib.Pooling.Objects;
using Project.Enemies;
using UnityEngine;

namespace Project.Buildings
{
    public class ProjectileBehaviour : PooledObject
    {
        [SerializeField] private AnimationCurve _HeightCurve;
        [SerializeField] private float _Duration;

        private Vector3 _startPosition;
        private Transform _targetTransform;
        private float _damage;
        private float _lerp;

        private const float TARGET_OFFSET = 0.3f;

        public void Setup(Vector3 pStart, Transform pTarget, float pDamage)
        {
            _lerp = 0;
            _startPosition = pStart;
            _targetTransform = pTarget;
            transform.LookAt(pTarget);
            _damage = pDamage;
        }

        private void Update()
        {
            if (_targetTransform == null)
            {
                Deactivate();
                return;
            }

            _lerp = Mathf.Clamp01(_lerp + Time.deltaTime / _Duration);

            if (_lerp >= 1)
            {
                _targetTransform.GetComponent<EnemyBehaviour>().Damage(_damage);
                Deactivate();
                return;
            }

            Vector3 tPos = _targetTransform.position;
            float height = TARGET_OFFSET + (_startPosition.y - tPos.y) * _HeightCurve.Evaluate01(_lerp);
            
            transform.position = Vector3.Lerp(
                new Vector3(_startPosition.x, 0f, _startPosition.z), 
                new Vector3(tPos.x, 0f, tPos.z), _lerp) + Vector3.up * height;
        }
    }
}
