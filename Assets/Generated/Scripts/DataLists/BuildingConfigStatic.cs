using JvLib.Data;

namespace Project.Buildings
{
    public partial class BuildingConfig
    {
        private static Project.Buildings.BuildingConfigs values;
        private static Project.Buildings.BuildingConfig base_01;
        private static Project.Buildings.BuildingConfig harvester_01;
        private static Project.Buildings.BuildingConfig tower_00;

        public static Project.Buildings.BuildingConfigs Values
        {
            get
            {
                if (values == null)
                    values = (Project.Buildings.BuildingConfigs)DataRegistry.GetList("655a6e6b6d252ce47b060d32f18c3b18");
                return values;
            }
        }

        public static Project.Buildings.BuildingConfig Base_01
        {
            get
            {
                if (base_01 == null && Values != null)
                    base_01 = (Project.Buildings.BuildingConfig)Values.GetEntry("73d8a604981007c419be22468e2cb64f");
                return base_01;
            }
        }

        public static Project.Buildings.BuildingConfig Harvester_01
        {
            get
            {
                if (harvester_01 == null && Values != null)
                    harvester_01 = (Project.Buildings.BuildingConfig)Values.GetEntry("6942056ac39b281428e31304c8f392f5");
                return harvester_01;
            }
        }

        public static Project.Buildings.BuildingConfig Tower_00
        {
            get
            {
                if (tower_00 == null && Values != null)
                    tower_00 = (Project.Buildings.BuildingConfig)Values.GetEntry("64afad7ada6e00c4abd7428f88ed53ba");
                return tower_00;
            }
        }

    }
}

