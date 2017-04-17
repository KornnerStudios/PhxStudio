using System.ComponentModel.Composition;
using Gemini.Framework.Menus;

namespace PhxStudio.Modules.Main
{
	public static class MenuDefinitions
	{
		#region File
		[Export]
		public static MenuItemDefinition FileNewProject = new CommandMenuItemDefinition
			<Project.Commands.FileNewProjectCommandDefinition>(
				Gemini.Modules.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 1);

		[Export]
		public static MenuItemDefinition FileOpenProject = new CommandMenuItemDefinition
			<Project.Commands.FileOpenProjectCommandDefinition>(
				Gemini.Modules.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 2);

		[Export]
		public static MenuItemDefinition FileSaveProject = new CommandMenuItemDefinition
			<Project.Commands.FileSaveProjectCommandDefinition>(
				Gemini.Modules.MainMenu.MenuDefinitions.FileSaveMenuGroup, 1);

		[Export]
		public static MenuItemDefinition FileSaveProjectAs = new CommandMenuItemDefinition
			<Project.Commands.FileSaveProjectAsCommandDefinition>(
				Gemini.Modules.MainMenu.MenuDefinitions.FileSaveMenuGroup, 2);
		#endregion

		#region View
		[Export]
		public static MenuItemDefinition ViewProjectExplorer = new CommandMenuItemDefinition
			<ProjectExplorer.Commands.ViewProjectExplorerDefinition>(
				Gemini.Modules.MainMenu.MenuDefinitions.ViewToolsMenuGroup, 0);

		[Export]
		public static MenuItemDefinition ViewTraceList = new CommandMenuItemDefinition
			<TraceList.Commands.ViewTraceListCommandDefinition>(
				Gemini.Modules.MainMenu.MenuDefinitions.ViewToolsMenuGroup, 1);
		#endregion

		#region Debug
		[Export]
		public static MenuDefinition DebugMenu = new MenuDefinition(
			Gemini.Modules.MainMenu.MenuDefinitions.MainMenuBar,
			int.MaxValue,
			"DEBUG");

		[Export]
		public static MenuItemGroupDefinition DebugTestMenuGroup = new MenuItemGroupDefinition(
			DebugMenu, 0);
		#endregion
	};
}