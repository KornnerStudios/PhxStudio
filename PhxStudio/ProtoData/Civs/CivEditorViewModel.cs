using System.ComponentModel.Composition;

namespace PhxStudio.ProtoData.Civs
{
	using Modules.ProtoData;
	using Modules.PhxInspectors;
	using Modules.PhxInspectors.Inspectors;

	[Export(typeof(CivEditorViewModel))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	class CivEditorViewModel
		: ProtoDataObjectEditorViewModel<KSoft.Phoenix.Phx.BCiv, CivsExplorerViewModel>
	{
		protected override void BuildProtoInspector(InspectablePhxObjectBuilder builder)
		{
			base.BuildProtoInspector(builder);

			var proto = RequiredProto;
			base.BuildInspector(builder, proto.UserInterfaceTextData);

			builder
				.WithCheckBoxEditor(proto, o => o.IsExcludedFromAlpha)
				.WithCheckBoxEditor(proto, o => o.PowerFromHero)
				.WithObjectProperty(proto, o => o.TechID)
				.WithObjectProperty(proto, o => o.CommandAckObjectID)
				.WithObjectProperty(proto, o => o.RallyPointObjectID)
				.WithObjectProperty(proto, o => o.LocalRallyPointObjectID)
				.WithObjectProperty(proto, o => o.TransportObjectID)
				.WithObjectProperty(proto, o => o.TransportTriggerObjectID)
				.WithObjectProperty(proto, o => o.HullExpansionRadius)
				.WithObjectProperty(proto, o => o.TerrainPushOffRadius)
				.WithObjectProperty(proto, o => o.BuildingMagnetRange)
				.WithObjectProperty(proto, o => o.SoundBank)
				.WithObjectProperty(proto, o => o.UIControlBackground)
				;
		}

		protected override void BuildInspectorForUserInterfaceText(CollapsibleGroupBuilder group)
		{
			base.BuildInspectorForUserInterfaceText(group);

			group
				.WithObjectProperty(RequiredProto, o => o.LeaderMenuNameID);
		}
	};
}
