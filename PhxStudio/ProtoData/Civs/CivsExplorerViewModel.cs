using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace PhxStudio.ProtoData.Civs
{
	using Modules.ProtoData;

	[Export(typeof(CivsExplorerViewModel))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	class CivsExplorerViewModel
		: ProtoDataObjectExplorerViewModel
	{
		[ImportingConstructor]
		public CivsExplorerViewModel(IEventAggregator eventAggregator)
			: base(eventAggregator, (int)KSoft.Phoenix.Phx.DatabaseObjectKind.Civ)
		{
			DisplayName = "Civs";

			base.ObjectSource = new KSoft.Phoenix.Phx.ProtoDataObjectSource(
				KSoft.Phoenix.Phx.ProtoDataObjectSourceKind.Database,
				KSoft.Phoenix.Phx.BCiv.kXmlFileInfo);

			base.mObjectsArePreloaded = false;
		}

		protected override void OnOpenObject(object obj)
		{
			var vm = new CivEditorViewModel();
			vm.Proto = (KSoft.Phoenix.Phx.BCiv)obj;

			Shell.OpenDocument(vm);
		}
	};
}
