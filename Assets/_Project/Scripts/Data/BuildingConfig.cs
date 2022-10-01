using System.Collections;
using System.Collections.Generic;
using JvLib.Data;
using Project.Generation;
using UnityEngine;

namespace Project.Buildings
{
    public partial class BuildingConfig : DataEntry
    {
        [SerializeField] private string _Name;
        
        [SerializeField] private bool _CanBeDemolished;
        [SerializeField] private EWorldCellContent[] _AllowedCells;

        [SerializeField] private GameObject _Base;
        public GameObject Base => _Base;
        [SerializeField] private GameObject _Middle;
        public GameObject Middle => _Middle;
        [SerializeField] private GameObject _Top;
        public GameObject Top => _Top;
    }
}
