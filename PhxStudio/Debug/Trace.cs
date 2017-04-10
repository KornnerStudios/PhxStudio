using Diag = System.Diagnostics;

namespace PhxStudio.Debug
{
	internal static class Trace
	{
		static Diag.TraceSource kPhxStudioSource
			;

		static Trace()
		{
			kPhxStudioSource = new		Diag.TraceSource("PhxStudio",				Diag.SourceLevels.All);
		}

		/// <summary>Tracer for the <see cref="PhxStudio"/> namespace</summary>
		public static Diag.TraceSource PhxStudio { get { return kPhxStudioSource; } }
	};
}
