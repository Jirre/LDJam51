using JvLib.Data;
using UnityEngine;

namespace Project.Generation
{
    [CreateAssetMenu(
        menuName = "Project/Generation/Configs",
        fileName = nameof(WorldGeneratorConfigs),
        order = 170)]
    public class WorldGeneratorConfigs : DataList<WorldGeneratorConfig>
    {
    }
}
