using System;
using System.Collections.Generic;
using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.Tools.Commands;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
	// Token: 0x02000993 RID: 2451
	public class StiffnessJointsPass : ICacheCommand
	{
		// Token: 0x06003D35 RID: 15669 RVA: 0x001294FB File Offset: 0x001278FB
		public StiffnessJointsPass(ClothSettings settings)
		{
			this.data = settings.GeometryData;
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x0012950F File Offset: 0x0012790F
		public void CancelCache()
		{
			this.cancelCache = true;
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x00129518 File Offset: 0x00127918
		public void PrepCache()
		{
			this.cancelCache = false;
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x00129524 File Offset: 0x00127924
		public void Cache()
		{
			if (this.data != null)
			{
				List<Int2> list = this.CreateJointsList(this.data.AllTringles, this.data.MeshToPhysicsVerticesMap);
				if (!this.cancelCache)
				{
					this.data.StiffnessJointGroups = this.CreateJointsGroups(list);
				}
			}
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x00129578 File Offset: 0x00127978
		private void AddNeighbors(Dictionary<int, HashSet<int>> vertToNeighborVerts, int index, int neighbor1, int neighbor2)
		{
			HashSet<int> hashSet;
			if (!vertToNeighborVerts.TryGetValue(index, out hashSet))
			{
				hashSet = new HashSet<int>();
				vertToNeighborVerts.Add(index, hashSet);
			}
			hashSet.Add(neighbor1);
			hashSet.Add(neighbor2);
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x001295B4 File Offset: 0x001279B4
		private List<Int2> CreateJointsList(int[] indices, int[] meshToPhysicsVerticesMap)
		{
			HashSet<Int2> hashSet = new HashSet<Int2>();
			Dictionary<int, HashSet<int>> dictionary = new Dictionary<int, HashSet<int>>();
			for (int i = 0; i < indices.Length; i += 3)
			{
				if (this.cancelCache)
				{
					return null;
				}
				int num = meshToPhysicsVerticesMap[indices[i]];
				int num2 = meshToPhysicsVerticesMap[indices[i + 1]];
				int num3 = meshToPhysicsVerticesMap[indices[i + 2]];
				this.AddNeighbors(dictionary, num, num2, num3);
				this.AddNeighbors(dictionary, num2, num, num3);
				this.AddNeighbors(dictionary, num3, num, num2);
			}
			for (int j = 0; j < indices.Length; j++)
			{
				if (this.cancelCache)
				{
					return null;
				}
				if (j % 100 == 0)
				{
					this.data.status = "Stiffness Joints: " + j * 100 / indices.Length + "%";
				}
				int num4 = meshToPhysicsVerticesMap[indices[j]];
				HashSet<int> hashSet2;
				if (dictionary.TryGetValue(num4, out hashSet2))
				{
					foreach (int num5 in hashSet2)
					{
						this.AddToHashSet(hashSet, num4, num5);
						HashSet<int> hashSet3;
						if (dictionary.TryGetValue(num5, out hashSet3))
						{
							foreach (int num6 in hashSet3)
							{
								if (num4 != num6 && !hashSet2.Contains(num6))
								{
									this.AddToHashSet(hashSet, num4, num6);
								}
							}
						}
					}
				}
			}
			return hashSet.ToList<Int2>();
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00129764 File Offset: 0x00127B64
		private void AddToHashSet(HashSet<Int2> set, int i1, int i2)
		{
			if (i1 == -1 || i2 == -1)
			{
				return;
			}
			set.Add((i1 <= i2) ? new Int2(i2, i1) : new Int2(i1, i2));
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x00129798 File Offset: 0x00127B98
		private List<Int2ListContainer> CreateJointsGroups(List<Int2> list)
		{
			this.jointGroupsSet = new List<HashSet<int>>();
			this.jointGroups = new List<Int2ListContainer>();
			foreach (Int2 item in list)
			{
				if (this.cancelCache)
				{
					return null;
				}
				this.AddJoint(item);
			}
			return this.jointGroups;
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x00129820 File Offset: 0x00127C20
		private void AddJoint(Int2 item)
		{
			for (int i = 0; i < this.jointGroupsSet.Count; i++)
			{
				HashSet<int> hashSet = this.jointGroupsSet[i];
				List<Int2> list = this.jointGroups[i].List;
				if (!hashSet.Contains(item.X) && !hashSet.Contains(item.Y))
				{
					hashSet.Add(item.X);
					hashSet.Add(item.Y);
					list.Add(item);
					return;
				}
			}
			this.CreateNewGroup(item);
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x001298B8 File Offset: 0x00127CB8
		private void CreateNewGroup(Int2 item)
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(item.X);
			hashSet.Add(item.Y);
			this.jointGroupsSet.Add(hashSet);
			List<Int2> list = new List<Int2>
			{
				item
			};
			this.jointGroups.Add(new Int2ListContainer
			{
				List = list
			});
		}

		// Token: 0x04002F13 RID: 12051
		private readonly ClothGeometryData data;

		// Token: 0x04002F14 RID: 12052
		private List<HashSet<int>> jointGroupsSet;

		// Token: 0x04002F15 RID: 12053
		private List<Int2ListContainer> jointGroups;

		// Token: 0x04002F16 RID: 12054
		protected bool cancelCache;
	}
}
