using System.Collections.Generic;
using JvLib.Services;
using UnityEngine;

namespace Project.Gameplay
{
    [ServiceInterface(Name = "Gameplay")]
    public partial class GameplayServiceManager : MonoBehaviour, IService
    {
        public bool IsServiceReady { get; private set; }

        [SerializeField] private GameConfig _Config;
        
        private float _timer;
        [SerializeField] private float _TimerDuration = 10f;
        public float GetScaledTime() => _timer / _TimerDuration;

        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
            IsServiceReady = true;
            _timer = _TimerDuration;
            InitBuildSettings();
            InitResources();
        }

        private void Update()
        {
            BuildMouseOver();

            _timer = Mathf.Clamp(_timer - Time.deltaTime, 0, _TimerDuration);
            if (_timer <= 0)
            {
                AddResources();
                _timer = _TimerDuration;
            }
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                List<List<Vector2Int>> paths = Svc.World.Paths;
            }
        }
    }
}
