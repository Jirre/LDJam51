using System.Collections;
using System.Collections.Generic;
using JvLib.Data;
using JvLib.Routines;
using Project.Enemies;
using UnityEngine;

namespace Project.Gameplay
{
    public partial class GameplayServiceManager //Enemies
    {
        [SerializeField]
        private Dictionary<Vector2Int, Vector2Int[]> _paths;

        private const float SPAWN_HEIGHT = 0.2f;

        private List<Routine> _spawnRoutines;

        public void AddPath(Vector2Int[] pPath)
        {
            _paths ??= new Dictionary<Vector2Int, Vector2Int[]>();
            _paths.Add(pPath[0], pPath);
        }
        
        private void SpawnWave()
        {
            _spawnRoutines ??= new List<Routine>();
            foreach (Routine r in _spawnRoutines)
            {
                if (r.IsRunning())
                    r.Stop();
            }
            _spawnRoutines.Clear();

            foreach (KeyValuePair<Vector2Int, Vector2Int[]> kv in _paths)
            {
                Routine r = new(Spawn(kv.Key, kv.Value));
                _spawnRoutines.Add(r);
                r.Start();
            }
        }

        private IEnumerator Spawn(Vector2Int pStart, Vector2Int[] pPath)
        {
            EnemyConfig eConfig = _Config.GetWave(_wave, out int count);
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(eConfig.Prefab.gameObject);
                obj.transform.position = new Vector3(pStart.x, SPAWN_HEIGHT, pStart.y);
                EnemyBehaviour behaviour = obj.GetComponent<EnemyBehaviour>();
                behaviour.SetConfig(eConfig, pPath);

                yield return new WaitForSeconds(1.5f / eConfig.Speed);
            }
        }
    }
}
