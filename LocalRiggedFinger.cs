using System;
using Leap;
using Leap.Unity;
using UnityEngine;

// Token: 0x02000BAC RID: 2988
public class LocalRiggedFinger : RiggedFinger
{
	// Token: 0x0600554F RID: 21839 RVA: 0x001F35A3 File Offset: 0x001F19A3
	public LocalRiggedFinger()
	{
	}

	// Token: 0x06005550 RID: 21840 RVA: 0x001F35AC File Offset: 0x001F19AC
	public override void UpdateFinger()
	{
		for (int i = 0; i < this.bones.Length; i++)
		{
			if (this.bones[i] != null)
			{
				if (this.finger_ != null)
				{
					Quaternion rhs = this.finger_.Bone((Bone.BoneType)i).Rotation.ToQuaternion() * base.Reorientation();
					Quaternion rotation;
					if (i == 0 || this.bones[i - 1] == null)
					{
						rotation = base.transform.parent.rotation;
					}
					else
					{
						rotation = this.finger_.Bone((Bone.BoneType)(i - 1)).Rotation.ToQuaternion() * base.Reorientation();
					}
					Quaternion localRotation = Quaternion.Inverse(rotation) * rhs;
					this.bones[i].localRotation = localRotation;
				}
				else
				{
					Debug.LogError("Finger not set");
				}
			}
		}
	}
}
