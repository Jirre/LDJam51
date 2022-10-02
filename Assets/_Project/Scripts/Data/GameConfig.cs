using System.Collections.Generic;
using JvLib.Data;
using Project.Generation;
using UnityEngine;

namespace Project.Gameplay
{
    public partial class GameConfig : DataEntry
    {
        [SerializeField] private List<WorldGeneratorConfig> _WorldSettings;
        public List<WorldGeneratorConfig> WorldSettings => _WorldSettings;
        
        [SerializeField] private SerializableDictionary<EResources, int> _StartingResources;
        public SerializableDictionary<EResources, int> StartingResources => _StartingResources;
    }
}
