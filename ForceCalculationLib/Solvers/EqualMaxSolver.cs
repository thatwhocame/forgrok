namespace ForceCalculationLib.Solvers
{
    public class EqualMaxSolver : ISolver
    {
        public float Solve(Platform platform)
        {
            var cases = SolverHelper.CreateSolvingCases(platform.Supports);

            float minForce = float.MaxValue;
            foreach (var supportPair in cases)
            {
                float force = SolveCase(platform, supportPair.First, supportPair.Second);
                if (force < minForce)
                    minForce = force;
            }

            // Ограничение максимальной силы
            if (minForce > platform.MaxForcePerRobot)
            {
                minForce = platform.MaxForcePerRobot;
            }

            // Проверка валидности решения
            if (!IsValidSolution(minForce, platform))
            {
                return 0; // Невалидное решение
            }

            // Присваиваем силу всем роботам
            foreach (var robot in platform.Robots)
            {
                robot.PushingForce = minForce;
            }

            return minForce * platform.Robots.Length; // Суммарная сила
        }

        private float SolveCase(Platform platform, Support firstSupport, Support secondSupport)
        {
            Diagram diagram = Diagram.FromPlatform(platform, firstSupport, secondSupport);

            Arm[] robotArms = [.. diagram.RobotForces.Select(f => new Arm(f.Position))];

            float maxRobotForce1 = CalculateMaxRobotForce(diagram.Supports[0], diagram.GravityForce, robotArms);
            float maxRobotForce2 = CalculateMaxRobotForce(diagram.Supports[1], diagram.GravityForce, robotArms);

            return Math.Min(maxRobotForce1, maxRobotForce2);
        }

        private float CalculateMaxRobotForce(Arm support, Force gravity, Arm[] robots)
        {
            float shift = -support.Position;

            gravity.Position += shift;

            float cumulateRobotArm = 0;
            foreach (Arm robot in robots)
            {
                float unbiasedRobotPosition = robot.Position + shift;
                cumulateRobotArm += unbiasedRobotPosition;
            }

            return -(gravity.Value * gravity.Position) / cumulateRobotArm;
        }

        private float CalculateMaxRobotForce(ArmOld baseSupport, ArmOld targetSupport, ForceOld gravity, ArmOld[] robots, float minN)
        {
            float cumulateRobotArm = 0;
            foreach (ArmOld arm in robots)
            {
                float unbiasedRobotPosition = arm.Position - baseSupport.Position;
                cumulateRobotArm += unbiasedRobotPosition;
            }

            float unbiasedTargetPosition = targetSupport.Position - baseSupport.Position;
            float unbiasedGravityPosition = gravity.Position - baseSupport.Position;

            return (gravity.Value * unbiasedGravityPosition - minN * unbiasedTargetPosition) / cumulateRobotArm;
        }

        private bool IsValidSolution(float force, Platform platform)
        {
            var cases = SolverHelper.CreateSolvingCases(platform.Supports);
            foreach (var supportPair in cases)
            {
                Diagram diagram = Diagram.FromPlatform(platform, supportPair.First, supportPair.Second);
                Arm[] robotArms = [.. diagram.RobotForces.Select(f => new Arm(f.Position))];

                // Рассчитываем реакции для каждой опоры
                float reaction1 = CalculateReaction(diagram.Supports[0], force, diagram.GravityForce, robotArms);
                float reaction2 = CalculateReaction(diagram.Supports[1], force, diagram.GravityForce, robotArms);

                if (reaction1 < 0 || reaction2 < 0)
                    return false;
            }
            return true;
        }

        private float CalculateReaction(Arm support, float force, Force gravity, Arm[] robots)
        {
            // Простая модель: сумма сил = 0, реакции распределяются
            float totalForce = force * robots.Length + gravity.Value;
            int numSupports = 2; // Для пары опор
            return totalForce / numSupports; // Заглушка, адаптируйте под вашу логику
        }
    }
}