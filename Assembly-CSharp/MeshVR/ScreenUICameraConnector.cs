using System;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000DDD RID: 3549
	public class ScreenUICameraConnector : MonoBehaviour
	{
		// Token: 0x06006DD1 RID: 28113 RVA: 0x002941E3 File Offset: 0x002925E3
		public ScreenUICameraConnector()
		{
		}

		// Token: 0x06006DD2 RID: 28114 RVA: 0x002941EB File Offset: 0x002925EB
		private void Awake()
		{
			this.canvas = base.GetComponent<Canvas>();
		}

		// Token: 0x06006DD3 RID: 28115 RVA: 0x002941F9 File Offset: 0x002925F9
		protected void CheckCamera()
		{
			if (this.canvas != null && this.canvas.worldCamera == null)
			{
				this.canvas.worldCamera = ScreenUICamera.screenUICamera;
			}
		}

		// Token: 0x06006DD4 RID: 28116 RVA: 0x00294232 File Offset: 0x00292632
		private void Update()
		{
			this.CheckCamera();
		}

		// Token: 0x04005F18 RID: 24344
		protected Canvas canvas;
	}
}
