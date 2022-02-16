using System.Collections.Generic;
using Module_Constructor.Models;

namespace Module_Constructor.Data
{
    public static class TestData
    {
        public static IEnumerable<Panel> GetKitchenCabinetPanels()
        {
            var ldsp = new Material()
            {
                Name = "ЛДСП 16",
                Thickness = 16
            };


            yield return new Panel()
            {
                Name = "Дно",
                Orientation = Panel.PanelOrientation.Horizontal,
                BottomMargin = 0,
                Material = ldsp,
            };

            yield return new Panel()
            {
                Name = "Бок левый",
                Orientation = Panel.PanelOrientation.Vertical,
                Material = ldsp,
                LeftMargin = 0
            };

            yield return new Panel()
            {
                Name = "Бок правый",
                Orientation = Panel.PanelOrientation.Vertical,
                Material = ldsp,
                RightMargin = 0
            };

            yield return new Panel()
            {
                Name = "Планка фронт",
                Orientation = Panel.PanelOrientation.Horizontal,
                FixedWidth = 100,
                Material = ldsp,
                TopMargin = 0,
                FrontMargin = 0
            };

            yield return new Panel()
            {
                Name = "Планка зад",
                Orientation = Panel.PanelOrientation.Horizontal,
                FixedWidth = 100,
                Material = ldsp,
                TopMargin = 0,
                BackMargin = 0
            };

            yield return new Panel()
            {
                Name = "Полка",
                Orientation = Panel.PanelOrientation.Horizontal,
                FrontMargin = 30,
                Material = ldsp,
                BackMargin = 0
            };

        }
    }
}
