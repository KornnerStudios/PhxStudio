using System.Windows.Media;
using Gemini.Framework.Services;

namespace PhxStudio.UI.ViewModels.FileTreeView
{
	using IEditorProviderSelector = Modules.Main.IEditorProviderSelector;

	// #TODO need a service that allows configuring how file info is gathered by items
	// * How to look for file icons

	public sealed class FileItemViewModel
		: TreeViewItemBase
	{
		ImageSource mIcon;
		public ImageSource Icon
		{
			get { return mIcon; }
			set { this.SetFieldRef(ref mIcon, value); }
		}

		public IEditorProvider EditorProvider { get; private set; }
		public bool IsEditorAvailable { get { return EditorProvider != null; } }

		public FileItemViewModel(IEditorProviderSelector editorProviderSelector, string filePath)
		{
			SetPathToFile(filePath);

			if (editorProviderSelector != null)
				EditorProvider = editorProviderSelector.GetEditor(FilePath, FileName, FileExtension);
		}

		public void LookUpIconFromOperatingSystem()
		{
			if (Icon != null)
				return;

			Icon = Utils.FileIconsLoader.GetSmallIcon(FilePath);
		}
	};
}
