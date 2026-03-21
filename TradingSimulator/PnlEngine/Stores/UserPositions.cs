using PnlEngine.Models;
using System.Collections.Concurrent;

public class UserPositions
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Position>> _positions = new();

    public Position? Get(string user, string ticker)
    {
        if (_positions.TryGetValue(user, out var userPositions) &&
            userPositions.TryGetValue(ticker, out var position))
        {
            return position;
        }

        return null;
    }

    public IEnumerable<Position> GetAll(string user)
    {
        if (_positions.TryGetValue(user, out var userPositions))
            return userPositions.Values;

        return Enumerable.Empty<Position>();
    }

    public void Update(string user, Position position)
    {
        var userPositions = _positions.GetOrAdd(user, _ => new ConcurrentDictionary<string, Position>());

        userPositions[position.Ticker] = position;
    }
}