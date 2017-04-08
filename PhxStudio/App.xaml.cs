using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhxStudio
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static RenderTargetBitmap AppIconBitmap { get; private set; }

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);

			RenderAppIconBitmap();
		}

		private void RenderAppIconBitmap()
		{
			if (AppIconBitmap != null)
				return;

			var grid = (Grid)FindResource("PhxLogoGrid");
			if (grid == null)
				throw new ArgumentException("Failed to find logo Grid", "PhxLogoGrid");

			var viewbox = new Viewbox();
			viewbox.Child = grid;
			viewbox.Measure(new Size(512, 512));
			viewbox.Arrange(new Rect(0, 0, 512, 512));
			viewbox.UpdateLayout();

			var viewbox_ps = PresentationSource.FromVisual(viewbox);
			double dpiX = 96.0, dpiY = 96.0;
			if (viewbox_ps != null)
			{
				dpiX *= viewbox_ps.CompositionTarget.TransformToDevice.M11;
				dpiY *= viewbox_ps.CompositionTarget.TransformToDevice.M22;
			}

			AppIconBitmap = new RenderTargetBitmap((int)viewbox.ActualWidth, (int)viewbox.ActualHeight, dpiX, dpiY, PixelFormats.Pbgra32);
			AppIconBitmap.Render(viewbox);
		}
	};
}
