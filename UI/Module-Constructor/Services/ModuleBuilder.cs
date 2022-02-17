using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Module_Constructor.Models;
using Module_Constructor.Services.Interfaces;
using Module_Constructor.Visualisation;

namespace Module_Constructor.Services
{
    public class ModuleBuilder : IModuleBuilder
    {
        private readonly ILogger<ModuleBuilder> _Logger;

        public ModuleBuilder(ILogger<ModuleBuilder> Logger)
        {
            _Logger = Logger;
        }

        public IEnumerable<PanelViewModel> BuildPanels(Module module)
        {
            var panels = new List<PanelViewModel>();

            foreach (var modulePart in module.Panels.OrderBy(p => p.Order))
            {

                var panelModel = BuildViewModel(modulePart, module.Width, module.Height, module.Depth);

                ExcludeCollisions(panelModel, panels, module.Width, module.Height, module.Depth);

                if (panelModel.HasErrors)
                {
                    _Logger.LogWarning("Ошибка построения детали {0}", panelModel.Panel.Name);
                    continue;
                }

                panels.Add(panelModel);

                yield return panelModel;
            }
        }

        public PanelViewModel BuildPanel(Module module, Panel panel) => 
            BuildPanels(module)
                .FirstOrDefault(p => p.Panel.Equals(panel));


        public void AddPanel(Module module, Panel panel)
        {
            var order = module.Panels.Count + 1;
            panel.Order = order;
            module.Panels.Add(panel);
        }

        public void RemovePanel(Module module, Panel panel)
        {
            if (!module.Panels.Remove(panel)) return;

            // Упорядочить номера деталей
            var index = 1;
            foreach (var p in module.Panels.OrderBy(p => p.Order)) 
                p.Order = index++;

        }

        public void SetPanelOrder(Module module, Panel panel, int order)
        {
            if (order < 1 || order > module.Panels.Count)
                throw new ArgumentOutOfRangeException(nameof(order),
                    "Номер детали должен быть не менее 1 и не более общего количества деталей");

            // Упорядочить номера деталей
            var index = order + 1;
            foreach (var p in module.Panels
                         .Where(p => p.Order > order && !p.Equals(panel))
                         .OrderBy(p => p.Order)) 
                p.Order = index++;

            panel.Order = order;
        }

        #region Private Methods

        // Построить модель из детали
        private PanelViewModel BuildViewModel(Panel panel, int AreaWidth, int AreaHeight, int AreaDepth)
        {
            var viewModel = new PanelViewModel()
            {
                Panel = panel
            };

            // Установить размеры и положение в зависимости от ориентации
            switch (panel.Orientation)
            {
                case Panel.PanelOrientation.Horizontal:
                    viewModel.Width = panel.FixedLenght ?? GetSize(AreaWidth, panel.LeftMargin, panel.RightMargin);
                    viewModel.Height = panel.Thickness;
                    viewModel.Depth = panel.FixedWidth ?? GetSize(AreaDepth, panel.FrontMargin, panel.BackMargin);

                    viewModel.IsFixedWidth = panel.FixedLenght != null;
                    viewModel.IsFixedHeight = true;
                    viewModel.IsFixedDepth = panel.FixedWidth != null;

                    break;
                case Panel.PanelOrientation.Vertical:
                    viewModel.Width = panel.Thickness;
                    viewModel.Height = panel.FixedLenght ?? GetSize(AreaHeight, panel.BottomMargin, panel.TopMargin);
                    viewModel.Depth = panel.FixedWidth ?? GetSize(AreaDepth, panel.FrontMargin, panel.BackMargin);

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


        // Получить размер детали
        private int GetSize(int Bound, int? FirstMargin, int? SecondMargin)
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


        // Получить смещение детали отностиельно начала координат
        private int GetOffset(int Bound, int DetalSize, int? FirstMargin, int? SecondMargin)
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
                .Where(p => p.Panel.Anchor == Panel.PanelAnchor.Left && current.Panel.Anchor != Panel.PanelAnchor.Right)
                .Where(p => CheckCollision(current.Position.Y, current.Height, p.Position.Y, p.Height)
                            && CheckCollision(current.Position.Z, current.Depth, p.Position.Z, p.Depth))
                .Select(p => p.Position.X + p.Width)
                .DefaultIfEmpty()
                .Max();


            var rightOffset = previousModels
                .Where(p => p.Panel.Anchor == Panel.PanelAnchor.Right && current.Panel.Anchor != Panel.PanelAnchor.Left)
                .Where(p => CheckCollision(current.Position.Y, current.Height, p.Position.Y, p.Height)
                            && CheckCollision(current.Position.Z, current.Depth, p.Position.Z, p.Depth))
                .Select(p => AreaWidth - p.Position.X)
                .DefaultIfEmpty()
                .Max();

            if (current.IsFixedWidth)
            {
                if (current.Panel.LeftMargin is { }) current.Position.X += leftOffset;
                if (current.Panel.RightMargin is { }) current.Position.X -= rightOffset;
            }
            else
            {
                current.Position.X += leftOffset;
                current.Width -= rightOffset + leftOffset;
            }


            var bottomOffset = previousModels
                .Where(p => p.Panel.Anchor == Panel.PanelAnchor.Bottom && current.Panel.Anchor != Panel.PanelAnchor.Top)
                .Where(p => CheckCollision(current.Position.X, current.Width, p.Position.X, p.Width)
                            && CheckCollision(current.Position.Z, current.Depth, p.Position.Z, p.Depth))
                .Select(p => p.Position.Y + p.Height)
                .DefaultIfEmpty()
                .Max();



            var topOffset = previousModels
                .Where(p => p.Panel.Anchor == Panel.PanelAnchor.Top && current.Panel.Anchor != Panel.PanelAnchor.Bottom)
                .Where(p => CheckCollision(current.Position.X, current.Width, p.Position.X, p.Width)
                            && CheckCollision(current.Position.Z, current.Depth, p.Position.Z, p.Depth))
                .Select(p => AreaHeight - p.Position.Y)
                .DefaultIfEmpty()
                .Max();

            if (current.IsFixedHeight)
            {
                if (current.Panel.BottomMargin is { }) current.Position.Y += bottomOffset;
                if (current.Panel.TopMargin is { }) current.Position.Y -= topOffset;
            }
            else
            {
                current.Position.Y += bottomOffset;
                current.Height -= topOffset + bottomOffset;
            }

            var backOffset = previousModels
                .Where(p => p.Panel.Anchor == Panel.PanelAnchor.Back && current.Panel.Anchor != Panel.PanelAnchor.Front)
                .Where(p => CheckCollision(current.Position.X, current.Width, p.Position.X, p.Width)
                            && CheckCollision(current.Position.Y, current.Height, p.Position.Y, p.Height))
                .Select(p => p.Position.Z + p.Depth)
                .DefaultIfEmpty()
                .Max();



            var frontOffset = previousModels
                .Where(p => p.Panel.Anchor == Panel.PanelAnchor.Front && current.Panel.Anchor != Panel.PanelAnchor.Back)
                .Where(p => CheckCollision(current.Position.X, current.Width, p.Position.X, p.Width)
                            && CheckCollision(current.Position.Y, current.Height, p.Position.Y, p.Height))
                .Select(p => AreaDepth - p.Position.Z)
                .DefaultIfEmpty()
                .Max();

            if (current.IsFixedDepth)
            {
                if (current.Panel.BackMargin is { }) current.Position.Z += backOffset;
                if (current.Panel.FrontMargin is { }) current.Position.Z -= frontOffset;
            }
            else
            {
                current.Position.Z += backOffset;
                current.Depth -= frontOffset + backOffset;
            }

        }


        // Определить пересечения по отрезкам
        private bool CheckCollision(int point1, int lenght1, int point2, int lenght2)
        {
            var finish1 = point1 + lenght1;
            var finish2 = point2 + lenght2;

            return finish2 > point1 && point2 < finish1;
        }

        #endregion

    }
}
