using System.Text.Json;

public static class StorageService
{
    public static void Save<T>(List<T> extend, string fileName)
    {
        string json = JsonSerializer.Serialize(extend, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(fileName, json);
    }

    public static List<T> Load<T>(string fileName)
    {
        if (!File.Exists(fileName))
            return new List<T>();

        string json = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize<List<T>>(json)
               ?? new List<T>();
    }
}