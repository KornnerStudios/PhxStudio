using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace PhxStudio.ProtoData.Leaders
{
	using Modules.ProtoData;

	[Export(typeof(LeadersExplorerViewModel))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "Gemini view model activated by MEF composition.")]
	class LeadersExplorerViewModel
		: ProtoDataObjectExplorerViewModel
	{
		public LeadersExplorerViewModel()
		{
			DisplayName = "Leaders";
			LookupViewModel = IoC.Get<LeadersLookupViewModel>();
		}
	};

	[Export(typeof(LeadersLookupViewModel))]
	[Export(kExportContractName, typeof(IProtoDataObjectLookup))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "Gemini view model activated by MEF composition.")]
	class LeadersLookupViewModel
		: ProtoDataObjectLookupViewModel
	{
		public const string kExportContractName
			= nameof(KSoft.Phoenix.Phx.ProtoDataObjectSourceKind.Database)
			+ "."
			+ nameof(KSoft.Phoenix.Phx.DatabaseObjectKind.Leader)
			;

		public LeadersLookupViewModel()
			: base((int)KSoft.Phoenix.Phx.DatabaseObjectKind.Leader)
		{
			base.ObjectSource = new KSoft.Phoenix.Phx.ProtoDataObjectSource(
				KSoft.Phoenix.Phx.ProtoDataObjectSourceKind.Database,
				KSoft.Phoenix.Phx.BLeader.kXmlFileInfo);

			base.mObjectsArePreloaded = false;
		}
	};
}
