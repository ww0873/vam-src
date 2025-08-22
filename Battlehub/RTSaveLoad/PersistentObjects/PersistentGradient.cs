using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000114 RID: 276
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentGradient : PersistentData
	{
		// Token: 0x060006A9 RID: 1705 RVA: 0x0002D942 File Offset: 0x0002BD42
		public PersistentGradient()
		{
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0002D94C File Offset: 0x0002BD4C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Gradient gradient = (Gradient)obj;
			gradient.colorKeys = this.colorKeys;
			gradient.alphaKeys = this.alphaKeys;
			gradient.mode = (GradientMode)this.mode;
			return gradient;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0002D998 File Offset: 0x0002BD98
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Gradient gradient = (Gradient)obj;
			this.colorKeys = gradient.colorKeys;
			this.alphaKeys = gradient.alphaKeys;
			this.mode = (uint)gradient.mode;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0002D9DE File Offset: 0x0002BDDE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000679 RID: 1657
		public GradientColorKey[] colorKeys;

		// Token: 0x0400067A RID: 1658
		public GradientAlphaKey[] alphaKeys;

		// Token: 0x0400067B RID: 1659
		public uint mode;
	}
}
