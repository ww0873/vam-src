using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200016A RID: 362
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentFont : PersistentObject
	{
		// Token: 0x06000807 RID: 2055 RVA: 0x000347B1 File Offset: 0x00032BB1
		public PersistentFont()
		{
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x000347BC File Offset: 0x00032BBC
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Font font = (Font)obj;
			font.material = (Material)objects.Get(this.material);
			font.fontNames = this.fontNames;
			font.characterInfo = this.characterInfo;
			return font;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00034814 File Offset: 0x00032C14
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Font font = (Font)obj;
			this.material = font.material.GetMappedInstanceID();
			this.fontNames = font.fontNames;
			this.characterInfo = font.characterInfo;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0003485F File Offset: 0x00032C5F
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.material, dependencies, objects, allowNulls);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0003487C File Offset: 0x00032C7C
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Font font = (Font)obj;
			base.AddDependency(font.material, dependencies);
		}

		// Token: 0x04000898 RID: 2200
		public long material;

		// Token: 0x04000899 RID: 2201
		public string[] fontNames;

		// Token: 0x0400089A RID: 2202
		public CharacterInfo[] characterInfo;
	}
}
