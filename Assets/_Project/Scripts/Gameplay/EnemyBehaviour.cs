using System.Collections.Generic;
using UnityEngine;

namespace Project.Enemies
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private EnemyConfig _config;
        private List<Vector2Int> _path;

        private int _targetIndex;
        private Vector2Int _current;
        private Vector2Int _target;
        private float _lerp;
        
        private void Awake()
        {
            _targetIndex = 0;
        }

        private void Update()
        {
            _lerp = Mathf.Clamp01(_lerp + Time.deltaTime * _config.Speed);
            transform.position = Vector2.Lerp(_current, _target, _lerp);
            if (_lerp >= 1)
                GotoNextTarget();
        }

        public void SetConfig(EnemyConfig pConfig, List<Vector2Int> pPath)
        {
            _config = pConfig;
            _path = pPath;
        }

        private void GotoNextTarget()
        {
            _current = _path[_targetIndex++];
            _target = _path[_targetIndex];
            _lerp = 0;
        }
    }
}
