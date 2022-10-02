using System;
using System.Collections.Generic;
using JvLib.Events;
using UnityEngine;

namespace Project.Gameplay
{
    public partial class GameplayServiceManager
    {
        private int[] _resources;
        private int[] _income;
        
        private const int MAX_AMOUNT = 99999;
        
        private SafeEvent _onResourceUpdate = new();
        public event Action OnResourceUpdate
        {
            add => _onResourceUpdate += value;
            remove => _onResourceUpdate -= value;
        }

        private void InitResources()
        {
            _resources = new int [Enum.GetValues(typeof(EResources)).Length];
            foreach (KeyValuePair<EResources, int> kv in _Config.StartingResources)
            {
                _resources[(int) kv.Key] = kv.Value;
            }
            _onResourceUpdate.Dispatch();
        }

        public bool TrySpendResource(ResourceAmount[] pAmounts)
        {
            _resources ??= new int [Enum.GetValues(typeof(EResources)).Length];
            
            int[] totals = new int [Enum.GetValues(typeof(EResources)).Length];
            foreach (ResourceAmount a in pAmounts)
            {
                totals[(int)a.Resource] += a.Amount;
            }

            for (int i = 0; i < Enum.GetValues(typeof(EResources)).Length; i++)
            {
                if (totals[i] > _resources[i])
                    return false;
            }
            
            for (int i = 0; i < Enum.GetValues(typeof(EResources)).Length; i++)
            {
                _resources[i] -= totals[i];
            }
            
            _onResourceUpdate.Dispatch();
            return true;
        }
        
        public int GetResource(EResources pResources)
        {
            _resources ??= new int [Enum.GetValues(typeof(EResources)).Length];
            return _resources[(int)pResources];
        }

        public void AddIncome(EResources pResource, int pAmount)
        {
            _income ??= new int [Enum.GetValues(typeof(EResources)).Length];
            _income[(int) pResource] += pAmount;
        }

        private void AddResources()
        {
            _income ??= new int [Enum.GetValues(typeof(EResources)).Length];
            for (int i = 0; i < _resources.Length; i++)
            {
                _resources[i] = Mathf.Clamp(_resources[i] + _income[i], 0, MAX_AMOUNT);
            }
            _onResourceUpdate.Dispatch();
        }
    }
}
