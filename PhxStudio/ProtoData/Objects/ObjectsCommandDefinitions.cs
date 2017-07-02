using System;
using Gemini.Framework.Commands;

namespace PhxStudio.ProtoData.Objects.Commands
{
	[CommandDefinition]
	sealed class ViewObjectsExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.ObjectsExplorer";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "Objects Explorer";
		} }

		public override string ToolTip { get {
			return "Open Objects Explorer";
		} }

		public override Uri IconSource { get { return new Uri(
				"pack://application:,,,/Gemini;component/Resources/Icons/Open.png"
			);
		} }
	};
}