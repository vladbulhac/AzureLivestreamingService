using Livestreaming.Application.DTOs;
using Livestreaming.Domain.Models;
using Livestreaming.Infrastructure.Repositories;
using Livestreaming.Infrastructure.Services.Azure.MediaServices;
using Microsoft.Extensions.Logging;

namespace Livestreaming.Application.Services;

public class LivestreamService
{
    private readonly ILivestreamRepository repository;
    private readonly ILogger<AzureLivestreamingService> logger;

    public LivestreamService(ILivestreamRepository repository, ILogger<AzureLivestreamingService> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    public async Task<SetupLivestreamResultDto> SetupLivestreamAsync(SetupLivestreamDto setupData)
    {
        var livestreamProperties = new LivestreamProperties(setupData.Title,
                                                            setupData.StreamingProtocol,
                                                            setupData.EncodingType,
                                                            userId: setupData.UserId,
                                                            description: setupData.Description,
                                                            recordingDuration: setupData.RecordingDuration);
        var azureStreamingService = await AzureLivestreamingService.FromAsync(livestreamProperties, logger);
        await azureStreamingService.SetupAsync();

        livestreamProperties = azureStreamingService.GetProperties();
        await repository.AddAsync(livestreamProperties);

        return new() { LivestreamId = livestreamProperties.LivestreamId };
    }

    public async Task<LivestreamEndpoints> StartLivestreamAsync(StartLivestreamDto startData)
    {
        var livestreamProperties = await repository.GetAsync(startData.LivestreamId);
        if (livestreamProperties == default)
            return default;

        var azureStreamingService = await AzureLivestreamingService.FromAsync(livestreamProperties, logger);
        var runningEndpoints = await azureStreamingService.StartAsync();

        await repository.SaveEntitiesChangesAsync();

        return runningEndpoints;
    }

    public async Task StopLivestreamAsync(StopLivestreamDto stopData)
    {
        var livestreamProperties = await repository.GetAsync(stopData.LivestreamId);
        if (livestreamProperties == default)
            return;

        var azureStreamingService = await AzureLivestreamingService.FromAsync(livestreamProperties, logger);
        var playbackEndpoints = await azureStreamingService.StopAsync();

        await repository.SaveEntitiesChangesAsync();
    }

    public async Task<LivestreamDetails> GetLivestreamStatusAsync(GetLivestreamStatusDto getStatusData)
    {
        var livestreamData = await repository.GetAsync(getStatusData.LivestreamId);
        if (livestreamData == default)
            return default;

        return new()
        {
            LivestreamId = livestreamData.LivestreamId,
            Title = livestreamData.Title,
            Description = livestreamData.Description,
            Date = livestreamData.LiveStartDate,
            Status = livestreamData.Status.ToString()
        };
    }

    public async Task<UserLivestreamsDto> GetLivestreamsAsync(GetLivestreamsDto fetchData)
    {
        var userLivestreams = await repository.GetAllAsync(fetchData.UserId, fetchData.Page);
        if (userLivestreams.Count == 0)
            return new() { livestreams = Enumerable.Empty<LivestreamDetails>() };

        return new()
        {
            livestreams = userLivestreams.Select(ls => new LivestreamDetails()
            {
                LivestreamId = ls.LivestreamId,
                Title = ls.Title,
                Description = ls.Description,
                Date = ls.LiveStartDate,
                Status = ls.Status.ToString()
            })
            .ToList()
        };
    }

    public async Task<GetLivestreamEndpointsResult> GetLivestreamEndpointsAsync(GetLivestreamEndpointsDto livestreamData)
    {
        var result = await repository.GetAsync(livestreamData.LivestreamId);
        return new()
        {
            Title = result.Title,
            Description = result.Description,
            Status = result.Status.ToString(),
            Date = result.LiveStartDate,
            LivestreamId = result.LivestreamId,
            RunningEndpoints = result.RunningEndpoints,
            PlaybackEndpoints = result.PlaybackEndpoints
        };
    }
}