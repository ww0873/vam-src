using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x0200049F RID: 1183
	public class testHref : MonoBehaviour
	{
		// Token: 0x06001DD9 RID: 7641 RVA: 0x000AB4F6 File Offset: 0x000A98F6
		public testHref()
		{
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x000AB4FE File Offset: 0x000A98FE
		private void Awake()
		{
			this.textPic = base.GetComponent<TextPic>();
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x000AB50C File Offset: 0x000A990C
		private void OnEnable()
		{
			this.textPic.onHrefClick.AddListener(new UnityAction<string>(this.OnHrefClick));
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x000AB52A File Offset: 0x000A992A
		private void OnDisable()
		{
			this.textPic.onHrefClick.RemoveListener(new UnityAction<string>(this.OnHrefClick));
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x000AB548 File Offset: 0x000A9948
		private void OnHrefClick(string hrefName)
		{
			Debug.Log("Click on the " + hrefName);
		}

		// Token: 0x04001944 RID: 6468
		public TextPic textPic;
	}
}
