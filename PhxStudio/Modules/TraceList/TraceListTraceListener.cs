using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using KSoft;

namespace PhxStudio.Modules.TraceList
{
	public sealed class TraceListTraceListener
		: TraceListener
	{
		const string kTraceAsTraceSource = "Trace";

		private readonly record struct PendingTrace(
			TraceListItemType Type,
			long TimeStamp,
			string? SourceName,
			string? Message,
			object?[]? Data);

		static readonly object gListenersLock = new();
		static readonly List<WeakReference<TraceListTraceListener>> gListeners = [];
		static ITraceList? gTraceList;

		public TraceListTraceListener() : this(ScheduleOnUIThread)
		{
		}

		internal TraceListTraceListener(Action<Action> scheduleDrain)
		{
			ArgumentNullException.ThrowIfNull(scheduleDrain);
			mScheduleDrain = scheduleDrain;

			lock (gListenersLock)
			{
				mTraceList = gTraceList;
				gListeners.Add(new WeakReference<TraceListTraceListener>(this));
			}
		}

		readonly object mTraceListLock = new();
		readonly Queue<PendingTrace> mPendingTraces = new();
		readonly Action<Action> mScheduleDrain;
		ITraceList? mTraceList;
		bool mDrainScheduled;

		internal static void Attach(ITraceList traceList)
		{
			ArgumentNullException.ThrowIfNull(traceList);

			List<TraceListTraceListener> listeners = [];
			lock (gListenersLock)
			{
				gTraceList = traceList;

				for (int x = gListeners.Count - 1; x >= 0; x--)
				{
					if (gListeners[x].TryGetTarget(out TraceListTraceListener? listener))
						listeners.Add(listener);
					else
						gListeners.RemoveAt(x);
				}
			}

			foreach (TraceListTraceListener listener in listeners)
				listener.AttachCore(traceList);
		}

		private void AttachCore(ITraceList traceList)
		{
			bool scheduleDrain;
			lock (mTraceListLock)
			{
				mTraceList = traceList;
				scheduleDrain = TryScheduleDrain();
			}

			if (scheduleDrain)
				ScheduleDrain();
		}

		private void AddItem(TraceListItemType type, long timeStamp, string? sourceName, string? message, object?[]? data = null)
		{
			var pendingTrace = new PendingTrace(type, timeStamp, sourceName, message, data);
			bool scheduleDrain;

			lock (mTraceListLock)
			{
				mPendingTraces.Enqueue(pendingTrace);
				scheduleDrain = TryScheduleDrain();
			}

			if (scheduleDrain)
				ScheduleDrain();
		}

		private bool TryScheduleDrain()
		{
			if (mTraceList == null || mPendingTraces.Count == 0 || mDrainScheduled)
				return false;

			mDrainScheduled = true;
			return true;
		}

		private void ScheduleDrain()
		{
			try
			{
				mScheduleDrain(DrainPendingTraces);
			}
			catch
			{
				lock (mTraceListLock)
					mDrainScheduled = false;
				throw;
			}
		}

		private void DrainPendingTraces()
		{
			try
			{
				while (true)
				{
					PendingTrace pendingTrace;
					ITraceList traceList;

					lock (mTraceListLock)
					{
						if (mTraceList == null || mPendingTraces.Count == 0)
						{
							mDrainScheduled = false;
							return;
						}

						traceList = mTraceList;
						pendingTrace = mPendingTraces.Dequeue();
					}

					traceList.AddItem(
						pendingTrace.Type,
						pendingTrace.TimeStamp,
						pendingTrace.SourceName,
						pendingTrace.Message,
						pendingTrace.Data);
				}
			}
			catch
			{
				lock (mTraceListLock)
					mDrainScheduled = false;
				throw;
			}
		}

		private static void ScheduleOnUIThread(Action drain)
		{
			var dispatcher = Application.Current?.Dispatcher;
			if (dispatcher == null || dispatcher.CheckAccess())
				drain();
			else
				_ = dispatcher.BeginInvoke(DispatcherPriority.DataBind, drain);
		}

		private static long GetTimeStamp(TraceEventCache? eventCache) => eventCache?.Timestamp ?? 0;

		public override void Write(string? message) => this.WriteLine(message);

		public override void WriteLine(string? message) => this.TraceEvent(null, kTraceAsTraceSource, TraceEventType.Information, 0, message);

		public override void Fail(string? message, string? detailMessage)
		{
			var failMessage = new StringBuilder(message);
			if (detailMessage != null)
			{
				failMessage.Append(' ');
				failMessage.Append(detailMessage);
			}

			this.TraceEvent(null, kTraceAsTraceSource, TraceEventType.Error, TypeExtensions.kNone, failMessage.ToString());
		}

		public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? format, params object?[]? args)
		{
			if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, format, args, data1: null, data: null))
				return;

			string? message;
			if (args is { Length: > 0 })
				message = string.Format(CultureInfo.InvariantCulture, format ?? string.Empty, args);
			else
				message = format;

			AddItem(eventType.ToTraceListItemType(),
				GetTimeStamp(eventCache), source, message);
		}

		public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? message)
		{
			if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, message, args: null, data1: null, data: null))
				return;

			AddItem(eventType.ToTraceListItemType(),
				GetTimeStamp(eventCache), source, message);
		}

		public override void TraceData(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, object? data)
		{
			if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, args: null, data1: data, data: null))
				return;

			var item_data = GetDataEntryForTraceListItem(data);
			var message = item_data?.ToString();

			AddItem(eventType.ToTraceListItemType(),
				GetTimeStamp(eventCache), source, message, new object?[] { item_data });
		}

		public override void TraceData(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, params object?[]? data)
		{
			if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, args: null, data1: null, data: data))
				return;

			var item_data = GetDataForTraceListItem(data);
			var message = item_data is { Length: > 0 }
				? item_data[0]?.ToString()
				: "NO MESSAGE";

			AddItem(eventType.ToTraceListItemType(),
				GetTimeStamp(eventCache), source, message, item_data);
		}

		public override void TraceTransfer(TraceEventCache? eventCache, string source, int id, string? message, Guid relatedActivityId)
		{
			var eventType = TraceEventType.Transfer;

			if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, message, args: null, data1: null, data: null))
				return;

			AddItem(eventType.ToTraceListItemType(),
				GetTimeStamp(eventCache), source, relatedActivityId.ToString());
		}

		private object?[]? GetDataForTraceListItem(params object?[]? args)
		{
			if (args is null || args.Length == 0)
				return args;

			var result = new object?[args.Length];

			for (int x = 0; x < result.Length; x++)
			{
				var arg = args[x];
				result[x] = GetDataEntryForTraceListItem(arg);
			}

			return result;
		}

		private object? GetDataEntryForTraceListItem(object? data)
		{
			if (data == null)
				return data;

			var result = data;
			if (result is Exception)
			{
			}
			else
			{
				result = result.ToString();
			}
			return result;
		}
	};
}
