using System;
using Gemini.Framework.Commands;

namespace PhxStudio.ProtoData.Leaders.Commands
{
	[CommandDefinition]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Discovered by Gemini through CommandDefinition reflection.")]
	sealed class ViewLeadersExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.LeadersExplorer";

		public override string Name => CommandName;

		public override string Text => "Leaders Explorer";

		public override string ToolTip => "Open Leaders Explorer";

		public override Uri IconSource => new Uri(
			"pack://application:,,,/Gemini;component/Resources/Icons/Open.png");
	};
}
