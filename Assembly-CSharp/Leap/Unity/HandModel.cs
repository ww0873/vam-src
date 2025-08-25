using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006E6 RID: 1766
	public abstract class HandModel : HandModelBase
	{
		// Token: 0x06002AB2 RID: 10930 RVA: 0x000E6CCD File Offset: 0x000E50CD
		protected HandModel()
		{
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06002AB3 RID: 10931 RVA: 0x000E6CEC File Offset: 0x000E50EC
		// (set) Token: 0x06002AB4 RID: 10932 RVA: 0x000E6CF4 File Offset: 0x000E50F4
		public override Chirality Handedness
		{
			get
			{
				return this.handedness;
			}
			set
			{
				this.handedness = value;
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06002AB5 RID: 10933
		public abstract override ModelType HandModelType { get; }

		// Token: 0x06002AB6 RID: 10934 RVA: 0x000E6CFD File Offset: 0x000E50FD
		public Vector3 GetPalmPosition()
		{
			return this.hand_.PalmPosition.ToVector3();
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x000E6D0F File Offset: 0x000E510F
		public Quaternion GetPalmRotation()
		{
			if (this.hand_ != null)
			{
				return this.hand_.Basis.CalculateRotation();
			}
			if (this.palm)
			{
				return this.palm.rotation;
			}
			return Quaternion.identity;
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x000E6D4E File Offset: 0x000E514E
		public Vector3 GetPalmDirection()
		{
			if (this.hand_ != null)
			{
				return this.hand_.Direction.ToVector3();
			}
			if (this.palm)
			{
				return this.palm.forward;
			}
			return Vector3.forward;
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x000E6D90 File Offset: 0x000E5190
		public Vector3 GetPalmNormal()
		{
			if (this.hand_ != null)
			{
				return this.hand_.PalmNormal.ToVector3();
			}
			if (this.palm)
			{
				return -this.palm.up;
			}
			return -Vector3.up;
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x000E6DE4 File Offset: 0x000E51E4
		public Vector3 GetArmDirection()
		{
			if (this.hand_ != null)
			{
				return this.hand_.Arm.Direction.ToVector3();
			}
			if (this.forearm)
			{
				return this.forearm.forward;
			}
			return Vector3.forward;
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x000E6E34 File Offset: 0x000E5234
		public Vector3 GetArmCenter()
		{
			if (this.hand_ != null)
			{
				Vector vector = 0.5f * (this.hand_.Arm.WristPosition + this.hand_.Arm.ElbowPosition);
				return vector.ToVector3();
			}
			if (this.forearm)
			{
				return this.forearm.position;
			}
			return Vector3.zero;
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x000E6EA4 File Offset: 0x000E52A4
		public float GetArmLength()
		{
			return (this.hand_.Arm.WristPosition - this.hand_.Arm.ElbowPosition).Magnitude;
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x000E6EDE File Offset: 0x000E52DE
		public float GetArmWidth()
		{
			return this.hand_.Arm.Width;
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x000E6EF0 File Offset: 0x000E52F0
		public Vector3 GetElbowPosition()
		{
			if (this.hand_ != null)
			{
				return this.hand_.Arm.ElbowPosition.ToVector3();
			}
			if (this.elbowJoint)
			{
				return this.elbowJoint.position;
			}
			return Vector3.zero;
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x000E6F44 File Offset: 0x000E5344
		public Vector3 GetWristPosition()
		{
			if (this.hand_ != null)
			{
				return this.hand_.Arm.WristPosition.ToVector3();
			}
			if (this.wristJoint)
			{
				return this.wristJoint.position;
			}
			return Vector3.zero;
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x000E6F98 File Offset: 0x000E5398
		public Quaternion GetArmRotation()
		{
			if (this.hand_ != null)
			{
				return this.hand_.Arm.Rotation.ToQuaternion();
			}
			if (this.forearm)
			{
				return this.forearm.rotation;
			}
			return Quaternion.identity;
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x000E6FE9 File Offset: 0x000E53E9
		public override Hand GetLeapHand()
		{
			return this.hand_;
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000E6FF4 File Offset: 0x000E53F4
		public override void SetLeapHand(Hand hand)
		{
			this.hand_ = hand;
			for (int i = 0; i < this.fingers.Length; i++)
			{
				if (this.fingers[i] != null)
				{
					this.fingers[i].SetLeapHand(this.hand_);
				}
			}
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x000E7048 File Offset: 0x000E5448
		public override void InitHand()
		{
			for (int i = 0; i < this.fingers.Length; i++)
			{
				if (this.fingers[i] != null)
				{
					this.fingers[i].fingerType = (Finger.FingerType)i;
					this.fingers[i].InitFinger();
				}
			}
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x000E709C File Offset: 0x000E549C
		public int LeapID()
		{
			if (this.hand_ != null)
			{
				return this.hand_.Id;
			}
			return -1;
		}

		// Token: 0x06002AC5 RID: 10949
		public abstract override void UpdateHand();

		// Token: 0x040022B2 RID: 8882
		[SerializeField]
		private Chirality handedness;

		// Token: 0x040022B3 RID: 8883
		private ModelType handModelType;

		// Token: 0x040022B4 RID: 8884
		public const int NUM_FINGERS = 5;

		// Token: 0x040022B5 RID: 8885
		public float handModelPalmWidth = 0.085f;

		// Token: 0x040022B6 RID: 8886
		public FingerModel[] fingers = new FingerModel[5];

		// Token: 0x040022B7 RID: 8887
		public Transform palm;

		// Token: 0x040022B8 RID: 8888
		public Transform forearm;

		// Token: 0x040022B9 RID: 8889
		public Transform wristJoint;

		// Token: 0x040022BA RID: 8890
		public Transform elbowJoint;

		// Token: 0x040022BB RID: 8891
		protected Hand hand_;
	}
}
