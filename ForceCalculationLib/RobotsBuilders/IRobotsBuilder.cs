namespace ForceCalculationLib.RobotsBuilders
{
    public interface IRobotsBuilder
    {
        public Robot[] Build(Supports supports, int count, int seed);
    }
}
