using System;
using Leap.Unity;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000C26 RID: 3110
	public class LeapHandModelControl : HandModelControl
	{
		// Token: 0x06005A71 RID: 23153 RVA: 0x0021331B File Offset: 0x0021171B
		public LeapHandModelControl()
		{
		}

		// Token: 0x06005A72 RID: 23154 RVA: 0x00213324 File Offset: 0x00211724
		protected void SyncHands()
		{
			if (this.handModelManager != null)
			{
				this.handModelManager.DisableGroup("Main");
				this.handModelManager.RemoveGroupWait("Main");
				this.handModelManager.AddNewGroupWait("Main", this.currentLeftHand, this.currentRightHand);
				this.handModelManager.EnableGroup("Main");
			}
		}

		// Token: 0x06005A73 RID: 23155 RVA: 0x00213390 File Offset: 0x00211790
		protected override void SetLeftHand(string choice)
		{
			bool flag = false;
			for (int i = 0; i < this.leftHands.Length; i++)
			{
				if (this.leftHands[i].name == choice)
				{
					this._leftHandChoice = choice;
					if (this.leftHands[i].transform != null)
					{
						if (this._leftHandEnabled)
						{
							RiggedHand component = this.leftHands[i].transform.GetComponent<RiggedHand>();
							this.currentLeftHand = component;
						}
						else
						{
							this.currentLeftHand = null;
						}
					}
					else
					{
						this.currentLeftHand = null;
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this._leftHandChoice = "None";
				this.currentLeftHand = null;
			}
			this.SyncHands();
		}

		// Token: 0x06005A74 RID: 23156 RVA: 0x00213454 File Offset: 0x00211854
		protected override void SetRightHand(string choice)
		{
			bool flag = false;
			for (int i = 0; i < this.rightHands.Length; i++)
			{
				if (this.rightHands[i].name == choice)
				{
					this._rightHandChoice = choice;
					if (this.rightHands[i].transform != null)
					{
						if (this._rightHandEnabled)
						{
							RiggedHand component = this.rightHands[i].transform.GetComponent<RiggedHand>();
							this.currentRightHand = component;
						}
						else
						{
							this.currentRightHand = null;
						}
					}
					else
					{
						this.currentRightHand = null;
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this._rightHandChoice = "None";
				this.currentRightHand = null;
			}
			this.SyncHands();
		}

		// Token: 0x06005A75 RID: 23157 RVA: 0x00213515 File Offset: 0x00211915
		protected void FindGrabbers()
		{
			this.grabbers = base.GetComponentsInChildren<LeapMotionGrabber>(true);
		}

		// Token: 0x06005A76 RID: 23158 RVA: 0x00213524 File Offset: 0x00211924
		protected void SetPinchGrab(bool b)
		{
			this._allowPinchGrab = b;
			foreach (LeapMotionGrabber leapMotionGrabber in this.grabbers)
			{
				leapMotionGrabber.controllerGrabOn = b;
			}
		}

		// Token: 0x06005A77 RID: 23159 RVA: 0x0021355E File Offset: 0x0021195E
		protected void SyncAllowPinchGrab(bool b)
		{
			this.SetPinchGrab(b);
			UserPreferences.singleton.SavePreferences();
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06005A78 RID: 23160 RVA: 0x00213571 File Offset: 0x00211971
		// (set) Token: 0x06005A79 RID: 23161 RVA: 0x00213590 File Offset: 0x00211990
		public bool allowPinchGrab
		{
			get
			{
				if (this.allowPinchGrabJSON != null)
				{
					return this.allowPinchGrabJSON.val;
				}
				return this._allowPinchGrab;
			}
			set
			{
				if (this.allowPinchGrabJSON != null)
				{
					this.allowPinchGrabJSON.valNoCallback = value;
				}
				this.SetPinchGrab(value);
			}
		}

		// Token: 0x06005A7A RID: 23162 RVA: 0x002135B0 File Offset: 0x002119B0
		protected override void Init()
		{
			base.Init();
			this.allowPinchGrabJSON = new JSONStorableBool("allowPinchGrab", this._allowPinchGrab, new JSONStorableBool.SetBoolCallback(this.SyncAllowPinchGrab));
			this.FindGrabbers();
			this.SetPinchGrab(this._allowPinchGrab);
		}

		// Token: 0x06005A7B RID: 23163 RVA: 0x002135EC File Offset: 0x002119EC
		protected override void InitUI(Transform t, bool isAlt)
		{
			base.InitUI(t, isAlt);
			if (t != null)
			{
				LeapHandModelControlUI componentInChildren = t.GetComponentInChildren<LeapHandModelControlUI>(true);
				if (componentInChildren != null)
				{
					this.allowPinchGrabJSON.RegisterToggle(componentInChildren.allowPinchGrabToggle, isAlt);
				}
			}
		}

		// Token: 0x04004AB7 RID: 19127
		public HandModelManager handModelManager;

		// Token: 0x04004AB8 RID: 19128
		protected RiggedHand currentLeftHand;

		// Token: 0x04004AB9 RID: 19129
		protected RiggedHand currentRightHand;

		// Token: 0x04004ABA RID: 19130
		protected LeapMotionGrabber[] grabbers;

		// Token: 0x04004ABB RID: 19131
		[SerializeField]
		protected bool _allowPinchGrab;

		// Token: 0x04004ABC RID: 19132
		protected JSONStorableBool allowPinchGrabJSON;
	}
}
