using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200010E RID: 270
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentColorBySpeedModule : PersistentData
	{
		// Token: 0x0600068E RID: 1678 RVA: 0x0002D20E File Offset: 0x0002B60E
		public PersistentColorBySpeedModule()
		{
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0002D218 File Offset: 0x0002B618
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.ColorBySpeedModule colorBySpeedModule = (ParticleSystem.ColorBySpeedModule)obj;
			colorBySpeedModule.enabled = this.enabled;
			colorBySpeedModule.color = base.Write<ParticleSystem.MinMaxGradient>(colorBySpeedModule.color, this.color, objects);
			colorBySpeedModule.range = this.range;
			return colorBySpeedModule;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0002D27C File Offset: 0x0002B67C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.ColorBySpeedModule colorBySpeedModule = (ParticleSystem.ColorBySpeedModule)obj;
			this.enabled = colorBySpeedModule.enabled;
			this.color = base.Read<PersistentMinMaxGradient>(this.color, colorBySpeedModule.color);
			this.range = colorBySpeedModule.range;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0002D2D6 File Offset: 0x0002B6D6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxGradient>(this.color, dependencies, objects, allowNulls);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0002D2F0 File Offset: 0x0002B6F0
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.ColorBySpeedModule colorBySpeedModule = (ParticleSystem.ColorBySpeedModule)obj;
			base.GetDependencies<PersistentMinMaxGradient>(this.color, colorBySpeedModule.color, dependencies);
		}

		// Token: 0x0400065B RID: 1627
		public bool enabled;

		// Token: 0x0400065C RID: 1628
		public PersistentMinMaxGradient color;

		// Token: 0x0400065D RID: 1629
		public Vector2 range;
	}
}
