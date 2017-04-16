using System;
using Gemini.Framework.Commands;

namespace PhxStudio.Modules.TraceList.Commands
{
	[CommandDefinition]
	public sealed class ViewTraceListCommandDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.TraceList";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "Trace List";
		} }

		public override string ToolTip { get {
			return "View the app tracing list";
		} }
	};

	[CommandDefinition]
	public abstract class ToggleGroupCommandDefinitionBase
		: CommandDefinition
	{
		private string mCommandName;
		private Uri mImageSource;
		private string mTypeName;

		protected ToggleGroupCommandDefinitionBase(string commandName, TraceListItemType groupType)
		{
			mCommandName = commandName;

			mImageSource = new Uri(string.Format("pack://application:,,,/PhxStudio;component/Modules/TraceList/Images/{0}.png",
				groupType.ToString()));
		}

		public override string Name { get {
			return mCommandName;
		} }

		public override string Text { get {
			return "[NotUsed]";
		} }

		public override string ToolTip { get {
			return "[NotUsed]";
		} }

		public override Uri IconSource { get {
			return mImageSource;
		} }
	};

	[CommandDefinition]
	public sealed class ToggleCriticalGroupCommandDefinition
		: ToggleGroupCommandDefinitionBase
	{
		public const string CommandName = "ErrorList.ToggleCritical";

		public ToggleCriticalGroupCommandDefinition()
			: base(CommandName, TraceListItemType.Critical)
		{
		}
	};

	[CommandDefinition]
	public sealed class ToggleErrorsGroupCommandDefinition
		: ToggleGroupCommandDefinitionBase
	{
		public const string CommandName = "ErrorList.ToggleErrors";

		public ToggleErrorsGroupCommandDefinition()
			: base(CommandName, TraceListItemType.Error)
		{
		}
	};

	[CommandDefinition]
	public sealed class ToggleWarningsGroupCommandDefinition
		: ToggleGroupCommandDefinitionBase
	{
		public const string CommandName = "ErrorList.ToggleWarnings";

		public ToggleWarningsGroupCommandDefinition()
			: base(CommandName, TraceListItemType.Warning)
		{
		}
	};

	[CommandDefinition]
	public sealed class ToggleInformationGroupCommandDefinition
		: ToggleGroupCommandDefinitionBase
	{
		public const string CommandName = "ErrorList.ToggleInformation";

		public ToggleInformationGroupCommandDefinition()
			: base(CommandName, TraceListItemType.Information)
		{
		}
	};

	[CommandDefinition]
	public sealed class ToggleVerboseGroupCommandDefinition
		: ToggleGroupCommandDefinitionBase
	{
		public const string CommandName = "ErrorList.ToggleVerbose";

		public ToggleVerboseGroupCommandDefinition()
			: base(CommandName, TraceListItemType.Verbose)
		{
		}
	};

	[CommandDefinition]
	public sealed class ToggleStartGroupCommandDefinition
		: ToggleGroupCommandDefinitionBase
	{
		public const string CommandName = "ErrorList.ToggleStart";

		public ToggleStartGroupCommandDefinition()
			: base(CommandName, TraceListItemType.Start)
		{
		}
	};

	[CommandDefinition]
	public sealed class ToggleStopGroupCommandDefinition
		: ToggleGroupCommandDefinitionBase
	{
		public const string CommandName = "ErrorList.ToggleStop";

		public ToggleStopGroupCommandDefinition()
			: base(CommandName, TraceListItemType.Stop)
		{
		}
	};

	[CommandDefinition]
	public sealed class ToggleSuspendGroupCommandDefinition
		: ToggleGroupCommandDefinitionBase
	{
		public const string CommandName = "ErrorList.ToggleSuspend";

		public ToggleSuspendGroupCommandDefinition()
			: base(CommandName, TraceListItemType.Suspend)
		{
		}
	};

	[CommandDefinition]
	public sealed class ToggleResumeGroupCommandDefinition
		: ToggleGroupCommandDefinitionBase
	{
		public const string CommandName = "ErrorList.ToggleResume";

		public ToggleResumeGroupCommandDefinition()
			: base(CommandName, TraceListItemType.Resume)
		{
		}
	};

	[CommandDefinition]
	public sealed class ToggleTransferGroupCommandDefinition
		: ToggleGroupCommandDefinitionBase
	{
		public const string CommandName = "ErrorList.ToggleTransfer";

		public ToggleTransferGroupCommandDefinition()
			: base(CommandName, TraceListItemType.Transfer)
		{
		}
	};
}
