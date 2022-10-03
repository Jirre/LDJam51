using System;
using DG.Tweening;
using JvLib.Services;
using Project.Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Audio
{
    [ServiceInterface(Name = "Audio")]
    public class AudioServiceManager : MonoBehaviour, IService
    {
        public bool IsServiceReady { get; private set; }

        [SerializeField] private AudioSource _ClickSource;
        [SerializeField] private AudioSource _PlaceSource;
        [SerializeField] private AudioSource _VoiceSource;

        [SerializeField] private AudioClip[] _DamageClips;
        [SerializeField] private AudioClip[] _ResourceClips;
        
        [SerializeField] private float _DamageTimeout;
        private float _damageTimer;

        [Space] 
        [SerializeField] private AudioSource _MusicSource;
        [SerializeField] private AudioClip _MenuMusic;
        [SerializeField] private AudioClip _GameMusic;

        [SerializeField] private float _SfxVolume;
        public float SfxVolume => _SfxVolume;
        [SerializeField] private float _MusicVolume;
        public float MusicVolume => _MusicVolume;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
            IsServiceReady = true;
        }

        private void Start()
        {
            FadeMusic(_MenuMusic);
            SetSfxVolume(_SfxVolume);
            Svc.Gameplay.OnGameStateChange += OnStateChange;
        }

        private void OnDestroy()
        {
            Svc.Gameplay.OnGameStateChange -= OnStateChange;
        }

        private void Update()
        {
            _damageTimer -= Time.deltaTime;
        }

        private void OnStateChange(EGameStates pState)
        {
            switch (pState)
            {
                case EGameStates.Init or EGameStates.GameOver:
                    _MusicSource.DOFade(0, 0.5f).onComplete += () => FadeMusic(_MenuMusic);
                    break;
                case EGameStates.InitGame:
                    _MusicSource.DOFade(0, 0.5f).onComplete += () => FadeMusic(_GameMusic);
                    break;
            }
        }

        public void SetMusicVolume(float pVolume)
        {
            _MusicVolume = pVolume;
            _MusicSource.volume = pVolume;
        }

        public void SetSfxVolume(float pVolume)
        {
            _SfxVolume = pVolume;
            _ClickSource.volume = pVolume;
            _PlaceSource.volume = pVolume;
        }

        private void FadeMusic(AudioClip pClip)
        {
            _MusicSource.DOFade(0, 0.5f).onComplete += () =>
            {
                _MusicSource.Stop();
                _MusicSource.clip = pClip;
                _MusicSource.Play();
                _MusicSource.DOFade(_MusicVolume, 0.5f);
            };
        }

        public void Click()
        {
            _ClickSource.Play();
        }
        public void Place()
        {
            _PlaceSource.Play();
        }

        public void Damage()
        {
            if (_damageTimer > 0 || _VoiceSource.isPlaying) return;
            
            _VoiceSource.clip = _DamageClips[Random.Range(0, _DamageClips.Length)];
            _VoiceSource.Play();
            _damageTimer = _DamageTimeout;
        }

        public void Resources()
        {
            if (_VoiceSource.isPlaying) return;
            
            _VoiceSource.clip = _ResourceClips[Random.Range(0, _ResourceClips.Length)];
            _VoiceSource.Play();
        }
    }
}
