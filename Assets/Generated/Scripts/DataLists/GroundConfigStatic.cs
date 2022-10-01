using JvLib.Data;

namespace Project.Generation
{
    public partial class GroundConfig
    {
        private static Project.Generation.GroundConfigs values;
        private static Project.Generation.GroundConfig empty;
        private static Project.Generation.GroundConfig road;
        private static Project.Generation.GroundConfig spawn;

        public static Project.Generation.GroundConfigs Values
        {
            get
            {
                if (values == null)
                    values = (Project.Generation.GroundConfigs)DataRegistry.GetList("38da0a704238e7e429a950ed3aff6d6e");
                return values;
            }
        }

        public static Project.Generation.GroundConfig Empty
        {
            get
            {
                if (empty == null && Values != null)
                    empty = (Project.Generation.GroundConfig)Values.GetEntry("70add872cca7cb44b86fda647993b269");
                return empty;
            }
        }

        public static Project.Generation.GroundConfig Road
        {
            get
            {
                if (road == null && Values != null)
                    road = (Project.Generation.GroundConfig)Values.GetEntry("b91dd1fa73292d944b060f34ef351f85");
                return road;
            }
        }

        public static Project.Generation.GroundConfig Spawn
        {
            get
            {
                if (spawn == null && Values != null)
                    spawn = (Project.Generation.GroundConfig)Values.GetEntry("d0aba3f58684cce45bee46d627a4ae32");
                return spawn;
            }
        }

    }
}

