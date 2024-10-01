using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace OnDijon.Common.ValueConverters
{
	public class NotEmptyStringToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string strValue)
				return !string.IsNullOrEmpty(strValue);

			return false;

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}