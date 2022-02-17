using System.Collections.Generic;

namespace Module_Constructor.Models
{
    /// <summary> Модуль </summary>
    public class Module
    {
        /// <summary> Имя модуля </summary>
        public string Name { get; set; }

        /// <summary> Ширина модуля </summary>
        public int Width { get; set; }

        /// <summary> Высота модуля </summary>
        public int Height { get; set; }

        /// <summary> Глубина модуля </summary>
        public int Depth { get; set; }

        public ICollection<Panel> Panels { get; set; }

    }
}
