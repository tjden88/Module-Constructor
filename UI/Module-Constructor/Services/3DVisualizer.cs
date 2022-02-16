using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Media3D;
using Microsoft.Extensions.Logging;
using Module_Constructor.Models;
using Module_Constructor.Services.Interfaces;
using Module_Constructor.Visualisation;

namespace Module_Constructor.Services
{
    public class Visualizer : I3DVisualizer
    {
        private readonly ILogger _Logger;

        public Visualizer(ILogger<Visualizer> Logger) => _Logger = Logger;

        public Model3D CreateModel(Module Module, Panel SelectedPanel = null)
        {
            var sw = Stopwatch.StartNew();
            _Logger.LogInformation("Визуализация модуля {0} построена за {1} мс.", Module.Name, sw.ElapsedMilliseconds);

            return null;
        }

        // Построить модель из детали
        private PanelViewModel BuildViewModel(Panel panel, int AreaWidth, int AreaHeight, int AreaDepth)
        {
            var viewModel = new PanelViewModel()
            {
                Orientation = panel.Orientation,
                Anchor = panel.Anchor,
            };

            // Установить размеры и положение в зависимости от ориентации
            switch (panel.Orientation)
            {
                case Panel.PanelOrientation.Horizontal:
                    viewModel.Width = panel.FixedLenght ?? AreaWidth;
                    viewModel.Height = panel.Thickness;
                    viewModel.Depth = panel.FixedWidth ?? AreaDepth;


                    break;
                case Panel.PanelOrientation.Vertical:
                    viewModel.Width = panel.Thickness;
                    viewModel.Height = panel.FixedLenght ?? AreaHeight;
                    viewModel.Depth = panel.FixedWidth ?? AreaDepth;
                    break;
                case Panel.PanelOrientation.Frontal:
                    viewModel.Width = panel.FixedLenght ?? AreaWidth;
                    viewModel.Height = panel.FixedWidth ?? AreaHeight;
                    viewModel.Depth = panel.Thickness;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //// Установить смещения в соответствии с отступами
            viewModel.Position.X = GetOffset(AreaWidth, viewModel.Width, panel.LeftMargin, panel.RightMargin);
            viewModel.Position.Y = GetOffset(AreaHeight, viewModel.Height, panel.BottomMargin, panel.TopMargin);
            viewModel.Position.Z = GetOffset(AreaDepth, viewModel.Depth, panel.BackMargin, panel.FrontMargin);

            return viewModel;
        }


        private int GetOffset(int Bound, int DetalSize, int? FirstMargin, int? SecondMargin) // Получить смещение детали отностиельно начала координат
        {
            if (FirstMargin.HasValue)
            {
                return FirstMargin.Value;
            }

            if (SecondMargin.HasValue)
            {
                return Bound - DetalSize - SecondMargin.Value;
            }

            return (Bound - DetalSize) / 2;
        }


        // Устранить пересечения
        private void ExcludeCollisions(PanelViewModel current, ICollection<PanelViewModel> previousModels, int AreaWidth, int AreaHeight, int AreaDepth)
        {
            var leftOffset = previousModels
                .Where(p => p.Anchor == Panel.PanelAnchor.Left)
                .Select(p => p.Position.X + p.Width)
                .DefaultIfEmpty()
                .Max();

            current.Position.X += leftOffset;


            var rightOffset = previousModels
                .Where(p => p.Anchor == Panel.PanelAnchor.Right)
                .Select(p => AreaWidth - p.Position.X)
                .DefaultIfEmpty()
                .Max();

            current.Width -= rightOffset;
        }

    }
}
