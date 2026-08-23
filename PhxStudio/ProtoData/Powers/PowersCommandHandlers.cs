using System.Threading.Tasks;
using Gemini.Framework.Commands;
using Gemini.Framework.Threading;

namespace PhxStudio.ProtoData.Powers.Commands
{
	[CommandHandler]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Activated by Gemini through CommandHandler MEF discovery.")]
	sealed class ViewPowersExplorerHandler
		: Modules.ProtoData.ProtoDataViewObjectExplorerCommandHandlerBase<ViewPowersExplorerDefinition>
	{
		public override Task Run(Command command)
		{
			Shell.ShowTool<PowersExplorerViewModel>();
			return TaskUtility.Completed;
		}
	};
}