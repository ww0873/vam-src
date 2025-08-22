using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000175 RID: 373
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentGUIStyleState : PersistentData
	{
		// Token: 0x0600082E RID: 2094 RVA: 0x000359D8 File Offset: 0x00033DD8
		public PersistentGUIStyleState()
		{
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x000359E0 File Offset: 0x00033DE0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			GUIStyleState guistyleState = (GUIStyleState)obj;
			guistyleState.background = (Texture2D)objects.Get(this.background);
			guistyleState.textColor = this.textColor;
			return guistyleState;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00035A2C File Offset: 0x00033E2C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			GUIStyleState guistyleState = (GUIStyleState)obj;
			this.background = guistyleState.background.GetMappedInstanceID();
			this.textColor = guistyleState.textColor;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00035A6B File Offset: 0x00033E6B
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.background, dependencies, objects, allowNulls);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00035A88 File Offset: 0x00033E88
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			GUIStyleState guistyleState = (GUIStyleState)obj;
			base.AddDependency(guistyleState.background, dependencies);
		}

		// Token: 0x040008DB RID: 2267
		public long background;

		// Token: 0x040008DC RID: 2268
		public long[] scaledBackgrounds;

		// Token: 0x040008DD RID: 2269
		public Color textColor;
	}
}
