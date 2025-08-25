using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001B3 RID: 435
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentShader : PersistentObject
	{
		// Token: 0x06000902 RID: 2306 RVA: 0x000389C0 File Offset: 0x00036DC0
		public PersistentShader()
		{
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x000389C8 File Offset: 0x00036DC8
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Shader shader = (Shader)obj;
			shader.maximumLOD = this.maximumLOD;
			return shader;
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x000389FC File Offset: 0x00036DFC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Shader shader = (Shader)obj;
			this.maximumLOD = shader.maximumLOD;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00038A2A File Offset: 0x00036E2A
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000A0D RID: 2573
		public int maximumLOD;
	}
}
