using System;
using System.Collections.Generic;
using Gemini.Framework;

namespace PhxStudio.Modules.PhxInspectors
{
	public partial class PhxInspectorViewModel
		: Document
	{
		IInspectableObject? mInspectableModel;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mInspectableModel))]
		public partial IInspectableObject? InspectableModel { get; set; }

		public void HandleViewLoaded()
		{
			if (InspectableModel == null)
				return;

			RecurseEditors(InspectableModel.Inspectors, HandleViewLoadedAction);
		}

		private static void HandleViewLoadedAction(Inspectors.IEditor? editor)
		{
			if (editor != null)
				editor.HandleViewLoaded();
		}

		public void ResetAll()
		{
			if (InspectableModel == null)
				return;

			RecurseEditors(InspectableModel.Inspectors, ResetEditorAction);
		}

		private static void ResetEditorAction(Inspectors.IEditor? editor)
		{
			if (editor != null && editor.CanReset)
				editor.Reset();
		}

		public void RecurseEditors(IEnumerable<Inspectors.IInspector> inspectors, Action<Inspectors.IEditor?> action)
		{
			ArgumentNullException.ThrowIfNull(inspectors);
			ArgumentNullException.ThrowIfNull(action);

			foreach (var inspector in inspectors)
			{
				var group = inspector as Inspectors.CollapsibleGroupViewModel;
				if (group != null)
				{
					RecurseEditors(group.Children, action);
				}
				else
				{
					action(inspector as Inspectors.IEditor);
				}
			}
		}
	};
}
