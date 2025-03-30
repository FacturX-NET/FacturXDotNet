namespace Tests.FacturXDotNet.CLI;

class TestUtils
{
    public static string DuplicateFileAtRandomLocation(string path)
    {
        string? dir = Path.GetDirectoryName(path);
        string newPath = Path.Join(dir, Guid.CreateVersion7() + Path.GetExtension(path));
        File.Copy(path, newPath);
        return newPath;
    }
}
