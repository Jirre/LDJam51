using JvLib.Data;
using UnityEngine;

namespace Project.Enemies
{
    [CreateAssetMenu(
        menuName = "Project/Enemies",
        fileName = nameof(EnemyConfigs),
        order = 172)]
    public class EnemyConfigs : DataList<EnemyConfig>
    {
    }
}
