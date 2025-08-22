using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000487 RID: 1159
	[RequireComponent(typeof(SoftMaskScript))]
	public class CooldownEffect_SAUIM : MonoBehaviour
	{
		// Token: 0x06001D8C RID: 7564 RVA: 0x000AA3DD File Offset: 0x000A87DD
		public CooldownEffect_SAUIM()
		{
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x000AA3E5 File Offset: 0x000A87E5
		private void Start()
		{
			if (this.cooldown == null)
			{
				Debug.LogError("Missing Cooldown Button assignment");
			}
			this.sauim = base.GetComponent<SoftMaskScript>();
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x000AA40E File Offset: 0x000A880E
		private void Update()
		{
			this.sauim.CutOff = Mathf.Lerp(0f, 1f, this.cooldown.CooldownTimeElapsed / this.cooldown.CooldownTimeout);
		}

		// Token: 0x04001904 RID: 6404
		public CooldownButton cooldown;

		// Token: 0x04001905 RID: 6405
		private SoftMaskScript sauim;
	}
}
