using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000198 RID: 408
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentPhysicMaterial : PersistentObject
	{
		// Token: 0x060008B5 RID: 2229 RVA: 0x00037701 File Offset: 0x00035B01
		public PersistentPhysicMaterial()
		{
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0003770C File Offset: 0x00035B0C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			PhysicMaterial physicMaterial = (PhysicMaterial)obj;
			physicMaterial.dynamicFriction = this.dynamicFriction;
			physicMaterial.staticFriction = this.staticFriction;
			physicMaterial.bounciness = this.bounciness;
			physicMaterial.frictionCombine = (PhysicMaterialCombine)this.frictionCombine;
			physicMaterial.bounceCombine = (PhysicMaterialCombine)this.bounceCombine;
			return physicMaterial;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00037770 File Offset: 0x00035B70
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			PhysicMaterial physicMaterial = (PhysicMaterial)obj;
			this.dynamicFriction = physicMaterial.dynamicFriction;
			this.staticFriction = physicMaterial.staticFriction;
			this.bounciness = physicMaterial.bounciness;
			this.frictionCombine = (uint)physicMaterial.frictionCombine;
			this.bounceCombine = (uint)physicMaterial.bounceCombine;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x000377CE File Offset: 0x00035BCE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000986 RID: 2438
		public float dynamicFriction;

		// Token: 0x04000987 RID: 2439
		public float staticFriction;

		// Token: 0x04000988 RID: 2440
		public float bounciness;

		// Token: 0x04000989 RID: 2441
		public uint frictionCombine;

		// Token: 0x0400098A RID: 2442
		public uint bounceCombine;
	}
}
