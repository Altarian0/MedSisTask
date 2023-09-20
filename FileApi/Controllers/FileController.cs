using Confluent.Kafka;
using EvilDICOM.Core;
using FileApi.Configs;
using FileApi.Controllers.Common;
using FileApi.Models;
using FileApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly KafkaServices _kafkaServices;
    
    public FileController(KafkaServices kafkaServices)
    {
        _kafkaServices = kafkaServices;
    }

    /// <summary>
    /// Загрузить файл DICOM
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost(ApiRouters.UploadFile)]
    public async Task<ActionResult> UploadFile([FromForm] UploadFileRequestModel model)
    {
        const string studyInstance = "0020000D";
        
        var ext = Path.GetExtension(model.File.FileName);
        if (ext != ".dcm")
            return BadRequest("Файл должен быть формата .dcm");

        string message;
        using (var ms = new MemoryStream())
        {
            try
            {
                await model.File.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                var dicomObject = DICOMObject.Read(fileBytes);
                message = dicomObject.FindFirst(studyInstance).DData.ToString() ?? throw new InvalidOperationException();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("Ошибка чтения dcm файла.");
            }
        }

        await _kafkaServices.SendToKafka(message);
        
        return Ok();
    }
    
    
}