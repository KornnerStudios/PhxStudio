using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace PhxStudio.Modules.Project
{
	[Export(typeof(IProjectService))]
	class ProjectService
		: PropertyChangedBase
		, IProjectService
	{
		public PhxStudioProjectViewModel CurrentProject
		{
			get { return App.CurrentProjectViewModel; }
		}

		string mCurrentProjectFilePath;
		public string CurrentProjectFilePath
		{
			get { return mCurrentProjectFilePath; }
			set
			{
				if (this.SetFieldObj(ref mCurrentProjectFilePath, value))
				{
					if (mCurrentProjectFilePath != null && CurrentProject != null)
					{
						CurrentProject.Model.ProjectFilePath = mCurrentProjectFilePath;
					}
				}
			}
		}

		public Exception CreateNew()
		{
			var operation_exception = CurrentProject.CreateNewInternal();
			if (operation_exception != null)
				return operation_exception;

			CurrentProjectFilePath = CurrentProject.Model.ProjectFilePath;

			return operation_exception;
		}

		public Exception Open(string path)
		{
			var operation_exception = CurrentProject.OpenInternal(path);
			if (operation_exception != null)
				return operation_exception;

			CurrentProjectFilePath = CurrentProject.Model.ProjectFilePath;

			return operation_exception;
		}

		public Exception Save(string path)
		{
			var operation_exception = CurrentProject.SaveInternal(path);
			if (operation_exception != null)
				return operation_exception;

			CurrentProjectFilePath = CurrentProject.Model.ProjectFilePath;

			return operation_exception;
		}
	};
}
