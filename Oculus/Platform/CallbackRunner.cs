using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x020007E6 RID: 2022
	public class CallbackRunner : MonoBehaviour
	{
		// Token: 0x06003316 RID: 13078 RVA: 0x00109314 File Offset: 0x00107714
		public CallbackRunner()
		{
		}

		// Token: 0x06003317 RID: 13079
		[DllImport("LibOVRPlatform64_1")]
		private static extern void ovr_UnityResetTestPlatform();

		// Token: 0x06003318 RID: 13080 RVA: 0x00109324 File Offset: 0x00107724
		private void Awake()
		{
			CallbackRunner x = UnityEngine.Object.FindObjectOfType<CallbackRunner>();
			if (x != this)
			{
				Debug.LogWarning("You only need one instance of CallbackRunner");
			}
			if (this.IsPersistantBetweenSceneLoads)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x00109363 File Offset: 0x00107763
		private void Update()
		{
			Request.RunCallbacks(0U);
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x0010936B File Offset: 0x0010776B
		private void OnDestroy()
		{
		}

		// Token: 0x04002710 RID: 10000
		public bool IsPersistantBetweenSceneLoads = true;
	}
}
