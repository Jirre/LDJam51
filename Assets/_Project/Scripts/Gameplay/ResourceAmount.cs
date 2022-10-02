using System;
using UnityEngine;

namespace Project.Gameplay
{
    [Serializable]
    public struct ResourceAmount
    {
        [SerializeField] private EResources _Resource;
        public EResources Resource => _Resource;
        [SerializeField] private int _Amount;
        public int Amount => _Amount;

        public ResourceAmount(EResources pResources, int pAmount)
        {
            _Resource = pResources;
            _Amount = pAmount;
        }
    }
}
