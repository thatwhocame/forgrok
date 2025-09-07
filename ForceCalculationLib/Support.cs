using GeometryLib;

namespace ForceCalculationLib
{
    public class Support
    {
        public readonly Vector2F Position;

        public float ReactionForce;



        public Support(Vector2F position)
        {
            Position = position;
        }
    }
}
