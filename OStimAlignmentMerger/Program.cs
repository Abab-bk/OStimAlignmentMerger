using Newtonsoft.Json;

namespace OStimAlignmentMerger;

public static class Program
{
    public static async Task Main()
    {
        if (!File.Exists("settings.json") ||
            JsonConvert.DeserializeObject<Setting>(File.ReadAllText("settings.json")) == null
            )
        {
            var setting = SetupGuide();
            while (setting == null)
            {
                setting = SetupGuide();
            }
        }

        var mergerSetting = JsonConvert.DeserializeObject<Setting>(File.ReadAllText("settings.json"));

        if (mergerSetting == null)
        {
            Console.WriteLine("ERROR: Failed to read settings.json.");
            return;
        }

        var mergerCommand = new MergerCommand(mergerSetting);
        await mergerCommand.ExecuteAsync();
        
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    private static Setting? SetupGuide()
    {
        var outputFilePath = string.Empty;
        while (!IsFilePath(outputFilePath))
        {
            Console.WriteLine("Input output file path, " +
                              "eg: \"C:\\Users\\USERNAME\\Documents\\My Games\\Skyrim Special Edition\\OStim\\alignment.json\"");
            outputFilePath = Console.ReadLine() ?? string.Empty;
        }

        var sourceFolderPath = string.Empty;
        while (!IsFolderPath(sourceFolderPath))
        {
            Console.WriteLine("Input source folder path, for mo2 user, is mods folder." +
                              "eg: D:\\Games\\The Elder Scrolls V\\Modding\\mods");
            sourceFolderPath = Console.ReadLine() ?? string.Empty;
        }

        var setting = new Setting(outputFilePath, sourceFolderPath);
        File.WriteAllText("settings.json", JsonConvert.SerializeObject(setting));

        return JsonConvert.DeserializeObject<Setting>(File.ReadAllText("settings.json"));
    }

    private static bool IsFilePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return false;
        var invalidChars = Path.GetInvalidPathChars();
        if (!path.EndsWith(".json")) return false;
        return path.IndexOfAny(invalidChars) == -1;
    }
    
    private static bool IsFolderPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return false;
        if (path.IndexOfAny(Path.GetInvalidPathChars()) != -1) return false;
        try
        {
            var _ = Path.GetFullPath(path);
            return true;
        }
        catch
        {
            return false;
        }
    }
}