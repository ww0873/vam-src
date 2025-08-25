using System;
using System.Collections.Generic;
using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.Tools.Commands;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
	// Token: 0x0200098F RID: 2447
	public class NearbyJointsPass : ICacheCommand
	{
		// Token: 0x06003D16 RID: 15638 RVA: 0x0012882F File Offset: 0x00126C2F
		public NearbyJointsPass(ClothSettings settings)
		{
			this.clothSettings = settings;
			this.data = settings.GeometryData;
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x00128855 File Offset: 0x00126C55
		public void CancelCache()
		{
			this.cancelCache = true;
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x0012885E File Offset: 0x00126C5E
		public void PrepCache()
		{
			this.cancelCache = false;
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x00128868 File Offset: 0x00126C68
		public void Cache()
		{
			if (this.data != null)
			{
				List<Int2> list = this.CreateJointsList(this.data.AllTringles, this.data.Particles, this.data.MeshToPhysicsVerticesMap);
				if (!this.cancelCache)
				{
					this.data.NearbyJointGroups = this.CreateJointsGroups(list);
				}
			}
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x001288C8 File Offset: 0x00126CC8
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

		// Token: 0x06003D1B RID: 15643 RVA: 0x00128904 File Offset: 0x00126D04
		private List<Int2> CreateJointsList(int[] indices, Vector3[] particles, int[] meshToPhysicsVerticesMap)
		{
			HashSet<Int2> hashSet = new HashSet<Int2>();
			HashSet<Int2> hashSet2 = new HashSet<Int2>();
			if (this.clothSettings.CreateNearbyJoints)
			{
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
						this.data.status = "Nearby Joints Pass 1: " + j * 100 / indices.Length + "%";
					}
					int num4 = meshToPhysicsVerticesMap[indices[j]];
					HashSet<int> hashSet3;
					if (dictionary.TryGetValue(num4, out hashSet3))
					{
						foreach (int num5 in hashSet3)
						{
							this.AddToHashSet(hashSet, num4, num5);
							HashSet<int> hashSet4;
							if (dictionary.TryGetValue(num5, out hashSet4))
							{
								foreach (int num6 in hashSet4)
								{
									if (num4 != num6 && !hashSet3.Contains(num6))
									{
										this.AddToHashSet(hashSet, num4, num6);
									}
								}
							}
						}
					}
				}
				int num7 = 0;
				for (int k = 0; k < particles.Length; k++)
				{
					if (k % 100 == 0)
					{
						this.data.status = "Nearby Joints Pass 2: " + k * 100 / particles.Length + "%";
					}
					Vector3 a = particles[k];
					for (int l = k + 1; l < particles.Length; l++)
					{
						if (this.cancelCache)
						{
							return null;
						}
						Int2 item;
						item.X = k;
						item.Y = l;
						if (!hashSet.Contains(item))
						{
							Vector3 b = particles[l];
							if (Vector3.Distance(a, b) < this.clothSettings.NearbyJointsMaxDistance)
							{
								hashSet2.Add(item);
								num7++;
								if (num7 > this.jointNumberLimit)
								{
									Debug.LogError("Reached nearby joint hard limit " + this.jointNumberLimit);
									break;
								}
							}
						}
					}
				}
			}
			return hashSet2.ToList<Int2>();
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x00128BD8 File Offset: 0x00126FD8
		private void AddToHashSet(HashSet<Int2> set, int i1, int i2)
		{
			if (i1 == -1 || i2 == -1)
			{
				return;
			}
			set.Add((i1 <= i2) ? new Int2(i2, i1) : new Int2(i1, i2));
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x00128C0C File Offset: 0x0012700C
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

		// Token: 0x06003D1E RID: 15646 RVA: 0x00128C94 File Offset: 0x00127094
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

		// Token: 0x06003D1F RID: 15647 RVA: 0x00128D2C File Offset: 0x0012712C
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

		// Token: 0x04002F03 RID: 12035
		private readonly ClothGeometryData data;

		// Token: 0x04002F04 RID: 12036
		private readonly ClothSettings clothSettings;

		// Token: 0x04002F05 RID: 12037
		private List<HashSet<int>> jointGroupsSet;

		// Token: 0x04002F06 RID: 12038
		private List<Int2ListContainer> jointGroups;

		// Token: 0x04002F07 RID: 12039
		private int jointNumberLimit = 200000;

		// Token: 0x04002F08 RID: 12040
		protected bool cancelCache;
	}
}
