using System;
using System.IO;
using Caliburn.Micro;

namespace PhxStudio.UI.ViewModels.FileTreeView
{
	public abstract partial class TreeViewItemBase
		: PropertyChangedBase
		, ITreeViewItem
	{
		string mFilePath = string.Empty;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mFilePath),
			AlwaysNotify = true)]
		public partial string FilePath { get; private set; }

		string mFileName = string.Empty;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mFileName),
			AlwaysNotify = true)]
		public partial string FileName { get; private set; }

		string? mFileExtension;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mFileExtension),
			AlwaysNotify = true)]
		public partial string? FileExtension { get; private set; }

		public string FileNameAndExtension { get; private set; } = string.Empty;

		object? mUserData;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mUserData))]
		public partial object? UserData { get; set; }

		protected void SetPathToFile(string filePath)
		{
			if (string.Equals(mFilePath, filePath, StringComparison.OrdinalIgnoreCase))
				return;

			FilePath = filePath;
			FileName = Path.GetFileNameWithoutExtension(filePath);
			FileExtension = Path.GetExtension(filePath) ?? "";
			FileNameAndExtension = FileName + FileExtension;
		}

		protected void SetPathToDirectory(string directoryPath)
		{
			if (string.Equals(mFilePath, directoryPath, StringComparison.OrdinalIgnoreCase))
				return;

			FilePath = directoryPath;
			FileName = Path.GetFileNameWithoutExtension(directoryPath);
			FileExtension = null;
			FileNameAndExtension = FileName;
		}
	};
}