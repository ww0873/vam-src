using System;
using System.IO;

namespace MVR.FileManagement
{
	// Token: 0x02000BD9 RID: 3033
	public static class FileChecker
	{
		// Token: 0x0600562D RID: 22061 RVA: 0x001F78E4 File Offset: 0x001F5CE4
		public static bool CheckSignature(string filepath, int signatureSize, string expectedSignature)
		{
			if (string.IsNullOrEmpty(filepath))
			{
				throw new ArgumentException("Must specify a filepath");
			}
			if (string.IsNullOrEmpty(expectedSignature))
			{
				throw new ArgumentException("Must specify a value for the expected file signature");
			}
			bool result;
			using (FileEntryStream fileEntryStream = FileManager.OpenStream(filepath, false))
			{
				Stream stream = fileEntryStream.Stream;
				if (stream.Length < (long)signatureSize)
				{
					result = false;
				}
				else
				{
					byte[] array = new byte[signatureSize];
					int i = signatureSize;
					int num = 0;
					while (i > 0)
					{
						int num2 = stream.Read(array, num, i);
						i -= num2;
						num += num2;
					}
					string a = BitConverter.ToString(array);
					if (a == expectedSignature)
					{
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x0600562E RID: 22062 RVA: 0x001F79B8 File Offset: 0x001F5DB8
		public static bool IsGzipped(string filepath)
		{
			return FileChecker.CheckSignature(filepath, 3, "1F-8B-08");
		}
	}
}
