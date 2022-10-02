using System;
using JvLib.Data;
using Project.Gameplay;
using Project.Generation;
using UnityEngine;

namespace Project.Buildings
{
    public class GathererConfig : BuildingConfig
    {
        [SerializeField] private SerializableDictionary<EResources, int> _GatheringSpeed;
        public int GetGatheringSpeed(EResources pResource)
        {
            return _GatheringSpeed.TryGetValue(pResource, out int value) ? value : 0;
        }
    }
}
