using System;
using JvLib.Events;
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

        private int _wave;
        public int Wave => _wave;
        
        private SafeEvent _onTimerTick = new();
        public event Action OnTimerTick
        {
            add => _onTimerTick += value;
            remove => _onTimerTick -= value;
        }

        private float _health;
        public float Health => _health;
        [SerializeField] private float _MaxHealth;
        public float MaxHealth => _MaxHealth;

        public float GetScaledHealth() => _health / _MaxHealth;
        
        private SafeEvent _onDamageTaken = new();
        public event Action OnDamageTaken
        {
            add => _onDamageTaken += value;
            remove => _onDamageTaken -= value;
        }

        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
            IsServiceReady = true;
            _timer = _TimerDuration;
            _health = _MaxHealth;
            InitBuildSettings();
            InitResources();
        }

        private void Update()
        {
            BuildMouseOver();

            _timer = Mathf.Clamp(_timer - Time.deltaTime, 0, _TimerDuration);
            if (!(_timer <= 0)) return;
            
            AddResources();
            SpawnWave();
                
            _timer = _TimerDuration;
            _onTimerTick.Dispatch();
        }

        public void Damage(float pDamage)
        {
            _health -= pDamage;
            _onDamageTaken.Dispatch();
        }
    }
}
