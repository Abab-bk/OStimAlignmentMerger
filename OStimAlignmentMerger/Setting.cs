namespace OStimAlignmentMerger;

public class Setting(string outputFile, string sourceFolder)
{
    public string OutputFile { get; set; } = outputFile;
    public string SourceFolder { get; set; } = sourceFolder;
}