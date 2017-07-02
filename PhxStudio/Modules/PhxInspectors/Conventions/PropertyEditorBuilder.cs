using System;
using System.ComponentModel;

namespace PhxStudio.Modules.PhxInspectors.Conventions
{
	using Inspectors;

	public abstract class PropertyEditorBuilder
	{
		public abstract bool IsApplicable(PropertyDescriptor propertyDescriptor);
		public abstract IEditor BuildEditor(PropertyDescriptor propertyDescriptor);
	};

	public sealed class StandardPropertyEditorBuilder<T, TEditor>
		: PropertyEditorBuilder
		where TEditor : IEditor, new()
	{
		public override bool IsApplicable(PropertyDescriptor propertyDescriptor)
		{
			return propertyDescriptor.PropertyType == typeof(T);
		}

		public override IEditor BuildEditor(PropertyDescriptor propertyDescriptor)
		{
			return new TEditor();
		}
	};

	public sealed class EnumPropertyEditorBuilder
		: PropertyEditorBuilder
	{
		public override bool IsApplicable(PropertyDescriptor propertyDescriptor)
		{
			return typeof(Enum).IsAssignableFrom(propertyDescriptor.PropertyType);
		}

		public override IEditor BuildEditor(PropertyDescriptor propertyDescriptor)
		{
			return new EnumEditorViewModel(propertyDescriptor.PropertyType);
		}
	};
}
