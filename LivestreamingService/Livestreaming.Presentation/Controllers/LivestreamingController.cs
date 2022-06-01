using Livestreaming.Application.DTOs;
using Livestreaming.Application.Services;
using Livestreaming.Infrastructure.Services.Azure.Exceptions;
using Livestreaming.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Livestreaming.Presentation.Controllers;

[ApiController]
[Route("api/v1/livestreams")]
public class LivestreamingController : ControllerBase
{
    public readonly LivestreamService livestreamService;

    public LivestreamingController(LivestreamService livestreamService)
    {
        this.livestreamService = livestreamService ?? throw new ArgumentNullException(nameof(livestreamService));
    }

    [HttpPost("setup")]
    public async Task<IActionResult> SetupLivestreamAsync([FromBody] SetupLivestreamDto dto)
    {
        try
        {
            var result = await livestreamService.SetupLivestreamAsync(dto);
            if (result == default)
                return BadRequest();

            return Ok(result);
        }
        catch (LivestreamSetupException e)
        {
            return StatusCode((int)e.StatusCode, e.Message);
        }
    }

    [HttpPost("{livestreamId}/start")]
    public async Task<IActionResult> StartLivestreamAsync([FromRoute] string livestreamId, [FromBody] StartLivestreamRequest requestBody)
    {
        try
        {
            var result = await livestreamService.StartLivestreamAsync(new() { LivestreamId = livestreamId, UserId = requestBody.UserId });
            if (result == default)
                return NotFound();

            return Ok(result);
        }
        catch (LivestreamStartException e)
        {
            return StatusCode((int)e.StatusCode, e.Message);
        }
    }

    [HttpGet("{livestreamId}/status")]
    public async Task<IActionResult> GetLivestreamStatusAsync([FromRoute] string livestreamId)
    {
        var result = await livestreamService.GetLivestreamStatusAsync(new() { LivestreamId = livestreamId });
        if (result == default)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("{livestreamId}/stop")]
    public async Task<IActionResult> StopLivestreamAsync([FromRoute] string livestreamId, [FromBody] StopLivestreamRequest requestBody)
    {
        try
        {
            await livestreamService.StopLivestreamAsync(new() { LivestreamId = livestreamId, UserId = requestBody.UserId });
            return Ok();
        }
        catch (LivestreamCleanupException e)
        {
            return StatusCode((int)e.StatusCode, e.Message);
        }
    }

    [HttpGet("ofUser/{userId}/{page}")]
    public async Task<IActionResult> GetLivestreamsAsync([FromRoute] string userId, [FromRoute] int page)

    {
        var result = await livestreamService.GetLivestreamsAsync(new() { UserId = userId, Page = page });
        if (result == default)
            return NoContent();

        return Ok(result);
    }

    [HttpGet("{livestreamId}/watch")]
    public async Task<IActionResult> GetLivestreamEndpointsAsync([FromRoute] string livestreamId)
    {
        var result = await livestreamService.GetLivestreamEndpointsAsync(new() { LivestreamId = livestreamId });
        if (result == default)
            return NotFound();

        return Ok(result); ;
    }
}