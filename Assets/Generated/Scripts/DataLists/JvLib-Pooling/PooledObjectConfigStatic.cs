using JvLib.Data;

namespace JvLib.Pooling.Data.Objects
{
    public partial class PooledObjectConfig
    {
        private static JvLib.Pooling.Data.Objects.PooledObjectConfigs values;
        private static JvLib.Pooling.Data.Objects.PooledObjectConfig cannonBall;
        private static JvLib.Pooling.Data.Objects.PooledObjectConfig arrow;
        private static JvLib.Pooling.Data.Objects.PooledObjectConfig catapultStone;
        private static JvLib.Pooling.Data.Objects.PooledObjectConfig magic;

        public static JvLib.Pooling.Data.Objects.PooledObjectConfigs Values
        {
            get
            {
                if (values == null)
                    values = (JvLib.Pooling.Data.Objects.PooledObjectConfigs)DataRegistry.GetList("66f03615de1d04247ab4211d1b493f79");
                return values;
            }
        }

        public static JvLib.Pooling.Data.Objects.PooledObjectConfig CannonBall
        {
            get
            {
                if (cannonBall == null && Values != null)
                    cannonBall = (JvLib.Pooling.Data.Objects.PooledObjectConfig)Values.GetEntry("e5ed327db6fe2f64ca64e3dbe5c8f95c");
                return cannonBall;
            }
        }

        public static JvLib.Pooling.Data.Objects.PooledObjectConfig Arrow
        {
            get
            {
                if (arrow == null && Values != null)
                    arrow = (JvLib.Pooling.Data.Objects.PooledObjectConfig)Values.GetEntry("aa43413d84c17d2499ed61d4cbc42e54");
                return arrow;
            }
        }

        public static JvLib.Pooling.Data.Objects.PooledObjectConfig CatapultStone
        {
            get
            {
                if (catapultStone == null && Values != null)
                    catapultStone = (JvLib.Pooling.Data.Objects.PooledObjectConfig)Values.GetEntry("13fd97863b793c04c810f56b272c4c03");
                return catapultStone;
            }
        }

        public static JvLib.Pooling.Data.Objects.PooledObjectConfig Magic
        {
            get
            {
                if (magic == null && Values != null)
                    magic = (JvLib.Pooling.Data.Objects.PooledObjectConfig)Values.GetEntry("ad8ced5304de2ff4989932eebbb56184");
                return magic;
            }
        }

    }
}

