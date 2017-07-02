using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace PhxStudio.ProtoData.Powers
{
	using Modules.ProtoData;

	[Export(typeof(PowersExplorerViewModel))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	class PowersExplorerViewModel
		: ProtoDataObjectExplorerViewModel
	{
		[ImportingConstructor]
		public PowersExplorerViewModel(IEventAggregator eventAggregator)
			: base(eventAggregator, (int)KSoft.Phoenix.Phx.DatabaseObjectKind.Power)
		{
			DisplayName = "Powers";

			base.ObjectSource = new KSoft.Phoenix.Phx.ProtoDataObjectSource(
				KSoft.Phoenix.Phx.ProtoDataObjectSourceKind.Database,
				KSoft.Phoenix.Phx.BProtoPower.kXmlFileInfo);
		}
	};
}
