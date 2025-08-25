using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000977 RID: 2423
public class OVRScreenFade : MonoBehaviour
{
	// Token: 0x06003C74 RID: 15476 RVA: 0x00124E0C File Offset: 0x0012320C
	public OVRScreenFade()
	{
	}

	// Token: 0x170006B2 RID: 1714
	// (get) Token: 0x06003C75 RID: 15477 RVA: 0x00124E5B File Offset: 0x0012325B
	// (set) Token: 0x06003C76 RID: 15478 RVA: 0x00124E63 File Offset: 0x00123263
	public float currentAlpha
	{
		[CompilerGenerated]
		get
		{
			return this.<currentAlpha>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<currentAlpha>k__BackingField = value;
		}
	}

	// Token: 0x06003C77 RID: 15479 RVA: 0x00124E6C File Offset: 0x0012326C
	private void Awake()
	{
		this.fadeMaterial = new Material(Shader.Find("Oculus/Unlit Transparent Color"));
		this.fadeMesh = base.gameObject.AddComponent<MeshFilter>();
		this.fadeRenderer = base.gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		this.fadeMesh.mesh = mesh;
		Vector3[] array = new Vector3[4];
		float num = 2f;
		float num2 = 2f;
		float z = 1f;
		array[0] = new Vector3(-num, -num2, z);
		array[1] = new Vector3(num, -num2, z);
		array[2] = new Vector3(-num, num2, z);
		array[3] = new Vector3(num, num2, z);
		mesh.vertices = array;
		mesh.triangles = new int[]
		{
			0,
			2,
			1,
			2,
			3,
			1
		};
		mesh.normals = new Vector3[]
		{
			-Vector3.forward,
			-Vector3.forward,
			-Vector3.forward,
			-Vector3.forward
		};
		mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		this.SetFadeLevel(0f);
	}

	// Token: 0x06003C78 RID: 15480 RVA: 0x00125060 File Offset: 0x00123460
	public void FadeOut()
	{
		base.StartCoroutine(this.Fade(0f, 1f));
	}

	// Token: 0x06003C79 RID: 15481 RVA: 0x00125079 File Offset: 0x00123479
	private void OnLevelFinishedLoading(int level)
	{
		base.StartCoroutine(this.Fade(1f, 0f));
	}

	// Token: 0x06003C7A RID: 15482 RVA: 0x00125092 File Offset: 0x00123492
	private void Start()
	{
		if (this.fadeOnStart)
		{
			base.StartCoroutine(this.Fade(1f, 0f));
		}
	}

	// Token: 0x06003C7B RID: 15483 RVA: 0x001250B6 File Offset: 0x001234B6
	private void OnEnable()
	{
		if (!this.fadeOnStart)
		{
			this.SetFadeLevel(0f);
		}
	}

	// Token: 0x06003C7C RID: 15484 RVA: 0x001250D0 File Offset: 0x001234D0
	private void OnDestroy()
	{
		if (this.fadeRenderer != null)
		{
			UnityEngine.Object.Destroy(this.fadeRenderer);
		}
		if (this.fadeMaterial != null)
		{
			UnityEngine.Object.Destroy(this.fadeMaterial);
		}
		if (this.fadeMesh != null)
		{
			UnityEngine.Object.Destroy(this.fadeMesh);
		}
	}

	// Token: 0x06003C7D RID: 15485 RVA: 0x00125131 File Offset: 0x00123531
	public void SetUIFade(float level)
	{
		this.uiFadeAlpha = Mathf.Clamp01(level);
		this.SetMaterialAlpha();
	}

	// Token: 0x06003C7E RID: 15486 RVA: 0x00125145 File Offset: 0x00123545
	public void SetFadeLevel(float level)
	{
		this.currentAlpha = level;
		this.SetMaterialAlpha();
	}

	// Token: 0x06003C7F RID: 15487 RVA: 0x00125154 File Offset: 0x00123554
	private IEnumerator Fade(float startAlpha, float endAlpha)
	{
		float elapsedTime = 0f;
		while (elapsedTime < this.fadeTime)
		{
			elapsedTime += Time.deltaTime;
			this.currentAlpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / this.fadeTime));
			this.SetMaterialAlpha();
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x06003C80 RID: 15488 RVA: 0x00125180 File Offset: 0x00123580
	private void SetMaterialAlpha()
	{
		Color color = this.fadeColor;
		color.a = Mathf.Max(this.currentAlpha, this.uiFadeAlpha);
		this.isFading = (color.a > 0f);
		if (this.fadeMaterial != null)
		{
			this.fadeMaterial.color = color;
			this.fadeMaterial.renderQueue = this.renderQueue;
			this.fadeRenderer.material = this.fadeMaterial;
			this.fadeRenderer.enabled = this.isFading;
		}
	}

	// Token: 0x04002E59 RID: 11865
	[Tooltip("Fade duration")]
	public float fadeTime = 2f;

	// Token: 0x04002E5A RID: 11866
	[Tooltip("Screen color at maximum fade")]
	public Color fadeColor = new Color(0.01f, 0.01f, 0.01f, 1f);

	// Token: 0x04002E5B RID: 11867
	public bool fadeOnStart = true;

	// Token: 0x04002E5C RID: 11868
	public int renderQueue = 5000;

	// Token: 0x04002E5D RID: 11869
	private float uiFadeAlpha;

	// Token: 0x04002E5E RID: 11870
	private MeshRenderer fadeRenderer;

	// Token: 0x04002E5F RID: 11871
	private MeshFilter fadeMesh;

	// Token: 0x04002E60 RID: 11872
	private Material fadeMaterial;

	// Token: 0x04002E61 RID: 11873
	private bool isFading;

	// Token: 0x04002E62 RID: 11874
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private float <currentAlpha>k__BackingField;

	// Token: 0x02000FBA RID: 4026
	[CompilerGenerated]
	private sealed class <Fade>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060074F2 RID: 29938 RVA: 0x00125210 File Offset: 0x00123610
		[DebuggerHidden]
		public <Fade>c__Iterator0()
		{
		}

		// Token: 0x060074F3 RID: 29939 RVA: 0x00125218 File Offset: 0x00123618
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				elapsedTime = 0f;
				break;
			case 1U:
				break;
			default:
				return false;
			}
			if (elapsedTime < this.fadeTime)
			{
				elapsedTime += Time.deltaTime;
				base.currentAlpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / this.fadeTime));
				base.SetMaterialAlpha();
				this.$current = new WaitForEndOfFrame();
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			this.$PC = -1;
			return false;
		}

		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x060074F4 RID: 29940 RVA: 0x001252E5 File Offset: 0x001236E5
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x060074F5 RID: 29941 RVA: 0x001252ED File Offset: 0x001236ED
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060074F6 RID: 29942 RVA: 0x001252F5 File Offset: 0x001236F5
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060074F7 RID: 29943 RVA: 0x00125305 File Offset: 0x00123705
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006908 RID: 26888
		internal float <elapsedTime>__0;

		// Token: 0x04006909 RID: 26889
		internal float startAlpha;

		// Token: 0x0400690A RID: 26890
		internal float endAlpha;

		// Token: 0x0400690B RID: 26891
		internal OVRScreenFade $this;

		// Token: 0x0400690C RID: 26892
		internal object $current;

		// Token: 0x0400690D RID: 26893
		internal bool $disposing;

		// Token: 0x0400690E RID: 26894
		internal int $PC;
	}
}
