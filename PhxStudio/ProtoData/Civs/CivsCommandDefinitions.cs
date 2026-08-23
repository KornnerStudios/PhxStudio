using System;
using Gemini.Framework.Commands;

namespace PhxStudio.ProtoData.Civs.Commands
{
	[CommandDefinition]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Discovered by Gemini through CommandDefinition reflection.")]
	sealed class ViewCivsExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.CivsExplorer";

		public override string Name => CommandName;

		public override string Text => "Civs Explorer";

		public override string ToolTip => "Open Civs Explorer";

		public override Uri IconSource => new Uri(
			"pack://application:,,,/Gemini;component/Resources/Icons/Open.png");
	};
}
