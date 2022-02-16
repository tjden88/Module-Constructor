namespace Module_Constructor.Models;

/// <summary> Часть модуля с привязками (детали, секции)</summary>
public abstract class ModulePart
{
    /// <summary> Имя </summary>
    public string Name { get; set; }

    /// <summary> Порядок построения </summary>
    public int Order { get; set; }

    /// <summary> Привязка или отступ с левого края </summary>
    public int? LeftMargin { get; set; }

    /// <summary> Привязка или отступ с правого края </summary>
    public int? RightMargin { get; set; }

    /// <summary> Привязка или отступ сверху </summary>
    public int? TopMargin { get; set; }

    /// <summary> Привязка или отступ снизу </summary>
    public int? BottomMargin { get; set; }

    /// <summary> Привязка или отступ с переднего края </summary>
    public int? FrontMargin { get; set; }

    /// <summary> Привязка или отступ с заднего края </summary>
    public int? BackMargin { get; set; }

    /// <summary> Фиксированная длина </summary>
    public int? FixedLenght { get; set; }

    /// <summary> Фиксированная ширина </summary>
    public int? FixedWidth { get; set; }

    /// <summary> Фиксированная глубина </summary>
    public int? FixedDepth { get; set; }


}