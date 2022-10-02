using System.Collections.Generic;
using JvLib.Services;
using UnityEngine;

namespace Project.Gameplay
{
    [ServiceInterface]
    public partial class GameplayServiceManager : MonoBehaviour, IService
    {
        public bool IsServiceReady { get; private set; }

        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
            IsServiceReady = true;
            InitBuildSettings();
        }

        public void StartGame()
        {
            
        }

        public void SpawnWave()
        {
            
        }

        private void Update()
        {
            BuildMouseOver();
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                List<List<Vector2Int>> paths = Svc.World.Paths;
            }
        }
    }
}
