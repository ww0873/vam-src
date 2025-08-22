using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006E3 RID: 1763
	public abstract class FingerModel : MonoBehaviour
	{
		// Token: 0x06002A99 RID: 10905 RVA: 0x000E651D File Offset: 0x000E491D
		protected FingerModel()
		{
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000E6544 File Offset: 0x000E4944
		public void SetLeapHand(Hand hand)
		{
			this.hand_ = hand;
			if (this.hand_ != null)
			{
				this.finger_ = hand.Fingers[(int)this.fingerType];
			}
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x000E656F File Offset: 0x000E496F
		public Hand GetLeapHand()
		{
			return this.hand_;
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x000E6577 File Offset: 0x000E4977
		public Finger GetLeapFinger()
		{
			return this.finger_;
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x000E657F File Offset: 0x000E497F
		public virtual void InitFinger()
		{
			this.UpdateFinger();
		}

		// Token: 0x06002A9E RID: 10910
		public abstract void UpdateFinger();

		// Token: 0x06002A9F RID: 10911 RVA: 0x000E6588 File Offset: 0x000E4988
		public Vector3 GetTipPosition()
		{
			if (this.finger_ != null)
			{
				return this.finger_.Bone(Bone.BoneType.TYPE_DISTAL).NextJoint.ToVector3();
			}
			if (this.bones[3] && this.joints[1])
			{
				return 2f * this.bones[3].position - this.joints[1].position;
			}
			return Vector3.zero;
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x000E660C File Offset: 0x000E4A0C
		public Vector3 GetJointPosition(int joint)
		{
			if (joint >= 4)
			{
				return this.GetTipPosition();
			}
			if (this.finger_ != null)
			{
				return this.finger_.Bone((Bone.BoneType)joint).PrevJoint.ToVector3();
			}
			if (this.joints[joint])
			{
				return this.joints[joint].position;
			}
			return Vector3.zero;
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x000E6670 File Offset: 0x000E4A70
		public Ray GetRay()
		{
			Ray result = new Ray(this.GetTipPosition(), this.GetBoneDirection(3));
			return result;
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x000E6694 File Offset: 0x000E4A94
		public Vector3 GetBoneCenter(int bone_type)
		{
			if (this.finger_ != null)
			{
				Bone bone = this.finger_.Bone((Bone.BoneType)bone_type);
				return bone.Center.ToVector3();
			}
			if (this.bones[bone_type])
			{
				return this.bones[bone_type].position;
			}
			return Vector3.zero;
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x000E66EC File Offset: 0x000E4AEC
		public Vector3 GetBoneDirection(int bone_type)
		{
			if (this.finger_ != null)
			{
				return (this.GetJointPosition(bone_type + 1) - this.GetJointPosition(bone_type)).normalized;
			}
			if (this.bones[bone_type])
			{
				return this.bones[bone_type].forward;
			}
			return Vector3.forward;
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x000E6748 File Offset: 0x000E4B48
		public Quaternion GetBoneRotation(int bone_type)
		{
			if (this.finger_ != null)
			{
				return this.finger_.Bone((Bone.BoneType)bone_type).Rotation.ToQuaternion();
			}
			if (this.bones[bone_type])
			{
				return this.bones[bone_type].rotation;
			}
			return Quaternion.identity;
		}

		// Token: 0x06002AA5 RID: 10917 RVA: 0x000E679E File Offset: 0x000E4B9E
		public float GetBoneLength(int bone_type)
		{
			return this.finger_.Bone((Bone.BoneType)bone_type).Length;
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x000E67B1 File Offset: 0x000E4BB1
		public float GetBoneWidth(int bone_type)
		{
			return this.finger_.Bone((Bone.BoneType)bone_type).Width;
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x000E67C4 File Offset: 0x000E4BC4
		public float GetFingerJointStretchMecanim(int joint_type)
		{
			Quaternion quaternion = Quaternion.identity;
			if (this.finger_ != null)
			{
				quaternion = Quaternion.Inverse(this.finger_.Bone((Bone.BoneType)joint_type).Rotation.ToQuaternion()) * this.finger_.Bone(joint_type + Bone.BoneType.TYPE_PROXIMAL).Rotation.ToQuaternion();
			}
			else if (this.bones[joint_type] && this.bones[joint_type + 1])
			{
				quaternion = Quaternion.Inverse(this.GetBoneRotation(joint_type)) * this.GetBoneRotation(joint_type + 1);
			}
			float num = -quaternion.eulerAngles.x;
			if (num <= -180f)
			{
				num += 360f;
			}
			return num;
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x000E6888 File Offset: 0x000E4C88
		public float GetFingerJointSpreadMecanim()
		{
			Quaternion quaternion = Quaternion.identity;
			if (this.finger_ != null)
			{
				quaternion = Quaternion.Inverse(this.finger_.Bone(Bone.BoneType.TYPE_METACARPAL).Rotation.ToQuaternion()) * this.finger_.Bone(Bone.BoneType.TYPE_PROXIMAL).Rotation.ToQuaternion();
			}
			else if (this.bones[0] && this.bones[1])
			{
				quaternion = Quaternion.Inverse(this.GetBoneRotation(0)) * this.GetBoneRotation(1);
			}
			float num = 0f;
			Finger.FingerType fingerType = this.fingerType;
			if (this.finger_ != null)
			{
				this.fingerType = this.finger_.Type;
			}
			if (fingerType == Finger.FingerType.TYPE_INDEX || fingerType == Finger.FingerType.TYPE_MIDDLE)
			{
				num = quaternion.eulerAngles.y;
				if (num > 180f)
				{
					num -= 360f;
				}
			}
			if (fingerType == Finger.FingerType.TYPE_THUMB || fingerType == Finger.FingerType.TYPE_RING || fingerType == Finger.FingerType.TYPE_PINKY)
			{
				num = -quaternion.eulerAngles.y;
				if (num <= -180f)
				{
					num += 360f;
				}
			}
			return num;
		}

		// Token: 0x040022A8 RID: 8872
		public const int NUM_BONES = 4;

		// Token: 0x040022A9 RID: 8873
		public const int NUM_JOINTS = 3;

		// Token: 0x040022AA RID: 8874
		public Finger.FingerType fingerType = Finger.FingerType.TYPE_INDEX;

		// Token: 0x040022AB RID: 8875
		public Transform[] bones = new Transform[4];

		// Token: 0x040022AC RID: 8876
		public Transform[] joints = new Transform[3];

		// Token: 0x040022AD RID: 8877
		protected Hand hand_;

		// Token: 0x040022AE RID: 8878
		protected Finger finger_;
	}
}
