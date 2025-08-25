using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000152 RID: 338
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCharacterJoint : PersistentJoint
	{
		// Token: 0x0600079C RID: 1948 RVA: 0x00033A2C File Offset: 0x00031E2C
		public PersistentCharacterJoint()
		{
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00033A34 File Offset: 0x00031E34
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			CharacterJoint characterJoint = (CharacterJoint)obj;
			characterJoint.swingAxis = this.swingAxis;
			characterJoint.twistLimitSpring = this.twistLimitSpring;
			characterJoint.swingLimitSpring = this.swingLimitSpring;
			characterJoint.lowTwistLimit = this.lowTwistLimit;
			characterJoint.highTwistLimit = this.highTwistLimit;
			characterJoint.swing1Limit = this.swing1Limit;
			characterJoint.swing2Limit = this.swing2Limit;
			characterJoint.enableProjection = this.enableProjection;
			characterJoint.projectionDistance = this.projectionDistance;
			characterJoint.projectionAngle = this.projectionAngle;
			return characterJoint;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00033AD4 File Offset: 0x00031ED4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			CharacterJoint characterJoint = (CharacterJoint)obj;
			this.swingAxis = characterJoint.swingAxis;
			this.twistLimitSpring = characterJoint.twistLimitSpring;
			this.swingLimitSpring = characterJoint.swingLimitSpring;
			this.lowTwistLimit = characterJoint.lowTwistLimit;
			this.highTwistLimit = characterJoint.highTwistLimit;
			this.swing1Limit = characterJoint.swing1Limit;
			this.swing2Limit = characterJoint.swing2Limit;
			this.enableProjection = characterJoint.enableProjection;
			this.projectionDistance = characterJoint.projectionDistance;
			this.projectionAngle = characterJoint.projectionAngle;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00033B6E File Offset: 0x00031F6E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000839 RID: 2105
		public Vector3 swingAxis;

		// Token: 0x0400083A RID: 2106
		public SoftJointLimitSpring twistLimitSpring;

		// Token: 0x0400083B RID: 2107
		public SoftJointLimitSpring swingLimitSpring;

		// Token: 0x0400083C RID: 2108
		public SoftJointLimit lowTwistLimit;

		// Token: 0x0400083D RID: 2109
		public SoftJointLimit highTwistLimit;

		// Token: 0x0400083E RID: 2110
		public SoftJointLimit swing1Limit;

		// Token: 0x0400083F RID: 2111
		public SoftJointLimit swing2Limit;

		// Token: 0x04000840 RID: 2112
		public bool enableProjection;

		// Token: 0x04000841 RID: 2113
		public float projectionDistance;

		// Token: 0x04000842 RID: 2114
		public float projectionAngle;
	}
}
