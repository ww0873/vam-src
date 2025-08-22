using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000183 RID: 387
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentLightProbeProxyVolume : PersistentBehaviour
	{
		// Token: 0x06000866 RID: 2150 RVA: 0x00036539 File Offset: 0x00034939
		public PersistentLightProbeProxyVolume()
		{
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00036544 File Offset: 0x00034944
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			LightProbeProxyVolume lightProbeProxyVolume = (LightProbeProxyVolume)obj;
			lightProbeProxyVolume.sizeCustom = this.sizeCustom;
			lightProbeProxyVolume.originCustom = this.originCustom;
			lightProbeProxyVolume.boundingBoxMode = (LightProbeProxyVolume.BoundingBoxMode)this.boundingBoxMode;
			lightProbeProxyVolume.resolutionMode = (LightProbeProxyVolume.ResolutionMode)this.resolutionMode;
			lightProbeProxyVolume.probePositionMode = (LightProbeProxyVolume.ProbePositionMode)this.probePositionMode;
			lightProbeProxyVolume.refreshMode = (LightProbeProxyVolume.RefreshMode)this.refreshMode;
			lightProbeProxyVolume.probeDensity = this.probeDensity;
			lightProbeProxyVolume.gridResolutionX = this.gridResolutionX;
			lightProbeProxyVolume.gridResolutionY = this.gridResolutionY;
			lightProbeProxyVolume.gridResolutionZ = this.gridResolutionZ;
			return lightProbeProxyVolume;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x000365E4 File Offset: 0x000349E4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			LightProbeProxyVolume lightProbeProxyVolume = (LightProbeProxyVolume)obj;
			this.sizeCustom = lightProbeProxyVolume.sizeCustom;
			this.originCustom = lightProbeProxyVolume.originCustom;
			this.boundingBoxMode = (uint)lightProbeProxyVolume.boundingBoxMode;
			this.resolutionMode = (uint)lightProbeProxyVolume.resolutionMode;
			this.probePositionMode = (uint)lightProbeProxyVolume.probePositionMode;
			this.refreshMode = (uint)lightProbeProxyVolume.refreshMode;
			this.probeDensity = lightProbeProxyVolume.probeDensity;
			this.gridResolutionX = lightProbeProxyVolume.gridResolutionX;
			this.gridResolutionY = lightProbeProxyVolume.gridResolutionY;
			this.gridResolutionZ = lightProbeProxyVolume.gridResolutionZ;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0003667E File Offset: 0x00034A7E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000926 RID: 2342
		public Vector3 sizeCustom;

		// Token: 0x04000927 RID: 2343
		public Vector3 originCustom;

		// Token: 0x04000928 RID: 2344
		public uint boundingBoxMode;

		// Token: 0x04000929 RID: 2345
		public uint resolutionMode;

		// Token: 0x0400092A RID: 2346
		public uint probePositionMode;

		// Token: 0x0400092B RID: 2347
		public uint refreshMode;

		// Token: 0x0400092C RID: 2348
		public float probeDensity;

		// Token: 0x0400092D RID: 2349
		public int gridResolutionX;

		// Token: 0x0400092E RID: 2350
		public int gridResolutionY;

		// Token: 0x0400092F RID: 2351
		public int gridResolutionZ;
	}
}
