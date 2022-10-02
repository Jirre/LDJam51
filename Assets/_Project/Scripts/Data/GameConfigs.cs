using JvLib.Data;
using UnityEngine;

namespace Project.Gameplay
{
    [CreateAssetMenu(
        menuName = "Project/Game Config",
        fileName = nameof(GameConfigs),
        order = 170)]
    public class GameConfigs : DataList<GameConfig>
    {
    }
}
