using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.Inspector;
using KSoft;

namespace PhxStudio.Modules.TraceList
{
	// #TODO_PHXSTUDIO Filtering should probably be done with a CollectionViewSoruce instead

	[Export(typeof(ITraceList))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public sealed partial class TraceListViewModel
		: Tool
		, ITraceList
	{
		public override PaneLocation PreferredLocation => PaneLocation.Bottom;

		#region Imports
#pragma warning disable 649

		[Import] IInspectorTool? mInspectorTool;

#pragma warning restore 649
		#endregion

		private int mItemNumber;

		BindableCollection<TraceListItem> mItems;
		public IObservableCollection<TraceListItem> Items => mItems;

		public IEnumerable<TraceListItem> FilteredItems { get {
			if (ShowEverything)
				return mItems;

			var items =
				from item in mItems
				where (ShowCritical		&& item.ItemType == TraceListItemType.Critical)
					||(ShowError		&& item.ItemType == TraceListItemType.Error)
					||(ShowWarning		&& item.ItemType == TraceListItemType.Warning)
					||(ShowInformation	&& item.ItemType == TraceListItemType.Information)
					||(ShowVerbose		&& item.ItemType == TraceListItemType.Verbose)
					||(ShowStart		&& item.ItemType == TraceListItemType.Start)
					||(ShowStop			&& item.ItemType == TraceListItemType.Stop)
					||(ShowSuspend		&& item.ItemType == TraceListItemType.Suspend)
					||(ShowResume		&& item.ItemType == TraceListItemType.Resume)
					||(ShowTransfer		&& item.ItemType == TraceListItemType.Transfer)
				select item;
			return items;
		} }

		#region PauseTracing
		bool mPauseTracing;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mPauseTracing))]
		public partial bool PauseTracing { get; set; }
		#endregion

		#region TotalNumberOfTraces
		int mTotalNumberOfTraces;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mTotalNumberOfTraces))]
		public partial int TotalNumberOfTraces { get; set; }
		#endregion

		private bool ShowEverything
			=> ShowCritical
			&& ShowError
			&& ShowWarning
			&& ShowInformation
			&& ShowVerbose
			&& ShowStart
			&& ShowStop
			&& ShowSuspend
			&& ShowResume
			&& ShowTransfer;

		#region ShowCritical
		bool mShowCritical = true;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mShowCritical),
			DependentProperties = new[] { nameof(FilteredItems) })]
		public partial bool ShowCritical { get; set; }
		#endregion

		#region ShowError
		bool mShowError = true;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mShowError),
			DependentProperties = new[] { nameof(FilteredItems) })]
		public partial bool ShowError { get; set; }
		#endregion

		#region ShowWarning
		bool mShowWarning = true;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mShowWarning),
			DependentProperties = new[] { nameof(FilteredItems) })]
		public partial bool ShowWarning { get; set; }
		#endregion

		#region ShowInformation
		bool mShowInformation = true;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mShowInformation),
			DependentProperties = new[] { nameof(FilteredItems) })]
		public partial bool ShowInformation { get; set; }
		#endregion

		#region ShowVerbose
		bool mShowVerbose = true;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mShowVerbose),
			DependentProperties = new[] { nameof(FilteredItems) })]
		public partial bool ShowVerbose { get; set; }
		#endregion

		#region ShowStart
		bool mShowStart = true;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mShowStart),
			DependentProperties = new[] { nameof(FilteredItems) })]
		public partial bool ShowStart { get; set; }
		#endregion

		#region ShowStop
		bool mShowStop = true;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mShowStop),
			DependentProperties = new[] { nameof(FilteredItems) })]
		public partial bool ShowStop { get; set; }
		#endregion

		#region ShowSuspend
		bool mShowSuspend = true;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mShowSuspend),
			DependentProperties = new[] { nameof(FilteredItems) })]
		public partial bool ShowSuspend { get; set; }
		#endregion

		#region ShowResume
		bool mShowResume = true;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mShowResume),
			DependentProperties = new[] { nameof(FilteredItems) })]
		public partial bool ShowResume { get; set; }
		#endregion

		#region ShowTransfer
		bool mShowTransfer = true;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(
			BackingField = nameof(mShowTransfer),
			DependentProperties = new[] { nameof(FilteredItems) })]
		public partial bool ShowTransfer { get; set; }
		#endregion

		#region TailTraces
		bool mTailTraces = true;
		[Description("When enabled, UI will snap to new traces as they come in")]
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mTailTraces))]
		public partial bool TailTraces { get; set; }
		#endregion

		public TraceListViewModel()
		{
			DisplayName = "Trace List";

			ToolBarDefinition = ToolBarDefinitions.TraceListToolBar;

			mItems = new BindableCollection<TraceListItem>();
			mItems.CollectionChanged += OnItemsCollectionChanged;
		}

		private void OnItemsCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			NotifyOfPropertyChange(nameof(FilteredItems));
		}

		public void AddItem(TraceListItemType type, long timeStamp, string? sourceName, string? message
			, object?[]? data = null, System.Action? onClick = null)
		{
			if (PauseTracing)
			{
				// #NOTE_PHXSTUDIO I think I still want to track the total number of would-be traces
				++mItemNumber;
				TotalNumberOfTraces = mItemNumber;
				return;
			}

			var settings = PhxStudio.Properties.Settings.Default;
			var traceSettings = settings.TraceSourceOptions;
			if (traceSettings != null)
			{
				if (traceSettings.MaxTraceListItems.IsNotNone())
				{
					int surplus_count = Items.Count;
					surplus_count += 1; // we're adding one
					surplus_count -= traceSettings.MaxTraceListItems;
					while (surplus_count-- > 0)
					{
						Items.RemoveAt(0);
					}
				}
			}

			var item = new TraceListItem
			{
				ItemType = type,
				Number = ++mItemNumber,
				TimeStamp = timeStamp,
				SourceName = sourceName,
				Message = message,
				Data = data ?? KSoft.Util.EmptyArray,
				OnClick = onClick,
			};

			Items.Add(item);
			TotalNumberOfTraces = mItemNumber;
		}

		public void ClearAll() => Items.Clear();

		public void OnSelectedItemChanged(TraceListItem? selectedItem)
		{
			if (mInspectorTool == null)
				return;

			if (selectedItem == null)
			{
				mInspectorTool.SelectedObject = null;
				return;
			}

			var item_inspector = new InspectableObjectBuilder()
				.WithObjectProperties(selectedItem, TraceListItemPropertyFilter);
			if (selectedItem.HasData)
			{
				item_inspector
					.WithEditor(selectedItem, x => x.Data, new Inspectors.TraceDataEditorViewModel());
			}

			mInspectorTool.SelectedObject = item_inspector.ToInspectableObject();
		}

		private static bool TraceListItemPropertyFilter(PropertyDescriptor pd)
		{
			switch (pd.Name)
			{
				case nameof(TraceListItem.SourceName):
				case nameof(TraceListItem.Message):
					return true;

				default:
					return false;
			}
		}
	};
}
