using System.Windows.Controls;
using Module_Constructor.Visualisation.Base;

namespace Module_Constructor.Visualisation
{
    public class PanelViewModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }

        public Position Position { get; set; }

        public Orientation Orientation { get; set; }

    }
}
