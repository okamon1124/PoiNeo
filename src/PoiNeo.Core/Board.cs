namespace PoiNeo.Core;

public enum CellType { P, F, S, VS }

public enum Side { Player, Enemy }

/// <summary>格子座標，從 0 開始。顯示給玩家時要 +1（第 1 列第 1 格）。</summary>
public readonly record struct Pos(int Row, int Col)
{
    public override string ToString() => $"第{Row + 1}列第{Col + 1}格";
}

/// <summary>場地：格子種類和雙方的路線，照 GameDesign.drawio 的「遊戲場地」頁。</summary>
public sealed class Board
{
    public int Rows { get; }
    public int Cols { get; }
    private readonly CellType[,] _cells;
    private readonly Dictionary<Side, IReadOnlyList<Pos>> _paths;

    private Board(CellType[,] cells, Dictionary<Side, IReadOnlyList<Pos>> paths)
    {
        _cells = cells;
        _paths = paths;
        Rows = cells.GetLength(0);
        Cols = cells.GetLength(1);
    }

    public CellType this[Pos p] => _cells[p.Row, p.Col];

    /// <summary>路線：第 0 格是 S，最後一格是自己的 VS。</summary>
    public IReadOnlyList<Pos> PathOf(Side side) => _paths[side];

    public Pos VsOf(Side side) => _paths[side][^1];

    public static Board CreateDefault()
    {
        // VS 在圖上是一個跨兩格的框：左邊（第 3 格）是敵方的，右邊（第 4 格）是我方的。
        const CellType P = CellType.P, F = CellType.F, S = CellType.S, V = CellType.VS;
        var cells = new CellType[,]
        {
            { P, P, S, F, F, F },
            { P, F, F, F, F, F },
            { P, P, V, V, P, P },
            { F, F, F, F, F, P },
            { F, F, F, S, P, P },
        };

        var paths = new Dictionary<Side, IReadOnlyList<Pos>>
        {
            [Side.Player] = [new(4, 3), new(4, 4), new(4, 5), new(3, 5), new(2, 5), new(2, 4), new(2, 3)],
            [Side.Enemy] = [new(0, 2), new(0, 1), new(0, 0), new(1, 0), new(2, 0), new(2, 1), new(2, 2)],
        };

        return new Board(cells, paths);
    }
}
