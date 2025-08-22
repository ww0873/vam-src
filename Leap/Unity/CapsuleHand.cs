using System;
using System.Collections.Generic;
using Leap.Unity.Attributes;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006E1 RID: 1761
	public class CapsuleHand : HandModelBase
	{
		// Token: 0x06002A79 RID: 10873 RVA: 0x000E59C8 File Offset: 0x000E3DC8
		public CapsuleHand()
		{
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06002A7A RID: 10874 RVA: 0x000E5A1D File Offset: 0x000E3E1D
		public override ModelType HandModelType
		{
			get
			{
				return ModelType.Graphics;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06002A7B RID: 10875 RVA: 0x000E5A20 File Offset: 0x000E3E20
		// (set) Token: 0x06002A7C RID: 10876 RVA: 0x000E5A28 File Offset: 0x000E3E28
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

		// Token: 0x06002A7D RID: 10877 RVA: 0x000E5A2A File Offset: 0x000E3E2A
		public override bool SupportsEditorPersistence()
		{
			return true;
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x000E5A2D File Offset: 0x000E3E2D
		public override Hand GetLeapHand()
		{
			return this._hand;
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x000E5A35 File Offset: 0x000E3E35
		public override void SetLeapHand(Hand hand)
		{
			this._hand = hand;
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x000E5A3E File Offset: 0x000E3E3E
		public override void InitHand()
		{
			if (this._material != null)
			{
				this._sphereMat = new Material(this._material);
				this._sphereMat.hideFlags = HideFlags.DontSaveInEditor;
			}
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x000E5A6E File Offset: 0x000E3E6E
		private void OnValidate()
		{
			this._meshMap.Clear();
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x000E5A7C File Offset: 0x000E3E7C
		public override void BeginHand()
		{
			base.BeginHand();
			if (this._hand.IsLeft)
			{
				this._sphereMat.color = CapsuleHand._leftColorList[CapsuleHand._leftColorIndex];
				CapsuleHand._leftColorIndex = (CapsuleHand._leftColorIndex + 1) % CapsuleHand._leftColorList.Length;
			}
			else
			{
				this._sphereMat.color = CapsuleHand._rightColorList[CapsuleHand._rightColorIndex];
				CapsuleHand._rightColorIndex = (CapsuleHand._rightColorIndex + 1) % CapsuleHand._rightColorList.Length;
			}
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x000E5B0C File Offset: 0x000E3F0C
		public override void UpdateHand()
		{
			if (this._spherePositions == null || this._spherePositions.Length != 20)
			{
				this._spherePositions = new Vector3[20];
			}
			if (this._sphereMat == null)
			{
				this._sphereMat = new Material(this._material);
				this._sphereMat.hideFlags = HideFlags.DontSaveInEditor;
			}
			foreach (Finger finger in this._hand.Fingers)
			{
				for (int i = 0; i < 4; i++)
				{
					int fingerJointIndex = this.getFingerJointIndex((int)finger.Type, i);
					Vector3 vector = finger.Bone((Bone.BoneType)i).NextJoint.ToVector3();
					this._spherePositions[fingerJointIndex] = vector;
					this.drawSphere(vector);
				}
			}
			Vector3 position = this._hand.PalmPosition.ToVector3();
			this.drawSphere(position, this._palmRadius);
			Vector3 inDirection = this._spherePositions[0] - this._hand.PalmPosition.ToVector3();
			Vector3 vector2 = this._hand.PalmPosition.ToVector3() + Vector3.Reflect(inDirection, this._hand.Basis.xBasis.ToVector3());
			this.drawSphere(vector2);
			if (this._showArm)
			{
				Arm arm = this._hand.Arm;
				Vector3 b = arm.Basis.xBasis.ToVector3() * arm.Width * 0.7f * 0.5f;
				Vector3 a = arm.WristPosition.ToVector3();
				Vector3 vector3 = arm.ElbowPosition.ToVector3();
				float d = Vector3.Distance(a, vector3);
				a -= arm.Direction.ToVector3() * d * 0.05f;
				Vector3 vector4 = a + b;
				Vector3 vector5 = a - b;
				Vector3 vector6 = vector3 + b;
				Vector3 vector7 = vector3 - b;
				this.drawSphere(vector4);
				this.drawSphere(vector5);
				this.drawSphere(vector7);
				this.drawSphere(vector6);
				this.drawCylinder(vector5, vector4);
				this.drawCylinder(vector7, vector6);
				this.drawCylinder(vector5, vector7);
				this.drawCylinder(vector4, vector6);
			}
			for (int j = 0; j < 5; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					int fingerJointIndex2 = this.getFingerJointIndex(j, k);
					int fingerJointIndex3 = this.getFingerJointIndex(j, k + 1);
					Vector3 a2 = this._spherePositions[fingerJointIndex2];
					Vector3 b2 = this._spherePositions[fingerJointIndex3];
					this.drawCylinder(a2, b2);
				}
			}
			for (int l = 0; l < 4; l++)
			{
				int fingerJointIndex4 = this.getFingerJointIndex(l, 0);
				int fingerJointIndex5 = this.getFingerJointIndex(l + 1, 0);
				Vector3 a3 = this._spherePositions[fingerJointIndex4];
				Vector3 b3 = this._spherePositions[fingerJointIndex5];
				this.drawCylinder(a3, b3);
			}
			this.drawCylinder(vector2, 0);
			this.drawCylinder(vector2, 16);
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x000E5E8C File Offset: 0x000E428C
		private void drawSphere(Vector3 position)
		{
			this.drawSphere(position, this._jointRadius);
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x000E5E9C File Offset: 0x000E429C
		private void drawSphere(Vector3 position, float radius)
		{
			Graphics.DrawMesh(this._sphereMesh, Matrix4x4.TRS(position, Quaternion.identity, Vector3.one * radius * 2f * base.transform.lossyScale.x), this._sphereMat, 0, null, 0, null, this._castShadows);
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x000E5EFC File Offset: 0x000E42FC
		private void drawCylinder(Vector3 a, Vector3 b)
		{
			float magnitude = (a - b).magnitude;
			Graphics.DrawMesh(this.getCylinderMesh(magnitude), Matrix4x4.TRS(a, Quaternion.LookRotation(b - a), new Vector3(base.transform.lossyScale.x, base.transform.lossyScale.x, 1f)), this._material, base.gameObject.layer, null, 0, null, this._castShadows);
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x000E5F81 File Offset: 0x000E4381
		private void drawCylinder(int a, int b)
		{
			this.drawCylinder(this._spherePositions[a], this._spherePositions[b]);
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x000E5FAB File Offset: 0x000E43AB
		private void drawCylinder(Vector3 a, int b)
		{
			this.drawCylinder(a, this._spherePositions[b]);
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x000E5FC5 File Offset: 0x000E43C5
		private int getFingerJointIndex(int fingerIndex, int jointIndex)
		{
			return fingerIndex * 4 + jointIndex;
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000E5FCC File Offset: 0x000E43CC
		private Mesh getCylinderMesh(float length)
		{
			int key = Mathf.RoundToInt(length * 100f / 0.1f);
			Mesh mesh;
			if (this._meshMap.TryGetValue(key, out mesh))
			{
				return mesh;
			}
			mesh = new Mesh();
			mesh.name = "GeneratedCylinder";
			mesh.hideFlags = HideFlags.DontSave;
			List<Vector3> list = new List<Vector3>();
			List<Color> list2 = new List<Color>();
			List<int> list3 = new List<int>();
			Vector3 zero = Vector3.zero;
			Vector3 a = Vector3.forward * length;
			for (int i = 0; i < this._cylinderResolution; i++)
			{
				float f = 6.2831855f * (float)i / (float)this._cylinderResolution;
				float x = this._cylinderRadius * Mathf.Cos(f);
				float y = this._cylinderRadius * Mathf.Sin(f);
				Vector3 b = new Vector3(x, y, 0f);
				list.Add(zero + b);
				list.Add(a + b);
				list2.Add(Color.white);
				list2.Add(Color.white);
				int count = list.Count;
				int num = this._cylinderResolution * 2;
				list3.Add(count % num);
				list3.Add((count + 2) % num);
				list3.Add((count + 1) % num);
				list3.Add((count + 2) % num);
				list3.Add((count + 3) % num);
				list3.Add((count + 1) % num);
			}
			mesh.SetVertices(list);
			mesh.SetIndices(list3.ToArray(), MeshTopology.Triangles, 0);
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			mesh.UploadMeshData(true);
			this._meshMap[key] = mesh;
			return mesh;
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000E6170 File Offset: 0x000E4570
		// Note: this type is marked as 'beforefieldinit'.
		static CapsuleHand()
		{
		}

		// Token: 0x0400228F RID: 8847
		private const int TOTAL_JOINT_COUNT = 20;

		// Token: 0x04002290 RID: 8848
		private const float CYLINDER_MESH_RESOLUTION = 0.1f;

		// Token: 0x04002291 RID: 8849
		private const int THUMB_BASE_INDEX = 0;

		// Token: 0x04002292 RID: 8850
		private const int PINKY_BASE_INDEX = 16;

		// Token: 0x04002293 RID: 8851
		private static int _leftColorIndex = 0;

		// Token: 0x04002294 RID: 8852
		private static int _rightColorIndex = 0;

		// Token: 0x04002295 RID: 8853
		private static Color[] _leftColorList = new Color[]
		{
			new Color(0f, 0f, 1f),
			new Color(0.2f, 0f, 0.4f),
			new Color(0f, 0.2f, 0.2f)
		};

		// Token: 0x04002296 RID: 8854
		private static Color[] _rightColorList = new Color[]
		{
			new Color(1f, 0f, 0f),
			new Color(1f, 1f, 0f),
			new Color(1f, 0.5f, 0f)
		};

		// Token: 0x04002297 RID: 8855
		[SerializeField]
		private Chirality handedness;

		// Token: 0x04002298 RID: 8856
		[SerializeField]
		private bool _showArm = true;

		// Token: 0x04002299 RID: 8857
		[SerializeField]
		private bool _castShadows = true;

		// Token: 0x0400229A RID: 8858
		[SerializeField]
		private Material _material;

		// Token: 0x0400229B RID: 8859
		[SerializeField]
		private Mesh _sphereMesh;

		// Token: 0x0400229C RID: 8860
		[MinValue(3f)]
		[SerializeField]
		private int _cylinderResolution = 12;

		// Token: 0x0400229D RID: 8861
		[MinValue(0f)]
		[SerializeField]
		private float _jointRadius = 0.008f;

		// Token: 0x0400229E RID: 8862
		[MinValue(0f)]
		[SerializeField]
		private float _cylinderRadius = 0.006f;

		// Token: 0x0400229F RID: 8863
		[MinValue(0f)]
		[SerializeField]
		private float _palmRadius = 0.015f;

		// Token: 0x040022A0 RID: 8864
		private Material _sphereMat;

		// Token: 0x040022A1 RID: 8865
		private Hand _hand;

		// Token: 0x040022A2 RID: 8866
		private Vector3[] _spherePositions;

		// Token: 0x040022A3 RID: 8867
		private Dictionary<int, Mesh> _meshMap = new Dictionary<int, Mesh>();
	}
}
