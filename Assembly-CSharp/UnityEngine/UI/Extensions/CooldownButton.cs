using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004C9 RID: 1225
	[AddComponentMenu("UI/Extensions/Cooldown Button")]
	public class CooldownButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x06001EE7 RID: 7911 RVA: 0x000B0393 File Offset: 0x000AE793
		public CooldownButton()
		{
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001EE8 RID: 7912 RVA: 0x000B03A6 File Offset: 0x000AE7A6
		// (set) Token: 0x06001EE9 RID: 7913 RVA: 0x000B03AE File Offset: 0x000AE7AE
		public float CooldownTimeout
		{
			get
			{
				return this.cooldownTimeout;
			}
			set
			{
				this.cooldownTimeout = value;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001EEA RID: 7914 RVA: 0x000B03B7 File Offset: 0x000AE7B7
		// (set) Token: 0x06001EEB RID: 7915 RVA: 0x000B03BF File Offset: 0x000AE7BF
		public float CooldownSpeed
		{
			get
			{
				return this.cooldownSpeed;
			}
			set
			{
				this.cooldownSpeed = value;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06001EEC RID: 7916 RVA: 0x000B03C8 File Offset: 0x000AE7C8
		public bool CooldownInEffect
		{
			get
			{
				return this.cooldownInEffect;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06001EED RID: 7917 RVA: 0x000B03D0 File Offset: 0x000AE7D0
		// (set) Token: 0x06001EEE RID: 7918 RVA: 0x000B03D8 File Offset: 0x000AE7D8
		public bool CooldownActive
		{
			get
			{
				return this.cooldownActive;
			}
			set
			{
				this.cooldownActive = value;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06001EEF RID: 7919 RVA: 0x000B03E1 File Offset: 0x000AE7E1
		// (set) Token: 0x06001EF0 RID: 7920 RVA: 0x000B03E9 File Offset: 0x000AE7E9
		public float CooldownTimeElapsed
		{
			get
			{
				return this.cooldownTimeElapsed;
			}
			set
			{
				this.cooldownTimeElapsed = value;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06001EF1 RID: 7921 RVA: 0x000B03F2 File Offset: 0x000AE7F2
		public float CooldownTimeRemaining
		{
			get
			{
				return this.cooldownTimeRemaining;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06001EF2 RID: 7922 RVA: 0x000B03FA File Offset: 0x000AE7FA
		public int CooldownPercentRemaining
		{
			get
			{
				return this.cooldownPercentRemaining;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001EF3 RID: 7923 RVA: 0x000B0402 File Offset: 0x000AE802
		public int CooldownPercentComplete
		{
			get
			{
				return this.cooldownPercentComplete;
			}
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x000B040C File Offset: 0x000AE80C
		private void Update()
		{
			if (this.CooldownActive)
			{
				this.cooldownTimeRemaining -= Time.deltaTime * this.cooldownSpeed;
				this.cooldownTimeElapsed = this.CooldownTimeout - this.CooldownTimeRemaining;
				if (this.cooldownTimeRemaining < 0f)
				{
					this.StopCooldown();
				}
				else
				{
					this.cooldownPercentRemaining = (int)(100f * this.cooldownTimeRemaining * this.CooldownTimeout / 100f);
					this.cooldownPercentComplete = (int)((this.CooldownTimeout - this.cooldownTimeRemaining) / this.CooldownTimeout * 100f);
				}
			}
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x000B04AC File Offset: 0x000AE8AC
		public void PauseCooldown()
		{
			if (this.CooldownInEffect)
			{
				this.CooldownActive = false;
			}
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x000B04C0 File Offset: 0x000AE8C0
		public void RestartCooldown()
		{
			if (this.CooldownInEffect)
			{
				this.CooldownActive = true;
			}
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x000B04D4 File Offset: 0x000AE8D4
		public void StopCooldown()
		{
			this.cooldownTimeElapsed = this.CooldownTimeout;
			this.cooldownTimeRemaining = 0f;
			this.cooldownPercentRemaining = 0;
			this.cooldownPercentComplete = 100;
			this.cooldownActive = (this.cooldownInEffect = false);
			if (this.OnCoolDownFinish != null)
			{
				this.OnCoolDownFinish.Invoke(this.buttonSource.button);
			}
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x000B0538 File Offset: 0x000AE938
		public void CancelCooldown()
		{
			this.cooldownActive = (this.cooldownInEffect = false);
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x000B0558 File Offset: 0x000AE958
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			this.buttonSource = eventData;
			if (this.CooldownInEffect && this.OnButtonClickDuringCooldown != null)
			{
				this.OnButtonClickDuringCooldown.Invoke(eventData.button);
			}
			if (!this.CooldownInEffect)
			{
				if (this.OnCooldownStart != null)
				{
					this.OnCooldownStart.Invoke(eventData.button);
				}
				this.cooldownTimeRemaining = this.cooldownTimeout;
				this.cooldownActive = (this.cooldownInEffect = true);
			}
		}

		// Token: 0x04001A20 RID: 6688
		[SerializeField]
		private float cooldownTimeout;

		// Token: 0x04001A21 RID: 6689
		[SerializeField]
		private float cooldownSpeed = 1f;

		// Token: 0x04001A22 RID: 6690
		[SerializeField]
		[ReadOnly]
		private bool cooldownActive;

		// Token: 0x04001A23 RID: 6691
		[SerializeField]
		[ReadOnly]
		private bool cooldownInEffect;

		// Token: 0x04001A24 RID: 6692
		[SerializeField]
		[ReadOnly]
		private float cooldownTimeElapsed;

		// Token: 0x04001A25 RID: 6693
		[SerializeField]
		[ReadOnly]
		private float cooldownTimeRemaining;

		// Token: 0x04001A26 RID: 6694
		[SerializeField]
		[ReadOnly]
		private int cooldownPercentRemaining;

		// Token: 0x04001A27 RID: 6695
		[SerializeField]
		[ReadOnly]
		private int cooldownPercentComplete;

		// Token: 0x04001A28 RID: 6696
		private PointerEventData buttonSource;

		// Token: 0x04001A29 RID: 6697
		[Tooltip("Event that fires when a button is initially pressed down")]
		public CooldownButton.CooldownButtonEvent OnCooldownStart;

		// Token: 0x04001A2A RID: 6698
		[Tooltip("Event that fires when a button is released")]
		public CooldownButton.CooldownButtonEvent OnButtonClickDuringCooldown;

		// Token: 0x04001A2B RID: 6699
		[Tooltip("Event that continually fires while a button is held down")]
		public CooldownButton.CooldownButtonEvent OnCoolDownFinish;

		// Token: 0x020004CA RID: 1226
		[Serializable]
		public class CooldownButtonEvent : UnityEvent<PointerEventData.InputButton>
		{
			// Token: 0x06001EFA RID: 7930 RVA: 0x000B05D6 File Offset: 0x000AE9D6
			public CooldownButtonEvent()
			{
			}
		}
	}
}
