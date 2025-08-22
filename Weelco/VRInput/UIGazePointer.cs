using System;
using UnityEngine;
using UnityEngine.UI;

namespace Weelco.VRInput
{
	// Token: 0x0200058C RID: 1420
	public class UIGazePointer : IUIPointer
	{
		// Token: 0x060023BA RID: 9146 RVA: 0x000CF15B File Offset: 0x000CD55B
		public UIGazePointer()
		{
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x000CF163 File Offset: 0x000CD563
		public override void Initialize()
		{
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x000CF165 File Offset: 0x000CD565
		public override void OnEnterControl(GameObject control)
		{
			this._isOver = true;
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x000CF16E File Offset: 0x000CD56E
		public override void OnExitControl(GameObject control)
		{
			this._isOver = false;
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x000CF177 File Offset: 0x000CD577
		public bool IsOver()
		{
			return this._isOver;
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060023BF RID: 9151 RVA: 0x000CF17F File Offset: 0x000CD57F
		public override Transform target
		{
			get
			{
				if (this.GazeCanvas == null)
				{
					throw new NullReferenceException("VRInputSettings::While Gaze Input , must contain Gaze Dot GameObject");
				}
				return this.GazeCanvas;
			}
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x000CF1A3 File Offset: 0x000CD5A3
		protected override void UpdateRaycasting(bool isHit, float distance)
		{
		}

		// Token: 0x04001E23 RID: 7715
		public Transform GazeCanvas;

		// Token: 0x04001E24 RID: 7716
		public Image GazeProgressBar;

		// Token: 0x04001E25 RID: 7717
		public float GazeClickTimer;

		// Token: 0x04001E26 RID: 7718
		public float GazeClickTimerDelay;

		// Token: 0x04001E27 RID: 7719
		private bool _isOver;
	}
}
