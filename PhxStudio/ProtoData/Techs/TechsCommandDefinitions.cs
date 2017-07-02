using System;
using Gemini.Framework.Commands;

namespace PhxStudio.ProtoData.Techs.Commands
{
	[CommandDefinition]
	sealed class ViewTechsExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.TechsExplorer";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "Techs Explorer";
		} }

		public override string ToolTip { get {
			return "Open Techs Explorer";
		} }

		public override Uri IconSource { get { return new Uri(
				"pack://application:,,,/Gemini;component/Resources/Icons/Open.png"
			);
		} }
	};
}