using JvLib.Services;
using Project.Generation;

namespace Project.Buildings
{
    public class ExpansionBehaviour : BuildingBehaviour
    {
        public override void OnBuild(WorldCell pCell)
        {
            if (_Config is ExpansionConfig eConfig)
            {
                Svc.World.Generate(pCell.Position, eConfig.AddedCells);
            }
            base.OnBuild(pCell);
        }
    }
}
