using System;
using Gemini.Framework.Commands;

namespace PhxStudio.ProtoData.Leaders.Commands
{
	[CommandDefinition]
	sealed class ViewLeadersExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.LeadersExplorer";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "Leaders Explorer";
		} }

		public override string ToolTip { get {
			return "Open Leaders Explorer";
		} }

		public override Uri IconSource { get { return new Uri(
				"pack://application:,,,/Gemini;component/Resources/Icons/Open.png"
			);
		} }
	};
}