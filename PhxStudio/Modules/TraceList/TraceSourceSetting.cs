using System;
using System.Configuration;
using System.Diagnostics;
using Caliburn.Micro;

namespace PhxStudio.Modules.TraceList
{
	[SettingsSerializeAs(SettingsSerializeAs.Xml)]
	public sealed partial class TraceSourceSetting
		: PropertyChangedBase
	{
		string mName = string.Empty;
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mName))]
		public partial string Name { get; set; }

		TraceLevel mLevel = TraceLevel.Verbose;
		[DefaultSettingValue(nameof(TraceLevel.Verbose))]
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mLevel))]
		public partial TraceLevel Level { get; set; }

		bool mDisabled;
		[DefaultSettingValue("false")]
		[KSoft.PropertyChanged.SourceGeneration.GeneratedPropertyChanged(BackingField = nameof(mDisabled))]
		public partial bool Disabled { get; set; }

		public TraceSourceSetting Clone()
		{
			var copy = new TraceSourceSetting();
			copy.mName = this.Name;
			copy.mLevel = this.Level;
			copy.mDisabled = this.Disabled;
			return copy;
		}

		public void Sync(TraceSourceSetting src)
		{
			ArgumentNullException.ThrowIfNull(src);

			this.Name = src.Name;
			this.Level = src.Level;
			this.Disabled = src.Disabled;
		}
	};
}
