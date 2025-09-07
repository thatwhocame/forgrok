namespace ForceCalculationLib.Solvers
{
    public class ForceOld : ArmOld
    {
        public float Value;



        public ForceOld()
        {
            Position = 0;
            Value = 0;
        }

        public ForceOld(float position, float value)
        {
            Position = position;
            Value = value;
        }
    }
}
