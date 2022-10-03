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

        private const string VOTE_URL = "https://ldjam.com/events/ludum-dare/51/rightful";
        
        [SerializeField] private GameConfig _Config;
        
        private float _timer;
        [SerializeField] private float _TimerDuration = 10f;
        public float GetScaledTime() => _timer / _TimerDuration;

        private int _wave;
        public int Wave => _wave;

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
            
            InitStateMachine();
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        public void Damage(float pDamage)
        {
            _health -= pDamage;
            if (pDamage > 0)
                Svc.Audio.Damage();
            _onDamageTaken.Dispatch();
        }
    }
}
