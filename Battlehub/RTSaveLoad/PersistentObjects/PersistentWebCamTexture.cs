using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001D6 RID: 470
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentWebCamTexture : PersistentTexture
	{
		// Token: 0x0600097E RID: 2430 RVA: 0x0003AB9B File Offset: 0x00038F9B
		public PersistentWebCamTexture()
		{
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0003ABA4 File Offset: 0x00038FA4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			WebCamTexture webCamTexture = (WebCamTexture)obj;
			webCamTexture.deviceName = this.deviceName;
			webCamTexture.requestedFPS = this.requestedFPS;
			webCamTexture.requestedWidth = this.requestedWidth;
			webCamTexture.requestedHeight = this.requestedHeight;
			return webCamTexture;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0003ABFC File Offset: 0x00038FFC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			WebCamTexture webCamTexture = (WebCamTexture)obj;
			this.deviceName = webCamTexture.deviceName;
			this.requestedFPS = webCamTexture.requestedFPS;
			this.requestedWidth = webCamTexture.requestedWidth;
			this.requestedHeight = webCamTexture.requestedHeight;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0003AC4E File Offset: 0x0003904E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000AB8 RID: 2744
		public string deviceName;

		// Token: 0x04000AB9 RID: 2745
		public float requestedFPS;

		// Token: 0x04000ABA RID: 2746
		public int requestedWidth;

		// Token: 0x04000ABB RID: 2747
		public int requestedHeight;
	}
}
