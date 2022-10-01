namespace JvLib.Routines
{
    public interface IRoutineYield
    {
        bool Running { get; }
        bool Stopped { get; }

        void Stop();
    }
}
