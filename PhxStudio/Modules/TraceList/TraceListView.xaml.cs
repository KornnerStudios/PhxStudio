﻿using System.Windows.Controls;
using System.Windows.Input;

namespace PhxStudio.Modules.TraceList
{
	/// <summary>
	/// Interaction logic for TraceListView.xaml
	/// </summary>
	public partial class TraceListView : UserControl
	{
		public TraceListView()
		{
			InitializeComponent();
		}

		private void OnDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var dataGrid = (DataGrid)sender;
			if (dataGrid.SelectedItems == null || dataGrid.SelectedItems.Count != 1)
				return;

			var traceListItem = (TraceListItem)dataGrid.SelectedItem;
			if (traceListItem.OnClick != null)
				traceListItem.OnClick();
		}

		private void OnDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var dataGrid = (DataGrid)sender;
			if (dataGrid.SelectedItems == null || dataGrid.SelectedItems.Count != 1)
				return;

			var traceList = (TraceListViewModel)DataContext;
			var traceListItem = (TraceListItem)dataGrid.SelectedItem;
			traceList.OnSelectedItemChanged(traceListItem);
		}
	};
}
