using UnityEngine;

namespace Project.Buildings
{
    public class ExpansionConfig : BuildingConfig
    {
        [SerializeField] private int _AddedCells;
        public int AddedCells => _AddedCells;
    }
}
