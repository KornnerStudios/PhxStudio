using System.Threading.Tasks;
using Gemini.Framework.Commands;
using Gemini.Framework.Threading;

namespace PhxStudio.ProtoData.Civs.Commands
{
	[CommandHandler]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Activated by Gemini through CommandHandler MEF discovery.")]
	sealed class ViewCivsExplorerHandler
		: Modules.ProtoData.ProtoDataViewObjectExplorerCommandHandlerBase<ViewCivsExplorerDefinition>
	{
		public override Task Run(Command command)
		{
			Shell.ShowTool<CivsExplorerViewModel>();
			return TaskUtility.Completed;
		}
	};
}