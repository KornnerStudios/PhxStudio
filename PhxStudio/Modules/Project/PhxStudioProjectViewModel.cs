using System;
using System.IO;
using System.Xml;
using KSoft;
using KSoft.IO;

namespace PhxStudio.Modules.Project
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Activated by Gemini composition when the project module is loaded.")]
	class PhxStudioProjectViewModel
		: KSoft.ObjectModel.BasicViewModel
	{
		PhxStudioProject mModel = new PhxStudioProject();
		public PhxStudioProject Model
		{
			get { return mModel; }
			private set { this.SetField(ref mModel, value); }
		}

		internal Exception? CreateNewInternal()
		{
			Exception? caught_exception = null;

			var new_project_model = new PhxStudioProject();
			this.Model = new_project_model;

			return caught_exception;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Project-open failures are returned to the UI as an Exception result.")]
		internal Exception? OpenInternal(string path)
		{
			Exception? caught_exception = null;
			try
			{
				var opened_project_model = new PhxStudioProject();

				using (var s = new XmlElementStream(path, FileAccess.Read))
				{
					s.InitializeAtRootElement();
					opened_project_model.Serialize(s);
				}

				this.Model = opened_project_model;
			}
			catch (Exception ex)
			{
				caught_exception = ex;
			}
			return caught_exception;
		}

		internal Exception? SaveInternal(string? path = null)
		{
			Exception? caught_exception = null;
			try
			{
				if (path == null)
					path = Model.ProjectFilePath;
				if (path.IsNullOrEmpty())
				{
					return new InvalidOperationException(
						"Tried to save project with a null-or-empty path.");
				}

				using (var s = XmlElementStream.CreateForWrite(PhxStudioProject.XmlRootName))
				{
					s.InitializeAtRootElement();
					Model.Serialize(s);

					s.Document.Save(path);
				}
			}
			catch (XmlException ex)
			{
				caught_exception = ex;
			}
			catch (UnauthorizedAccessException ex)
			{
				caught_exception = ex;
			}
			catch (IOException ex)
			{
				caught_exception = ex;
			}
			return caught_exception;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Engine initialization failures are intentionally returned to the UI as recoverable operation failures.")]
		internal Exception? PreloadEngineInternal()
		{
			if (Model == null)
			{
				return new InvalidOperationException("No Model is loaded");
			}
			else if (Model.Engine == null)
			{
				return new InvalidOperationException("No Model.Engine is loaded");
			}

			Exception? caught_exception = null;
			try
			{
				var engine = Model.Engine;

				if (!engine.Preload())
				{
					return new InvalidOperationException("Failed to Preload engine system");
				}

			} catch (Exception ex)
			{
				caught_exception = ex;
			}

			return caught_exception;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Engine load failures are intentionally returned to the UI as recoverable operation failures.")]
		internal Exception? LoadEngineInternal()
		{
			if (Model == null)
			{
				return new InvalidOperationException("No Model is loaded");
			}
			else if (Model.Engine == null)
			{
				return new InvalidOperationException("No Model.Engine is loaded");
			}

			Exception? caught_exception = null;
			try
			{
				var engine = Model.Engine;

				if (!engine.Load())
				{
					return new InvalidOperationException("Failed to Load engine system");
				}

			}
			catch (Exception ex)
			{
				caught_exception = ex;
			}

			return caught_exception;
		}
	};
}