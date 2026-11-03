using System.Text;
using PoiNeo.Core;

namespace PoiNeo.Cli;

/// <summary>用文字把場地畫成固定寬度的表格。中文字算 2 格寬，英數算 1 格寬。</summary>
public static class BoardRenderer
{
    private const int CellWidth = 6;
    private const string RowLabelPad = "     ";

    public static string Render(Game game)
    {
        var board = game.Board;
        var sb = new StringBuilder();
        var border = RowLabelPad + "+" + string.Concat(Enumerable.Repeat(new string('-', CellWidth) + "+", board.Cols));

        sb.Append(RowLabelPad);
        for (var c = 0; c < board.Cols; c++)
            sb.Append(' ').Append(Center((c + 1).ToString(), CellWidth));
        sb.AppendLine();
        sb.AppendLine(border);

        for (var r = 0; r < board.Rows; r++)
        {
            var types = new StringBuilder($"第{r + 1}列|");
            var units = new StringBuilder(RowLabelPad + "|");
            for (var c = 0; c < board.Cols; c++)
            {
                var p = new Pos(r, c);
                types.Append(Center(CellLabel(board, p), CellWidth)).Append('|');
                units.Append(Center(UnitLabel(game, p), CellWidth)).Append('|');
            }
            sb.AppendLine(types.ToString());
            sb.AppendLine(units.ToString());
            sb.AppendLine(border);
        }

        foreach (var group in game.Creatures.GroupBy(game.PositionOf))
        {
            var stack = string.Join("、", group.Select(c => $"{c.Name} {c.Power}/{c.Health} 移速{c.Speed}"));
            var label = group.Count() > 1 ? $"牌堆，從前到後：{stack}" : stack;
            sb.AppendLine($"・{group.Key}：{label}");
        }

        return sb.ToString();
    }

    public static string RenderPath(Board board, Side side)
    {
        var name = side == Side.Player ? "我方" : "敵方";
        var steps = board.PathOf(side).Select((p, i) => i == 0 ? $"{p}(S)" : i == board.PathOf(side).Count - 1 ? $"{p}({name}VS)" : p.ToString());
        return $"{name}路線：" + string.Join(" → ", steps);
    }

    private static string CellLabel(Board board, Pos p)
    {
        var type = board[p];
        if (type != CellType.VS) return type.ToString();
        return p == board.VsOf(Side.Player) ? "VS我" : "VS敵";
    }

    private static string UnitLabel(Game game, Pos p)
    {
        var here = game.CreaturesAt(p).ToList();
        return here.Count switch
        {
            0 => "",
            1 => here[0].ShortName,
            _ => $"{here[0].ShortName}x{here.Count}",
        };
    }

    private static string Center(string s, int width)
    {
        var pad = Math.Max(width - DisplayWidth(s), 0);
        var left = pad / 2;
        return new string(' ', left) + s + new string(' ', pad - left);
    }

    public static int DisplayWidth(string s) => s.Sum(ch => IsWide(ch) ? 2 : 1);

    private static bool IsWide(char ch) =>
        ch is >= 'ᄀ' and <= 'ᅟ'
           or >= '⺀' and <= '꓏'
           or >= '가' and <= '힣'
           or >= '豈' and <= '﫿'
           or >= '︰' and <= '﹏'
           or >= '＀' and <= '｠'
           or >= '￠' and <= '￦';
}
