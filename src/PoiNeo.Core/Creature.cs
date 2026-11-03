namespace PoiNeo.Core;

public sealed class Creature
{
    public required string Name { get; init; }

    /// <summary>畫在格子裡的簡稱，最多兩個中文字。</summary>
    public required string ShortName { get; init; }

    public required Side Side { get; init; }
    public int Power { get; set; }
    public int Health { get; set; }
    public int Speed { get; set; }

    /// <summary>在自己路線上的第幾格，0 是 S。</summary>
    public int PathIndex { get; set; }
}
