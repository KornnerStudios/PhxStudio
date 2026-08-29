using System.ComponentModel.Composition;
using System.Globalization;

namespace PhxStudio.Modules.ProtoData
{
	public abstract class ProtoDataObjectEditorViewModel<TProto, TExplorer>
		: PhxInspectors.PhxInspectorViewModel
		where TProto : class, KSoft.Collections.IListAutoIdObject
		where TExplorer : ProtoDataObjectExplorerViewModel
	{
		#region Imports
#pragma warning disable 649

		[Import] TExplorer mExplorer = null!;

#pragma warning restore 649

		protected TExplorer Explorer { get { return mExplorer; } }
		#endregion

		TProto? mProto;
		public TProto? Proto
		{
			get { return mProto; }
			set
			{
				if (this.SetField(ref mProto, value))
				{
					InspectableModel = null;
					OnProtoChanged();
				}
			}
		}

		protected TProto RequiredProto => Proto ?? throw new System.InvalidOperationException("A proto object must be selected before building its inspector.");

		protected virtual void OnProtoChanged()
		{
			var proto = Proto;
			if (proto is null)
			{
				DisplayName = string.Create(CultureInfo.CurrentCulture,
					$"Null.{typeof(TProto).Name}");
			}
			else
			{
				DisplayName = proto.Data;

				var builder = new PhxInspectors.InspectablePhxObjectBuilder();
				BuildProtoInspector(builder);
				if (builder.HasInspectors)
				{
					base.InspectableModel = builder.ToInspectableObject();
				}
			}
		}

		protected virtual void BuildProtoInspector(PhxInspectors.InspectablePhxObjectBuilder builder)
		{
		}

		protected void BuildInspector(PhxInspectors.InspectablePhxObjectBuilder builder
			, KSoft.Phoenix.Phx.DatabaseObjectUserInterfaceTextData? data)
		{
			System.ArgumentNullException.ThrowIfNull(builder);

			if (data == null)
				return;

			var group = new PhxInspectors.Inspectors.CollapsibleGroupBuilder();

			if (data != null)
			{
				if (data.HasNameID)
					group.WithObjectProperty(data, o => o.NameID);
				if (data.HasDisplayNameID)
					group.WithObjectProperty(data, o => o.DisplayNameID);
				if (data.HasDisplayName2ID)
					group.WithObjectProperty(data, o => o.DisplayName2ID);
				if (data.HasDescriptionID)
					group.WithObjectProperty(data, o => o.DescriptionID);
				if (data.HasLongDescriptionID)
					group.WithObjectProperty(data, o => o.LongDescriptionID);
				if (data.HasPrereqTextID)
					group.WithObjectProperty(data, o => o.PrereqTextID);
				if (data.HasStatsNameID)
					group.WithObjectProperty(data, o => o.StatsNameID);
				if (data.HasRoleTextID)
					group.WithObjectProperty(data, o => o.RoleTextID);
				if (data.HasRolloverTextID)
					group.WithObjectProperty(data, o => o.RolloverTextID);
				if (data.HasEnemyRolloverTextID)
					group.WithObjectProperty(data, o => o.EnemyRolloverTextID);

				// #TODO_PHXSTUDIO GaiaRolloverText

				if (data.HasChooseTextID)
					group.WithObjectProperty(data, o => o.ChooseTextID);
			}

			BuildInspectorForUserInterfaceText(group);

			if (group.HasInspectors)
				builder.WithCollapsibleGroup("UI Text", group);
		}

		protected virtual void BuildInspectorForUserInterfaceText(PhxInspectors.Inspectors.CollapsibleGroupBuilder group)
		{
		}
	};
}
