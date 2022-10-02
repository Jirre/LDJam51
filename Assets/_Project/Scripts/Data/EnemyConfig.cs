using JvLib.Data;
using UnityEngine;

namespace Project.Enemies
{
    public class EnemyConfig : DataEntry
    {
        [SerializeField] private GameObject _Prefab;
        public GameObject Prefab => _Prefab;
        [SerializeField] private float _Speed;
        public float Speed => _Speed;
    }
}
