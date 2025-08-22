using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006E2 RID: 1762
	public class DebugHand : HandModelBase
	{
		// Token: 0x06002A8C RID: 10892 RVA: 0x000E6260 File Offset: 0x000E4660
		public DebugHand()
		{
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06002A8D RID: 10893 RVA: 0x000E62CA File Offset: 0x000E46CA
		// (set) Token: 0x06002A8E RID: 10894 RVA: 0x000E62D2 File Offset: 0x000E46D2
		public bool VisualizeBasis
		{
			get
			{
				return this.visualizeBasis;
			}
			set
			{
				this.visualizeBasis = value;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06002A8F RID: 10895 RVA: 0x000E62DB File Offset: 0x000E46DB
		public override ModelType HandModelType
		{
			get
			{
				return ModelType.Graphics;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06002A90 RID: 10896 RVA: 0x000E62DE File Offset: 0x000E46DE
		// (set) Token: 0x06002A91 RID: 10897 RVA: 0x000E62E6 File Offset: 0x000E46E6
		public override Chirality Handedness
		{
			get
			{
				return this.handedness;
			}
			set
			{
			}
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x000E62E8 File Offset: 0x000E46E8
		public override Hand GetLeapHand()
		{
			return this.hand_;
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x000E62F0 File Offset: 0x000E46F0
		public override void SetLeapHand(Hand hand)
		{
			this.hand_ = hand;
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x000E62F9 File Offset: 0x000E46F9
		public override bool SupportsEditorPersistence()
		{
			return true;
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x000E62FC File Offset: 0x000E46FC
		public override void InitHand()
		{
			this.DrawDebugLines();
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000E6304 File Offset: 0x000E4704
		public override void UpdateHand()
		{
			this.DrawDebugLines();
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x000E630C File Offset: 0x000E470C
		protected void DrawDebugLines()
		{
			Hand leapHand = this.GetLeapHand();
			Debug.DrawLine(leapHand.Arm.ElbowPosition.ToVector3(), leapHand.Arm.WristPosition.ToVector3(), Color.red);
			Debug.DrawLine(leapHand.WristPosition.ToVector3(), leapHand.PalmPosition.ToVector3(), Color.white);
			Debug.DrawLine(leapHand.PalmPosition.ToVector3(), (leapHand.PalmPosition + leapHand.PalmNormal * leapHand.PalmWidth / 2f).ToVector3(), Color.black);
			if (this.VisualizeBasis)
			{
				this.DrawBasis(leapHand.PalmPosition, leapHand.Basis, leapHand.PalmWidth / 4f);
				this.DrawBasis(leapHand.Arm.ElbowPosition, leapHand.Arm.Basis, 0.01f);
			}
			for (int i = 0; i < 5; i++)
			{
				Finger finger = leapHand.Fingers[i];
				for (int j = 0; j < 4; j++)
				{
					Bone bone = finger.Bone((Bone.BoneType)j);
					Debug.DrawLine(bone.PrevJoint.ToVector3(), bone.PrevJoint.ToVector3() + bone.Direction.ToVector3() * bone.Length, this.colors[j]);
					if (this.VisualizeBasis)
					{
						this.DrawBasis(bone.PrevJoint, bone.Basis, 0.01f);
					}
				}
			}
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x000E64A0 File Offset: 0x000E48A0
		public void DrawBasis(Vector position, LeapTransform basis, float scale)
		{
			Vector3 vector = position.ToVector3();
			Debug.DrawLine(vector, vector + basis.xBasis.ToVector3() * scale, Color.red);
			Debug.DrawLine(vector, vector + basis.yBasis.ToVector3() * scale, Color.green);
			Debug.DrawLine(vector, vector + basis.zBasis.ToVector3() * scale, Color.blue);
		}

		// Token: 0x040022A4 RID: 8868
		private Hand hand_;

		// Token: 0x040022A5 RID: 8869
		[SerializeField]
		private bool visualizeBasis = true;

		// Token: 0x040022A6 RID: 8870
		protected Color[] colors = new Color[]
		{
			Color.gray,
			Color.yellow,
			Color.cyan,
			Color.magenta
		};

		// Token: 0x040022A7 RID: 8871
		[SerializeField]
		private Chirality handedness;
	}
}
