using System.Diagnostics;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhxStudio.Modules.TraceList;

namespace Test.PhxStudio;

[TestClass]
public sealed class TraceListTraceListenerTests
{
	[TestMethod]
	public void Attach_DeliversQueuedAndSubsequentEvents()
	{
		using var listener = new TraceListTraceListener();
		listener.TraceEvent(null, "QueuedSource", TraceEventType.Warning, 1, "queued");

		var traceList = new RecordingTraceList();
		TraceListTraceListener.Attach(traceList);

		listener.TraceEvent(null, "AttachedSource", TraceEventType.Error, 2, "attached");

		Assert.HasCount(2, traceList.Items);
		Assert.AreEqual("QueuedSource", traceList.Items[0].SourceName);
		Assert.AreEqual("queued", traceList.Items[0].Message);
		Assert.AreEqual("AttachedSource", traceList.Items[1].SourceName);
		Assert.AreEqual("attached", traceList.Items[1].Message);
	}

	[TestMethod]
	public void MakeRoomForNewItem_HandlesZeroBoundedAndUnlimitedLimits()
	{
		var items = new BindableCollection<TraceListItem>
		{
			new TraceListItem(),
			new TraceListItem(),
		};

		Assert.IsFalse(TraceListViewModel.MakeRoomForNewItem(items, 0));
		Assert.HasCount(2, items);

		Assert.IsTrue(TraceListViewModel.MakeRoomForNewItem(items, 2));
		Assert.HasCount(1, items);

		Assert.IsTrue(TraceListViewModel.MakeRoomForNewItem(items, -1));
		Assert.HasCount(1, items);
	}

	private sealed class RecordingTraceList : Tool, ITraceList
	{
		private int mItemNumber;
		private readonly BindableCollection<TraceListItem> mItems = [];

		public override PaneLocation PreferredLocation => PaneLocation.Bottom;
		public int TotalNumberOfTraces { get; set; }
		public bool PauseTracing { get; set; }
		public bool ShowCritical { get; set; }
		public bool ShowError { get; set; }
		public bool ShowWarning { get; set; }
		public bool ShowInformation { get; set; }
		public bool ShowVerbose { get; set; }
		public bool ShowStart { get; set; }
		public bool ShowStop { get; set; }
		public bool ShowSuspend { get; set; }
		public bool ShowResume { get; set; }
		public bool ShowTransfer { get; set; }
		public bool TailTraces { get; set; }
		public IObservableCollection<TraceListItem> Items => mItems;

		public void AddItem(
			TraceListItemType type,
			long timeStamp,
			string? sourceName,
			string? message,
			object?[]? data = null,
			System.Action? onClick = null)
		{
			mItems.Add(new TraceListItem
			{
				ItemType = type,
				Number = ++mItemNumber,
				TimeStamp = timeStamp,
				SourceName = sourceName,
				Message = message,
				Data = data ?? [],
				OnClick = onClick,
			});
			TotalNumberOfTraces = mItemNumber;
		}

		public void ClearAll() => mItems.Clear();
	}
}
