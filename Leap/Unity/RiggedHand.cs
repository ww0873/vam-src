using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Leap.Unity.Attributes;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006EC RID: 1772
	public class RiggedHand : HandModel
	{
		// Token: 0x06002AE3 RID: 10979 RVA: 0x000E73E0 File Offset: 0x000E57E0
		public RiggedHand()
		{
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06002AE4 RID: 10980 RVA: 0x000E745D File Offset: 0x000E585D
		public override ModelType HandModelType
		{
			get
			{
				return ModelType.Graphics;
			}
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x000E7460 File Offset: 0x000E5860
		public override bool SupportsEditorPersistence()
		{
			return this.SetEditorLeapPose;
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06002AE6 RID: 10982 RVA: 0x000E7468 File Offset: 0x000E5868
		// (set) Token: 0x06002AE7 RID: 10983 RVA: 0x000E7470 File Offset: 0x000E5870
		public bool SetEditorLeapPose
		{
			get
			{
				return this.setEditorLeapPose;
			}
			set
			{
				if (!value)
				{
					this.RestoreJointsStartPose();
				}
				this.setEditorLeapPose = value;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06002AE8 RID: 10984 RVA: 0x000E7485 File Offset: 0x000E5885
		// (set) Token: 0x06002AE9 RID: 10985 RVA: 0x000E748D File Offset: 0x000E588D
		public bool scaleLastFingerBones
		{
			get
			{
				return this._scaleLastFingerBones;
			}
			set
			{
				this._scaleLastFingerBones = value;
				this.setScaleLastFingerBoneInFingers(this._scaleLastFingerBones);
			}
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x000E74A2 File Offset: 0x000E58A2
		public override void InitHand()
		{
			this.UpdateHand();
			this.setDeformPositionsInFingers(this.deformPositionsState);
			this.setScaleLastFingerBoneInFingers(this.scaleLastFingerBones);
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x000E74C4 File Offset: 0x000E58C4
		public Quaternion Reorientation()
		{
			if (this.modelFingerPointing == Vector3.zero || this.modelPalmFacing == Vector3.zero)
			{
				return Quaternion.identity;
			}
			return Quaternion.Inverse(Quaternion.LookRotation(this.modelFingerPointing, -this.modelPalmFacing));
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x000E751C File Offset: 0x000E591C
		public override void UpdateHand()
		{
			if (this.palm != null)
			{
				if (this.ModelPalmAtLeapWrist)
				{
					this.palm.position = base.GetWristPosition();
				}
				else
				{
					this.palm.position = base.GetPalmPosition();
					if (this.wristJoint)
					{
						this.wristJoint.position = base.GetWristPosition();
					}
				}
				this.palm.rotation = this.GetRiggedPalmRotation() * this.Reorientation();
			}
			if (this.forearm != null)
			{
				this.forearm.rotation = base.GetArmRotation() * this.Reorientation();
			}
			for (int i = 0; i < this.fingers.Length; i++)
			{
				if (this.fingers[i] != null)
				{
					this.fingers[i].fingerType = (Finger.FingerType)i;
					this.fingers[i].UpdateFinger();
				}
			}
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x000E7620 File Offset: 0x000E5A20
		public Quaternion GetRiggedPalmRotation()
		{
			if (this.hand_ != null)
			{
				LeapTransform basis = this.hand_.Basis;
				return this.CalculateRotation(basis);
			}
			if (this.palm)
			{
				return this.palm.rotation;
			}
			return Quaternion.identity;
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x000E7670 File Offset: 0x000E5A70
		private Quaternion CalculateRotation(LeapTransform trs)
		{
			Vector3 upwards = trs.yBasis.ToVector3();
			Vector3 forward = trs.zBasis.ToVector3();
			return Quaternion.LookRotation(forward, upwards);
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x000E76A0 File Offset: 0x000E5AA0
		[ContextMenu("Setup Rigged Hand")]
		public void SetupRiggedHand()
		{
			Debug.Log("Using transform naming to setup RiggedHand on " + base.transform.name);
			this.modelFingerPointing = new Vector3(0f, 0f, 0f);
			this.modelPalmFacing = new Vector3(0f, 0f, 0f);
			this.assignRiggedFingersByName();
			this.SetupRiggedFingers();
			this.modelPalmFacing = this.calculateModelPalmFacing(this.palm, this.fingers[2].transform, this.fingers[1].transform);
			this.modelFingerPointing = this.calculateModelFingerPointing();
			this.setFingerPalmFacing();
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x000E7748 File Offset: 0x000E5B48
		public void AutoRigRiggedHand(Transform palm, Transform finger1, Transform finger2)
		{
			Debug.Log("Using Mecanim mapping to setup RiggedHand on " + base.transform.name);
			this.modelFingerPointing = new Vector3(0f, 0f, 0f);
			this.modelPalmFacing = new Vector3(0f, 0f, 0f);
			this.SetupRiggedFingers();
			this.modelPalmFacing = this.calculateModelPalmFacing(palm, finger1, finger2);
			this.modelFingerPointing = this.calculateModelFingerPointing();
			this.setFingerPalmFacing();
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x000E77CC File Offset: 0x000E5BCC
		private void assignRiggedFingersByName()
		{
			List<string> source = new List<string>
			{
				"palm"
			};
			List<string> source2 = new List<string>
			{
				"thumb",
				"tmb"
			};
			List<string> source3 = new List<string>
			{
				"index",
				"idx"
			};
			List<string> source4 = new List<string>
			{
				"middle",
				"mid"
			};
			List<string> source5 = new List<string>
			{
				"ring"
			};
			List<string> source6 = new List<string>
			{
				"pinky",
				"pin"
			};
			Transform[] componentsInChildren = base.transform.GetComponentsInChildren<Transform>();
			if (source.Any(new Func<string, bool>(this.<assignRiggedFingersByName>m__0)))
			{
				this.palm = base.transform;
			}
			else
			{
				Transform[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					RiggedHand.<assignRiggedFingersByName>c__AnonStorey0 <assignRiggedFingersByName>c__AnonStorey = new RiggedHand.<assignRiggedFingersByName>c__AnonStorey0();
					<assignRiggedFingersByName>c__AnonStorey.t = array[i];
					if (source.Any(new Func<string, bool>(<assignRiggedFingersByName>c__AnonStorey.<>m__0)))
					{
						this.palm = <assignRiggedFingersByName>c__AnonStorey.t;
					}
				}
			}
			if (!this.palm)
			{
				this.palm = base.transform;
			}
			if (this.palm)
			{
				foreach (Transform transform in componentsInChildren)
				{
					RiggedHand.<assignRiggedFingersByName>c__AnonStorey1 <assignRiggedFingersByName>c__AnonStorey2 = new RiggedHand.<assignRiggedFingersByName>c__AnonStorey1();
					RiggedFinger component = transform.GetComponent<RiggedFinger>();
					<assignRiggedFingersByName>c__AnonStorey2.lowercaseName = transform.name.ToLower();
					if (!component)
					{
						if (source2.Any(new Func<string, bool>(<assignRiggedFingersByName>c__AnonStorey2.<>m__0)) && transform.parent == this.palm)
						{
							Transform transform2 = transform;
							RiggedFinger riggedFinger = transform2.gameObject.AddComponent<RiggedFinger>();
							riggedFinger.fingerType = Finger.FingerType.TYPE_THUMB;
						}
						if (source3.Any(new Func<string, bool>(<assignRiggedFingersByName>c__AnonStorey2.<>m__1)) && transform.parent == this.palm)
						{
							Transform transform3 = transform;
							RiggedFinger riggedFinger2 = transform3.gameObject.AddComponent<RiggedFinger>();
							riggedFinger2.fingerType = Finger.FingerType.TYPE_INDEX;
						}
						if (source4.Any(new Func<string, bool>(<assignRiggedFingersByName>c__AnonStorey2.<>m__2)) && transform.parent == this.palm)
						{
							Transform transform4 = transform;
							RiggedFinger riggedFinger3 = transform4.gameObject.AddComponent<RiggedFinger>();
							riggedFinger3.fingerType = Finger.FingerType.TYPE_MIDDLE;
						}
						if (source5.Any(new Func<string, bool>(<assignRiggedFingersByName>c__AnonStorey2.<>m__3)) && transform.parent == this.palm)
						{
							Transform transform5 = transform;
							RiggedFinger riggedFinger4 = transform5.gameObject.AddComponent<RiggedFinger>();
							riggedFinger4.fingerType = Finger.FingerType.TYPE_RING;
						}
						if (source6.Any(new Func<string, bool>(<assignRiggedFingersByName>c__AnonStorey2.<>m__4)) && transform.parent == this.palm)
						{
							Transform transform6 = transform;
							RiggedFinger riggedFinger5 = transform6.gameObject.AddComponent<RiggedFinger>();
							riggedFinger5.fingerType = Finger.FingerType.TYPE_PINKY;
						}
					}
				}
			}
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x000E7B04 File Offset: 0x000E5F04
		private void SetupRiggedFingers()
		{
			RiggedFinger[] componentsInChildren = base.GetComponentsInChildren<RiggedFinger>();
			for (int i = 0; i < 5; i++)
			{
				int num = componentsInChildren[i].fingerType.indexOf();
				this.fingers[num] = componentsInChildren[i];
				componentsInChildren[i].SetupRiggedFinger(this.UseMetaCarpals);
			}
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x000E7B58 File Offset: 0x000E5F58
		private void setFingerPalmFacing()
		{
			RiggedFinger[] componentsInChildren = base.GetComponentsInChildren<RiggedFinger>();
			for (int i = 0; i < 5; i++)
			{
				int num = componentsInChildren[i].fingerType.indexOf();
				this.fingers[num] = componentsInChildren[i];
				componentsInChildren[i].modelPalmFacing = this.modelPalmFacing;
			}
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x000E7BAC File Offset: 0x000E5FAC
		private Vector3 calculateModelPalmFacing(Transform palm, Transform finger1, Transform finger2)
		{
			Vector3 b = palm.transform.InverseTransformPoint(palm.position);
			Vector3 a = palm.transform.InverseTransformPoint(finger1.position);
			Vector3 a2 = palm.transform.InverseTransformPoint(finger2.position);
			Vector3 vector = a - b;
			Vector3 vector2 = a2 - b;
			Vector3 vectorToZero;
			if (this.Handedness == Chirality.Left)
			{
				vectorToZero = Vector3.Cross(vector, vector2);
			}
			else
			{
				vectorToZero = Vector3.Cross(vector2, vector);
			}
			return RiggedHand.CalculateZeroedVector(vectorToZero);
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x000E7C30 File Offset: 0x000E6030
		private Vector3 calculateModelFingerPointing()
		{
			Vector3 vectorToZero = this.palm.transform.InverseTransformPoint(this.fingers[2].transform.GetChild(0).transform.position) - this.palm.transform.InverseTransformPoint(this.palm.position);
			Vector3 a = RiggedHand.CalculateZeroedVector(vectorToZero);
			return a * -1f;
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x000E7CA0 File Offset: 0x000E60A0
		public static Vector3 CalculateZeroedVector(Vector3 vectorToZero)
		{
			Vector3 result = default(Vector3);
			float num = Mathf.Max(new float[]
			{
				Mathf.Abs(vectorToZero.x),
				Mathf.Abs(vectorToZero.y),
				Mathf.Abs(vectorToZero.z)
			});
			if (Mathf.Abs(vectorToZero.x) == num)
			{
				result = ((vectorToZero.x >= 0f) ? new Vector3(-1f, 0f, 0f) : new Vector3(1f, 0f, 0f));
			}
			if (Mathf.Abs(vectorToZero.y) == num)
			{
				result = ((vectorToZero.y >= 0f) ? new Vector3(0f, -1f, 0f) : new Vector3(0f, 1f, 0f));
			}
			if (Mathf.Abs(vectorToZero.z) == num)
			{
				result = ((vectorToZero.z >= 0f) ? new Vector3(0f, 0f, -1f) : new Vector3(0f, 0f, 1f));
			}
			return result;
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x000E7DE4 File Offset: 0x000E61E4
		[ContextMenu("StoreJointsStartPose")]
		public void StoreJointsStartPose()
		{
			foreach (Transform transform in this.palm.parent.GetComponentsInChildren<Transform>())
			{
				this.jointList.Add(transform);
				this.localRotations.Add(transform.localRotation);
				this.localPositions.Add(transform.localPosition);
			}
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x000E7E48 File Offset: 0x000E6248
		[ContextMenu("RestoreJointsStartPose")]
		public void RestoreJointsStartPose()
		{
			for (int i = 0; i < this.jointList.Count; i++)
			{
				Transform transform = this.jointList[i];
				transform.localRotation = this.localRotations[i];
				transform.localPosition = this.localPositions[i];
			}
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x000E7EA4 File Offset: 0x000E62A4
		private void setDeformPositionsInFingers(bool onOff)
		{
			RiggedFinger[] componentsInChildren = base.GetComponentsInChildren<RiggedFinger>();
			foreach (RiggedFinger riggedFinger in componentsInChildren)
			{
				riggedFinger.deformPosition = onOff;
			}
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x000E7EDC File Offset: 0x000E62DC
		private void setScaleLastFingerBoneInFingers(bool shouldScale)
		{
			RiggedFinger[] componentsInChildren = base.GetComponentsInChildren<RiggedFinger>();
			foreach (RiggedFinger riggedFinger in componentsInChildren)
			{
				riggedFinger.scaleLastFingerBone = shouldScale;
			}
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x000E7F14 File Offset: 0x000E6314
		public void OnValidate()
		{
			if (this.DeformPositionsInFingers != this.deformPositionsState)
			{
				this.RestoreJointsStartPose();
				this.setDeformPositionsInFingers(this.DeformPositionsInFingers);
				this.deformPositionsState = this.DeformPositionsInFingers;
			}
			if (!this.setEditorLeapPose)
			{
				this.RestoreJointsStartPose();
			}
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x000E7F61 File Offset: 0x000E6361
		[CompilerGenerated]
		private bool <assignRiggedFingersByName>m__0(string w)
		{
			return base.transform.name.ToLower().Contains(w);
		}

		// Token: 0x040022CC RID: 8908
		[Tooltip("When True, hands will be put into a Leap editor pose near the LeapServiceProvider's transform.  When False, the hands will be returned to their Start Pose if it has been saved.")]
		[SerializeField]
		private bool setEditorLeapPose = true;

		// Token: 0x040022CD RID: 8909
		[SerializeField]
		public bool DeformPositionsInFingers;

		// Token: 0x040022CE RID: 8910
		[Tooltip("When True, hands will be put into a Leap editor pose near the LeapServiceProvider's transform.  When False, the hands will be returned to their Start Pose if it has been saved.")]
		[SerializeField]
		[HideInInspector]
		private bool deformPositionsState;

		// Token: 0x040022CF RID: 8911
		[Tooltip("Hands are typically rigged in 3D packages with the palm transform near the wrist. Uncheck this is your model's palm transform is at the center of the palm similar to Leap's API drives")]
		public bool ModelPalmAtLeapWrist = true;

		// Token: 0x040022D0 RID: 8912
		[Tooltip("Set to True if each finger has an extra trasform between palm and base of the finger.")]
		public bool UseMetaCarpals;

		// Token: 0x040022D1 RID: 8913
		[Tooltip("Because bones only exist at their roots in model rigs, the length of the last fingertip bone is lost when placing bones at positions in the tracked hand. This option scales the last bone along its X axis (length axis) to match its bone length to the tracked bone length. This option only has an effect if Deform Positions In Fingers is enabled.")]
		[DisableIf("DeformPositionsInFingers", false, null)]
		[SerializeField]
		[OnEditorChange("scaleLastFingerBones")]
		private bool _scaleLastFingerBones = true;

		// Token: 0x040022D2 RID: 8914
		public Vector3 modelFingerPointing = new Vector3(0f, 0f, 0f);

		// Token: 0x040022D3 RID: 8915
		public Vector3 modelPalmFacing = new Vector3(0f, 0f, 0f);

		// Token: 0x040022D4 RID: 8916
		[Header("Values for Stored Start Pose")]
		[SerializeField]
		private List<Transform> jointList = new List<Transform>();

		// Token: 0x040022D5 RID: 8917
		[SerializeField]
		private List<Quaternion> localRotations = new List<Quaternion>();

		// Token: 0x040022D6 RID: 8918
		[SerializeField]
		private List<Vector3> localPositions = new List<Vector3>();

		// Token: 0x02000FA8 RID: 4008
		[CompilerGenerated]
		private sealed class <assignRiggedFingersByName>c__AnonStorey0
		{
			// Token: 0x060074B4 RID: 29876 RVA: 0x000E7F79 File Offset: 0x000E6379
			public <assignRiggedFingersByName>c__AnonStorey0()
			{
			}

			// Token: 0x060074B5 RID: 29877 RVA: 0x000E7F81 File Offset: 0x000E6381
			internal bool <>m__0(string w)
			{
				return this.t.name.ToLower().Contains(w);
			}

			// Token: 0x040068CF RID: 26831
			internal Transform t;
		}

		// Token: 0x02000FA9 RID: 4009
		[CompilerGenerated]
		private sealed class <assignRiggedFingersByName>c__AnonStorey1
		{
			// Token: 0x060074B6 RID: 29878 RVA: 0x000E7F99 File Offset: 0x000E6399
			public <assignRiggedFingersByName>c__AnonStorey1()
			{
			}

			// Token: 0x060074B7 RID: 29879 RVA: 0x000E7FA1 File Offset: 0x000E63A1
			internal bool <>m__0(string w)
			{
				return this.lowercaseName.Contains(w);
			}

			// Token: 0x060074B8 RID: 29880 RVA: 0x000E7FAF File Offset: 0x000E63AF
			internal bool <>m__1(string w)
			{
				return this.lowercaseName.Contains(w);
			}

			// Token: 0x060074B9 RID: 29881 RVA: 0x000E7FBD File Offset: 0x000E63BD
			internal bool <>m__2(string w)
			{
				return this.lowercaseName.Contains(w);
			}

			// Token: 0x060074BA RID: 29882 RVA: 0x000E7FCB File Offset: 0x000E63CB
			internal bool <>m__3(string w)
			{
				return this.lowercaseName.Contains(w);
			}

			// Token: 0x060074BB RID: 29883 RVA: 0x000E7FD9 File Offset: 0x000E63D9
			internal bool <>m__4(string w)
			{
				return this.lowercaseName.Contains(w);
			}

			// Token: 0x040068D0 RID: 26832
			internal string lowercaseName;
		}
	}
}
