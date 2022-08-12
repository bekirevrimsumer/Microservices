using Microsoft.AspNetCore.Mvc;
using PhotoStock.API.Dtos;
using Shared.BaseController;
using Shared.Dtos;

namespace PhotoStock.API.Controllers;

public class PhotoController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
    {
        if (photo.Length <= 0) 
            return CreateActionResultInstance(Response<PhotoDto>.Error("Photo is empty", 400));
        
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", photo.FileName);
        await using var stream = new FileStream(path, FileMode.Create);
        await photo.CopyToAsync(stream, cancellationToken);
        var returnPath = Path.Combine("Photos/" + photo.FileName);
        var photoDto = new PhotoDto()
        {
            Url = returnPath
        };

        return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
    }
    
    [HttpGet]
    public IActionResult PhotoDelete(string photoUrl)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", photoUrl);
        if (!System.IO.File.Exists(path))
        {
            return CreateActionResultInstance(Response<NoContent>.Error("Photo is not found", 400));
        }
        System.IO.File.Delete(path);
        return CreateActionResultInstance(Response<NoContent>.Success(204));
    }
}