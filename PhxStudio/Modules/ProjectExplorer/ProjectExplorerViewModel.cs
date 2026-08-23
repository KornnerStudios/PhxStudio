using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;

namespace PhxStudio.Modules.ProjectExplorer
{
	using Modules.Project;
	using UI.ViewModels.FileTreeView;

	[Export(typeof(ProjectExplorerViewModel))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	class ProjectExplorerViewModel
		: Tool
		, IHandle<ProjectOpeningEventArgs>
		, IHandle<ProjectClosingEventArgs>
		, IHandle<ProjectWorkDirectoryChangedEventArgs>
	{
		#region Imports
#pragma warning disable 649

		[Import] IShell mShell = null!;
		[Import] IEventAggregator mEventAggregator;
		[Import] Main.IEditorProviderSelector mEditorProviderSelector = null!;

#pragma warning restore 649
		#endregion

		FolderItemViewModel? mRoot;
		public FolderItemViewModel? Root
		{
			get { return mRoot; }
			private set
			{
				mRoot = value;
				if (this.SetField(ref mRoot, value, overrideChecks: true))
				{
					// trigger INPC
					Items = Items;
				}
			}
		}

		public ObservableCollection<ITreeViewItem>? Items
		{
			get { return mRoot?.Children; }
			set { this.SetPropertyChanged(); }
		}

		public bool ShowAllFiles { get; set; }

		[ImportingConstructor]
		public ProjectExplorerViewModel(IEventAggregator eventAggregator)
		{
			mEventAggregator = eventAggregator;
			mEventAggregator.SubscribeOnPublishedThread(this);

			DisplayName = "Project Explorer";

			//this.ToolBarDefinition = ProjectExplorer.ToolBarDefenitions.ProjectExplorerToolBar;
		}

		public override PaneLocation PreferredLocation => PaneLocation.Left;

#if false // Activate and Deactivate aren't called on Tool :|
		protected override void OnActivate()
		{
			base.OnActivate();

			mEventAggregator.SubscribeOnPublishedThread(this);
		}

		protected override void OnDeactivate(bool close)
		{
			base.OnDeactivate(close);

			mEventAggregator.Unsubscribe(this);
		}
#endif

		public void UpdateTree()
		{
			var root = Root;
			if (root is null)
				return;

			root.Refresh();
			// trigger INPC
			Root = root;
		}

		public void Open(string? directory)
		{
			if (string.IsNullOrEmpty(directory))
				return;

			if (!System.IO.Directory.Exists(directory))
				return;

			Root = new FolderItemViewModel(mEditorProviderSelector, directory);
			UpdateTree();
		}

		private void Close()
		{
			Root = null;
			UpdateTree();
		}

		public void OnMouseDown(object source, FileItemViewModel fileItem, MouseButtonEventArgs args)
		{
			if (args.LeftButton == MouseButtonState.Pressed && args.ClickCount == 2)
			{
				OpenAsync(fileItem);
			}
		}

		private async void OpenAsync(FileItemViewModel file)
		{
			// #TODO_PHXSTUDIO need to figure out how to support files that require an external viewer or such tool (eg, ddx until we support in-editor viewing)

			if (file.EditorProvider is not { } editor)
			{
				Debug.Trace.PhxStudio.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0,
					"Can't find editor for file",
					file.FilePath);
			}
			else
			{
				var vm = editor.Create();

				Debug.Trace.PhxStudio.TraceInformation("Opening file with {0}: {1}",
					editor, file.FilePath);

				await editor.Open(vm, file.FilePath);
				await mShell.OpenDocumentAsync(vm);
			}
		}

		private void OpenCurrentProjectWorkDir()
		{
			string? work_dir = App.CurrentProjectViewModel.Model.WorkDirectory;
			Open(work_dir);
		}

		Task IHandle<ProjectOpeningEventArgs>.HandleAsync(ProjectOpeningEventArgs message, CancellationToken cancellationToken)
			=> Task.Run(OpenCurrentProjectWorkDir, cancellationToken);

		Task IHandle<ProjectClosingEventArgs>.HandleAsync(ProjectClosingEventArgs message, CancellationToken cancellationToken)
			=> /*Close()*/TryCloseAsync();

		Task IHandle<ProjectWorkDirectoryChangedEventArgs>.HandleAsync(ProjectWorkDirectoryChangedEventArgs message, CancellationToken cancellationToken)
			=> Task.Run(OpenCurrentProjectWorkDir, cancellationToken);
	};
}
