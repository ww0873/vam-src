using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Leap.Unity.Attachments
{
	// Token: 0x02000669 RID: 1641
	[ExecuteInEditMode]
	public class AttachmentHands : MonoBehaviour
	{
		// Token: 0x06002830 RID: 10288 RVA: 0x000DDB21 File Offset: 0x000DBF21
		public AttachmentHands()
		{
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06002831 RID: 10289 RVA: 0x000DDB30 File Offset: 0x000DBF30
		// (set) Token: 0x06002832 RID: 10290 RVA: 0x000DDB38 File Offset: 0x000DBF38
		public AttachmentPointFlags attachmentPoints
		{
			get
			{
				return this._attachmentPoints;
			}
			set
			{
				if (this._attachmentPoints != value)
				{
					this._attachmentPoints = value;
					this.refreshAttachmentHandTransforms();
				}
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06002833 RID: 10291 RVA: 0x000DDB53 File Offset: 0x000DBF53
		// (set) Token: 0x06002834 RID: 10292 RVA: 0x000DDB5B File Offset: 0x000DBF5B
		public Func<Hand>[] handAccessors
		{
			get
			{
				return this._handAccessors;
			}
			set
			{
				this._handAccessors = value;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06002835 RID: 10293 RVA: 0x000DDB64 File Offset: 0x000DBF64
		// (set) Token: 0x06002836 RID: 10294 RVA: 0x000DDB6C File Offset: 0x000DBF6C
		public AttachmentHand[] attachmentHands
		{
			get
			{
				return this._attachmentHands;
			}
			set
			{
				this._attachmentHands = value;
			}
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x000DDB75 File Offset: 0x000DBF75
		private void Awake()
		{
			this.reinitialize();
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x000DDB7D File Offset: 0x000DBF7D
		private void reinitialize()
		{
			this.refreshHandAccessors();
			this.refreshAttachmentHands();
			this.refreshAttachmentHandTransforms();
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x000DDB94 File Offset: 0x000DBF94
		private void Update()
		{
			bool flag = false;
			using (new ProfilerSample("Attachment Hands Update", base.gameObject))
			{
				for (int i = 0; i < this._attachmentHands.Length; i++)
				{
					AttachmentHand attachmentHand = this.attachmentHands[i];
					if (attachmentHand == null)
					{
						flag = true;
						break;
					}
					Hand hand = this.handAccessors[i]();
					attachmentHand.isTracked = (hand != null);
					using (new ProfilerSample(attachmentHand.gameObject.name + " Update Points"))
					{
						foreach (AttachmentPointBehaviour attachmentPointBehaviour in attachmentHand.points)
						{
							attachmentPointBehaviour.SetTransformUsingHand(hand);
						}
					}
				}
				if (flag)
				{
					this.reinitialize();
				}
			}
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x000DDCA8 File Offset: 0x000DC0A8
		private void refreshHandAccessors()
		{
			if (this._handAccessors == null || this._handAccessors.Length == 0)
			{
				this._handAccessors = new Func<Hand>[2];
				Func<Hand>[] handAccessors = this._handAccessors;
				int num = 0;
				if (AttachmentHands.<>f__am$cache0 == null)
				{
					AttachmentHands.<>f__am$cache0 = new Func<Hand>(AttachmentHands.<refreshHandAccessors>m__0);
				}
				handAccessors[num] = AttachmentHands.<>f__am$cache0;
				Func<Hand>[] handAccessors2 = this._handAccessors;
				int num2 = 1;
				if (AttachmentHands.<>f__am$cache1 == null)
				{
					AttachmentHands.<>f__am$cache1 = new Func<Hand>(AttachmentHands.<refreshHandAccessors>m__1);
				}
				handAccessors2[num2] = AttachmentHands.<>f__am$cache1;
			}
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x000DDD24 File Offset: 0x000DC124
		private void refreshAttachmentHands()
		{
			bool flag = false;
			if (this._attachmentHands == null || this._attachmentHands.Length == 0 || this._attachmentHands[0] == null || this._attachmentHands[1] == null)
			{
				this._attachmentHands = new AttachmentHand[2];
				foreach (Transform transform in base.transform.GetChildren())
				{
					AttachmentHand component = transform.GetComponent<AttachmentHand>();
					if (component != null)
					{
						this._attachmentHands[(component.chirality != Chirality.Left) ? 1 : 0] = component;
					}
				}
				if (flag && (this._attachmentHands[0] == null || this._attachmentHands[0].transform.parent != base.transform || this._attachmentHands[1] == null || this._attachmentHands[1].transform.parent != base.transform))
				{
					return;
				}
				if (this._attachmentHands[0] == null)
				{
					GameObject gameObject = new GameObject();
					this._attachmentHands[0] = gameObject.AddComponent<AttachmentHand>();
					this._attachmentHands[0].chirality = Chirality.Left;
				}
				this._attachmentHands[0].gameObject.name = "Attachment Hand (Left)";
				if (this._attachmentHands[0].transform.parent != base.transform)
				{
					this._attachmentHands[0].transform.parent = base.transform;
				}
				if (this._attachmentHands[1] == null)
				{
					GameObject gameObject2 = new GameObject();
					this._attachmentHands[1] = gameObject2.AddComponent<AttachmentHand>();
					this._attachmentHands[1].chirality = Chirality.Right;
				}
				this._attachmentHands[1].gameObject.name = "Attachment Hand (Right)";
				if (this._attachmentHands[1].transform.parent != base.transform)
				{
					this._attachmentHands[1].transform.parent = base.transform;
				}
				this._attachmentHands[0].transform.SetSiblingIndex(0);
				this._attachmentHands[1].transform.SetSiblingIndex(1);
			}
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x000DDFA8 File Offset: 0x000DC3A8
		private void refreshAttachmentHandTransforms()
		{
			if (this == null)
			{
				return;
			}
			bool flag = false;
			if (this._attachmentHands == null)
			{
				flag = true;
			}
			else
			{
				foreach (AttachmentHand attachmentHand in this._attachmentHands)
				{
					if (attachmentHand == null)
					{
						flag = true;
						break;
					}
					attachmentHand.refreshAttachmentTransforms(this._attachmentPoints);
				}
			}
			if (flag)
			{
				this.reinitialize();
			}
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x000DE020 File Offset: 0x000DC420
		[CompilerGenerated]
		private static Hand <refreshHandAccessors>m__0()
		{
			return Hands.Left;
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x000DE027 File Offset: 0x000DC427
		[CompilerGenerated]
		private static Hand <refreshHandAccessors>m__1()
		{
			return Hands.Right;
		}

		// Token: 0x0400217E RID: 8574
		[SerializeField]
		private AttachmentPointFlags _attachmentPoints = AttachmentPointFlags.Wrist | AttachmentPointFlags.Palm;

		// Token: 0x0400217F RID: 8575
		private Func<Hand>[] _handAccessors;

		// Token: 0x04002180 RID: 8576
		private AttachmentHand[] _attachmentHands;

		// Token: 0x04002181 RID: 8577
		[CompilerGenerated]
		private static Func<Hand> <>f__am$cache0;

		// Token: 0x04002182 RID: 8578
		[CompilerGenerated]
		private static Func<Hand> <>f__am$cache1;
	}
}
