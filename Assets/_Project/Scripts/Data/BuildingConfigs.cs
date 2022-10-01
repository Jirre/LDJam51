using JvLib.Data;
using UnityEngine;

namespace Project.Buildings
{
    [CreateAssetMenu(
        menuName = "Project/Buildings",
        fileName = nameof(BuildingConfigs),
        order = 170)]
    public class BuildingConfigs : DataList<BuildingConfig>
    {
    }
}