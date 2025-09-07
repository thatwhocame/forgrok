using GeometryLib;

namespace ForceCalculationLib
{
    public class Platform
    {
        public readonly float GravityForce;
        public readonly Supports Supports;
        public readonly Robot[] Robots;
        public readonly float MaxForcePerRobot; // Добавлено

        public Platform(Vector2F[] supportPositions, Vector2F[] robotPositions)
            : this(supportPositions, robotPositions, 0, 50.0f) { } // Дефолт: maxForce = 50.0f

        public Platform(Vector2F[] supportPositions, Vector2F[] robotPositions, float gravityForce)
            : this(supportPositions, robotPositions, gravityForce, 50.0f) { }

        public Platform(Vector2F[] supportPositions, Vector2F[] robotPositions, float gravityForce, float maxForcePerRobot)
        {
            if (gravityForce > 0)
                throw new ArgumentException("Сила тяжести должна быть < 0"); // Оставляем как было
            if (maxForcePerRobot <= 0)
                throw new ArgumentException("Максимальная сила поджатия должна быть > 0");

            Supports = Supports.FromPositions(supportPositions);
            Robots = new Robot[robotPositions.Length];
            for (int i = 0; i < robotPositions.Length; i++)
                Robots[i] = new Robot(robotPositions[i]);
            GravityForce = gravityForce;
            MaxForcePerRobot = maxForcePerRobot;
        }

        public Platform(Supports supports, Robot[] robots, float gravity, float maxForcePerRobot)
        {
            if (gravity > 0)
                throw new ArgumentException("Сила тяжести должна быть < 0");
            if (maxForcePerRobot <= 0)
                throw new ArgumentException("Максимальная сила поджатия должна быть > 0");

            Supports = supports;
            Robots = robots;
            GravityForce = gravity;
            MaxForcePerRobot = maxForcePerRobot;
        }
    }
}