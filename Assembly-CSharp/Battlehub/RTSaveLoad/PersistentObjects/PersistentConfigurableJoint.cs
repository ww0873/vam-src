using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000159 RID: 345
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentConfigurableJoint : PersistentJoint
	{
		// Token: 0x060007B7 RID: 1975 RVA: 0x00033CB9 File Offset: 0x000320B9
		public PersistentConfigurableJoint()
		{
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00033CC4 File Offset: 0x000320C4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ConfigurableJoint configurableJoint = (ConfigurableJoint)obj;
			configurableJoint.secondaryAxis = this.secondaryAxis;
			configurableJoint.xMotion = (ConfigurableJointMotion)this.xMotion;
			configurableJoint.yMotion = (ConfigurableJointMotion)this.yMotion;
			configurableJoint.zMotion = (ConfigurableJointMotion)this.zMotion;
			configurableJoint.angularXMotion = (ConfigurableJointMotion)this.angularXMotion;
			configurableJoint.angularYMotion = (ConfigurableJointMotion)this.angularYMotion;
			configurableJoint.angularZMotion = (ConfigurableJointMotion)this.angularZMotion;
			configurableJoint.linearLimitSpring = this.linearLimitSpring;
			configurableJoint.angularXLimitSpring = this.angularXLimitSpring;
			configurableJoint.angularYZLimitSpring = this.angularYZLimitSpring;
			configurableJoint.linearLimit = this.linearLimit;
			configurableJoint.lowAngularXLimit = this.lowAngularXLimit;
			configurableJoint.highAngularXLimit = this.highAngularXLimit;
			configurableJoint.angularYLimit = this.angularYLimit;
			configurableJoint.angularZLimit = this.angularZLimit;
			configurableJoint.targetPosition = this.targetPosition;
			configurableJoint.targetVelocity = this.targetVelocity;
			configurableJoint.xDrive = this.xDrive;
			configurableJoint.yDrive = this.yDrive;
			configurableJoint.zDrive = this.zDrive;
			configurableJoint.targetRotation = this.targetRotation;
			configurableJoint.targetAngularVelocity = this.targetAngularVelocity;
			configurableJoint.rotationDriveMode = (RotationDriveMode)this.rotationDriveMode;
			configurableJoint.angularXDrive = this.angularXDrive;
			configurableJoint.angularYZDrive = this.angularYZDrive;
			configurableJoint.slerpDrive = this.slerpDrive;
			configurableJoint.projectionMode = (JointProjectionMode)this.projectionMode;
			configurableJoint.projectionDistance = this.projectionDistance;
			configurableJoint.projectionAngle = this.projectionAngle;
			configurableJoint.configuredInWorldSpace = this.configuredInWorldSpace;
			configurableJoint.swapBodies = this.swapBodies;
			return configurableJoint;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00033E60 File Offset: 0x00032260
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ConfigurableJoint configurableJoint = (ConfigurableJoint)obj;
			this.secondaryAxis = configurableJoint.secondaryAxis;
			this.xMotion = (uint)configurableJoint.xMotion;
			this.yMotion = (uint)configurableJoint.yMotion;
			this.zMotion = (uint)configurableJoint.zMotion;
			this.angularXMotion = (uint)configurableJoint.angularXMotion;
			this.angularYMotion = (uint)configurableJoint.angularYMotion;
			this.angularZMotion = (uint)configurableJoint.angularZMotion;
			this.linearLimitSpring = configurableJoint.linearLimitSpring;
			this.angularXLimitSpring = configurableJoint.angularXLimitSpring;
			this.angularYZLimitSpring = configurableJoint.angularYZLimitSpring;
			this.linearLimit = configurableJoint.linearLimit;
			this.lowAngularXLimit = configurableJoint.lowAngularXLimit;
			this.highAngularXLimit = configurableJoint.highAngularXLimit;
			this.angularYLimit = configurableJoint.angularYLimit;
			this.angularZLimit = configurableJoint.angularZLimit;
			this.targetPosition = configurableJoint.targetPosition;
			this.targetVelocity = configurableJoint.targetVelocity;
			this.xDrive = configurableJoint.xDrive;
			this.yDrive = configurableJoint.yDrive;
			this.zDrive = configurableJoint.zDrive;
			this.targetRotation = configurableJoint.targetRotation;
			this.targetAngularVelocity = configurableJoint.targetAngularVelocity;
			this.rotationDriveMode = (uint)configurableJoint.rotationDriveMode;
			this.angularXDrive = configurableJoint.angularXDrive;
			this.angularYZDrive = configurableJoint.angularYZDrive;
			this.slerpDrive = configurableJoint.slerpDrive;
			this.projectionMode = (uint)configurableJoint.projectionMode;
			this.projectionDistance = configurableJoint.projectionDistance;
			this.projectionAngle = configurableJoint.projectionAngle;
			this.configuredInWorldSpace = configurableJoint.configuredInWorldSpace;
			this.swapBodies = configurableJoint.swapBodies;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00033FF6 File Offset: 0x000323F6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000853 RID: 2131
		public Vector3 secondaryAxis;

		// Token: 0x04000854 RID: 2132
		public uint xMotion;

		// Token: 0x04000855 RID: 2133
		public uint yMotion;

		// Token: 0x04000856 RID: 2134
		public uint zMotion;

		// Token: 0x04000857 RID: 2135
		public uint angularXMotion;

		// Token: 0x04000858 RID: 2136
		public uint angularYMotion;

		// Token: 0x04000859 RID: 2137
		public uint angularZMotion;

		// Token: 0x0400085A RID: 2138
		public SoftJointLimitSpring linearLimitSpring;

		// Token: 0x0400085B RID: 2139
		public SoftJointLimitSpring angularXLimitSpring;

		// Token: 0x0400085C RID: 2140
		public SoftJointLimitSpring angularYZLimitSpring;

		// Token: 0x0400085D RID: 2141
		public SoftJointLimit linearLimit;

		// Token: 0x0400085E RID: 2142
		public SoftJointLimit lowAngularXLimit;

		// Token: 0x0400085F RID: 2143
		public SoftJointLimit highAngularXLimit;

		// Token: 0x04000860 RID: 2144
		public SoftJointLimit angularYLimit;

		// Token: 0x04000861 RID: 2145
		public SoftJointLimit angularZLimit;

		// Token: 0x04000862 RID: 2146
		public Vector3 targetPosition;

		// Token: 0x04000863 RID: 2147
		public Vector3 targetVelocity;

		// Token: 0x04000864 RID: 2148
		public JointDrive xDrive;

		// Token: 0x04000865 RID: 2149
		public JointDrive yDrive;

		// Token: 0x04000866 RID: 2150
		public JointDrive zDrive;

		// Token: 0x04000867 RID: 2151
		public Quaternion targetRotation;

		// Token: 0x04000868 RID: 2152
		public Vector3 targetAngularVelocity;

		// Token: 0x04000869 RID: 2153
		public uint rotationDriveMode;

		// Token: 0x0400086A RID: 2154
		public JointDrive angularXDrive;

		// Token: 0x0400086B RID: 2155
		public JointDrive angularYZDrive;

		// Token: 0x0400086C RID: 2156
		public JointDrive slerpDrive;

		// Token: 0x0400086D RID: 2157
		public uint projectionMode;

		// Token: 0x0400086E RID: 2158
		public float projectionDistance;

		// Token: 0x0400086F RID: 2159
		public float projectionAngle;

		// Token: 0x04000870 RID: 2160
		public bool configuredInWorldSpace;

		// Token: 0x04000871 RID: 2161
		public bool swapBodies;
	}
}
