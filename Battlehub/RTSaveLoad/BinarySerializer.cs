using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000231 RID: 561
	public static class BinarySerializer
	{
		// Token: 0x06000BBD RID: 3005 RVA: 0x0004A1E4 File Offset: 0x000485E4
		public static TData Deserialize<TData>(byte[] b)
		{
			TData result;
			using (MemoryStream memoryStream = new MemoryStream(b))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.SurrogateSelector = SerializationSurrogates.CreateSelector();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				object obj = binaryFormatter.Deserialize(memoryStream);
				result = (TData)((object)obj);
			}
			return result;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0004A248 File Offset: 0x00048648
		public static TData DeserializeFromString<TData>(string data)
		{
			return BinarySerializer.Deserialize<TData>(Convert.FromBase64String(data));
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0004A258 File Offset: 0x00048658
		public static byte[] Serialize<TData>(TData settings)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				new BinaryFormatter
				{
					SurrogateSelector = SerializationSurrogates.CreateSelector()
				}.Serialize(memoryStream, settings);
				memoryStream.Flush();
				memoryStream.Position = 0L;
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0004A2C4 File Offset: 0x000486C4
		public static string SerializeToString<TData>(TData settings)
		{
			return Convert.ToBase64String(BinarySerializer.Serialize<TData>(settings));
		}
	}
}
