using Livestreaming.Domain.Models;

namespace Livestreaming.Infrastructure.Services;

public interface ILivestreamingService
{
    public LivestreamProperties GetProperties();

    public Task<LivestreamProperties> SetupAsync();

    public Task<LivestreamEndpoints> StartAsync();

    public Task<LivestreamPlaybackEndpoints> StopAsync();

    public Task DeleteAsync();
}