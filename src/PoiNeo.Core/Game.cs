namespace PoiNeo.Core;

/// <summary>一場戰鬥的狀態。目前只做了放生物和移動階段。</summary>
public sealed class Game
{
    public Board Board { get; }
    public int Turn { get; private set; } = 1;
    private readonly List<Creature> _creatures = [];

    public Game(Board board) => Board = board;

    public IReadOnlyList<Creature> Creatures => _creatures;

    public Pos PositionOf(Creature c) => Board.PathOf(c.Side)[c.PathIndex];

    /// <summary>同一格的生物，照牌堆順序（最前面的在前）。</summary>
    public IEnumerable<Creature> CreaturesAt(Pos p) => _creatures.Where(c => PositionOf(c) == p);

    /// <summary>放到自己的 S 格，排在牌堆最後面（詳細規則第 5 條）。</summary>
    public void PlaceAtS(Creature c)
    {
        c.PathIndex = 0;
        _creatures.Add(c);
    }

    /// <summary>移動階段：該方的生物依各自的移速前進，走到 VS 就停（第 9～12 條）。</summary>
    public List<string> MovePhase(Side side)
    {
        var log = new List<string>();
        var path = Board.PathOf(side);
        foreach (var c in _creatures.Where(c => c.Side == side))
        {
            var from = PositionOf(c);
            var steps = Math.Min(Math.Max(c.Speed, 0), path.Count - 1 - c.PathIndex);
            if (steps == 0)
            {
                log.Add(c.PathIndex == path.Count - 1 ? $"{c.Name} 已經在 VS，不移動" : $"{c.Name} 移速 0，停在原地");
                continue;
            }

            c.PathIndex += steps;
            var to = PositionOf(c);
            var arrived = c.PathIndex == path.Count - 1 ? "，抵達 VS" : "";
            log.Add($"{c.Name}（移速 {c.Speed}）從{from}走 {steps} 步到{to}{arrived}");
        }
        return log;
    }

    public void EndTurn() => Turn++;
}
