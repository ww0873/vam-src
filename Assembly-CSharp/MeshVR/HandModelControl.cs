using System;
using System.Collections.Generic;
using GPUTools.Physics.Scripts.Behaviours;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000C1A RID: 3098
	public class HandModelControl : JSONStorable
	{
		// Token: 0x060059FF RID: 23039 RVA: 0x0021180C File Offset: 0x0020FC0C
		public HandModelControl()
		{
		}

		// Token: 0x06005A00 RID: 23040 RVA: 0x00211834 File Offset: 0x0020FC34
		protected virtual void SetLeftHand(string choice)
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
							this.leftHands[i].transform.gameObject.SetActive(true);
						}
						else
						{
							this.leftHands[i].transform.gameObject.SetActive(false);
						}
					}
					flag = true;
				}
				else if (this.leftHands[i].transform != null)
				{
					this.leftHands[i].transform.gameObject.SetActive(false);
				}
			}
			if (this.disabledLeftHand != null && this.disabledLeftHand.transform != null)
			{
				if (!this._leftHandEnabled || !flag)
				{
					this.disabledLeftHand.transform.gameObject.SetActive(true);
				}
				else
				{
					this.disabledLeftHand.transform.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06005A01 RID: 23041 RVA: 0x0021196D File Offset: 0x0020FD6D
		protected void SyncLeftHand(string choice)
		{
			this.SetLeftHand(choice);
			if (this._linkHands || this.alwaysLinkHands)
			{
				this.rightHandChooser.val = this._leftHandChoice;
			}
			UserPreferences.singleton.SavePreferences();
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06005A02 RID: 23042 RVA: 0x002119A7 File Offset: 0x0020FDA7
		// (set) Token: 0x06005A03 RID: 23043 RVA: 0x002119C6 File Offset: 0x0020FDC6
		public string leftHandChoice
		{
			get
			{
				if (this.leftHandChooser != null)
				{
					return this.leftHandChooser.val;
				}
				return this._leftHandChoice;
			}
			set
			{
				if (this.leftHandChooser != null)
				{
					this.leftHandChooser.valNoCallback = value;
				}
				this.SetLeftHand(value);
			}
		}

		// Token: 0x06005A04 RID: 23044 RVA: 0x002119E6 File Offset: 0x0020FDE6
		protected void SyncLeftHandEnabled(bool b)
		{
			this._leftHandEnabled = b;
			this.SetLeftHand(this._leftHandChoice);
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06005A05 RID: 23045 RVA: 0x002119FB File Offset: 0x0020FDFB
		// (set) Token: 0x06005A06 RID: 23046 RVA: 0x00211A1A File Offset: 0x0020FE1A
		public bool leftHandEnabled
		{
			get
			{
				if (this.leftHandEnabledJSON != null)
				{
					return this.leftHandEnabledJSON.val;
				}
				return this._leftHandEnabled;
			}
			set
			{
				if (this.leftHandEnabledJSON != null)
				{
					this.leftHandEnabledJSON.val = value;
				}
				else
				{
					this.SyncLeftHandEnabled(value);
				}
			}
		}

		// Token: 0x06005A07 RID: 23047 RVA: 0x00211A3F File Offset: 0x0020FE3F
		public void ToggleLeftHandEnabled()
		{
			this.leftHandEnabled = !this._leftHandEnabled;
		}

		// Token: 0x06005A08 RID: 23048 RVA: 0x00211A50 File Offset: 0x0020FE50
		protected virtual void SetRightHand(string choice)
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
							this.rightHands[i].transform.gameObject.SetActive(true);
						}
						else
						{
							this.rightHands[i].transform.gameObject.SetActive(false);
						}
					}
					flag = true;
				}
				else if (this.rightHands[i].transform != null)
				{
					this.rightHands[i].transform.gameObject.SetActive(false);
				}
			}
			if (this.disabledRightHand != null && this.disabledRightHand.transform != null)
			{
				if (!this._rightHandEnabled || !flag)
				{
					this.disabledRightHand.transform.gameObject.SetActive(true);
				}
				else
				{
					this.disabledRightHand.transform.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06005A09 RID: 23049 RVA: 0x00211B89 File Offset: 0x0020FF89
		protected void SyncRightHand(string choice)
		{
			this.SetRightHand(choice);
			if (this._linkHands || this.alwaysLinkHands)
			{
				this.leftHandChooser.val = this._rightHandChoice;
			}
			UserPreferences.singleton.SavePreferences();
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06005A0A RID: 23050 RVA: 0x00211BC3 File Offset: 0x0020FFC3
		// (set) Token: 0x06005A0B RID: 23051 RVA: 0x00211BE2 File Offset: 0x0020FFE2
		public string rightHandChoice
		{
			get
			{
				if (this.rightHandChooser != null)
				{
					return this.rightHandChooser.val;
				}
				return this._rightHandChoice;
			}
			set
			{
				if (this.rightHandChooser != null)
				{
					this.rightHandChooser.valNoCallback = value;
				}
				this.SetRightHand(value);
			}
		}

		// Token: 0x06005A0C RID: 23052 RVA: 0x00211C02 File Offset: 0x00210002
		protected void SyncRightHandEnabled(bool b)
		{
			this._rightHandEnabled = b;
			this.SetRightHand(this._rightHandChoice);
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06005A0D RID: 23053 RVA: 0x00211C17 File Offset: 0x00210017
		// (set) Token: 0x06005A0E RID: 23054 RVA: 0x00211C36 File Offset: 0x00210036
		public bool rightHandEnabled
		{
			get
			{
				if (this.rightHandEnabledJSON != null)
				{
					return this.rightHandEnabledJSON.val;
				}
				return this._rightHandEnabled;
			}
			set
			{
				if (this.rightHandEnabledJSON != null)
				{
					this.rightHandEnabledJSON.val = value;
				}
				else
				{
					this.SyncRightHandEnabled(value);
				}
			}
		}

		// Token: 0x06005A0F RID: 23055 RVA: 0x00211C5B File Offset: 0x0021005B
		public void ToggleRightHandEnabled()
		{
			this.rightHandEnabled = !this._rightHandEnabled;
		}

		// Token: 0x06005A10 RID: 23056 RVA: 0x00211C6C File Offset: 0x0021006C
		public void ToggleBothHandsEnabled()
		{
			if (this._rightHandEnabled || this._leftHandEnabled)
			{
				this.rightHandEnabled = false;
				this.leftHandEnabled = false;
			}
			else
			{
				this.rightHandEnabled = true;
				this.leftHandEnabled = true;
			}
		}

		// Token: 0x06005A11 RID: 23057 RVA: 0x00211CA5 File Offset: 0x002100A5
		protected void SyncLinkHands(bool b)
		{
			this._linkHands = b;
			if (this._linkHands || this.alwaysLinkHands)
			{
				this.rightHandChoice = this.leftHandChoice;
			}
			UserPreferences.singleton.SavePreferences();
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06005A12 RID: 23058 RVA: 0x00211CDA File Offset: 0x002100DA
		// (set) Token: 0x06005A13 RID: 23059 RVA: 0x00211CF9 File Offset: 0x002100F9
		public bool linkHands
		{
			get
			{
				if (this.linkHandsJSON != null)
				{
					return this.linkHandsJSON.val;
				}
				return this._linkHands;
			}
			set
			{
				if (this.linkHandsJSON != null)
				{
					this.linkHandsJSON.valNoCallback = value;
				}
				else
				{
					this._linkHands = value;
				}
			}
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x00211D20 File Offset: 0x00210120
		protected void FindJointReconnectors()
		{
			this.leftJointReconnectors = new List<ConfigurableJointReconnector>();
			foreach (HandModelControl.Hand hand in this.leftHands)
			{
				if (hand.transform != null)
				{
					ConfigurableJointReconnector[] componentsInChildren = hand.transform.GetComponentsInChildren<ConfigurableJointReconnector>();
					foreach (ConfigurableJointReconnector configurableJointReconnector in componentsInChildren)
					{
						if (configurableJointReconnector.controlRelativePositionAndRotation)
						{
							this.leftJointReconnectors.Add(configurableJointReconnector);
						}
					}
				}
			}
			this.rightJointReconnectors = new List<ConfigurableJointReconnector>();
			foreach (HandModelControl.Hand hand2 in this.rightHands)
			{
				if (hand2.transform != null)
				{
					ConfigurableJointReconnector[] componentsInChildren2 = hand2.transform.GetComponentsInChildren<ConfigurableJointReconnector>();
					foreach (ConfigurableJointReconnector configurableJointReconnector2 in componentsInChildren2)
					{
						if (configurableJointReconnector2.controlRelativePositionAndRotation)
						{
							this.rightJointReconnectors.Add(configurableJointReconnector2);
						}
					}
				}
			}
		}

		// Token: 0x06005A15 RID: 23061 RVA: 0x00211E40 File Offset: 0x00210240
		protected void SyncPosition()
		{
			this._rightPosition.x = this._xPosition;
			this._rightPosition.y = this._yPosition;
			this._rightPosition.z = this._zPosition;
			this._leftPosition = this._rightPosition;
			HandModelControl.Axis axis = this.rightLeftAxis;
			if (axis != HandModelControl.Axis.X)
			{
				if (axis != HandModelControl.Axis.Y)
				{
					if (axis == HandModelControl.Axis.Z)
					{
						this._leftPosition.z = -this._zPosition;
					}
				}
				else
				{
					this._leftPosition.y = -this._yPosition;
				}
			}
			else
			{
				this._leftPosition.x = -this._xPosition;
			}
			foreach (ConfigurableJointReconnector configurableJointReconnector in this.leftJointReconnectors)
			{
				if (this._ignorePositionRotationLeft)
				{
					configurableJointReconnector.controlledRelativePosition = Vector3.zero;
				}
				else
				{
					configurableJointReconnector.controlledRelativePosition = this._leftPosition;
				}
			}
			foreach (ConfigurableJointReconnector configurableJointReconnector2 in this.rightJointReconnectors)
			{
				if (this._ignorePositionRotationRight)
				{
					configurableJointReconnector2.controlledRelativePosition = Vector3.zero;
				}
				else
				{
					configurableJointReconnector2.controlledRelativePosition = this._rightPosition;
				}
			}
		}

		// Token: 0x06005A16 RID: 23062 RVA: 0x00211FCC File Offset: 0x002103CC
		protected void SetXPosition(float f)
		{
			this._xPosition = f;
			this.SyncPosition();
		}

		// Token: 0x06005A17 RID: 23063 RVA: 0x00211FDB File Offset: 0x002103DB
		protected void SyncXPosition(float f)
		{
			this.SetXPosition(f);
			UserPreferences.singleton.SavePreferences();
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06005A18 RID: 23064 RVA: 0x00211FEE File Offset: 0x002103EE
		// (set) Token: 0x06005A19 RID: 23065 RVA: 0x0021200D File Offset: 0x0021040D
		public float xPosition
		{
			get
			{
				if (this.xPositionJSON != null)
				{
					return this.xPositionJSON.val;
				}
				return this._xPosition;
			}
			set
			{
				if (this.xPositionJSON != null)
				{
					this.xPositionJSON.valNoCallback = value;
				}
				this.SetXPosition(value);
			}
		}

		// Token: 0x06005A1A RID: 23066 RVA: 0x0021202D File Offset: 0x0021042D
		protected void SetYPosition(float f)
		{
			this._yPosition = f;
			this.SyncPosition();
		}

		// Token: 0x06005A1B RID: 23067 RVA: 0x0021203C File Offset: 0x0021043C
		protected void SyncYPosition(float f)
		{
			this.SetYPosition(f);
			UserPreferences.singleton.SavePreferences();
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06005A1C RID: 23068 RVA: 0x0021204F File Offset: 0x0021044F
		// (set) Token: 0x06005A1D RID: 23069 RVA: 0x0021206E File Offset: 0x0021046E
		public float yPosition
		{
			get
			{
				if (this.yPositionJSON != null)
				{
					return this.yPositionJSON.val;
				}
				return this._yPosition;
			}
			set
			{
				if (this.yPositionJSON != null)
				{
					this.yPositionJSON.valNoCallback = value;
				}
				this.SetYPosition(value);
			}
		}

		// Token: 0x06005A1E RID: 23070 RVA: 0x0021208E File Offset: 0x0021048E
		protected void SetZPosition(float f)
		{
			this._zPosition = f;
			this.SyncPosition();
		}

		// Token: 0x06005A1F RID: 23071 RVA: 0x0021209D File Offset: 0x0021049D
		protected void SyncZPosition(float f)
		{
			this.SetZPosition(f);
			UserPreferences.singleton.SavePreferences();
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06005A20 RID: 23072 RVA: 0x002120B0 File Offset: 0x002104B0
		// (set) Token: 0x06005A21 RID: 23073 RVA: 0x002120CF File Offset: 0x002104CF
		public float zPosition
		{
			get
			{
				if (this.zPositionJSON != null)
				{
					return this.zPositionJSON.val;
				}
				return this._zPosition;
			}
			set
			{
				if (this.zPositionJSON != null)
				{
					this.zPositionJSON.valNoCallback = value;
				}
				this.SetZPosition(value);
			}
		}

		// Token: 0x06005A22 RID: 23074 RVA: 0x002120F0 File Offset: 0x002104F0
		protected void SyncRotation()
		{
			this._rightRotation.x = this._xRotation;
			this._rightRotation.y = this._yRotation;
			this._rightRotation.z = this._zRotation;
			this._leftRotation = this._rightRotation;
			HandModelControl.Axis axis = this.upDownAxis;
			if (axis != HandModelControl.Axis.X)
			{
				if (axis != HandModelControl.Axis.Y)
				{
					if (axis == HandModelControl.Axis.Z)
					{
						this._leftRotation.z = -this._zRotation;
					}
				}
				else
				{
					this._leftRotation.y = -this._yRotation;
				}
			}
			else
			{
				this._leftRotation.x = -this._xRotation;
			}
			HandModelControl.Axis axis2 = this.forwardBackAxis;
			if (axis2 != HandModelControl.Axis.X)
			{
				if (axis2 != HandModelControl.Axis.Y)
				{
					if (axis2 == HandModelControl.Axis.Z)
					{
						this._leftRotation.z = -this._zRotation;
					}
				}
				else
				{
					this._leftRotation.y = -this._yRotation;
				}
			}
			else
			{
				this._leftRotation.x = -this._xRotation;
			}
			this._qRightRotation = Quaternion.Euler(this._rightRotation);
			this._qLeftRotation = Quaternion.Euler(this._leftRotation);
			foreach (ConfigurableJointReconnector configurableJointReconnector in this.leftJointReconnectors)
			{
				if (this._ignorePositionRotationLeft)
				{
					configurableJointReconnector.controlledRelativeRotation = Quaternion.identity;
				}
				else
				{
					configurableJointReconnector.controlledRelativeRotation = this._qLeftRotation;
				}
			}
			foreach (ConfigurableJointReconnector configurableJointReconnector2 in this.rightJointReconnectors)
			{
				if (this._ignorePositionRotationRight)
				{
					configurableJointReconnector2.controlledRelativeRotation = Quaternion.identity;
				}
				else
				{
					configurableJointReconnector2.controlledRelativeRotation = this._qRightRotation;
				}
			}
		}

		// Token: 0x06005A23 RID: 23075 RVA: 0x00212308 File Offset: 0x00210708
		protected void SetXRotation(float f)
		{
			this._xRotation = f;
			this.SyncRotation();
		}

		// Token: 0x06005A24 RID: 23076 RVA: 0x00212317 File Offset: 0x00210717
		protected void SyncXRotation(float f)
		{
			this.SetXRotation(f);
			UserPreferences.singleton.SavePreferences();
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06005A25 RID: 23077 RVA: 0x0021232A File Offset: 0x0021072A
		// (set) Token: 0x06005A26 RID: 23078 RVA: 0x00212349 File Offset: 0x00210749
		public float xRotation
		{
			get
			{
				if (this.xRotationJSON != null)
				{
					return this.xRotationJSON.val;
				}
				return this._xRotation;
			}
			set
			{
				if (this.xRotationJSON != null)
				{
					this.xRotationJSON.valNoCallback = value;
				}
				this.SetXRotation(value);
			}
		}

		// Token: 0x06005A27 RID: 23079 RVA: 0x00212369 File Offset: 0x00210769
		protected void SetYRotation(float f)
		{
			this._yRotation = f;
			this.SyncRotation();
		}

		// Token: 0x06005A28 RID: 23080 RVA: 0x00212378 File Offset: 0x00210778
		protected void SyncYRotation(float f)
		{
			this.SetYRotation(f);
			UserPreferences.singleton.SavePreferences();
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06005A29 RID: 23081 RVA: 0x0021238B File Offset: 0x0021078B
		// (set) Token: 0x06005A2A RID: 23082 RVA: 0x002123AA File Offset: 0x002107AA
		public float yRotation
		{
			get
			{
				if (this.yRotationJSON != null)
				{
					return this.yRotationJSON.val;
				}
				return this._yRotation;
			}
			set
			{
				if (this.yRotationJSON != null)
				{
					this.yRotationJSON.valNoCallback = value;
				}
				this.SetYRotation(value);
			}
		}

		// Token: 0x06005A2B RID: 23083 RVA: 0x002123CA File Offset: 0x002107CA
		protected void SetZRotation(float f)
		{
			this._zRotation = f;
			this.SyncRotation();
		}

		// Token: 0x06005A2C RID: 23084 RVA: 0x002123D9 File Offset: 0x002107D9
		protected void SyncZRotation(float f)
		{
			this.SetZRotation(f);
			UserPreferences.singleton.SavePreferences();
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06005A2D RID: 23085 RVA: 0x002123EC File Offset: 0x002107EC
		// (set) Token: 0x06005A2E RID: 23086 RVA: 0x0021240B File Offset: 0x0021080B
		public float zRotation
		{
			get
			{
				if (this.zRotationJSON != null)
				{
					return this.zRotationJSON.val;
				}
				return this._zRotation;
			}
			set
			{
				if (this.zRotationJSON != null)
				{
					this.zRotationJSON.valNoCallback = value;
				}
				this.SetZRotation(value);
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06005A2F RID: 23087 RVA: 0x0021242B File Offset: 0x0021082B
		// (set) Token: 0x06005A30 RID: 23088 RVA: 0x00212433 File Offset: 0x00210833
		public bool ignorePositionRotationLeft
		{
			get
			{
				return this._ignorePositionRotationLeft;
			}
			set
			{
				if (this._ignorePositionRotationLeft != value)
				{
					this._ignorePositionRotationLeft = value;
					this.SyncPosition();
					this.SyncRotation();
				}
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06005A31 RID: 23089 RVA: 0x00212454 File Offset: 0x00210854
		// (set) Token: 0x06005A32 RID: 23090 RVA: 0x0021245C File Offset: 0x0021085C
		public bool ignorePositionRotationRight
		{
			get
			{
				return this._ignorePositionRotationRight;
			}
			set
			{
				if (this._ignorePositionRotationRight != value)
				{
					this._ignorePositionRotationRight = value;
					this.SyncPosition();
					this.SyncRotation();
				}
			}
		}

		// Token: 0x06005A33 RID: 23091 RVA: 0x0021247D File Offset: 0x0021087D
		public void ToggleCollision()
		{
			this.useCollision = !this._useCollision;
		}

		// Token: 0x06005A34 RID: 23092 RVA: 0x00212490 File Offset: 0x00210890
		protected void SetUseCollision(bool b)
		{
			this._useCollision = b;
			if (this.colliders != null)
			{
				foreach (Collider collider in this.colliders)
				{
					if (collider != null)
					{
						if (b)
						{
							collider.gameObject.layer = this.collisionLayer;
						}
						else
						{
							collider.gameObject.layer = this.noCollisionLayer;
						}
					}
				}
			}
			if (this.gpuLineSphereColliders != null)
			{
				foreach (CapsuleLineSphereCollider capsuleLineSphereCollider in this.gpuLineSphereColliders)
				{
					if (capsuleLineSphereCollider != null)
					{
						capsuleLineSphereCollider.enabled = b;
					}
				}
			}
			if (this.gpuSphereColliders != null)
			{
				foreach (GpuSphereCollider gpuSphereCollider in this.gpuSphereColliders)
				{
					if (gpuSphereCollider != null)
					{
						gpuSphereCollider.enabled = b;
					}
				}
			}
		}

		// Token: 0x06005A35 RID: 23093 RVA: 0x002125F8 File Offset: 0x002109F8
		protected void SyncUseCollision(bool b)
		{
			this._useCollision = b;
			this.SetUseCollision(b);
			UserPreferences.singleton.SavePreferences();
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06005A36 RID: 23094 RVA: 0x00212612 File Offset: 0x00210A12
		// (set) Token: 0x06005A37 RID: 23095 RVA: 0x00212631 File Offset: 0x00210A31
		public bool useCollision
		{
			get
			{
				if (this.useCollisionJSON != null)
				{
					return this.useCollisionJSON.val;
				}
				return this._useCollision;
			}
			set
			{
				if (this.useCollisionJSON != null)
				{
					this.useCollisionJSON.valNoCallback = value;
				}
				this.SetUseCollision(value);
			}
		}

		// Token: 0x06005A38 RID: 23096 RVA: 0x00212654 File Offset: 0x00210A54
		protected void FindCollidersInHandArray(HandModelControl.Hand[] hands)
		{
			foreach (HandModelControl.Hand hand in hands)
			{
				if (hand.transform != null)
				{
					Collider[] componentsInChildren = hand.transform.GetComponentsInChildren<Collider>(true);
					foreach (Collider item in componentsInChildren)
					{
						this.colliders.Add(item);
					}
					CapsuleLineSphereCollider[] componentsInChildren2 = hand.transform.GetComponentsInChildren<CapsuleLineSphereCollider>(true);
					foreach (CapsuleLineSphereCollider item2 in componentsInChildren2)
					{
						this.gpuLineSphereColliders.Add(item2);
					}
					GpuSphereCollider[] componentsInChildren3 = hand.transform.GetComponentsInChildren<GpuSphereCollider>(true);
					foreach (GpuSphereCollider item3 in componentsInChildren3)
					{
						this.gpuSphereColliders.Add(item3);
					}
				}
			}
		}

		// Token: 0x06005A39 RID: 23097 RVA: 0x00212747 File Offset: 0x00210B47
		protected void FindColliders()
		{
			this.colliders = new List<Collider>();
			this.gpuLineSphereColliders = new List<CapsuleLineSphereCollider>();
			this.gpuSphereColliders = new List<GpuSphereCollider>();
			this.FindCollidersInHandArray(this.leftHands);
			this.FindCollidersInHandArray(this.rightHands);
		}

		// Token: 0x06005A3A RID: 23098 RVA: 0x00212784 File Offset: 0x00210B84
		protected virtual void Init()
		{
			List<string> list = new List<string>();
			foreach (HandModelControl.Hand hand in this.leftHands)
			{
				list.Add(hand.name);
			}
			List<string> list2 = new List<string>();
			foreach (HandModelControl.Hand hand2 in this.rightHands)
			{
				list2.Add(hand2.name);
			}
			if (this.alwaysLinkHands)
			{
				this.leftHandChooser = new JSONStorableStringChooser("leftHandChoice", list, this._leftHandChoice, "Hand Choice", new JSONStorableStringChooser.SetStringCallback(this.SyncLeftHand));
			}
			else
			{
				this.leftHandChooser = new JSONStorableStringChooser("leftHandChoice", list, this._leftHandChoice, "Left Hand Choice", new JSONStorableStringChooser.SetStringCallback(this.SyncLeftHand));
			}
			this.rightHandChooser = new JSONStorableStringChooser("rightHandChoice", list2, this._rightHandChoice, "Right Hand Choice", new JSONStorableStringChooser.SetStringCallback(this.SyncRightHand));
			this.linkHandsJSON = new JSONStorableBool("linkHands", this._linkHands, new JSONStorableBool.SetBoolCallback(this.SyncLinkHands));
			this.leftHandEnabledJSON = new JSONStorableBool("leftHandEnabled", this._leftHandEnabled, new JSONStorableBool.SetBoolCallback(this.SyncLeftHandEnabled));
			this.rightHandEnabledJSON = new JSONStorableBool("rightHandEnabled", this._rightHandEnabled, new JSONStorableBool.SetBoolCallback(this.SyncRightHandEnabled));
			this.SetLeftHand(this._leftHandChoice);
			this.SetRightHand(this._rightHandChoice);
			this.useCollisionJSON = new JSONStorableBool("useCollision", this._useCollision, new JSONStorableBool.SetBoolCallback(this.SyncUseCollision));
			this.xPositionJSON = new JSONStorableFloat("xPosition", this._xPosition, new JSONStorableFloat.SetFloatCallback(this.SyncXPosition), -0.2f, 0.2f, true, true);
			this.yPositionJSON = new JSONStorableFloat("yPosition", this._yPosition, new JSONStorableFloat.SetFloatCallback(this.SyncYPosition), -0.2f, 0.2f, true, true);
			this.zPositionJSON = new JSONStorableFloat("zPosition", this._zPosition, new JSONStorableFloat.SetFloatCallback(this.SyncZPosition), -0.2f, 0.2f, true, true);
			this.xRotationJSON = new JSONStorableFloat("xRotation", this._xRotation, new JSONStorableFloat.SetFloatCallback(this.SyncXRotation), -90f, 90f, true, true);
			this.yRotationJSON = new JSONStorableFloat("yRotation", this._yRotation, new JSONStorableFloat.SetFloatCallback(this.SyncYRotation), -90f, 90f, true, true);
			this.zRotationJSON = new JSONStorableFloat("zRotation", this._zRotation, new JSONStorableFloat.SetFloatCallback(this.SyncZRotation), -90f, 90f, true, true);
			this.FindColliders();
			this.SetUseCollision(this._useCollision);
			this.FindJointReconnectors();
			this.SyncPosition();
			this.SyncRotation();
		}

		// Token: 0x06005A3B RID: 23099 RVA: 0x00212A60 File Offset: 0x00210E60
		protected override void InitUI(Transform t, bool isAlt)
		{
			if (t != null)
			{
				HandModelControlUI componentInChildren = t.GetComponentInChildren<HandModelControlUI>(true);
				if (componentInChildren != null)
				{
					this.leftHandChooser.RegisterPopup(componentInChildren.leftHandChooserPopup, isAlt);
					this.rightHandChooser.RegisterPopup(componentInChildren.rightHandChooserPopup, isAlt);
					this.leftHandEnabledJSON.RegisterToggle(componentInChildren.leftHandEnabledToggle, isAlt);
					this.rightHandEnabledJSON.RegisterToggle(componentInChildren.rightHandEnabledToggle, isAlt);
					this.linkHandsJSON.RegisterToggle(componentInChildren.linkHandsToggle, isAlt);
					this.useCollisionJSON.RegisterToggle(componentInChildren.useCollisionToggle, isAlt);
					this.xPositionJSON.RegisterSlider(componentInChildren.xPositionSlider, isAlt);
					this.yPositionJSON.RegisterSlider(componentInChildren.yPositionSlider, isAlt);
					this.zPositionJSON.RegisterSlider(componentInChildren.zPositionSlider, isAlt);
					this.xRotationJSON.RegisterSlider(componentInChildren.xRotationSlider, isAlt);
					this.yRotationJSON.RegisterSlider(componentInChildren.yRotationSlider, isAlt);
					this.zRotationJSON.RegisterSlider(componentInChildren.zRotationSlider, isAlt);
				}
			}
		}

		// Token: 0x06005A3C RID: 23100 RVA: 0x00212B65 File Offset: 0x00210F65
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

		// Token: 0x04004A37 RID: 18999
		public HandModelControl.Hand[] leftHands;

		// Token: 0x04004A38 RID: 19000
		public HandModelControl.Hand[] rightHands;

		// Token: 0x04004A39 RID: 19001
		public HandModelControl.Hand disabledLeftHand;

		// Token: 0x04004A3A RID: 19002
		public HandModelControl.Hand disabledRightHand;

		// Token: 0x04004A3B RID: 19003
		protected JSONStorableStringChooser leftHandChooser;

		// Token: 0x04004A3C RID: 19004
		[SerializeField]
		protected string _leftHandChoice = "None";

		// Token: 0x04004A3D RID: 19005
		protected JSONStorableBool leftHandEnabledJSON;

		// Token: 0x04004A3E RID: 19006
		[SerializeField]
		protected bool _leftHandEnabled;

		// Token: 0x04004A3F RID: 19007
		protected JSONStorableStringChooser rightHandChooser;

		// Token: 0x04004A40 RID: 19008
		[SerializeField]
		protected string _rightHandChoice = "None";

		// Token: 0x04004A41 RID: 19009
		protected JSONStorableBool rightHandEnabledJSON;

		// Token: 0x04004A42 RID: 19010
		[SerializeField]
		protected bool _rightHandEnabled;

		// Token: 0x04004A43 RID: 19011
		[SerializeField]
		protected bool _linkHands = true;

		// Token: 0x04004A44 RID: 19012
		protected JSONStorableBool linkHandsJSON;

		// Token: 0x04004A45 RID: 19013
		public bool alwaysLinkHands;

		// Token: 0x04004A46 RID: 19014
		protected List<ConfigurableJointReconnector> leftJointReconnectors;

		// Token: 0x04004A47 RID: 19015
		protected List<ConfigurableJointReconnector> rightJointReconnectors;

		// Token: 0x04004A48 RID: 19016
		protected Vector3 _leftPosition;

		// Token: 0x04004A49 RID: 19017
		protected Vector3 _rightPosition;

		// Token: 0x04004A4A RID: 19018
		public HandModelControl.Axis forwardBackAxis;

		// Token: 0x04004A4B RID: 19019
		public HandModelControl.Axis rightLeftAxis;

		// Token: 0x04004A4C RID: 19020
		public HandModelControl.Axis upDownAxis;

		// Token: 0x04004A4D RID: 19021
		protected JSONStorableFloat xPositionJSON;

		// Token: 0x04004A4E RID: 19022
		protected float _xPosition;

		// Token: 0x04004A4F RID: 19023
		protected JSONStorableFloat yPositionJSON;

		// Token: 0x04004A50 RID: 19024
		protected float _yPosition;

		// Token: 0x04004A51 RID: 19025
		protected JSONStorableFloat zPositionJSON;

		// Token: 0x04004A52 RID: 19026
		protected float _zPosition;

		// Token: 0x04004A53 RID: 19027
		protected Vector3 _leftRotation;

		// Token: 0x04004A54 RID: 19028
		protected Vector3 _rightRotation;

		// Token: 0x04004A55 RID: 19029
		protected Quaternion _qLeftRotation;

		// Token: 0x04004A56 RID: 19030
		protected Quaternion _qRightRotation;

		// Token: 0x04004A57 RID: 19031
		protected JSONStorableFloat xRotationJSON;

		// Token: 0x04004A58 RID: 19032
		protected float _xRotation;

		// Token: 0x04004A59 RID: 19033
		protected JSONStorableFloat yRotationJSON;

		// Token: 0x04004A5A RID: 19034
		protected float _yRotation;

		// Token: 0x04004A5B RID: 19035
		protected JSONStorableFloat zRotationJSON;

		// Token: 0x04004A5C RID: 19036
		protected float _zRotation;

		// Token: 0x04004A5D RID: 19037
		protected bool _ignorePositionRotationLeft;

		// Token: 0x04004A5E RID: 19038
		protected bool _ignorePositionRotationRight;

		// Token: 0x04004A5F RID: 19039
		public int collisionLayer;

		// Token: 0x04004A60 RID: 19040
		public int noCollisionLayer;

		// Token: 0x04004A61 RID: 19041
		protected List<Collider> colliders;

		// Token: 0x04004A62 RID: 19042
		protected List<CapsuleLineSphereCollider> gpuLineSphereColliders;

		// Token: 0x04004A63 RID: 19043
		protected List<GpuSphereCollider> gpuSphereColliders;

		// Token: 0x04004A64 RID: 19044
		[SerializeField]
		protected bool _useCollision;

		// Token: 0x04004A65 RID: 19045
		protected JSONStorableBool useCollisionJSON;

		// Token: 0x02000C1B RID: 3099
		[Serializable]
		public class Hand
		{
			// Token: 0x06005A3D RID: 23101 RVA: 0x00212B8A File Offset: 0x00210F8A
			public Hand()
			{
			}

			// Token: 0x04004A66 RID: 19046
			public string name;

			// Token: 0x04004A67 RID: 19047
			public Transform transform;
		}

		// Token: 0x02000C1C RID: 3100
		public enum Axis
		{
			// Token: 0x04004A69 RID: 19049
			X,
			// Token: 0x04004A6A RID: 19050
			Y,
			// Token: 0x04004A6B RID: 19051
			Z
		}
	}
}
