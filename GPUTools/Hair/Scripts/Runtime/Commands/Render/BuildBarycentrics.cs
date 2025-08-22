using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Render
{
	// Token: 0x02000A16 RID: 2582
	public class BuildBarycentrics : IBuildCommand
	{
		// Token: 0x0600416E RID: 16750 RVA: 0x00137525 File Offset: 0x00135925
		public BuildBarycentrics(HairSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x0600416F RID: 16751 RVA: 0x0013754C File Offset: 0x0013594C
		public void Build()
		{
			this.Gen();
			if (this.settings.RuntimeData.Barycentrics != null)
			{
				this.settings.RuntimeData.Barycentrics.Dispose();
			}
			if (this.barycentric.Count > 0)
			{
				this.settings.RuntimeData.Barycentrics = new GpuBuffer<Vector3>(this.barycentric.ToArray(), 12);
			}
			else
			{
				this.settings.RuntimeData.Barycentrics = null;
			}
			if (this.settings.RuntimeData.BarycentricsFixed != null)
			{
				this.settings.RuntimeData.BarycentricsFixed.Dispose();
			}
			if (this.barycentricFixed.Count > 0)
			{
				this.settings.RuntimeData.BarycentricsFixed = new GpuBuffer<Vector3>(this.barycentricFixed.ToArray(), 12);
			}
			else
			{
				this.settings.RuntimeData.BarycentricsFixed = null;
			}
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x00137645 File Offset: 0x00135A45
		public void Dispatch()
		{
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x00137647 File Offset: 0x00135A47
		public void FixedDispatch()
		{
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x0013764C File Offset: 0x00135A4C
		public void UpdateSettings()
		{
			this.Gen();
			if (this.settings.RuntimeData.Barycentrics != null)
			{
				this.settings.RuntimeData.Barycentrics.PushData();
			}
			if (this.settings.RuntimeData.BarycentricsFixed != null)
			{
				this.settings.RuntimeData.BarycentricsFixed.PushData();
			}
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x001376B4 File Offset: 0x00135AB4
		public void Dispose()
		{
			if (this.settings.RuntimeData.Barycentrics != null)
			{
				this.settings.RuntimeData.Barycentrics.Dispose();
			}
			if (this.settings.RuntimeData.BarycentricsFixed != null)
			{
				this.settings.RuntimeData.BarycentricsFixed.Dispose();
			}
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x00137718 File Offset: 0x00135B18
		private void Gen()
		{
			UnityEngine.Random.InitState(6);
			this.barycentric = new List<Vector3>();
			for (int i = 0; i < this.settings.StandsSettings.Provider.GetStandsNum(); i++)
			{
				for (int j = 0; j < BuildBarycentrics.MaxCount; j++)
				{
					this.barycentric.Add(this.GetRandomK());
				}
			}
			this.barycentricFixed = new List<Vector3>();
			for (int k = 0; k < this.settings.StandsSettings.Provider.GetStandsNum(); k++)
			{
				for (int l = 0; l < BuildBarycentrics.MaxCount; l += 3)
				{
					this.barycentricFixed.Add(new Vector3(0.99f, 0.005f, 0.005f));
					this.barycentricFixed.Add(new Vector3(0.005f, 0.99f, 0.005f));
					this.barycentricFixed.Add(new Vector3(0.005f, 0.005f, 0.99f));
				}
			}
			this.barycentricFixed = this.barycentricFixed.GetRange(0, BuildBarycentrics.MaxCount * this.settings.StandsSettings.Provider.GetStandsNum());
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x00137854 File Offset: 0x00135C54
		private void Split(Vector3 b1, Vector3 b2, Vector3 b3, int steps)
		{
			steps--;
			this.TryAdd(b1);
			this.TryAdd(b2);
			this.TryAdd(b3);
			Vector3 vector = (b1 + b2) * 0.5f;
			Vector3 vector2 = (b2 + b3) * 0.5f;
			Vector3 b4 = (b3 + b1) * 0.5f;
			if (steps < 0)
			{
				return;
			}
			this.Split(b1, vector, b4, steps);
			this.Split(b2, vector, vector2, steps);
			this.Split(b3, vector2, b4, steps);
			this.Split(vector, vector2, b4, steps);
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x001378E7 File Offset: 0x00135CE7
		private void TryAdd(Vector3 v)
		{
			if (!this.barycentric.Contains(v))
			{
				this.barycentric.Add(v);
			}
		}

		// Token: 0x06004177 RID: 16759 RVA: 0x00137908 File Offset: 0x00135D08
		private Vector3 GetRandomK()
		{
			float num = UnityEngine.Random.Range(0f, 1f);
			float num2 = UnityEngine.Random.Range(0f, 1f);
			if (num + num2 > 1f)
			{
				num = 1f - num;
				num2 = 1f - num2;
			}
			float z = 1f - (num + num2);
			return new Vector3(num, num2, z);
		}

		// Token: 0x06004178 RID: 16760 RVA: 0x00137964 File Offset: 0x00135D64
		// Note: this type is marked as 'beforefieldinit'.
		static BuildBarycentrics()
		{
		}

		// Token: 0x04003111 RID: 12561
		public static int MaxCount = 64;

		// Token: 0x04003112 RID: 12562
		private readonly HairSettings settings;

		// Token: 0x04003113 RID: 12563
		private List<Vector3> barycentric = new List<Vector3>();

		// Token: 0x04003114 RID: 12564
		private List<Vector3> barycentricFixed = new List<Vector3>();
	}
}
