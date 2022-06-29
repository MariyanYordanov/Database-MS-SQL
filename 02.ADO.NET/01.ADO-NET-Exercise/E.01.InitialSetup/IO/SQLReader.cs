namespace E._01.InitialSetup.IO
{
    using System.IO;
    public class SQLReader : IReader
    {
        public SQLReader(string fileName)
        {
            this.FileName = fileName;
        }

        public string FileName { get; }

        public string ReadToEnd()
        {
            string currentDirectoryPath = Directory.GetCurrentDirectory();
            string fullPath = Path.Combine(currentDirectoryPath, $"../Queries/{this.FileName}");
            string query = File.ReadAllText(fullPath);
            return query;
        }
    }
}
