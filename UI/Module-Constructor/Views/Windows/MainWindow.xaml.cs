using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Module_Constructor.ViewModels.Windows;

namespace Module_Constructor.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _Vm;
        public MainWindow()
        {
            InitializeComponent();
            _Vm = App.Services.GetRequiredService<MainWindowViewModel>();
        }

        private void UIElement_OnMouseLeftButtonDown(object Sender, MouseButtonEventArgs E)
        {

            //var viewport = (HelixViewport3D)Sender;
            //var firstHit = viewport.Viewport.FindHits(E.GetPosition(viewport)).FirstOrDefault();
            //if (firstHit != null)
            //{
            //    ListBox.SelectedItem = firstHit.Visual;
            //}
            //else
            //{
            //    ListBox.SelectedItem = null;
            //}

        }

        private void ResetCamera_Click(object Sender, RoutedEventArgs E)
        {
            // Position="600,-400,0" LookDirection="-688,438,-300" UpDirection="-0.3,0.2,0.9"
            //Camera.AnimateTo(new Point3D(600,-400,0), new Vector3D(-688,438,-300), new Vector3D(-0.3,0.2,0.9), 500 );
            //Viewport3D.ZoomExtents(500);
            Viewport3D.FitView(new Vector3D(-2, 1, -1), new Vector3D(0, 0, 1), 500);
        }
    }
}
