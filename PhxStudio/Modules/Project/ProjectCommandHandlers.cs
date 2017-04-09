using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Gemini.Framework.Commands;
using Gemini.Framework.Threading;
using Microsoft.Win32;

namespace PhxStudio.Modules.Project.Commands
{
	[CommandHandler]
	class FileNewProjectCommandHandler
		: CommandHandlerBase<FileNewProjectCommandDefinition>
	{
#pragma warning disable 649
		[Import] IEventAggregator mEventAggregator;
		[Import] IProjectService mProjectService;
#pragma warning restore 649

		[Export]
		public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<FileNewProjectCommandDefinition>(
			new KeyGesture(Key.N, ModifierKeys.Control | ModifierKeys.Shift));

		public override async Task Run(Command command)
		{
			mEventAggregator.PublishOnUIThread(new ProjectClosingEventArgs());

			var project_task = Task.Factory.StartNew(mProjectService.CreateNew,
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.Default);
			var project_task_result = await project_task;
			if (project_task_result != null)
				return;

			mEventAggregator.PublishOnUIThread(new ProjectOpeningEventArgs());
		}
	};

	[CommandHandler]
	class FileOpenProjectCommandHandler
		: CommandHandlerBase<FileOpenProjectCommandDefinition>
	{
#pragma warning disable 649
		[Import] IEventAggregator mEventAggregator;
		[Import] IProjectService mProjectService;
#pragma warning restore 649

		[Export]
		public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<FileOpenProjectCommandDefinition>(
			new KeyGesture(Key.O, ModifierKeys.Control | ModifierKeys.Shift));

		public override async Task Run(Command command)
		{
			var dialog = new OpenFileDialog();
			dialog.SetupViaEditorFileType(PhxStudioProject.FileType);
			if (dialog.ShowDialog() != true)
				return;

			mEventAggregator.PublishOnUIThread(new ProjectClosingEventArgs());

			var project_task = Task.Factory.StartNew(OpenProjectCallback, dialog.FileName,
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.Default);
			var project_task_result = await project_task;
			if (project_task_result != null)
				return;

			mEventAggregator.PublishOnUIThread(new ProjectOpeningEventArgs());
		}

		private Exception OpenProjectCallback(object state)
		{
			var file_name = (string)state;
			return mProjectService.Open(file_name);
		}
	};

	[CommandHandler]
	class FileSaveProjectCommandHandler
		: CommandHandlerBase<FileSaveProjectCommandDefinition>
	{
#pragma warning disable 649
		[Import] IEventAggregator mEventAggregator;
		[Import] IProjectService mProjectService;
#pragma warning restore 649

		[ImportingConstructor]
		public FileSaveProjectCommandHandler(IEventAggregator eventAggregator
			, IProjectService service)
		{
			mEventAggregator = eventAggregator;
			mProjectService = service;
		}

		public override async Task Run(Command command)
		{
			var project_task = Task.Factory.StartNew(SaveProjectCallback, mProjectService,
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.Default);
			var project_task_result = await project_task;
			if (project_task_result != null)
				return;
		}

		private static Exception SaveProjectCallback(object state)
		{
			var service = (IProjectService)state;
			return service.Save();
		}

		public override void Update(Command command)
		{
			base.Update(command);

			command.Enabled = mProjectService.CurrentProject.Model.IsOnDisk;
		}
	};

	[CommandHandler]
	class FileSaveProjectAsCommandHandler
		: CommandHandlerBase<FileSaveProjectAsCommandDefinition>
	{
#pragma warning disable 649
		[Import] IEventAggregator mEventAggregator;
		[Import] IProjectService mProjectService;
#pragma warning restore 649

		[ImportingConstructor]
		public FileSaveProjectAsCommandHandler(IEventAggregator eventAggregator
			, IProjectService service)
		{
			mEventAggregator = eventAggregator;
			mProjectService = service;
		}

		public override async Task Run(Command command)
		{
			var dialog = new SaveFileDialog();
			dialog.SetupViaEditorFileType(PhxStudioProject.FileType);
			dialog.FileName = mProjectService.CurrentProjectFilePath ?? "";
			if (dialog.ShowDialog() != true)
				return;

			var project_task = Task.Factory.StartNew(SaveProjectAsCallback, dialog.FileName,
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.Default);
			var project_task_result = await project_task;
			if (project_task_result != null)
				return;
		}

		private Exception SaveProjectAsCallback(object state)
		{
			var file_name = (string)state;
			return mProjectService.Open(file_name);
		}
	};
}
