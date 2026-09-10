using KSoft;
using Caliburn.Micro;
using EditorFileType = Gemini.Framework.Services.EditorFileType;
using GameVersionType = KSoft.Phoenix.HaloWars.GameVersionType;

namespace PhxStudio.Modules.Project
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "WPF project binding model.")]
	partial class PhxStudioProject
		: KSoft.ObjectModel.BasicViewModel
		, KSoft.IO.ITagElementStringNameStreamable
	{
		public const string XmlRootName = "Project";

		#region FileType
		public static string FileExtension => ".phxproj";
		private static EditorFileType? gFileType;
		public static EditorFileType FileType => gFileType ??= new EditorFileType("PhxStudio Project", FileExtension);
		#endregion

		#region ProjectFilePath
		string? mProjectFilePath;
		/// <summary>Not serialized, just for remembering where a project was loaded and should be saved to</summary>
		public string? ProjectFilePath
		{
			get { return mProjectFilePath; }
			set
			{
				if (!SetField(ref mProjectFilePath, value))
					return;

				OnPropertyChanged(nameof(IsOnDisk));
			}
		}

		public bool IsOnDisk
		{
			get { return ProjectFilePath.IsNotNullOrEmpty(); }
			set
			{
				bool dummy = false;
				SetFieldVal(ref dummy, value, overrideChecks: true);
			}
		}
		#endregion

		#region ProjectName
		const string kDefaultProjectName = "HaloWars Mod";

		string mProjectName = kDefaultProjectName;
		public string ProjectName
		{
			get { return mProjectName; }
			set
			{
				if (this.SetFieldObj(ref mProjectName, value))
				{
					this.OnPropertyChanged(nameof(ProjectNameIsValid));
				}
			}
		}

		public bool ProjectNameIsValid => ProjectName.IsNotNullOrEmpty();
		#endregion

		#region GameVersion
		GameVersionType mGameVersion = GameVersionType.DefinitiveEdition;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mGameVersion))]
		public partial GameVersionType GameVersion { get; set; }
		#endregion

		#region WorkDirectory
		string? mWorkDirectory;
		public string? WorkDirectory
		{
			get { return mWorkDirectory; }
			set
			{
				if (this.SetField(ref mWorkDirectory, value))
				{
					CreateOrUnloadEngine();
				}
			}
		}
		#endregion

		#region FinalDirectory
		string? mFinalDirectory;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mFinalDirectory))]
		public partial string? FinalDirectory { get; set; }
		#endregion

		#region Engine
		KSoft.Phoenix.Engine.PhxEngine? mEngine;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mEngine))]
		public partial KSoft.Phoenix.Engine.PhxEngine? Engine { get; private set; }
		#endregion

		bool mEngineCreationDisabled;
		private void CreateOrUnloadEngine()
		{
			if (mEngineCreationDisabled)
				return;

			// don't do anything when the engine was never loaded to begin with
			if (WorkDirectory.IsNullOrEmpty() && Engine == null)
				return;

			var eventAggregator = IoC.Get<IEventAggregator>();

			bool unload = Engine != null;
			bool reload = unload && WorkDirectory.IsNotNullOrEmpty();
			bool load = !unload && WorkDirectory.IsNotNullOrEmpty();

			if (unload)
			{
				Engine = null;
				eventAggregator.PublishOnUIThreadAsync(new ProjectEngineUnloadedEventArgs());
			}

			if (reload || load)
			{
				var workDirectory = WorkDirectory;
				if (workDirectory is null)
					return;

				var engine = KSoft.Phoenix.Engine.PhxEngine.CreateForHaloWars(
					workDirectory, workDirectory,
					GameVersion == GameVersionType.Xbox360);

				Engine = engine;
				eventAggregator.PublishOnUIThreadAsync(new ProjectEngineCreatedEventArgs(Engine));
			}
		}

		public void Serialize<TDoc, TCursor>(KSoft.IO.TagElementStream<TDoc, TCursor, string> s)
			where TDoc : class
			where TCursor : class
		{
			mEngineCreationDisabled = true;

			s.StreamElementOpt("ProjectName", this, obj => obj.ProjectName, x => x != kDefaultProjectName);
			s.StreamElementEnumOpt("GameVersion", this, obj => obj.GameVersion, x => x != GameVersionType.DefinitiveEdition);
			s.StreamElementOpt("WorkDir", this, obj => obj.WorkDirectory, Predicates.IsNotNullOrEmpty);
			s.StreamElementOpt("FinalDir", this, obj => obj.FinalDirectory, Predicates.IsNotNullOrEmpty);

			mEngineCreationDisabled = false;

			if (s.IsReading)
			{
				CreateOrUnloadEngine();
			}
		}
	};
}
