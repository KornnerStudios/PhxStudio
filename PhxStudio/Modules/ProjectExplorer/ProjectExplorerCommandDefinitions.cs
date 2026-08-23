using System;
using Gemini.Framework.Commands;

namespace PhxStudio.Modules.ProjectExplorer.Commands
{
	[CommandDefinition]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Discovered by Gemini through CommandDefinition reflection.")]
	sealed class ViewProjectExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.ProjectExplorer";

		public override string Name => CommandName;

		public override string Text => "Project Explorer";

		public override string ToolTip => "Open Project Explorer";

		public override Uri IconSource => new Uri(
			"pack://application:,,,/Gemini;component/Resources/Icons/Open.png");
	};
}
