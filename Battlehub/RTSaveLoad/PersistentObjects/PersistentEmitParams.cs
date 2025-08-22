using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000111 RID: 273
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentEmitParams : PersistentData
	{
		// Token: 0x0600069C RID: 1692 RVA: 0x0002D4AA File Offset: 0x0002B8AA
		public PersistentEmitParams()
		{
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0002D4B4 File Offset: 0x0002B8B4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams)obj;
			emitParams.position = this.position;
			emitParams.applyShapeToPosition = this.applyShapeToPosition;
			emitParams.velocity = this.velocity;
			emitParams.startLifetime = this.startLifetime;
			emitParams.startSize = this.startSize;
			emitParams.startSize3D = this.startSize3D;
			emitParams.axisOfRotation = this.axisOfRotation;
			emitParams.rotation = this.rotation;
			emitParams.rotation3D = this.rotation3D;
			emitParams.angularVelocity = this.angularVelocity;
			emitParams.angularVelocity3D = this.angularVelocity3D;
			emitParams.startColor = this.startColor;
			emitParams.randomSeed = this.randomSeed;
			return emitParams;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0002D58C File Offset: 0x0002B98C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.EmitParams emitParams = (ParticleSystem.EmitParams)obj;
			this.position = emitParams.position;
			this.applyShapeToPosition = emitParams.applyShapeToPosition;
			this.velocity = emitParams.velocity;
			this.startLifetime = emitParams.startLifetime;
			this.startSize = emitParams.startSize;
			this.startSize3D = emitParams.startSize3D;
			this.axisOfRotation = emitParams.axisOfRotation;
			this.rotation = emitParams.rotation;
			this.rotation3D = emitParams.rotation3D;
			this.angularVelocity = emitParams.angularVelocity;
			this.angularVelocity3D = emitParams.angularVelocity3D;
			this.startColor = emitParams.startColor;
			this.randomSeed = emitParams.randomSeed;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0002D657 File Offset: 0x0002BA57
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000661 RID: 1633
		public Vector3 position;

		// Token: 0x04000662 RID: 1634
		public bool applyShapeToPosition;

		// Token: 0x04000663 RID: 1635
		public Vector3 velocity;

		// Token: 0x04000664 RID: 1636
		public float startLifetime;

		// Token: 0x04000665 RID: 1637
		public float startSize;

		// Token: 0x04000666 RID: 1638
		public Vector3 startSize3D;

		// Token: 0x04000667 RID: 1639
		public Vector3 axisOfRotation;

		// Token: 0x04000668 RID: 1640
		public float rotation;

		// Token: 0x04000669 RID: 1641
		public Vector3 rotation3D;

		// Token: 0x0400066A RID: 1642
		public float angularVelocity;

		// Token: 0x0400066B RID: 1643
		public Vector3 angularVelocity3D;

		// Token: 0x0400066C RID: 1644
		public Color32 startColor;

		// Token: 0x0400066D RID: 1645
		public uint randomSeed;
	}
}
