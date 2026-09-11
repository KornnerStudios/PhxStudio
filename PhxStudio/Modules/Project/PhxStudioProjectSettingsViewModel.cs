using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Modules.Settings;
using KSoft;
using GameVersionType = KSoft.Phoenix.HaloWars.GameVersionType;

namespace PhxStudio.Modules.Project
{
	[Export(typeof(ISettingsEditor))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "Gemini settings editor activated by MEF composition.")]
	partial class PhxStudioProjectSettingsViewModel
		: PropertyChangedBase
		, ISettingsEditor
	{
		private IEventAggregator mEventAggregator;

		private PhxStudioProjectViewModel mProjectViewModel;

		#region ProjectName
		string mProjectName = string.Empty;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mProjectName),
			DependentProperties = new[] { nameof(ProjectNameIsValid) })]
		public partial string ProjectName { get; set; }

		public bool ProjectNameIsValid => ProjectName.IsNotNullOrEmpty();
		#endregion

		#region GameVersion
		GameVersionType mGameVersion = GameVersionType.DefinitiveEdition;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mGameVersion))]
		public partial GameVersionType GameVersion { get; set; }
		#endregion

		#region WorkDirectory
		string? mWorkDirectory;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mWorkDirectory))]
		public partial string? WorkDirectory { get; set; }
		#endregion

		#region FinalDirectory
		string? mFinalDirectory;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mFinalDirectory))]
		public partial string? FinalDirectory { get; set; }
		#endregion

		#region RevertSettingsCommand
		ICommand? mRevertSettingsCommand;
		public ICommand RevertSettingsCommand => mRevertSettingsCommand ??= new RelayCommand(_ => RevertSettings());
		#endregion

		#region SaveSettingsCommand
		ICommand? mSaveSettingsCommand;
		public ICommand SaveSettingsCommand => mSaveSettingsCommand ??= new RelayCommand(_ => SaveSettings());
		#endregion

		[ImportingConstructor]
		public PhxStudioProjectSettingsViewModel(IEventAggregator events)
		{
			mEventAggregator = events;
			mProjectViewModel = App.CurrentProjectViewModel;

			RevertSettings();
		}

		private void RevertSettings()
		{
			ProjectName = mProjectViewModel.Model.ProjectName;
			GameVersion = mProjectViewModel.Model.GameVersion;
			WorkDirectory = mProjectViewModel.Model.WorkDirectory;
			FinalDirectory = mProjectViewModel.Model.FinalDirectory;
		}

		private void SaveSettings()
		{
			bool workDirChanged =
				!string.Equals(mProjectViewModel.Model.WorkDirectory, WorkDirectory, StringComparison.OrdinalIgnoreCase);
			bool finalDirChanged =
				!string.Equals(mProjectViewModel.Model.FinalDirectory, FinalDirectory, StringComparison.OrdinalIgnoreCase);

			mProjectViewModel.Model.ProjectName = ProjectName;
			mProjectViewModel.Model.GameVersion = GameVersion;
			mProjectViewModel.Model.WorkDirectory = WorkDirectory;
			mProjectViewModel.Model.FinalDirectory = FinalDirectory;

			if (workDirChanged)
				mEventAggregator.PublishOnUIThreadAsync(new ProjectWorkDirectoryChangedEventArgs());
			if (finalDirChanged)
				mEventAggregator.PublishOnUIThreadAsync(new ProjectFinalDirectoryChangedEventArgs());
		}

		#region ISettingsEditor
		public string SettingsPagePath => Constants.SettingsPaths.Project;
		public string SettingsPageName => Constants.SettingsPages.Project_Info;

		public void ApplyChanges()
		{
			SaveSettings();
		}
		#endregion
	}
}
