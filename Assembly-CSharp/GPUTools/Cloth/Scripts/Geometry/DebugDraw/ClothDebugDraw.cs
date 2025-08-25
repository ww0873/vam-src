using System;
using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Cloth.Scripts.Types;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.DebugDraw
{
	// Token: 0x0200098A RID: 2442
	public class ClothDebugDraw
	{
		// Token: 0x06003CFA RID: 15610 RVA: 0x00127C02 File Offset: 0x00126002
		public ClothDebugDraw()
		{
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x00127C0C File Offset: 0x0012600C
		public static void Draw(ClothSettings settings)
		{
			if (settings.GeometryData.Particles == null)
			{
				return;
			}
			Gizmos.color = Color.green;
			Matrix4x4 toWorldMatrix = settings.MeshProvider.ToWorldMatrix;
			ClothGeometryData geometryData = settings.GeometryData;
			if (settings.GeometryDebugDrawDistanceJoints)
			{
				foreach (Int2ListContainer int2ListContainer in geometryData.JointGroups)
				{
					foreach (Int2 @int in int2ListContainer.List)
					{
						Vector3 from = toWorldMatrix.MultiplyPoint3x4(geometryData.Particles[@int.X]);
						Vector3 to = toWorldMatrix.MultiplyPoint3x4(geometryData.Particles[@int.Y]);
						Gizmos.DrawLine(from, to);
					}
				}
			}
			if (settings.GeometryDebugDrawStiffnessJoints)
			{
				Gizmos.color = Color.yellow;
				foreach (Int2ListContainer int2ListContainer2 in geometryData.StiffnessJointGroups)
				{
					foreach (Int2 int2 in int2ListContainer2.List)
					{
						Vector3 from2 = toWorldMatrix.MultiplyPoint3x4(geometryData.Particles[int2.X]);
						Vector3 to2 = toWorldMatrix.MultiplyPoint3x4(geometryData.Particles[int2.Y]);
						Gizmos.DrawLine(from2, to2);
					}
				}
			}
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x00127E14 File Offset: 0x00126214
		public static void DrawAlways(ClothSettings settings)
		{
			if (settings.CustomBounds)
			{
				Bounds bounds = settings.Bounds;
				Bounds bounds2 = new Bounds(settings.transform.TransformPoint(bounds.center), bounds.size);
				Gizmos.DrawWireCube(bounds2.center, bounds2.size);
			}
		}
	}
}
