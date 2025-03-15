using Microsoft.AspNetCore.Mvc;
using Files.Models;
using Files.Services;


namespace Files.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class FileController : ControllerBase 
    {
        public readonly FileService _fileService;
        public FileController(FileService fileService) 
        {
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<Files.Models.File>> GetFile(long id) 
        {
            var file = await _fileService.GetFileById(id);

            if (file == null) {
                return NotFound("File not found");
            }

            return file;
        }

        [HttpGet]
        public async Task<ActionResult> FileList() 
        {
            var files = await _fileService.GetFiles();
            return Ok(files);
        }

        [HttpPost]
        public async Task<ActionResult<string>> ConvertFileToB64(IFormFile file) 
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                string encodedBlob = await _fileService.ByteEncoder(file);
                return Ok(encodedBlob);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Files.Models.FileDTO>> UploadFile(FileDTO fileDTO) 
        {
            try {
                var isValid = await _fileService.ValidateFile(fileDTO);
                if (isValid) {
                    return BadRequest("Invalid file");
                }

                var file = await _fileService.SaveFileAsync(fileDTO);

                await _fileService.CreateFile(fileDTO);

                return CreatedAtAction(nameof(GetFile), new { id = file.Id }, file);
            }
            catch (InvalidOperationException ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}