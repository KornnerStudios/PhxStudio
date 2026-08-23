using System;
using Gemini.Framework.Commands;

namespace PhxStudio.ProtoData.Squads.Commands
{
	[CommandDefinition]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Discovered by Gemini through CommandDefinition reflection.")]
	sealed class ViewSquadsExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.SquadsExplorer";

		public override string Name => CommandName;

		public override string Text => "Squads Explorer";

		public override string ToolTip => "Open Squads Explorer";

		public override Uri IconSource => new Uri(
			"pack://application:,,,/Gemini;component/Resources/Icons/Open.png");
	};
}
