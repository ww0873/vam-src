using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Procedural
{
	// Token: 0x02000A06 RID: 2566
	public class ProceduralScalp : MonoBehaviour
	{
		// Token: 0x060040FC RID: 16636 RVA: 0x001350B3 File Offset: 0x001334B3
		public ProceduralScalp()
		{
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x001350BC File Offset: 0x001334BC
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			for (int i = 0; i <= this.Grid.ViewSizeX; i++)
			{
				for (int j = 0; j <= this.Grid.ViewSizeY; j++)
				{
					Vector3 splinePoint = this.Grid.GetSplinePoint((float)i / (float)this.Grid.ViewSizeX, (float)j / (float)this.Grid.ViewSizeY);
					Vector3 from = base.transform.TransformPoint(splinePoint);
					if (i < this.Grid.ViewSizeX)
					{
						Vector3 splinePoint2 = this.Grid.GetSplinePoint((float)(i + 1) / (float)this.Grid.ViewSizeX, (float)j / (float)this.Grid.ViewSizeY);
						Vector3 to = base.transform.TransformPoint(splinePoint2);
						Gizmos.DrawLine(from, to);
					}
					if (j < this.Grid.ViewSizeY)
					{
						Vector3 splinePoint3 = this.Grid.GetSplinePoint((float)i / (float)this.Grid.ViewSizeX, (float)(j + 1) / (float)this.Grid.ViewSizeY);
						Vector3 to2 = base.transform.TransformPoint(splinePoint3);
						Gizmos.DrawLine(from, to2);
					}
				}
			}
		}

		// Token: 0x040030E1 RID: 12513
		[SerializeField]
		public CurveGrid Grid;
	}
}
