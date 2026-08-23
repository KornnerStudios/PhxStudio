using System;
using Gemini.Framework.Commands;

namespace PhxStudio.ProtoData.Objects.Commands
{
	[CommandDefinition]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Discovered by Gemini through CommandDefinition reflection.")]
	sealed class ViewObjectsExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.ObjectsExplorer";

		public override string Name => CommandName;

		public override string Text => "Objects Explorer";

		public override string ToolTip => "Open Objects Explorer";

		public override Uri IconSource => new Uri(
			"pack://application:,,,/Gemini;component/Resources/Icons/Open.png");
	};
}
