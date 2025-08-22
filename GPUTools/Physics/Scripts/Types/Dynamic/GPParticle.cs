using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Dynamic
{
	// Token: 0x02000A7C RID: 2684
	public struct GPParticle
	{
		// Token: 0x060045B1 RID: 17841 RVA: 0x0013F620 File Offset: 0x0013DA20
		public GPParticle(Vector3 position, float radius)
		{
			this.Position = position;
			this.LastPosition = position;
			this.LastPositionInner = position;
			this.DrawPosition = position;
			this.Velocity = Vector3.zero;
			this.Radius = radius;
			this.ColliderDelta = Vector3.zero;
			this.CollisionDrag = 0f;
			this.CollisionHold = 0f;
			this.CollisionPower = 0f;
			this.Strength = 0.1f;
			this.CollisionEnabled = 1;
			this.SphereCollisionID = -1;
			this.LineSphereCollisionID = -1;
			this.GrabID = -1;
			this.GrabDistance = 0f;
			this.AuxData = 0f;
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x0013F6C4 File Offset: 0x0013DAC4
		public static int Size()
		{
			return 116;
		}

		// Token: 0x0400334C RID: 13132
		public Vector3 Position;

		// Token: 0x0400334D RID: 13133
		public Vector3 LastPosition;

		// Token: 0x0400334E RID: 13134
		public Vector3 LastPositionInner;

		// Token: 0x0400334F RID: 13135
		public Vector3 DrawPosition;

		// Token: 0x04003350 RID: 13136
		public Vector3 Velocity;

		// Token: 0x04003351 RID: 13137
		public float Radius;

		// Token: 0x04003352 RID: 13138
		public Vector3 ColliderDelta;

		// Token: 0x04003353 RID: 13139
		public float CollisionDrag;

		// Token: 0x04003354 RID: 13140
		public float CollisionHold;

		// Token: 0x04003355 RID: 13141
		public float CollisionPower;

		// Token: 0x04003356 RID: 13142
		public float Strength;

		// Token: 0x04003357 RID: 13143
		public int CollisionEnabled;

		// Token: 0x04003358 RID: 13144
		public int SphereCollisionID;

		// Token: 0x04003359 RID: 13145
		public int LineSphereCollisionID;

		// Token: 0x0400335A RID: 13146
		public int GrabID;

		// Token: 0x0400335B RID: 13147
		public float GrabDistance;

		// Token: 0x0400335C RID: 13148
		public float AuxData;
	}
}
