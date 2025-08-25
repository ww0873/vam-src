using System;
using System.IO;
using System.Runtime.CompilerServices;
using ProtoBuf.Meta;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000233 RID: 563
	public static class ProtobufSerializer
	{
		// Token: 0x06000BC2 RID: 3010 RVA: 0x0004A2D9 File Offset: 0x000486D9
		static ProtobufSerializer()
		{
			ProtobufSerializer.model.DynamicTypeFormatting += ProtobufSerializer.<ProtobufSerializer>m__0;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0004A2FB File Offset: 0x000486FB
		public static TData DeepClone<TData>(TData data)
		{
			return (TData)((object)ProtobufSerializer.model.DeepClone(data));
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0004A314 File Offset: 0x00048714
		public static TData Deserialize<TData>(byte[] b)
		{
			TData result;
			using (MemoryStream memoryStream = new MemoryStream(b))
			{
				TData tdata = (TData)((object)ProtobufSerializer.model.Deserialize(memoryStream, null, typeof(TData)));
				result = tdata;
			}
			return result;
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0004A36C File Offset: 0x0004876C
		public static byte[] Serialize<TData>(TData data)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				ProtobufSerializer.model.Serialize(memoryStream, data);
				memoryStream.Flush();
				memoryStream.Position = 0L;
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0004A3C8 File Offset: 0x000487C8
		[CompilerGenerated]
		private static void <ProtobufSerializer>m__0(object sender, TypeFormatEventArgs args)
		{
			if (args.FormattedName == null)
			{
				return;
			}
			if (Type.GetType(args.FormattedName) == null)
			{
				args.Type = typeof(NilContainer);
			}
		}

		// Token: 0x04000CA8 RID: 3240
		private static RTTypeModel model = new RTTypeModel();
	}
}
