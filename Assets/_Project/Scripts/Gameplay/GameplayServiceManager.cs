using JvLib.Services;

namespace Project.Gameplay
{
    [ServiceInterface]
    public class GameplayServiceManager : IService
    {
        public bool IsServiceReady { get; private set; }
        
        
    }
}
