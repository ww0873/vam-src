using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000219 RID: 537
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTexture2D : PersistentTexture
	{
		// Token: 0x06000ACF RID: 2767 RVA: 0x00043148 File Offset: 0x00041548
		public PersistentTexture2D()
		{
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00043150 File Offset: 0x00041550
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Texture2D texture2D = (Texture2D)obj;
			try
			{
				texture2D.Resize(this.width, this.height);
			}
			catch
			{
			}
			return texture2D;
		}
	}
}
