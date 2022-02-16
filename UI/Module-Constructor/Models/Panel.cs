
using System.ComponentModel;

namespace Module_Constructor.Models
{
    /// <summary> Шаблон детали </summary>
    public class Panel : ModulePart
    {
        public enum PanelOrientation
        {
            [Description("Горизонтальная")]
            Horizontal,
            [Description("Вертикальная")]
            Vertical,
            [Description("Фронтальная")]
            Frontal
        }

        public enum PanelAnchor
        {
            None,
            Left,
            Right,
            Top,
            Bottom
        }


        /// <summary> Материал детали </summary>
        public Material Material { get; set; }

        /// <summary> Ориентация детали </summary>
        public PanelOrientation Orientation { get; set; }

        /// <summary> Толщина материала детали </summary>
        public int Thickness => Material?.Thickness ?? 0;

        /// <summary> Привязка детали к краям </summary>
        public PanelAnchor Anchor { get; set; } = PanelAnchor.None;

    }
}
