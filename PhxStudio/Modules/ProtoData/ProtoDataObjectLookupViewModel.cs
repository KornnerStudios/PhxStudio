using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;

namespace PhxStudio.Modules.ProtoData
{
	public interface IProtoDataObjectLookup
		: System.ComponentModel.INotifyPropertyChanged
	{
		KSoft.Phoenix.Phx.ProtoDataObjectSource ObjectSource { get; }

		int SourceObjectDatabaseKindId { get; }

		KSoft.Phoenix.Phx.ProtoDataObjectDatabase? SourceObjectDatabase { get; }

		object? SourceObjectDatabaseCollection { get; }
		string? SourceObjectDatabaseCollectionFilter { get; }
		ObservableCollection<string>? SourceObjectDatabaseUndefinedMembers { get; }
		bool HasSourceObjectDatabaseUndefinedMembers { get; }
	};

	public abstract partial class ProtoDataObjectLookupViewModel
		: KSoft.ObjectModel.BasicViewModel
		, IProtoDataObjectLookup
		, IHandle<Project.ProjectEngineUnloadedEventArgs>
		, IHandle<Project.ProjectEnginePreloadedEventArgs>
		, IHandle<Project.ProjectEngineLoadedEventArgs>
	{
		#region Imports
#pragma warning disable 649

		[Import] IShell mShell = null!;
		IEventAggregator mEventAggregator;

#pragma warning restore 649

		protected IShell Shell { get { return mShell; } }
		#endregion

		KSoft.Phoenix.Phx.ProtoDataObjectSource mObjectSource = null!;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mObjectSource))]
		public partial KSoft.Phoenix.Phx.ProtoDataObjectSource ObjectSource { get; protected set; }

		public int SourceObjectDatabaseKindId { get; private set; }

		KSoft.Phoenix.Phx.ProtoDataObjectDatabase? mSourceObjectDatabase;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChangedEventArgs]
		public KSoft.Phoenix.Phx.ProtoDataObjectDatabase? SourceObjectDatabase
		{
			get { return mSourceObjectDatabase; }
			protected set
			{
				if (this.SetField(ref mSourceObjectDatabase, value, kSourceObjectDatabaseChangedEventArgs))
				{
					SetupSourceObjectDatabaseCollection();
				}
			}
		}

		object? mSourceObjectDatabaseCollection;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mSourceObjectDatabaseCollection))]
		public partial object? SourceObjectDatabaseCollection { get; private set; }

		string? mSourceObjectDatabaseCollectionFilter;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mSourceObjectDatabaseCollectionFilter))]
		public partial string? SourceObjectDatabaseCollectionFilter { get; set; }

		ObservableCollection<string>? mSourceObjectDatabaseUndefinedMembers;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChangedEventArgs]
		public ObservableCollection<string>? SourceObjectDatabaseUndefinedMembers
		{
			get { return mSourceObjectDatabaseUndefinedMembers; }
			private set
			{
				if (this.SetField(ref mSourceObjectDatabaseUndefinedMembers, value, kSourceObjectDatabaseUndefinedMembersChangedEventArgs))
				{
					this.OnPropertyChanged(nameof(HasSourceObjectDatabaseUndefinedMembers));
				}
			}
		}

		public bool HasSourceObjectDatabaseUndefinedMembers => SourceObjectDatabaseUndefinedMembers != null && SourceObjectDatabaseUndefinedMembers.Count > 0;

		// #HACK_PHXSTUDIO Civs and Leaders are not currently preloaded and need this set
		protected bool mObjectsArePreloaded = false;//true;

		protected ProtoDataObjectLookupViewModel(int sourceObjectDatabaseKindId)
		{
			mEventAggregator = IoC.Get<IEventAggregator>();
			mEventAggregator.SubscribeOnPublishedThread(this);

			SourceObjectDatabaseKindId = sourceObjectDatabaseKindId;
		}

		private void SetupSourceObjectDatabaseCollection()
		{
			if (SourceObjectDatabase == null)
			{
				SourceObjectDatabaseCollection = null;
				SourceObjectDatabaseUndefinedMembers = null;
				return;
			}

			var provider = SourceObjectDatabase.Provider;
			var list = provider.GetNamesInterface(SourceObjectDatabaseKindId);
			SourceObjectDatabaseCollection = list.UnderlyingObjectsCollection;
		}

		private void SetupSourceObjectDatabaseUndefinedMembers()
		{
			if (SourceObjectDatabase == null)
			{
				return;
			}

			var provider = SourceObjectDatabase.Provider;
			var list = provider.GetNamesInterface(SourceObjectDatabaseKindId);
			SourceObjectDatabaseUndefinedMembers = list.UndefinedInterface.UndefinedMembers;
		}

		public bool IsSourceObjectDatabaseCollectionItemFiltered(object? obj)
		{
			string? filter = SourceObjectDatabaseCollectionFilter;
			if (string.IsNullOrEmpty(filter))
			{
				return true;
			}
			else if (obj == null)
			{
				return false;
			}

			var item = obj as KSoft.Collections.IListAutoIdObject;
			if (item != null)
			{
				return item.Data != null
					//&& item.Data.Contains(filter)
					&& item.Data.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0
					;
			}

			var itemAsStr = obj as string;
			if (itemAsStr != null)
			{
				return
					//itemAsStr.Contains(filter)
					itemAsStr.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0
					;
			}

			return true;
		}

		#region ProjectEngineUnloadedEventArgs
		Task IHandle<Project.ProjectEngineUnloadedEventArgs>.HandleAsync(Project.ProjectEngineUnloadedEventArgs message, CancellationToken cancellationToken)
			=> Task.Run(() => OnProjectEngineUnloaded(message), cancellationToken);

		protected virtual void OnProjectEngineUnloaded(Project.ProjectEngineUnloadedEventArgs message) => SourceObjectDatabase = null;
		#endregion

		#region ProjectEnginePreloadedEventArgs
		Task IHandle<Project.ProjectEnginePreloadedEventArgs>.HandleAsync(Project.ProjectEnginePreloadedEventArgs message, CancellationToken cancellationToken)
			=> Task.Run(() => OnProjectEnginePreloaded(message), cancellationToken);

		protected virtual void OnProjectEnginePreloaded(Project.ProjectEnginePreloadedEventArgs message)
		{
			if (mObjectsArePreloaded)
			{
				var engine = message.Engine;

				SourceObjectDatabase = ObjectSource.GetObjectDatabase(engine);
			}
		}
		#endregion

		#region ProjectEngineLoadedEventArgs
		Task IHandle<Project.ProjectEngineLoadedEventArgs>.HandleAsync(Project.ProjectEngineLoadedEventArgs message, CancellationToken cancellationToken)
			=> Task.Run(() => OnProjectEngineLoaded(message), cancellationToken);

		protected virtual void OnProjectEngineLoaded(Project.ProjectEngineLoadedEventArgs message)
		{
			if (mObjectsArePreloaded == false)
			{
				var engine = message.Engine;

				SourceObjectDatabase = ObjectSource.GetObjectDatabase(engine);
			}

			// This needs to be setup after Load because the undefined members are modified during load, so there would be a data binding error if we set up after Preload
			SetupSourceObjectDatabaseUndefinedMembers();
		}
		#endregion
	};
}
