using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace PhxStudio.ProtoData.Leaders
{
	using Modules.ProtoData;

	[Export(typeof(LeadersExplorerViewModel))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	class LeadersExplorerViewModel
		: ProtoDataObjectExplorerViewModel
	{
		[ImportingConstructor]
		public LeadersExplorerViewModel(IEventAggregator eventAggregator)
			: base(eventAggregator, (int)KSoft.Phoenix.Phx.DatabaseObjectKind.Leader)
		{
			DisplayName = "Leaders";

			base.ObjectSource = new KSoft.Phoenix.Phx.ProtoDataObjectSource(
				KSoft.Phoenix.Phx.ProtoDataObjectSourceKind.Database,
				KSoft.Phoenix.Phx.BLeader.kXmlFileInfo);

			base.mObjectsArePreloaded = false;
		}
	};
}
