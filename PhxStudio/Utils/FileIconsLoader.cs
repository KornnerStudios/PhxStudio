using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhxStudio.Utils
{
	static class FileIconsLoader
	{
		const uint SHGFI_ICON = 0x100;
		const uint SHGFI_SMALLICON = 0x1;
		const uint SHGFI_LARGEICON = 0x0;

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		struct ShFileInfo
		{
			// Handle to the icon representing the file
			public IntPtr hIcon;
			// Index of the icon within the image list
			public int iIcon;
			// Various attributes of the file
			public uint dwAttributes;
			// Path to the file
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string szDisplayName;
			// File type
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		};

		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Shell32.dll", EntryPoint = "SHGetFileInfoW", CharSet = CharSet.Unicode, ExactSpelling = true)]
		static extern IntPtr SHGetFileInfo([MarshalAs(UnmanagedType.LPWStr)] string pszPath, uint dwFileAttributes, ref ShFileInfo psfi,
			int cbFileInfo, uint uFlags);

		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("User32.dll", ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool DestroyIcon(IntPtr hIcon);

		static ImageSource? GetIcon(string fileName, uint flags)
		{
			var shinfo = new ShFileInfo();
			var result = SHGetFileInfo(fileName, 0, ref shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON | flags);
			if (result == IntPtr.Zero || shinfo.hIcon == IntPtr.Zero)
				return null;

			try
			{
				using (var icon = System.Drawing.Icon.FromHandle(shinfo.hIcon))
				{
					var img = Imaging.CreateBitmapSourceFromHIcon(icon.Handle,
						new Int32Rect(0, 0, icon.Width, icon.Height),
						BitmapSizeOptions.FromEmptyOptions());
					return img;
				}
			}
			finally
			{
				DestroyIcon(shinfo.hIcon);
			}
		}

		public static ImageSource? GetSmallIcon(string fileName)
		{
			return GetIcon(fileName, SHGFI_SMALLICON);
		}

		public static ImageSource? GetLargeIcon(string fileName)
		{
			return GetIcon(fileName, SHGFI_LARGEICON);
		}
	};
}
