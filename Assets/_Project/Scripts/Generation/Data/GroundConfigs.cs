using JvLib.Data;
using UnityEngine;

namespace Project.Generation
{
    [CreateAssetMenu(
        menuName = "Project/Generation/Grounds",
        fileName = nameof(GroundConfigs),
        order = 170)]
    public class GroundConfigs : DataList<GroundConfig>
    {
        public static GroundConfig GetConfig(EWorldCellContent pContent)
        {
            foreach (GroundConfig config in Entries)
            {
                if (config.ContentType == pContent)
                    return config;
            }

            return null;
        }
    }
}
