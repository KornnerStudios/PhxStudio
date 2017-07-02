using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace PhxStudio.ProtoData.Objects
{
	using Modules.ProtoData;

	[Export(typeof(ObjectsExplorerViewModel))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	class ObjectsExplorerViewModel
		: ProtoDataObjectExplorerViewModel
	{
		[ImportingConstructor]
		public ObjectsExplorerViewModel(IEventAggregator eventAggregator)
			: base(eventAggregator, (int)KSoft.Phoenix.Phx.DatabaseObjectKind.Object)
		{
			DisplayName = "Objects";

			base.ObjectSource = new KSoft.Phoenix.Phx.ProtoDataObjectSource(
				KSoft.Phoenix.Phx.ProtoDataObjectSourceKind.Database,
				KSoft.Phoenix.Phx.BProtoObject.kXmlFileInfo);
		}
	};
}
