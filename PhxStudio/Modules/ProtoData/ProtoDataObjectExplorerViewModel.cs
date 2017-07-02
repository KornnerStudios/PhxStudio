using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;

namespace PhxStudio.Modules.ProtoData
{
	public interface IProtoDataObjectExplorerTool
		: ITool
	{
	};

	public abstract class ProtoDataObjectExplorerViewModel
		: Tool
		, IProtoDataObjectExplorerTool
		, IHandle<Project.ProjectEngineUnloadedEventArgs>
		, IHandle<Project.ProjectEnginePreloadedEventArgs>
		, IHandle<Project.ProjectEngineLoadedEventArgs>
	{
		#region Imports
#pragma warning disable 649

		IEventAggregator mEventAggregator;

#pragma warning restore 649
		#endregion

		public override PaneLocation PreferredLocation { get { return PaneLocation.Right; } }

		public override double PreferredWidth { get { return 150; } }

		KSoft.Phoenix.Phx.ProtoDataObjectSource mObjectSource;
		public KSoft.Phoenix.Phx.ProtoDataObjectSource ObjectSource
		{
			get { return mObjectSource; }
			protected set { this.SetField(ref mObjectSource, value); }
		}

		public int SourceObjectDatabaseKindId { get; private set; }

		KSoft.Phoenix.Phx.ProtoDataObjectDatabase mSourceObjectDatabase;
		public KSoft.Phoenix.Phx.ProtoDataObjectDatabase SourceObjectDatabase
		{
			get { return mSourceObjectDatabase; }
			protected set
			{
				if (this.SetField(ref mSourceObjectDatabase, value))
				{
					SetupSourceObjectDatabaseCollection();
				}
			}
		}

		object mSourceObjectDatabaseCollection;
		public object SourceObjectDatabaseCollection
		{
			get { return mSourceObjectDatabaseCollection; }
			private set { this.SetField(ref mSourceObjectDatabaseCollection, value); }
		}

		string mSourceObjectDatabaseCollectionFilter;
		public string SourceObjectDatabaseCollectionFilter
		{
			get { return mSourceObjectDatabaseCollectionFilter; }
			set { this.SetFieldObj(ref mSourceObjectDatabaseCollectionFilter, value); }
		}

		ObservableCollection<string> mSourceObjectDatabaseUndefinedMembers;
		public ObservableCollection<string> SourceObjectDatabaseUndefinedMembers
		{
			get { return mSourceObjectDatabaseUndefinedMembers; }
			private set
			{
				if (this.SetField(ref mSourceObjectDatabaseUndefinedMembers, value))
				{
					this.NotifyOfPropertyChange(nameof(HasSourceObjectDatabaseUndefinedMembers));
				}
			}
		}

		public bool HasSourceObjectDatabaseUndefinedMembers
		{
			get { return SourceObjectDatabaseUndefinedMembers != null && SourceObjectDatabaseUndefinedMembers.Count > 0; }
		}

		// #HACK Civs and Leaders are not currently preloaded and need this set
		protected bool mObjectsArePreloaded = true;

		protected ProtoDataObjectExplorerViewModel(IEventAggregator eventAggregator, int sourceObjectDatabaseKindId)
		{
			mEventAggregator = eventAggregator;
			mEventAggregator.Subscribe(this);

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
			SourceObjectDatabaseUndefinedMembers = list.UndefinedInterface.UndefinedMembers;
		}

		public void OnFilterSourceObjectDatabaseCollection(object sender, System.Windows.Data.FilterEventArgs e)
		{
			string filter = SourceObjectDatabaseCollectionFilter;
			e.Accepted = false;

			if (string.IsNullOrEmpty(filter))
			{
				e.Accepted = true;
				return;
			}
			else if (e.Item == null)
			{
				return;
			}

			var item = e.Item as KSoft.Collections.IListAutoIdObject;
			if (item != null)
			{
				e.Accepted = item.Data != null && item.Data.Contains(filter);
				return;
			}

			var itemAsStr = e.Item as string;
			if (itemAsStr != null)
			{
				e.Accepted = itemAsStr.Contains(filter);
				return;
			}
		}

		public bool IsSourceObjectDatabaseCollectionItemFiltered(object obj)
		{
			string filter = SourceObjectDatabaseCollectionFilter;
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
		void IHandle<Project.ProjectEngineUnloadedEventArgs>.Handle(Project.ProjectEngineUnloadedEventArgs message)
		{
			OnProjectEngineUnloaded(message);
		}
		protected virtual void OnProjectEngineUnloaded(Project.ProjectEngineUnloadedEventArgs message)
		{
			SourceObjectDatabase = null;
		}
		#endregion

		#region ProjectEnginePreloadedEventArgs
		void IHandle<Project.ProjectEnginePreloadedEventArgs>.Handle(Project.ProjectEnginePreloadedEventArgs message)
		{
			OnProjectEnginePreloaded(message);
		}
		protected virtual void OnProjectEnginePreloaded(Project.ProjectEnginePreloadedEventArgs message)
		{
			if (mObjectsArePreloaded == false)
				return;

			var engine = message.Engine;

			SourceObjectDatabase = ObjectSource.GetObjectDatabase(engine);
		}
		#endregion

		#region ProjectEngineLoadedEventArgs
		void IHandle<Project.ProjectEngineLoadedEventArgs>.Handle(Project.ProjectEngineLoadedEventArgs message)
		{
			OnProjectEngineLoaded(message);
		}
		protected virtual void OnProjectEngineLoaded(Project.ProjectEngineLoadedEventArgs message)
		{
			if (mObjectsArePreloaded == true)
				return;

			var engine = message.Engine;

			SourceObjectDatabase = ObjectSource.GetObjectDatabase(engine);
		}
		#endregion
	};

	abstract class ProtoDataViewObjectExplorerCommandHandlerBase<TCommandDefinition>
		: CommandHandlerBase<TCommandDefinition>
		where TCommandDefinition : CommandDefinition
	{
#pragma warning disable 649
		[Import] IShell mShell;
		[Import] Project.IProjectService mProjectService;
#pragma warning restore 649

		protected IShell Shell { get { return mShell; } }

		public override void Update(Command command)
		{
			base.Update(command);

			var engine = mProjectService.CurrentProject.Model.Engine;

			command.Enabled = engine != null && engine.HasAlreadyPreloaded;
		}
	};
}