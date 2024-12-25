namespace Files.Models;

public class File
{
    public long Id { get; set; }
    public string Name { get; set; }
    public byte[] BinaryContent { get; set; }
}

public class FileDTO
{
    public string Name { get; set; }
    public byte[] BinaryContent { get; set; }
}