using System;
using UnityEngine;

// Token: 0x02000C92 RID: 3218
public class HairStripV2
{
	// Token: 0x06006103 RID: 24835 RVA: 0x0024ADFC File Offset: 0x002491FC
	public HairStripV2()
	{
	}

	// Token: 0x06006104 RID: 24836 RVA: 0x0024AE0C File Offset: 0x0024920C
	private void SetLineSegmentPointRoot(int index1, Vector3 p2)
	{
		this.hmverts[index1] = p2;
		this.hmnormals[index1] = this.anchorToRoot;
		if (this.globalSettings.createTangents)
		{
			this.hmtangents[index1] = this.anchorTangent;
		}
		if (this.debug)
		{
			Debug.DrawLine(p2, p2 + this.anchorToRoot * this.globalSettings.debugWidth, Color.cyan);
			Debug.DrawLine(p2, p2 + this.anchorTangent * this.globalSettings.debugWidth, Color.white);
			Debug.DrawLine(p2, p2 + this.anchorTangent2 * this.globalSettings.debugWidth, Color.yellow);
		}
	}

	// Token: 0x06006105 RID: 24837 RVA: 0x0024AEF0 File Offset: 0x002492F0
	private void SetLineSegmentPoint(int index1, Vector3 p1, Vector3 p2)
	{
		Vector3 vector;
		vector.x = p2.x - p1.x;
		vector.y = p2.y - p1.y;
		vector.z = p2.z - p1.z;
		float num = 1f / vector.magnitude;
		vector.x *= num;
		vector.y *= num;
		vector.z *= num;
		Vector3 a;
		a.x = vector.y * this.anchorToRoot.z - vector.z * (this.anchorToRoot.y + this.globalSettings.quarterSegmentLength);
		a.y = vector.z * this.anchorToRoot.x - vector.x * this.anchorToRoot.z;
		a.z = vector.x * (this.anchorToRoot.y + this.globalSettings.quarterSegmentLength) - vector.y * this.anchorToRoot.x;
		num = 1f / a.magnitude;
		a.x *= num;
		a.y *= num;
		a.z *= num;
		if (a.sqrMagnitude <= 0.5f)
		{
			a.x = vector.y * this.anchorToRoot.z - vector.z * (this.anchorToRoot.y + this.globalSettings.quarterSegmentLength * 2f);
			a.y = vector.z * this.anchorToRoot.x - vector.x * this.anchorToRoot.z;
			a.z = vector.x * (this.anchorToRoot.y + this.globalSettings.quarterSegmentLength * 2f) - vector.y * this.anchorToRoot.x;
			num = 1f / a.magnitude;
			a.x *= num;
			a.y *= num;
			a.z *= num;
			if (a.sqrMagnitude <= 0.5f)
			{
				a.x = vector.y * this.anchorToRoot.z - vector.z * this.anchorToRoot.y;
				a.y = vector.z * (this.anchorToRoot.x + this.globalSettings.quarterSegmentLength) - vector.x * this.anchorToRoot.z;
				a.z = vector.x * this.anchorToRoot.y - vector.y * (this.anchorToRoot.x + this.globalSettings.quarterSegmentLength);
				num = 1f / a.magnitude;
				a.x *= num;
				a.y *= num;
				a.z *= num;
			}
		}
		Vector3 vector2;
		vector2.x = a.y * vector.z - a.z * vector.y;
		vector2.y = a.z * vector.x - a.x * vector.z;
		vector2.z = a.x * vector.y - a.y * vector.x;
		this.hmverts[index1] = p2;
		this.hmnormals[index1] = vector2;
		if (this.debug)
		{
			Debug.DrawLine(p2, p2 + vector2 * this.globalSettings.debugWidth, Color.cyan);
			Debug.DrawLine(p2, p2 + a * this.globalSettings.debugWidth, Color.white);
		}
		if (this.globalSettings.createTangents)
		{
			this.hmtangents[index1].x = a.x;
			this.hmtangents[index1].y = a.y;
			this.hmtangents[index1].z = a.z;
			this.hmtangents[index1].w = 1f;
		}
	}

	// Token: 0x06006106 RID: 24838 RVA: 0x0024B394 File Offset: 0x00249794
	private void SetQuad(int index1, Vector3 p1, Vector3 p2)
	{
		int num = index1 + 1;
		int num2 = num + 1;
		int num3 = num2 + 1;
		Vector3 vector;
		vector.x = this.cameraPosition.x - p1.x;
		vector.y = this.cameraPosition.y - p1.y;
		vector.z = this.cameraPosition.z - p1.z;
		Vector3 vector2;
		vector2.x = this.cameraPosition.x - p2.x;
		vector2.y = this.cameraPosition.y - p2.y;
		vector2.z = this.cameraPosition.z - p2.z;
		Vector3 vector3;
		vector3.x = vector2.x - vector.x;
		vector3.y = vector2.y - vector.y;
		vector3.z = vector2.z - vector.z;
		Vector3 vector4;
		vector4.x = vector2.y * vector.z - vector2.z * vector.y;
		vector4.y = vector2.z * vector.x - vector2.x * vector.z;
		vector4.z = vector2.x * vector.y - vector2.y * vector.x;
		Vector3 vector5;
		vector5.x = vector4.y * vector3.z - vector4.z * vector3.y;
		vector5.y = vector4.z * vector3.x - vector4.x * vector3.z;
		vector5.z = vector4.x * vector3.y - vector4.y * vector3.x;
		float num4 = 1f / vector4.magnitude;
		vector4.x *= num4;
		vector4.y *= num4;
		vector4.z *= num4;
		num4 = 1f / vector5.magnitude;
		vector5.x *= num4;
		vector5.y *= num4;
		vector5.z *= num4;
		Vector3 vector6;
		vector6.x = vector4.x * this.globalSettings.hairHalfWidth;
		vector6.y = vector4.y * this.globalSettings.hairHalfWidth;
		vector6.z = vector4.z * this.globalSettings.hairHalfWidth;
		this.hmverts[index1].x = p1.x + vector6.x;
		this.hmverts[index1].y = p1.y + vector6.y;
		this.hmverts[index1].z = p1.z + vector6.z;
		this.hmverts[num].x = p1.x - vector6.x;
		this.hmverts[num].y = p1.y - vector6.y;
		this.hmverts[num].z = p1.z - vector6.z;
		this.hmverts[num2].x = p2.x + vector6.x;
		this.hmverts[num2].y = p2.y + vector6.y;
		this.hmverts[num2].z = p2.z + vector6.z;
		this.hmverts[num3].x = p2.x - vector6.x;
		this.hmverts[num3].y = p2.y - vector6.y;
		this.hmverts[num3].z = p2.z - vector6.z;
		this.hmnormals[index1] = vector5;
		this.hmnormals[num] = vector5;
		this.hmnormals[num2] = vector5;
		this.hmnormals[num3] = vector5;
		if (this.globalSettings.createTangents)
		{
			this.hmtangents[index1].x = vector4.x;
			this.hmtangents[index1].y = vector4.y;
			this.hmtangents[index1].z = vector4.z;
			this.hmtangents[index1].w = 1f;
			this.hmtangents[num].x = vector4.x;
			this.hmtangents[num].y = vector4.y;
			this.hmtangents[num].z = vector4.z;
			this.hmtangents[num].w = 1f;
			this.hmtangents[num2].x = vector4.x;
			this.hmtangents[num2].y = vector4.y;
			this.hmtangents[num2].z = vector4.z;
			this.hmtangents[num2].w = 1f;
			this.hmtangents[num3].x = vector4.x;
			this.hmtangents[num3].y = vector4.y;
			this.hmtangents[num3].z = vector4.z;
			this.hmtangents[num3].w = 1f;
		}
	}

	// Token: 0x06006107 RID: 24839 RVA: 0x0024B998 File Offset: 0x00249D98
	private void SetSheetLayer(int index1, Vector3 p1, Vector3 p2)
	{
		int num = index1 + 1;
		Vector3 vector;
		vector.x = this.cameraPosition.x - p1.x;
		vector.y = this.cameraPosition.y - p1.y;
		vector.z = this.cameraPosition.z - p1.z;
		Vector3 vector2;
		vector2.x = this.cameraPosition.x - p2.x;
		vector2.y = this.cameraPosition.y - p2.y;
		vector2.z = this.cameraPosition.z - p2.z;
		Vector3 vector3;
		vector3.x = vector2.x - vector.x;
		vector3.y = vector2.y - vector.y;
		vector3.z = vector2.z - vector.z;
		Vector3 vector4;
		vector4.x = vector2.y * vector.z - vector2.z * vector.y;
		vector4.y = vector2.z * vector.x - vector2.x * vector.z;
		vector4.z = vector2.x * vector.y - vector2.y * vector.x;
		float num2 = 1f / vector4.magnitude;
		vector4.x *= num2;
		vector4.y *= num2;
		vector4.z *= num2;
		Vector3 vector5;
		vector5.x = vector4.y * vector3.z - vector4.z * vector3.y;
		vector5.y = vector4.z * vector3.x - vector4.x * vector3.z;
		vector5.z = vector4.x * vector3.y - vector4.y * vector3.x;
		num2 = 1f / vector5.magnitude;
		vector5.x *= num2;
		vector5.y *= num2;
		vector5.z *= num2;
		Vector3 vector6;
		vector6.x = vector4.x * this.globalSettings.hairHalfWidth;
		vector6.y = vector4.y * this.globalSettings.hairHalfWidth;
		vector6.z = vector4.z * this.globalSettings.hairHalfWidth;
		this.hmverts[index1].x = p2.x + vector6.x;
		this.hmverts[index1].y = p2.y + vector6.y;
		this.hmverts[index1].z = p2.z + vector6.z;
		this.hmverts[num].x = p2.x - vector6.x;
		this.hmverts[num].y = p2.y - vector6.y;
		this.hmverts[num].z = p2.z - vector6.z;
		if (this.globalSettings.roundSheetHairs)
		{
			float num3 = this.sqrtOneMinusSheetHairRoundness * vector5.x;
			float num4 = this.sqrtOneMinusSheetHairRoundness * vector5.y;
			float num5 = this.sqrtOneMinusSheetHairRoundness * vector5.z;
			float num6 = this.sqrtSheetHairRoundness * vector4.x;
			float num7 = this.sqrtSheetHairRoundness * vector4.y;
			float num8 = this.sqrtSheetHairRoundness * vector4.z;
			Vector3 vector7;
			vector7.x = num3 + num6;
			vector7.y = num4 + num7;
			vector7.z = num5 + num8;
			Vector3 vector8;
			vector8.x = num3 - num6;
			vector8.y = num4 - num7;
			vector8.z = num5 - num8;
			this.hmnormals[index1] = vector7;
			this.hmnormals[num] = vector8;
		}
		else
		{
			this.hmnormals[index1] = vector5;
			this.hmnormals[num] = vector5;
		}
		if (this.globalSettings.createTangents)
		{
			this.hmtangents[index1].x = vector4.x;
			this.hmtangents[index1].y = vector4.y;
			this.hmtangents[index1].z = vector4.z;
			this.hmtangents[index1].w = 1f;
			this.hmtangents[num].x = vector4.x;
			this.hmtangents[num].y = vector4.y;
			this.hmtangents[num].z = vector4.z;
			this.hmtangents[num].w = 1f;
		}
	}

	// Token: 0x06006108 RID: 24840 RVA: 0x0024BECC File Offset: 0x0024A2CC
	private void SetCylinderLayer(int index1, Vector3 p1, Vector3 p2)
	{
		int num = index1 + 1;
		int num2 = num + 1;
		int num3 = num2 + 1;
		Vector3 vector;
		vector.x = this.cameraPosition.x - p1.x;
		vector.y = this.cameraPosition.y - p1.y;
		vector.z = this.cameraPosition.z - p1.z;
		Vector3 vector2;
		vector2.x = this.cameraPosition.x - p2.x;
		vector2.y = this.cameraPosition.y - p2.y;
		vector2.z = this.cameraPosition.z - p2.z;
		Vector3 vector3;
		vector3.x = vector2.x - vector.x;
		vector3.y = vector2.y - vector.y;
		vector3.z = vector2.z - vector.z;
		Vector3 vector4;
		vector4.x = vector2.y * vector.z - vector2.z * vector.y;
		vector4.y = vector2.z * vector.x - vector2.x * vector.z;
		vector4.z = vector2.x * vector.y - vector2.y * vector.x;
		Vector3 vector5;
		vector5.x = vector4.y * vector3.z - vector4.z * vector3.y;
		vector5.y = vector4.z * vector3.x - vector4.x * vector3.z;
		vector5.z = vector4.x * vector3.y - vector4.y * vector3.x;
		float num4 = 1f / vector3.magnitude;
		vector3.x *= num4;
		vector3.y *= num4;
		vector3.z *= num4;
		num4 = 1f / vector4.magnitude;
		vector4.x *= num4;
		vector4.y *= num4;
		vector4.z *= num4;
		num4 = 1f / vector5.magnitude;
		vector5.x *= num4;
		vector5.y *= num4;
		vector5.z *= num4;
		Vector3 vector6;
		vector6.x = vector4.x * this.globalSettings.hairHalfWidth;
		vector6.y = vector4.y * this.globalSettings.hairHalfWidth;
		vector6.z = vector4.z * this.globalSettings.hairHalfWidth;
		Vector3 vector7;
		vector7.x = vector5.x * this.globalSettings.hairHalfWidth;
		vector7.y = vector5.y * this.globalSettings.hairHalfWidth;
		vector7.z = vector5.z * this.globalSettings.hairHalfWidth;
		this.hmverts[index1].x = p2.x + vector6.x;
		this.hmverts[index1].y = p2.y + vector6.y;
		this.hmverts[index1].z = p2.z + vector6.z;
		this.hmverts[num].x = p2.x + vector7.x;
		this.hmverts[num].y = p2.y + vector7.y;
		this.hmverts[num].z = p2.z + vector7.z;
		this.hmverts[num2].x = p2.x - vector6.x;
		this.hmverts[num2].y = p2.y - vector6.y;
		this.hmverts[num2].z = p2.z - vector6.z;
		this.hmverts[num3].x = p2.x - vector7.x;
		this.hmverts[num3].y = p2.y - vector7.y;
		this.hmverts[num3].z = p2.z - vector7.z;
		this.hmnormals[index1] = vector4;
		this.hmnormals[num] = vector5;
		this.hmnormals[num2].x = -vector4.x;
		this.hmnormals[num2].y = -vector4.y;
		this.hmnormals[num2].z = -vector4.z;
		this.hmnormals[num3].x = -vector5.x;
		this.hmnormals[num3].y = -vector5.y;
		this.hmnormals[num3].z = -vector5.z;
		if (this.globalSettings.createTangents)
		{
			this.hmtangents[index1].x = vector4.x;
			this.hmtangents[index1].y = vector4.y;
			this.hmtangents[index1].z = vector4.z;
			this.hmtangents[index1].w = 1f;
			this.hmtangents[num].x = vector5.x;
			this.hmtangents[num].y = vector5.y;
			this.hmtangents[num].z = vector5.z;
			this.hmtangents[num].w = 1f;
			this.hmtangents[num2].x = vector4.x;
			this.hmtangents[num2].y = vector4.y;
			this.hmtangents[num2].z = vector4.z;
			this.hmtangents[num2].w = 1f;
			this.hmtangents[num3].x = vector5.x;
			this.hmtangents[num3].y = vector5.y;
			this.hmtangents[num3].z = vector5.z;
			this.hmtangents[num3].w = 1f;
		}
	}

	// Token: 0x06006109 RID: 24841 RVA: 0x0024C5C4 File Offset: 0x0024A9C4
	private void SetQuadTriangles(int tindex, int vindex)
	{
		this.hmtriangles[tindex] = vindex;
		this.hmtriangles[tindex + 1] = vindex + 1;
		this.hmtriangles[tindex + 2] = vindex + 2;
		this.hmtriangles[tindex + 3] = vindex + 1;
		this.hmtriangles[tindex + 4] = vindex + 3;
		this.hmtriangles[tindex + 5] = vindex + 2;
	}

	// Token: 0x0600610A RID: 24842 RVA: 0x0024C61C File Offset: 0x0024AA1C
	private void SetSheetLayerTriangles(int tindex, int vindex)
	{
		this.hmtriangles[tindex] = vindex - 2;
		this.hmtriangles[tindex + 1] = vindex - 1;
		this.hmtriangles[tindex + 2] = vindex;
		this.hmtriangles[tindex + 3] = vindex - 1;
		this.hmtriangles[tindex + 4] = vindex + 1;
		this.hmtriangles[tindex + 5] = vindex;
	}

	// Token: 0x0600610B RID: 24843 RVA: 0x0024C674 File Offset: 0x0024AA74
	private void SetCylinderLayerTriangles(int tindex, int vindex)
	{
		this.hmtriangles[tindex] = vindex - 4;
		this.hmtriangles[tindex + 1] = vindex - 3;
		this.hmtriangles[tindex + 2] = vindex + 1;
		this.hmtriangles[tindex + 3] = vindex - 4;
		this.hmtriangles[tindex + 4] = vindex + 1;
		this.hmtriangles[tindex + 5] = vindex;
		this.hmtriangles[tindex + 6] = vindex - 3;
		this.hmtriangles[tindex + 7] = vindex - 2;
		this.hmtriangles[tindex + 8] = vindex + 2;
		this.hmtriangles[tindex + 9] = vindex - 3;
		this.hmtriangles[tindex + 10] = vindex + 2;
		this.hmtriangles[tindex + 11] = vindex + 1;
		this.hmtriangles[tindex + 12] = vindex - 2;
		this.hmtriangles[tindex + 13] = vindex - 1;
		this.hmtriangles[tindex + 14] = vindex + 3;
		this.hmtriangles[tindex + 15] = vindex - 2;
		this.hmtriangles[tindex + 16] = vindex + 3;
		this.hmtriangles[tindex + 17] = vindex + 2;
		this.hmtriangles[tindex + 18] = vindex - 1;
		this.hmtriangles[tindex + 19] = vindex - 4;
		this.hmtriangles[tindex + 20] = vindex;
		this.hmtriangles[tindex + 21] = vindex - 1;
		this.hmtriangles[tindex + 22] = vindex;
		this.hmtriangles[tindex + 23] = vindex + 3;
	}

	// Token: 0x0600610C RID: 24844 RVA: 0x0024C7C0 File Offset: 0x0024ABC0
	private void determineNumVerticesRequired()
	{
		if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Tube)
		{
			this.numVerticesPerHair = 4 * (this.globalSettings.numberSegments + 1);
			this.vertInc = 4;
		}
		else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Sheet)
		{
			this.numVerticesPerHair = 2 * (this.globalSettings.numberSegments + 1);
			this.vertInc = 2;
		}
		else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.LineStrip || this.globalSettings.hairDrawType == HairStripV2.HairDrawType.GPULines)
		{
			this.numVerticesPerHair = this.globalSettings.numberSegments + 1;
			this.vertInc = 1;
		}
		else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Lines)
		{
			this.numVerticesPerHair = this.globalSettings.numberSegments + 1;
			this.vertInc = 1;
		}
		else
		{
			this.numVerticesPerHair = 4 * this.globalSettings.numberSegments;
			this.vertInc = 4;
		}
		this.numVertices = this.numVerticesPerHair * this.numHairs;
	}

	// Token: 0x0600610D RID: 24845 RVA: 0x0024C8D0 File Offset: 0x0024ACD0
	private void determineNumTrianglePointsRequired()
	{
		if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Tube)
		{
			this.numTrianglePointsPerHair = 24 * this.globalSettings.numberSegments;
			this.triInc = 24;
		}
		else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Sheet)
		{
			this.numTrianglePointsPerHair = 6 * this.globalSettings.numberSegments;
			this.triInc = 6;
		}
		else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.LineStrip || this.globalSettings.hairDrawType == HairStripV2.HairDrawType.GPULines)
		{
			this.numTrianglePointsPerHair = this.globalSettings.numberSegments + 1;
			this.triInc = 1;
		}
		else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Lines)
		{
			this.numTrianglePointsPerHair = 2 * this.globalSettings.numberSegments;
			this.triInc = 2;
		}
		else
		{
			this.numTrianglePointsPerHair = 6 * this.globalSettings.numberSegments;
			this.triInc = 6;
		}
		this.numTrianglePoints = this.numTrianglePointsPerHair * this.numHairs;
	}

	// Token: 0x0600610E RID: 24846 RVA: 0x0024C9DC File Offset: 0x0024ADDC
	private void CreateMeshDataFromPoints(int vindex, int tindex)
	{
		int num = vindex;
		int num2 = tindex;
		float num3 = 1f;
		float num4 = -1f / (float)this.globalSettings.numberSegments;
		float x = UnityEngine.Random.Range(0f, 1f);
		if (this.globalSettings.ownMesh)
		{
			this.hmverts = new Vector3[this.numVertices];
			this.hmnormals = new Vector3[this.numVertices];
			if (this.globalSettings.createTangents)
			{
				this.hmtangents = new Vector4[this.numVertices];
			}
			this.hmtriangles = new int[this.numTrianglePoints];
			this.hmuvs = new Vector2[this.numVertices];
		}
		if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Tube)
		{
			if (this.globalSettings.drawFromAnchor)
			{
				this.SetCylinderLayer(vindex, this.rootPoint.position, this.anchor);
			}
			else
			{
				this.SetCylinderLayer(vindex, this.anchor, this.rootPoint.position);
			}
			this.hmuvs[vindex].x = 0f;
			this.hmuvs[vindex].y = num3;
			this.hmuvs[vindex + 1].x = 1f;
			this.hmuvs[vindex + 1].y = num3;
			this.hmuvs[vindex + 2].x = 0f;
			this.hmuvs[vindex + 2].y = num3;
			this.hmuvs[vindex + 3].x = 1f;
			this.hmuvs[vindex + 3].y = num3;
			vindex += this.vertInc;
			num3 += num4;
		}
		else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Sheet)
		{
			if (this.globalSettings.drawFromAnchor)
			{
				this.SetSheetLayer(vindex, this.rootPoint.position, this.anchor);
			}
			else
			{
				this.SetSheetLayer(vindex, this.anchor, this.rootPoint.position);
			}
			this.hmuvs[vindex].x = 0f;
			this.hmuvs[vindex].y = num3;
			this.hmuvs[vindex + 1].x = 1f;
			this.hmuvs[vindex + 1].y = num3;
			vindex += this.vertInc;
			num3 += num4;
		}
		else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.LineStrip || this.globalSettings.hairDrawType == HairStripV2.HairDrawType.GPULines)
		{
			if (this.globalSettings.drawFromAnchor)
			{
				this.SetLineSegmentPointRoot(vindex, this.anchor);
			}
			else
			{
				this.SetLineSegmentPointRoot(vindex, this.rootPoint.position);
			}
			this.hmtriangles[tindex] = vindex;
			this.hmuvs[vindex].x = x;
			this.hmuvs[vindex].y = num3;
			vindex += this.vertInc;
			tindex += this.triInc;
			num3 += num4;
		}
		else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Lines)
		{
			if (this.globalSettings.drawFromAnchor)
			{
				this.SetLineSegmentPointRoot(vindex, this.anchor);
			}
			else
			{
				this.SetLineSegmentPointRoot(vindex, this.rootPoint.position);
			}
			this.hmuvs[vindex].x = 0f;
			this.hmuvs[vindex].y = num3;
			vindex += this.vertInc;
			num3 += num4;
		}
		Vector3 position;
		if (this.globalSettings.drawFromAnchor)
		{
			position = this.anchor;
		}
		else
		{
			position = this.rootPoint.position;
		}
		this.thisPoint = this.rootPoint.next;
		while (this.thisPoint != null)
		{
			if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Tube)
			{
				this.SetCylinderLayer(vindex, position, this.thisPoint.position);
				this.SetCylinderLayerTriangles(tindex, vindex);
				this.hmuvs[vindex].x = 0f;
				this.hmuvs[vindex].y = num3;
				this.hmuvs[vindex + 1].x = 1f;
				this.hmuvs[vindex + 1].y = num3;
				this.hmuvs[vindex + 2].x = 0f;
				this.hmuvs[vindex + 2].y = num3;
				this.hmuvs[vindex + 3].x = 1f;
				this.hmuvs[vindex + 3].y = num3;
			}
			else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Sheet)
			{
				this.SetSheetLayer(vindex, position, this.thisPoint.position);
				this.SetSheetLayerTriangles(tindex, vindex);
				this.hmuvs[vindex].x = 0f;
				this.hmuvs[vindex].y = num3;
				this.hmuvs[vindex + 1].x = 1f;
				this.hmuvs[vindex + 1].y = num3;
			}
			else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.LineStrip)
			{
				this.SetLineSegmentPoint(vindex, position, this.thisPoint.position);
				this.hmtriangles[tindex] = vindex;
				this.hmuvs[vindex].x = x;
				this.hmuvs[vindex].y = num3;
			}
			else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.GPULines)
			{
				this.SetLineSegmentPoint(vindex, position, this.thisPoint.position);
				this.hmtriangles[tindex] = vindex;
				this.hmuvs[vindex].x = x;
				this.hmuvs[vindex].y = num3;
			}
			else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Lines)
			{
				this.SetLineSegmentPoint(vindex, position, this.thisPoint.position);
				this.hmtriangles[tindex] = vindex - 1;
				this.hmtriangles[tindex + 1] = vindex;
				this.hmuvs[vindex].x = 0f;
				this.hmuvs[vindex].y = num3;
			}
			else
			{
				this.SetQuad(vindex, position, this.thisPoint.position);
				this.SetQuadTriangles(tindex, vindex);
				this.hmuvs[vindex].x = 0f;
				this.hmuvs[vindex].y = num3;
				this.hmuvs[vindex + 1].x = 1f;
				this.hmuvs[vindex + 1].y = num3;
				this.hmuvs[vindex + 2].x = 0f;
				this.hmuvs[vindex + 2].y = num3 + num4;
				this.hmuvs[vindex + 3].x = 1f;
				this.hmuvs[vindex + 3].y = num3 + num4;
			}
			position = this.thisPoint.position;
			this.thisPoint = this.thisPoint.next;
			vindex += this.vertInc;
			tindex += this.triInc;
			num3 += num4;
		}
		if (this.numHairs > 1)
		{
			for (int i = 1; i < this.numHairs; i++)
			{
				Vector3 zero = Vector3.zero;
				float num5 = this.subHairXYZOffsets[i].x * this.globalSettings.scale;
				float num6 = this.subHairXYZOffsets[i].y * this.globalSettings.scale;
				float num7 = this.subHairXYZOffsets[i].z * this.globalSettings.scale;
				if (num5 != 0f)
				{
					zero.x += this.anchorTangent.x * num5;
					zero.y += this.anchorTangent.y * num5;
					zero.z += this.anchorTangent.z * num5;
				}
				if (num6 != 0f)
				{
					zero.x += this.anchorTangent2.x * num6;
					zero.y += this.anchorTangent2.y * num6;
					zero.z += this.anchorTangent2.z * num6;
				}
				if (num7 != 0f)
				{
					zero.x += this.anchorToRoot.x * num7;
					zero.y += this.anchorToRoot.y * num7;
					zero.z += this.anchorToRoot.z * num7;
				}
				for (int j = num; j < num + this.numVerticesPerHair; j++)
				{
					this.hmverts[vindex].x = this.hmverts[j].x + zero.x;
					this.hmverts[vindex].y = this.hmverts[j].y + zero.y;
					this.hmverts[vindex].z = this.hmverts[j].z + zero.z;
					this.hmnormals[vindex] = this.hmnormals[j];
					if (this.globalSettings.createTangents)
					{
						this.hmtangents[vindex] = this.hmtangents[j];
					}
					this.hmuvs[vindex] = this.hmuvs[j];
					vindex++;
				}
			}
			for (int k = 1; k < this.numHairs; k++)
			{
				int num8 = this.numVerticesPerHair * k;
				for (int l = num2; l < num2 + this.numTrianglePointsPerHair; l++)
				{
					this.hmtriangles[tindex] = this.hmtriangles[l] + num8;
					tindex++;
				}
			}
		}
	}

	// Token: 0x0600610F RID: 24847 RVA: 0x0024D468 File Offset: 0x0024B868
	private void UpdateMeshDataFromPoints(int vindex)
	{
		int num = vindex;
		if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Tube)
		{
			if (this.enableDraw)
			{
				if (this.globalSettings.drawFromAnchor)
				{
					this.SetCylinderLayer(vindex, this.rootPoint.position, this.anchor);
				}
				else
				{
					this.SetCylinderLayer(vindex, this.anchor, this.rootPoint.position);
				}
			}
			else
			{
				this.hmverts[vindex] = Vector3.zero;
				this.hmverts[vindex + 1] = Vector3.zero;
				this.hmverts[vindex + 2] = Vector3.zero;
				this.hmverts[vindex + 3] = Vector3.zero;
			}
			vindex += 4;
		}
		else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Sheet)
		{
			if (this.enableDraw)
			{
				if (this.globalSettings.drawFromAnchor)
				{
					this.SetSheetLayer(vindex, this.rootPoint.position, this.anchor);
				}
				else
				{
					this.SetSheetLayer(vindex, this.anchor, this.rootPoint.position);
				}
			}
			else
			{
				this.hmverts[vindex] = Vector3.zero;
				this.hmverts[vindex + 1] = Vector3.zero;
			}
			vindex += 2;
		}
		else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.LineStrip)
		{
			if (this.enableDraw)
			{
				if (this.globalSettings.drawFromAnchor)
				{
					this.SetLineSegmentPoint(vindex, this.rootPoint.position, this.anchor);
				}
				else
				{
					this.SetLineSegmentPoint(vindex, this.anchor, this.rootPoint.position);
				}
			}
			else
			{
				this.hmverts[vindex] = Vector3.zero;
			}
			vindex++;
		}
		else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.GPULines)
		{
			if (this.enableDraw)
			{
				if (this.globalSettings.drawFromAnchor)
				{
					this.SetLineSegmentPointRoot(vindex, this.anchor);
				}
				else
				{
					this.SetLineSegmentPointRoot(vindex, this.rootPoint.position);
				}
			}
			else
			{
				this.hmverts[vindex] = Vector3.zero;
			}
			vindex++;
		}
		else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Lines)
		{
			if (this.enableDraw)
			{
				if (this.globalSettings.drawFromAnchor)
				{
					this.SetLineSegmentPoint(vindex, this.rootPoint.position, this.anchor);
				}
				else
				{
					this.SetLineSegmentPoint(vindex, this.anchor, this.rootPoint.position);
				}
			}
			else
			{
				this.hmverts[vindex] = Vector3.zero;
			}
			vindex++;
		}
		Vector3 position;
		if (this.globalSettings.drawFromAnchor)
		{
			position = this.anchor;
		}
		else
		{
			position = this.rootPoint.position;
		}
		this.thisPoint = this.rootPoint.next;
		while (this.thisPoint != null)
		{
			if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Tube)
			{
				if (this.enableDraw)
				{
					this.SetCylinderLayer(vindex, position, this.thisPoint.position);
				}
				else
				{
					this.hmverts[vindex] = Vector3.zero;
					this.hmverts[vindex + 1] = Vector3.zero;
					this.hmverts[vindex + 2] = Vector3.zero;
					this.hmverts[vindex + 3] = Vector3.zero;
				}
				vindex += 4;
			}
			else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Sheet)
			{
				if (this.enableDraw)
				{
					this.SetSheetLayer(vindex, position, this.thisPoint.position);
				}
				else
				{
					this.hmverts[vindex] = Vector3.zero;
					this.hmverts[vindex + 1] = Vector3.zero;
				}
				vindex += 2;
			}
			else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.LineStrip || this.globalSettings.hairDrawType == HairStripV2.HairDrawType.GPULines)
			{
				if (this.enableDraw)
				{
					this.SetLineSegmentPoint(vindex, position, this.thisPoint.position);
				}
				else
				{
					this.hmverts[vindex] = Vector3.zero;
				}
				vindex++;
			}
			else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Lines)
			{
				if (this.enableDraw)
				{
					this.SetLineSegmentPoint(vindex, position, this.thisPoint.position);
				}
				else
				{
					this.hmverts[vindex] = Vector3.zero;
				}
				vindex++;
			}
			else
			{
				if (this.enableDraw)
				{
					this.SetQuad(vindex, position, this.thisPoint.position);
				}
				else
				{
					this.hmverts[vindex] = Vector3.zero;
					this.hmverts[vindex + 1] = Vector3.zero;
					this.hmverts[vindex + 2] = Vector3.zero;
					this.hmverts[vindex + 3] = Vector3.zero;
				}
				vindex += 4;
			}
			position = this.thisPoint.position;
			this.thisPoint = this.thisPoint.next;
		}
		if (this.numHairs > 1)
		{
			for (int i = 1; i < this.numHairs; i++)
			{
				Vector3 zero = Vector3.zero;
				float num2 = this.subHairXYZOffsets[i].x * this.globalSettings.scale;
				float num3 = this.subHairXYZOffsets[i].y * this.globalSettings.scale;
				float num4 = this.subHairXYZOffsets[i].z * this.globalSettings.scale;
				if (num2 != 0f)
				{
					zero.x += this.anchorTangent.x * num2;
					zero.y += this.anchorTangent.y * num2;
					zero.z += this.anchorTangent.z * num2;
				}
				if (num3 != 0f)
				{
					zero.x += this.anchorTangent2.x * num3;
					zero.y += this.anchorTangent2.y * num3;
					zero.z += this.anchorTangent2.z * num3;
				}
				if (num4 != 0f)
				{
					zero.x += this.anchorToRoot.x * num4;
					zero.y += this.anchorToRoot.y * num4;
					zero.z += this.anchorToRoot.z * num4;
				}
				if (this.debug)
				{
					MyDebug.DrawWireCube(this.hmverts[num], this.globalSettings.debugWidth * 0.2f, Color.red);
					MyDebug.DrawWireCube(this.hmverts[num] + zero, this.globalSettings.debugWidth * 0.2f, Color.green);
				}
				for (int j = num; j < num + this.numVerticesPerHair; j++)
				{
					this.hmverts[vindex].x = this.hmverts[j].x + zero.x;
					this.hmverts[vindex].y = this.hmverts[j].y + zero.y;
					this.hmverts[vindex].z = this.hmverts[j].z + zero.z;
					this.hmnormals[vindex] = this.hmnormals[j];
					if (this.globalSettings.createTangents)
					{
						this.hmtangents[vindex] = this.hmtangents[j];
					}
					vindex++;
				}
			}
		}
	}

	// Token: 0x06006110 RID: 24848 RVA: 0x0024DD14 File Offset: 0x0024C114
	private void CreateMesh()
	{
		if (this.globalSettings.ownMesh)
		{
			this.hm = new Mesh();
			this.hm.vertices = this.hmverts;
			this.hm.normals = this.hmnormals;
			if (this.globalSettings.createTangents)
			{
				this.hm.tangents = this.hmtangents;
			}
			this.hm.uv = this.hmuvs;
			if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.LineStrip)
			{
				this.hm.SetIndices(this.hmtriangles, MeshTopology.LineStrip, 0);
			}
			else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.GPULines)
			{
				this.hm.SetIndices(this.hmtriangles, MeshTopology.Quads, 0);
			}
			else if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Lines)
			{
				this.hm.SetIndices(this.hmtriangles, MeshTopology.Lines, 0);
			}
			else
			{
				this.hm.triangles = this.hmtriangles;
			}
			float num = this.globalSettings.hairLength * 2f;
			this.hm.bounds = new Bounds(this.rootPoint.position, new Vector3(num, num, num));
		}
	}

	// Token: 0x06006111 RID: 24849 RVA: 0x0024DE54 File Offset: 0x0024C254
	private void UpdateMesh()
	{
		if (this.globalSettings.ownMesh)
		{
			this.hm.vertices = this.hmverts;
			this.hm.normals = this.hmnormals;
			if (this.globalSettings.createTangents)
			{
				this.hm.tangents = this.hmtangents;
			}
			float num = this.globalSettings.hairLength * 2f;
			this.hm.bounds = new Bounds(this.rootPoint.position, new Vector3(num, num, num));
		}
	}

	// Token: 0x06006112 RID: 24850 RVA: 0x0024DEEC File Offset: 0x0024C2EC
	private void CreatePoints()
	{
		this.anchorPoint = new LinkedPoint(this.anchor);
		this.rootPoint = new LinkedPoint(this.root);
		this.lastPoint = this.rootPoint;
		for (int i = 0; i < this.globalSettings.numberSegments; i++)
		{
			this.thisPoint = new LinkedPoint(this.lastPoint.position + this.anchorToRoot * this.globalSettings.segmentLength);
			this.thisPoint.previous = this.lastPoint;
			this.thisPoint.force = new Vector3(0f, 0f, 0f);
			this.lastPoint.next = this.thisPoint;
			this.lastPoint = this.thisPoint;
		}
	}

	// Token: 0x06006113 RID: 24851 RVA: 0x0024DFC4 File Offset: 0x0024C3C4
	private void MoveThisPointPhysically(float stiffnessGraded)
	{
		this.thisPoint.previous_position = Vector3.Lerp(this.thisPoint.position, this.thisPoint.stiff_position, stiffnessGraded);
		float num = this.globalSettings.deltaTimeSqr * this.globalSettings.scale;
		float num2 = this.globalSettings.velocityFactor * this.globalSettings.deltaTime;
		this.thisPoint.unconstrained_position.x = this.thisPoint.previous_position.x + num2 * this.thisPoint.velocity.x + num * (this.thisPoint.force.x + this.globalSettings.gravityForce.x);
		this.thisPoint.unconstrained_position.y = this.thisPoint.previous_position.y + num2 * this.thisPoint.velocity.y + num * (this.thisPoint.force.y + this.globalSettings.gravityForce.y);
		this.thisPoint.unconstrained_position.z = this.thisPoint.previous_position.z + num2 * this.thisPoint.velocity.z + num * (this.thisPoint.force.z + this.globalSettings.gravityForce.z);
		this.thisPoint.position = this.thisPoint.unconstrained_position;
	}

	// Token: 0x06006114 RID: 24852 RVA: 0x0024E144 File Offset: 0x0024C544
	private void MoveThisPointLegalTo2BackPoint()
	{
		if (this.lastPoint.previous != null)
		{
			LinkedPoint previous = this.lastPoint.previous;
			Vector3 vector;
			vector.x = this.thisPoint.position.x - previous.position.x;
			vector.y = this.thisPoint.position.y - previous.position.y;
			vector.z = this.thisPoint.position.z - previous.position.z;
			float num = this.globalSettings.segmentLength / vector.magnitude;
			if (num > 1f)
			{
				vector.x *= num;
				vector.y *= num;
				vector.z *= num;
				this.thisPoint.position.x = previous.position.x + vector.x;
				this.thisPoint.position.y = previous.position.y + vector.y;
				this.thisPoint.position.z = previous.position.z + vector.z;
			}
		}
	}

	// Token: 0x06006115 RID: 24853 RVA: 0x0024E290 File Offset: 0x0024C690
	private void MoveThisPointLegalToLastPoint()
	{
		Vector3 vector;
		vector.x = this.thisPoint.position.x - this.lastPoint.position.x;
		vector.y = this.thisPoint.position.y - this.lastPoint.position.y;
		vector.z = this.thisPoint.position.z - this.lastPoint.position.z;
		float num = this.globalSettings.segmentLength / vector.magnitude;
		vector.x *= num;
		vector.y *= num;
		vector.z *= num;
		this.thisPoint.position.x = this.lastPoint.position.x + vector.x;
		this.thisPoint.position.y = this.lastPoint.position.y + vector.y;
		this.thisPoint.position.z = this.lastPoint.position.z + vector.z;
	}

	// Token: 0x06006116 RID: 24854 RVA: 0x0024E3D4 File Offset: 0x0024C7D4
	private void MoveThisPointToCapsuleColliderSurface(int index)
	{
		ExtendedCapsuleCollider extendedCapsuleCollider = this.globalSettings.extendedColliders[index];
		float num = extendedCapsuleCollider.endPoint2.x - extendedCapsuleCollider.endPoint1.x;
		float num2 = extendedCapsuleCollider.endPoint2.y - extendedCapsuleCollider.endPoint1.y;
		float num3 = extendedCapsuleCollider.endPoint2.z - extendedCapsuleCollider.endPoint1.z;
		float num4 = this.thisPoint.position.x - extendedCapsuleCollider.endPoint1.x;
		float num5 = this.thisPoint.position.y - extendedCapsuleCollider.endPoint1.y;
		float num6 = this.thisPoint.position.z - extendedCapsuleCollider.endPoint1.z;
		float num7 = num4 * num + num5 * num2 + num6 * num3;
		if (num7 < 0f)
		{
			float num8 = num4 * num4 + num5 * num5 + num6 * num6;
			if (num8 > extendedCapsuleCollider.radiusSquared)
			{
				return;
			}
			float num9 = 1f / Mathf.Sqrt(num8);
			this.thisPoint.position.x = extendedCapsuleCollider.endPoint1.x + num4 * num9 * extendedCapsuleCollider.radius;
			this.thisPoint.position.y = extendedCapsuleCollider.endPoint1.y + num5 * num9 * extendedCapsuleCollider.radius;
			this.thisPoint.position.z = extendedCapsuleCollider.endPoint1.z + num6 * num9 * extendedCapsuleCollider.radius;
			this.thisPoint.collided = true;
			if (this.debug)
			{
				MyDebug.DrawWireCube(this.thisPoint.position, 0.005f, Color.green);
			}
		}
		else if (num7 > extendedCapsuleCollider.lengthSquared)
		{
			num4 = this.thisPoint.position.x - extendedCapsuleCollider.endPoint2.x;
			num5 = this.thisPoint.position.y - extendedCapsuleCollider.endPoint2.y;
			num6 = this.thisPoint.position.z - extendedCapsuleCollider.endPoint2.z;
			float num8 = num4 * num4 + num5 * num5 + num6 * num6;
			if (num8 > extendedCapsuleCollider.radiusSquared)
			{
				return;
			}
			float num10 = 1f / Mathf.Sqrt(num8);
			this.thisPoint.position.x = extendedCapsuleCollider.endPoint2.x + num4 * num10 * extendedCapsuleCollider.radius;
			this.thisPoint.position.y = extendedCapsuleCollider.endPoint2.y + num5 * num10 * extendedCapsuleCollider.radius;
			this.thisPoint.position.z = extendedCapsuleCollider.endPoint2.z + num6 * num10 * extendedCapsuleCollider.radius;
			this.thisPoint.collided = true;
			if (this.debug)
			{
				MyDebug.DrawWireCube(this.thisPoint.position, 0.005f, Color.yellow);
			}
		}
		else
		{
			float num8 = num4 * num4 + num5 * num5 + num6 * num6 - num7 * num7 * extendedCapsuleCollider.oneOverLengthSquared;
			if (num8 > extendedCapsuleCollider.radiusSquared)
			{
				return;
			}
			float num11 = num7 * extendedCapsuleCollider.oneOverLengthSquared;
			float num12 = extendedCapsuleCollider.endPoint1.x + num * num11;
			float num13 = extendedCapsuleCollider.endPoint1.y + num2 * num11;
			float num14 = extendedCapsuleCollider.endPoint1.z + num3 * num11;
			if (this.debug)
			{
				Vector3 position;
				position.x = num12;
				position.y = num13;
				position.z = num14;
				MyDebug.DrawWireCube(position, 0.005f, Color.blue);
			}
			float num15 = this.thisPoint.position.x - num12;
			float num16 = this.thisPoint.position.y - num13;
			float num17 = this.thisPoint.position.z - num14;
			float num18 = 1f / Mathf.Sqrt(num15 * num15 + num16 * num16 + num17 * num17);
			this.thisPoint.collided = true;
			this.thisPoint.position.x = num12 + num15 * num18 * extendedCapsuleCollider.radius;
			this.thisPoint.position.y = num13 + num16 * num18 * extendedCapsuleCollider.radius;
			this.thisPoint.position.z = num14 + num17 * num18 * extendedCapsuleCollider.radius;
			if (this.debug)
			{
				MyDebug.DrawWireCube(this.thisPoint.position, 0.005f, Color.red);
			}
		}
	}

	// Token: 0x06006117 RID: 24855 RVA: 0x0024E864 File Offset: 0x0024CC64
	private void MoveThisPointToColliderSurface(int colliderNum, bool calculateTangent)
	{
		Collider collider = this.globalSettings.colliders[colliderNum];
		float num = 10f;
		Vector3 vector = this.globalSettings.colliderCenters[colliderNum];
		Vector3 direction;
		direction.x = vector.x - this.thisPoint.position.x;
		direction.y = vector.y - this.thisPoint.position.y;
		direction.z = vector.z - this.thisPoint.position.z;
		this.r.origin = this.thisPoint.position;
		this.r.direction = direction;
		RaycastHit raycastHit;
		if (!collider.Raycast(this.r, out raycastHit, num))
		{
			float num2 = num / direction.magnitude;
			Vector3 direction2;
			direction2.x = direction.x * num2;
			direction2.y = direction.y * num2;
			direction2.z = direction.z * num2;
			Vector3 origin;
			origin.x = this.thisPoint.position.x - direction2.x;
			origin.y = this.thisPoint.position.y - direction2.y;
			origin.z = this.thisPoint.position.z - direction2.z;
			this.r.origin = origin;
			this.r.direction = direction2;
			if (collider.Raycast(this.r, out raycastHit, num))
			{
				Vector3 normal = raycastHit.normal;
				this.thisPoint.position = raycastHit.point;
				if (this.debug)
				{
					MyDebug.DrawWireCube(this.thisPoint.position, this.globalSettings.debugWidth, Color.red);
				}
				this.thisPoint.collided = true;
				if (calculateTangent)
				{
					Vector3 vector2;
					vector2.x = direction2.y * normal.z - direction2.z * normal.y;
					vector2.y = direction2.z * normal.x - direction2.x * normal.z;
					vector2.z = direction2.x * normal.y - direction2.y * normal.x;
					num2 = 1f / vector2.magnitude;
					vector2.x *= num2;
					vector2.y *= num2;
					vector2.z *= num2;
					if (vector2.sqrMagnitude <= 0.5f)
					{
						direction2.x += this.globalSettings.quarterSegmentLength;
						vector2.x = direction2.y * normal.z - direction2.z * normal.y;
						vector2.y = direction2.z * normal.x - direction2.x * normal.z;
						vector2.z = direction2.x * normal.y - direction2.y * normal.x;
						num2 = 1f / vector2.magnitude;
						vector2.x *= num2;
						vector2.y *= num2;
						vector2.z *= num2;
						if (vector2.sqrMagnitude <= 0.5f)
						{
							direction2.y += this.globalSettings.quarterSegmentLength;
							vector2.x = direction2.y * normal.z - direction2.z * normal.y;
							vector2.y = direction2.z * normal.x - direction2.x * normal.z;
							vector2.z = direction2.x * normal.y - direction2.y * normal.x;
							num2 = 1f / vector2.magnitude;
							vector2.x *= num2;
							vector2.y *= num2;
							vector2.z *= num2;
						}
					}
				}
			}
		}
	}

	// Token: 0x06006118 RID: 24856 RVA: 0x0024ECBC File Offset: 0x0024D0BC
	private void SimulateRoot()
	{
		this.anchorPoint.previous_position = this.anchorPoint.position;
		this.anchorPoint.position = this.anchor;
		this.rootPoint.previous_position = this.rootPoint.position;
		this.rootPoint.position = this.root;
		this.rootPoint.unconstrained_position = this.root;
		this.rootPoint.position = this.root;
		this.lastPoint = this.anchorPoint;
		this.thisPoint = this.rootPoint;
	}

	// Token: 0x06006119 RID: 24857 RVA: 0x0024ED54 File Offset: 0x0024D154
	private void SimulateThisPoint(int ind)
	{
		this.MoveThisPointPhysically(Mathf.Lerp(this.stiffnessRootVaried, this.stiffnessEndVaried, (float)ind * this.globalSettings.invNumberSegments));
		this.MoveThisPointLegalTo2BackPoint();
		this.MoveThisPointLegalToLastPoint();
		if (this.globalSettings.enableCollision)
		{
			this.thisPoint.had_collided = this.thisPoint.collided;
			this.thisPoint.collided = false;
			if (this.globalSettings.extendedColliders != null && this.globalSettings.useExtendedColliders)
			{
				for (int i = 0; i < this.globalSettings.extendedColliders.Length; i++)
				{
					this.MoveThisPointToCapsuleColliderSurface(i);
				}
			}
			else
			{
				int num = 0;
				foreach (Collider collider in this.globalSettings.colliders)
				{
					if (!(collider is MeshCollider))
					{
						this.MoveThisPointToColliderSurface(num, false);
					}
					num++;
				}
			}
			this.MoveThisPointLegalToLastPoint();
			if (this.globalSettings.staticFriction && this.thisPoint.had_collided && this.thisPoint.collided)
			{
				Vector3 vector;
				vector.x = this.thisPoint.position.x - this.thisPoint.stiff_position.x;
				vector.y = this.thisPoint.position.y - this.thisPoint.stiff_position.y;
				vector.z = this.thisPoint.position.z - this.thisPoint.stiff_position.z;
				if (vector.sqrMagnitude < this.globalSettings.staticMoveDistanceSqr)
				{
					this.thisPoint.position = this.thisPoint.stiff_position;
				}
			}
		}
		this.thisPoint.delta_position.x = this.thisPoint.unconstrained_position.x - this.thisPoint.position.x;
		this.thisPoint.delta_position.y = this.thisPoint.unconstrained_position.y - this.thisPoint.position.y;
		this.thisPoint.delta_position.z = this.thisPoint.unconstrained_position.z - this.thisPoint.position.z;
	}

	// Token: 0x0600611A RID: 24858 RVA: 0x0024EFC0 File Offset: 0x0024D3C0
	private void SimulatePoints()
	{
		if (this.debug)
		{
			MyDebug.DrawWireCube(this.anchor, this.globalSettings.debugWidth, Color.green);
		}
		this.SimulateRoot();
		this.lastPoint = this.rootPoint;
		this.thisPoint = this.rootPoint.next;
		int num = 0;
		while (this.thisPoint != null)
		{
			if (this.globalSettings.enableSimulation)
			{
				this.thisPoint.stiff_position = this.rootChangeMatrix.MultiplyPoint3x4(this.thisPoint.position);
				this.SimulateThisPoint(num);
			}
			else
			{
				this.thisPoint.previous_position = this.thisPoint.position;
				this.thisPoint.position = this.rootChangeMatrix.MultiplyPoint3x4(this.thisPoint.position);
			}
			if (this.debug)
			{
				MyDebug.DrawWireCube(this.thisPoint.position, this.globalSettings.debugWidth, Color.blue);
			}
			this.lastPoint = this.thisPoint;
			this.thisPoint = this.thisPoint.next;
			num++;
		}
		if (this.globalSettings.enableSimulation && this.globalSettings.velocityFactor > 0f)
		{
			this.thisPoint = this.rootPoint.next;
			num = 0;
			while (this.thisPoint != null)
			{
				Vector3 vector;
				vector.x = this.thisPoint.velocity.x;
				vector.y = this.thisPoint.velocity.y;
				vector.z = this.thisPoint.velocity.z;
				Vector3 vector2;
				vector2.x = this.thisPoint.position.x - this.thisPoint.previous_position.x;
				vector2.y = this.thisPoint.position.y - this.thisPoint.previous_position.y;
				vector2.z = this.thisPoint.position.z - this.thisPoint.previous_position.z;
				this.thisPoint.velocity.x = vector2.x * this.globalSettings.invDeltaTime;
				this.thisPoint.velocity.y = vector2.y * this.globalSettings.invDeltaTime;
				this.thisPoint.velocity.z = vector2.z * this.globalSettings.invDeltaTime;
				if (this.thisPoint.next != null)
				{
					LinkedPoint linkedPoint = this.thisPoint;
					linkedPoint.velocity.x = linkedPoint.velocity.x + this.globalSettings.invdtdampen * this.thisPoint.next.delta_position.x;
					LinkedPoint linkedPoint2 = this.thisPoint;
					linkedPoint2.velocity.y = linkedPoint2.velocity.y + this.globalSettings.invdtdampen * this.thisPoint.next.delta_position.y;
					LinkedPoint linkedPoint3 = this.thisPoint;
					linkedPoint3.velocity.z = linkedPoint3.velocity.z + this.globalSettings.invdtdampen * this.thisPoint.next.delta_position.z;
				}
				Vector3 vector3;
				vector3.x = this.thisPoint.velocity.x - vector.x;
				vector3.y = this.thisPoint.velocity.y - vector.y;
				vector3.z = this.thisPoint.velocity.z - vector.z;
				if (this.globalSettings.clampAcceleration)
				{
					vector3.x = Mathf.Clamp(vector3.x, -this.globalSettings.accelerationClamp, this.globalSettings.accelerationClamp);
					vector3.y = Mathf.Clamp(vector3.y, -this.globalSettings.accelerationClamp, this.globalSettings.accelerationClamp);
					vector3.z = Mathf.Clamp(vector3.z, -this.globalSettings.accelerationClamp, this.globalSettings.accelerationClamp);
				}
				this.thisPoint.velocity.x = vector.x + vector3.x;
				this.thisPoint.velocity.y = vector.y + vector3.y;
				this.thisPoint.velocity.z = vector.z + vector3.z;
				if (this.globalSettings.clampVelocity)
				{
					this.thisPoint.velocity.x = Mathf.Clamp(this.thisPoint.velocity.x, -this.globalSettings.velocityClamp, this.globalSettings.velocityClamp);
					this.thisPoint.velocity.y = Mathf.Clamp(this.thisPoint.velocity.y, -this.globalSettings.velocityClamp, this.globalSettings.velocityClamp);
					this.thisPoint.velocity.z = Mathf.Clamp(this.thisPoint.velocity.z, -this.globalSettings.velocityClamp, this.globalSettings.velocityClamp);
				}
				this.thisPoint = this.thisPoint.next;
				num++;
			}
		}
	}

	// Token: 0x0600611B RID: 24859 RVA: 0x0024F530 File Offset: 0x0024D930
	public void SetVarsThreadSafe()
	{
		if (this.lastStiffnessVariance != this.globalSettings.stiffnessVariance)
		{
			this.randomStiffnessVariance = UnityEngine.Random.Range(-this.globalSettings.stiffnessVariance, this.globalSettings.stiffnessVariance);
			this.lastStiffnessVariance = this.globalSettings.stiffnessVariance;
		}
		this.stiffnessRootVaried = Mathf.Clamp01(this.globalSettings.stiffnessRoot + this.randomStiffnessVariance);
		this.stiffnessEndVaried = Mathf.Clamp01(this.globalSettings.stiffnessEnd + this.randomStiffnessVariance);
	}

	// Token: 0x0600611C RID: 24860 RVA: 0x0024F5C0 File Offset: 0x0024D9C0
	public void SetVars()
	{
		if (this.cameraTransform != null)
		{
			this.cameraPosition = this.cameraTransform.position;
		}
		else
		{
			this.cameraPosition = Vector3.zero;
		}
		if (this.globalSettings.hairDrawType == HairStripV2.HairDrawType.Sheet && this.globalSettings.roundSheetHairs)
		{
			this.oneMinusSheetHairRoundness = 1f - this.globalSettings.sheetHairRoundness;
			this.sqrtSheetHairRoundness = Mathf.Sqrt(this.globalSettings.sheetHairRoundness);
			this.sqrtOneMinusSheetHairRoundness = Mathf.Sqrt(this.oneMinusSheetHairRoundness);
		}
		this.SetVarsThreadSafe();
	}

	// Token: 0x0600611D RID: 24861 RVA: 0x0024F664 File Offset: 0x0024DA64
	public void Init()
	{
		if (Camera.main != null)
		{
			this.cameraTransform = Camera.main.transform;
		}
		this.r = default(Ray);
		float f = UnityEngine.Random.Range((float)this.globalSettings.numHairsMin, (float)this.globalSettings.numHairsMax + 0.999f);
		this.numHairs = Mathf.FloorToInt(f);
		this.determineNumVerticesRequired();
		this.determineNumTrianglePointsRequired();
	}

	// Token: 0x0600611E RID: 24862 RVA: 0x0024F6DC File Offset: 0x0024DADC
	public void Start(int vindex, int tindex)
	{
		this.rootMatrix = Matrix4x4.identity;
		this.rootChangeMatrix = Matrix4x4.identity;
		this.SetVars();
		this.CreatePoints();
		this.subHairXYZOffsets = new Vector3[this.numHairs];
		this.subHairXYZOffsets[0] = Vector3.zero;
		for (int i = 1; i < this.numHairs; i++)
		{
			if (this.globalSettings.bundleType == HairStripV2.HairBundleType.Circular)
			{
				float num = UnityEngine.Random.Range(0f, this.globalSettings.subHairXOffsetMax);
				float num2 = UnityEngine.Random.Range(0f, 6.2831855f);
				float x = Mathf.Cos(num2) * num;
				float y = Mathf.Sin(num2) * num;
				float z = this.globalSettings.subHairZOffsetBend * num;
				this.subHairXYZOffsets[i] = new Vector3(x, y, z);
				if (this.debug)
				{
					Debug.Log(string.Concat(new object[]
					{
						"rnd is ",
						num,
						" angle is ",
						num2,
						" offsets are ",
						this.subHairXYZOffsets[i].ToString("F3")
					}));
				}
			}
			else
			{
				float num3 = UnityEngine.Random.Range(-this.globalSettings.subHairXOffsetMax, this.globalSettings.subHairXOffsetMax);
				float num4 = UnityEngine.Random.Range(-this.globalSettings.subHairYOffsetMax, this.globalSettings.subHairYOffsetMax);
				float num5 = Mathf.Sqrt(num3 * num3 + num4 * num4);
				float z2 = this.globalSettings.subHairZOffsetBend * num5;
				this.subHairXYZOffsets[i] = new Vector3(num3, num4, z2);
			}
		}
		this.CreateMeshDataFromPoints(vindex, tindex);
		this.CreateMesh();
		this.SimulatePoints();
		this.UpdateMeshDataFromPoints(vindex);
		this.UpdateMesh();
	}

	// Token: 0x0600611F RID: 24863 RVA: 0x0024F8BC File Offset: 0x0024DCBC
	public void Update(int vindex)
	{
		this.SetVars();
		this.SimulatePoints();
		this.UpdateMeshDataFromPoints(vindex);
		if (this.globalSettings.ownMesh && this.enableDraw)
		{
			this.UpdateMesh();
			Matrix4x4 identity = Matrix4x4.identity;
			Graphics.DrawMesh(this.hm, identity, this.globalSettings.hairMaterial, 0, null, 0, null, this.globalSettings.castShadows, this.globalSettings.receiveShadows);
		}
	}

	// Token: 0x06006120 RID: 24864 RVA: 0x0024F934 File Offset: 0x0024DD34
	public void UpdateThreadSafe(int vindex)
	{
		this.SetVarsThreadSafe();
		this.SimulatePoints();
		this.UpdateMeshDataFromPoints(vindex);
	}

	// Token: 0x040050CD RID: 20685
	public HairGlobalSettings globalSettings;

	// Token: 0x040050CE RID: 20686
	public Vector3[] hmverts;

	// Token: 0x040050CF RID: 20687
	public Vector3[] hmnormals;

	// Token: 0x040050D0 RID: 20688
	public Vector4[] hmtangents;

	// Token: 0x040050D1 RID: 20689
	public Vector2[] hmuvs;

	// Token: 0x040050D2 RID: 20690
	public int[] hmtriangles;

	// Token: 0x040050D3 RID: 20691
	public int numVertices;

	// Token: 0x040050D4 RID: 20692
	public int numTrianglePoints;

	// Token: 0x040050D5 RID: 20693
	public Vector3 anchor;

	// Token: 0x040050D6 RID: 20694
	public Vector3 root;

	// Token: 0x040050D7 RID: 20695
	public Matrix4x4 rootMatrix;

	// Token: 0x040050D8 RID: 20696
	public Matrix4x4 rootChangeMatrix;

	// Token: 0x040050D9 RID: 20697
	public Vector3 anchorToRoot;

	// Token: 0x040050DA RID: 20698
	public Vector3 anchorTangent;

	// Token: 0x040050DB RID: 20699
	public Vector3 anchorTangent2;

	// Token: 0x040050DC RID: 20700
	public bool enableDraw = true;

	// Token: 0x040050DD RID: 20701
	public bool debug;

	// Token: 0x040050DE RID: 20702
	private LinkedPoint anchorPoint;

	// Token: 0x040050DF RID: 20703
	private LinkedPoint rootPoint;

	// Token: 0x040050E0 RID: 20704
	private LinkedPoint lastPoint;

	// Token: 0x040050E1 RID: 20705
	private LinkedPoint thisPoint;

	// Token: 0x040050E2 RID: 20706
	private int numHairs;

	// Token: 0x040050E3 RID: 20707
	private Mesh hm;

	// Token: 0x040050E4 RID: 20708
	private Vector3[] subHairXYZOffsets;

	// Token: 0x040050E5 RID: 20709
	private Vector3 cameraPosition;

	// Token: 0x040050E6 RID: 20710
	private int numVerticesPerHair;

	// Token: 0x040050E7 RID: 20711
	private int numTrianglePointsPerHair;

	// Token: 0x040050E8 RID: 20712
	private int vertInc;

	// Token: 0x040050E9 RID: 20713
	private int triInc;

	// Token: 0x040050EA RID: 20714
	private float oneMinusSheetHairRoundness;

	// Token: 0x040050EB RID: 20715
	private float sqrtSheetHairRoundness;

	// Token: 0x040050EC RID: 20716
	private float sqrtOneMinusSheetHairRoundness;

	// Token: 0x040050ED RID: 20717
	private Transform cameraTransform;

	// Token: 0x040050EE RID: 20718
	private Ray r;

	// Token: 0x040050EF RID: 20719
	private float randomStiffnessVariance;

	// Token: 0x040050F0 RID: 20720
	private float stiffnessEndVaried;

	// Token: 0x040050F1 RID: 20721
	private float stiffnessRootVaried;

	// Token: 0x040050F2 RID: 20722
	private float lastStiffnessVariance;

	// Token: 0x02000C93 RID: 3219
	public enum HairDrawType
	{
		// Token: 0x040050F4 RID: 20724
		Sheet,
		// Token: 0x040050F5 RID: 20725
		Quads,
		// Token: 0x040050F6 RID: 20726
		Tube,
		// Token: 0x040050F7 RID: 20727
		LineStrip,
		// Token: 0x040050F8 RID: 20728
		Lines,
		// Token: 0x040050F9 RID: 20729
		GPULines
	}

	// Token: 0x02000C94 RID: 3220
	public enum HairBundleType
	{
		// Token: 0x040050FB RID: 20731
		Rectangular,
		// Token: 0x040050FC RID: 20732
		Circular
	}
}
