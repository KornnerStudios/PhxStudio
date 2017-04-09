using System;
using System.Collections.Generic;
using Caliburn.Micro;

namespace PhxStudio.Modules.Project
{
	interface IProjectService
		: INotifyPropertyChangedEx
	{
		PhxStudioProjectViewModel CurrentProject { get; }
		string CurrentProjectFilePath { get; }

		Exception CreateNew();
		Exception Open(string path);
		Exception Save(string path = null);
	};
}
