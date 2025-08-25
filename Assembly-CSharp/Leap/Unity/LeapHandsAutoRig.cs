using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000756 RID: 1878
	[AddComponentMenu("Leap/Auto Rig Hands")]
	public class LeapHandsAutoRig : MonoBehaviour
	{
		// Token: 0x0600304D RID: 12365 RVA: 0x000FA384 File Offset: 0x000F8784
		public LeapHandsAutoRig()
		{
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x000FA400 File Offset: 0x000F8800
		[ContextMenu("AutoRig")]
		public void AutoRig()
		{
			this.HandPoolToPopulate = UnityEngine.Object.FindObjectOfType<HandModelManager>();
			this.AnimatorForMapping = base.gameObject.GetComponent<Animator>();
			if (this.AnimatorForMapping != null)
			{
				if (this.AnimatorForMapping.isHuman)
				{
					this.AutoRigMecanim();
					this.RiggedHand_L.StoreJointsStartPose();
					this.RiggedHand_R.StoreJointsStartPose();
					return;
				}
				Debug.LogWarning("The Mecanim Avatar for this asset does not contain a valid IsHuman definition.  Attempting to auto map by name.");
			}
			this.AutoRigByName();
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x000FA478 File Offset: 0x000F8878
		[ContextMenu("StoreStartPose")]
		public void StoreStartPose()
		{
			if (this.RiggedHand_L && this.RiggedHand_R)
			{
				this.RiggedHand_L.StoreJointsStartPose();
				this.RiggedHand_R.StoreJointsStartPose();
			}
			else
			{
				Debug.LogWarning("Please AutoRig before attempting to Store Start Pose");
			}
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x000FA4CC File Offset: 0x000F88CC
		[ContextMenu("AutoRigByName")]
		private void AutoRigByName()
		{
			List<string> source = new List<string>
			{
				"left"
			};
			List<string> source2 = new List<string>
			{
				"right"
			};
			this.HandPoolToPopulate = UnityEngine.Object.FindObjectOfType<HandModelManager>();
			this.Reset();
			Transform transform = null;
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					LeapHandsAutoRig.<AutoRigByName>c__AnonStorey0 <AutoRigByName>c__AnonStorey = new LeapHandsAutoRig.<AutoRigByName>c__AnonStorey0();
					<AutoRigByName>c__AnonStorey.t = (Transform)enumerator.Current;
					if (source.Any(new Func<string, bool>(<AutoRigByName>c__AnonStorey.<>m__0)))
					{
						transform = <AutoRigByName>c__AnonStorey.t;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			if (transform != null)
			{
				this.RiggedHand_L = transform.gameObject.AddComponent<RiggedHand>();
				this.HandTransitionBehavior_L = transform.gameObject.AddComponent<HandEnableDisable>();
				this.RiggedHand_L.Handedness = Chirality.Left;
				this.RiggedHand_L.SetEditorLeapPose = false;
				this.RiggedHand_L.UseMetaCarpals = this.UseMetaCarpals;
				this.RiggedHand_L.SetupRiggedHand();
				this.RiggedFinger_L_Thumb = (RiggedFinger)this.RiggedHand_L.fingers[0];
				this.RiggedFinger_L_Index = (RiggedFinger)this.RiggedHand_L.fingers[1];
				this.RiggedFinger_L_Mid = (RiggedFinger)this.RiggedHand_L.fingers[2];
				this.RiggedFinger_L_Ring = (RiggedFinger)this.RiggedHand_L.fingers[3];
				this.RiggedFinger_L_Pinky = (RiggedFinger)this.RiggedHand_L.fingers[4];
				this.modelFingerPointing_L = this.RiggedHand_L.modelFingerPointing;
				this.modelPalmFacing_L = this.RiggedHand_L.modelPalmFacing;
				this.RiggedHand_L.StoreJointsStartPose();
			}
			Transform transform2 = null;
			IEnumerator enumerator2 = base.transform.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					LeapHandsAutoRig.<AutoRigByName>c__AnonStorey1 <AutoRigByName>c__AnonStorey2 = new LeapHandsAutoRig.<AutoRigByName>c__AnonStorey1();
					<AutoRigByName>c__AnonStorey2.t = (Transform)enumerator2.Current;
					if (source2.Any(new Func<string, bool>(<AutoRigByName>c__AnonStorey2.<>m__0)))
					{
						transform2 = <AutoRigByName>c__AnonStorey2.t;
					}
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator2 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
			if (transform2 != null)
			{
				this.RiggedHand_R = transform2.gameObject.AddComponent<RiggedHand>();
				this.HandTransitionBehavior_R = transform2.gameObject.AddComponent<HandEnableDisable>();
				this.RiggedHand_R.Handedness = Chirality.Right;
				this.RiggedHand_R.SetEditorLeapPose = false;
				this.RiggedHand_R.UseMetaCarpals = this.UseMetaCarpals;
				this.RiggedHand_R.SetupRiggedHand();
				this.RiggedFinger_R_Thumb = (RiggedFinger)this.RiggedHand_R.fingers[0];
				this.RiggedFinger_R_Index = (RiggedFinger)this.RiggedHand_R.fingers[1];
				this.RiggedFinger_R_Mid = (RiggedFinger)this.RiggedHand_R.fingers[2];
				this.RiggedFinger_R_Ring = (RiggedFinger)this.RiggedHand_R.fingers[3];
				this.RiggedFinger_R_Pinky = (RiggedFinger)this.RiggedHand_R.fingers[4];
				this.modelFingerPointing_R = this.RiggedHand_R.modelFingerPointing;
				this.modelPalmFacing_R = this.RiggedHand_R.modelPalmFacing;
				this.RiggedHand_R.StoreJointsStartPose();
			}
			if (this.ModelGroupName == string.Empty || this.ModelGroupName != null)
			{
				this.ModelGroupName = base.transform.name;
			}
			this.HandPoolToPopulate.AddNewGroup(this.ModelGroupName, this.RiggedHand_L, this.RiggedHand_R);
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x000FA87C File Offset: 0x000F8C7C
		[ContextMenu("AutoRigMecanim")]
		private void AutoRigMecanim()
		{
			this.AnimatorForMapping = base.gameObject.GetComponent<Animator>();
			this.HandPoolToPopulate = UnityEngine.Object.FindObjectOfType<HandModelManager>();
			this.Reset();
			Transform boneTransform = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.LeftHand);
			if (boneTransform.GetComponent<RiggedHand>())
			{
				this.RiggedHand_L = boneTransform.GetComponent<RiggedHand>();
			}
			else
			{
				this.RiggedHand_L = boneTransform.gameObject.AddComponent<RiggedHand>();
			}
			this.HandTransitionBehavior_L = boneTransform.gameObject.AddComponent<HandDrop>();
			this.RiggedHand_L.Handedness = Chirality.Left;
			this.RiggedHand_L.SetEditorLeapPose = false;
			Transform boneTransform2 = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.RightHand);
			if (boneTransform2.GetComponent<RiggedHand>())
			{
				this.RiggedHand_R = boneTransform2.GetComponent<RiggedHand>();
			}
			else
			{
				this.RiggedHand_R = boneTransform2.gameObject.AddComponent<RiggedHand>();
			}
			this.HandTransitionBehavior_R = boneTransform2.gameObject.AddComponent<HandDrop>();
			this.RiggedHand_R.Handedness = Chirality.Right;
			this.RiggedHand_R.SetEditorLeapPose = false;
			this.RiggedHand_L.palm = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.LeftHand);
			this.RiggedHand_R.palm = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.RightHand);
			this.RiggedHand_R.UseMetaCarpals = this.UseMetaCarpals;
			this.RiggedHand_L.UseMetaCarpals = this.UseMetaCarpals;
			this.findAndAssignRiggedFingers(this.UseMetaCarpals);
			this.RiggedHand_L.AutoRigRiggedHand(this.RiggedHand_L.palm, this.RiggedFinger_L_Pinky.transform, this.RiggedFinger_L_Index.transform);
			this.RiggedHand_R.AutoRigRiggedHand(this.RiggedHand_R.palm, this.RiggedFinger_R_Pinky.transform, this.RiggedFinger_R_Index.transform);
			if (this.ModelGroupName == string.Empty || this.ModelGroupName != null)
			{
				this.ModelGroupName = base.transform.name;
			}
			this.HandPoolToPopulate.AddNewGroup(this.ModelGroupName, this.RiggedHand_L, this.RiggedHand_R);
			this.modelFingerPointing_L = this.RiggedHand_L.modelFingerPointing;
			this.modelPalmFacing_L = this.RiggedHand_L.modelPalmFacing;
			this.modelFingerPointing_R = this.RiggedHand_R.modelFingerPointing;
			this.modelPalmFacing_R = this.RiggedHand_R.modelPalmFacing;
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x000FAAC8 File Offset: 0x000F8EC8
		private void findAndAssignRiggedFingers(bool useMetaCarpals)
		{
			if (!useMetaCarpals)
			{
				this.RiggedFinger_L_Thumb = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.LeftThumbProximal).gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_L_Index = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.LeftIndexProximal).gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_L_Mid = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.LeftMiddleProximal).gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_L_Ring = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.LeftRingProximal).gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_L_Pinky = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.LeftLittleProximal).gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_R_Thumb = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.RightThumbProximal).gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_R_Index = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.RightIndexProximal).gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_R_Mid = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.RightMiddleProximal).gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_R_Ring = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.RightRingProximal).gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_R_Pinky = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.RightLittleProximal).gameObject.AddComponent<RiggedFinger>();
			}
			else
			{
				this.RiggedFinger_L_Thumb = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.LeftThumbProximal).gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_L_Index = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.LeftIndexProximal).gameObject.transform.parent.gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_L_Mid = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.LeftMiddleProximal).gameObject.transform.parent.gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_L_Ring = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.LeftRingProximal).gameObject.transform.parent.gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_L_Pinky = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.LeftLittleProximal).gameObject.transform.parent.gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_R_Thumb = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.RightThumbProximal).gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_R_Index = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.RightIndexProximal).gameObject.transform.parent.gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_R_Mid = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.RightMiddleProximal).gameObject.transform.parent.gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_R_Ring = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.RightRingProximal).gameObject.transform.parent.gameObject.AddComponent<RiggedFinger>();
				this.RiggedFinger_R_Pinky = this.AnimatorForMapping.GetBoneTransform(HumanBodyBones.RightLittleProximal).gameObject.transform.parent.gameObject.AddComponent<RiggedFinger>();
			}
			this.RiggedFinger_L_Thumb.fingerType = Finger.FingerType.TYPE_THUMB;
			this.RiggedFinger_L_Index.fingerType = Finger.FingerType.TYPE_INDEX;
			this.RiggedFinger_L_Mid.fingerType = Finger.FingerType.TYPE_MIDDLE;
			this.RiggedFinger_L_Ring.fingerType = Finger.FingerType.TYPE_RING;
			this.RiggedFinger_L_Pinky.fingerType = Finger.FingerType.TYPE_PINKY;
			this.RiggedFinger_R_Thumb.fingerType = Finger.FingerType.TYPE_THUMB;
			this.RiggedFinger_R_Index.fingerType = Finger.FingerType.TYPE_INDEX;
			this.RiggedFinger_R_Mid.fingerType = Finger.FingerType.TYPE_MIDDLE;
			this.RiggedFinger_R_Ring.fingerType = Finger.FingerType.TYPE_RING;
			this.RiggedFinger_R_Pinky.fingerType = Finger.FingerType.TYPE_PINKY;
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000FAE14 File Offset: 0x000F9214
		private void Reset()
		{
			RiggedFinger[] componentsInChildren = base.GetComponentsInChildren<RiggedFinger>();
			foreach (RiggedFinger obj in componentsInChildren)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			UnityEngine.Object.DestroyImmediate(this.RiggedHand_L);
			UnityEngine.Object.DestroyImmediate(this.RiggedHand_R);
			UnityEngine.Object.DestroyImmediate(this.HandTransitionBehavior_L);
			UnityEngine.Object.DestroyImmediate(this.HandTransitionBehavior_R);
			if (this.HandPoolToPopulate != null)
			{
				this.HandPoolToPopulate.RemoveGroup(this.ModelGroupName);
			}
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x000FAE98 File Offset: 0x000F9298
		public void PushVectorValues()
		{
			if (this.RiggedHand_L)
			{
				this.RiggedHand_L.modelFingerPointing = this.modelFingerPointing_L;
				this.RiggedHand_L.modelPalmFacing = this.modelPalmFacing_L;
			}
			if (this.RiggedHand_R)
			{
				this.RiggedHand_R.modelFingerPointing = this.modelFingerPointing_R;
				this.RiggedHand_R.modelPalmFacing = this.modelPalmFacing_R;
			}
			if (this.RiggedFinger_L_Thumb)
			{
				this.RiggedFinger_L_Thumb.modelFingerPointing = this.modelFingerPointing_L;
				this.RiggedFinger_L_Thumb.modelPalmFacing = this.modelPalmFacing_L;
			}
			if (this.RiggedFinger_L_Index)
			{
				this.RiggedFinger_L_Index.modelFingerPointing = this.modelFingerPointing_L;
				this.RiggedFinger_L_Index.modelPalmFacing = this.modelPalmFacing_L;
			}
			if (this.RiggedFinger_L_Mid)
			{
				this.RiggedFinger_L_Mid.modelFingerPointing = this.modelFingerPointing_L;
				this.RiggedFinger_L_Mid.modelPalmFacing = this.modelPalmFacing_L;
			}
			if (this.RiggedFinger_L_Ring)
			{
				this.RiggedFinger_L_Ring.modelFingerPointing = this.modelFingerPointing_L;
				this.RiggedFinger_L_Ring.modelPalmFacing = this.modelPalmFacing_L;
			}
			if (this.RiggedFinger_L_Pinky)
			{
				this.RiggedFinger_L_Pinky.modelFingerPointing = this.modelFingerPointing_L;
				this.RiggedFinger_L_Pinky.modelPalmFacing = this.modelPalmFacing_L;
			}
			if (this.RiggedFinger_R_Thumb)
			{
				this.RiggedFinger_R_Thumb.modelFingerPointing = this.modelFingerPointing_R;
				this.RiggedFinger_R_Thumb.modelPalmFacing = this.modelPalmFacing_R;
			}
			if (this.RiggedFinger_R_Index)
			{
				this.RiggedFinger_R_Index.modelFingerPointing = this.modelFingerPointing_R;
				this.RiggedFinger_R_Index.modelPalmFacing = this.modelPalmFacing_R;
			}
			if (this.RiggedFinger_R_Mid)
			{
				this.RiggedFinger_R_Mid.modelFingerPointing = this.modelFingerPointing_R;
				this.RiggedFinger_R_Mid.modelPalmFacing = this.modelPalmFacing_R;
			}
			if (this.RiggedFinger_R_Ring)
			{
				this.RiggedFinger_R_Ring.modelFingerPointing = this.modelFingerPointing_R;
				this.RiggedFinger_R_Ring.modelPalmFacing = this.modelPalmFacing_R;
			}
			if (this.RiggedFinger_R_Pinky)
			{
				this.RiggedFinger_R_Pinky.modelFingerPointing = this.modelFingerPointing_R;
				this.RiggedFinger_R_Pinky.modelPalmFacing = this.modelPalmFacing_R;
			}
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x000FB100 File Offset: 0x000F9500
		private void OnValidate()
		{
			if (this.FlipPalms != this.flippedPalmsState)
			{
				this.modelPalmFacing_L *= -1f;
				this.modelPalmFacing_R *= -1f;
				this.flippedPalmsState = this.FlipPalms;
				this.PushVectorValues();
			}
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x000FB15C File Offset: 0x000F955C
		private void OnDestroy()
		{
			if (this.HandPoolToPopulate != null)
			{
				this.HandPoolToPopulate.RemoveGroup(this.ModelGroupName);
			}
		}

		// Token: 0x0400242C RID: 9260
		private HandModelManager HandPoolToPopulate;

		// Token: 0x0400242D RID: 9261
		public Animator AnimatorForMapping;

		// Token: 0x0400242E RID: 9262
		public string ModelGroupName;

		// Token: 0x0400242F RID: 9263
		[Tooltip("Set to True if each finger has an extra trasform between palm and base of the finger.")]
		public bool UseMetaCarpals;

		// Token: 0x04002430 RID: 9264
		[Header("RiggedHand Components")]
		public RiggedHand RiggedHand_L;

		// Token: 0x04002431 RID: 9265
		public RiggedHand RiggedHand_R;

		// Token: 0x04002432 RID: 9266
		[Header("HandTransitionBehavior Components")]
		public HandTransitionBehavior HandTransitionBehavior_L;

		// Token: 0x04002433 RID: 9267
		public HandTransitionBehavior HandTransitionBehavior_R;

		// Token: 0x04002434 RID: 9268
		[Tooltip("Test")]
		[Header("RiggedFinger Components")]
		public RiggedFinger RiggedFinger_L_Thumb;

		// Token: 0x04002435 RID: 9269
		public RiggedFinger RiggedFinger_L_Index;

		// Token: 0x04002436 RID: 9270
		public RiggedFinger RiggedFinger_L_Mid;

		// Token: 0x04002437 RID: 9271
		public RiggedFinger RiggedFinger_L_Ring;

		// Token: 0x04002438 RID: 9272
		public RiggedFinger RiggedFinger_L_Pinky;

		// Token: 0x04002439 RID: 9273
		public RiggedFinger RiggedFinger_R_Thumb;

		// Token: 0x0400243A RID: 9274
		public RiggedFinger RiggedFinger_R_Index;

		// Token: 0x0400243B RID: 9275
		public RiggedFinger RiggedFinger_R_Mid;

		// Token: 0x0400243C RID: 9276
		public RiggedFinger RiggedFinger_R_Ring;

		// Token: 0x0400243D RID: 9277
		public RiggedFinger RiggedFinger_R_Pinky;

		// Token: 0x0400243E RID: 9278
		[Header("Palm & Finger Direction Vectors.")]
		public Vector3 modelFingerPointing_L = new Vector3(0f, 0f, 0f);

		// Token: 0x0400243F RID: 9279
		public Vector3 modelPalmFacing_L = new Vector3(0f, 0f, 0f);

		// Token: 0x04002440 RID: 9280
		public Vector3 modelFingerPointing_R = new Vector3(0f, 0f, 0f);

		// Token: 0x04002441 RID: 9281
		public Vector3 modelPalmFacing_R = new Vector3(0f, 0f, 0f);

		// Token: 0x04002442 RID: 9282
		[Tooltip("Toggling this value will reverse the ModelPalmFacing vectors to both RiggedHand's and all RiggedFingers.  Change if hands appear backward when tracking.")]
		[SerializeField]
		public bool FlipPalms;

		// Token: 0x04002443 RID: 9283
		[SerializeField]
		[HideInInspector]
		private bool flippedPalmsState;

		// Token: 0x02000FB3 RID: 4019
		[CompilerGenerated]
		private sealed class <AutoRigByName>c__AnonStorey0
		{
			// Token: 0x060074D4 RID: 29908 RVA: 0x000FB180 File Offset: 0x000F9580
			public <AutoRigByName>c__AnonStorey0()
			{
			}

			// Token: 0x060074D5 RID: 29909 RVA: 0x000FB188 File Offset: 0x000F9588
			internal bool <>m__0(string w)
			{
				return this.t.name.ToLower().Contains(w);
			}

			// Token: 0x040068E0 RID: 26848
			internal Transform t;
		}

		// Token: 0x02000FB4 RID: 4020
		[CompilerGenerated]
		private sealed class <AutoRigByName>c__AnonStorey1
		{
			// Token: 0x060074D6 RID: 29910 RVA: 0x000FB1A0 File Offset: 0x000F95A0
			public <AutoRigByName>c__AnonStorey1()
			{
			}

			// Token: 0x060074D7 RID: 29911 RVA: 0x000FB1A8 File Offset: 0x000F95A8
			internal bool <>m__0(string w)
			{
				return this.t.name.ToLower().Contains(w);
			}

			// Token: 0x040068E1 RID: 26849
			internal Transform t;
		}
	}
}
