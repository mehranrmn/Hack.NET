using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;


namespace Files.Models
{
    public class File
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string B64ByteContent { get; set; }
        public Metadata Metadata { get; set; }
    }

    public class FileDTO
    {
        [Required, MinLength(1, ErrorMessage = "File name is required")]
        public string Name { get; set; }

        [Required, MinLength(1, ErrorMessage = "File content is required")]
        public string B64ByteContent { get; set; }
    }

    public class Metadata
    {
        public long Id { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public long FileId { get; set; }

        [JsonIgnore]
        public File File { get; set; }
    }

    [Serializable]
    public class MetadataDTO
    {
        public string Author { get; set; }
        public string Description { get; set; }
    }
}