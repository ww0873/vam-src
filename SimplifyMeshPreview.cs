using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UltimateGameTools.MeshSimplifier;
using UnityEngine;

// Token: 0x02000473 RID: 1139
public class SimplifyMeshPreview : MonoBehaviour
{
	// Token: 0x06001CDE RID: 7390 RVA: 0x000A4A9C File Offset: 0x000A2E9C
	public SimplifyMeshPreview()
	{
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x000A4B08 File Offset: 0x000A2F08
	private void Start()
	{
		if (this.ShowcaseObjects != null && this.ShowcaseObjects.Length > 0)
		{
			for (int i = 0; i < this.ShowcaseObjects.Length; i++)
			{
				this.ShowcaseObjects[i].m_description = this.ShowcaseObjects[i].m_description.Replace("\\n", Environment.NewLine);
			}
			this.SetActiveObject(0);
		}
		Simplifier.CoroutineFrameMiliseconds = 20;
	}

	// Token: 0x06001CE0 RID: 7392 RVA: 0x000A4B80 File Offset: 0x000A2F80
	private void Progress(string strTitle, string strMessage, float fT)
	{
		int num = Mathf.RoundToInt(fT * 100f);
		if (num != this.m_nLastProgress || this.m_strLastTitle != strTitle || this.m_strLastMessage != strMessage)
		{
			this.m_strLastTitle = strTitle;
			this.m_strLastMessage = strMessage;
			this.m_nLastProgress = num;
		}
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x000A4BE0 File Offset: 0x000A2FE0
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			this.m_bGUIEnabled = !this.m_bGUIEnabled;
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			this.m_bWireframe = !this.m_bWireframe;
			this.SetWireframe(this.m_bWireframe);
		}
		if (this.m_selectedMeshSimplify != null)
		{
			if (Input.GetMouseButton(0) && Input.mousePosition.y > 100f)
			{
				Vector3 eulerAngles = this.ShowcaseObjects[this.m_nSelectedIndex].m_rotationAxis * -((Input.mousePosition.x - this.m_fLastMouseX) * this.MouseSensitvity);
				this.m_selectedMeshSimplify.transform.Rotate(eulerAngles, Space.Self);
			}
			else if (Input.GetMouseButtonUp(0) && Input.mousePosition.y > 100f)
			{
				this.m_fRotationSpeed = -(Input.mousePosition.x - this.m_fLastMouseX) * this.MouseReleaseSpeed;
			}
			else
			{
				Vector3 eulerAngles2 = this.ShowcaseObjects[this.m_nSelectedIndex].m_rotationAxis * (this.m_fRotationSpeed * Time.deltaTime);
				this.m_selectedMeshSimplify.transform.Rotate(eulerAngles2, Space.Self);
			}
		}
		this.m_fLastMouseX = Input.mousePosition.x;
	}

	// Token: 0x06001CE2 RID: 7394 RVA: 0x000A4D48 File Offset: 0x000A3148
	private void OnGUI()
	{
		int num = 150;
		int num2 = 50;
		int num3 = 20;
		int num4 = 10;
		Rect rect = new Rect((float)(Screen.width / 2 - num / 2), 0f, (float)num, (float)num2);
		Rect screenRect = new Rect(rect.x + (float)num3, rect.y + (float)num4, (float)(num - num3 * 2), (float)(num2 - num4 * 2));
		GUILayout.BeginArea(screenRect);
		if (GUILayout.Button("Exit", new GUILayoutOption[0]))
		{
			Application.Quit();
		}
		GUILayout.EndArea();
		if (!this.m_bGUIEnabled)
		{
			return;
		}
		int num5 = 400;
		if (this.ShowcaseObjects == null)
		{
			return;
		}
		bool flag = true;
		if (!string.IsNullOrEmpty(this.m_strLastTitle) && !string.IsNullOrEmpty(this.m_strLastMessage))
		{
			flag = false;
		}
		GUI.Box(new Rect(0f, 0f, (float)(num5 + 10), 400f), string.Empty);
		GUILayout.Label("Select model:", new GUILayoutOption[]
		{
			GUILayout.Width((float)num5)
		});
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		for (int i = 0; i < this.ShowcaseObjects.Length; i++)
		{
			if (GUILayout.Button(this.ShowcaseObjects[i].m_meshSimplify.name, new GUILayoutOption[0]) && flag)
			{
				if (this.m_selectedMeshSimplify != null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_selectedMeshSimplify.gameObject);
				}
				this.SetActiveObject(i);
			}
		}
		GUILayout.EndHorizontal();
		if (this.m_selectedMeshSimplify != null)
		{
			GUILayout.Space(20f);
			GUILayout.Label(this.ShowcaseObjects[this.m_nSelectedIndex].m_description, new GUILayoutOption[0]);
			GUILayout.Space(20f);
			GUI.changed = false;
			this.m_bWireframe = GUILayout.Toggle(this.m_bWireframe, "Show wireframe", new GUILayoutOption[0]);
			if (GUI.changed && this.m_selectedMeshSimplify != null)
			{
				this.SetWireframe(this.m_bWireframe);
			}
			GUILayout.Space(20f);
			int simplifiedVertexCount = this.m_selectedMeshSimplify.GetSimplifiedVertexCount(true);
			int originalVertexCount = this.m_selectedMeshSimplify.GetOriginalVertexCount(true);
			GUILayout.Label(string.Concat(new object[]
			{
				"Vertex count: ",
				simplifiedVertexCount,
				"/",
				originalVertexCount,
				" ",
				Mathf.RoundToInt((float)simplifiedVertexCount / (float)originalVertexCount * 100f).ToString(),
				"% from original"
			}), new GUILayoutOption[0]);
			GUILayout.Space(20f);
			if (!string.IsNullOrEmpty(this.m_strLastTitle) && !string.IsNullOrEmpty(this.m_strLastMessage))
			{
				GUILayout.Label(this.m_strLastTitle + ": " + this.m_strLastMessage, new GUILayoutOption[]
				{
					GUILayout.MaxWidth((float)num5)
				});
				GUI.color = Color.blue;
				Rect lastRect = GUILayoutUtility.GetLastRect();
				GUI.Box(new Rect(10f, lastRect.yMax + 5f, 204f, 24f), string.Empty);
				GUI.Box(new Rect(12f, lastRect.yMax + 7f, (float)(this.m_nLastProgress * 2), 20f), string.Empty);
			}
			else
			{
				GUILayout.Label("Vertices: " + (this.m_fVertexAmount * 100f).ToString("0.00") + "%", new GUILayoutOption[0]);
				this.m_fVertexAmount = GUILayout.HorizontalSlider(this.m_fVertexAmount, 0f, 1f, new GUILayoutOption[]
				{
					GUILayout.Width(200f)
				});
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Space(3f);
				if (GUILayout.Button("Compute simplified mesh", new GUILayoutOption[]
				{
					GUILayout.Width(200f)
				}))
				{
					base.StartCoroutine(this.ComputeMeshWithVertices(this.m_fVertexAmount));
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
		}
	}

	// Token: 0x06001CE3 RID: 7395 RVA: 0x000A516C File Offset: 0x000A356C
	private void SetActiveObject(int index)
	{
		this.m_nSelectedIndex = index;
		MeshSimplify meshSimplify = UnityEngine.Object.Instantiate<MeshSimplify>(this.ShowcaseObjects[index].m_meshSimplify);
		meshSimplify.transform.position = this.ShowcaseObjects[index].m_position;
		meshSimplify.transform.rotation = Quaternion.Euler(this.ShowcaseObjects[index].m_angles);
		this.m_selectedMeshSimplify = meshSimplify;
		this.m_objectMaterials = new Dictionary<GameObject, Material[]>();
		this.AddMaterials(meshSimplify.gameObject, this.m_objectMaterials);
		this.m_bWireframe = false;
	}

	// Token: 0x06001CE4 RID: 7396 RVA: 0x000A51F4 File Offset: 0x000A35F4
	private void AddMaterials(GameObject theGameObject, Dictionary<GameObject, Material[]> dicMaterials)
	{
		Renderer component = theGameObject.GetComponent<Renderer>();
		MeshSimplify component2 = theGameObject.GetComponent<MeshSimplify>();
		if (component != null && component.sharedMaterials != null && component2 != null)
		{
			dicMaterials.Add(theGameObject, component.sharedMaterials);
		}
		for (int i = 0; i < theGameObject.transform.childCount; i++)
		{
			this.AddMaterials(theGameObject.transform.GetChild(i).gameObject, dicMaterials);
		}
	}

	// Token: 0x06001CE5 RID: 7397 RVA: 0x000A5274 File Offset: 0x000A3674
	private void SetWireframe(bool bEnabled)
	{
		this.m_bWireframe = bEnabled;
		foreach (KeyValuePair<GameObject, Material[]> keyValuePair in this.m_objectMaterials)
		{
			Renderer component = keyValuePair.Key.GetComponent<Renderer>();
			if (bEnabled)
			{
				Material[] array = new Material[keyValuePair.Value.Length];
				for (int i = 0; i < keyValuePair.Value.Length; i++)
				{
					array[i] = this.WireframeMaterial;
				}
				component.sharedMaterials = array;
			}
			else
			{
				component.sharedMaterials = keyValuePair.Value;
			}
		}
	}

	// Token: 0x06001CE6 RID: 7398 RVA: 0x000A5334 File Offset: 0x000A3734
	private IEnumerator ComputeMeshWithVertices(float fAmount)
	{
		foreach (KeyValuePair<GameObject, Material[]> pair in this.m_objectMaterials)
		{
			MeshSimplify meshSimplify = pair.Key.GetComponent<MeshSimplify>();
			MeshFilter meshFilter = pair.Key.GetComponent<MeshFilter>();
			SkinnedMeshRenderer skin = pair.Key.GetComponent<SkinnedMeshRenderer>();
			if (meshSimplify && (meshFilter != null || skin != null))
			{
				Mesh newMesh = null;
				if (meshFilter != null)
				{
					newMesh = UnityEngine.Object.Instantiate<Mesh>(meshFilter.sharedMesh);
				}
				else if (skin != null)
				{
					newMesh = UnityEngine.Object.Instantiate<Mesh>(skin.sharedMesh);
				}
				if (meshSimplify.GetMeshSimplifier() != null)
				{
					meshSimplify.GetMeshSimplifier().CoroutineEnded = false;
					base.StartCoroutine(meshSimplify.GetMeshSimplifier().ComputeMeshWithVertexCount(pair.Key, newMesh, Mathf.RoundToInt(fAmount * (float)meshSimplify.GetMeshSimplifier().GetOriginalMeshUniqueVertexCount()), meshSimplify.name, new Simplifier.ProgressDelegate(this.Progress)));
					while (!meshSimplify.GetMeshSimplifier().CoroutineEnded)
					{
						yield return null;
					}
					if (meshFilter != null)
					{
						meshFilter.mesh = newMesh;
					}
					else if (skin != null)
					{
						skin.sharedMesh = newMesh;
					}
					meshSimplify.m_simplifiedMesh = newMesh;
				}
			}
		}
		this.m_strLastTitle = string.Empty;
		this.m_strLastMessage = string.Empty;
		this.m_nLastProgress = 0;
		yield break;
	}

	// Token: 0x0400188A RID: 6282
	public SimplifyMeshPreview.ShowcaseObject[] ShowcaseObjects;

	// Token: 0x0400188B RID: 6283
	public Material WireframeMaterial;

	// Token: 0x0400188C RID: 6284
	public float MouseSensitvity = 0.3f;

	// Token: 0x0400188D RID: 6285
	public float MouseReleaseSpeed = 3f;

	// Token: 0x0400188E RID: 6286
	private Dictionary<GameObject, Material[]> m_objectMaterials;

	// Token: 0x0400188F RID: 6287
	private MeshSimplify m_selectedMeshSimplify;

	// Token: 0x04001890 RID: 6288
	private int m_nSelectedIndex = -1;

	// Token: 0x04001891 RID: 6289
	private bool m_bWireframe;

	// Token: 0x04001892 RID: 6290
	private float m_fRotationSpeed = 10f;

	// Token: 0x04001893 RID: 6291
	private float m_fLastMouseX;

	// Token: 0x04001894 RID: 6292
	private Mesh m_newMesh;

	// Token: 0x04001895 RID: 6293
	private int m_nLastProgress = -1;

	// Token: 0x04001896 RID: 6294
	private string m_strLastTitle = string.Empty;

	// Token: 0x04001897 RID: 6295
	private string m_strLastMessage = string.Empty;

	// Token: 0x04001898 RID: 6296
	private float m_fVertexAmount = 1f;

	// Token: 0x04001899 RID: 6297
	private bool m_bGUIEnabled = true;

	// Token: 0x02000474 RID: 1140
	[Serializable]
	public class ShowcaseObject
	{
		// Token: 0x06001CE7 RID: 7399 RVA: 0x000A5356 File Offset: 0x000A3756
		public ShowcaseObject()
		{
		}

		// Token: 0x0400189A RID: 6298
		public MeshSimplify m_meshSimplify;

		// Token: 0x0400189B RID: 6299
		public Vector3 m_position;

		// Token: 0x0400189C RID: 6300
		public Vector3 m_angles;

		// Token: 0x0400189D RID: 6301
		public Vector3 m_rotationAxis = Vector3.up;

		// Token: 0x0400189E RID: 6302
		public string m_description;
	}

	// Token: 0x02000F68 RID: 3944
	[CompilerGenerated]
	private sealed class <ComputeMeshWithVertices>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060073B0 RID: 29616 RVA: 0x000A5369 File Offset: 0x000A3769
		[DebuggerHidden]
		public <ComputeMeshWithVertices>c__Iterator0()
		{
		}

		// Token: 0x060073B1 RID: 29617 RVA: 0x000A5374 File Offset: 0x000A3774
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			bool flag = false;
			switch (num)
			{
			case 0U:
				enumerator = this.m_objectMaterials.GetEnumerator();
				num = 4294967293U;
				break;
			case 1U:
				break;
			default:
				return false;
			}
			try
			{
				switch (num)
				{
				case 1U:
					IL_1DB:
					if (!meshSimplify.GetMeshSimplifier().CoroutineEnded)
					{
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
					if (meshFilter != null)
					{
						meshFilter.mesh = newMesh;
					}
					else if (skin != null)
					{
						skin.sharedMesh = newMesh;
					}
					meshSimplify.m_simplifiedMesh = newMesh;
					break;
				}
				while (enumerator.MoveNext())
				{
					pair = enumerator.Current;
					meshSimplify = pair.Key.GetComponent<MeshSimplify>();
					meshFilter = pair.Key.GetComponent<MeshFilter>();
					skin = pair.Key.GetComponent<SkinnedMeshRenderer>();
					if (meshSimplify && (meshFilter != null || skin != null))
					{
						newMesh = null;
						if (meshFilter != null)
						{
							newMesh = UnityEngine.Object.Instantiate<Mesh>(meshFilter.sharedMesh);
						}
						else if (skin != null)
						{
							newMesh = UnityEngine.Object.Instantiate<Mesh>(skin.sharedMesh);
						}
						if (meshSimplify.GetMeshSimplifier() != null)
						{
							meshSimplify.GetMeshSimplifier().CoroutineEnded = false;
							base.StartCoroutine(meshSimplify.GetMeshSimplifier().ComputeMeshWithVertexCount(pair.Key, newMesh, Mathf.RoundToInt(fAmount * (float)meshSimplify.GetMeshSimplifier().GetOriginalMeshUniqueVertexCount()), meshSimplify.name, new Simplifier.ProgressDelegate(base.Progress)));
							goto IL_1DB;
						}
					}
				}
			}
			finally
			{
				if (!flag)
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			this.m_strLastTitle = string.Empty;
			this.m_strLastMessage = string.Empty;
			this.m_nLastProgress = 0;
			this.$PC = -1;
			return false;
		}

		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x060073B2 RID: 29618 RVA: 0x000A5648 File Offset: 0x000A3A48
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x060073B3 RID: 29619 RVA: 0x000A5650 File Offset: 0x000A3A50
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060073B4 RID: 29620 RVA: 0x000A5658 File Offset: 0x000A3A58
		[DebuggerHidden]
		public void Dispose()
		{
			uint num = (uint)this.$PC;
			this.$disposing = true;
			this.$PC = -1;
			switch (num)
			{
			case 1U:
				try
				{
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				break;
			}
		}

		// Token: 0x060073B5 RID: 29621 RVA: 0x000A56B4 File Offset: 0x000A3AB4
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006793 RID: 26515
		internal Dictionary<GameObject, Material[]>.Enumerator $locvar0;

		// Token: 0x04006794 RID: 26516
		internal KeyValuePair<GameObject, Material[]> <pair>__1;

		// Token: 0x04006795 RID: 26517
		internal MeshSimplify <meshSimplify>__2;

		// Token: 0x04006796 RID: 26518
		internal MeshFilter <meshFilter>__2;

		// Token: 0x04006797 RID: 26519
		internal SkinnedMeshRenderer <skin>__2;

		// Token: 0x04006798 RID: 26520
		internal Mesh <newMesh>__3;

		// Token: 0x04006799 RID: 26521
		internal float fAmount;

		// Token: 0x0400679A RID: 26522
		internal SimplifyMeshPreview $this;

		// Token: 0x0400679B RID: 26523
		internal object $current;

		// Token: 0x0400679C RID: 26524
		internal bool $disposing;

		// Token: 0x0400679D RID: 26525
		internal int $PC;
	}
}
