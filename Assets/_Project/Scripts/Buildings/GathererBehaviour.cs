using System;
using JvLib.Services;
using Project.Gameplay;
using Project.Generation;

namespace Project.Buildings
{
    public class GathererBehaviour : BuildingBehaviour
    {
        private EResources _resources;
        
        public override void OnBuild(WorldCell pCell)
        {
            _resources = pCell.Content switch
            {
                EWorldCellContent.Trees => EResources.Wood,
                EWorldCellContent.Stones => EResources.Stone,
                EWorldCellContent.Crystals => EResources.Crystal,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            if (_Config is GathererConfig gConfig)
            {
                int speed = gConfig.GetGatheringSpeed(_resources);
                Svc.Gameplay.AddIncome(_resources, speed);
            }
            base.OnBuild(pCell);
        }

        public override void OnDemolish()
        {
            if (_Config is GathererConfig gConfig)
            {
                int speed = gConfig.GetGatheringSpeed(_resources);
                Svc.Gameplay.AddIncome(_resources, -speed);
            }
            base.OnDemolish();
        }
    }
}
