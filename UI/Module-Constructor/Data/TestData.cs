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
            var hdf = new Material()
            {
                Name = "ХДФ 3",
                Thickness = 3
            };


            yield return new Panel()
            {
                Name = "Дно",
                Orientation = Panel.PanelOrientation.Horizontal,
                BottomMargin = 0,
                Material = ldsp,
                Anchor = Panel.PanelAnchor.Bottom
            };

            yield return new Panel()
            {
                Name = "Бок левый",
                Orientation = Panel.PanelOrientation.Vertical,
                Material = ldsp,
                LeftMargin = 0,
                Anchor = Panel.PanelAnchor.Left
            };

            yield return new Panel()
            {
                Name = "Бок правый",
                Orientation = Panel.PanelOrientation.Vertical,
                Material = ldsp,
                RightMargin = 0,
                Anchor = Panel.PanelAnchor.Right
            };

            yield return new Panel()
            {
                Name = "Планка фронт",
                Orientation = Panel.PanelOrientation.Horizontal,
                FixedWidth = 100,
                Material = ldsp,
                TopMargin = 0,
                FrontMargin = 0,
                Anchor = Panel.PanelAnchor.Top
            };

            yield return new Panel()
            {
                Name = "Планка зад",
                Orientation = Panel.PanelOrientation.Horizontal,
                FixedWidth = 100,
                Material = ldsp,
                TopMargin = 0,
                BackMargin = 0,
                Anchor = Panel.PanelAnchor.Top
            };

            yield return new Panel()
            {
                Name = "Полка",
                Orientation = Panel.PanelOrientation.Horizontal,
                FrontMargin = 30,
                Material = ldsp,
                BackMargin = 0,
                LeftMargin = 1,
                RightMargin = 1
            };


            yield return new Panel()
            {
                Name = "Задник",
                Orientation = Panel.PanelOrientation.Frontal,
                BackMargin = -3,
                LeftMargin = 2,
                RightMargin = 2,
                TopMargin = 2,
                BottomMargin = 2,
                Material = hdf,
                Anchor = Panel.PanelAnchor.Back
            };

        }

        public static IEnumerable<Panel> GetDeskPanels()
        {
            var ldsp = new Material()
            {
                Name = "ЛДСП 16",
                Thickness = 16
            };
            var ldsp32 = new Material()
            {
                Name = "ЛДСП 32",
                Thickness = 32
            };


            yield return new Panel()
            {
                Name = "Верх",
                Orientation = Panel.PanelOrientation.Horizontal,
                TopMargin = 0,
                Material = ldsp32,
                Anchor = Panel.PanelAnchor.Top
            };

            yield return new Panel()
            {
                Name = "Бок левый",
                Orientation = Panel.PanelOrientation.Vertical,
                Material = ldsp,
                LeftMargin = 0,
                Anchor = Panel.PanelAnchor.Left
            };

            yield return new Panel()
            {
                Name = "Бок правый",
                Orientation = Panel.PanelOrientation.Vertical,
                Material = ldsp,
                RightMargin = 0,
                Anchor = Panel.PanelAnchor.Right
            };

            yield return new Panel()
            {
                Name = "Цоколь",
                Orientation = Panel.PanelOrientation.Frontal,
                FixedWidth = 70,
                Material = ldsp,
                BottomMargin = 0,
                FrontMargin = 0,
                Anchor = Panel.PanelAnchor.Bottom
            };

            yield return new Panel()
            {
                Name = "Дно",
                Orientation = Panel.PanelOrientation.Horizontal,
                Material = ldsp,
                BottomMargin = 0,
                Anchor = Panel.PanelAnchor.Bottom
            };

            yield return new Panel()
            {
                Name = "Задник",
                Orientation = Panel.PanelOrientation.Frontal,
                BackMargin = 0,
                Material = ldsp,
                Anchor = Panel.PanelAnchor.Back
            };

            yield return new Panel()
            {
                Name = "Полка",
                Orientation = Panel.PanelOrientation.Horizontal,
                FrontMargin = 30,
                Material = ldsp,
                BackMargin = 0,
                LeftMargin = 1,
                RightMargin = 1
            };

        }

    }
}
