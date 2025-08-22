using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x0200072F RID: 1839
	public class FrameRateControls : MonoBehaviour
	{
		// Token: 0x06002CB3 RID: 11443 RVA: 0x000EFBD0 File Offset: 0x000EDFD0
		public FrameRateControls()
		{
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x000EFC34 File Offset: 0x000EE034
		private void Awake()
		{
			if (QualitySettings.vSyncCount != 0)
			{
				Debug.LogWarning("vSync will override target frame rate. vSyncCount = " + QualitySettings.vSyncCount);
			}
			Application.targetFrameRate = this.targetRenderRate;
			Time.fixedDeltaTime = 1f / (float)this.fixedPhysicsRate;
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x000EFC84 File Offset: 0x000EE084
		private void Update()
		{
			if (Input.GetKey(this.unlockRender))
			{
				if (Input.GetKeyDown(this.decrease) && this.targetRenderRate > this.targetRenderRateStep)
				{
					this.targetRenderRate -= this.targetRenderRateStep;
					Application.targetFrameRate = this.targetRenderRate;
				}
				if (Input.GetKeyDown(this.increase))
				{
					this.targetRenderRate += this.targetRenderRateStep;
					Application.targetFrameRate = this.targetRenderRate;
				}
				if (Input.GetKeyDown(this.resetRate))
				{
					this.ResetRender();
				}
			}
			if (Input.GetKey(this.unlockPhysics))
			{
				if (Input.GetKeyDown(this.decrease) && this.fixedPhysicsRate > this.fixedPhysicsRateStep)
				{
					this.fixedPhysicsRate -= this.fixedPhysicsRateStep;
					Time.fixedDeltaTime = 1f / (float)this.fixedPhysicsRate;
				}
				if (Input.GetKeyDown(this.increase))
				{
					this.fixedPhysicsRate += this.fixedPhysicsRateStep;
					Time.fixedDeltaTime = 1f / (float)this.fixedPhysicsRate;
				}
				if (Input.GetKeyDown(this.resetRate))
				{
					this.ResetPhysics();
				}
			}
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000EFDC5 File Offset: 0x000EE1C5
		public void ResetRender()
		{
			this.targetRenderRate = 60;
			Application.targetFrameRate = -1;
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x000EFDD5 File Offset: 0x000EE1D5
		public void ResetPhysics()
		{
			this.fixedPhysicsRate = 50;
			Time.fixedDeltaTime = 0.02f;
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x000EFDE9 File Offset: 0x000EE1E9
		public void ResetAll()
		{
			this.ResetRender();
			this.ResetPhysics();
		}

		// Token: 0x040023A9 RID: 9129
		public int targetRenderRate = 60;

		// Token: 0x040023AA RID: 9130
		public int targetRenderRateStep = 1;

		// Token: 0x040023AB RID: 9131
		public int fixedPhysicsRate = 50;

		// Token: 0x040023AC RID: 9132
		public int fixedPhysicsRateStep = 1;

		// Token: 0x040023AD RID: 9133
		public KeyCode unlockRender = KeyCode.RightShift;

		// Token: 0x040023AE RID: 9134
		public KeyCode unlockPhysics = KeyCode.LeftShift;

		// Token: 0x040023AF RID: 9135
		public KeyCode decrease = KeyCode.DownArrow;

		// Token: 0x040023B0 RID: 9136
		public KeyCode increase = KeyCode.UpArrow;

		// Token: 0x040023B1 RID: 9137
		public KeyCode resetRate = KeyCode.Backspace;
	}
}
