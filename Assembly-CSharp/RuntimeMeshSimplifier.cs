using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UltimateGameTools.MeshSimplifier;
using UnityEngine;

// Token: 0x0200047C RID: 1148
[RequireComponent(typeof(MeshSimplify))]
public class RuntimeMeshSimplifier : MonoBehaviour
{
	// Token: 0x06001D2E RID: 7470 RVA: 0x000A6E1B File Offset: 0x000A521B
	public RuntimeMeshSimplifier()
	{
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x06001D2F RID: 7471 RVA: 0x000A6E40 File Offset: 0x000A5240
	public string ProgressTitle
	{
		get
		{
			return this.m_strLastTitle;
		}
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x06001D30 RID: 7472 RVA: 0x000A6E48 File Offset: 0x000A5248
	public string ProgressMessage
	{
		get
		{
			return this.m_strLastMessage;
		}
	}

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06001D31 RID: 7473 RVA: 0x000A6E50 File Offset: 0x000A5250
	public int ProgressPercent
	{
		get
		{
			return this.m_nLastProgress;
		}
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06001D32 RID: 7474 RVA: 0x000A6E58 File Offset: 0x000A5258
	public bool Finished
	{
		get
		{
			return this.m_bFinished;
		}
	}

	// Token: 0x06001D33 RID: 7475 RVA: 0x000A6E60 File Offset: 0x000A5260
	public void Simplify(float percent)
	{
		if (!this.m_bFinished)
		{
			base.StartCoroutine(this.ComputeMeshWithVertices(Mathf.Clamp01(percent / 100f)));
		}
	}

	// Token: 0x06001D34 RID: 7476 RVA: 0x000A6E86 File Offset: 0x000A5286
	private void Awake()
	{
		this.m_selectedMeshSimplify = base.GetComponent<MeshSimplify>();
		this.m_objectMaterials = new Dictionary<GameObject, Material[]>();
		this.AddMaterials(this.m_selectedMeshSimplify.gameObject, this.m_objectMaterials);
		this.m_bFinished = false;
	}

	// Token: 0x06001D35 RID: 7477 RVA: 0x000A6EC0 File Offset: 0x000A52C0
	private void AddMaterials(GameObject theGameObject, Dictionary<GameObject, Material[]> dicMaterials)
	{
		Renderer component = theGameObject.GetComponent<Renderer>();
		if (component != null && component.sharedMaterials != null && (MeshSimplify.HasValidMeshData(theGameObject) || theGameObject.GetComponent<MeshSimplify>() != null))
		{
			dicMaterials.Add(theGameObject, component.sharedMaterials);
		}
		if (this.m_selectedMeshSimplify.RecurseIntoChildren)
		{
			for (int i = 0; i < theGameObject.transform.childCount; i++)
			{
				this.AddMaterials(theGameObject.transform.GetChild(i).gameObject, dicMaterials);
			}
		}
	}

	// Token: 0x06001D36 RID: 7478 RVA: 0x000A6F58 File Offset: 0x000A5358
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

	// Token: 0x06001D37 RID: 7479 RVA: 0x000A6FB8 File Offset: 0x000A53B8
	private IEnumerator ComputeMeshWithVertices(float fAmount)
	{
		Simplifier.CoroutineFrameMiliseconds = 20;
		foreach (KeyValuePair<GameObject, Material[]> pair in this.m_objectMaterials)
		{
			MeshSimplify meshSimplify = pair.Key.GetComponent<MeshSimplify>();
			MeshFilter meshFilter = pair.Key.GetComponent<MeshFilter>();
			SkinnedMeshRenderer skin = pair.Key.GetComponent<SkinnedMeshRenderer>();
			if (meshSimplify == null)
			{
				meshSimplify = pair.Key.AddComponent<MeshSimplify>();
				meshSimplify.m_meshSimplifyRoot = this.m_selectedMeshSimplify;
				this.m_selectedMeshSimplify.m_listDependentChildren.Add(meshSimplify);
			}
			if (meshSimplify.MeshSimplifier == null)
			{
				meshSimplify.MeshSimplifier = meshSimplify.gameObject.AddComponent<Simplifier>();
				meshSimplify.MeshSimplifier.hideFlags = HideFlags.HideInInspector;
				meshSimplify.ConfigureSimplifier();
			}
			if (meshSimplify && MeshSimplify.HasValidMeshData(pair.Key))
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
				if (!meshSimplify.HasData())
				{
					meshSimplify.GetMeshSimplifier().CoroutineEnded = false;
					base.StartCoroutine(meshSimplify.GetMeshSimplifier().ProgressiveMesh(pair.Key, (!(meshFilter != null)) ? skin.sharedMesh : meshFilter.sharedMesh, null, meshSimplify.name, new Simplifier.ProgressDelegate(this.Progress)));
					while (!meshSimplify.GetMeshSimplifier().CoroutineEnded)
					{
						yield return null;
					}
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
		this.m_bFinished = true;
		yield break;
	}

	// Token: 0x040018CC RID: 6348
	private Dictionary<GameObject, Material[]> m_objectMaterials;

	// Token: 0x040018CD RID: 6349
	private MeshSimplify m_selectedMeshSimplify;

	// Token: 0x040018CE RID: 6350
	private bool m_bFinished;

	// Token: 0x040018CF RID: 6351
	private Mesh m_newMesh;

	// Token: 0x040018D0 RID: 6352
	private int m_nLastProgress = -1;

	// Token: 0x040018D1 RID: 6353
	private string m_strLastTitle = string.Empty;

	// Token: 0x040018D2 RID: 6354
	private string m_strLastMessage = string.Empty;

	// Token: 0x02000F69 RID: 3945
	[CompilerGenerated]
	private sealed class <ComputeMeshWithVertices>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060073B6 RID: 29622 RVA: 0x000A6FDA File Offset: 0x000A53DA
		[DebuggerHidden]
		public <ComputeMeshWithVertices>c__Iterator0()
		{
		}

		// Token: 0x060073B7 RID: 29623 RVA: 0x000A6FE4 File Offset: 0x000A53E4
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			bool flag = false;
			switch (num)
			{
			case 0U:
				Simplifier.CoroutineFrameMiliseconds = 20;
				enumerator = this.m_objectMaterials.GetEnumerator();
				num = 4294967293U;
				break;
			case 1U:
			case 2U:
				break;
			default:
				return false;
			}
			try
			{
				switch (num)
				{
				case 1U:
					IL_286:
					if (meshSimplify.GetMeshSimplifier().CoroutineEnded)
					{
						goto IL_29B;
					}
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					flag = true;
					return true;
				case 2U:
					IL_34A:
					if (!meshSimplify.GetMeshSimplifier().CoroutineEnded)
					{
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 2;
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
				IL_3B9:
				while (enumerator.MoveNext())
				{
					pair = enumerator.Current;
					meshSimplify = pair.Key.GetComponent<MeshSimplify>();
					meshFilter = pair.Key.GetComponent<MeshFilter>();
					skin = pair.Key.GetComponent<SkinnedMeshRenderer>();
					if (meshSimplify == null)
					{
						meshSimplify = pair.Key.AddComponent<MeshSimplify>();
						meshSimplify.m_meshSimplifyRoot = this.m_selectedMeshSimplify;
						this.m_selectedMeshSimplify.m_listDependentChildren.Add(meshSimplify);
					}
					if (meshSimplify.MeshSimplifier == null)
					{
						meshSimplify.MeshSimplifier = meshSimplify.gameObject.AddComponent<Simplifier>();
						meshSimplify.MeshSimplifier.hideFlags = HideFlags.HideInInspector;
						meshSimplify.ConfigureSimplifier();
					}
					if (meshSimplify && MeshSimplify.HasValidMeshData(pair.Key))
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
						if (!meshSimplify.HasData())
						{
							meshSimplify.GetMeshSimplifier().CoroutineEnded = false;
							base.StartCoroutine(meshSimplify.GetMeshSimplifier().ProgressiveMesh(pair.Key, (!(meshFilter != null)) ? skin.sharedMesh : meshFilter.sharedMesh, null, meshSimplify.name, new Simplifier.ProgressDelegate(base.Progress)));
							goto IL_286;
						}
						goto IL_29B;
					}
				}
				goto IL_3E4;
				IL_29B:
				if (meshSimplify.GetMeshSimplifier() != null)
				{
					meshSimplify.GetMeshSimplifier().CoroutineEnded = false;
					base.StartCoroutine(meshSimplify.GetMeshSimplifier().ComputeMeshWithVertexCount(pair.Key, newMesh, Mathf.RoundToInt(fAmount * (float)meshSimplify.GetMeshSimplifier().GetOriginalMeshUniqueVertexCount()), meshSimplify.name, new Simplifier.ProgressDelegate(base.Progress)));
					goto IL_34A;
				}
				goto IL_3B9;
			}
			finally
			{
				if (!flag)
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			IL_3E4:
			this.m_bFinished = true;
			this.$PC = -1;
			return false;
		}

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x060073B8 RID: 29624 RVA: 0x000A7408 File Offset: 0x000A5808
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x060073B9 RID: 29625 RVA: 0x000A7410 File Offset: 0x000A5810
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060073BA RID: 29626 RVA: 0x000A7418 File Offset: 0x000A5818
		[DebuggerHidden]
		public void Dispose()
		{
			uint num = (uint)this.$PC;
			this.$disposing = true;
			this.$PC = -1;
			switch (num)
			{
			case 1U:
			case 2U:
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

		// Token: 0x060073BB RID: 29627 RVA: 0x000A7478 File Offset: 0x000A5878
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400679E RID: 26526
		internal Dictionary<GameObject, Material[]>.Enumerator $locvar0;

		// Token: 0x0400679F RID: 26527
		internal KeyValuePair<GameObject, Material[]> <pair>__1;

		// Token: 0x040067A0 RID: 26528
		internal MeshSimplify <meshSimplify>__2;

		// Token: 0x040067A1 RID: 26529
		internal MeshFilter <meshFilter>__2;

		// Token: 0x040067A2 RID: 26530
		internal SkinnedMeshRenderer <skin>__2;

		// Token: 0x040067A3 RID: 26531
		internal Mesh <newMesh>__3;

		// Token: 0x040067A4 RID: 26532
		internal float fAmount;

		// Token: 0x040067A5 RID: 26533
		internal RuntimeMeshSimplifier $this;

		// Token: 0x040067A6 RID: 26534
		internal object $current;

		// Token: 0x040067A7 RID: 26535
		internal bool $disposing;

		// Token: 0x040067A8 RID: 26536
		internal int $PC;
	}
}
