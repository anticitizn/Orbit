using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Orbit
{
    public class ScreenPosConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // [0] - pos | [1] - camera offset | [2] - scale
            float entPos = (float)values[0];
            float camPos = (float)values[1];
            float scale = (float)values[2];

            double result = (entPos / scale) + camPos;
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // not needed
            throw new NotImplementedException();
        }
    }

    public class ScreenDiameterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // [0] - mass | [1] - scale
            ulong mass = (ulong)values[0];
            float scale = (float)values[1];

            double diam = Math.Sqrt(mass) / scale / 50 + 2;
            return diam;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // not needed
            throw new NotImplementedException();
        }
    }

    public class PointCollectionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // [0] - pos | [1] - camera offset | [2] - scale
            PointCollection points = (PointCollection)values[0];
            PointCollection screenPosPoints = new PointCollection();
            float camPosX = (float)values[1];
            float camPosY = (float)values[2];
            float scale = (float)values[3];

            for (int i = 0; i < points.Count; i++)
            {
                screenPosPoints.Add(new Point((points[i].X / scale) + camPosX, (points[i].Y / scale) + camPosY));
                //Console.WriteLine(points[i].X.ToString() + ' ' + camPosX.ToString() + ' ' + scale.ToString() + ' ' + (points[i].X / scale).ToString());
                //Console.WriteLine(screenPosPoints[i]);
            }

            return screenPosPoints;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // not needed
            throw new NotImplementedException();
        }
    }
}
