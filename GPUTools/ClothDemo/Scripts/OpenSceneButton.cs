using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GPUTools.ClothDemo.Scripts
{
	// Token: 0x020009E0 RID: 2528
	public class OpenSceneButton : MonoBehaviour
	{
		// Token: 0x06003FB3 RID: 16307 RVA: 0x0013018B File Offset: 0x0012E58B
		public OpenSceneButton()
		{
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x00130193 File Offset: 0x0012E593
		public void Start()
		{
			base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x001301B1 File Offset: 0x0012E5B1
		private void OnClick()
		{
			SceneManager.LoadScene(this.sceneName);
		}

		// Token: 0x0400302A RID: 12330
		[SerializeField]
		private string sceneName;
	}
}
