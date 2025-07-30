using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace SC.AgentDeployer.Client.ValueConverters;

public class TruncateStringValueConverter : MarkupExtension, IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			return string.Empty;
		}
		if (value is string text)
		{
			int num = 100;
			if (parameter != null && int.TryParse(parameter.ToString(), out var result))
			{
				num = result;
			}
			if (text.Length > num)
			{
				return text.Substring(0, num) + "...";
			}
			return text;
		}
		return string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}
}
