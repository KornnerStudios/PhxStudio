using Diag = System.Diagnostics;

namespace PhxStudio.Debug
{
	internal static class Trace
	{
		/// <summary>Tracer for the <see cref="PhxStudio"/> namespace</summary>
		public static Diag.TraceSource PhxStudio { get; } = new		Diag.TraceSource("PhxStudio",	Diag.SourceLevels.All);
	};
}
