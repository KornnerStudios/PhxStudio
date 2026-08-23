using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design",
	"CA2211:Non-constant fields should not be visible",
	Justification = "Gemini command, menu, and toolbar definitions are intentionally public static composition instances")]
[assembly: SuppressMessage("Reliability",
	"CA2007:Consider calling ConfigureAwait on the awaited task",
	Justification = "WPF and Gemini command continuations intentionally resume on the UI synchronization context")]
