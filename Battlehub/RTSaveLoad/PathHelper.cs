using System;
using System.Collections.Generic;
using System.IO;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000264 RID: 612
	public static class PathHelper
	{
		// Token: 0x06000D04 RID: 3332 RVA: 0x0004DDD7 File Offset: 0x0004C1D7
		public static bool IsPathRooted(string path)
		{
			return Path.IsPathRooted(path);
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0004DDE0 File Offset: 0x0004C1E0
		public static string GetRelativePath(string filespec, string folder)
		{
			Uri uri = new Uri(filespec);
			if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				folder += Path.DirectorySeparatorChar;
			}
			Uri uri2 = new Uri(folder);
			return Uri.UnescapeDataString(uri2.MakeRelativeUri(uri).ToString().Replace('/', Path.DirectorySeparatorChar));
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0004DE48 File Offset: 0x0004C248
		public static string RemoveInvalidFineNameCharacters(string name)
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			for (int i = 0; i < invalidFileNameChars.Length; i++)
			{
				name = name.Replace(invalidFileNameChars[i].ToString(), string.Empty);
			}
			return name;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0004DE90 File Offset: 0x0004C290
		public static string GetUniqueName(string desiredName, string ext, string[] existingNames)
		{
			if (existingNames == null || existingNames.Length == 0)
			{
				return desiredName;
			}
			for (int i = 0; i < existingNames.Length; i++)
			{
				existingNames[i] = existingNames[i].ToLower();
			}
			HashSet<string> hashSet = new HashSet<string>(existingNames);
			if (string.IsNullOrEmpty(ext))
			{
				if (!hashSet.Contains(desiredName.ToLower()))
				{
					return desiredName;
				}
			}
			else if (!hashSet.Contains(string.Format("{0}.{1}", desiredName.ToLower(), ext)))
			{
				return desiredName;
			}
			string[] array = desiredName.Split(new char[]
			{
				' '
			});
			string text = array[array.Length - 1];
			int num;
			if (!int.TryParse(text, out num))
			{
				num = 1;
			}
			else
			{
				desiredName = desiredName.Substring(0, desiredName.Length - text.Length).TrimEnd(new char[]
				{
					' '
				});
			}
			for (int j = 0; j < 10000; j++)
			{
				string text2;
				if (string.IsNullOrEmpty(ext))
				{
					text2 = string.Format("{0} {1}", desiredName, num);
				}
				else
				{
					text2 = string.Format("{0} {1}.{2}", desiredName, num, ext);
				}
				if (!hashSet.Contains(text2.ToLower()))
				{
					return text2;
				}
				num++;
			}
			if (string.IsNullOrEmpty(ext))
			{
				return string.Format("{0} {1}", desiredName, Guid.NewGuid().ToString("N"));
			}
			return string.Format("{0} {1}.{2}", desiredName, Guid.NewGuid().ToString("N"), ext);
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0004E020 File Offset: 0x0004C420
		public static string GetUniqueName(string desiredName, string[] existingNames)
		{
			return PathHelper.GetUniqueName(desiredName, null, existingNames);
		}
	}
}
