using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;
using Microsoft.SolverFoundation.Solvers;

namespace ForceCalculationLib.Solvers
{
    public class SimplexSolver
    {
        public const double E = 0.05;

        public LinearResult Solve(Platform platform)
        {
            var cases = SolverHelper.CreateSolvingCases(platform.Supports);

            Microsoft.SolverFoundation.Solvers.SimplexSolver solver = new();

            // Добавляем переменные сил роботов и закрепляем их положительность
            int[] robotIndices = new int[platform.Robots.Length];
            for (int i = 0; i < platform.Robots.Length; i++)
            {
                solver.AddVariable($"robot_force_{i}", out robotIndices[i]);
                solver.SetBounds(robotIndices[i], E, Rational.PositiveInfinity);
                // Новое: верхнее ограничение
                solver.SetUpperBound(robotIndices[i], (double)platform.MaxForcePerRobot);
            }

            float gravity = platform.GravityForce;

            // Добавляем уравнения для каждого случая
            for (int i = 0; i < cases.Count; i++)
            {
                (Support firstSupport, Support secondSupport) = cases[i];

                Diagram diagram = Diagram.FromPlatform(platform, firstSupport, secondSupport);

                float positionA = diagram.Supports[0].Position;
                float positionB = diagram.Supports[1].Position;

                solver.AddVariable($"A_{i}", out int A);
                solver.AddVariable($"B_{i}", out int B);
                solver.SetBounds(A, E, Rational.PositiveInfinity);
                solver.SetBounds(B, E, Rational.PositiveInfinity);

                // Уравнение суммы сил
                solver.AddRow($"force_sum_{i}", out int forceSum);
                foreach (int robotIndex in robotIndices)
                    solver.SetCoefficient(forceSum, robotIndex, 1);
                solver.SetBounds(forceSum, -gravity, -gravity + E);

                // Уравнение суммы моментов относительно А
                solver.AddRow($"moment_sum_A_{i}", out int momentSumA);
                solver.SetCoefficient(momentSumA, B, positionB - positionA);
                for (int j = 0; j < robotIndices.Length; j++)
                {
                    int robotIndex = robotIndices[j];
                    float robotPosition = diagram.RobotForces[j].Position - positionA;
                    solver.SetCoefficient(momentSumA, robotIndex, robotPosition);
                }
                float gravityArmA = diagram.GravityForce.Position - positionA;
                solver.SetBounds(momentSumA, -gravity * gravityArmA, -gravity * gravityArmA + E);

                // Уравнение суммы моментов относительно B
                solver.AddRow($"moment_sum_B_{i}", out int momentSumB);
                solver.SetCoefficient(momentSumB, A, positionA - positionB);
                for (int j = 0; j < robotIndices.Length; j++)
                {
                    int robotIndex = robotIndices[j];
                    float robotPosition = diagram.RobotForces[j].Position - positionB;
                    solver.SetCoefficient(momentSumB, robotIndex, robotPosition);
                }
                float gravityArmB = diagram.GravityForce.Position - positionB;
                solver.SetBounds(momentSumB, -gravity * gravityArmB, -gravity * gravityArmB + E);
            }

            // Устанавливаем стремление суммы сил роботов к максимуму
            solver.AddRow("robot_force_sum", out int robotForceSumIndex);
            foreach (int i in robotIndices)
                solver.SetCoefficient(robotForceSumIndex, i, 1);
            solver.AddGoal(robotForceSumIndex, 1, false);

            ILinearSolution result = solver.Solve(new SimplexSolverParams(new SimplexDirective()
            { TimeLimit = 1000, IterationLimit = 5000 }));

            for (int i = 0; i < robotIndices.Length; i++)
            {
                float force = (float)result.GetValue(robotIndices[i]).ToDouble();
                force = float.IsRealNumber(force) && float.IsFinite(force) ? force : 0;
                platform.Robots[i].PushingForce = force;
            }

            return result.Result;
        }
    }
}