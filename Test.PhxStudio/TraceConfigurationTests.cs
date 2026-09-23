using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.PhxStudio;

[TestClass]
public sealed class TraceConfigurationTests
{
	[TestMethod]
	public void AppConfig_CoversAllApplicationTraceSources()
	{
		string configPath = Path.Combine(AppContext.BaseDirectory, "TraceConfigs", "PhxStudio.dll.config");
		var document = XDocument.Load(configPath);
		var diagnostics = document.Root?.Element("system.diagnostics");
		Assert.IsNotNull(diagnostics);

		var configuredSources = diagnostics.Element("sources")!
			.Elements("source")
			.ToDictionary(source => (string)source.Attribute("name")!, StringComparer.Ordinal);

		foreach (string sourceName in global::PhxStudio.App.AllTraceSources.Select(source => source.Name))
		{
			Assert.IsTrue(configuredSources.TryGetValue(sourceName, out XElement? source), $"Missing trace source '{sourceName}'.");
			AssertHasListener(source, "BetterTextTrace");
			AssertHasListener(source, "TraceList");
		}

		XElement fileListener = diagnostics.Element("sharedListeners")!
			.Elements("add")
			.Single(listener => (string?)listener.Attribute("name") == "BetterTextTrace");
		Assert.AreEqual("PhxStudio", (string?)fileListener.Attribute("BaseFileName"));
	}

	private static void AssertHasListener(XElement? source, string listenerName)
	{
		Assert.IsNotNull(source);
		Assert.IsTrue(source.Element("listeners")!
			.Elements("add")
			.Any(listener => (string?)listener.Attribute("name") == listenerName));
	}
}
