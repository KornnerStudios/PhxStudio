﻿using System;
using System.IO;
using KSoft;
using KSoft.IO;

namespace PhxStudio.Modules.Project
{
	class PhxStudioProjectViewModel
		: KSoft.ObjectModel.BasicViewModel
	{
		PhxStudioProject mModel = new PhxStudioProject();
		public PhxStudioProject Model
		{
			get { return mModel; }
			private set { SetField(ref mModel, value); }
		}

		internal Exception CreateNewInternal()
		{
			Exception caught_exception = null;

			var new_project_model = new PhxStudioProject();
			this.Model = new_project_model;

			return caught_exception;
		}

		internal Exception OpenInternal(string path)
		{
			Exception caught_exception = null;
			try
			{
				var opened_project_model = new PhxStudioProject();

				using (var s = new XmlElementStream(path, FileAccess.Read))
				{
					s.InitializeAtRootElement();
					opened_project_model.Serialize(s);
				}

				this.Model = opened_project_model;
			} catch (Exception ex)
			{
				caught_exception = ex;
			}
			return caught_exception;
		}

		internal Exception SaveInternal(string path = null)
		{
			Exception caught_exception = null;
			try
			{
				if (path == null)
					path = Model.ProjectFilePath;
				if (path.IsNullOrEmpty())
				{
					throw new InvalidOperationException(
						"Tried to save project with a null-or-empty path.");
				}

				using (var s = new XmlElementStream(path, FileAccess.Write))
				{
					s.InitializeAtRootElement();
					Model.Serialize(s);
				}
			} catch (Exception ex)
			{
				caught_exception = ex;
			}
			return caught_exception;
		}
	};
}