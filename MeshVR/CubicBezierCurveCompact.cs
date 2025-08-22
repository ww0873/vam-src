using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000C8A RID: 3210
	public class CubicBezierCurveCompact
	{
		// Token: 0x060060D7 RID: 24791 RVA: 0x002494D4 File Offset: 0x002478D4
		public CubicBezierCurveCompact()
		{
		}

		// Token: 0x060060D8 RID: 24792 RVA: 0x002494EC File Offset: 0x002478EC
		public JSONClass GetJSON()
		{
			return new JSONClass();
		}

		// Token: 0x060060D9 RID: 24793 RVA: 0x00249500 File Offset: 0x00247900
		public void RestoreFromJSON(JSONClass jc)
		{
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x060060DA RID: 24794 RVA: 0x00249502 File Offset: 0x00247902
		// (set) Token: 0x060060DB RID: 24795 RVA: 0x0024950A File Offset: 0x0024790A
		public bool draw
		{
			get
			{
				return this._draw;
			}
			set
			{
				if (this._draw != value)
				{
					this._draw = value;
				}
			}
		}

		// Token: 0x060060DC RID: 24796 RVA: 0x00249520 File Offset: 0x00247920
		public void SetDrawColor(Color c)
		{
			if (this._drawColor.r != c.r || this._drawColor.g != c.g || this._drawColor.b != c.b)
			{
				this._drawColor.r = c.r;
				this._drawColor.g = c.g;
				this._drawColor.b = c.b;
				this.materialLocal.color = this._drawColor;
			}
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x060060DD RID: 24797 RVA: 0x002495B9 File Offset: 0x002479B9
		// (set) Token: 0x060060DE RID: 24798 RVA: 0x002495C1 File Offset: 0x002479C1
		public List<CubicBezierPointCompact> points
		{
			get
			{
				return this._points;
			}
			set
			{
				this._points = value;
				this.AutoComputeControlPoints();
				this.RegenerateMesh();
			}
		}

		// Token: 0x060060DF RID: 24799 RVA: 0x002495D8 File Offset: 0x002479D8
		public CubicBezierPointCompact CreatePoint()
		{
			return new CubicBezierPointCompact
			{
				parent = this
			};
		}

		// Token: 0x060060E0 RID: 24800 RVA: 0x002495F3 File Offset: 0x002479F3
		public void AddPointAt(int index, CubicBezierPointCompact point, bool updateControlPoints = true)
		{
			if (index < this.points.Count)
			{
				this.points.Insert(index, point);
			}
			if (updateControlPoints)
			{
				this.AutoComputeControlPoints();
			}
		}

		// Token: 0x060060E1 RID: 24801 RVA: 0x0024961F File Offset: 0x00247A1F
		public void AddPoint(CubicBezierPointCompact point, bool updateControlPoints = true)
		{
			this.points.Add(point);
			if (updateControlPoints)
			{
				this.AutoComputeControlPoints();
			}
		}

		// Token: 0x060060E2 RID: 24802 RVA: 0x00249639 File Offset: 0x00247A39
		public void RemovePointAt(int index, bool updateControlPoints = true)
		{
			this.points.RemoveAt(index);
			if (updateControlPoints)
			{
				this.AutoComputeControlPoints();
			}
		}

		// Token: 0x060060E3 RID: 24803 RVA: 0x00249653 File Offset: 0x00247A53
		protected void SyncLoop()
		{
			this.RegenerateMesh();
		}

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x060060E4 RID: 24804 RVA: 0x0024965B File Offset: 0x00247A5B
		// (set) Token: 0x060060E5 RID: 24805 RVA: 0x00249663 File Offset: 0x00247A63
		public bool loop
		{
			get
			{
				return this._loop;
			}
			set
			{
				if (this._loop != value)
				{
					this._loop = value;
					this.SyncLoop();
				}
			}
		}

		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x060060E6 RID: 24806 RVA: 0x0024967E File Offset: 0x00247A7E
		// (set) Token: 0x060060E7 RID: 24807 RVA: 0x00249686 File Offset: 0x00247A86
		public int curveSmooth
		{
			get
			{
				return this._curveSmooth;
			}
			set
			{
				if (this._curveSmooth != value)
				{
					this._curveSmooth = value;
					this.RegenerateMesh();
				}
			}
		}

		// Token: 0x060060E8 RID: 24808 RVA: 0x002496A4 File Offset: 0x00247AA4
		protected void AutoComputeControlPoints()
		{
			if (this._points != null && this._points.Count != 0)
			{
				int count = this._points.Count;
				if (count == 1)
				{
					this._points[0].controlPointIn = this._points[0].position;
					this._points[0].controlPointOut = this._points[0].position;
				}
				else if (count == 2 && !this._loop)
				{
					this._points[0].controlPointIn = this._points[0].position;
					this._points[0].controlPointOut = this._points[0].position;
					this._points[1].controlPointIn = this._points[1].position;
					this._points[1].controlPointOut = this._points[1].position;
				}
				else
				{
					int num;
					if (this._loop)
					{
						num = count + 1;
					}
					else
					{
						num = count - 1;
					}
					if (this.K == null || this.K.Length < num + 1)
					{
						this.K = new Vector3[num + 1];
					}
					if (this._loop)
					{
						this.K[0] = this._points[count - 1].position;
						for (int i = 1; i < num; i++)
						{
							this.K[i] = this._points[i - 1].position;
						}
						this.K[num] = this._points[0].position;
					}
					else
					{
						for (int j = 0; j < count; j++)
						{
							this.K[j] = this._points[j].position;
						}
					}
					if (this.a == null || this.a.Length < num)
					{
						this.a = new float[num];
					}
					if (this.b == null || this.b.Length < num)
					{
						this.b = new float[num];
					}
					if (this.c == null || this.c.Length < num)
					{
						this.c = new float[num];
					}
					if (this.r == null || this.r.Length < num)
					{
						this.r = new Vector3[num];
					}
					this.a[0] = 0f;
					this.b[0] = 2f;
					this.c[0] = 1f;
					this.r[0] = this.K[0] + 2f * this.K[1];
					for (int k = 1; k < num - 1; k++)
					{
						this.a[k] = 1f;
						this.b[k] = 4f;
						this.c[k] = 1f;
						this.r[k] = 4f * this.K[k] + 2f * this.K[k + 1];
					}
					this.a[num - 1] = 2f;
					this.b[num - 1] = 7f;
					this.c[num - 1] = 0f;
					this.r[num - 1] = 8f * this.K[num - 1] + this.K[num];
					for (int l = 1; l < num; l++)
					{
						float num2 = this.a[l] / this.b[l - 1];
						this.b[l] = this.b[l] - num2 * this.c[l - 1];
						this.r[l] = this.r[l] - num2 * this.r[l - 1];
					}
					if (this._loop)
					{
						Vector3 vector = this.r[num - 1] / this.b[num - 1];
						this._points[num - 2].controlPointOut = (this.r[num - 1] - this.c[num - 1] * vector) / this.b[num - 1];
						for (int m = num - 3; m >= 0; m--)
						{
							this._points[m].controlPointOut = (this.r[m + 1] - this.c[m + 1] * this._points[m + 1].controlPointOut) / this.b[m + 1];
						}
					}
					else
					{
						this._points[num].controlPointOut = this._points[num].position;
						this._points[num - 1].controlPointOut = this.r[num - 1] / this.b[num - 1];
						for (int n = num - 2; n >= 0; n--)
						{
							this._points[n].controlPointOut = (this.r[n] - this.c[n] * this._points[n + 1].controlPointOut) / this.b[n];
						}
					}
					if (this._loop)
					{
						for (int num3 = 0; num3 < num - 1; num3++)
						{
							this._points[num3].controlPointIn = 2f * this.K[num3 + 1] - this._points[num3].controlPointOut;
						}
					}
					else
					{
						this._points[0].controlPointIn = this._points[0].position;
						for (int num4 = 1; num4 < num; num4++)
						{
							this._points[num4].controlPointIn = 2f * this.K[num4] - this._points[num4].controlPointOut;
						}
						this._points[num].controlPointIn = 0.5f * (this.K[num] + this._points[num - 1].controlPointOut);
					}
				}
			}
		}

		// Token: 0x060060E9 RID: 24809 RVA: 0x00249E4C File Offset: 0x0024824C
		public void RegenerateMesh()
		{
			if (this._points != null && this._points.Count > 1)
			{
				int num = this._points.Count - 1;
				if (this._loop)
				{
					num++;
				}
				this.mesh = new Mesh();
				this.indices = new int[num * this._curveSmooth];
				this.vertices = new Vector3[num * this._curveSmooth];
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < this._curveSmooth; j++)
					{
						this.indices[num2] = num2;
						num2++;
					}
				}
				this.mesh.vertices = this.vertices;
				this.mesh.SetIndices(this.indices, MeshTopology.LineStrip, 0);
			}
			else
			{
				this.mesh = new Mesh();
			}
		}

		// Token: 0x060060EA RID: 24810 RVA: 0x00249F30 File Offset: 0x00248330
		protected void UpdateMesh()
		{
			if (this.mesh != null && this._points.Count > 1)
			{
				int num = this._points.Count - 1;
				if (this._loop)
				{
					num++;
				}
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					Vector3 position = this._points[i].position;
					Vector3 controlPointOut = this._points[i].controlPointOut;
					Vector3 controlPointIn;
					Vector3 position2;
					if (this._loop && i == this._points.Count - 1)
					{
						controlPointIn = this._points[0].controlPointIn;
						position2 = this._points[0].position;
					}
					else
					{
						controlPointIn = this._points[i + 1].controlPointIn;
						position2 = this._points[i + 1].position;
					}
					for (int j = 0; j < this._curveSmooth; j++)
					{
						float num3 = (float)j * 1f / (float)(this._curveSmooth - 1);
						float num4 = 1f - num3;
						float num5 = num4 * num4;
						float d = num5 * num4;
						float num6 = num3 * num3;
						float d2 = num6 * num3;
						Vector3 position3 = position * d + 3f * controlPointOut * num5 * num3 + 3f * controlPointIn * num4 * num6 + position2 * d2;
						this.vertices[num2] = this.transform.TransformPoint(position3);
						num2++;
					}
				}
				this.mesh.vertices = this.vertices;
				this.mesh.RecalculateBounds();
			}
		}

		// Token: 0x060060EB RID: 24811 RVA: 0x0024A114 File Offset: 0x00248514
		protected void DrawMesh(GameObject gameObject)
		{
			if (this.mesh != null && this.material != null && this._draw)
			{
				this.UpdateMesh();
				Matrix4x4 identity = Matrix4x4.identity;
				Graphics.DrawMesh(this.mesh, identity, this.materialLocal, gameObject.layer, null, 0, null, false, false);
			}
		}

		// Token: 0x060060EC RID: 24812 RVA: 0x0024A178 File Offset: 0x00248578
		public Vector3 GetPositionFromPoint(int fromPoint, float t)
		{
			Vector3 position = this._points[fromPoint].position;
			Vector3 controlPointOut = this._points[fromPoint].controlPointOut;
			Vector3 position2;
			if (this._points.Count == 1)
			{
				position2 = position;
			}
			else
			{
				Vector3 controlPointIn;
				Vector3 position3;
				if (fromPoint == this._points.Count - 1)
				{
					if (!this._loop)
					{
						return position;
					}
					controlPointIn = this._points[0].controlPointIn;
					position3 = this._points[0].position;
				}
				else
				{
					controlPointIn = this._points[fromPoint + 1].controlPointIn;
					position3 = this._points[fromPoint + 1].position;
				}
				float num = 1f - t;
				float num2 = num * num;
				float d = num2 * num;
				float num3 = t * t;
				float d2 = num3 * t;
				position2 = position * d + 3f * controlPointOut * num2 * t + 3f * controlPointIn * num * num3 + position3 * d2;
			}
			return this.transform.TransformPoint(position2);
		}

		// Token: 0x060060ED RID: 24813 RVA: 0x0024A2BC File Offset: 0x002486BC
		public Quaternion GetRotationFromPoint(int fromPoint, float t)
		{
			Quaternion rotation = this._points[fromPoint].rotation;
			Quaternion rhs;
			if (this._points.Count == 1)
			{
				rhs = rotation;
			}
			else
			{
				Quaternion rotation2;
				if (fromPoint == this._points.Count - 1)
				{
					if (!this._loop)
					{
						return rotation;
					}
					rotation2 = this._points[0].rotation;
				}
				else
				{
					rotation2 = this._points[fromPoint + 1].rotation;
				}
				rhs = Quaternion.Lerp(rotation, rotation2, t);
			}
			return this.transform.rotation * rhs;
		}

		// Token: 0x060060EE RID: 24814 RVA: 0x0024A35D File Offset: 0x0024875D
		protected virtual void Init()
		{
			if (this.material != null)
			{
				this.materialLocal = UnityEngine.Object.Instantiate<Material>(this.material);
				this._drawColor = this.materialLocal.color;
			}
		}

		// Token: 0x060060EF RID: 24815 RVA: 0x0024A392 File Offset: 0x00248792
		protected void Update(GameObject gameObject)
		{
			if (this.mesh == null)
			{
				this.RegenerateMesh();
			}
			this.DrawMesh(gameObject);
		}

		// Token: 0x0400506C RID: 20588
		public Transform transform;

		// Token: 0x0400506D RID: 20589
		protected bool _draw;

		// Token: 0x0400506E RID: 20590
		protected Color _drawColor;

		// Token: 0x0400506F RID: 20591
		protected List<CubicBezierPointCompact> _points;

		// Token: 0x04005070 RID: 20592
		protected bool _loop = true;

		// Token: 0x04005071 RID: 20593
		protected int _curveSmooth = 10;

		// Token: 0x04005072 RID: 20594
		public Material material;

		// Token: 0x04005073 RID: 20595
		protected Material materialLocal;

		// Token: 0x04005074 RID: 20596
		protected int[] indices;

		// Token: 0x04005075 RID: 20597
		protected Vector3[] vertices;

		// Token: 0x04005076 RID: 20598
		protected Mesh mesh;

		// Token: 0x04005077 RID: 20599
		protected Vector3[] K;

		// Token: 0x04005078 RID: 20600
		protected Vector3[] r;

		// Token: 0x04005079 RID: 20601
		protected float[] a;

		// Token: 0x0400507A RID: 20602
		protected float[] b;

		// Token: 0x0400507B RID: 20603
		protected float[] c;
	}
}
