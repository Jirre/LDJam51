using System;
using DG.Tweening;
using JvLib.Services;
using Project.Gameplay;
using UnityEngine;

namespace Project.Audio
{
    [ServiceInterface(Name = "Audio")]
    public class AudioServiceManager : MonoBehaviour, IService
    {
        public bool IsServiceReady { get; private set; }

        [SerializeField] private AudioSource _ClickSource;
        [SerializeField] private AudioSource _PlaceSource;

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
    }
}
