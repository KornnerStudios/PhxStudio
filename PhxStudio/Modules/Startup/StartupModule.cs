using System.ComponentModel.Composition;
using Gemini.Framework;
using Gemini.Framework.Menus;

namespace PhxStudio.Modules.Startup
{
	[Export(typeof(IModule))]
	sealed class StartupModule : ModuleBase
	{
		public override void Initialize()
		{
			MainWindow.Title = "PhxStudio";
			MainWindow.Icon = App.AppIconBitmap;
		}
	};
}
