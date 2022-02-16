namespace Module_Constructor.Visualisation.Base;

/// <summary> Позиция детали или секции (нулевая точка - внизу слева сзади) </summary>
public class Position
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public override string ToString() => $"X={X} Y={Y} Z={Z}";
}