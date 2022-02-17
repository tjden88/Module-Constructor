using Module_Constructor.Models;
using Module_Constructor.Visualisation.Base;

namespace Module_Constructor.Visualisation
{
    public class PanelViewModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }

        public bool IsFixedWidth { get; set; }
        public bool IsFixedHeight { get; set; }
        public bool IsFixedDepth { get; set; }

        public Position Position { get; set; } = new();

        public Panel Panel { get; set; }

        public bool HasErrors => Width < 1 || Height <1 || Depth <1; // TODO: валидация данных

    }
}
