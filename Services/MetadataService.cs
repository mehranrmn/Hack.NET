using System;
using Files.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using MaliciousMetadataWriter;


namespace Metadata.Services
{
    public class MetadataService {
        private readonly FileContext _context;
        private readonly IWebHostEnvironment _env;
        public MetadataService(FileContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<Files.Models.Metadata?> GetMetadataById(long id)
        {
            return await _context.MetadataEntries.FindAsync(id);
        }

        public async Task<List<Files.Models.Metadata>> GetAllMetadata()
        {
            return await _context.MetadataEntries.ToListAsync();
        }

        public string SerializeMetadata(Files.Models.MetadataDTO metadataDTO)
        {
            string jsonSerializedMetadata = JsonSerializer.Serialize(metadataDTO);

            string base64SerializedMetadata = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonSerializedMetadata));

            return base64SerializedMetadata;
        }

        public Files.Models.MetadataDTO DeserializeMetadata(string Content)
        {
            string jsonSerializedMetadata = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Content));

            Files.Models.MetadataDTO deserializedMetadata = JsonSerializer.Deserialize<Files.Models.MetadataDTO>(jsonSerializedMetadata);

            return deserializedMetadata;
        }

        public async Task<Files.Models.Metadata> SaveMetadata(Files.Models.File file, Files.Models.MetadataDTO deserializedMetadata)
        {
            var metadata = new Files.Models.Metadata {
                Author = deserializedMetadata.Author,
                Description = deserializedMetadata.Description,
                FileId = file.Id,
                File = file
            };

            _context.MetadataEntries.Add(metadata);
            await _context.SaveChangesAsync();

            return metadata;
        }

        public async Task<string> UpdateFileMetadata(long fileId, long metadataId)
        {
            var metadata = await _context.MetadataEntries.FindAsync(metadataId);
            var file = await _context.FileItems.FindAsync(fileId);
            if (metadata == null || file == null) {
                throw new InvalidOperationException("File or Metadata not found.");
            }

            string fileName = file.Name;            
            string uploadDir = Path.Combine(_env.WebRootPath, "images");
            string filePath = Path.Combine(uploadDir, fileName);

            return MaliciousMetadataWriter.MetadataHelper.WriteMetadata(filePath, metadata.Author, metadata.Description);
        }
    }
}