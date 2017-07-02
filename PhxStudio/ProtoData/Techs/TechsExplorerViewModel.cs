using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace PhxStudio.ProtoData.Techs
{
	using Modules.ProtoData;

	[Export(typeof(TechsExplorerViewModel))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	class TechsExplorerViewModel
		: ProtoDataObjectExplorerViewModel
	{
		[ImportingConstructor]
		public TechsExplorerViewModel(IEventAggregator eventAggregator)
			: base(eventAggregator, (int)KSoft.Phoenix.Phx.DatabaseObjectKind.Tech)
		{
			DisplayName = "Techs";

			base.ObjectSource = new KSoft.Phoenix.Phx.ProtoDataObjectSource(
				KSoft.Phoenix.Phx.ProtoDataObjectSourceKind.Database,
				KSoft.Phoenix.Phx.BProtoTech.kXmlFileInfo);
		}
	};
}
