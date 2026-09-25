using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using KSoft.Debug;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhxStudio.Modules.TraceList;

namespace Test.PhxStudio;

[TestClass]
[DoNotParallelize]
public sealed class TraceSourceLoggerIntegrationTests
{
	[TestMethod]
	public void Logger_UsesLiveSourceSettingsAndBothConfiguredListenerContracts()
	{
		string testDirectory = Path.Combine(Path.GetTempPath(), $"PhxStudioLogger-{Guid.NewGuid():N}");
		Directory.CreateDirectory(testDirectory);
		string sourceName = $"PhxStudio.Test.{Guid.NewGuid():N}";
		var source = new TraceSource(sourceName, SourceLevels.All);

		try
		{
			KSoft.Program.Initialize();

			source.Listeners.Clear();
			using var fileListener = new KSoftFileLogTraceListener
			{
				Append = false,
				AutoFlush = true,
				BaseFileName = "logger-bridge",
				CustomLocation = testDirectory,
				MaxFileSize = 1024 * 1024,
				ReserveDiskSpace = 0,
			};
			using var traceListListener = new TraceListTraceListener();
			var traceList = new RecordingTraceList();
			TraceListTraceListener.Attach(traceList);
			source.Listeners.Add(fileListener);
			source.Listeners.Add(traceListListener);
			KSoft.Program.RegisterTraceSources(source);

			var sourceSettings = new TraceSourceSettings();
			var setting = new TraceSourceSetting
			{
				Name = sourceName,
				Level = TraceLevel.Warning,
			};
			sourceSettings.SourceSettings.Add(setting);
			sourceSettings.ApplyTo([source]);

			ILogger logger = KSoft.Program.CreateLogger(sourceName + ".Child");
			logger.LogInformation("filtered");

			var exception = new InvalidOperationException("expected failure");
			logger.Log(
				LogLevel.Error,
				new EventId(42, "Failure"),
				"operation failed",
				exception,
				static (state, _) => state);
			source.Flush();

			Assert.HasCount(1, traceList.Items);
			TraceListItem item = traceList.Items[0];
			Assert.AreEqual(sourceName, item.SourceName);
			Assert.AreEqual(TraceListItemType.Error, item.ItemType);
			Assert.AreEqual("operation failed", item.Message);
			Assert.HasCount(2, item.Data);
			Assert.AreSame(exception, item.Data[1]);

			setting.Disabled = true;
			sourceSettings.ApplyTo([source]);
			logger.LogWarning("disabled");
			Assert.HasCount(1, traceList.Items);

			setting.Disabled = false;
			setting.Level = TraceLevel.Verbose;
			sourceSettings.ApplyTo([source]);
			logger.LogInformation("enabled");
			Assert.HasCount(2, traceList.Items);

			source.Close();
			string log = File.ReadAllText(Path.Combine(testDirectory, "logger-bridge.log"), Encoding.UTF8);
			StringAssert.Contains(log, $"{sourceName} Error: 42 :");
			StringAssert.Contains(log, "operation failed");
			StringAssert.Contains(log, "expected failure");
			StringAssert.Contains(log, "enabled");
			Assert.IsFalse(log.Contains("filtered", StringComparison.Ordinal));
			Assert.IsFalse(log.Contains("disabled", StringComparison.Ordinal));
		}
		finally
		{
			source.Close();
			KSoft.Program.Dispose();
			Directory.Delete(testDirectory, recursive: true);
		}
	}

	private sealed class RecordingTraceList : Tool, ITraceList
	{
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
				Number = ++TotalNumberOfTraces,
				TimeStamp = timeStamp,
				SourceName = sourceName,
				Message = message,
				Data = data ?? [],
				OnClick = onClick,
			});
		}

		public void ClearAll() => mItems.Clear();
	}
}
