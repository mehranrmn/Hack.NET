using Microsoft.AspNetCore.Mvc;
using Files.Models;
using Microsoft.EntityFrameworkCore;

namespace Files.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class FileController : ControllerBase 
    {
        private readonly FileContext _context;

        public FileController(FileContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Files.Models.File>> GetFile(long id) 
        {
            var file = await _context.FileItems.FindAsync(id);

            if (file == null) {
                return NotFound("File not found");
            }

            return file;
        }

        [HttpGet]
        public async Task<ActionResult> GetFiles() 
        {
            var files = await _context.FileItems.ToListAsync();
            return Ok(files);
        }

        [HttpPost]
        public async Task<ActionResult<Files.Models.FileDTO>> UploadFile(FileDTO fileDTO) 
        {
            var existingFile = _context.FileItems.FirstOrDefaultAsync(e => e.Name == fileDTO.Name).Result;
            if (existingFile != null) {
                return BadRequest("File with this name already exists");
            }

            var file = new Files.Models.File {
                Name = fileDTO.Name,
                BinaryContent = fileDTO.BinaryContent
            };
            
            _context.FileItems.Add(file);
            await _context.SaveChangesAsync();

            // return Ok();
            return CreatedAtAction(nameof(GetFile), new { id = file.Id }, file);
        }
    }
}