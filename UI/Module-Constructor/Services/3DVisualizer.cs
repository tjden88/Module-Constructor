using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
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


        public Model3D CreateModel(Module Module, Panel SelectedPanel)
        {
            var sw = Stopwatch.StartNew();

            // Create a model group
            var modelGroup = new Model3DGroup();

            // Create some materials
            var greenMaterial = MaterialHelper.CreateMaterial(Colors.Green);
            var redMaterial = MaterialHelper.CreateMaterial(Colors.Red);
            var blueMaterial = MaterialHelper.CreateMaterial(Colors.Blue);
            var insideMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);


            var panels = new List<PanelViewModel>();

            foreach (var modulePart in Module.Parts)
            {
                var isSelected = SelectedPanel?.Equals(modulePart);

                var panelModel = BuildViewModel((Panel)modulePart, Module.Width, Module.Height, Module.Depth);

                ExcludeCollisions(panelModel, panels, Module.Width, Module.Height, Module.Depth);

                if (panelModel.HasErrors)
                {
                    _Logger.LogWarning("Ошибки построения детали {0}", panelModel.Name);
                    continue;
                }

                panels.Add(panelModel);

                var meshBuilder = new MeshBuilder(false, false);


                var locationPoint = new Point3D(panelModel.Position.Z, panelModel.Position.X, panelModel.Position.Y);
                var size = new Size3D(panelModel.Depth, panelModel.Width, panelModel.Height);

                var rect = new Rect3D(locationPoint, size);

                meshBuilder.AddBox(rect);

                // Create a mesh from the builder (and freeze it)
                var mesh = meshBuilder.ToMesh(true);

                var material = isSelected == true ? greenMaterial : redMaterial;
                // Add 3 models to the group (using the same mesh, that's why we had to freeze it)
                modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = material, BackMaterial = insideMaterial });
            }
            _Logger.LogInformation("Визуализация модуля {0} построена за {1} мс.", Module.Name, sw.ElapsedMilliseconds);

            return modelGroup;
        }



        // Построить модель из детали
        private PanelViewModel BuildViewModel(Panel panel, int AreaWidth, int AreaHeight, int AreaDepth)
        {
            var viewModel = new PanelViewModel()
            {
                Orientation = panel.Orientation,
                Anchor = panel.Anchor,
                Name = panel.Name
            };

            // Установить размеры и положение в зависимости от ориентации
            switch (panel.Orientation)
            {
                case Panel.PanelOrientation.Horizontal:
                    viewModel.Width = panel.FixedLenght ?? GetSize(AreaWidth, panel.LeftMargin, panel.RightMargin);
                    viewModel.Height = panel.Thickness;
                    viewModel.Depth = panel.FixedWidth ?? GetSize(AreaDepth, panel.FrontMargin, panel.BottomMargin);

                    viewModel.IsFixedWidth = panel.FixedLenght != null;
                    viewModel.IsFixedHeight = true;
                    viewModel.IsFixedDepth = panel.FixedWidth != null;

                    break;
                case Panel.PanelOrientation.Vertical:
                    viewModel.Width = panel.Thickness;
                    viewModel.Height = panel.FixedLenght ?? GetSize(AreaHeight, panel.BottomMargin, panel.TopMargin);
                    viewModel.Depth = panel.FixedWidth ?? GetSize(AreaDepth, panel.FrontMargin, panel.BottomMargin);

                    viewModel.IsFixedWidth = true;
                    viewModel.IsFixedHeight = panel.FixedLenght != null;
                    viewModel.IsFixedDepth = panel.FixedWidth != null;
                    break;
                case Panel.PanelOrientation.Frontal:
                    viewModel.Width = panel.FixedLenght ?? GetSize(AreaWidth, panel.LeftMargin, panel.RightMargin);
                    viewModel.Height = panel.FixedWidth ?? GetSize(AreaHeight, panel.BottomMargin, panel.TopMargin);
                    viewModel.Depth = panel.Thickness;

                    viewModel.IsFixedWidth = panel.FixedLenght != null;
                    viewModel.IsFixedHeight = panel.FixedWidth != null;
                    viewModel.IsFixedDepth = true;
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


        private int GetSize(int Bound, int? FirstMargin, int? SecondMargin) // Получить размер детали
        {
            if (FirstMargin.HasValue)
            {
                Bound -= FirstMargin.Value;
            }

            if (SecondMargin.HasValue)
            {
                Bound -= SecondMargin.Value;
            }
            return Bound;
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
                .Where(p => p.Anchor == Panel.PanelAnchor.Left && current.Anchor != Panel.PanelAnchor.Right)
                .Select(p => p.Position.X + p.Width)
                .DefaultIfEmpty()
                .Max();

            current.Position.X += leftOffset;


            var rightOffset = previousModels
                .Where(p => p.Anchor == Panel.PanelAnchor.Right && current.Anchor != Panel.PanelAnchor.Left)
                .Select(p => AreaWidth - p.Position.X)
                .DefaultIfEmpty()
                .Max();

            if (!current.IsFixedWidth)
                current.Width -= rightOffset + leftOffset;


            var bottomOffset = previousModels
                .Where(p => p.Anchor == Panel.PanelAnchor.Bottom && current.Anchor != Panel.PanelAnchor.Top)
                .Select(p => p.Position.Y + p.Height)
                .DefaultIfEmpty()
                .Max();

            current.Position.Y += bottomOffset;


            var topOffset = previousModels
                .Where(p => p.Anchor == Panel.PanelAnchor.Top && current.Anchor != Panel.PanelAnchor.Bottom)
                .Select(p => AreaHeight - p.Position.Y)
                .DefaultIfEmpty()
                .Max();

            if (!current.IsFixedHeight)
                current.Height -= topOffset + bottomOffset;


        }

    }
}
