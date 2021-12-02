namespace src.Util;

public static class FileReader
{
    private const string FilePath = @"C:\Dev\aoc\aoc2021\src\Input\";

    public static List<string> ReadFileToList(string fileName, bool isFullPath = true)
    {
        var filePath = isFullPath ? fileName : FilePath + fileName;
        
        return File.ReadAllLines(filePath).ToList();
    }
}

