using System;
using System.IO;
using System.Reflection;

namespace MHLab.PATCH.Settings
{
	// Token: 0x0200033F RID: 831
	public class Settings : Singleton<Settings>
	{
		// Token: 0x06001426 RID: 5158 RVA: 0x000747B8 File Offset: 0x00072BB8
		public Settings()
		{
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x000747C0 File Offset: 0x00072BC0
		// Note: this type is marked as 'beforefieldinit'.
		static Settings()
		{
		}

		// Token: 0x04001172 RID: 4466
		public static string APP_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;

		// Token: 0x04001173 RID: 4467
		public static string ASSETS_PATH = string.Concat(new object[]
		{
			"Assets",
			Path.DirectorySeparatorChar,
			"MHLab",
			Path.DirectorySeparatorChar,
			"PATCH",
			Path.DirectorySeparatorChar
		});

		// Token: 0x04001174 RID: 4468
		public static string LANGUAGE_PATH = string.Concat(new object[]
		{
			Settings.ASSETS_PATH,
			"Resources",
			Path.DirectorySeparatorChar,
			"Localizatron",
			Path.DirectorySeparatorChar,
			"Locale",
			Path.DirectorySeparatorChar
		});

		// Token: 0x04001175 RID: 4469
		public static string SAVING_LANGUAGE_PATH = string.Concat(new object[]
		{
			Path.DirectorySeparatorChar,
			"Resources",
			Path.DirectorySeparatorChar,
			"Localizatron",
			Path.DirectorySeparatorChar,
			"Locale",
			Path.DirectorySeparatorChar
		});

		// Token: 0x04001176 RID: 4470
		public static string LANGUAGE_EXTENSION = ".txt";

		// Token: 0x04001177 RID: 4471
		public static string LANGUAGE_DEFAULT = "en_EN";
	}
}
