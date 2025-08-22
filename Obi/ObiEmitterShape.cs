using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003C6 RID: 966
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public abstract class ObiEmitterShape : MonoBehaviour
	{
		// Token: 0x06001891 RID: 6289 RVA: 0x0008AFB1 File Offset: 0x000893B1
		protected ObiEmitterShape()
		{
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06001892 RID: 6290 RVA: 0x0008AFC4 File Offset: 0x000893C4
		public int DistributionPointsCount
		{
			get
			{
				return this.distribution.Count;
			}
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x0008AFD4 File Offset: 0x000893D4
		public void OnEnable()
		{
			ObiEmitter component = base.GetComponent<ObiEmitter>();
			if (component != null)
			{
				component.EmitterShape = this;
			}
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x0008AFFC File Offset: 0x000893FC
		public void OnDisable()
		{
			ObiEmitter component = base.GetComponent<ObiEmitter>();
			if (component != null)
			{
				component.EmitterShape = null;
			}
		}

		// Token: 0x06001895 RID: 6293
		public abstract void GenerateDistribution();

		// Token: 0x06001896 RID: 6294
		public abstract bool SupportsAllSamplingMethods();

		// Token: 0x06001897 RID: 6295 RVA: 0x0008B024 File Offset: 0x00089424
		public ObiEmitterShape.DistributionPoint GetDistributionPoint()
		{
			if (this.lastDistributionPoint >= this.distribution.Count)
			{
				return default(ObiEmitterShape.DistributionPoint);
			}
			ObiEmitterShape.DistributionPoint result = this.distribution[this.lastDistributionPoint];
			this.lastDistributionPoint = (this.lastDistributionPoint + 1) % this.distribution.Count;
			return result;
		}

		// Token: 0x040013F0 RID: 5104
		public ObiEmitterShape.SamplingMethod samplingMethod;

		// Token: 0x040013F1 RID: 5105
		[HideInInspector]
		public float particleSize;

		// Token: 0x040013F2 RID: 5106
		protected List<ObiEmitterShape.DistributionPoint> distribution = new List<ObiEmitterShape.DistributionPoint>();

		// Token: 0x040013F3 RID: 5107
		protected int lastDistributionPoint;

		// Token: 0x020003C7 RID: 967
		public enum SamplingMethod
		{
			// Token: 0x040013F5 RID: 5109
			SURFACE,
			// Token: 0x040013F6 RID: 5110
			LAYER,
			// Token: 0x040013F7 RID: 5111
			FILL
		}

		// Token: 0x020003C8 RID: 968
		public struct DistributionPoint
		{
			// Token: 0x06001898 RID: 6296 RVA: 0x0008B07E File Offset: 0x0008947E
			public DistributionPoint(Vector3 position, Vector3 velocity)
			{
				this.position = position;
				this.velocity = velocity;
				this.color = Color.white;
			}

			// Token: 0x06001899 RID: 6297 RVA: 0x0008B099 File Offset: 0x00089499
			public DistributionPoint(Vector3 position, Vector3 velocity, Color color)
			{
				this.position = position;
				this.velocity = velocity;
				this.color = color;
			}

			// Token: 0x040013F8 RID: 5112
			public Vector3 position;

			// Token: 0x040013F9 RID: 5113
			public Vector3 velocity;

			// Token: 0x040013FA RID: 5114
			public Color color;
		}
	}
}
