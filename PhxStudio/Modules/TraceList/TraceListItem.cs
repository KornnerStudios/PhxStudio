using System;
using System.ComponentModel;
using Caliburn.Micro;
using KSoft;

namespace PhxStudio.Modules.TraceList
{
	public sealed partial class TraceListItem
		: PropertyChangedBase
	{
		TraceListItemType mItemType = TraceListItemType.Invalid;
		[ReadOnly(true)]
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mItemType))]
		public partial TraceListItemType ItemType { get; set; }

		int mNumber;
		[ReadOnly(true)]
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mNumber))]
		public partial int Number { get; set; }

		long mTimeStamp;
		[ReadOnly(true)]
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mTimeStamp))]
		public partial long TimeStamp { get; set; }

		string? mSourceName;
		[ReadOnly(true)]
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mSourceName))]
		public partial string? SourceName { get; set; }

		string? mMessage;
		[ReadOnly(true)]
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mMessage))]
		public partial string? Message { get; set; }

		// #HACK_PHXSTUDIO using EmptyArray here because Gemini's Inspector won't update
		// the TraceDataEditorView when using null and the previous item selected
		// actually had data.
		// Also not using ReadOnly because it disables the View's DataGrid completely.
		object?[] mData = KSoft.Util.EmptyArray;
		[Browsable(false)]
		public object?[] Data
		{
			get { return mData; }
			set
			{
				var data = value;
				if (data is null || data.Length == 0)
					data = KSoft.Util.EmptyArray;

				if (this.SetFieldRef(ref mData, data))
				{
					NotifyOfPropertyChange(nameof(HasData));
				}
			}
		}
		[Browsable(false)]
		public bool HasData => !mData.IsNullOrEmpty() && mData != KSoft.Util.EmptyArray;

		[Browsable(false)]
		public System.Action? OnClick { get; set; }
	};
}
