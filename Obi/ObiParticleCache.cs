using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003BD RID: 957
	public class ObiParticleCache : ScriptableObject
	{
		// Token: 0x06001870 RID: 6256 RVA: 0x0008A688 File Offset: 0x00088A88
		public ObiParticleCache()
		{
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06001871 RID: 6257 RVA: 0x0008A6A2 File Offset: 0x00088AA2
		public float Duration
		{
			get
			{
				return this.duration;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06001872 RID: 6258 RVA: 0x0008A6AA File Offset: 0x00088AAA
		public int FrameCount
		{
			get
			{
				return this.frames.Count;
			}
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x0008A6B8 File Offset: 0x00088AB8
		public void OnEnable()
		{
			if (this.frames == null)
			{
				this.frames = new List<ObiParticleCache.Frame>();
			}
			if (this.references == null)
			{
				this.references = new List<int>
				{
					0
				};
			}
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x0008A6FC File Offset: 0x00088AFC
		public int SizeInBytes()
		{
			int num = 0;
			foreach (ObiParticleCache.Frame frame in this.frames)
			{
				num += frame.SizeInBytes();
			}
			num += this.references.Count * 4;
			return num;
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x0008A770 File Offset: 0x00088B70
		public void Clear()
		{
			this.duration = 0f;
			this.frames.Clear();
			this.references.Clear();
			this.references.Add(0);
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x0008A7A0 File Offset: 0x00088BA0
		private int GetBaseFrame(float time)
		{
			int num = Mathf.FloorToInt(time / this.referenceIntervalSeconds);
			if (num >= 0 && num < this.references.Count)
			{
				return this.references[num];
			}
			return int.MaxValue;
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x0008A7E8 File Offset: 0x00088BE8
		public void AddFrame(ObiParticleCache.Frame frame)
		{
			int num = Mathf.FloorToInt(frame.time / this.referenceIntervalSeconds);
			if (num >= this.references.Count)
			{
				for (float num2 = frame.time - (float)Mathf.Max(0, this.references.Count - 1) * this.referenceIntervalSeconds; num2 >= this.referenceIntervalSeconds; num2 -= this.referenceIntervalSeconds)
				{
					this.references.Add(this.frames.Count);
				}
			}
			if (frame.time >= this.duration)
			{
				this.frames.Add(frame);
				this.duration = frame.time;
			}
			else
			{
				int num3 = this.references[num];
				for (int i = num3; i < this.frames.Count; i++)
				{
					if (this.frames[i].time > frame.time)
					{
						this.frames[i] = frame;
						return;
					}
				}
			}
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x0008A8F0 File Offset: 0x00088CF0
		public void GetFrame(float time, bool interpolate, ref ObiParticleCache.Frame result)
		{
			time = Mathf.Clamp(time, 0f, this.duration);
			int baseFrame = this.GetBaseFrame(time);
			for (int i = baseFrame; i < this.frames.Count; i++)
			{
				if (this.frames[i].time > time)
				{
					if (interpolate)
					{
						int num = Mathf.Max(0, i - 1);
						float mu = 0f;
						if (i != num)
						{
							mu = (time - this.frames[num].time) / (this.frames[i].time - this.frames[num].time);
						}
						ObiParticleCache.Frame.Lerp(this.frames[num], this.frames[i], ref result, mu);
					}
					else
					{
						result = this.frames[i];
					}
					return;
				}
			}
		}

		// Token: 0x040013D4 RID: 5076
		public float referenceIntervalSeconds = 0.5f;

		// Token: 0x040013D5 RID: 5077
		public bool localSpace = true;

		// Token: 0x040013D6 RID: 5078
		[SerializeField]
		private float duration;

		// Token: 0x040013D7 RID: 5079
		[SerializeField]
		private List<ObiParticleCache.Frame> frames;

		// Token: 0x040013D8 RID: 5080
		[SerializeField]
		private List<int> references;

		// Token: 0x020003BE RID: 958
		public class UncompressedFrame
		{
			// Token: 0x06001879 RID: 6265 RVA: 0x0008A9D3 File Offset: 0x00088DD3
			public UncompressedFrame()
			{
			}

			// Token: 0x040013D9 RID: 5081
			public List<int> indices = new List<int>();

			// Token: 0x040013DA RID: 5082
			public List<Vector3> positions = new List<Vector3>();
		}

		// Token: 0x020003BF RID: 959
		[Serializable]
		public class Frame
		{
			// Token: 0x0600187A RID: 6266 RVA: 0x0008A9F1 File Offset: 0x00088DF1
			public Frame()
			{
				this.time = 0f;
				this.positions = new List<Vector3>();
				this.indices = new List<int>();
			}

			// Token: 0x0600187B RID: 6267 RVA: 0x0008AA1A File Offset: 0x00088E1A
			public void Clear()
			{
				this.time = 0f;
				this.positions.Clear();
				this.indices.Clear();
			}

			// Token: 0x0600187C RID: 6268 RVA: 0x0008AA3D File Offset: 0x00088E3D
			public int SizeInBytes()
			{
				return 4 + this.positions.Count * 4 * 3 + this.indices.Count * 4;
			}

			// Token: 0x0600187D RID: 6269 RVA: 0x0008AA60 File Offset: 0x00088E60
			public static void Lerp(ObiParticleCache.Frame a, ObiParticleCache.Frame b, ref ObiParticleCache.Frame result, float mu)
			{
				result.Clear();
				result.time = Mathf.Lerp(a.time, b.time, mu);
				int num = 0;
				int num2 = 0;
				int count = a.indices.Count;
				int count2 = b.indices.Count;
				float num3 = 1f - mu;
				Vector3 zero = Vector3.zero;
				while (num < count && num2 < count2)
				{
					int num4 = a.indices[num];
					int num5 = b.indices[num2];
					if (num4 > num5)
					{
						result.indices.Add(num5);
						result.positions.Add(b.positions[num2]);
						num2++;
					}
					else if (num4 < num5)
					{
						result.indices.Add(num4);
						result.positions.Add(a.positions[num]);
						num++;
					}
					else
					{
						result.indices.Add(num4);
						Vector3 vector = a.positions[num];
						Vector3 vector2 = b.positions[num2];
						zero.Set(vector.x * num3 + vector2.x * mu, vector.y * num3 + vector2.y * mu, vector.z * num3 + vector2.z * mu);
						result.positions.Add(zero);
						num++;
						num2++;
					}
				}
			}

			// Token: 0x040013DB RID: 5083
			public float time;

			// Token: 0x040013DC RID: 5084
			public List<Vector3> positions;

			// Token: 0x040013DD RID: 5085
			public List<int> indices;
		}
	}
}
