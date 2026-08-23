using System.Threading.Tasks;
using Gemini.Framework.Commands;
using Gemini.Framework.Threading;

namespace PhxStudio.ProtoData.Leaders.Commands
{
	[CommandHandler]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Activated by Gemini through CommandHandler MEF discovery.")]
	sealed class ViewLeadersExplorerHandler
		: Modules.ProtoData.ProtoDataViewObjectExplorerCommandHandlerBase<ViewLeadersExplorerDefinition>
	{
		public override Task Run(Command command)
		{
			Shell.ShowTool<LeadersExplorerViewModel>();
			return TaskUtility.Completed;
		}
	};
}