using System.ComponentModel.DataAnnotations;

namespace Files.Models;

public class File
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string BinaryContent { get; set; }
}

public class FileDTO
{
    [Required, MinLength(1, ErrorMessage = "File name is required")]
    public string Name { get; set; }

    [Required, MinLength(1, ErrorMessage = "File content is required")]
    public string BinaryContent { get; set; }
}