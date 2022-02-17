using System.Collections.Generic;
using Module_Constructor.Models;
using Module_Constructor.Visualisation;

namespace Module_Constructor.Services.Interfaces
{
    public interface IModuleBuilder
    {
        IEnumerable<PanelViewModel> BuildPanels(Module module);

        PanelViewModel BuildPanel(Module module);

        void AddPanel(Module module, Panel panel);

        void RemovePanel(Module module, Panel panel);

        void SetPanelOrder(Panel panel, int order);
    }
}
