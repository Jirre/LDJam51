using JvLib.Services;
using Project.Gameplay;
using UnityEngine;

namespace Project.UI
{
    [ServiceInterface(Name = "UI")]
    public class UIServiceManager : MonoBehaviour, IService
    {
        public bool IsServiceReady { get; private set; }

        [SerializeField] private Animator _MenuAnimator;
        [SerializeField] private Animator _GameplayAnimator;
        [SerializeField] private Animator _PauseAnimator;
        [SerializeField] private Animator _GameOverAnimator;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
            IsServiceReady = true;
        }

        private void Start()
        {
            Svc.Gameplay.OnGameStateChange += OnStateChange;
        }

        private void OnDestroy()
        {
            Svc.Gameplay.OnGameStateChange -= OnStateChange;
        }

        private void OnStateChange(EGameStates pState)
        {
            _MenuAnimator.SetBool("Shown", pState is EGameStates.Init or EGameStates.Menu);
            _GameplayAnimator.SetBool("Shown", pState is EGameStates.Gameplay or EGameStates.Pause);
            _PauseAnimator.SetBool("Shown", pState == EGameStates.Pause);
            _GameOverAnimator.SetBool("Shown", pState == EGameStates.GameOver);
        }
    }
}

