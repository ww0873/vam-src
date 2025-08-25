using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001B8 RID: 440
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSliderJoint2D : PersistentAnchoredJoint2D
	{
		// Token: 0x06000915 RID: 2325 RVA: 0x00038CA8 File Offset: 0x000370A8
		public PersistentSliderJoint2D()
		{
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x00038CB0 File Offset: 0x000370B0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			SliderJoint2D sliderJoint2D = (SliderJoint2D)obj;
			sliderJoint2D.autoConfigureAngle = this.autoConfigureAngle;
			sliderJoint2D.angle = this.angle;
			sliderJoint2D.useMotor = this.useMotor;
			sliderJoint2D.useLimits = this.useLimits;
			sliderJoint2D.motor = this.motor;
			sliderJoint2D.limits = this.limits;
			return sliderJoint2D;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00038D20 File Offset: 0x00037120
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			SliderJoint2D sliderJoint2D = (SliderJoint2D)obj;
			this.autoConfigureAngle = sliderJoint2D.autoConfigureAngle;
			this.angle = sliderJoint2D.angle;
			this.useMotor = sliderJoint2D.useMotor;
			this.useLimits = sliderJoint2D.useLimits;
			this.motor = sliderJoint2D.motor;
			this.limits = sliderJoint2D.limits;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00038D8A File Offset: 0x0003718A
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000A19 RID: 2585
		public bool autoConfigureAngle;

		// Token: 0x04000A1A RID: 2586
		public float angle;

		// Token: 0x04000A1B RID: 2587
		public bool useMotor;

		// Token: 0x04000A1C RID: 2588
		public bool useLimits;

		// Token: 0x04000A1D RID: 2589
		public JointMotor2D motor;

		// Token: 0x04000A1E RID: 2590
		public JointTranslationLimits2D limits;
	}
}
