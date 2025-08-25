using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000E17 RID: 3607
	public class PerfMonCamera : MonoBehaviour
	{
		// Token: 0x06006F20 RID: 28448 RVA: 0x0029BBD0 File Offset: 0x00299FD0
		public PerfMonCamera()
		{
		}

		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x06006F21 RID: 28449 RVA: 0x0029BBD8 File Offset: 0x00299FD8
		// (set) Token: 0x06006F22 RID: 28450 RVA: 0x0029BBDF File Offset: 0x00299FDF
		public static bool wasSet
		{
			[CompilerGenerated]
			get
			{
				return PerfMonCamera.<wasSet>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				PerfMonCamera.<wasSet>k__BackingField = value;
			}
		}

		// Token: 0x06006F23 RID: 28451 RVA: 0x0029BBE7 File Offset: 0x00299FE7
		private void LateUpdate()
		{
			PerfMonCamera.wasSet = false;
		}

		// Token: 0x06006F24 RID: 28452 RVA: 0x0029BBEF File Offset: 0x00299FEF
		private void OnDisable()
		{
			PerfMonCamera.wasSet = false;
		}

		// Token: 0x06006F25 RID: 28453 RVA: 0x0029BBF7 File Offset: 0x00299FF7
		private void OnPreCull()
		{
			if (!PerfMonCamera.wasSet)
			{
				PerfMonCamera.wasSet = true;
				PerfMonCamera.renderStartTime = GlobalStopwatch.GetElapsedMilliseconds();
			}
		}

		// Token: 0x04006056 RID: 24662
		public static float renderStartTime;

		// Token: 0x04006057 RID: 24663
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static bool <wasSet>k__BackingField;
	}
}
