using System.Collections.Generic;
using DG.Tweening;
using JvLib.Services;
using JvLib.Utilities;
using UnityEngine;

namespace Project.Enemies
{
    [RequireComponent(typeof(Animator))]
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private float _DefaultDirection;
        
        private EnemyConfig _config;
        private Vector2Int[] _path;

        private int _targetIndex;
        private Vector3 _current;
        private Vector3 _target;
        private float _lerp;

        private Animator _animator;

        private float _health;
        private bool _isDead;
        public bool IsDead => _isDead;

        private const float DEATH_DEPTH = -2f;
        private const float DEATH_DURATION = 3f;
        
        private const float WALK_HEIGHT = 0.2f;
        
        private void Awake()
        {
            _targetIndex = 0;
            _animator = GetComponent<Animator>();
            _isDead = false;
        }

        private void Update()
        {
            if (_isDead) return;
            _lerp = Mathf.Clamp01(_lerp + Time.deltaTime * _config.Speed);
            transform.position = Vector3.Lerp(_current, _target, _lerp);
            
            if (_targetIndex >= _path.Length - 1 && _lerp >= 0.5f)
            {
                Svc.Gameplay.Damage(_config.Damage);
                _isDead = true;
                Destroy(gameObject);
                return;
            }
            
            if (_lerp >= 1)
                GotoNextTarget();
        }

        public void SetConfig(EnemyConfig pConfig, Vector2Int[] pPath)
        {
            _config = pConfig;
            _path = pPath;

            _animator.SetFloat("Speed", pConfig.Speed);
            _health = pConfig.GetHealth(Svc.Gameplay.Wave);
            GotoNextTarget();
        }

        public void Damage(float pDamage)
        {
            if (_isDead)
                return;
            _health -= pDamage;
            
            if (_health > 0) return;
            
            _isDead = true;
            _animator.SetBool("IsDead", true);
            transform.DOMoveY(DEATH_DEPTH, DEATH_DURATION);
            Destroy(gameObject, DEATH_DURATION + 1f);
        }

        private void GotoNextTarget()
        {
            _current = new Vector3(_path[_targetIndex].x, WALK_HEIGHT, _path[_targetIndex].y);
            _targetIndex++;
            _target = new Vector3(_path[_targetIndex].x, WALK_HEIGHT, _path[_targetIndex].y);
            _lerp = 0;

            transform.eulerAngles =
                Vector3.up * (_DefaultDirection - MathUtility.DegPointDirection(_path[_targetIndex - 1], _path[_targetIndex]));
        }
    }
}
