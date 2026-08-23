using System;
using Gemini.Framework.Commands;

namespace PhxStudio.ProtoData.Powers.Commands
{
	[CommandDefinition]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Discovered by Gemini through CommandDefinition reflection.")]
	sealed class ViewPowersExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.PowersExplorer";

		public override string Name => CommandName;

		public override string Text => "Powers Explorer";

		public override string ToolTip => "Open Powers Explorer";

		public override Uri IconSource => new Uri(
			"pack://application:,,,/Gemini;component/Resources/Icons/Open.png");
	};
}
