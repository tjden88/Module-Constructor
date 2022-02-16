using System.Collections.Generic;
using Module_Constructor.Models;

namespace Module_Constructor.Data
{
    public static class TestData
    {
        public static IEnumerable<Panel> GetKitchenCabinetPanels()
        {
            yield return new Panel()
            {
                Name = "Дно",
                Orientation = Panel.DetalOrientation.Horizontal,
                BottomMargin = 0
            };

            yield return new Panel()
            {
                Name = "Бок левый",
                Orientation = Panel.DetalOrientation.Vertical,
                LeftMargin = 0
            };

            yield return new Panel()
            {
                Name = "Бок правый",
                Orientation = Panel.DetalOrientation.Vertical,
                RightMargin = 0
            };

            yield return new Panel()
            {
                Name = "Планка фронт",
                Orientation = Panel.DetalOrientation.Horizontal,
                FixedWidth = 100,
                TopMargin = 0
            };

            yield return new Panel()
            {
                Name = "Планка зад",
                Orientation = Panel.DetalOrientation.Horizontal,
                FixedWidth = 100,
                BackMargin = 0
            };

            yield return new Panel()
            {
                Name = "Полка",
                Orientation = Panel.DetalOrientation.Horizontal,
                FrontMargin = 30,
                BackMargin = 0
            };

        }
    }
}
