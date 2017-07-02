using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace PhxStudio.ProtoData.Squads
{
	using Modules.ProtoData;

	[Export(typeof(SquadsExplorerViewModel))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	class SquadsExplorerViewModel
		: ProtoDataObjectExplorerViewModel
	{
		[ImportingConstructor]
		public SquadsExplorerViewModel(IEventAggregator eventAggregator)
			: base(eventAggregator, (int)KSoft.Phoenix.Phx.DatabaseObjectKind.Squad)
		{
			DisplayName = "Squads";

			base.ObjectSource = new KSoft.Phoenix.Phx.ProtoDataObjectSource(
				KSoft.Phoenix.Phx.ProtoDataObjectSourceKind.Database,
				KSoft.Phoenix.Phx.BProtoSquad.kXmlFileInfo);
		}
	};
}
