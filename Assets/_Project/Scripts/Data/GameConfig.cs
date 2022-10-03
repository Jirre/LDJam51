using System;
using System.Collections.Generic;
using System.Linq;
using JvLib.Data;
using Project.Enemies;
using Project.Generation;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Gameplay
{
    public partial class GameConfig : DataEntry
    {
        [SerializeField] private List<WorldGeneratorConfig> _WorldSettings;
        public List<WorldGeneratorConfig> WorldSettings => _WorldSettings;
        
        [SerializeField] private SerializableDictionary<EResources, int> _StartingResources;
        public SerializableDictionary<EResources, int> StartingResources => _StartingResources;
        
        [SerializeField] private SerializableDictionary<EResources, int> _BaseIncome;
        public SerializableDictionary<EResources, int> BaseIncome => _BaseIncome;

        [SerializeField, TableList] private List<Wave> _Waves;

        public EnemyConfig GetWave(int pWave, out int pCount)
        {
            pCount = 1;

            List<Wave> possibilities = new();
            
            foreach (Wave wave in _Waves)
            {
                if (wave.MinWave > pWave && wave.MinWave > 0)
                    continue;

                if (wave.MaxWave < pWave && wave.MaxWave > 0)
                    continue;
                
                possibilities.Add(wave);
            }

            int index = Random.Range(0, possibilities.Count);
            pCount = possibilities[index].Count;
            return possibilities[index].Enemy;
        }

        [Serializable]
        private struct Wave
        {
            [SerializeField, VerticalGroup("Wave"), LabelText("Min"), TableColumnWidth(80, false)] private int _MinWave;
            public int MinWave => _MinWave;
            [SerializeField, VerticalGroup("Wave"), LabelText("Max"), TableColumnWidth(80, false)] private int _MaxWave;
            public int MaxWave => _MaxWave;
            
            [SerializeField, VerticalGroup("Enemies"), HideLabel] private EnemyConfig _Enemy;
            public EnemyConfig Enemy => _Enemy;
            [SerializeField, VerticalGroup("Enemies")] private int _Count;
            public int Count => _Count;
        }
    }
}
