using System;
using UnityEngine;

// Token: 0x020002F1 RID: 753
public abstract class SphereUtility
{
	// Token: 0x060011B6 RID: 4534 RVA: 0x00061CAD File Offset: 0x000600AD
	protected SphereUtility()
	{
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x00061CB5 File Offset: 0x000600B5
	public static float RadiusAtHeight(float yPos)
	{
		return Mathf.Abs(Mathf.Cos(Mathf.Asin(yPos)));
	}

	// Token: 0x060011B8 RID: 4536 RVA: 0x00061CC8 File Offset: 0x000600C8
	public static Vector3 SphericalToPoint(float yPosition, float radAngle)
	{
		float num = SphereUtility.RadiusAtHeight(yPosition);
		Vector3 result = new Vector3(num * Mathf.Cos(radAngle), yPosition, num * Mathf.Sin(radAngle));
		return result;
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x00061CF5 File Offset: 0x000600F5
	public static float RadAngleToPercent(float radAngle)
	{
		return radAngle / 6.2831855f;
	}

	// Token: 0x060011BA RID: 4538 RVA: 0x00061CFE File Offset: 0x000600FE
	public static float PercentToRadAngle(float percent)
	{
		return percent * 6.2831855f;
	}

	// Token: 0x060011BB RID: 4539 RVA: 0x00061D07 File Offset: 0x00060107
	public static float HeightToPercent(float yValue)
	{
		return yValue / 2f + 0.5f;
	}

	// Token: 0x060011BC RID: 4540 RVA: 0x00061D16 File Offset: 0x00060116
	public static float PercentToHeight(float hPercent)
	{
		return Mathf.Lerp(-1f, 1f, hPercent);
	}

	// Token: 0x060011BD RID: 4541 RVA: 0x00061D28 File Offset: 0x00060128
	public static float AngleToReachTarget(Vector2 point, float targetAngle)
	{
		float num = SphereUtility.Atan2Positive(point.y, point.x);
		return 6.2831855f - num + targetAngle;
	}

	// Token: 0x060011BE RID: 4542 RVA: 0x00061D54 File Offset: 0x00060154
	public static float Atan2Positive(float y, float x)
	{
		float num = Mathf.Atan2(y, x);
		if (num < 0f)
		{
			num = 3.1415927f + (3.1415927f + num);
		}
		return num;
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x00061D84 File Offset: 0x00060184
	public static Vector3 RotateAroundXAxis(Vector3 point, float angle)
	{
		Vector2 vector = SphereUtility.Rotate2d(new Vector2(point.z, point.y), angle);
		return new Vector3(point.x, vector.y, vector.x);
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x00061DC8 File Offset: 0x000601C8
	public static Vector3 RotateAroundYAxis(Vector3 point, float angle)
	{
		Vector2 vector = SphereUtility.Rotate2d(new Vector2(point.x, point.z), angle);
		return new Vector3(vector.x, point.y, vector.y);
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x00061E0C File Offset: 0x0006020C
	public static Vector3 RotatePoint(Vector3 point, float xAxisRotation, float yAxisRotation)
	{
		Vector3 point2 = SphereUtility.RotateAroundYAxis(point, yAxisRotation);
		return SphereUtility.RotateAroundXAxis(point2, xAxisRotation);
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x00061E2C File Offset: 0x0006022C
	public static Vector2 Rotate2d(Vector2 pos, float angle)
	{
		Vector4 matrix = new Vector4(Mathf.Cos(angle), -Mathf.Sin(angle), Mathf.Sin(angle), Mathf.Cos(angle));
		return SphereUtility.Matrix2x2Mult(matrix, pos);
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x00061E60 File Offset: 0x00060260
	public static Vector2 Matrix2x2Mult(Vector4 matrix, Vector2 pos)
	{
		return new Vector2(matrix[0] * pos[0] + matrix[1] * pos[1], matrix[2] * pos[0] + matrix[3] * pos[1]);
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x00061EB8 File Offset: 0x000602B8
	public static void CalculateStarRotation(Vector3 star, out float xRotationAngle, out float yRotationAngle)
	{
		Vector3 point = new Vector3(star.x, star.y, star.z);
		yRotationAngle = SphereUtility.AngleToReachTarget(new Vector2(point.x, point.z), 1.5707964f);
		point = SphereUtility.RotateAroundYAxis(point, yRotationAngle);
		xRotationAngle = SphereUtility.AngleToReachTarget(new Vector3(point.z, point.y), 0f);
	}
}
