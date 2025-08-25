using System;
using UnityEngine;
using UnityEngine.UI;

namespace GPUTools.Common.Scripts.Tools
{
	// Token: 0x020009CD RID: 2509
	public class FpsCounter : MonoBehaviour
	{
		// Token: 0x06003F51 RID: 16209 RVA: 0x0012EF16 File Offset: 0x0012D316
		public FpsCounter()
		{
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x0012EF20 File Offset: 0x0012D320
		private void Update()
		{
			this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
			float num = this.deltaTime * 1000f;
			float num2 = 1f / this.deltaTime;
			this.textField.text = string.Format("{0:0.0} ms ({1:0.} fps)", num, num2);
		}

		// Token: 0x04003000 RID: 12288
		[SerializeField]
		private Text textField;

		// Token: 0x04003001 RID: 12289
		private float deltaTime;
	}
}
