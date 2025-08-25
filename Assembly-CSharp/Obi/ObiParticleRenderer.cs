using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x02000367 RID: 871
	[ExecuteInEditMode]
	[RequireComponent(typeof(ObiActor))]
	public class ObiParticleRenderer : MonoBehaviour
	{
		// Token: 0x060015C1 RID: 5569 RVA: 0x0007C498 File Offset: 0x0007A898
		public ObiParticleRenderer()
		{
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060015C2 RID: 5570 RVA: 0x0007C58B File Offset: 0x0007A98B
		public IEnumerable<Mesh> ParticleMeshes
		{
			get
			{
				return this.meshes;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060015C3 RID: 5571 RVA: 0x0007C593 File Offset: 0x0007A993
		public Material ParticleMaterial
		{
			get
			{
				return this.material;
			}
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x0007C59B File Offset: 0x0007A99B
		public void Awake()
		{
			this.actor = base.GetComponent<ObiActor>();
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x0007C5AC File Offset: 0x0007A9AC
		public void OnEnable()
		{
			this.material = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>("ObiMaterials/Particle"));
			this.material.hideFlags = HideFlags.HideAndDontSave;
			if (this.actor != null && this.actor.Solver != null)
			{
				this.particlesPerDrawcall = 16250;
				this.drawcallCount = this.actor.positions.Length / this.particlesPerDrawcall + 1;
				this.particlesPerDrawcall = Mathf.Min(this.particlesPerDrawcall, this.actor.positions.Length);
				this.actor.Solver.RequireRenderablePositions();
				this.actor.Solver.OnFrameEnd += this.Actor_solver_OnFrameEnd;
			}
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x0007C674 File Offset: 0x0007AA74
		public void OnDisable()
		{
			if (this.actor != null && this.actor.Solver != null)
			{
				this.actor.Solver.RelinquishRenderablePositions();
				this.actor.Solver.OnFrameEnd -= this.Actor_solver_OnFrameEnd;
			}
			this.ClearMeshes();
			UnityEngine.Object.DestroyImmediate(this.material);
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x0007C6E8 File Offset: 0x0007AAE8
		private void Actor_solver_OnFrameEnd(object sender, EventArgs e)
		{
			if (this.actor == null || !this.actor.InSolver || !this.actor.isActiveAndEnabled)
			{
				this.ClearMeshes();
				return;
			}
			ObiSolver solver = this.actor.Solver;
			if (this.drawcallCount != this.meshes.Count)
			{
				this.ClearMeshes();
				for (int i = 0; i < this.drawcallCount; i++)
				{
					Mesh mesh = new Mesh();
					mesh.name = "Particle imposters";
					mesh.hideFlags = HideFlags.HideAndDontSave;
					mesh.MarkDynamic();
					this.meshes.Add(mesh);
				}
			}
			for (int j = 0; j < this.drawcallCount; j++)
			{
				this.vertices.Clear();
				this.normals.Clear();
				this.colors.Clear();
				this.triangles.Clear();
				Color color = Color.white;
				int num = 0;
				for (int k = j * this.particlesPerDrawcall; k < (j + 1) * this.particlesPerDrawcall; k++)
				{
					if (this.actor.active[k])
					{
						if (this.actor.colors != null && k < this.actor.colors.Length)
						{
							color = this.actor.colors[k] * this.particleColor;
						}
						else
						{
							color = this.particleColor;
						}
						this.AddParticle(num, solver.renderablePositions[this.actor.particleIndices[k]], color, this.actor.solidRadii[k] * this.radiusScale);
						num++;
					}
				}
				this.Apply(this.meshes[j]);
			}
			if (this.render)
			{
				this.DrawParticles();
			}
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0007C8E0 File Offset: 0x0007ACE0
		private void DrawParticles()
		{
			foreach (Mesh mesh in this.meshes)
			{
				Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, this.material, base.gameObject.layer);
			}
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x0007C958 File Offset: 0x0007AD58
		private void Apply(Mesh mesh)
		{
			mesh.Clear();
			mesh.SetVertices(this.vertices);
			mesh.SetNormals(this.normals);
			mesh.SetColors(this.colors);
			mesh.SetTriangles(this.triangles, 0, true);
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x0007C994 File Offset: 0x0007AD94
		private void ClearMeshes()
		{
			foreach (Mesh obj in this.meshes)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			this.meshes.Clear();
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x0007C9FC File Offset: 0x0007ADFC
		private void AddParticle(int i, Vector3 position, Color color, float radius)
		{
			int num = i * 4;
			int item = num + 1;
			int item2 = num + 2;
			int item3 = num + 3;
			this.vertices.Add(position);
			this.vertices.Add(position);
			this.vertices.Add(position);
			this.vertices.Add(position);
			this.particleOffsets[0].z = radius;
			this.particleOffsets[1].z = radius;
			this.particleOffsets[2].z = radius;
			this.particleOffsets[3].z = radius;
			this.normals.Add(this.particleOffsets[0]);
			this.normals.Add(this.particleOffsets[1]);
			this.normals.Add(this.particleOffsets[2]);
			this.normals.Add(this.particleOffsets[3]);
			this.colors.Add(color);
			this.colors.Add(color);
			this.colors.Add(color);
			this.colors.Add(color);
			this.triangles.Add(item2);
			this.triangles.Add(item);
			this.triangles.Add(num);
			this.triangles.Add(item3);
			this.triangles.Add(item2);
			this.triangles.Add(num);
		}

		// Token: 0x0400122E RID: 4654
		public bool render = true;

		// Token: 0x0400122F RID: 4655
		public Color particleColor = Color.white;

		// Token: 0x04001230 RID: 4656
		public float radiusScale = 1f;

		// Token: 0x04001231 RID: 4657
		private ObiActor actor;

		// Token: 0x04001232 RID: 4658
		private List<Mesh> meshes = new List<Mesh>();

		// Token: 0x04001233 RID: 4659
		private Material material;

		// Token: 0x04001234 RID: 4660
		private List<Vector3> vertices = new List<Vector3>();

		// Token: 0x04001235 RID: 4661
		private List<Vector3> normals = new List<Vector3>();

		// Token: 0x04001236 RID: 4662
		private List<Color> colors = new List<Color>();

		// Token: 0x04001237 RID: 4663
		private List<int> triangles = new List<int>();

		// Token: 0x04001238 RID: 4664
		private int particlesPerDrawcall;

		// Token: 0x04001239 RID: 4665
		private int drawcallCount;

		// Token: 0x0400123A RID: 4666
		private Vector3[] particleOffsets = new Vector3[]
		{
			new Vector3(1f, 1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(-1f, -1f, 0f),
			new Vector3(1f, -1f, 0f)
		};
	}
}
