using System;
using System.Collections.Generic;
using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.Tools.Commands;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
	// Token: 0x0200098E RID: 2446
	public class JointsPass : ICacheCommand
	{
		// Token: 0x06003D0D RID: 15629 RVA: 0x0012857A File Offset: 0x0012697A
		public JointsPass(ClothSettings settings)
		{
			this.data = settings.GeometryData;
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x0012858E File Offset: 0x0012698E
		public void CancelCache()
		{
			this.cancelCache = true;
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x00128597 File Offset: 0x00126997
		public void PrepCache()
		{
			this.cancelCache = false;
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x001285A0 File Offset: 0x001269A0
		public void Cache()
		{
			if (this.data != null)
			{
				List<Vector2> list = this.CreateJointsList(this.data.AllTringles, this.data.MeshToPhysicsVerticesMap);
				if (!this.cancelCache)
				{
					this.data.JointGroups = this.CreateJointsGroups(list);
				}
			}
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x001285F4 File Offset: 0x001269F4
		private List<Vector2> CreateJointsList(int[] indices, int[] meshToPhysicsVerticesMap)
		{
			HashSet<Vector2> hashSet = new HashSet<Vector2>();
			for (int i = 0; i < indices.Length; i += 3)
			{
				if (this.cancelCache)
				{
					return null;
				}
				int num = meshToPhysicsVerticesMap[indices[i]];
				int num2 = meshToPhysicsVerticesMap[indices[i + 1]];
				int num3 = meshToPhysicsVerticesMap[indices[i + 2]];
				this.AddToHashSet(hashSet, num, num2);
				this.AddToHashSet(hashSet, num2, num3);
				this.AddToHashSet(hashSet, num3, num);
			}
			return hashSet.ToList<Vector2>();
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x00128662 File Offset: 0x00126A62
		private void AddToHashSet(HashSet<Vector2> set, int i1, int i2)
		{
			if (i1 == -1 || i2 == -1)
			{
				return;
			}
			set.Add((i1 <= i2) ? new Vector2((float)i2, (float)i1) : new Vector2((float)i1, (float)i2));
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x00128698 File Offset: 0x00126A98
		private List<Int2ListContainer> CreateJointsGroups(List<Vector2> list)
		{
			this.jointGroupsSet = new List<HashSet<int>>();
			this.jointGroups = new List<Int2ListContainer>();
			foreach (Vector2 vector in list)
			{
				if (this.cancelCache)
				{
					return null;
				}
				this.AddJoint(new Int2((int)vector.x, (int)vector.y));
			}
			return this.jointGroups;
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x00128734 File Offset: 0x00126B34
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

		// Token: 0x06003D15 RID: 15637 RVA: 0x001287CC File Offset: 0x00126BCC
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

		// Token: 0x04002EFF RID: 12031
		private readonly ClothGeometryData data;

		// Token: 0x04002F00 RID: 12032
		private List<HashSet<int>> jointGroupsSet;

		// Token: 0x04002F01 RID: 12033
		private List<Int2ListContainer> jointGroups;

		// Token: 0x04002F02 RID: 12034
		protected bool cancelCache;
	}
}
