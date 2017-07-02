using System;
using Gemini.Framework.Commands;

namespace PhxStudio.ProtoData.Civs.Commands
{
	[CommandDefinition]
	sealed class ViewCivsExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.CivsExplorer";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "Civs Explorer";
		} }

		public override string ToolTip { get {
			return "Open Civs Explorer";
		} }

		public override Uri IconSource { get { return new Uri(
				"pack://application:,,,/Gemini;component/Resources/Icons/Open.png"
			);
		} }
	};
}