
using System.ComponentModel;

namespace Module_Constructor.Models
{
    /// <summary> Шаблон детали </summary>
    public class Panel : ModulePart
    {
        public enum DetalOrientation
        {
            [Description("Горизонтальная")]
            Horizontal,
            [Description("Вертикальная")]
            Vertical,
            [Description("Фронтальная")]
            Frontal
        }


        /// <summary> Материал детали </summary>
        public Material Material { get; set; }

        /// <summary> Ориентация детали </summary>
        public DetalOrientation Orientation { get; set; }

    }
}
