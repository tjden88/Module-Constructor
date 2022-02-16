using System.Windows.Media.Media3D;
using Module_Constructor.Models;

namespace Module_Constructor.Services.Interfaces
{
    public interface I3DVisualizer
    {
        Model3D CreateModel(Module Module, Panel SelectedPanel = null);
    }
}
