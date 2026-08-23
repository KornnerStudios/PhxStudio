using System.Windows;
using System.Windows.Controls;

namespace PhxStudio.Modules.PhxInspectors.Controls
{
	using Inspectors;

	public class PhxInspectorItemTemplateSelector
		: DataTemplateSelector
	{
		public DataTemplate LabelledTemplate { get; set; } = null!;
		public DataTemplate DefaultTemplate { get; set; } = null!;

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item is ILabelledInspector)
				return LabelledTemplate;

			return DefaultTemplate;
		}
	};
}
