namespace Livestreaming.Domain.Models;

public enum LivestreamStatus
{
    /// <summary>
    /// All the necessary resources had been allocated.
    /// </summary>
    Setup = 0,

    /// <summary>
    /// The livestream is running and can receive input.
    /// </summary>
    Live = 1,

    /// <summary>
    /// The video playback was saved and the allocated resources were released.
    /// </summary>
    Saved = 2
}