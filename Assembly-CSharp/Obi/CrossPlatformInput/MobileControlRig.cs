using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Obi.CrossPlatformInput
{
	// Token: 0x02000379 RID: 889
	[ExecuteInEditMode]
	public class MobileControlRig : MonoBehaviour
	{
		// Token: 0x06001635 RID: 5685 RVA: 0x0007D919 File Offset: 0x0007BD19
		public MobileControlRig()
		{
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x0007D921 File Offset: 0x0007BD21
		private void OnEnable()
		{
			this.CheckEnableControlRig();
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x0007D92C File Offset: 0x0007BD2C
		private void Start()
		{
			EventSystem x = UnityEngine.Object.FindObjectOfType<EventSystem>();
			if (x == null)
			{
				GameObject gameObject = new GameObject("EventSystem");
				gameObject.AddComponent<EventSystem>();
				gameObject.AddComponent<StandaloneInputModule>();
			}
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x0007D964 File Offset: 0x0007BD64
		private void CheckEnableControlRig()
		{
			this.EnableControlRig(false);
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x0007D970 File Offset: 0x0007BD70
		private void EnableControlRig(bool enabled)
		{
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(enabled);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}
}
