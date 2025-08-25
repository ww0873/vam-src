using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200016C RID: 364
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentGameObject : PersistentObject
	{
		// Token: 0x06000810 RID: 2064 RVA: 0x00034939 File Offset: 0x00032D39
		public PersistentGameObject()
		{
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00034944 File Offset: 0x00032D44
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			GameObject gameObject = (GameObject)obj;
			gameObject.layer = this.layer;
			gameObject.isStatic = this.isStatic;
			try
			{
				gameObject.tag = this.tag;
			}
			catch (UnityException ex)
			{
				Debug.LogWarning(ex.Message);
			}
			return gameObject;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x000349B8 File Offset: 0x00032DB8
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			GameObject gameObject = (GameObject)obj;
			this.layer = gameObject.layer;
			this.isStatic = gameObject.isStatic;
			this.tag = gameObject.tag;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x000349FE File Offset: 0x00032DFE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400089D RID: 2205
		public int layer;

		// Token: 0x0400089E RID: 2206
		public bool isStatic;

		// Token: 0x0400089F RID: 2207
		public string tag;
	}
}
