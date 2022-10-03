using JvLib.Data;

namespace Project.Buildings
{
    public partial class BuildingConfig
    {
        private static Project.Buildings.BuildingConfigs values;
        private static Project.Buildings.BuildingConfig base_01;
        private static Project.Buildings.GathererConfig gatherer_01;
        private static Project.Buildings.TurretConfig ballista_01;
        private static Project.Buildings.TurretConfig catapult_01;
        private static Project.Buildings.TurretConfig cannon_01;
        private static Project.Buildings.TurretConfig mage_01;
        private static Project.Buildings.ExpansionConfig watchTower;

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

        public static Project.Buildings.GathererConfig Gatherer_01
        {
            get
            {
                if (gatherer_01 == null && Values != null)
                    gatherer_01 = (Project.Buildings.GathererConfig)Values.GetEntry("35f0dcf2e8c57a64f96f0685437f31b4");
                return gatherer_01;
            }
        }

        public static Project.Buildings.TurretConfig Ballista_01
        {
            get
            {
                if (ballista_01 == null && Values != null)
                    ballista_01 = (Project.Buildings.TurretConfig)Values.GetEntry("1455e4ff27de3f8498bb3cf4c2ed7236");
                return ballista_01;
            }
        }

        public static Project.Buildings.TurretConfig Catapult_01
        {
            get
            {
                if (catapult_01 == null && Values != null)
                    catapult_01 = (Project.Buildings.TurretConfig)Values.GetEntry("8ef6bae1518fbcb44916bf981981ebfa");
                return catapult_01;
            }
        }

        public static Project.Buildings.TurretConfig Cannon_01
        {
            get
            {
                if (cannon_01 == null && Values != null)
                    cannon_01 = (Project.Buildings.TurretConfig)Values.GetEntry("a59022fb8e4cf5145a8d8e6df3ab002d");
                return cannon_01;
            }
        }

        public static Project.Buildings.TurretConfig Mage_01
        {
            get
            {
                if (mage_01 == null && Values != null)
                    mage_01 = (Project.Buildings.TurretConfig)Values.GetEntry("6f41df46dde99f54da214d1b3e15a10f");
                return mage_01;
            }
        }

        public static Project.Buildings.ExpansionConfig WatchTower
        {
            get
            {
                if (watchTower == null && Values != null)
                    watchTower = (Project.Buildings.ExpansionConfig)Values.GetEntry("882688c83e2b44d41aab93bf73eca7b4");
                return watchTower;
            }
        }

    }
}

