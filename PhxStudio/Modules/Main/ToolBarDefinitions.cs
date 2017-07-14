using System.ComponentModel.Composition;
using Gemini.Framework.ToolBars;

namespace PhxStudio.Modules.Main
{
	public static class ToolBarDefinitions
	{
		#region File
		[Export]
		public static ExcludeToolBarItemDefinition ExcludeViewErrorListMenuItem = new ExcludeToolBarItemDefinition(
			Gemini.Modules.Shell.ToolBarDefinitions.OpenFileToolBarItem);


		[Export]
		public static ToolBarItemDefinition PhxOpenFileToolBarItem = new CommandToolBarItemDefinition<Commands.PhxOpenFileCommandDefinition>(
			Gemini.Modules.Shell.ToolBarDefinitions.StandardOpenSaveToolBarGroup, 0);
		#endregion
	};
}