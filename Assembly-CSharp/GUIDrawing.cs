using System;
using UnityEngine;

// Token: 0x02000DC0 RID: 3520
public class GUIDrawing
{
	// Token: 0x06006D10 RID: 27920 RVA: 0x0029168E File Offset: 0x0028FA8E
	public GUIDrawing()
	{
	}

	// Token: 0x06006D11 RID: 27921 RVA: 0x00291696 File Offset: 0x0028FA96
	public static void DrawLine(Rect rect)
	{
		GUIDrawing.DrawLine(rect, GUI.contentColor, 1f);
	}

	// Token: 0x06006D12 RID: 27922 RVA: 0x002916A8 File Offset: 0x0028FAA8
	public static void DrawLine(Rect rect, Color color)
	{
		GUIDrawing.DrawLine(rect, color, 1f);
	}

	// Token: 0x06006D13 RID: 27923 RVA: 0x002916B6 File Offset: 0x0028FAB6
	public static void DrawLine(Rect rect, float width)
	{
		GUIDrawing.DrawLine(rect, GUI.contentColor, width);
	}

	// Token: 0x06006D14 RID: 27924 RVA: 0x002916C4 File Offset: 0x0028FAC4
	public static void DrawLine(Rect rect, Color color, float width)
	{
		GUIDrawing.DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height), color, width);
	}

	// Token: 0x06006D15 RID: 27925 RVA: 0x00291703 File Offset: 0x0028FB03
	public static void DrawLine(Vector2 pointA, Vector2 pointB)
	{
		GUIDrawing.DrawLine(pointA, pointB, GUI.contentColor, 1f);
	}

	// Token: 0x06006D16 RID: 27926 RVA: 0x00291716 File Offset: 0x0028FB16
	public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color)
	{
		GUIDrawing.DrawLine(pointA, pointB, color, 1f);
	}

	// Token: 0x06006D17 RID: 27927 RVA: 0x00291725 File Offset: 0x0028FB25
	public static void DrawLine(Vector2 pointA, Vector2 pointB, float width)
	{
		GUIDrawing.DrawLine(pointA, pointB, GUI.contentColor, width);
	}

	// Token: 0x06006D18 RID: 27928 RVA: 0x00291734 File Offset: 0x0028FB34
	public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
	{
		Matrix4x4 matrix = GUI.matrix;
		if (!GUIDrawing.lineTex)
		{
			GUIDrawing.lineTex = new Texture2D(1, 1);
		}
		Color color2 = GUI.color;
		GUI.color = color;
		float num = Vector3.Angle(pointB - pointA, Vector2.right);
		if (pointA.y > pointB.y)
		{
			num = -num;
		}
		GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
		GUIUtility.RotateAroundPivot(num, pointA);
		GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1f, 1f), GUIDrawing.lineTex);
		GUI.matrix = matrix;
		GUI.color = color2;
	}

	// Token: 0x04005E9C RID: 24220
	public static Texture2D lineTex;
}
