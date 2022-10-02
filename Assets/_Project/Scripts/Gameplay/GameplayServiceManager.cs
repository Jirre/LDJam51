using System;
using System.Collections.Generic;
using JvLib.Services;
using Project.Buildings;
using UnityEngine;

namespace Project.Gameplay
{
    [ServiceInterface]
    public class GameplayServiceManager : MonoBehaviour, IService
    {
        public bool IsServiceReady { get; private set; }

        private void Awake()
        {
            IsServiceReady = true;
        }

        public void StartGame()
        {
            
        }

        public void SpawnWave()
        {
            
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                List<List<Vector2Int>> paths = Svc.World.Paths;
            }
        }
    }
}
