using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using BO;

namespace PL
{
    public sealed class NullToEnableConverter : IValueConverter
    {
        /// <summary>
        /// checks if sender is null 
        /// </summary>
        /// <returns> bool </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class TextToEnableConverter : IValueConverter
    {
        /// <summary>
        /// checks if text in textBox is empty
        /// </summary>
        /// <returns> bool </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
    public sealed class TextToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// checks if text in textBox is empty
        /// </summary>
        /// <returns> Visibility </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class LattitudeToDmsConverter : IValueConverter
    {
        /// <summary>
        /// converts lattitude coordinate from double type to string of DMS form of the coordinate 
        /// </summary>
        /// <returns> string </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Double val = (Double)value;
                int tmp = ((int)val);
                int tmp2 = (int)((val % (int)val) * 60);
                double tmp3 = (((val % (int)val) * 60) % tmp2) * 60;
                string result = $"{ tmp}{(char)176} {tmp2}\" " + String.Format("{0:0.000}", tmp3) + "' ";
                if (tmp > 0)
                    return result + "E";
                else
                    return result + "W";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class LongtitudeToDmsConverter : IValueConverter
    {
        /// <summary>
        /// converts longtitude coordinate from double type to string of DMS form of the coordinate 
        /// </summary>
        /// <returns> string </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Double val = (Double)value;
                int tmp = ((int)val);
                int tmp2 = (int)((val % (int)val) * 60);
                double tmp3 = (((val % (int)val) * 60) % tmp2) * 60;
                string result = $"{ tmp}{(char)176} {tmp2}\" " + String.Format("{0:0.000}", tmp3) + "' ";
                if (tmp > 0)
                    return result + "N";
                else
                    return result + "S";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class NullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// checks if sender is null 
        /// </summary>
        /// <returns> Visibility </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class OrderedToVisibilitiyReveresedConverter : IValueConverter
    {
        /// <summary>
        /// checks if sender is null, visibility value returned in reverse , visible if null  
        /// </summary>
        /// <returns> Visibility </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return ((Parcel)value).Linked == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class AvailableToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// checks if drone status is Available
        /// </summary>
        /// <returns> Visibility </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return (DroneStatus)value == DroneStatus.Available ? Visibility.Visible : Visibility.Collapsed;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class MaintanenceToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// checks if drone status is Maintenance
        /// </summary>
        /// <returns> Visibility </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return (DroneStatus)value == DroneStatus.Maintenance ? Visibility.Visible : Visibility.Collapsed;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class PickUpToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// checks if drone status is Delivery and parcel wasn't picked up yet
        /// </summary>
        /// <returns> Visibility </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Drone temp = value as Drone;
            if (temp.Status == DroneStatus.Delivery && !temp.Parcel.InTransit)
                return Visibility.Visible;
            return Visibility.Collapsed;
            //return (DroneStatus)value == DroneStatus.Delivery ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class DeliveryToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// checks if drone status is Delivery and parcel has been  picked up already
        /// </summary>
        /// <returns> Visibility </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Drone temp = (Drone)value;
            if (temp.Status == DroneStatus.Delivery && temp.Parcel.InTransit)
                return Visibility.Visible;
            return Visibility.Collapsed;
            //return (DroneStatus)value == DroneStatus.Delivery ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class StatefDeliveryToStringx : IValueConverter
    {
        /// <summary>
        /// checks if drone status is Delivery and parcel has been  picked up already
        /// </summary>
        /// <returns> Visibility </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ParcelInDelivery temp = value as ParcelInDelivery;
            if (temp != null)
            {
                switch (temp.InTransit)
                {
                    case false:
                        { return "Distance To Sender:"; }
                    case true:
                        { return "Distance To Target:"; }
                }
                //return (DroneStatus)value == DroneStatus.Delivery ? Visibility.Visible : Visibility.Collapsed;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    internal class batteryToBackroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int  val = int.Parse(value.ToString());

                return val switch
                {
                    < 16 => Brushes.Red,
                    < 70 => Brushes.Yellow,
                    _ => Brushes.Green
                };
            };
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}

