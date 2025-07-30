using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace SC.AgentDeployer.Client.ValueConverters;

public class InvertBooleanValueConverter : MarkupExtension, IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return !(bool)value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return !(bool)value;
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}
}
