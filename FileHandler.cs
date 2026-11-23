using System.Text.Json;

namespace lecture_management_system;

/*
 * This file handles saving lecture data to a JSON file and loading it again.
 * It keeps the three lecture types separated while the app reads or writes data.
 */

// Groups each lecture type and the per-type ID counters so they are saved together.
public class LectureData
{
    public List<Programming> ProgrammingLectures { get; set; } = new List<Programming>();
    public List<Database> DatabaseLectures { get; set; } = new List<Database>();
    public List<Networking> NetworkingLectures { get; set; } = new List<Networking>();

    // Each counter tracks the next code number for its type. Never decremented on delete.
    public int NextProgrammingId { get; set; } = 101;
    public int NextDatabaseId { get; set; } = 101;
    public int NextNetworkingId { get; set; } = 101;
}

// Reads and writes lectures to the local JSON data file.
public class FileHandler
{
    // Location of the lecture data file
    private readonly string filePath;

    // Sets the file location, with lectures.json as the normal default
    public FileHandler(string filePath = "lectures.json")
    {
        this.filePath = filePath;
    }

    // Serialises the full data object (lectures + ID counters) as formatted JSON.
    public bool Save(LectureData data)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not save data: {ex.Message}");
            return false;
        }
    }

    // Returns the full data object, or a fresh default if the file is missing or damaged.
    public LectureData Load()
    {
        if (!File.Exists(filePath))
            return new LectureData();

        try
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<LectureData>(json) ?? new LectureData();
        }
        catch (JsonException)
        {
            Console.WriteLine("The data file is damaged. Starting with an empty list.");
            return new LectureData();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not load data: {ex.Message}");
            return new LectureData();
        }
    }
}