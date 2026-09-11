using System.Windows.Media;

namespace PhxStudio.Modules.PhxInspectors.Inspectors
{
	// #TODO Gemini.Modules.Inspector.Inspectors.ColorEditorView exists in 2025, did it not when I first wrote this code?
	public sealed partial class ColorEditorViewModel
		: SelectiveUndoEditorBase<Color>
		, ILabelledInspector
	{
		private bool mUsingAlphaChannel = true;

		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mUsingAlphaChannel))]
		public partial bool UsingAlphaChannel { get; set; }

		public void Opened()
		{
			OnBeginEdit();
		}

		public void Closed()
		{
			OnEndEdit();
		}
	}
}
