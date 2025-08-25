using System;
using UnityEngine;

// Token: 0x02000BC7 RID: 3015
public class SlaveConfigurableJointDrive : MonoBehaviour
{
	// Token: 0x060055BE RID: 21950 RVA: 0x001F5898 File Offset: 0x001F3C98
	public SlaveConfigurableJointDrive()
	{
	}

	// Token: 0x060055BF RID: 21951 RVA: 0x001F58A0 File Offset: 0x001F3CA0
	private void Start()
	{
		this.joint = base.GetComponent<ConfigurableJoint>();
		this.inverseStartingLocalRotation = Quaternion.Inverse(base.transform.localRotation);
	}

	// Token: 0x060055C0 RID: 21952 RVA: 0x001F58C4 File Offset: 0x001F3CC4
	private void Update()
	{
		if (this.joint != null)
		{
			Quaternion quaternion = this.inverseStartingLocalRotation * this.slaveTo.localRotation;
			Quaternion targetRotation = quaternion;
			if (this.joint.axis.x == 1f)
			{
				targetRotation.x = -quaternion.x;
				if (this.joint.secondaryAxis.x == 1f)
				{
					Debug.LogError("Illegal joint setup");
				}
				else if (this.joint.secondaryAxis.y == 1f)
				{
					targetRotation.y = -quaternion.y;
					targetRotation.z = -quaternion.z;
				}
				else if (this.joint.secondaryAxis.z == 1f)
				{
					targetRotation.y = -quaternion.z;
					targetRotation.z = -quaternion.y;
				}
				else
				{
					Debug.LogError("Did not account for secondary axis case " + this.joint.secondaryAxis);
				}
			}
			else if (this.joint.axis.y == 1f)
			{
				targetRotation.x = -quaternion.y;
				if (this.joint.secondaryAxis.x == 1f)
				{
					targetRotation.y = -quaternion.x;
					targetRotation.z = -quaternion.z;
				}
				else if (this.joint.secondaryAxis.y == 1f)
				{
					Debug.LogError("Illegal joint setup");
				}
				else if (this.joint.secondaryAxis.z == 1f)
				{
					targetRotation.y = -quaternion.z;
					targetRotation.z = -quaternion.x;
				}
				else
				{
					Debug.LogError("Did not account for secondary axis case " + this.joint.secondaryAxis);
				}
			}
			else if (this.joint.axis.z == 1f)
			{
				targetRotation.x = -quaternion.z;
				if (this.joint.secondaryAxis.x == 1f)
				{
					targetRotation.y = -quaternion.x;
					targetRotation.z = -quaternion.y;
				}
				else if (this.joint.secondaryAxis.y == 1f)
				{
					targetRotation.y = -quaternion.y;
					targetRotation.z = -quaternion.x;
				}
				else if (this.joint.secondaryAxis.z == 1f)
				{
					Debug.LogError("Illegal joint setup");
				}
				else
				{
					Debug.LogError("Did not account for secondary axis case " + this.joint.secondaryAxis);
				}
			}
			else
			{
				Debug.LogError("Did not account for axis case " + this.joint.axis);
			}
			this.joint.targetRotation = targetRotation;
		}
	}

	// Token: 0x040046E1 RID: 18145
	public Transform slaveTo;

	// Token: 0x040046E2 RID: 18146
	protected ConfigurableJoint joint;

	// Token: 0x040046E3 RID: 18147
	protected Quaternion inverseStartingLocalRotation;
}
