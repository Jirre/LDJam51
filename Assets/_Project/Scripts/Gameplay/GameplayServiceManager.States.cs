using System;
using System.Collections.Generic;
using JvLib.Events;
using JvLib.Routines;
using JvLib.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Gameplay
{
    public partial class GameplayServiceManager
    {
        private EventStateMachine<EGameStates> _stateMachine;
        public event Action<EGameStates> OnGameStateChange
        {
            add => _stateMachine.OnStateChanged += value;
            remove => _stateMachine.OnStateChanged -= value;
        }

        public bool IsCurrentGameState(EGameStates pState) => _stateMachine?.IsCurrentState(pState) ?? false;
        
        public void InitStateMachine()
        {
            _stateMachine = new EventStateMachine<EGameStates>(nameof(EGameStates));
            _stateMachine.Add(EGameStates.Init, InitState);
            _stateMachine.Add(EGameStates.Menu, MenuState);

            _stateMachine.Add(EGameStates.InitGame, InitGameState);
            _stateMachine.Add(EGameStates.Gameplay, GameplayState);
            _stateMachine.Add(EGameStates.Pause, PauseState);

            _stateMachine.Add(EGameStates.GameOver, GameOverState);

            _stateMachine.GotoState(EGameStates.Init);
        }

        private void InitState(EventState<EGameStates> pState)
        {
            pState.GotoState(EGameStates.Menu);
        }

        private void MenuState(EventState<EGameStates> pState)
        {
            if (!pState.IsFistFrame)
                return;
            
            Svc.World.Clear();
            Svc.World.SetConfig(_Config.WorldSettings[Random.Range(0, _Config.WorldSettings.Count)]);
            Svc.World.Generate(Vector2Int.zero, 0);
        }

        public void StartGame()
        {
            if (!_stateMachine.IsCurrentState(EGameStates.Menu))
                return;

            //_Config = pConfig; //TODO: If Multiple Configs are needed
            _stateMachine.GotoState(EGameStates.InitGame);
        }

        public void OpenVotePage()
        {
            Application.OpenURL(VOTE_URL);
        }
        
        public void ExitApplication()
        {
            Application.Quit();
        }

        public void RestartGame()
        {
            if (!_stateMachine.IsCurrentState(EGameStates.Pause) &&
                !_stateMachine.IsCurrentState(EGameStates.GameOver))
                return;
            
            _stateMachine.GotoState(EGameStates.InitGame);
        }

        private void InitGameState(EventState<EGameStates> pState)
        {
            if (!pState.IsFistFrame)
                return;

            _wave = 0;
            _timer = _TimerDuration;
            _health = _MaxHealth;
            _onDamageTaken.Dispatch();

            InitBuildSettings();

            _paths ??= new Dictionary<Vector2Int, Vector2Int[]>();
            _paths.Clear();

            Svc.World.Clear();
            
            InitResources();
            
            Svc.World.SetConfig(_Config.WorldSettings[Random.Range(0, _Config.WorldSettings.Count)]);
            Svc.World.Generate(Vector2Int.zero, 0);
            Svc.World.OnBuildFinish += WaitForBuild;
        }

        private void WaitForBuild()
        {
            Svc.World.OnBuildFinish -= WaitForBuild;
            _stateMachine.GotoState(EGameStates.Gameplay);
        }

        private SafeEvent _onTimerTick = new();
        public event Action OnTimerTick
        {
            add => _onTimerTick += value;
            remove => _onTimerTick -= value;
        }
        private void GameplayState(EventState<EGameStates> pState)
        {
            if (pState.IsFistFrame)
            {
                _spawnRoutines ??= new List<Routine>();
                foreach (Routine r in _spawnRoutines)
                {
                    r.Pause();
                }
            }

            BuildMouseOver();

            if (_health <= 0)
            {
                pState.GotoState(EGameStates.GameOver);
                return;
            }

            _timer = Mathf.Clamp(_timer - Time.deltaTime, 0, _TimerDuration);
            if (!(_timer <= 0)) return;
            
            AddResources();
            SpawnWave();

            _wave++;
            _timer = _TimerDuration;
            _onTimerTick.Dispatch();
        }

        public void PauseGame()
        {
            _stateMachine.GotoState(EGameStates.Pause);
        }

        public void UnpauseGame()
        {
            if (_stateMachine.IsCurrentState(EGameStates.Pause))
                _stateMachine.GotoState(EGameStates.Gameplay);
        }
        
        private void PauseState(EventState<EGameStates> pState)
        {
            if (!pState.IsFistFrame)
                return;
            
            _spawnRoutines ??= new List<Routine>();
            foreach (Routine r in _spawnRoutines)
            {
                r.Pause();
            }
        }
        
        public void StopGame()
        {
            if (!_stateMachine.IsCurrentState(EGameStates.Pause))
                return;
            
            _stateMachine.GotoState(EGameStates.GameOver);
        }
        
        public void GameOverState(EventState<EGameStates> pState)
        {
            if (!pState.IsFistFrame)
                return;
            
            _spawnRoutines ??= new List<Routine>();
            foreach (Routine r in _spawnRoutines)
            {
                r.Stop();
            }
        }

        public void GotoMenu()
        {
            if (!_stateMachine.IsCurrentState(EGameStates.GameOver))
                return;
            
            _stateMachine.GotoState(EGameStates.Menu);
        }
    }
}
