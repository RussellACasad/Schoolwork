using System.Text.Json;

namespace NavigationAtt2
{
    public class SaveLoad
    {
        public static string dir = @"C:\C#\Casad\";

        public static void Save(Hero hero)
        {
            string filePath = dir + hero.FileName;
            using FileStream stream = new(filePath, FileMode.Create);
            JsonSerializer.Serialize(stream, hero);
            stream.Dispose();

        }

        public static Hero Load(string fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    string jsonString = File.ReadAllText(fileName);
                    Hero hero = JsonSerializer.Deserialize<Hero>(jsonString) ?? new Hero();
                    return hero;
                }
                catch
                {
                    return new() { Name = "null"};
                }
            }
            else
            {
                return new() { Name = "null" };
            }
        }


    }

}
