using System;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x0200059E RID: 1438
	public class CoinPickup : MonoBehaviour
	{
		// Token: 0x06002416 RID: 9238 RVA: 0x000D0D4A File Offset: 0x000CF14A
		public CoinPickup()
		{
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x000D0D5D File Offset: 0x000CF15D
		public void Start()
		{
			this.coinVis = base.transform.Find("Vis");
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x000D0D75 File Offset: 0x000CF175
		public void Update()
		{
			this.coinVis.transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * this.spinSpeed, Vector3.up);
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x000D0DA8 File Offset: 0x000CF1A8
		public void OnTriggerEnter(Collider other)
		{
			PlayerInventory component = other.GetComponent<PlayerInventory>();
			if (!component)
			{
				return;
			}
			if (this.isMassive)
			{
				HUDManager.Instance.LoadBrowseLevel(false);
			}
			else
			{
				component.AddCoin();
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04001E64 RID: 7780
		private Transform coinVis;

		// Token: 0x04001E65 RID: 7781
		public float spinSpeed = 20f;

		// Token: 0x04001E66 RID: 7782
		public bool isMassive;
	}
}
