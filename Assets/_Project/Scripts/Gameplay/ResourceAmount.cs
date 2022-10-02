namespace Project.Gameplay
{
    public struct ResourceAmount
    {
        public EResources Resource { get; }
        public int Amount { get; }

        public ResourceAmount(EResources pResources, int pAmount)
        {
            Resource = pResources;
            Amount = pAmount;
        }
    }
}
