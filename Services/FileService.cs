using System;
using Files.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Files.Services
{
    public class FileService {
        private readonly FileContext _context;
        private readonly IWebHostEnvironment _env;
        public FileService(FileContext context, IWebHostEnvironment env) {
            _context = context;
            _env = env;
        }

        public async Task<Models.File?> GetFileById(long id) {
            return await _context.FileItems.FindAsync(id);
        }

        public async Task<List<Models.File>> GetFiles() {
            return await _context.FileItems.ToListAsync();
        }

        public async Task<bool> ValidateFile(FileDTO fileDTO) {
            var dupFile = await _context.FileItems.FirstOrDefaultAsync(e => e.Name == fileDTO.Name);
            return dupFile != null;
        }

        public async Task<Models.File> SaveFileAsync(Models.FileDTO fileDTO) {
            var file = new Files.Models.File {
                Name = fileDTO.Name,
                BinaryContent = fileDTO.BinaryContent
            };
            
            _context.FileItems.Add(file);
            await _context.SaveChangesAsync();

            return file;
        }

        public async Task<String> CreateFile(Models.FileDTO fileDTO) {
            byte[] contentInByte = Convert.FromBase64String(fileDTO.BinaryContent);
            // Console.WriteLine(System.Text.Encoding.UTF8.GetString(contentInByte));
            string fileName = fileDTO.Name;

            string filePath = Path.Combine(_env.WebRootPath, "images", fileName); 
            System.IO.File.WriteAllBytes(filePath, contentInByte);
            return fileName;
        }
    }
}