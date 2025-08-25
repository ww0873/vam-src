using System;
using UnityEngine;

namespace Battlehub.Utils
{
	// Token: 0x020000A4 RID: 164
	public class ObjectToTexture : MonoBehaviour
	{
		// Token: 0x0600026E RID: 622 RVA: 0x00011D70 File Offset: 0x00010170
		public ObjectToTexture()
		{
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00011DEE File Offset: 0x000101EE
		private void Awake()
		{
			if (this.Camera == null)
			{
				this.Camera = base.GetComponent<Camera>();
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00011E0D File Offset: 0x0001020D
		private void Start()
		{
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00011E10 File Offset: 0x00010210
		private void SetLayerRecursively(GameObject o, int layer)
		{
			foreach (Transform transform in o.GetComponentsInChildren<Transform>(true))
			{
				transform.gameObject.layer = layer;
			}
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00011E49 File Offset: 0x00010249
		public Texture2D TakeObjectSnapshot(GameObject prefab, GameObject fallback)
		{
			return this.TakeObjectSnapshot(prefab, fallback, this.defaultPosition, Quaternion.Euler(this.defaultRotation), this.defaultScale);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00011E6A File Offset: 0x0001026A
		public Texture2D TakeObjectSnapshot(GameObject prefab, GameObject fallback, Vector3 position)
		{
			return this.TakeObjectSnapshot(prefab, fallback, position, Quaternion.Euler(this.defaultRotation), this.defaultScale);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00011E88 File Offset: 0x00010288
		public Texture2D TakeObjectSnapshot(GameObject prefab, GameObject fallback, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			if (this.Camera == null)
			{
				throw new InvalidOperationException("Object Image Camera must be set");
			}
			if (this.objectImageLayer < 0 || this.objectImageLayer > 31)
			{
				throw new InvalidOperationException("Object Image Layer must specify a valid layer between 0 and 31");
			}
			bool activeSelf = prefab.activeSelf;
			prefab.SetActive(false);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, position, rotation * Quaternion.Inverse(prefab.transform.rotation));
			if (this.DestroyScripts)
			{
				MonoBehaviour[] componentsInChildren = gameObject.GetComponentsInChildren<MonoBehaviour>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					UnityEngine.Object.DestroyImmediate(componentsInChildren[i]);
				}
			}
			prefab.SetActive(activeSelf);
			Renderer[] array = gameObject.GetComponentsInChildren<Renderer>(true);
			if (array.Length == 0 && fallback)
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
				gameObject = UnityEngine.Object.Instantiate<GameObject>(fallback, position, rotation);
				array = new Renderer[]
				{
					fallback.GetComponentInChildren<Renderer>(true)
				};
			}
			Bounds bounds = gameObject.CalculateBounds();
			float num = this.Camera.fieldOfView * 0.017453292f;
			float num2 = Mathf.Max(new float[]
			{
				bounds.extents.y,
				bounds.extents.x,
				bounds.extents.z
			});
			float d = Mathf.Abs(num2 / Mathf.Sin(num / 2f));
			gameObject.SetActive(true);
			for (int j = 0; j < array.Length; j++)
			{
				array[j].gameObject.SetActive(true);
			}
			position += bounds.center;
			this.Camera.transform.position = position - d * this.Camera.transform.forward;
			this.Camera.orthographicSize = num2;
			this.SetLayerRecursively(gameObject, this.objectImageLayer);
			this.Camera.targetTexture = RenderTexture.GetTemporary(this.snapshotTextureWidth, this.snapshotTextureHeight, 24);
			this.Camera.Render();
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = this.Camera.targetTexture;
			Texture2D texture2D = new Texture2D(this.Camera.targetTexture.width, this.Camera.targetTexture.height);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)this.Camera.targetTexture.width, (float)this.Camera.targetTexture.height), 0, 0);
			texture2D.Apply();
			RenderTexture.active = active;
			RenderTexture.ReleaseTemporary(this.Camera.targetTexture);
			UnityEngine.Object.DestroyImmediate(gameObject);
			return texture2D;
		}

		// Token: 0x04000354 RID: 852
		public Camera Camera;

		// Token: 0x04000355 RID: 853
		[HideInInspector]
		public int objectImageLayer;

		// Token: 0x04000356 RID: 854
		public bool DestroyScripts = true;

		// Token: 0x04000357 RID: 855
		public int snapshotTextureWidth = 256;

		// Token: 0x04000358 RID: 856
		public int snapshotTextureHeight = 256;

		// Token: 0x04000359 RID: 857
		public Vector3 defaultPosition = new Vector3(0f, 0f, 1f);

		// Token: 0x0400035A RID: 858
		public Vector3 defaultRotation = new Vector3(345.8529f, 0f, 14.28433f);

		// Token: 0x0400035B RID: 859
		public Vector3 defaultScale = new Vector3(1f, 1f, 1f);
	}
}
