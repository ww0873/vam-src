using System;
using UnityEngine;

namespace MeshVR.Hands
{
	// Token: 0x02000E22 RID: 3618
	public class HandOutput : JSONStorable
	{
		// Token: 0x06006F42 RID: 28482 RVA: 0x0029D191 File Offset: 0x0029B591
		public HandOutput()
		{
		}

		// Token: 0x06006F43 RID: 28483 RVA: 0x0029D1A7 File Offset: 0x0029B5A7
		protected void SetInputConnected(bool b)
		{
			this._inputConnected = b;
		}

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x06006F44 RID: 28484 RVA: 0x0029D1B0 File Offset: 0x0029B5B0
		// (set) Token: 0x06006F45 RID: 28485 RVA: 0x0029D1CF File Offset: 0x0029B5CF
		public bool inputConnected
		{
			get
			{
				if (this.inputConnectedJSON != null)
				{
					return this.inputConnectedJSON.val;
				}
				return this._inputConnected;
			}
			set
			{
				if (this.inputConnectedJSON != null)
				{
					this.inputConnectedJSON.val = value;
				}
				else
				{
					this._inputConnected = value;
				}
			}
		}

		// Token: 0x06006F46 RID: 28486 RVA: 0x0029D1F4 File Offset: 0x0029B5F4
		protected void SetOutputConnected(bool b)
		{
			this._outputConnected = b;
			if (this._outputConnected)
			{
				this.SyncAllOutputs();
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x06006F47 RID: 28487 RVA: 0x0029D20E File Offset: 0x0029B60E
		// (set) Token: 0x06006F48 RID: 28488 RVA: 0x0029D22D File Offset: 0x0029B62D
		public bool outputConnected
		{
			get
			{
				if (this.outputConnectedJSON != null)
				{
					return this.outputConnectedJSON.val;
				}
				return this._outputConnected;
			}
			set
			{
				if (this.outputConnectedJSON != null)
				{
					this.outputConnectedJSON.val = value;
				}
				else
				{
					this._outputConnected = value;
				}
			}
		}

		// Token: 0x06006F49 RID: 28489 RVA: 0x0029D252 File Offset: 0x0029B652
		protected void SetThumbProximalBend(float f)
		{
			if (this.thumbProximal != null && this._outputConnected)
			{
				this.thumbProximal.currentBend = f;
				this.thumbProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F4A RID: 28490 RVA: 0x0029D287 File Offset: 0x0029B687
		protected void SetThumbProximalSpread(float f)
		{
			if (this.thumbProximal != null && this._outputConnected)
			{
				this.thumbProximal.currentSpread = f;
				this.thumbProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F4B RID: 28491 RVA: 0x0029D2BC File Offset: 0x0029B6BC
		protected void SetThumbProximalTwist(float f)
		{
			if (this.thumbProximal != null && this._outputConnected)
			{
				this.thumbProximal.currentTwist = f;
				this.thumbProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F4C RID: 28492 RVA: 0x0029D2F1 File Offset: 0x0029B6F1
		protected void SetThumbMiddleBend(float f)
		{
			if (this.thumbMiddle != null && this._outputConnected)
			{
				this.thumbMiddle.currentBend = f;
				this.thumbMiddle.UpdateOutput();
			}
		}

		// Token: 0x06006F4D RID: 28493 RVA: 0x0029D326 File Offset: 0x0029B726
		protected void SetThumbDistalBend(float f)
		{
			if (this.thumbDistal != null && this._outputConnected)
			{
				this.thumbDistal.currentBend = f;
				this.thumbDistal.UpdateOutput();
			}
		}

		// Token: 0x06006F4E RID: 28494 RVA: 0x0029D35B File Offset: 0x0029B75B
		protected void SetIndexProximalBend(float f)
		{
			if (this.indexProximal != null && this._outputConnected)
			{
				this.indexProximal.currentBend = f;
				this.indexProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F4F RID: 28495 RVA: 0x0029D390 File Offset: 0x0029B790
		protected void SetIndexProximalSpread(float f)
		{
			if (this.indexProximal != null && this._outputConnected)
			{
				this.indexProximal.currentSpread = f;
				this.indexProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F50 RID: 28496 RVA: 0x0029D3C5 File Offset: 0x0029B7C5
		protected void SetIndexProximalTwist(float f)
		{
			if (this.indexProximal != null && this._outputConnected)
			{
				this.indexProximal.currentTwist = f;
				this.indexProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F51 RID: 28497 RVA: 0x0029D3FA File Offset: 0x0029B7FA
		protected void SetIndexMiddleBend(float f)
		{
			if (this.indexMiddle != null && this._outputConnected)
			{
				this.indexMiddle.currentBend = f;
				this.indexMiddle.UpdateOutput();
			}
		}

		// Token: 0x06006F52 RID: 28498 RVA: 0x0029D42F File Offset: 0x0029B82F
		protected void SetIndexDistalBend(float f)
		{
			if (this.indexDistal != null && this._outputConnected)
			{
				this.indexDistal.currentBend = f;
				this.indexDistal.UpdateOutput();
			}
		}

		// Token: 0x06006F53 RID: 28499 RVA: 0x0029D464 File Offset: 0x0029B864
		protected void SetMiddleProximalBend(float f)
		{
			if (this.middleProximal != null && this._outputConnected)
			{
				this.middleProximal.currentBend = f;
				this.middleProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F54 RID: 28500 RVA: 0x0029D499 File Offset: 0x0029B899
		protected void SetMiddleProximalSpread(float f)
		{
			if (this.middleProximal != null && this._outputConnected)
			{
				this.middleProximal.currentSpread = f;
				this.middleProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F55 RID: 28501 RVA: 0x0029D4CE File Offset: 0x0029B8CE
		protected void SetMiddleProximalTwist(float f)
		{
			if (this.middleProximal != null && this._outputConnected)
			{
				this.middleProximal.currentTwist = f;
				this.middleProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F56 RID: 28502 RVA: 0x0029D503 File Offset: 0x0029B903
		protected void SetMiddleMiddleBend(float f)
		{
			if (this.middleMiddle != null && this._outputConnected)
			{
				this.middleMiddle.currentBend = f;
				this.middleMiddle.UpdateOutput();
			}
		}

		// Token: 0x06006F57 RID: 28503 RVA: 0x0029D538 File Offset: 0x0029B938
		protected void SetMiddleDistalBend(float f)
		{
			if (this.middleDistal != null && this._outputConnected)
			{
				this.middleDistal.currentBend = f;
				this.middleDistal.UpdateOutput();
			}
		}

		// Token: 0x06006F58 RID: 28504 RVA: 0x0029D56D File Offset: 0x0029B96D
		protected void SetRingProximalBend(float f)
		{
			if (this.ringProximal != null && this._outputConnected)
			{
				this.ringProximal.currentBend = f;
				this.ringProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F59 RID: 28505 RVA: 0x0029D5A2 File Offset: 0x0029B9A2
		protected void SetRingProximalSpread(float f)
		{
			if (this.ringProximal != null && this._outputConnected)
			{
				this.ringProximal.currentSpread = f;
				this.ringProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F5A RID: 28506 RVA: 0x0029D5D7 File Offset: 0x0029B9D7
		protected void SetRingProximalTwist(float f)
		{
			if (this.ringProximal != null && this._outputConnected)
			{
				this.ringProximal.currentTwist = f;
				this.ringProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F5B RID: 28507 RVA: 0x0029D60C File Offset: 0x0029BA0C
		protected void SetRingMiddleBend(float f)
		{
			if (this.ringMiddle != null && this._outputConnected)
			{
				this.ringMiddle.currentBend = f;
				this.ringMiddle.UpdateOutput();
			}
		}

		// Token: 0x06006F5C RID: 28508 RVA: 0x0029D641 File Offset: 0x0029BA41
		protected void SetRingDistalBend(float f)
		{
			if (this.ringDistal != null && this._outputConnected)
			{
				this.ringDistal.currentBend = f;
				this.ringDistal.UpdateOutput();
			}
		}

		// Token: 0x06006F5D RID: 28509 RVA: 0x0029D676 File Offset: 0x0029BA76
		protected void SetPinkyProximalBend(float f)
		{
			if (this.pinkyProximal != null && this._outputConnected)
			{
				this.pinkyProximal.currentBend = f;
				this.pinkyProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F5E RID: 28510 RVA: 0x0029D6AB File Offset: 0x0029BAAB
		protected void SetPinkyProximalSpread(float f)
		{
			if (this.pinkyProximal != null && this._outputConnected)
			{
				this.pinkyProximal.currentSpread = f;
				this.pinkyProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F5F RID: 28511 RVA: 0x0029D6E0 File Offset: 0x0029BAE0
		protected void SetPinkyProximalTwist(float f)
		{
			if (this.pinkyProximal != null && this._outputConnected)
			{
				this.pinkyProximal.currentTwist = f;
				this.pinkyProximal.UpdateOutput();
			}
		}

		// Token: 0x06006F60 RID: 28512 RVA: 0x0029D715 File Offset: 0x0029BB15
		protected void SetPinkyMiddleBend(float f)
		{
			if (this.pinkyMiddle != null && this._outputConnected)
			{
				this.pinkyMiddle.currentBend = f;
				this.pinkyMiddle.UpdateOutput();
			}
		}

		// Token: 0x06006F61 RID: 28513 RVA: 0x0029D74A File Offset: 0x0029BB4A
		protected void SetPinkyDistalBend(float f)
		{
			if (this.pinkyDistal != null && this._outputConnected)
			{
				this.pinkyDistal.currentBend = f;
				this.pinkyDistal.UpdateOutput();
			}
		}

		// Token: 0x06006F62 RID: 28514 RVA: 0x0029D780 File Offset: 0x0029BB80
		protected void SyncAllOutputs()
		{
			this.SetThumbProximalBend(this.thumbProximalBendJSON.val);
			this.SetThumbProximalSpread(this.thumbProximalSpreadJSON.val);
			this.SetThumbProximalTwist(this.thumbProximalTwistJSON.val);
			this.SetThumbMiddleBend(this.thumbMiddleBendJSON.val);
			this.SetThumbDistalBend(this.thumbDistalBendJSON.val);
			this.SetIndexProximalBend(this.indexProximalBendJSON.val);
			this.SetIndexProximalSpread(this.indexProximalSpreadJSON.val);
			this.SetIndexProximalTwist(this.indexProximalTwistJSON.val);
			this.SetIndexMiddleBend(this.indexMiddleBendJSON.val);
			this.SetIndexDistalBend(this.indexDistalBendJSON.val);
			this.SetMiddleProximalBend(this.middleProximalBendJSON.val);
			this.SetMiddleProximalSpread(this.middleProximalSpreadJSON.val);
			this.SetMiddleProximalTwist(this.middleProximalTwistJSON.val);
			this.SetMiddleMiddleBend(this.middleMiddleBendJSON.val);
			this.SetMiddleDistalBend(this.middleDistalBendJSON.val);
			this.SetRingProximalBend(this.ringProximalBendJSON.val);
			this.SetRingProximalSpread(this.ringProximalSpreadJSON.val);
			this.SetRingProximalTwist(this.ringProximalTwistJSON.val);
			this.SetRingMiddleBend(this.ringMiddleBendJSON.val);
			this.SetRingDistalBend(this.ringDistalBendJSON.val);
			this.SetPinkyProximalBend(this.pinkyProximalBendJSON.val);
			this.SetPinkyProximalSpread(this.pinkyProximalSpreadJSON.val);
			this.SetPinkyProximalTwist(this.pinkyProximalTwistJSON.val);
			this.SetPinkyMiddleBend(this.pinkyMiddleBendJSON.val);
			this.SetPinkyDistalBend(this.pinkyDistalBendJSON.val);
		}

		// Token: 0x06006F63 RID: 28515 RVA: 0x0029D938 File Offset: 0x0029BD38
		protected virtual void Init()
		{
			this.inputConnectedJSON = new JSONStorableBool("inputConnected", this._inputConnected, new JSONStorableBool.SetBoolCallback(this.SetInputConnected));
			this.outputConnectedJSON = new JSONStorableBool("outputConnected", this._outputConnected, new JSONStorableBool.SetBoolCallback(this.SetOutputConnected));
			this.thumbProximalBendJSON = new JSONStorableFloat("thumbProximalBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetThumbProximalBend), -100f, 100f, true, true);
			this.thumbProximalBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.thumbProximalBendJSON);
			this.thumbProximalSpreadJSON = new JSONStorableFloat("thumbProximalSpread", 0f, new JSONStorableFloat.SetFloatCallback(this.SetThumbProximalSpread), -100f, 100f, true, true);
			this.thumbProximalSpreadJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.thumbProximalSpreadJSON);
			this.thumbProximalTwistJSON = new JSONStorableFloat("thumbProximalTwist", 0f, new JSONStorableFloat.SetFloatCallback(this.SetThumbProximalTwist), -100f, 100f, true, true);
			this.thumbProximalTwistJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.thumbProximalTwistJSON);
			this.thumbMiddleBendJSON = new JSONStorableFloat("thumbMiddleBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetThumbMiddleBend), -100f, 100f, true, true);
			this.thumbMiddleBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.thumbMiddleBendJSON);
			this.thumbDistalBendJSON = new JSONStorableFloat("thumbDistalBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetThumbDistalBend), -100f, 100f, true, true);
			this.thumbDistalBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.thumbDistalBendJSON);
			this.indexProximalBendJSON = new JSONStorableFloat("indexProximalBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetIndexProximalBend), -100f, 100f, true, true);
			this.indexProximalBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.indexProximalBendJSON);
			this.indexProximalSpreadJSON = new JSONStorableFloat("indexProximalSpread", 0f, new JSONStorableFloat.SetFloatCallback(this.SetIndexProximalSpread), -100f, 100f, true, true);
			this.indexProximalSpreadJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.indexProximalSpreadJSON);
			this.indexProximalTwistJSON = new JSONStorableFloat("indexProximalTwist", 0f, new JSONStorableFloat.SetFloatCallback(this.SetIndexProximalTwist), -100f, 100f, true, true);
			this.indexProximalTwistJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.indexProximalTwistJSON);
			this.indexMiddleBendJSON = new JSONStorableFloat("indexMiddleBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetIndexMiddleBend), -100f, 100f, true, true);
			this.indexMiddleBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.indexMiddleBendJSON);
			this.indexDistalBendJSON = new JSONStorableFloat("indexDistalBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetIndexDistalBend), -100f, 100f, true, true);
			this.indexDistalBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.indexDistalBendJSON);
			this.middleProximalBendJSON = new JSONStorableFloat("middleProximalBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetMiddleProximalBend), -100f, 100f, true, true);
			this.middleProximalBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.middleProximalBendJSON);
			this.middleProximalSpreadJSON = new JSONStorableFloat("middleProximalSpread", 0f, new JSONStorableFloat.SetFloatCallback(this.SetMiddleProximalSpread), -100f, 100f, true, true);
			this.middleProximalSpreadJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.middleProximalSpreadJSON);
			this.middleProximalTwistJSON = new JSONStorableFloat("middleProximalTwist", 0f, new JSONStorableFloat.SetFloatCallback(this.SetMiddleProximalTwist), -100f, 100f, true, true);
			this.middleProximalTwistJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.middleProximalTwistJSON);
			this.middleMiddleBendJSON = new JSONStorableFloat("middleMiddleBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetMiddleMiddleBend), -100f, 100f, true, true);
			this.middleMiddleBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.middleMiddleBendJSON);
			this.middleDistalBendJSON = new JSONStorableFloat("middleDistalBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetMiddleDistalBend), -100f, 100f, true, true);
			this.middleDistalBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.middleDistalBendJSON);
			this.ringProximalBendJSON = new JSONStorableFloat("ringProximalBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetRingProximalBend), -100f, 100f, true, true);
			this.ringProximalBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.ringProximalBendJSON);
			this.ringProximalSpreadJSON = new JSONStorableFloat("ringProximalSpread", 0f, new JSONStorableFloat.SetFloatCallback(this.SetRingProximalSpread), -100f, 100f, true, true);
			this.ringProximalSpreadJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.ringProximalSpreadJSON);
			this.ringProximalTwistJSON = new JSONStorableFloat("ringProximalTwist", 0f, new JSONStorableFloat.SetFloatCallback(this.SetRingProximalTwist), -100f, 100f, true, true);
			this.ringProximalTwistJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.ringProximalTwistJSON);
			this.ringMiddleBendJSON = new JSONStorableFloat("ringMiddleBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetRingMiddleBend), -100f, 100f, true, true);
			this.ringMiddleBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.ringMiddleBendJSON);
			this.ringDistalBendJSON = new JSONStorableFloat("ringDistalBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetRingDistalBend), -100f, 100f, true, true);
			this.ringDistalBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.ringDistalBendJSON);
			this.pinkyProximalBendJSON = new JSONStorableFloat("pinkyProximalBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetPinkyProximalBend), -100f, 100f, true, true);
			this.pinkyProximalBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.pinkyProximalBendJSON);
			this.pinkyProximalSpreadJSON = new JSONStorableFloat("pinkyProximalSpread", 0f, new JSONStorableFloat.SetFloatCallback(this.SetPinkyProximalSpread), -100f, 100f, true, true);
			this.pinkyProximalSpreadJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.pinkyProximalSpreadJSON);
			this.pinkyProximalTwistJSON = new JSONStorableFloat("pinkyProximalTwist", 0f, new JSONStorableFloat.SetFloatCallback(this.SetPinkyProximalTwist), -100f, 100f, true, true);
			this.pinkyProximalTwistJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.pinkyProximalTwistJSON);
			this.pinkyMiddleBendJSON = new JSONStorableFloat("pinkyMiddleBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetPinkyMiddleBend), -100f, 100f, true, true);
			this.pinkyMiddleBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.pinkyMiddleBendJSON);
			this.pinkyDistalBendJSON = new JSONStorableFloat("pinkyDistalBend", 0f, new JSONStorableFloat.SetFloatCallback(this.SetPinkyDistalBend), -100f, 100f, true, true);
			this.pinkyDistalBendJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.pinkyDistalBendJSON);
		}

		// Token: 0x06006F64 RID: 28516 RVA: 0x0029E046 File Offset: 0x0029C446
		protected override void InitUI(Transform t, bool isAlt)
		{
			if (t != null)
			{
			}
		}

		// Token: 0x06006F65 RID: 28517 RVA: 0x0029E054 File Offset: 0x0029C454
		protected override void Awake()
		{
			if (!this.awakecalled)
			{
				base.Awake();
				this.Init();
				this.InitUI();
				this.InitUIAlt();
			}
		}

		// Token: 0x06006F66 RID: 28518 RVA: 0x0029E07C File Offset: 0x0029C47C
		protected void Update()
		{
			if (this._inputConnected)
			{
				if (this.hand == HandOutput.Hand.Left)
				{
					this.thumbProximalBendJSON.val = HandInput.leftThumbProximalBend;
					this.thumbProximalSpreadJSON.val = HandInput.leftThumbProximalSpread;
					this.thumbProximalTwistJSON.val = HandInput.leftThumbProximalTwist;
					this.thumbMiddleBendJSON.val = HandInput.leftThumbMiddleBend;
					this.thumbDistalBendJSON.val = HandInput.leftThumbDistalBend;
					this.indexProximalBendJSON.val = HandInput.leftIndexProximalBend;
					this.indexProximalSpreadJSON.val = HandInput.leftIndexProximalSpread;
					this.indexProximalTwistJSON.val = HandInput.leftIndexProximalTwist;
					this.indexMiddleBendJSON.val = HandInput.leftIndexMiddleBend;
					this.indexDistalBendJSON.val = HandInput.leftIndexDistalBend;
					this.middleProximalBendJSON.val = HandInput.leftMiddleProximalBend;
					this.middleProximalSpreadJSON.val = HandInput.leftMiddleProximalSpread;
					this.middleProximalTwistJSON.val = HandInput.leftMiddleProximalTwist;
					this.middleMiddleBendJSON.val = HandInput.leftMiddleMiddleBend;
					this.middleDistalBendJSON.val = HandInput.leftMiddleDistalBend;
					this.ringProximalBendJSON.val = HandInput.leftRingProximalBend;
					this.ringProximalSpreadJSON.val = HandInput.leftRingProximalSpread;
					this.ringProximalTwistJSON.val = HandInput.leftRingProximalTwist;
					this.ringMiddleBendJSON.val = HandInput.leftRingMiddleBend;
					this.ringDistalBendJSON.val = HandInput.leftRingDistalBend;
					this.pinkyProximalBendJSON.val = HandInput.leftPinkyProximalBend;
					this.pinkyProximalSpreadJSON.val = HandInput.leftPinkyProximalSpread;
					this.pinkyProximalTwistJSON.val = HandInput.leftPinkyProximalTwist;
					this.pinkyMiddleBendJSON.val = HandInput.leftPinkyMiddleBend;
					this.pinkyDistalBendJSON.val = HandInput.leftPinkyDistalBend;
				}
				else
				{
					this.thumbProximalBendJSON.val = HandInput.rightThumbProximalBend;
					this.thumbProximalSpreadJSON.val = HandInput.rightThumbProximalSpread;
					this.thumbProximalTwistJSON.val = HandInput.rightThumbProximalTwist;
					this.thumbMiddleBendJSON.val = HandInput.rightThumbMiddleBend;
					this.thumbDistalBendJSON.val = HandInput.rightThumbDistalBend;
					this.indexProximalBendJSON.val = HandInput.rightIndexProximalBend;
					this.indexProximalSpreadJSON.val = HandInput.rightIndexProximalSpread;
					this.indexProximalTwistJSON.val = HandInput.rightIndexProximalTwist;
					this.indexMiddleBendJSON.val = HandInput.rightIndexMiddleBend;
					this.indexDistalBendJSON.val = HandInput.rightIndexDistalBend;
					this.middleProximalBendJSON.val = HandInput.rightMiddleProximalBend;
					this.middleProximalSpreadJSON.val = HandInput.rightMiddleProximalSpread;
					this.middleProximalTwistJSON.val = HandInput.rightMiddleProximalTwist;
					this.middleMiddleBendJSON.val = HandInput.rightMiddleMiddleBend;
					this.middleDistalBendJSON.val = HandInput.rightMiddleDistalBend;
					this.ringProximalBendJSON.val = HandInput.rightRingProximalBend;
					this.ringProximalSpreadJSON.val = HandInput.rightRingProximalSpread;
					this.ringProximalTwistJSON.val = HandInput.rightRingProximalTwist;
					this.ringMiddleBendJSON.val = HandInput.rightRingMiddleBend;
					this.ringDistalBendJSON.val = HandInput.rightRingDistalBend;
					this.pinkyProximalBendJSON.val = HandInput.rightPinkyProximalBend;
					this.pinkyProximalSpreadJSON.val = HandInput.rightPinkyProximalSpread;
					this.pinkyProximalTwistJSON.val = HandInput.rightPinkyProximalTwist;
					this.pinkyMiddleBendJSON.val = HandInput.rightPinkyMiddleBend;
					this.pinkyDistalBendJSON.val = HandInput.rightPinkyDistalBend;
				}
			}
		}

		// Token: 0x040060C7 RID: 24775
		public JSONStorableBool inputConnectedJSON;

		// Token: 0x040060C8 RID: 24776
		[SerializeField]
		protected bool _inputConnected = true;

		// Token: 0x040060C9 RID: 24777
		public JSONStorableBool outputConnectedJSON;

		// Token: 0x040060CA RID: 24778
		[SerializeField]
		protected bool _outputConnected = true;

		// Token: 0x040060CB RID: 24779
		public HandOutput.Hand hand;

		// Token: 0x040060CC RID: 24780
		public FingerOutput thumbProximal;

		// Token: 0x040060CD RID: 24781
		public JSONStorableFloat thumbProximalBendJSON;

		// Token: 0x040060CE RID: 24782
		public JSONStorableFloat thumbProximalSpreadJSON;

		// Token: 0x040060CF RID: 24783
		public JSONStorableFloat thumbProximalTwistJSON;

		// Token: 0x040060D0 RID: 24784
		public FingerOutput thumbMiddle;

		// Token: 0x040060D1 RID: 24785
		public JSONStorableFloat thumbMiddleBendJSON;

		// Token: 0x040060D2 RID: 24786
		public FingerOutput thumbDistal;

		// Token: 0x040060D3 RID: 24787
		public JSONStorableFloat thumbDistalBendJSON;

		// Token: 0x040060D4 RID: 24788
		public FingerOutput indexProximal;

		// Token: 0x040060D5 RID: 24789
		public JSONStorableFloat indexProximalBendJSON;

		// Token: 0x040060D6 RID: 24790
		public JSONStorableFloat indexProximalSpreadJSON;

		// Token: 0x040060D7 RID: 24791
		public JSONStorableFloat indexProximalTwistJSON;

		// Token: 0x040060D8 RID: 24792
		public FingerOutput indexMiddle;

		// Token: 0x040060D9 RID: 24793
		public JSONStorableFloat indexMiddleBendJSON;

		// Token: 0x040060DA RID: 24794
		public FingerOutput indexDistal;

		// Token: 0x040060DB RID: 24795
		public JSONStorableFloat indexDistalBendJSON;

		// Token: 0x040060DC RID: 24796
		public FingerOutput middleProximal;

		// Token: 0x040060DD RID: 24797
		public JSONStorableFloat middleProximalBendJSON;

		// Token: 0x040060DE RID: 24798
		public JSONStorableFloat middleProximalSpreadJSON;

		// Token: 0x040060DF RID: 24799
		public JSONStorableFloat middleProximalTwistJSON;

		// Token: 0x040060E0 RID: 24800
		public FingerOutput middleMiddle;

		// Token: 0x040060E1 RID: 24801
		public JSONStorableFloat middleMiddleBendJSON;

		// Token: 0x040060E2 RID: 24802
		public FingerOutput middleDistal;

		// Token: 0x040060E3 RID: 24803
		public JSONStorableFloat middleDistalBendJSON;

		// Token: 0x040060E4 RID: 24804
		public FingerOutput ringProximal;

		// Token: 0x040060E5 RID: 24805
		public JSONStorableFloat ringProximalBendJSON;

		// Token: 0x040060E6 RID: 24806
		public JSONStorableFloat ringProximalSpreadJSON;

		// Token: 0x040060E7 RID: 24807
		public JSONStorableFloat ringProximalTwistJSON;

		// Token: 0x040060E8 RID: 24808
		public FingerOutput ringMiddle;

		// Token: 0x040060E9 RID: 24809
		public JSONStorableFloat ringMiddleBendJSON;

		// Token: 0x040060EA RID: 24810
		public FingerOutput ringDistal;

		// Token: 0x040060EB RID: 24811
		public JSONStorableFloat ringDistalBendJSON;

		// Token: 0x040060EC RID: 24812
		public FingerOutput pinkyProximal;

		// Token: 0x040060ED RID: 24813
		public JSONStorableFloat pinkyProximalBendJSON;

		// Token: 0x040060EE RID: 24814
		public JSONStorableFloat pinkyProximalSpreadJSON;

		// Token: 0x040060EF RID: 24815
		public JSONStorableFloat pinkyProximalTwistJSON;

		// Token: 0x040060F0 RID: 24816
		public FingerOutput pinkyMiddle;

		// Token: 0x040060F1 RID: 24817
		public JSONStorableFloat pinkyMiddleBendJSON;

		// Token: 0x040060F2 RID: 24818
		public FingerOutput pinkyDistal;

		// Token: 0x040060F3 RID: 24819
		public JSONStorableFloat pinkyDistalBendJSON;

		// Token: 0x02000E23 RID: 3619
		public enum Hand
		{
			// Token: 0x040060F5 RID: 24821
			Left,
			// Token: 0x040060F6 RID: 24822
			Right
		}
	}
}
