using System;
using Gemini.Framework.Commands;

namespace PhxStudio.ProtoData.Powers.Commands
{
	[CommandDefinition]
	sealed class ViewPowersExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.PowersExplorer";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "Powers Explorer";
		} }

		public override string ToolTip { get {
			return "Open Powers Explorer";
		} }

		public override Uri IconSource { get { return new Uri(
				"pack://application:,,,/Gemini;component/Resources/Icons/Open.png"
			);
		} }
	};
}