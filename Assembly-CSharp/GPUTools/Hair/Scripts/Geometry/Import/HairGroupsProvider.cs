using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.Tools.Debug;
using GPUTools.Hair.Scripts.Geometry.Tools;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Import
{
	// Token: 0x020009FC RID: 2556
	[Serializable]
	public class HairGroupsProvider
	{
		// Token: 0x060040BD RID: 16573 RVA: 0x00134234 File Offset: 0x00132634
		public HairGroupsProvider()
		{
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x00134247 File Offset: 0x00132647
		public void Process(Matrix4x4 worldToObject)
		{
			this.VerticesGroups = this.InitVerticesGroups(worldToObject);
			this.Vertices = this.InitVertices();
			this.ColorsGroups = this.InitColorGroups();
			this.Colors = this.InitColors();
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x0013427C File Offset: 0x0013267C
		private List<List<Vector3>> InitVerticesGroups(Matrix4x4 worldToObject)
		{
			HairGroupsProvider.<InitVerticesGroups>c__AnonStorey0 <InitVerticesGroups>c__AnonStorey = new HairGroupsProvider.<InitVerticesGroups>c__AnonStorey0();
			<InitVerticesGroups>c__AnonStorey.worldToObject = worldToObject;
			return this.HairFilters.Select(new Func<MeshFilter, List<Vector3>>(<InitVerticesGroups>c__AnonStorey.<>m__0)).ToList<List<Vector3>>();
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x001342B2 File Offset: 0x001326B2
		private List<Vector3> InitVertices()
		{
			IEnumerable<List<Vector3>> verticesGroups = this.VerticesGroups;
			if (HairGroupsProvider.<>f__am$cache0 == null)
			{
				HairGroupsProvider.<>f__am$cache0 = new Func<List<Vector3>, IEnumerable<Vector3>>(HairGroupsProvider.<InitVertices>m__0);
			}
			return verticesGroups.SelectMany(HairGroupsProvider.<>f__am$cache0).ToList<Vector3>();
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x001342E1 File Offset: 0x001326E1
		private List<List<Color>> InitColorGroups()
		{
			IEnumerable<MeshFilter> hairFilters = this.HairFilters;
			if (HairGroupsProvider.<>f__am$cache1 == null)
			{
				HairGroupsProvider.<>f__am$cache1 = new Func<MeshFilter, List<Color>>(HairGroupsProvider.<InitColorGroups>m__1);
			}
			return hairFilters.Select(HairGroupsProvider.<>f__am$cache1).ToList<List<Color>>();
		}

		// Token: 0x060040C2 RID: 16578 RVA: 0x00134310 File Offset: 0x00132710
		private List<Color> InitColors()
		{
			IEnumerable<List<Color>> colorsGroups = this.ColorsGroups;
			if (HairGroupsProvider.<>f__am$cache2 == null)
			{
				HairGroupsProvider.<>f__am$cache2 = new Func<List<Color>, IEnumerable<Color>>(HairGroupsProvider.<InitColors>m__2);
			}
			return colorsGroups.SelectMany(HairGroupsProvider.<>f__am$cache2).ToList<Color>();
		}

		// Token: 0x060040C3 RID: 16579 RVA: 0x0013433F File Offset: 0x0013273F
		public bool Validate(bool log)
		{
			if (Validator.TestList<MeshFilter>(this.HairFilters))
			{
				return true;
			}
			if (log)
			{
				Debug.LogError("Hair list is empty or contains empty elements ");
			}
			return false;
		}

		// Token: 0x060040C4 RID: 16580 RVA: 0x00134364 File Offset: 0x00132764
		[CompilerGenerated]
		private static IEnumerable<Vector3> <InitVertices>m__0(List<Vector3> verticesGroup)
		{
			return verticesGroup;
		}

		// Token: 0x060040C5 RID: 16581 RVA: 0x00134367 File Offset: 0x00132767
		[CompilerGenerated]
		private static List<Color> <InitColorGroups>m__1(MeshFilter filter)
		{
			return filter.sharedMesh.colors.ToList<Color>();
		}

		// Token: 0x060040C6 RID: 16582 RVA: 0x00134379 File Offset: 0x00132779
		[CompilerGenerated]
		private static IEnumerable<Color> <InitColors>m__2(List<Color> colorsGroup)
		{
			return colorsGroup;
		}

		// Token: 0x040030C0 RID: 12480
		[SerializeField]
		public List<MeshFilter> HairFilters = new List<MeshFilter>();

		// Token: 0x040030C1 RID: 12481
		[SerializeField]
		public List<List<Vector3>> VerticesGroups;

		// Token: 0x040030C2 RID: 12482
		[SerializeField]
		public List<Vector3> Vertices;

		// Token: 0x040030C3 RID: 12483
		[SerializeField]
		public List<List<Color>> ColorsGroups;

		// Token: 0x040030C4 RID: 12484
		[SerializeField]
		public List<Color> Colors;

		// Token: 0x040030C5 RID: 12485
		[CompilerGenerated]
		private static Func<List<Vector3>, IEnumerable<Vector3>> <>f__am$cache0;

		// Token: 0x040030C6 RID: 12486
		[CompilerGenerated]
		private static Func<MeshFilter, List<Color>> <>f__am$cache1;

		// Token: 0x040030C7 RID: 12487
		[CompilerGenerated]
		private static Func<List<Color>, IEnumerable<Color>> <>f__am$cache2;

		// Token: 0x02000FBD RID: 4029
		[CompilerGenerated]
		private sealed class <InitVerticesGroups>c__AnonStorey0
		{
			// Token: 0x060074FC RID: 29948 RVA: 0x0013437C File Offset: 0x0013277C
			public <InitVerticesGroups>c__AnonStorey0()
			{
			}

			// Token: 0x060074FD RID: 29949 RVA: 0x00134384 File Offset: 0x00132784
			internal List<Vector3> <>m__0(MeshFilter filter)
			{
				return MeshUtils.GetVerticesInSpace(filter.sharedMesh, filter.transform.localToWorldMatrix, this.worldToObject);
			}

			// Token: 0x04006915 RID: 26901
			internal Matrix4x4 worldToObject;
		}
	}
}
