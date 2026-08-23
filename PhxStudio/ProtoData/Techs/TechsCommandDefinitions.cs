using System;
using Gemini.Framework.Commands;

namespace PhxStudio.ProtoData.Techs.Commands
{
	[CommandDefinition]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Discovered by Gemini through CommandDefinition reflection.")]
	sealed class ViewTechsExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.TechsExplorer";

		public override string Name => CommandName;

		public override string Text => "Techs Explorer";

		public override string ToolTip => "Open Techs Explorer";

		public override Uri IconSource => new Uri(
			"pack://application:,,,/Gemini;component/Resources/Icons/Open.png");
	};
}
