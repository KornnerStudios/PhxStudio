using System;
using Gemini.Framework.Commands;

namespace PhxStudio.ProtoData.Squads.Commands
{
	[CommandDefinition]
	sealed class ViewSquadsExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.SquadsExplorer";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "Squads Explorer";
		} }

		public override string ToolTip { get {
			return "Open Squads Explorer";
		} }

		public override Uri IconSource { get { return new Uri(
				"pack://application:,,,/Gemini;component/Resources/Icons/Open.png"
			);
		} }
	};
}