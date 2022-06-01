using Livestreaming.Domain.Models;
using Livestreaming.Infrastructure.ORM;
using Microsoft.EntityFrameworkCore;

namespace Livestreaming.Infrastructure.Repositories;

public class LivestreamRepository : ILivestreamRepository
{
    private readonly LivestreamContext context;

    public LivestreamRepository(LivestreamContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(LivestreamProperties newStreamProperties)
    {
        await context.Livestreams.AddAsync(newStreamProperties);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var streamProperties = await context.Livestreams.FirstOrDefaultAsync(sp => sp.LivestreamId == id);
        if (streamProperties is not null)
        {
            context.Remove(streamProperties);
            await context.SaveChangesAsync();
        }
    }

    public async Task<LivestreamProperties> GetAsync(string id)
    {
        return await context.Livestreams.FirstOrDefaultAsync(sp => sp.LivestreamId == id);
    }

    public async Task<ICollection<LivestreamProperties>> GetAllAsync(string id, int page)
    {
        return await context.Livestreams.Where(ls => ls.UserId == id)
                                        .OrderByDescending(ls => ls.LiveStartDate)
                                        .ThenBy(ls => ls.Status)
                                        .Skip((page - 1) * 12)
                                        .Take(12)
                                        .ToListAsync();
    }

    public async Task SaveEntitiesChangesAsync() => await context.SaveChangesAsync();
}