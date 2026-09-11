using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Caliburn.Micro;
using Gemini.Framework.Results;
using KSoft;
using KSoft.Phoenix;
using KSoft.Phoenix.Phx.Meta;

namespace PhxStudio.Modules.PhxInspectors.Inspectors
{
	public partial class ProtoDataReferenceViewModel
		: EditorBase<int>
		, ILabelledInspector
	{
		public const string kNoneText = "NONE";

		public IProtoDataReferenceAttribute ReferenceAttribute { get; private set; }

		string? mText;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mText))]
		public partial string? Text { get; private set; }

		public ProtoDataReferenceViewModel(IProtoDataReferenceAttribute attr)
		{
			ReferenceAttribute = attr;
		}

		void UpdateText()
		{
			Text = kNoneText;

			if (Value.IsNotNone())
			{
				var lookup = IoC.Get<ProtoData.IProtoDataObjectLookup>(ReferenceAttribute.GetExportContractName());
				if (lookup == null)
				{
					Text = "INTERNAL ERROR";
					Debug.Trace.PhxStudio.TraceDataSansId(System.Diagnostics.TraceEventType.Error,
						"Failed to resolve ObjectLookup",
						ReferenceAttribute.GetExportContractName(),
						base.Name);
					return;
				}

				if (lookup.SourceObjectDatabase is not { } sourceObjectDatabase)
				{
					Text = "INTERNAL ERROR";
					return;
				}

				var provider = sourceObjectDatabase.Provider;
				var list = provider.GetNamesInterface(lookup.SourceObjectDatabaseKindId);
				Text = list.TryGetMemberName(this.Value);
			}
		}

		public override void HandleViewLoaded()
		{
			base.HandleViewLoaded();

			UpdateText();
		}

		public override void NotifyOfPropertyChange([CallerMemberName] string? propertyName = null)
		{
			if (propertyName == nameof(base.Value))
			{
				UpdateText();
			}
			base.NotifyOfPropertyChange(propertyName);
		}

		public IEnumerable<IResult> ClearReference()
		{
			if (IsReadOnly)
				yield return SimpleResult.Cancelled();

			Value = TypeExtensions.kNone;
			yield return SimpleResult.Succeeded();
		}

		public IEnumerable<IResult> ChooseReference()
		{
			yield break;
		}
	};
}
