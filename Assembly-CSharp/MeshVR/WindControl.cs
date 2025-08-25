using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000D93 RID: 3475
	public class WindControl : JSONStorable
	{
		// Token: 0x06006B1F RID: 27423 RVA: 0x00284EBC File Offset: 0x002832BC
		public WindControl()
		{
		}

		// Token: 0x06006B20 RID: 27424 RVA: 0x00284ECF File Offset: 0x002832CF
		protected void SyncIsGlobal(bool b)
		{
			this.isGlobal = b;
		}

		// Token: 0x06006B21 RID: 27425 RVA: 0x00284ED8 File Offset: 0x002832D8
		protected void SyncAtomChocies()
		{
			List<string> list = new List<string>();
			list.Add("None");
			foreach (string item in SuperController.singleton.GetVisibleAtomUIDs())
			{
				list.Add(item);
			}
			this.atomJSON.choices = list;
		}

		// Token: 0x06006B22 RID: 27426 RVA: 0x00284F58 File Offset: 0x00283358
		protected void OnAtomRename(string oldid, string newid)
		{
			this.SyncAtomChocies();
			if (this.atomJSON != null && this.receivingAtom != null)
			{
				this.atomJSON.valNoCallback = this.receivingAtom.uid;
			}
		}

		// Token: 0x06006B23 RID: 27427 RVA: 0x00284F94 File Offset: 0x00283394
		protected void SyncAtom(string atomUID)
		{
			List<string> list = new List<string>();
			list.Add("None");
			if (atomUID != null)
			{
				this.receivingAtom = SuperController.singleton.GetAtomByUid(atomUID);
				if (this.receivingAtom != null)
				{
					foreach (string item in this.receivingAtom.GetStorableIDs())
					{
						list.Add(item);
					}
				}
			}
			else
			{
				this.receivingAtom = null;
			}
			this.receiverJSON.choices = list;
			this.receiverJSON.val = "None";
		}

		// Token: 0x06006B24 RID: 27428 RVA: 0x00285058 File Offset: 0x00283458
		protected void CheckMissingReceiver()
		{
			if (this._missingReceiverStoreId != string.Empty && this.receivingAtom != null)
			{
				JSONStorable storableByID = this.receivingAtom.GetStorableByID(this._missingReceiverStoreId);
				if (storableByID != null)
				{
					string receiverTargetName = this._receiverTargetName;
					this.SyncReceiver(this._missingReceiverStoreId);
					this._missingReceiverStoreId = string.Empty;
					this.insideRestore = true;
					this.receiverTargetJSON.val = receiverTargetName;
					this.insideRestore = false;
				}
			}
		}

		// Token: 0x06006B25 RID: 27429 RVA: 0x002850E4 File Offset: 0x002834E4
		protected void SyncReceiver(string receiverID)
		{
			List<string> list = new List<string>();
			list.Add("None");
			if (this.receivingAtom != null && receiverID != null)
			{
				this.receiver = this.receivingAtom.GetStorableByID(receiverID);
				if (this.receiver != null)
				{
					foreach (string item in this.receiver.GetVector3ParamNames())
					{
						list.Add(item);
					}
				}
				else if (receiverID != "None")
				{
					this._missingReceiverStoreId = receiverID;
				}
			}
			else
			{
				this.receiver = null;
			}
			this.receiverTargetJSON.choices = list;
			this.receiverTargetJSON.val = "None";
		}

		// Token: 0x06006B26 RID: 27430 RVA: 0x002851D4 File Offset: 0x002835D4
		protected void SyncReceiverTarget(string receiverTargetName)
		{
			this._receiverTargetName = receiverTargetName;
			this.receiverTarget = null;
			if (this.receiver != null && receiverTargetName != null)
			{
				this.receiverTarget = this.receiver.GetVector3JSONParam(receiverTargetName);
			}
		}

		// Token: 0x06006B27 RID: 27431 RVA: 0x0028520D File Offset: 0x0028360D
		protected void SyncMagnitude(Vector3 v)
		{
		}

		// Token: 0x06006B28 RID: 27432 RVA: 0x00285210 File Offset: 0x00283610
		protected override void InitUI(Transform t, bool isAlt)
		{
			base.InitUI(t, isAlt);
			if (t != null)
			{
				WindControlUI componentInChildren = t.GetComponentInChildren<WindControlUI>(true);
				if (componentInChildren != null)
				{
					this.isGlobalJSON.RegisterToggle(componentInChildren.isGlobalToggle, isAlt);
					this.atomJSON.RegisterPopup(componentInChildren.atomPopup, isAlt);
					this.receiverJSON.RegisterPopup(componentInChildren.receiverPopup, isAlt);
					this.receiverTargetJSON.RegisterPopup(componentInChildren.receiverTargetPopup, isAlt);
					this.currentMagnitudeJSON.RegisterSlider(componentInChildren.currentMagnitudeSlider, false);
					this.autoJSON.RegisterToggle(componentInChildren.autoToggle, isAlt);
					this.periodJSON.RegisterSlider(componentInChildren.periodSlider, isAlt);
					this.quicknessJSON.RegisterSlider(componentInChildren.quicknessSlider, isAlt);
					this.lowerMagnitudeJSON.RegisterSlider(componentInChildren.lowerMagnitudeSlider, isAlt);
					this.upperMagnitudeJSON.RegisterSlider(componentInChildren.upperMagnitudeSlider, isAlt);
					this.targetMagnitudeJSON.RegisterSlider(componentInChildren.targetMagnitudeSlider, isAlt);
				}
			}
		}

		// Token: 0x06006B29 RID: 27433 RVA: 0x0028530C File Offset: 0x0028370C
		protected void Init()
		{
			if (SuperController.singleton)
			{
				SuperController singleton = SuperController.singleton;
				singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
			}
			this.isGlobalJSON = new JSONStorableBool("global", false, new JSONStorableBool.SetBoolCallback(this.SyncIsGlobal));
			base.RegisterBool(this.isGlobalJSON);
			this.atomJSON = new JSONStorableStringChooser("atom", SuperController.singleton.GetAtomUIDs(), null, "Atom", new JSONStorableStringChooser.SetStringCallback(this.SyncAtom));
			this.atomJSON.representsAtomUid = true;
			base.RegisterStringChooser(this.atomJSON);
			this.atomJSON.popupOpenCallback = new JSONStorableStringChooser.PopupOpenCallback(this.SyncAtomChocies);
			this.receiverJSON = new JSONStorableStringChooser("receiver", null, null, "Receiver", new JSONStorableStringChooser.SetStringCallback(this.SyncReceiver));
			base.RegisterStringChooser(this.receiverJSON);
			this.receiverTargetJSON = new JSONStorableStringChooser("receiverTarget", null, null, "Target", new JSONStorableStringChooser.SetStringCallback(this.SyncReceiverTarget));
			base.RegisterStringChooser(this.receiverTargetJSON);
			this.currentMagnitudeJSON = new JSONStorableFloat("currentMagnitude", 0f, -50f, 50f, false, true);
			base.RegisterFloat(this.currentMagnitudeJSON);
			this.autoJSON = new JSONStorableBool("auto", false);
			base.RegisterBool(this.autoJSON);
			this.periodJSON = new JSONStorableFloat("period", 0.5f, 0f, 10f, false, true);
			base.RegisterFloat(this.periodJSON);
			this.quicknessJSON = new JSONStorableFloat("quickness", 10f, 0f, 100f, true, true);
			base.RegisterFloat(this.quicknessJSON);
			this.lowerMagnitudeJSON = new JSONStorableFloat("lowerMagnitude", 0f, -50f, 50f, false, true);
			base.RegisterFloat(this.lowerMagnitudeJSON);
			this.upperMagnitudeJSON = new JSONStorableFloat("upperMagnitude", 0f, -50f, 50f, false, true);
			base.RegisterFloat(this.upperMagnitudeJSON);
			this.targetMagnitudeJSON = new JSONStorableFloat("targetMagnitude", 0f, -50f, 50f, false, false);
		}

		// Token: 0x06006B2A RID: 27434 RVA: 0x0028554F File Offset: 0x0028394F
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

		// Token: 0x06006B2B RID: 27435 RVA: 0x00285574 File Offset: 0x00283974
		private void Update()
		{
			if (this.currentMagnitudeJSON != null)
			{
				if (this.autoJSON != null && this.autoJSON.val)
				{
					this.timer -= Time.deltaTime;
					if (this.timer < 0f)
					{
						this.timer = this.periodJSON.val;
						this.targetMagnitudeJSON.val = UnityEngine.Random.Range(this.lowerMagnitudeJSON.val, this.upperMagnitudeJSON.val);
					}
					this.currentMagnitudeJSON.val = Mathf.Lerp(this.currentMagnitudeJSON.val, this.targetMagnitudeJSON.val, Time.deltaTime * this.quicknessJSON.val);
				}
				Vector3 vector = this.currentMagnitudeJSON.val * base.transform.forward;
				if (this.isGlobal)
				{
					WindControl.globalWind = vector;
				}
				else
				{
					this.CheckMissingReceiver();
					if (this.receiverTarget != null)
					{
						this.receiverTarget.val = this.currentMagnitudeJSON.val * base.transform.forward;
					}
				}
			}
		}

		// Token: 0x06006B2C RID: 27436 RVA: 0x002856A0 File Offset: 0x00283AA0
		protected void OnDestroy()
		{
			if (SuperController.singleton)
			{
				SuperController singleton = SuperController.singleton;
				singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
			}
		}

		// Token: 0x06006B2D RID: 27437 RVA: 0x002856D7 File Offset: 0x00283AD7
		// Note: this type is marked as 'beforefieldinit'.
		static WindControl()
		{
		}

		// Token: 0x04005CF0 RID: 23792
		public static Vector3 globalWind = Vector3.zero;

		// Token: 0x04005CF1 RID: 23793
		public bool isGlobal;

		// Token: 0x04005CF2 RID: 23794
		protected JSONStorableBool isGlobalJSON;

		// Token: 0x04005CF3 RID: 23795
		protected Atom receivingAtom;

		// Token: 0x04005CF4 RID: 23796
		protected JSONStorableStringChooser atomJSON;

		// Token: 0x04005CF5 RID: 23797
		protected string _missingReceiverStoreId = string.Empty;

		// Token: 0x04005CF6 RID: 23798
		protected JSONStorable receiver;

		// Token: 0x04005CF7 RID: 23799
		protected JSONStorableStringChooser receiverJSON;

		// Token: 0x04005CF8 RID: 23800
		protected string _receiverTargetName;

		// Token: 0x04005CF9 RID: 23801
		protected JSONStorableVector3 receiverTarget;

		// Token: 0x04005CFA RID: 23802
		protected JSONStorableStringChooser receiverTargetJSON;

		// Token: 0x04005CFB RID: 23803
		protected JSONStorableFloat currentMagnitudeJSON;

		// Token: 0x04005CFC RID: 23804
		protected JSONStorableBool autoJSON;

		// Token: 0x04005CFD RID: 23805
		protected JSONStorableFloat periodJSON;

		// Token: 0x04005CFE RID: 23806
		protected JSONStorableFloat quicknessJSON;

		// Token: 0x04005CFF RID: 23807
		protected JSONStorableFloat targetMagnitudeJSON;

		// Token: 0x04005D00 RID: 23808
		protected JSONStorableFloat lowerMagnitudeJSON;

		// Token: 0x04005D01 RID: 23809
		protected JSONStorableFloat upperMagnitudeJSON;

		// Token: 0x04005D02 RID: 23810
		protected float timer;
	}
}
