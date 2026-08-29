using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design",
	"CA2211:Non-constant fields should not be visible",
	Justification = "Gemini command, menu, and toolbar definitions are intentionally public static composition instances")]
[assembly: SuppressMessage("Reliability",
	"CA2007:Consider calling ConfigureAwait on the awaited task",
	Justification = "WPF and Gemini command continuations intentionally resume on the UI synchronization context")]
[assembly: SuppressMessage("Design",
	"CA1062:Validate arguments of public methods",
	Scope = "member",
	Target = "~M:PhxStudio.Modules.Main.Commands.PhxOpenFileCommandHandler.Update(Gemini.Framework.Commands.Command)",
	Justification = "The Gemini command framework supplies a non-null command to this override.")]
[assembly: SuppressMessage("Design",
	"CA1062:Validate arguments of public methods",
	Scope = "member",
	Target = "~M:PhxStudio.Modules.Main.Commands.PhxOpenFileCommandHandler.Run(Gemini.Framework.Commands.Command)",
	Justification = "The Gemini command framework supplies a non-null command to this override.")]
[assembly: SuppressMessage("Design",
	"CA1062:Validate arguments of public methods",
	Scope = "member",
	Target = "~M:PhxStudio.Modules.ProtoData.ProtoDataObjectExplorerViewModel.OnMouseDown(System.Object,KSoft.Collections.IListAutoIdObject,System.Windows.Input.MouseButtonEventArgs)",
	Justification = "The Caliburn.Micro event convention supplies non-null event arguments.")]
[assembly: SuppressMessage("Design",
	"CA1062:Validate arguments of public methods",
	Scope = "member",
	Target = "~M:PhxStudio.Modules.ProtoData.ProtoDataObjectLookupViewModel.OnProjectEnginePreloaded(PhxStudio.Modules.Project.ProjectEnginePreloadedEventArgs)",
	Justification = "The Caliburn.Micro IHandle framework supplies a non-null message.")]
[assembly: SuppressMessage("Design",
	"CA1062:Validate arguments of public methods",
	Scope = "member",
	Target = "~M:PhxStudio.Modules.ProtoData.ProtoDataObjectLookupViewModel.OnProjectEngineLoaded(PhxStudio.Modules.Project.ProjectEngineLoadedEventArgs)",
	Justification = "The Caliburn.Micro IHandle framework supplies a non-null message.")]
