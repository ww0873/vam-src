using System;
using GPUTools.Physics.Scripts.Behaviours;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Tools
{
	// Token: 0x02000A7B RID: 2683
	public class BoneCollidersPlacer : MonoBehaviour
	{
		// Token: 0x060045A6 RID: 17830 RVA: 0x0013F2A5 File Offset: 0x0013D6A5
		public BoneCollidersPlacer()
		{
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x0013F2B4 File Offset: 0x0013D6B4
		[ContextMenu("Process")]
		public void Process()
		{
			this.Clear();
			this.Init();
			this.PlaceRecursive(base.transform, this.Depth);
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x0013F2D4 File Offset: 0x0013D6D4
		[ContextMenu("Clear")]
		public void Clear()
		{
			LineSphereCollider[] componentsInChildren = base.gameObject.GetComponentsInChildren<LineSphereCollider>();
			foreach (LineSphereCollider obj in componentsInChildren)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x0013F310 File Offset: 0x0013D710
		public void Fit()
		{
			this.Init();
			LineSphereCollider[] componentsInChildren = base.gameObject.GetComponentsInChildren<LineSphereCollider>();
			foreach (LineSphereCollider lineSphere in componentsInChildren)
			{
				for (int j = 0; j < 20; j++)
				{
					this.Rotate(lineSphere, 0.01f);
				}
			}
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x0013F36C File Offset: 0x0013D76C
		[ContextMenu("Grow")]
		public void Grow()
		{
			this.Init();
			LineSphereCollider[] componentsInChildren = base.gameObject.GetComponentsInChildren<LineSphereCollider>();
			foreach (LineSphereCollider lineSphereCollider in componentsInChildren)
			{
				lineSphereCollider.RadiusA += 0.01f;
				lineSphereCollider.RadiusB += 0.01f;
			}
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x0013F3CC File Offset: 0x0013D7CC
		private void Init()
		{
			Mesh mesh = new Mesh();
			this.Skin.BakeMesh(mesh);
			this.vertices = mesh.vertices;
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x0013F3F8 File Offset: 0x0013D7F8
		private void PlaceRecursive(Transform bone, int depth)
		{
			depth--;
			if (depth == 0)
			{
				return;
			}
			for (int i = 0; i < bone.childCount; i++)
			{
				Transform child = bone.GetChild(i);
				this.AddLineSpheres(bone, child);
				this.PlaceRecursive(child, depth);
			}
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x0013F440 File Offset: 0x0013D840
		private void AddLineSpheres(Transform bone, Transform child)
		{
			LineSphereCollider lineSphereCollider = bone.gameObject.AddComponent<LineSphereCollider>();
			lineSphereCollider.B = child.localPosition;
			lineSphereCollider.RadiusA = this.FindNearestMeshDistnce(this.Skin.transform.InverseTransformPoint(lineSphereCollider.WorldA));
			lineSphereCollider.RadiusB = this.FindNearestMeshDistnce(this.Skin.transform.InverseTransformPoint(lineSphereCollider.WorldB));
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x0013F4AC File Offset: 0x0013D8AC
		private float FindNearestMeshDistnce(Vector3 point)
		{
			float num = (this.vertices[0] - point).sqrMagnitude;
			for (int i = 1; i < this.vertices.Length; i++)
			{
				Vector3 a = this.vertices[i];
				float sqrMagnitude = (a - point).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
				}
			}
			return Mathf.Sqrt(num);
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x0013F528 File Offset: 0x0013D928
		private void Rotate(LineSphereCollider lineSphere, float step)
		{
			for (int i = 0; i < 50; i++)
			{
				Vector3 vector = lineSphere.WorldA + this.RandomVector() * step;
				float num = this.FindNearestMeshDistnce(this.Skin.transform.InverseTransformPoint(vector));
				if (num > lineSphere.WorldRadiusA)
				{
					lineSphere.WorldRadiusA = num;
					lineSphere.WorldA = vector;
					break;
				}
			}
			for (int j = 0; j < 50; j++)
			{
				Vector3 vector2 = lineSphere.WorldB + this.RandomVector() * step;
				float num2 = this.FindNearestMeshDistnce(this.Skin.transform.InverseTransformPoint(vector2));
				if (num2 > lineSphere.WorldRadiusB)
				{
					lineSphere.WorldRadiusA = num2;
					lineSphere.WorldA = vector2;
					break;
				}
			}
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x0013F5FF File Offset: 0x0013D9FF
		private Vector3 RandomVector()
		{
			return new Vector3((float)UnityEngine.Random.Range(-1, 2), (float)UnityEngine.Random.Range(-1, 2), (float)UnityEngine.Random.Range(-1, 2));
		}

		// Token: 0x04003349 RID: 13129
		[SerializeField]
		public SkinnedMeshRenderer Skin;

		// Token: 0x0400334A RID: 13130
		[SerializeField]
		public int Depth = 5;

		// Token: 0x0400334B RID: 13131
		private Vector3[] vertices;
	}
}
