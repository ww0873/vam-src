using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine.SceneManagement;

namespace Battlehub.RTSaveLoad.UnityEngineNS.SceneManagementNS
{
	// Token: 0x020001EB RID: 491
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class SceneSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009DD RID: 2525 RVA: 0x0003D0C6 File Offset: 0x0003B4C6
		public SceneSurrogate()
		{
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0003D0D0 File Offset: 0x0003B4D0
		public static implicit operator Scene(SceneSurrogate v)
		{
			return default(Scene);
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0003D0E8 File Offset: 0x0003B4E8
		public static implicit operator SceneSurrogate(Scene v)
		{
			return new SceneSurrogate();
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0003D0FC File Offset: 0x0003B4FC
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0003D0FE File Offset: 0x0003B4FE
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			return obj;
		}
	}
}
