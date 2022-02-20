using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Microsoft.Extensions.Logging;
using Module_Constructor.Models;
using Module_Constructor.Services.Interfaces;

namespace Module_Constructor.Services
{
    public class Visualizer : I3DVisualizer
    {
        private readonly ILogger _Logger;
        private readonly IModuleBuilder _ModuleBuilder;


        public Visualizer(ILogger<Visualizer> Logger, IModuleBuilder ModuleBuilder)
        {
            _Logger = Logger;
            _ModuleBuilder = ModuleBuilder;
        }


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

            var texturedMaterial = MaterialHelper.CreateImageMaterial("texture.jpg");



            foreach (var panelModel in _ModuleBuilder.BuildPanels(Module))
            {
                var isSelected = SelectedPanel?.Equals(panelModel.Panel);

                var meshBuilder = new MeshBuilder(true, true);

                var locationPoint = new Point3D(panelModel.Position.Z, panelModel.Position.X, panelModel.Position.Y);
                var size = new Size3D(panelModel.Depth, panelModel.Width, panelModel.Height);

                var rect = new Rect3D(locationPoint, size);

                meshBuilder.AddBox(rect);

                // Create a mesh from the builder (and freeze it)
                var mesh = meshBuilder.ToMesh(true);


                var material = isSelected == true ? greenMaterial : texturedMaterial;
                // Add 3 models to the group (using the same mesh, that's why we had to freeze it)
                modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = material, BackMaterial = insideMaterial });
            }
            _Logger.LogInformation("Визуализация модуля {0} построена за {1} мс.", Module.Name, sw.ElapsedMilliseconds);

            return modelGroup;
        }

    }
}
