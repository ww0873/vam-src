using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
[AddComponentMenu("AQUAS/Buoyancy")]
[RequireComponent(typeof(Rigidbody))]
public class AQUAS_Buoyancy : MonoBehaviour
{
	// Token: 0x060000A0 RID: 160 RVA: 0x000050AD File Offset: 0x000034AD
	public AQUAS_Buoyancy()
	{
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x000050CC File Offset: 0x000034CC
	private void Start()
	{
		this.mesh = base.GetComponent<MeshFilter>().mesh;
		this.vertices = this.mesh.vertices;
		this.triangles = this.mesh.triangles;
		this.rb = base.GetComponent<Rigidbody>();
		this.regWaterDensity = this.waterDensity;
		this.maxWaterDensity = this.regWaterDensity + this.regWaterDensity * 0.5f * this.dynamicSurface;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00005144 File Offset: 0x00003544
	private void FixedUpdate()
	{
		if (this.balanceFactor.x < 0.001f)
		{
			this.balanceFactor.x = 0.001f;
		}
		if (this.balanceFactor.y < 0.001f)
		{
			this.balanceFactor.y = 0.001f;
		}
		if (this.balanceFactor.z < 0.001f)
		{
			this.balanceFactor.z = 0.001f;
		}
		this.AddForce();
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x000051C8 File Offset: 0x000035C8
	private void Update()
	{
		this.regWaterDensity = this.waterDensity;
		this.maxWaterDensity = this.regWaterDensity + this.regWaterDensity * 0.5f * this.dynamicSurface;
		this.effWaterDensity = (this.maxWaterDensity - this.regWaterDensity) / 2f + this.regWaterDensity + Mathf.Sin(Time.time * this.bounceFrequency) * (this.maxWaterDensity - this.regWaterDensity) / 2f;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00005248 File Offset: 0x00003648
	private void AddForce()
	{
		for (int i = 0; i < this.triangles.Length; i += 3)
		{
			Vector3 vector = this.vertices[this.triangles[i]];
			Vector3 vector2 = this.vertices[this.triangles[i + 1]];
			Vector3 vector3 = this.vertices[this.triangles[i + 2]];
			float num = this.waterLevel - this.Center(vector, vector2, vector3).y;
			if (num > 0f && this.Center(vector, vector2, vector3).y > (this.Center(vector, vector2, vector3) + this.Normal(vector, vector2, vector3)).y)
			{
				float y = this.effWaterDensity * Physics.gravity.y * num * this.Area(vector, vector2, vector3) * this.Normal(vector, vector2, vector3).normalized.y;
				if (this.useBalanceFactor)
				{
					this.rb.AddForceAtPosition(new Vector3(0f, y, 0f), base.transform.TransformPoint(new Vector3(base.transform.InverseTransformPoint(this.Center(vector, vector2, vector3)).x / (this.balanceFactor.x * base.transform.localScale.x * 1000f), base.transform.InverseTransformPoint(this.Center(vector, vector2, vector3)).y / (this.balanceFactor.y * base.transform.localScale.x * 1000f), base.transform.InverseTransformPoint(this.Center(vector, vector2, vector3)).z / (this.balanceFactor.z * base.transform.localScale.x * 1000f))));
				}
				else
				{
					this.rb.AddForceAtPosition(new Vector3(0f, y, 0f), base.transform.TransformPoint(new Vector3(base.transform.InverseTransformPoint(this.Center(vector, vector2, vector3)).x, base.transform.InverseTransformPoint(this.Center(vector, vector2, vector3)).y, base.transform.InverseTransformPoint(this.Center(vector, vector2, vector3)).z)));
				}
				if (this.debug == AQUAS_Buoyancy.debugModes.showAffectedFaces)
				{
					Debug.DrawLine(this.Center(vector, vector2, vector3), this.Center(vector, vector2, vector3) + this.Normal(vector, vector2, vector3), Color.white);
				}
				if (this.debug == AQUAS_Buoyancy.debugModes.showForceRepresentation)
				{
					Debug.DrawRay(this.Center(vector, vector2, vector3), new Vector3(0f, y, 0f), Color.red);
				}
				if (this.debug == AQUAS_Buoyancy.debugModes.showReferenceVolume)
				{
					Debug.DrawLine(new Vector3(base.transform.TransformPoint(vector).x, this.Center(vector, vector2, vector3).y, base.transform.TransformPoint(vector).z), new Vector3(base.transform.TransformPoint(vector2).x, this.Center(vector, vector2, vector3).y, base.transform.TransformPoint(vector2).z), Color.green);
					Debug.DrawLine(new Vector3(base.transform.TransformPoint(vector2).x, this.Center(vector, vector2, vector3).y, base.transform.TransformPoint(vector2).z), new Vector3(base.transform.TransformPoint(vector3).x, this.Center(vector, vector2, vector3).y, base.transform.TransformPoint(vector3).z), Color.green);
					Debug.DrawLine(new Vector3(base.transform.TransformPoint(vector3).x, this.Center(vector, vector2, vector3).y, base.transform.TransformPoint(vector3).z), new Vector3(base.transform.TransformPoint(vector).x, this.Center(vector, vector2, vector3).y, base.transform.TransformPoint(vector).z), Color.green);
					Debug.DrawLine(new Vector3(base.transform.TransformPoint(vector).x, this.waterLevel, base.transform.TransformPoint(vector).z), new Vector3(base.transform.TransformPoint(vector2).x, this.waterLevel, base.transform.TransformPoint(vector2).z), Color.green);
					Debug.DrawLine(new Vector3(base.transform.TransformPoint(vector2).x, this.waterLevel, base.transform.TransformPoint(vector2).z), new Vector3(base.transform.TransformPoint(vector3).x, this.waterLevel, base.transform.TransformPoint(vector3).z), Color.green);
					Debug.DrawLine(new Vector3(base.transform.TransformPoint(vector3).x, this.waterLevel, base.transform.TransformPoint(vector3).z), new Vector3(base.transform.TransformPoint(vector).x, this.waterLevel, base.transform.TransformPoint(vector).z), Color.green);
					Debug.DrawLine(new Vector3(base.transform.TransformPoint(vector).x, this.waterLevel, base.transform.TransformPoint(vector).z), new Vector3(base.transform.TransformPoint(vector).x, this.Center(vector, vector2, vector3).y, base.transform.TransformPoint(vector).z), Color.green);
					Debug.DrawLine(new Vector3(base.transform.TransformPoint(vector2).x, this.waterLevel, base.transform.TransformPoint(vector2).z), new Vector3(base.transform.TransformPoint(vector2).x, this.Center(vector, vector2, vector3).y, base.transform.TransformPoint(vector2).z), Color.green);
					Debug.DrawLine(new Vector3(base.transform.TransformPoint(vector3).x, this.waterLevel, base.transform.TransformPoint(vector3).z), new Vector3(base.transform.TransformPoint(vector3).x, this.Center(vector, vector2, vector3).y, base.transform.TransformPoint(vector3).z), Color.green);
				}
			}
		}
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x000059D8 File Offset: 0x00003DD8
	private Vector3 Center(Vector3 p1, Vector3 p2, Vector3 p3)
	{
		Vector3 position = (p1 + p2 + p3) / 3f;
		return base.transform.TransformPoint(position);
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00005A0C File Offset: 0x00003E0C
	private Vector3 Normal(Vector3 p1, Vector3 p2, Vector3 p3)
	{
		return Vector3.Cross(base.transform.TransformPoint(p2) - base.transform.TransformPoint(p1), base.transform.TransformPoint(p3) - base.transform.TransformPoint(p1)).normalized;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00005A64 File Offset: 0x00003E64
	private float Area(Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float num = Vector3.Distance(new Vector3(base.transform.TransformPoint(p1).x, this.Center(p1, p2, p3).y, base.transform.TransformPoint(p1).z), new Vector3(base.transform.TransformPoint(p2).x, this.Center(p1, p2, p3).y, base.transform.TransformPoint(p2).z));
		float num2 = Vector3.Distance(new Vector3(base.transform.TransformPoint(p3).x, this.Center(p1, p2, p3).y, base.transform.TransformPoint(p3).z), new Vector3(base.transform.TransformPoint(p1).x, this.Center(p1, p2, p3).y, base.transform.TransformPoint(p1).z));
		return num * num2 * Mathf.Sin(Vector3.Angle(new Vector3(base.transform.TransformPoint(p2).x, this.Center(p1, p2, p3).y, base.transform.TransformPoint(p2).z) - new Vector3(base.transform.TransformPoint(p1).x, this.Center(p1, p2, p3).y, base.transform.TransformPoint(p1).z), new Vector3(base.transform.TransformPoint(p3).x, this.Center(p1, p2, p3).y, base.transform.TransformPoint(p3).z) - new Vector3(base.transform.TransformPoint(p1).x, this.Center(p1, p2, p3).y, base.transform.TransformPoint(p1).z)) * 0.017453292f) / 2f;
	}

	// Token: 0x04000090 RID: 144
	public float waterLevel;

	// Token: 0x04000091 RID: 145
	public float waterDensity;

	// Token: 0x04000092 RID: 146
	[Space(5f)]
	public bool useBalanceFactor;

	// Token: 0x04000093 RID: 147
	public Vector3 balanceFactor;

	// Token: 0x04000094 RID: 148
	[Space(20f)]
	[Range(0f, 1f)]
	public float dynamicSurface = 0.3f;

	// Token: 0x04000095 RID: 149
	[Range(1f, 10f)]
	public float bounceFrequency = 3f;

	// Token: 0x04000096 RID: 150
	[Space(5f)]
	[Header("Debugging can be ver performance heavy!")]
	public AQUAS_Buoyancy.debugModes debug;

	// Token: 0x04000097 RID: 151
	private Vector3[] vertices;

	// Token: 0x04000098 RID: 152
	private int[] triangles;

	// Token: 0x04000099 RID: 153
	private Mesh mesh;

	// Token: 0x0400009A RID: 154
	private Rigidbody rb;

	// Token: 0x0400009B RID: 155
	private float effWaterDensity;

	// Token: 0x0400009C RID: 156
	private float regWaterDensity;

	// Token: 0x0400009D RID: 157
	private float maxWaterDensity;

	// Token: 0x02000012 RID: 18
	public enum debugModes
	{
		// Token: 0x0400009F RID: 159
		none,
		// Token: 0x040000A0 RID: 160
		showAffectedFaces,
		// Token: 0x040000A1 RID: 161
		showForceRepresentation,
		// Token: 0x040000A2 RID: 162
		showReferenceVolume
	}
}
