using System;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000DDC RID: 3548
	public class ScreenUICamera : MonoBehaviour
	{
		// Token: 0x06006DCE RID: 28110 RVA: 0x002941CC File Offset: 0x002925CC
		public ScreenUICamera()
		{
		}

		// Token: 0x06006DCF RID: 28111 RVA: 0x002941D4 File Offset: 0x002925D4
		private void Awake()
		{
			ScreenUICamera.screenUICamera = base.GetComponent<Camera>();
		}

		// Token: 0x06006DD0 RID: 28112 RVA: 0x002941E1 File Offset: 0x002925E1
		// Note: this type is marked as 'beforefieldinit'.
		static ScreenUICamera()
		{
		}

		// Token: 0x04005F17 RID: 24343
		public static Camera screenUICamera;
	}
}
