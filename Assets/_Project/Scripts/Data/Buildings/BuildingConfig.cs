using JvLib.Data;
using Project.Generation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Buildings
{
    public partial class BuildingConfig : DataEntry
    {
        [SerializeField] private string _Name;
        [SerializeField] private BuildingBehaviour _BuildingPrototype;
        public BuildingBehaviour Prototype => _BuildingPrototype;
        
        [SerializeField] private bool _CanBeDemolished;
        [SerializeField] private EWorldCellContent[] _AllowedCells;

        public bool IsCellAllowed(EWorldCellContent pContent)
        {
            foreach (EWorldCellContent c in _AllowedCells)
            {
                if (c == pContent)
                    return true;
            }
            return false;
        }

        [SerializeField, PreviewField] private GameObject _Base;
        public GameObject Base => _Base;
        [SerializeField, PreviewField] private GameObject _Middle;
        public GameObject Middle => _Middle;
        [SerializeField, PreviewField] private GameObject _Top;
        public GameObject Top => _Top;

        [Space] 
        [SerializeField, PreviewField] private GameObject _Weapon;
        public GameObject Weapon => _Weapon;
    }
}
