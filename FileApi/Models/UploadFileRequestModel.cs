using System.ComponentModel.DataAnnotations;

namespace FileApi.Models;

public class UploadFileRequestModel
{
    public IFormFile File { get; set; }
}