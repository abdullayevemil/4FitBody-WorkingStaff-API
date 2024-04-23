namespace Just4Fit_WorkingStaff.Presentation.Controllers;

using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.Food.Models;
using Just4Fit_WorkingStaff.Infrastructure.Food.Commands;
using Just4Fit_WorkingStaff.Infrastructure.Food.Queries;
using Just4Fit_WorkingStaff.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]/[action]")]
public class FoodController : ControllerBase
{
    private readonly ISender sender;
    private readonly BlobContainerService blobContainerService;

    public FoodController(ISender sender)
    {
        this.sender = sender;

        this.blobContainerService = new BlobContainerService();
    }

    [HttpGet]
    [ActionName("Index")]
    public async Task<IActionResult> GetAll()
    {
        var getAllQuery = new GetAllQuery();

        var food = await this.sender.Send(getAllQuery);

        return base.Ok(food);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] Food food, IFormFile imageFile, IFormFile contentFile)
    {
        var rawPath = Guid.NewGuid().ToString() + imageFile.FileName;

        var path = rawPath.Replace(" ", "%20");

        food.ImageUrl = "https://4fitbodystorage.blob.core.windows.net/images/" + path;

        await this.blobContainerService.UploadAsync(imageFile.OpenReadStream(), rawPath);

        var videoRawPath = Guid.NewGuid().ToString() + contentFile.FileName;

        var videoPath = videoRawPath.Replace(" ", "%20");

        food.VideoUrl = "https://4fitbodystorage.blob.core.windows.net/images/" + videoPath;

        await this.blobContainerService.UploadAsync(contentFile.OpenReadStream(), videoRawPath);

        var createCommand = new CreateCommand(food);

        await this.sender.Send(createCommand);

        return base.RedirectToAction("Index");
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int? id)
    {
        var createCommand = new DeleteCommand(id);

        await this.sender.Send(createCommand);

        return base.Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(int? id, [FromBody] Food food)
    {
        var createCommand = new UpdateCommand(id, food);

        await this.sender.Send(createCommand);

        return base.Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var getByIdQuery = new GetByIdQuery(id);

        var food = await this.sender.Send(getByIdQuery);

        return base.Ok(food);
    }
}
