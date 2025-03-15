using Microsoft.AspNetCore.Mvc;
using Files.Models;
using Files.Services;
using Metadata.Services;


namespace Metadata.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class MetadataController : ControllerBase 
    {
        public readonly MetadataService _metadataService;
        public readonly FileService _fileService;
        public MetadataController(MetadataService metadataService, FileService fileService)
        {
            _metadataService = metadataService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<Files.Models.Metadata>> GetMetadata(long id)
        {
            var metadata = await _metadataService.GetMetadataById(id);
            if (metadata == null) {
                return NotFound("Metadata not found");
            };  

            return Ok(metadata);
        }

        [HttpGet]
        public async Task<ActionResult<Files.Models.Metadata>> MetadataList()
        {
            var allMetadata = await _metadataService.GetAllMetadata();
            return Ok(allMetadata);
        }

        [HttpPost]
        public ActionResult<string> SerializeMetadata(Files.Models.MetadataDTO metadataDTO)
        {
            string serializedMetadata = _metadataService.SerializeMetadata(metadataDTO);
            return Ok(serializedMetadata);
        }

        [HttpPost]
        public async Task<ActionResult<Files.Models.Metadata>> AssignMetadataToFile(long fileId, string serializedMetadata)
        {
            var file = await _fileService.GetFileById(fileId);
            if (file == null) {
                return NotFound("File not found");
            }

            MetadataDTO deserializedMetadata = _metadataService.DeserializeMetadata(serializedMetadata);
            var metadata = await _metadataService.SaveMetadata(file, deserializedMetadata);
            var result = await _metadataService.UpdateFileMetadata(fileId, metadata.Id);

            return Ok(result);
        }
    }
}