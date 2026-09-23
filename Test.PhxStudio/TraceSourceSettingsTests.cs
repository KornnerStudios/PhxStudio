using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhxStudio.Modules.TraceList;

namespace Test.PhxStudio;

[TestClass]
public sealed class TraceSourceSettingsTests
{
	[TestMethod]
	public void ApplyTo_MapsSeverityActivityAndDisabledState()
	{
		var warningSource = new TraceSource("WarningSource", SourceLevels.All);
		var disabledSource = new TraceSource("DisabledSource", SourceLevels.All);
		try
		{
			var settings = new TraceSourceSettings();
			settings.SourceSettings.Add(new TraceSourceSetting
			{
				Name = warningSource.Name,
				Level = TraceLevel.Warning,
			});
			var disabledSetting = new TraceSourceSetting
			{
				Name = disabledSource.Name,
				Level = TraceLevel.Verbose,
				Disabled = true,
			};
			settings.SourceSettings.Add(disabledSetting);

			settings.ApplyTo([warningSource, disabledSource]);

			Assert.AreEqual(SourceLevels.Warning | SourceLevels.ActivityTracing, warningSource.Switch.Level);
			Assert.AreEqual(SourceLevels.Off, disabledSource.Switch.Level);

			disabledSetting.Disabled = false;
			settings.ApplyTo([warningSource, disabledSource]);

			Assert.AreEqual(SourceLevels.All, disabledSource.Switch.Level);
		}
		finally
		{
			warningSource.Close();
			disabledSource.Close();
		}
	}
}
