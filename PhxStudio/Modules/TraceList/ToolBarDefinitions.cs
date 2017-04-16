using System.ComponentModel.Composition;
using Gemini.Framework.ToolBars;

namespace PhxStudio.Modules.TraceList
{
	using Commands;

	public static class ToolBarDefinitions
	{
		public static ToolBarDefinition TraceListToolBar = new ToolBarDefinition(0, "TraceList");
		private static int gSortOrder = -1;

		[Export]
		public static ToolBarItemGroupDefinition ToggleCriticalGroup = new ToolBarItemGroupDefinition(
			TraceListToolBar, ++gSortOrder);
		[Export]
		public static ToolBarItemDefinition ToggleCriticalToolBarItem = new CommandToolBarItemDefinition<ToggleCriticalGroupCommandDefinition>(
			ToggleCriticalGroup, gSortOrder, ToolBarItemDisplay.IconAndText);


		[Export]
		public static ToolBarItemGroupDefinition ToggleErrorsGroup = new ToolBarItemGroupDefinition(
			TraceListToolBar, ++gSortOrder);
		[Export]
		public static ToolBarItemDefinition ToggleErrorsToolBarItem = new CommandToolBarItemDefinition<ToggleErrorsGroupCommandDefinition>(
			ToggleErrorsGroup, gSortOrder, ToolBarItemDisplay.IconAndText);


		[Export]
		public static ToolBarItemGroupDefinition ToggleWarningsGroup = new ToolBarItemGroupDefinition(
			TraceListToolBar, ++gSortOrder);
		[Export]
		public static ToolBarItemDefinition ToggleWarningsToolBarItem = new CommandToolBarItemDefinition<ToggleWarningsGroupCommandDefinition>(
			ToggleWarningsGroup, gSortOrder, ToolBarItemDisplay.IconAndText);


		[Export]
		public static ToolBarItemGroupDefinition ToggleInformationGroup = new ToolBarItemGroupDefinition(
			TraceListToolBar, ++gSortOrder);
		[Export]
		public static ToolBarItemDefinition ToggleInformationToolBarItem = new CommandToolBarItemDefinition<ToggleInformationGroupCommandDefinition>(
			ToggleInformationGroup, gSortOrder, ToolBarItemDisplay.IconAndText);


		[Export]
		public static ToolBarItemGroupDefinition ToggleVerboseGroup = new ToolBarItemGroupDefinition(
			TraceListToolBar, ++gSortOrder);
		[Export]
		public static ToolBarItemDefinition ToggleVerboseToolBarItem = new CommandToolBarItemDefinition<ToggleVerboseGroupCommandDefinition>(
			ToggleVerboseGroup, gSortOrder, ToolBarItemDisplay.IconAndText);


		[Export]
		public static ToolBarItemGroupDefinition ToggleStartGroup = new ToolBarItemGroupDefinition(
			TraceListToolBar, ++gSortOrder);
		[Export]
		public static ToolBarItemDefinition ToggleStartToolBarItem = new CommandToolBarItemDefinition<ToggleStartGroupCommandDefinition>(
			ToggleStartGroup, gSortOrder, ToolBarItemDisplay.IconAndText);


		[Export]
		public static ToolBarItemGroupDefinition ToggleStopGroup = new ToolBarItemGroupDefinition(
			TraceListToolBar, ++gSortOrder);
		[Export]
		public static ToolBarItemDefinition ToggleStopToolBarItem = new CommandToolBarItemDefinition<ToggleStopGroupCommandDefinition>(
			ToggleStopGroup, gSortOrder, ToolBarItemDisplay.IconAndText);


		[Export]
		public static ToolBarItemGroupDefinition ToggleSuspendGroup = new ToolBarItemGroupDefinition(
			TraceListToolBar, ++gSortOrder);
		[Export]
		public static ToolBarItemDefinition ToggleSuspendToolBarItem = new CommandToolBarItemDefinition<ToggleSuspendGroupCommandDefinition>(
			ToggleSuspendGroup, gSortOrder, ToolBarItemDisplay.IconAndText);


		[Export]
		public static ToolBarItemGroupDefinition ToggleResumeGroup = new ToolBarItemGroupDefinition(
			TraceListToolBar, ++gSortOrder);
		[Export]
		public static ToolBarItemDefinition ToggleResumeToolBarItem = new CommandToolBarItemDefinition<ToggleResumeGroupCommandDefinition>(
			ToggleResumeGroup, gSortOrder, ToolBarItemDisplay.IconAndText);


		[Export]
		public static ToolBarItemGroupDefinition ToggleTransferGroup = new ToolBarItemGroupDefinition(
			TraceListToolBar, ++gSortOrder);
		[Export]
		public static ToolBarItemDefinition ToggleTransferToolBarItem = new CommandToolBarItemDefinition<ToggleTransferGroupCommandDefinition>(
			ToggleTransferGroup, gSortOrder, ToolBarItemDisplay.IconAndText);
	};
}
