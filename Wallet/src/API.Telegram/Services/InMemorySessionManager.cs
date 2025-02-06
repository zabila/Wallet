using System.Collections.Concurrent;
using API.Telegram.Contracts;
using API.Telegram.Models;
using SharedKernel.Extensions;

namespace API.Telegram.Services;

public class InMemorySessionManager(IBotStateMachineFactory machineFactory, IWalletIdentityClient identityClient) : ISessionManager
{
    private readonly ConcurrentDictionary<long, UserSession> _sessions = [];

    public async Task<UserSession> GetOrCreateSessionAsync(long charId)
    {
        var session = _sessions.GetOrAdd(charId, static id => new UserSession
        {
            ChatId = id
        });

        if (session.LoggenUser is null)
        {
            var currentUser = await identityClient.GetCurrentUserByTelegramUserIdAsync(charId).EnsureExists();

            session.LoggenUser = new LoggenUser
            {
                UserId = currentUser.Id,
                AccoundId = currentUser.AccountId,
                Localization = currentUser.Localization
            };
        }


        if (session.CurrentStateMachine is not null)
        {
            return session;
        }

        session.CurrentStateMachine = machineFactory.CreateStateMachine(session);
        return session;
    }

    public Task RemoveSessionAsync(long chatId)
    {
        _sessions.TryRemove(chatId, out _);
        return Task.CompletedTask;
    }
}
