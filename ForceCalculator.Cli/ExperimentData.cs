namespace ForceCalculator.Cli;
public class ExperimentData
{
    public required PlatformType SupportsGenerationType { get; set; }
    public required string SupportsGenerationTypeDescription { get; set; }
    public required int SupportsNumber { get; set; }
    
    public required RobotsType RobotsGenerationType { get; set; }
    public required string RobotsGenerationTypeDescription { get; set; }
    public required int RobotsNumber { get; set; }
    
    public int CaseNumber { get; set; }
    public required List<CaseData> Cases { get; set; }
}