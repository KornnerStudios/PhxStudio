using Caliburn.Micro;

namespace PhxStudio.Modules.PhxInspectors.Inspectors
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Marker interface selects labelled inspector templates.")]
	public interface ILabelledInspector
	{
	};

	public interface IInspector
	{
		string Name { get; }
		bool IsReadOnly { get; }
	};

	public abstract class InspectorBase
		: PropertyChangedBase
		, IInspector
	{
		public abstract string Name { get; }
		public abstract bool IsReadOnly { get; }
	};
}
