using System.Threading.Tasks;
using Gemini.Framework.Commands;
using Gemini.Framework.Threading;

namespace PhxStudio.ProtoData.Squads.Commands
{
	[CommandHandler]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Activated by Gemini through CommandHandler MEF discovery.")]
	sealed class ViewSquadsExplorerHandler
		: Modules.ProtoData.ProtoDataViewObjectExplorerCommandHandlerBase<ViewSquadsExplorerDefinition>
	{
		public override Task Run(Command command)
		{
			Shell.ShowTool<SquadsExplorerViewModel>();
			return TaskUtility.Completed;
		}
	};
}