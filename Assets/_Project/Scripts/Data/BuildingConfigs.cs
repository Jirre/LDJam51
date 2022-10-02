using JvLib.Data;
using UnityEngine;

namespace Project.Buildings
{
    [CreateAssetMenu(
        menuName = "Project/Buildings",
        fileName = nameof(BuildingConfigs),
        order = 171)]
    public class BuildingConfigs : DataList<BuildingConfig>
    {
    }
}