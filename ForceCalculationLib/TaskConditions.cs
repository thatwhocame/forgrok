using GeometryLib;

namespace ForceCalculationLib
{
    public class TaskConditions
    {
        public float GrivityForce;
        public Vector2F[] SupportPositions;
        public Vector2F[] RobotPositions;



        public TaskConditions()
        {
            GrivityForce = 0;
            SupportPositions = [];
			RobotPositions = [];
        }
    }
}
