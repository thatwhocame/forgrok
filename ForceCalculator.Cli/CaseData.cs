namespace ForceCalculator.Cli;
[Serializable]
public class CaseData
{
    public required PlatformData Platform { get; set; }
    
    public required bool IsEqualSolvedValid { get; set; }
    public required float EqualRobotForceSum { get; set; }
    public required List<float> EqualRobotForces { get; set; }
    
    public required bool IsIndividualSolvedValid { get; set; }
    public required float IndividualRobotForceSum { get; set; }
    public required List<float> IndividualRobotForces { get; set; }
}