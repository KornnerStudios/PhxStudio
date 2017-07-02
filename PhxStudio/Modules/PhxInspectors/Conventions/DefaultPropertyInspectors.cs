using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace PhxStudio.Modules.PhxInspectors.Conventions
{
	using Inspectors;

	public static class DefaultPropertyInspectors
	{
		private static readonly List<PropertyEditorBuilder> gInspectorBuilders;

		public static List<PropertyEditorBuilder> InspectorBuilders
		{
			get { return gInspectorBuilders; }
		}

		static DefaultPropertyInspectors()
		{
			gInspectorBuilders = new List<PropertyEditorBuilder>
			{
				new RangePropertyEditorBuilder(),
				new EnumPropertyEditorBuilder(),

				new StandardPropertyEditorBuilder<bool, CheckBoxEditorViewModel>(),
				new StandardPropertyEditorBuilder<Color, ColorEditorViewModel>(),
				new StandardPropertyEditorBuilder<double, TextBoxEditorViewModel<double>>(),
				new StandardPropertyEditorBuilder<float, TextBoxEditorViewModel<float>>(),
				new StandardPropertyEditorBuilder<int, TextBoxEditorViewModel<int>>(),
				new StandardPropertyEditorBuilder<double?, TextBoxEditorViewModel<double?>>(),
				new StandardPropertyEditorBuilder<float?, TextBoxEditorViewModel<float?>>(),
				new StandardPropertyEditorBuilder<int?, TextBoxEditorViewModel<int?>>(),
				new StandardPropertyEditorBuilder<Point3D, Point3DEditorViewModel>(),
				new StandardPropertyEditorBuilder<string, TextBoxEditorViewModel<string>>(),

				new StandardPropertyEditorBuilder<BitmapSource, BitmapSourceEditorViewModel>()
			};
		}

		public static IEditor CreateEditor(PropertyDescriptor propertyDescriptor)
		{
			foreach (var inspectorBuilder in gInspectorBuilders)
			{
				if (inspectorBuilder.IsApplicable(propertyDescriptor))
					return inspectorBuilder.BuildEditor(propertyDescriptor);
			}
			return null;
		}
	};
}
