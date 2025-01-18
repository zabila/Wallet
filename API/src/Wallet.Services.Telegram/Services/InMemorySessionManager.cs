using System.Collections.Concurrent;
using Stateless;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.Services;

public class InMemorySessionManager(IBotStateMachineFactory machineFactory) : ISessionManager {
    private readonly ConcurrentDictionary<long, UserSession> _sessions = [];

    public Task<UserSession> GetOrCreateSessionAsync(long charId) {
        return Task.Run(() => {
            var session = _sessions.GetOrAdd(charId, static id => new UserSession {
                ChatId = id
            });

            session.CurrentStateMachine = machineFactory.CreateStateMachine(session);
            return session;
        });
    }

    public Task RemoveSessionAsync(long chatId) {
        _sessions.TryRemove(chatId, out _);
        return Task.CompletedTask;
    }
}