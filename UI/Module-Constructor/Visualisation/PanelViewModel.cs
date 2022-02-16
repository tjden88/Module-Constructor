using Module_Constructor.Models;
using Module_Constructor.Visualisation.Base;

namespace Module_Constructor.Visualisation
{
    public class PanelViewModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }

        public Position Position { get; set; } = new();

        public Panel.PanelOrientation Orientation { get; set; }

        public Panel.PanelAnchor Anchor { get; set; }

        public bool HasErrors => false; // TODO: валидация данных

    }
}
