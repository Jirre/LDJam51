using JvLib.Data;

namespace Project.Gameplay
{
    public partial class GameConfig
    {
        private static Project.Gameplay.GameConfigs values;
        private static Project.Gameplay.GameConfig _default;

        public static Project.Gameplay.GameConfigs Values
        {
            get
            {
                if (values == null)
                    values = (Project.Gameplay.GameConfigs)DataRegistry.GetList("5f6fccbdf0c728048a6c9fc3b6da75e4");
                return values;
            }
        }

        public static Project.Gameplay.GameConfig Default
        {
            get
            {
                if (_default == null && Values != null)
                    _default = (Project.Gameplay.GameConfig)Values.GetEntry("085b44d07eab5dc4bbace268569c7739");
                return _default;
            }
        }

    }
}

