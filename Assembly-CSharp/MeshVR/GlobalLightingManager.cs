using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000C10 RID: 3088
	public class GlobalLightingManager : MonoBehaviour
	{
		// Token: 0x060059C9 RID: 22985 RVA: 0x00210895 File Offset: 0x0020EC95
		public GlobalLightingManager()
		{
		}

		// Token: 0x060059CA RID: 22986 RVA: 0x002108A0 File Offset: 0x0020ECA0
		protected void ResyncLightmapData()
		{
			LightmapData[] lightmaps = this.lightmapDataStack[this.lightmapDataStack.Count - 1];
			LightmapSettings.lightmaps = lightmaps;
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x060059CB RID: 22987 RVA: 0x002108CC File Offset: 0x0020ECCC
		protected LightmapData[] currentLightmapData
		{
			get
			{
				return this.lightmapDataStack[this.lightmapDataStack.Count - 1];
			}
		}

		// Token: 0x060059CC RID: 22988 RVA: 0x002108E8 File Offset: 0x0020ECE8
		public bool PushLightmapData(LightmapData[] lmd)
		{
			LightmapData[] array = this.lightmapDataStack[this.lightmapDataStack.Count - 1];
			if (array == lmd)
			{
				return false;
			}
			this.lightmapDataStack.Add(lmd);
			this.ResyncLightmapData();
			return true;
		}

		// Token: 0x060059CD RID: 22989 RVA: 0x0021092A File Offset: 0x0020ED2A
		public void RemoveLightmapData(LightmapData[] lmd)
		{
			if (lmd != null)
			{
				this.lightmapDataStack.Remove(lmd);
			}
			this.ResyncLightmapData();
		}

		// Token: 0x060059CE RID: 22990 RVA: 0x00210948 File Offset: 0x0020ED48
		protected void ResyncLightProbes()
		{
			GlobalLightingManager.LightProbesHolder lightProbesHolder = this.lightProbesStack[this.lightProbesStack.Count - 1];
			LightmapSettings.lightProbes = lightProbesHolder.lp;
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x060059CF RID: 22991 RVA: 0x00210979 File Offset: 0x0020ED79
		protected GlobalLightingManager.LightProbesHolder currentLightProbesHolder
		{
			get
			{
				return this.lightProbesStack[this.lightProbesStack.Count - 1];
			}
		}

		// Token: 0x060059D0 RID: 22992 RVA: 0x00210994 File Offset: 0x0020ED94
		public GlobalLightingManager.LightProbesHolder PushLightProbes(LightProbes lp)
		{
			GlobalLightingManager.LightProbesHolder lightProbesHolder = this.lightProbesStack[this.lightProbesStack.Count - 1];
			if (lightProbesHolder.lp == lp)
			{
				return null;
			}
			GlobalLightingManager.LightProbesHolder lightProbesHolder2 = new GlobalLightingManager.LightProbesHolder();
			lightProbesHolder2.lp = lp;
			this.lightProbesStack.Add(lightProbesHolder2);
			this.ResyncLightProbes();
			return lightProbesHolder2;
		}

		// Token: 0x060059D1 RID: 22993 RVA: 0x002109ED File Offset: 0x0020EDED
		public void PushLightProbesHolder(GlobalLightingManager.LightProbesHolder lph)
		{
			this.lightProbesStack.Add(lph);
			this.ResyncLightProbes();
		}

		// Token: 0x060059D2 RID: 22994 RVA: 0x00210A01 File Offset: 0x0020EE01
		public void RemoveLightProbesHolder(GlobalLightingManager.LightProbesHolder lph)
		{
			if (lph != null)
			{
				this.lightProbesStack.Remove(lph);
			}
			this.ResyncLightProbes();
		}

		// Token: 0x060059D3 RID: 22995 RVA: 0x00210A1C File Offset: 0x0020EE1C
		private void Awake()
		{
			GlobalLightingManager.singleton = this;
			this._startingLightmapData = LightmapSettings.lightmaps;
			this._startingLightProbesHolder = new GlobalLightingManager.LightProbesHolder();
			this._startingLightProbesHolder.lp = LightmapSettings.lightProbes;
			this.lightmapDataStack = new List<LightmapData[]>();
			this.lightmapDataStack.Add(this._startingLightmapData);
			this.lightProbesStack = new List<GlobalLightingManager.LightProbesHolder>();
			this.lightProbesStack.Add(this._startingLightProbesHolder);
		}

		// Token: 0x040049E3 RID: 18915
		public static GlobalLightingManager singleton;

		// Token: 0x040049E4 RID: 18916
		protected LightmapData[] _startingLightmapData;

		// Token: 0x040049E5 RID: 18917
		protected List<LightmapData[]> lightmapDataStack;

		// Token: 0x040049E6 RID: 18918
		protected GlobalLightingManager.LightProbesHolder _startingLightProbesHolder;

		// Token: 0x040049E7 RID: 18919
		protected List<GlobalLightingManager.LightProbesHolder> lightProbesStack;

		// Token: 0x02000C11 RID: 3089
		public class LightProbesHolder
		{
			// Token: 0x060059D4 RID: 22996 RVA: 0x00210A8D File Offset: 0x0020EE8D
			public LightProbesHolder()
			{
			}

			// Token: 0x040049E8 RID: 18920
			public LightProbes lp;
		}
	}
}
