using Newtonsoft.Json.Linq;

namespace OStimAlignmentMerger;

public class MergerCommand(Setting setting)
{
    public ValueTask ExecuteAsync()
    {
        Console.WriteLine("Running, please wait...");
        
        File.WriteAllText(setting.OutputFile, "{}");
        
        var files = GetFilePaths(setting.SourceFolder, "alignment.json");

        var targetFile = JObject.Parse(File.ReadAllText(setting.OutputFile));
        
        foreach (var file in files)
        {
            targetFile.Merge(JObject.Parse(File.ReadAllText(file)));
            Console.WriteLine($"Merging {file}");
        }
        
        File.WriteAllText(setting.OutputFile, targetFile.ToString());
        
        Console.WriteLine("Done.");
        
        return ValueTask.CompletedTask;
    }

    private static string[] GetFilePaths(string rootFolder, string fileName)
    {
        try
        {
            return Directory
                .GetFiles(rootFolder, fileName, SearchOption.AllDirectories)
                .Select(f => f.Replace('\\', '/'))
                .ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            return [];
        }
    }
}