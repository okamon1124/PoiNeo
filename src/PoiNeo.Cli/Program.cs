using System.Text;
using PoiNeo.Cli;
using PoiNeo.Core;

Console.OutputEncoding = Encoding.UTF8;

var game = new Game(Board.CreateDefault());
game.PlaceAtS(new Creature { Name = "哥布林", ShortName = "哥", Side = Side.Player, Power = 2, Health = 1, Speed = 2 });

Console.WriteLine(BoardRenderer.RenderPath(game.Board, Side.Player));
Console.WriteLine(BoardRenderer.RenderPath(game.Board, Side.Enemy));
Console.WriteLine();
Console.WriteLine("=== 開始：哥布林放在我方 S ===");
Console.WriteLine(BoardRenderer.Render(game));

while (true)
{
    Console.Write("按 Enter 跑下一個移動階段，輸入 q 離開：");
    var input = Console.ReadLine();
    if (input is null || input.Trim().Equals("q", StringComparison.OrdinalIgnoreCase)) break;

    var log = game.MovePhase(Side.Player);
    Console.WriteLine();
    Console.WriteLine($"=== 第 {game.Turn} 回合・我方・移動階段後 ===");
    Console.WriteLine(BoardRenderer.Render(game));
    foreach (var line in log) Console.WriteLine("・" + line);
    Console.WriteLine();
    game.EndTurn();
}
