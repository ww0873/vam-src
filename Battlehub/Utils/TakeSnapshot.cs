using System;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.Utils
{
	// Token: 0x020000A5 RID: 165
	public class TakeSnapshot : MonoBehaviour
	{
		// Token: 0x06000275 RID: 629 RVA: 0x0001213C File Offset: 0x0001053C
		public TakeSnapshot()
		{
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0001215E File Offset: 0x0001055E
		private void Start()
		{
			if (this.TargetPrefab == null)
			{
				return;
			}
			this.Run();
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00012179 File Offset: 0x00010579
		private void OnDestroy()
		{
			if (this.m_texture != null)
			{
				UnityEngine.Object.Destroy(this.m_texture);
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00012198 File Offset: 0x00010598
		public Sprite Run()
		{
			GameObject gameObject;
			if (this.CameraPrefab != null)
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this.CameraPrefab);
			}
			else
			{
				gameObject = new GameObject();
			}
			if (!gameObject.GetComponent<Camera>())
			{
				Camera camera = gameObject.AddComponent<Camera>();
				camera.orthographic = true;
				camera.orthographicSize = 1f;
			}
			ObjectToTexture objectToTexture = gameObject.GetComponent<ObjectToTexture>();
			if (objectToTexture == null)
			{
				objectToTexture = gameObject.AddComponent<ObjectToTexture>();
			}
			objectToTexture.defaultScale = this.Scale;
			if (this.m_texture != null)
			{
				UnityEngine.Object.Destroy(this.m_texture);
			}
			this.m_texture = objectToTexture.TakeObjectSnapshot(this.TargetPrefab, this.FallbackPrefab);
			Sprite sprite = null;
			if (this.m_texture != null)
			{
				sprite = Sprite.Create(this.m_texture, new Rect(0f, 0f, (float)this.m_texture.width, (float)this.m_texture.height), new Vector2(0.5f, 0.5f));
				if (this.TargetImage != null)
				{
					this.TargetImage.sprite = sprite;
				}
			}
			UnityEngine.Object.Destroy(gameObject);
			return sprite;
		}

		// Token: 0x0400035C RID: 860
		public GameObject CameraPrefab;

		// Token: 0x0400035D RID: 861
		public GameObject TargetPrefab;

		// Token: 0x0400035E RID: 862
		public GameObject FallbackPrefab;

		// Token: 0x0400035F RID: 863
		public Vector3 Scale = new Vector3(0.9f, 0.9f, 0.9f);

		// Token: 0x04000360 RID: 864
		public Image TargetImage;

		// Token: 0x04000361 RID: 865
		private Texture2D m_texture;
	}
}
