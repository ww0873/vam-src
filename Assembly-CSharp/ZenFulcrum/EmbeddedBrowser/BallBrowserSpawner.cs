using System;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x0200059D RID: 1437
	[RequireComponent(typeof(Browser))]
	public class BallBrowserSpawner : MonoBehaviour, INewWindowHandler
	{
		// Token: 0x06002413 RID: 9235 RVA: 0x000D0CA7 File Offset: 0x000CF0A7
		public BallBrowserSpawner()
		{
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x000D0CAF File Offset: 0x000CF0AF
		public void Start()
		{
			base.GetComponent<Browser>().SetNewWindowHandler(Browser.NewWindowAction.NewBrowser, this);
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x000D0CC0 File Offset: 0x000CF0C0
		public Browser CreateBrowser(Browser parent)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.AddComponent<Rigidbody>();
			gameObject.transform.localScale = new Vector3(this.size, this.size, this.size);
			gameObject.transform.position = this.spawnPosition.position + Vector3.one * UnityEngine.Random.value * 0.01f;
			Browser browser = gameObject.AddComponent<Browser>();
			browser.UIHandler = null;
			browser.Resize(110, 110);
			return browser;
		}

		// Token: 0x04001E62 RID: 7778
		public Transform spawnPosition;

		// Token: 0x04001E63 RID: 7779
		public float size;
	}
}
