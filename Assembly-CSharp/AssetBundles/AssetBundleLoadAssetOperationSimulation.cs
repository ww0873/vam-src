using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200009A RID: 154
	public class AssetBundleLoadAssetOperationSimulation : AssetBundleLoadAssetOperation
	{
		// Token: 0x06000232 RID: 562 RVA: 0x00010537 File Offset: 0x0000E937
		public AssetBundleLoadAssetOperationSimulation(UnityEngine.Object simulatedObject)
		{
			this.m_SimulatedObject = simulatedObject;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00010546 File Offset: 0x0000E946
		public override T GetAsset<T>()
		{
			return this.m_SimulatedObject as T;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00010558 File Offset: 0x0000E958
		public override bool Update()
		{
			return false;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0001055B File Offset: 0x0000E95B
		public override bool IsDone()
		{
			return true;
		}

		// Token: 0x0400031D RID: 797
		private UnityEngine.Object m_SimulatedObject;
	}
}
