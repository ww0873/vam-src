using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200015B RID: 347
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentConstantForce2D : PersistentPhysicsUpdateBehaviour2D
	{
		// Token: 0x060007BF RID: 1983 RVA: 0x000340C9 File Offset: 0x000324C9
		public PersistentConstantForce2D()
		{
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x000340D4 File Offset: 0x000324D4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ConstantForce2D constantForce2D = (ConstantForce2D)obj;
			constantForce2D.force = this.force;
			constantForce2D.relativeForce = this.relativeForce;
			constantForce2D.torque = this.torque;
			return constantForce2D;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00034120 File Offset: 0x00032520
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ConstantForce2D constantForce2D = (ConstantForce2D)obj;
			this.force = constantForce2D.force;
			this.relativeForce = constantForce2D.relativeForce;
			this.torque = constantForce2D.torque;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00034166 File Offset: 0x00032566
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000876 RID: 2166
		public Vector2 force;

		// Token: 0x04000877 RID: 2167
		public Vector2 relativeForce;

		// Token: 0x04000878 RID: 2168
		public float torque;
	}
}
