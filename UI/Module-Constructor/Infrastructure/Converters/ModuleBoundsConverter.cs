using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using Module_Constructor.Models;

namespace Module_Constructor.Infrastructure.Converters
{
    [ValueConversion(typeof(Module), typeof(Rect3D))]
    internal class ModuleBoundsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Module module ? new Rect3D(0,0,0,module.Depth, module.Width, module.Height) : Rect3D.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
