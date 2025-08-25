using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Rendering;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000180 RID: 384
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentLight : PersistentBehaviour
	{
		// Token: 0x0600085C RID: 2140 RVA: 0x00036220 File Offset: 0x00034620
		public PersistentLight()
		{
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00036228 File Offset: 0x00034628
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Light light = (Light)obj;
			light.type = (LightType)this.type;
			light.color = this.color;
			light.colorTemperature = this.colorTemperature;
			light.intensity = this.intensity;
			light.bounceIntensity = this.bounceIntensity;
			light.shadows = (LightShadows)this.shadows;
			light.shadowStrength = this.shadowStrength;
			light.shadowResolution = (LightShadowResolution)this.shadowResolution;
			light.shadowCustomResolution = this.shadowCustomResolution;
			light.shadowBias = this.shadowBias;
			light.shadowNormalBias = this.shadowNormalBias;
			light.shadowNearPlane = this.shadowNearPlane;
			light.range = this.range;
			light.spotAngle = this.spotAngle;
			light.cookieSize = this.cookieSize;
			light.cookie = (Texture)objects.Get(this.cookie);
			light.flare = (Flare)objects.Get(this.flare);
			light.renderMode = (LightRenderMode)this.renderMode;
			light.cullingMask = this.cullingMask;
			return light;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0003634C File Offset: 0x0003474C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Light light = (Light)obj;
			this.type = (uint)light.type;
			this.color = light.color;
			this.colorTemperature = light.colorTemperature;
			this.intensity = light.intensity;
			this.bounceIntensity = light.bounceIntensity;
			this.shadows = (uint)light.shadows;
			this.shadowStrength = light.shadowStrength;
			this.shadowResolution = (uint)light.shadowResolution;
			this.shadowCustomResolution = light.shadowCustomResolution;
			this.shadowBias = light.shadowBias;
			this.shadowNormalBias = light.shadowNormalBias;
			this.shadowNearPlane = light.shadowNearPlane;
			this.range = light.range;
			this.spotAngle = light.spotAngle;
			this.cookieSize = light.cookieSize;
			this.cookie = light.cookie.GetMappedInstanceID();
			this.flare = light.flare.GetMappedInstanceID();
			this.renderMode = (uint)light.renderMode;
			this.cullingMask = light.cullingMask;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0003645C File Offset: 0x0003485C
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.cookie, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.flare, dependencies, objects, allowNulls);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x00036488 File Offset: 0x00034888
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Light light = (Light)obj;
			base.AddDependency(light.cookie, dependencies);
			base.AddDependency(light.flare, dependencies);
		}

		// Token: 0x04000911 RID: 2321
		public uint type;

		// Token: 0x04000912 RID: 2322
		public Color color;

		// Token: 0x04000913 RID: 2323
		public float colorTemperature;

		// Token: 0x04000914 RID: 2324
		public float intensity;

		// Token: 0x04000915 RID: 2325
		public float bounceIntensity;

		// Token: 0x04000916 RID: 2326
		public uint shadows;

		// Token: 0x04000917 RID: 2327
		public float shadowStrength;

		// Token: 0x04000918 RID: 2328
		public uint shadowResolution;

		// Token: 0x04000919 RID: 2329
		public int shadowCustomResolution;

		// Token: 0x0400091A RID: 2330
		public float shadowBias;

		// Token: 0x0400091B RID: 2331
		public float shadowNormalBias;

		// Token: 0x0400091C RID: 2332
		public float shadowNearPlane;

		// Token: 0x0400091D RID: 2333
		public float range;

		// Token: 0x0400091E RID: 2334
		public float spotAngle;

		// Token: 0x0400091F RID: 2335
		public float cookieSize;

		// Token: 0x04000920 RID: 2336
		public long cookie;

		// Token: 0x04000921 RID: 2337
		public long flare;

		// Token: 0x04000922 RID: 2338
		public uint renderMode;

		// Token: 0x04000923 RID: 2339
		public bool alreadyLightmapped;

		// Token: 0x04000924 RID: 2340
		public int cullingMask;
	}
}
