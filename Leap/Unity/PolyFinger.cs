using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000758 RID: 1880
	public class PolyFinger : FingerModel
	{
		// Token: 0x06003061 RID: 12385 RVA: 0x000FB366 File Offset: 0x000F9766
		public PolyFinger()
		{
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x000FB381 File Offset: 0x000F9781
		public override void InitFinger()
		{
			this.InitJointVertices();
			this.InitCapsMesh();
			this.InitMesh();
			base.GetComponent<MeshFilter>().mesh = new Mesh();
			this.UpdateFinger();
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x000FB3AC File Offset: 0x000F97AC
		public override void UpdateFinger()
		{
			this.UpdateMesh();
			this.UpdateCapMesh();
			if (this.vertices_ == null)
			{
				return;
			}
			this.mesh_.vertices = this.vertices_;
			if (this.smoothNormals)
			{
				this.mesh_.normals = this.normals_;
			}
			else
			{
				this.mesh_.RecalculateNormals();
			}
			this.cap_mesh_.vertices = this.cap_vertices_;
			this.cap_mesh_.RecalculateNormals();
			CombineInstance[] array = new CombineInstance[2];
			array[0].mesh = this.mesh_;
			array[1].mesh = this.cap_mesh_;
			base.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(array, true, false);
			base.GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x000FB478 File Offset: 0x000F9878
		private void OnDestroy()
		{
			UnityEngine.Object.Destroy(this.mesh_);
			UnityEngine.Object.Destroy(this.cap_mesh_);
			UnityEngine.Object.Destroy(base.GetComponent<MeshFilter>().mesh);
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x000FB4A0 File Offset: 0x000F98A0
		private void Update()
		{
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x000FB4A2 File Offset: 0x000F98A2
		protected Quaternion GetJointRotation(int joint)
		{
			if (joint <= 0)
			{
				return base.GetBoneRotation(joint);
			}
			if (joint >= 4)
			{
				return base.GetBoneRotation(joint - 1);
			}
			return Quaternion.Slerp(base.GetBoneRotation(joint - 1), base.GetBoneRotation(joint), 0.5f);
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x000FB4E0 File Offset: 0x000F98E0
		protected void InitJointVertices()
		{
			this.joint_vertices_ = new Vector3[this.sides];
			for (int i = 0; i < this.sides; i++)
			{
				float angle = this.startingAngle + (float)i * 360f / (float)this.sides;
				this.joint_vertices_[i] = Quaternion.AngleAxis(angle, -Vector3.forward) * Vector3.up;
			}
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x000FB558 File Offset: 0x000F9958
		protected void UpdateMesh()
		{
			if (this.joint_vertices_ == null || this.joint_vertices_.Length != this.sides)
			{
				this.InitJointVertices();
			}
			if (this.normals_ == null || this.normals_.Length != 4 * this.sides * 4 || this.vertices_ == null || this.vertices_.Length != 4 * this.sides * 4)
			{
				this.InitMesh();
			}
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				Vector3 a = base.transform.InverseTransformPoint(base.GetJointPosition(i));
				Vector3 a2 = base.transform.InverseTransformPoint(base.GetJointPosition(i + 1));
				Quaternion rotation = Quaternion.Inverse(base.transform.rotation) * this.GetJointRotation(i);
				Quaternion rotation2 = Quaternion.Inverse(base.transform.rotation) * this.GetJointRotation(i + 1);
				for (int j = 0; j < this.sides; j++)
				{
					int num2 = (j + 1) % this.sides;
					if (this.smoothNormals)
					{
						Vector3 vector = rotation * this.joint_vertices_[j];
						Vector3 vector2 = rotation * this.joint_vertices_[num2];
						this.normals_[num] = (this.normals_[num + 2] = vector);
						this.normals_[num + 1] = (this.normals_[num + 3] = vector2);
					}
					Vector3 b = rotation * (this.widths[i] * this.joint_vertices_[j]);
					this.vertices_[num++] = a + b;
					b = rotation * (this.widths[i] * this.joint_vertices_[num2]);
					this.vertices_[num++] = a + b;
					b = rotation2 * (this.widths[i + 1] * this.joint_vertices_[j]);
					this.vertices_[num++] = a2 + b;
					b = rotation2 * (this.widths[i + 1] * this.joint_vertices_[num2]);
					this.vertices_[num++] = a2 + b;
				}
			}
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x000FB828 File Offset: 0x000F9C28
		protected void UpdateCapMesh()
		{
			Vector3 a = base.transform.InverseTransformPoint(base.GetJointPosition(0));
			Vector3 a2 = base.transform.InverseTransformPoint(base.GetJointPosition(2));
			Quaternion rotation = Quaternion.Inverse(base.transform.rotation) * this.GetJointRotation(0);
			Quaternion rotation2 = Quaternion.Inverse(base.transform.rotation) * this.GetJointRotation(2);
			if (this.cap_vertices_ == null || this.cap_vertices_.Length != 2 * this.sides)
			{
				this.InitCapsMesh();
			}
			for (int i = 0; i < this.sides; i++)
			{
				this.cap_vertices_[i] = a + rotation * (this.widths[0] * this.joint_vertices_[i]);
				this.cap_vertices_[this.sides + i] = a2 + rotation2 * (this.widths[2] * this.joint_vertices_[i]);
			}
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x000FB958 File Offset: 0x000F9D58
		protected void InitMesh()
		{
			this.mesh_ = new Mesh();
			this.mesh_.MarkDynamic();
			int num = 0;
			int num2 = 4 * this.sides * 4;
			this.vertices_ = new Vector3[num2];
			this.normals_ = new Vector3[num2];
			Vector2[] array = new Vector2[num2];
			int num3 = 0;
			int num4 = 6 * this.sides * 4;
			int[] array2 = new int[num4];
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < this.sides; j++)
				{
					array2[num3++] = num;
					array2[num3++] = num + 2;
					array2[num3++] = num + 1;
					array2[num3++] = num + 2;
					array2[num3++] = num + 3;
					array2[num3++] = num + 1;
					array[num] = new Vector3(1f * (float)j / (float)this.sides, 1f * (float)i / 4f);
					array[num + 1] = new Vector3((1f + (float)j) / (float)this.sides, 1f * (float)i / 4f);
					array[num + 2] = new Vector3(1f * (float)j / (float)this.sides, (1f + (float)i) / 4f);
					array[num + 3] = new Vector3((1f + (float)j) / (float)this.sides, (1f + (float)i) / 4f);
					this.vertices_[num++] = new Vector3(0f, 0f, 0f);
					this.vertices_[num++] = new Vector3(0f, 0f, 0f);
					this.vertices_[num++] = new Vector3(0f, 0f, 0f);
					this.vertices_[num++] = new Vector3(0f, 0f, 0f);
				}
			}
			this.mesh_.vertices = this.vertices_;
			this.mesh_.normals = this.normals_;
			this.mesh_.uv = array;
			this.mesh_.triangles = array2;
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x000FBBEC File Offset: 0x000F9FEC
		protected void InitCapsMesh()
		{
			this.cap_mesh_ = new Mesh();
			this.cap_mesh_.MarkDynamic();
			this.cap_vertices_ = this.cap_mesh_.vertices;
			int num = 2 * this.sides;
			if (num != this.cap_vertices_.Length)
			{
				Array.Resize<Vector3>(ref this.cap_vertices_, num);
			}
			Vector2[] uv = this.cap_mesh_.uv;
			if (uv.Length != num)
			{
				Array.Resize<Vector2>(ref uv, num);
			}
			int num2 = 0;
			int[] triangles = this.cap_mesh_.triangles;
			int num3 = 6 * (this.sides - 2);
			if (num3 != triangles.Length)
			{
				Array.Resize<int>(ref triangles, num3);
			}
			for (int i = 0; i < this.sides; i++)
			{
				this.cap_vertices_[i] = new Vector3(0f, 0f, 0f);
				this.cap_vertices_[i + this.sides] = new Vector3(0f, 0f, 0f);
				uv[i] = 0.5f * this.joint_vertices_[i];
				uv[i] += new Vector2(0.5f, 0.5f);
				uv[i + this.sides] = 0.5f * this.joint_vertices_[i];
				uv[i + this.sides] += new Vector2(0.5f, 0.5f);
			}
			for (int j = 0; j < this.sides - 2; j++)
			{
				triangles[num2++] = 0;
				triangles[num2++] = j + 1;
				triangles[num2++] = j + 2;
				triangles[num2++] = this.sides;
				triangles[num2++] = this.sides + j + 2;
				triangles[num2++] = this.sides + j + 1;
			}
			this.cap_mesh_.vertices = this.cap_vertices_;
			this.cap_mesh_.uv = uv;
			this.cap_mesh_.triangles = triangles;
		}

		// Token: 0x0400244E RID: 9294
		private const int MAX_SIDES = 30;

		// Token: 0x0400244F RID: 9295
		private const int TRIANGLE_INDICES_PER_QUAD = 6;

		// Token: 0x04002450 RID: 9296
		private const int VERTICES_PER_QUAD = 4;

		// Token: 0x04002451 RID: 9297
		public int sides = 4;

		// Token: 0x04002452 RID: 9298
		public bool smoothNormals;

		// Token: 0x04002453 RID: 9299
		public float startingAngle;

		// Token: 0x04002454 RID: 9300
		public float[] widths = new float[3];

		// Token: 0x04002455 RID: 9301
		protected Vector3[] vertices_;

		// Token: 0x04002456 RID: 9302
		protected Vector3[] normals_;

		// Token: 0x04002457 RID: 9303
		protected Vector3[] joint_vertices_;

		// Token: 0x04002458 RID: 9304
		protected Mesh mesh_;

		// Token: 0x04002459 RID: 9305
		protected Mesh cap_mesh_;

		// Token: 0x0400245A RID: 9306
		protected Vector3[] cap_vertices_;
	}
}
