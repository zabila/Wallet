using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.Contracts;

/// <summary>
/// Defines methods for managing user sessions within the Telegram-based wallet service.
/// </summary>
public interface ISessionManager {
    /// <summary>
    /// Retrieves an existing user session associated with the specified chat ID,
    /// or creates a new session if none exists.
    /// </summary>
    /// <param name="charId">The unique identifier of the chat for which the session is to be retrieved or created.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="UserSession"/> associated with the given chat ID.</returns>
    Task<UserSession> GetOrCreateSessionAsync(long charId);

    /// <summary>
    /// Removes an existing user session based on the provided chat identifier.
    /// </summary>
    /// <param name="chatId">A unique identifier for the chat session to be removed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveSessionAsync(long chatId);
}