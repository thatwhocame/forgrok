using GeometryLib;

namespace ForceCalculationLib
{
    public class Robot
    {
        public readonly Vector2F Position;

        public float PushingForce;



        public Robot(Vector2F position)
        {
            Position = position;
        }
    }
}
