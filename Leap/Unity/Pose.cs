using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006B6 RID: 1718
	[Serializable]
	public struct Pose : IEquatable<Pose>
	{
		// Token: 0x06002958 RID: 10584 RVA: 0x000E1316 File Offset: 0x000DF716
		public Pose(Vector3 position)
		{
			this = new Pose(position, Quaternion.identity);
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x000E1324 File Offset: 0x000DF724
		public Pose(Quaternion rotation)
		{
			this = new Pose(Vector3.zero, rotation);
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x000E1332 File Offset: 0x000DF732
		public Pose(Vector3 position, Quaternion rotation)
		{
			this.position = position;
			this.rotation = rotation;
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x0600295B RID: 10587 RVA: 0x000E1344 File Offset: 0x000DF744
		public Pose inverse
		{
			get
			{
				Quaternion quaternion = Quaternion.Inverse(this.rotation);
				return new Pose(quaternion * -this.position, quaternion);
			}
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x000E1374 File Offset: 0x000DF774
		public static Pose operator *(Pose A, Pose B)
		{
			return new Pose(A.position + A.rotation * B.position, A.rotation * B.rotation);
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x000E13AD File Offset: 0x000DF7AD
		public static Pose operator +(Pose A, Pose B)
		{
			return new Pose(A.position + B.position, A.rotation * B.rotation);
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x000E13DA File Offset: 0x000DF7DA
		public static Pose operator *(Pose pose, Vector3 localPosition)
		{
			return new Pose(pose.position + pose.rotation * localPosition, pose.rotation);
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x000E1401 File Offset: 0x000DF801
		public bool ApproxEquals(Pose other)
		{
			return this.position.ApproxEquals(other.position) && this.rotation.ApproxEquals(other.rotation);
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x000E1430 File Offset: 0x000DF830
		public static Pose Lerp(Pose a, Pose b, float t)
		{
			if (t >= 1f)
			{
				return b;
			}
			if (t <= 0f)
			{
				return a;
			}
			return new Pose(Vector3.Lerp(a.position, b.position, t), Quaternion.Lerp(Quaternion.Slerp(a.rotation, b.rotation, t), Quaternion.identity, 0f));
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x000E1493 File Offset: 0x000DF893
		public static Pose LerpUnclamped(Pose a, Pose b, float t)
		{
			return new Pose(Vector3.LerpUnclamped(a.position, b.position, t), Quaternion.SlerpUnclamped(a.rotation, b.rotation, t));
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x000E14C2 File Offset: 0x000DF8C2
		public static Pose LerpUnclampedTimed(Pose a, float aTime, Pose b, float bTime, float extrapolateTime)
		{
			return Pose.LerpUnclamped(a, b, extrapolateTime.MapUnclamped(aTime, bTime, 0f, 1f));
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x000E14E0 File Offset: 0x000DF8E0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[Pose | Position: ",
				this.position.ToString(),
				", Rotation: ",
				this.rotation.ToString(),
				"]"
			});
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x000E1538 File Offset: 0x000DF938
		public string ToString(string format)
		{
			return string.Concat(new string[]
			{
				"[Pose | Position: ",
				this.position.ToString(format),
				", Rotation: ",
				this.rotation.ToString(format),
				"]"
			});
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x000E1586 File Offset: 0x000DF986
		public override bool Equals(object obj)
		{
			return obj is Pose && this.Equals((Pose)obj);
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x000E15A1 File Offset: 0x000DF9A1
		public bool Equals(Pose other)
		{
			return other.position == this.position && other.rotation == this.rotation;
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x000E15D0 File Offset: 0x000DF9D0
		public override int GetHashCode()
		{
			return new Hash
			{
				this.position,
				this.rotation
			};
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x000E1605 File Offset: 0x000DFA05
		public static bool operator ==(Pose a, Pose b)
		{
			return a.Equals(b);
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x000E160F File Offset: 0x000DFA0F
		public static bool operator !=(Pose a, Pose b)
		{
			return !a.Equals(b);
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x000E161C File Offset: 0x000DFA1C
		// Note: this type is marked as 'beforefieldinit'.
		static Pose()
		{
		}

		// Token: 0x040021EC RID: 8684
		public Vector3 position;

		// Token: 0x040021ED RID: 8685
		public Quaternion rotation;

		// Token: 0x040021EE RID: 8686
		public static readonly Pose identity = new Pose(Vector3.zero, Quaternion.identity);
	}
}
