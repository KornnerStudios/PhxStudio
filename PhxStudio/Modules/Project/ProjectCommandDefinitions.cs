﻿using Gemini.Framework.Commands;

namespace PhxStudio.Modules.Project.Commands
{
	[CommandDefinition]
	public sealed class FileNewProjectCommandDefinition
		: CommandDefinition
	{
		public const string CommandName = "File.NewProject";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "New Project";
		} }

		public override string ToolTip { get {
			return "Create a new project";
		} }
	};

	[CommandDefinition]
	public sealed class FileOpenProjectCommandDefinition
		: CommandDefinition
	{
		public const string CommandName = "File.OpenProject";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "Open Project";
		} }

		public override string ToolTip { get {
			return "Open an existing project";
		} }
	};

	[CommandDefinition]
	public sealed class FileSaveProjectCommandDefinition
		: CommandDefinition
	{
		public const string CommandName = "File.SaveProject";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "Save Project";
		} }

		public override string ToolTip { get {
			return "Save current project";
		} }
	};

	[CommandDefinition]
	public sealed class FileSaveProjectAsCommandDefinition
		: CommandDefinition
	{
		public const string CommandName = "File.SaveProjectAs";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "Save Project As...";
		} }

		public override string ToolTip { get {
			return "Save current project to a new file";
		} }
	};

	[CommandDefinition]
	public sealed class ProjectEnginePreloadCommandDefinition
		: CommandDefinition
	{
		public const string CommandName = "Project.PreloadEngine";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "Preload Engine System";
		} }

		public override string ToolTip { get {
			return "Preloads core ProtoData used by the engine";
		} }
	};

	[CommandDefinition]
	public sealed class ProjectEngineLoadCommandDefinition
		: CommandDefinition
	{
		public const string CommandName = "Project.LoadEngine";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "Load Engine System";
		} }

		public override string ToolTip { get {
			return "Loads core ProtoData used by the engine";
		} }
	};
}