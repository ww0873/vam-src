using System;
using Leap.Unity;
using UnityEngine;

// Token: 0x02000BBE RID: 3006
public class PhysicalRiggedFinger : RiggedFinger
{
	// Token: 0x0600559B RID: 21915 RVA: 0x001F4C51 File Offset: 0x001F3051
	public PhysicalRiggedFinger()
	{
	}

	// Token: 0x0600559C RID: 21916 RVA: 0x001F4C5C File Offset: 0x001F305C
	private void OnEnable()
	{
		this.configurableJoints = new ConfigurableJoint[this.bones.Length];
		this.inverseStartingLocalRotations = new Quaternion[this.bones.Length];
		for (int i = 0; i < this.bones.Length; i++)
		{
			if (this.bones[i] != null)
			{
				ConfigurableJoint component = this.bones[i].GetComponent<ConfigurableJoint>();
				this.configurableJoints[i] = component;
				this.inverseStartingLocalRotations[i] = Quaternion.Inverse(this.bones[i].transform.localRotation);
			}
		}
	}

	// Token: 0x0600559D RID: 21917 RVA: 0x001F4CFC File Offset: 0x001F30FC
	public override void UpdateFinger()
	{
		for (int i = 0; i < this.bones.Length; i++)
		{
			if (this.bones[i] != null && this.configurableJoints != null && i < this.configurableJoints.Length)
			{
				ConfigurableJoint configurableJoint = this.configurableJoints[i];
				if (configurableJoint != null)
				{
					Quaternion rhs = base.GetBoneRotation(i) * base.Reorientation();
					Quaternion rotation;
					if (i == 0 || this.bones[i - 1] == null)
					{
						rotation = configurableJoint.connectedBody.transform.rotation;
					}
					else
					{
						rotation = base.GetBoneRotation(i - 1) * base.Reorientation();
					}
					Quaternion quaternion = Quaternion.Inverse(rotation) * rhs;
					Quaternion quaternion2 = quaternion;
					Quaternion targetRotation = quaternion2;
					if (configurableJoint.axis.x == 1f)
					{
						targetRotation.x = -quaternion2.x;
						if (configurableJoint.secondaryAxis.x == 1f)
						{
							Debug.LogError("Illegal joint setup");
						}
						else if (configurableJoint.secondaryAxis.y == 1f)
						{
							targetRotation.y = -quaternion2.y;
							targetRotation.z = -quaternion2.z;
						}
						else if (configurableJoint.secondaryAxis.z == 1f)
						{
							targetRotation.y = -quaternion2.z;
							targetRotation.z = -quaternion2.y;
						}
						else
						{
							Debug.LogError("Did not account for secondary axis case " + configurableJoint.secondaryAxis);
						}
					}
					else if (configurableJoint.axis.y == 1f)
					{
						targetRotation.x = -quaternion2.y;
						if (configurableJoint.secondaryAxis.x == 1f)
						{
							targetRotation.y = -quaternion2.x;
							targetRotation.z = -quaternion2.z;
						}
						else if (configurableJoint.secondaryAxis.y == 1f)
						{
							Debug.LogError("Illegal joint setup");
						}
						else if (configurableJoint.secondaryAxis.z == 1f)
						{
							targetRotation.y = -quaternion2.z;
							targetRotation.z = -quaternion2.x;
						}
						else
						{
							Debug.LogError("Did not account for secondary axis case " + configurableJoint.secondaryAxis);
						}
					}
					else if (configurableJoint.axis.z == 1f)
					{
						targetRotation.x = -quaternion2.z;
						if (configurableJoint.secondaryAxis.x == 1f)
						{
							targetRotation.y = -quaternion2.x;
							targetRotation.z = -quaternion2.y;
						}
						else if (configurableJoint.secondaryAxis.y == 1f)
						{
							targetRotation.y = -quaternion2.y;
							targetRotation.z = -quaternion2.x;
						}
						else if (configurableJoint.secondaryAxis.z == 1f)
						{
							Debug.LogError("Illegal joint setup");
						}
						else
						{
							Debug.LogError("Did not account for secondary axis case " + configurableJoint.secondaryAxis);
						}
					}
					else
					{
						Debug.LogError("Did not account for axis case " + configurableJoint.axis);
					}
					configurableJoint.targetRotation = targetRotation;
				}
			}
		}
	}

	// Token: 0x040046C6 RID: 18118
	public ConfigurableJoint[] configurableJoints;

	// Token: 0x040046C7 RID: 18119
	protected Quaternion[] inverseStartingLocalRotations;
}
