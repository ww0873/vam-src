using System;
using System.Collections.Generic;
using UnityEngine;

namespace AQUAS
{
	// Token: 0x02000016 RID: 22
	public class AQUAS_Look : MonoBehaviour
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x000074BF File Offset: 0x000058BF
		public AQUAS_Look()
		{
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000074FA File Offset: 0x000058FA
		private void Update()
		{
			this.MouseLookAveraged();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00007504 File Offset: 0x00005904
		private void MouseLookAveraged()
		{
			this.rotAverageX = 0f;
			this.rotAverageY = 0f;
			this.mouseDeltaX = 0f;
			this.mouseDeltaY = 0f;
			this.mouseDeltaX += Input.GetAxis("Mouse X") * this._sensitivityX;
			this.mouseDeltaY += Input.GetAxis("Mouse Y") * this._sensitivityY;
			this._rotArrayX.Add(this.mouseDeltaX);
			this._rotArrayY.Add(this.mouseDeltaY);
			if (this._rotArrayX.Count >= this._averageFromThisManySteps)
			{
				this._rotArrayX.RemoveAt(0);
			}
			if (this._rotArrayY.Count >= this._averageFromThisManySteps)
			{
				this._rotArrayY.RemoveAt(0);
			}
			for (int i = 0; i < this._rotArrayX.Count; i++)
			{
				this.rotAverageX += this._rotArrayX[i];
			}
			for (int j = 0; j < this._rotArrayY.Count; j++)
			{
				this.rotAverageY += this._rotArrayY[j];
			}
			this.rotAverageX /= (float)this._rotArrayX.Count;
			this.rotAverageY /= (float)this._rotArrayY.Count;
			this._playerRootT.Rotate(0f, this.rotAverageX, 0f, Space.World);
			this._cameraT.Rotate(-this.rotAverageY, 0f, 0f, Space.Self);
		}

		// Token: 0x040000D6 RID: 214
		[Header("Info")]
		private List<float> _rotArrayX = new List<float>();

		// Token: 0x040000D7 RID: 215
		private List<float> _rotArrayY = new List<float>();

		// Token: 0x040000D8 RID: 216
		private float rotAverageX;

		// Token: 0x040000D9 RID: 217
		private float rotAverageY;

		// Token: 0x040000DA RID: 218
		private float mouseDeltaX;

		// Token: 0x040000DB RID: 219
		private float mouseDeltaY;

		// Token: 0x040000DC RID: 220
		[Header("Settings")]
		public bool _isLocked;

		// Token: 0x040000DD RID: 221
		public float _sensitivityX = 1.5f;

		// Token: 0x040000DE RID: 222
		public float _sensitivityY = 1.5f;

		// Token: 0x040000DF RID: 223
		[Tooltip("The more steps, the smoother it will be.")]
		public int _averageFromThisManySteps = 3;

		// Token: 0x040000E0 RID: 224
		[Header("References")]
		[Tooltip("Object to be rotated when mouse moves left/right.")]
		public Transform _playerRootT;

		// Token: 0x040000E1 RID: 225
		[Tooltip("Object to be rotated when mouse moves up/down.")]
		public Transform _cameraT;
	}
}
