using System;
using System.Collections.Generic;
using Module_Constructor.Models;
using Module_Constructor.Services.Interfaces;
using Module_Constructor.Visualisation;

namespace Module_Constructor.Services
{
    public class ModuleBuilder : IModuleBuilder
    {
        public IEnumerable<PanelViewModel> BuildPanels(Module module)
        {
            throw new NotImplementedException();
        }

        public PanelViewModel BuildPanel(Module module)
        {
            throw new NotImplementedException();
        }

        public void AddPanel(Module module, Panel panel)
        {
            throw new NotImplementedException();
        }

        public void RemovePanel(Module module, Panel panel)
        {
            throw new NotImplementedException();
        }

        public void SetPanelOrder(Panel panel, int order)
        {
            throw new NotImplementedException();
        }
    }
}
