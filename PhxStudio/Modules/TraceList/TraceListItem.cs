using System;
using Caliburn.Micro;

namespace PhxStudio.Modules.TraceList
{
	public sealed class TraceListItem
		: PropertyChangedBase
	{
		TraceListItemType mItemType = TraceListItemType.Invalid;
		public TraceListItemType ItemType
		{
			get { return mItemType; }
			set { this.SetFieldEnum(ref mItemType, value); }
		}

		int mNumber;
		public int Number
		{
			get { return mNumber; }
			set { this.SetFieldVal(ref mNumber, value); }
		}

		long mTimeStamp;
		public long TimeStamp
		{
			get { return mTimeStamp; }
			set { this.SetFieldVal(ref mTimeStamp, value); }
		}

		string mSourceName;
		public string SourceName
		{
			get { return mSourceName; }
			set { this.SetFieldObj(ref mSourceName, value); }
		}

		string mMessage;
		public string Message
		{
			get { return mMessage; }
			set { this.SetFieldObj(ref mMessage, value); }
		}

		object[] mData;
		public object[] Data
		{
			get { return mData; }
			set
			{
				if (this.SetFieldRef(ref mData, value))
				{
					NotifyOfPropertyChange(nameof(HasData));
				}
			}
		}
		public bool HasData { get { return mData != null && mData.Length > 0; } }


		public System.Action OnClick { get; set; }
	};
}
