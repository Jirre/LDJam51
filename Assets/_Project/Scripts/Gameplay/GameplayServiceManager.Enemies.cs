using System.Collections;
using System.Collections.Generic;
using JvLib.Data;
using Project.Enemies;
using UnityEngine;

namespace Project.Gameplay
{
    public partial class GameplayServiceManager //Enemies
    {
        [SerializeField]
        private SerializableDictionary<Vector2Int, Vector2Int[]> _paths;

        private const float SPAWN_HEIGHT = 0.2f;

        public void AddPath(Vector2Int[] pPath)
        {
            _paths ??= new SerializableDictionary<Vector2Int, Vector2Int[]>();
            _paths.Add(pPath[0], pPath);
        }
        
        private void SpawnWave()
        {
            StopAllCoroutines();
            foreach (KeyValuePair<Vector2Int, Vector2Int[]> kv in _paths)
            {
                StartCoroutine(Spawn(kv.Key, kv.Value));
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
