using System;
using System.Collections;
using System.Collections.Generic;
using UltimateGameTools.MeshSimplifier;
using UnityEngine;

// Token: 0x02000475 RID: 1141
public class MeshSimplify : MonoBehaviour
{
	// Token: 0x06001CE8 RID: 7400 RVA: 0x000A56BC File Offset: 0x000A3ABC
	public MeshSimplify()
	{
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x000A572F File Offset: 0x000A3B2F
	public bool RecurseIntoChildren
	{
		get
		{
			return this.m_bGenerateIncludeChildren;
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06001CEA RID: 7402 RVA: 0x000A5737 File Offset: 0x000A3B37
	// (set) Token: 0x06001CEB RID: 7403 RVA: 0x000A573F File Offset: 0x000A3B3F
	public Simplifier MeshSimplifier
	{
		get
		{
			return this.m_meshSimplifier;
		}
		set
		{
			this.m_meshSimplifier = value;
		}
	}

	// Token: 0x06001CEC RID: 7404 RVA: 0x000A5748 File Offset: 0x000A3B48
	public static bool HasValidMeshData(GameObject go)
	{
		MeshFilter component = go.GetComponent<MeshFilter>();
		if (component != null)
		{
			return true;
		}
		SkinnedMeshRenderer component2 = go.GetComponent<SkinnedMeshRenderer>();
		return component2 != null;
	}

	// Token: 0x06001CED RID: 7405 RVA: 0x000A5780 File Offset: 0x000A3B80
	public static bool IsRootOrBelongsToTree(MeshSimplify meshSimplify, MeshSimplify root)
	{
		return !(meshSimplify == null) && !meshSimplify.m_bExcludedFromTree && (meshSimplify.m_meshSimplifyRoot == null || meshSimplify.m_meshSimplifyRoot == root || meshSimplify == root || meshSimplify.m_meshSimplifyRoot == root.m_meshSimplifyRoot);
	}

	// Token: 0x06001CEE RID: 7406 RVA: 0x000A57EB File Offset: 0x000A3BEB
	public bool IsGenerateIncludeChildrenActive()
	{
		return this.m_bGenerateIncludeChildren;
	}

	// Token: 0x06001CEF RID: 7407 RVA: 0x000A57F3 File Offset: 0x000A3BF3
	public bool HasDependentChildren()
	{
		return this.m_listDependentChildren != null && this.m_listDependentChildren.Count > 0;
	}

	// Token: 0x06001CF0 RID: 7408 RVA: 0x000A5811 File Offset: 0x000A3C11
	public bool HasDataDirty()
	{
		return this.m_bDataDirty;
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x000A581C File Offset: 0x000A3C1C
	public bool SetDataDirty(bool bDirty)
	{
		this.m_bDataDirty = bDirty;
		return bDirty;
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x000A5833 File Offset: 0x000A3C33
	public bool HasNonMeshSimplifyGameObjectsInTree()
	{
		return this.HasNonMeshSimplifyGameObjectsInTreeRecursive(this, base.gameObject);
	}

	// Token: 0x06001CF3 RID: 7411 RVA: 0x000A5844 File Offset: 0x000A3C44
	private bool HasNonMeshSimplifyGameObjectsInTreeRecursive(MeshSimplify root, GameObject gameObject)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component == null && MeshSimplify.HasValidMeshData(gameObject))
		{
			return true;
		}
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			if (this.HasNonMeshSimplifyGameObjectsInTreeRecursive(root, gameObject.transform.GetChild(i).gameObject))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x000A58B0 File Offset: 0x000A3CB0
	public void ConfigureSimplifier()
	{
		this.m_meshSimplifier.UseEdgeLength = ((!(this.m_meshSimplifyRoot != null) || this.m_bOverrideRootSettings) ? this.m_bUseEdgeLength : this.m_meshSimplifyRoot.m_bUseEdgeLength);
		this.m_meshSimplifier.UseCurvature = ((!(this.m_meshSimplifyRoot != null) || this.m_bOverrideRootSettings) ? this.m_bUseCurvature : this.m_meshSimplifyRoot.m_bUseCurvature);
		this.m_meshSimplifier.ProtectTexture = ((!(this.m_meshSimplifyRoot != null) || this.m_bOverrideRootSettings) ? this.m_bProtectTexture : this.m_meshSimplifyRoot.m_bProtectTexture);
		this.m_meshSimplifier.LockBorder = ((!(this.m_meshSimplifyRoot != null) || this.m_bOverrideRootSettings) ? this.m_bLockBorder : this.m_meshSimplifyRoot.m_bLockBorder);
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x000A59B1 File Offset: 0x000A3DB1
	public Simplifier GetMeshSimplifier()
	{
		return this.m_meshSimplifier;
	}

	// Token: 0x06001CF6 RID: 7414 RVA: 0x000A59B9 File Offset: 0x000A3DB9
	public void ComputeData(bool bRecurseIntoChildren, Simplifier.ProgressDelegate progress = null)
	{
		MeshSimplify.ComputeDataRecursive(this, base.gameObject, bRecurseIntoChildren, progress);
	}

	// Token: 0x06001CF7 RID: 7415 RVA: 0x000A59CC File Offset: 0x000A3DCC
	private static void ComputeDataRecursive(MeshSimplify root, GameObject gameObject, bool bRecurseIntoChildren, Simplifier.ProgressDelegate progress = null)
	{
		MeshSimplify meshSimplify = gameObject.GetComponent<MeshSimplify>();
		if (meshSimplify == null && root.m_bGenerateIncludeChildren && MeshSimplify.HasValidMeshData(gameObject))
		{
			meshSimplify = gameObject.AddComponent<MeshSimplify>();
			meshSimplify.m_meshSimplifyRoot = root;
			root.m_listDependentChildren.Add(meshSimplify);
		}
		if (meshSimplify != null && MeshSimplify.IsRootOrBelongsToTree(meshSimplify, root))
		{
			meshSimplify.FreeData(false);
			MeshFilter component = meshSimplify.GetComponent<MeshFilter>();
			if (component != null && component.sharedMesh != null)
			{
				if (component.sharedMesh.vertexCount > 0)
				{
					if (meshSimplify.m_originalMesh == null)
					{
						meshSimplify.m_originalMesh = component.sharedMesh;
					}
					Simplifier[] components = meshSimplify.GetComponents<Simplifier>();
					for (int i = 0; i < components.Length; i++)
					{
						if (Application.isEditor && !Application.isPlaying)
						{
							UnityEngine.Object.DestroyImmediate(components[i]);
						}
						else
						{
							UnityEngine.Object.Destroy(components[i]);
						}
					}
					meshSimplify.m_meshSimplifier = meshSimplify.gameObject.AddComponent<Simplifier>();
					meshSimplify.m_meshSimplifier.hideFlags = HideFlags.HideInInspector;
					meshSimplify.ConfigureSimplifier();
					IEnumerator enumerator = meshSimplify.m_meshSimplifier.ProgressiveMesh(gameObject, meshSimplify.m_originalMesh, root.m_aRelevanceSpheres, meshSimplify.name, progress);
					while (enumerator.MoveNext())
					{
						if (Simplifier.Cancelled)
						{
							return;
						}
					}
					if (Simplifier.Cancelled)
					{
						return;
					}
				}
			}
			else
			{
				SkinnedMeshRenderer component2 = meshSimplify.GetComponent<SkinnedMeshRenderer>();
				if (component2 != null && component2.sharedMesh.vertexCount > 0)
				{
					if (meshSimplify.m_originalMesh == null)
					{
						meshSimplify.m_originalMesh = component2.sharedMesh;
					}
					Simplifier[] components2 = meshSimplify.GetComponents<Simplifier>();
					for (int j = 0; j < components2.Length; j++)
					{
						if (Application.isEditor && !Application.isPlaying)
						{
							UnityEngine.Object.DestroyImmediate(components2[j]);
						}
						else
						{
							UnityEngine.Object.Destroy(components2[j]);
						}
					}
					meshSimplify.m_meshSimplifier = meshSimplify.gameObject.AddComponent<Simplifier>();
					meshSimplify.m_meshSimplifier.hideFlags = HideFlags.HideInInspector;
					meshSimplify.ConfigureSimplifier();
					IEnumerator enumerator2 = meshSimplify.m_meshSimplifier.ProgressiveMesh(gameObject, meshSimplify.m_originalMesh, root.m_aRelevanceSpheres, meshSimplify.name, progress);
					while (enumerator2.MoveNext())
					{
						if (Simplifier.Cancelled)
						{
							return;
						}
					}
					if (Simplifier.Cancelled)
					{
						return;
					}
				}
			}
			meshSimplify.m_bDataDirty = false;
		}
		if (bRecurseIntoChildren)
		{
			for (int k = 0; k < gameObject.transform.childCount; k++)
			{
				MeshSimplify.ComputeDataRecursive(root, gameObject.transform.GetChild(k).gameObject, bRecurseIntoChildren, progress);
				if (Simplifier.Cancelled)
				{
					return;
				}
			}
		}
	}

	// Token: 0x06001CF8 RID: 7416 RVA: 0x000A5C94 File Offset: 0x000A4094
	public bool HasData()
	{
		return (this.m_meshSimplifier != null && this.m_simplifiedMesh != null) || (this.m_listDependentChildren != null && this.m_listDependentChildren.Count != 0);
	}

	// Token: 0x06001CF9 RID: 7417 RVA: 0x000A5CE8 File Offset: 0x000A40E8
	public bool HasPhysicsData()
	{
		return (this.m_meshSimplifier != null && this.m_simplifiedMeshPhysics != null) || (this.m_listDependentChildren != null && this.m_listDependentChildren.Count != 0);
	}

	// Token: 0x06001CFA RID: 7418 RVA: 0x000A5D39 File Offset: 0x000A4139
	public bool HasSimplifiedMesh()
	{
		return this.m_simplifiedMesh != null && this.m_simplifiedMesh.vertexCount > 0;
	}

	// Token: 0x06001CFB RID: 7419 RVA: 0x000A5D5D File Offset: 0x000A415D
	public bool HasSimplifiedPhysicsMesh()
	{
		return this.m_simplifiedMeshPhysics != null && this.m_simplifiedMeshPhysics.vertexCount > 0;
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x000A5D81 File Offset: 0x000A4181
	public void ComputeMesh(bool bRecurseIntoChildren, Simplifier.ProgressDelegate progress = null)
	{
		MeshSimplify.ComputeMeshRecursive(this, base.gameObject, bRecurseIntoChildren, progress);
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x000A5D94 File Offset: 0x000A4194
	private static void ComputeMeshRecursive(MeshSimplify root, GameObject gameObject, bool bRecurseIntoChildren, Simplifier.ProgressDelegate progress = null)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_meshSimplifier != null)
		{
			if (component.m_simplifiedMesh)
			{
				component.m_simplifiedMesh.Clear();
			}
			if (component.m_simplifiedMeshPhysics)
			{
				component.m_simplifiedMeshPhysics.Clear();
			}
			float fVertexAmount = component.m_fVertexAmount;
			if (!component.m_bOverrideRootSettings && component.m_meshSimplifyRoot != null)
			{
				fVertexAmount = component.m_meshSimplifyRoot.m_fVertexAmount;
			}
			if (component.m_simplifiedMesh == null)
			{
				component.m_simplifiedMesh = MeshSimplify.CreateNewEmptyMesh(component);
			}
			if (component.m_useDifferentAmountForPhysics && component.m_simplifiedMeshPhysics == null)
			{
				component.m_simplifiedMeshPhysics = MeshSimplify.CreateNewEmptyMesh(component);
			}
			component.ConfigureSimplifier();
			IEnumerator enumerator = component.m_meshSimplifier.ComputeMeshWithVertexCount(gameObject, component.m_simplifiedMesh, Mathf.RoundToInt(fVertexAmount * (float)component.m_meshSimplifier.GetOriginalMeshUniqueVertexCount()), component.name + " Simplified", progress);
			while (enumerator.MoveNext())
			{
				if (Simplifier.Cancelled)
				{
					return;
				}
			}
			if (Simplifier.Cancelled)
			{
				return;
			}
			if (component.m_useDifferentAmountForPhysics)
			{
				float fVertexAmountPhysics = component.m_fVertexAmountPhysics;
				if (!component.m_bOverrideRootSettings && component.m_meshSimplifyRoot != null)
				{
					fVertexAmountPhysics = component.m_meshSimplifyRoot.m_fVertexAmountPhysics;
				}
				enumerator = component.m_meshSimplifier.ComputeMeshWithVertexCount(gameObject, component.m_simplifiedMeshPhysics, Mathf.RoundToInt(fVertexAmountPhysics * (float)component.m_meshSimplifier.GetOriginalMeshUniqueVertexCount()), component.name + " Simplified", progress);
				while (enumerator.MoveNext())
				{
					if (Simplifier.Cancelled)
					{
						return;
					}
				}
				if (Simplifier.Cancelled)
				{
					return;
				}
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.ComputeMeshRecursive(root, gameObject.transform.GetChild(i).gameObject, bRecurseIntoChildren, progress);
				if (Simplifier.Cancelled)
				{
					return;
				}
			}
		}
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x000A5FC0 File Offset: 0x000A43C0
	public void AssignSimplifiedMesh(bool bRecurseIntoChildren)
	{
		MeshSimplify.AssignSimplifiedMeshRecursive(this, base.gameObject, bRecurseIntoChildren);
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x000A5FD0 File Offset: 0x000A43D0
	private static void AssignSimplifiedMeshRecursive(MeshSimplify root, GameObject gameObject, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_simplifiedMesh != null)
		{
			MeshFilter component2 = component.GetComponent<MeshFilter>();
			if (component2 != null)
			{
				if (component.m_bSetMeshFilter)
				{
					component2.sharedMesh = component.m_simplifiedMesh;
				}
			}
			else
			{
				SkinnedMeshRenderer component3 = component.GetComponent<SkinnedMeshRenderer>();
				if (component3 != null)
				{
					component3.sharedMesh = component.m_simplifiedMesh;
				}
			}
			if (component.m_bSetMeshCollider)
			{
				MeshCollider component4 = component.GetComponent<MeshCollider>();
				if (component4 != null)
				{
					if (component.m_useDifferentAmountForPhysics && component.m_simplifiedMeshPhysics != null)
					{
						component4.sharedMesh = component.m_simplifiedMeshPhysics;
					}
					else
					{
						component4.sharedMesh = component.m_simplifiedMesh;
					}
				}
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.AssignSimplifiedMeshRecursive(root, gameObject.transform.GetChild(i).gameObject, bRecurseIntoChildren);
			}
		}
	}

	// Token: 0x06001D00 RID: 7424 RVA: 0x000A60F1 File Offset: 0x000A44F1
	public void RestoreOriginalMesh(bool bDeleteData, bool bRecurseIntoChildren)
	{
		MeshSimplify.RestoreOriginalMeshRecursive(this, base.gameObject, bDeleteData, bRecurseIntoChildren);
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x000A6104 File Offset: 0x000A4504
	private static void RestoreOriginalMeshRecursive(MeshSimplify root, GameObject gameObject, bool bDeleteData, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root))
		{
			if (component.m_originalMesh != null)
			{
				MeshFilter component2 = component.GetComponent<MeshFilter>();
				if (component2 != null)
				{
					if (component.m_bSetMeshFilter)
					{
						component2.sharedMesh = component.m_originalMesh;
					}
				}
				else
				{
					SkinnedMeshRenderer component3 = component.GetComponent<SkinnedMeshRenderer>();
					if (component3 != null)
					{
						component3.sharedMesh = component.m_originalMesh;
					}
				}
			}
			if (component.m_bSetMeshCollider)
			{
				MeshCollider component4 = component.GetComponent<MeshCollider>();
				if (component4 != null)
				{
					component4.sharedMesh = component.m_originalMesh;
				}
			}
			if (bDeleteData)
			{
				component.FreeData(false);
				component.m_listDependentChildren.Clear();
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.RestoreOriginalMeshRecursive(root, gameObject.transform.GetChild(i).gameObject, bDeleteData, bRecurseIntoChildren);
			}
		}
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x000A6211 File Offset: 0x000A4611
	public bool HasOriginalMeshActive(bool bRecurseIntoChildren)
	{
		return MeshSimplify.HasOriginalMeshActiveRecursive(this, base.gameObject, bRecurseIntoChildren);
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x000A6220 File Offset: 0x000A4620
	private static bool HasOriginalMeshActiveRecursive(MeshSimplify root, GameObject gameObject, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		bool flag = false;
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_originalMesh != null)
		{
			MeshFilter component2 = component.GetComponent<MeshFilter>();
			if (component2 != null)
			{
				if (component2.sharedMesh == component.m_originalMesh)
				{
					flag = true;
				}
			}
			else
			{
				SkinnedMeshRenderer component3 = component.GetComponent<SkinnedMeshRenderer>();
				if (component3 != null && component3.sharedMesh == component.m_originalMesh)
				{
					flag = true;
				}
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				flag = (flag || MeshSimplify.HasOriginalMeshActiveRecursive(root, gameObject.transform.GetChild(i).gameObject, bRecurseIntoChildren));
			}
		}
		return flag;
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x000A6304 File Offset: 0x000A4704
	public bool HasVertexData(bool bRecurseIntoChildren)
	{
		return MeshSimplify.HasVertexDataRecursive(this, base.gameObject, bRecurseIntoChildren);
	}

	// Token: 0x06001D05 RID: 7429 RVA: 0x000A6314 File Offset: 0x000A4714
	private static bool HasVertexDataRecursive(MeshSimplify root, GameObject gameObject, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_simplifiedMesh && component.m_simplifiedMesh.vertexCount > 0)
		{
			return true;
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				if (MeshSimplify.HasVertexDataRecursive(root, gameObject.transform.GetChild(i).gameObject, bRecurseIntoChildren))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x000A63A8 File Offset: 0x000A47A8
	public int GetOriginalVertexCount(bool bRecurseIntoChildren)
	{
		int result = 0;
		MeshSimplify.GetOriginalVertexCountRecursive(this, base.gameObject, ref result, bRecurseIntoChildren);
		return result;
	}

	// Token: 0x06001D07 RID: 7431 RVA: 0x000A63C8 File Offset: 0x000A47C8
	private static void GetOriginalVertexCountRecursive(MeshSimplify root, GameObject gameObject, ref int nVertexCount, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root))
		{
			if (component.m_originalMesh != null)
			{
				nVertexCount += component.m_originalMesh.vertexCount;
			}
			else
			{
				MeshFilter component2 = component.GetComponent<MeshFilter>();
				if (component2 != null && component2.sharedMesh != null)
				{
					nVertexCount += component2.sharedMesh.vertexCount;
				}
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.GetOriginalVertexCountRecursive(root, gameObject.transform.GetChild(i).gameObject, ref nVertexCount, bRecurseIntoChildren);
			}
		}
	}

	// Token: 0x06001D08 RID: 7432 RVA: 0x000A648C File Offset: 0x000A488C
	public int GetOriginalTriangleCount(bool bRecurseIntoChildren)
	{
		int result = 0;
		MeshSimplify.GetOriginalTriangleCountRecursive(this, base.gameObject, ref result, bRecurseIntoChildren);
		return result;
	}

	// Token: 0x06001D09 RID: 7433 RVA: 0x000A64AC File Offset: 0x000A48AC
	private static void GetOriginalTriangleCountRecursive(MeshSimplify root, GameObject gameObject, ref int nTriangleCount, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root))
		{
			if (component.m_originalMesh != null)
			{
				nTriangleCount += component.m_originalMesh.triangles.Length / 3;
			}
			else
			{
				MeshFilter component2 = component.GetComponent<MeshFilter>();
				if (component2 != null && component2.sharedMesh != null)
				{
					nTriangleCount += component2.sharedMesh.triangles.Length / 3;
				}
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.GetOriginalTriangleCountRecursive(root, gameObject.transform.GetChild(i).gameObject, ref nTriangleCount, bRecurseIntoChildren);
			}
		}
	}

	// Token: 0x06001D0A RID: 7434 RVA: 0x000A6578 File Offset: 0x000A4978
	public int GetSimplifiedVertexCount(bool bRecurseIntoChildren)
	{
		int result = 0;
		MeshSimplify.GetSimplifiedVertexCountRecursive(this, base.gameObject, ref result, bRecurseIntoChildren);
		return result;
	}

	// Token: 0x06001D0B RID: 7435 RVA: 0x000A6598 File Offset: 0x000A4998
	private static void GetSimplifiedVertexCountRecursive(MeshSimplify root, GameObject gameObject, ref int nVertexCount, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_simplifiedMesh != null)
		{
			nVertexCount += component.m_simplifiedMesh.vertexCount;
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.GetSimplifiedVertexCountRecursive(root, gameObject.transform.GetChild(i).gameObject, ref nVertexCount, bRecurseIntoChildren);
			}
		}
	}

	// Token: 0x06001D0C RID: 7436 RVA: 0x000A6620 File Offset: 0x000A4A20
	public int GetSimplifiedPhysicsVertexCount(bool bRecurseIntoChildren)
	{
		int result = 0;
		MeshSimplify.GetSimplifiedPhysicsVertexCountRecursive(this, base.gameObject, ref result, bRecurseIntoChildren);
		return result;
	}

	// Token: 0x06001D0D RID: 7437 RVA: 0x000A6640 File Offset: 0x000A4A40
	private static void GetSimplifiedPhysicsVertexCountRecursive(MeshSimplify root, GameObject gameObject, ref int nVertexCount, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_simplifiedMeshPhysics != null)
		{
			nVertexCount += component.m_simplifiedMeshPhysics.vertexCount;
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.GetSimplifiedPhysicsVertexCountRecursive(root, gameObject.transform.GetChild(i).gameObject, ref nVertexCount, bRecurseIntoChildren);
			}
		}
	}

	// Token: 0x06001D0E RID: 7438 RVA: 0x000A66C8 File Offset: 0x000A4AC8
	public int GetSimplifiedTriangleCount(bool bRecurseIntoChildren)
	{
		int result = 0;
		MeshSimplify.GetSimplifiedTriangleCountRecursive(this, base.gameObject, ref result, bRecurseIntoChildren);
		return result;
	}

	// Token: 0x06001D0F RID: 7439 RVA: 0x000A66E8 File Offset: 0x000A4AE8
	private static void GetSimplifiedTriangleCountRecursive(MeshSimplify root, GameObject gameObject, ref int nTriangleCount, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_simplifiedMesh != null)
		{
			nTriangleCount += component.m_simplifiedMesh.triangles.Length / 3;
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.GetSimplifiedTriangleCountRecursive(root, gameObject.transform.GetChild(i).gameObject, ref nTriangleCount, bRecurseIntoChildren);
			}
		}
	}

	// Token: 0x06001D10 RID: 7440 RVA: 0x000A6774 File Offset: 0x000A4B74
	public int GetSimplifiedPhysicsTriangleCount(bool bRecurseIntoChildren)
	{
		int result = 0;
		MeshSimplify.GetSimplifiedPhysicsTriangleCountRecursive(this, base.gameObject, ref result, bRecurseIntoChildren);
		return result;
	}

	// Token: 0x06001D11 RID: 7441 RVA: 0x000A6794 File Offset: 0x000A4B94
	private static void GetSimplifiedPhysicsTriangleCountRecursive(MeshSimplify root, GameObject gameObject, ref int nTriangleCount, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_simplifiedMeshPhysics != null)
		{
			nTriangleCount += component.m_simplifiedMeshPhysics.triangles.Length / 3;
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.GetSimplifiedPhysicsTriangleCountRecursive(root, gameObject.transform.GetChild(i).gameObject, ref nTriangleCount, bRecurseIntoChildren);
			}
		}
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x000A6820 File Offset: 0x000A4C20
	public void RemoveFromTree()
	{
		if (this.m_meshSimplifyRoot != null)
		{
			this.m_meshSimplifyRoot.m_listDependentChildren.Remove(this);
		}
		this.RestoreOriginalMesh(true, false);
		this.m_bExcludedFromTree = true;
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x000A6854 File Offset: 0x000A4C54
	public void FreeData(bool bRecurseIntoChildren)
	{
		MeshSimplify.FreeDataRecursive(this, base.gameObject, bRecurseIntoChildren);
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x000A6864 File Offset: 0x000A4C64
	private static void FreeDataRecursive(MeshSimplify root, GameObject gameObject, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root))
		{
			if (component.m_bEnablePrefabUsage)
			{
				component.m_simplifiedMesh = null;
				component.m_simplifiedMeshPhysics = null;
			}
			else
			{
				if (component.m_simplifiedMesh)
				{
					component.m_simplifiedMesh.Clear();
				}
				if (component.m_simplifiedMeshPhysics)
				{
					component.m_simplifiedMeshPhysics.Clear();
				}
			}
			Simplifier[] components = gameObject.GetComponents<Simplifier>();
			for (int i = 0; i < components.Length; i++)
			{
				if (Application.isEditor && !Application.isPlaying)
				{
					UnityEngine.Object.DestroyImmediate(components[i]);
				}
				else
				{
					UnityEngine.Object.Destroy(components[i]);
				}
			}
			component.m_bDataDirty = true;
		}
		if (bRecurseIntoChildren)
		{
			for (int j = 0; j < gameObject.transform.childCount; j++)
			{
				MeshSimplify.FreeDataRecursive(root, gameObject.transform.GetChild(j).gameObject, bRecurseIntoChildren);
			}
		}
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x000A696C File Offset: 0x000A4D6C
	private static Mesh CreateNewEmptyMesh(MeshSimplify meshSimplify)
	{
		if (meshSimplify.m_originalMesh == null)
		{
			return new Mesh();
		}
		Mesh mesh = UnityEngine.Object.Instantiate<Mesh>(meshSimplify.m_originalMesh);
		mesh.Clear();
		return mesh;
	}

	// Token: 0x0400189F RID: 6303
	[HideInInspector]
	public Mesh m_originalMesh;

	// Token: 0x040018A0 RID: 6304
	[HideInInspector]
	public Mesh m_simplifiedMesh;

	// Token: 0x040018A1 RID: 6305
	[HideInInspector]
	public Mesh m_simplifiedMeshPhysics;

	// Token: 0x040018A2 RID: 6306
	[HideInInspector]
	public bool m_bEnablePrefabUsage = true;

	// Token: 0x040018A3 RID: 6307
	[HideInInspector]
	public float m_fVertexAmount = 1f;

	// Token: 0x040018A4 RID: 6308
	[HideInInspector]
	public bool m_useDifferentAmountForPhysics;

	// Token: 0x040018A5 RID: 6309
	[HideInInspector]
	public float m_fVertexAmountPhysics = 1f;

	// Token: 0x040018A6 RID: 6310
	[HideInInspector]
	public string m_strAssetPath;

	// Token: 0x040018A7 RID: 6311
	[HideInInspector]
	public string m_strAssetPathPhysics;

	// Token: 0x040018A8 RID: 6312
	[HideInInspector]
	public MeshSimplify m_meshSimplifyRoot;

	// Token: 0x040018A9 RID: 6313
	[HideInInspector]
	public List<MeshSimplify> m_listDependentChildren = new List<MeshSimplify>();

	// Token: 0x040018AA RID: 6314
	[HideInInspector]
	public bool m_bExpandRelevanceSpheres = true;

	// Token: 0x040018AB RID: 6315
	public RelevanceSphere[] m_aRelevanceSpheres;

	// Token: 0x040018AC RID: 6316
	[SerializeField]
	[HideInInspector]
	private Simplifier m_meshSimplifier;

	// Token: 0x040018AD RID: 6317
	[SerializeField]
	[HideInInspector]
	private bool m_bGenerateIncludeChildren;

	// Token: 0x040018AE RID: 6318
	[HideInInspector]
	public bool m_bSetMeshFilter = true;

	// Token: 0x040018AF RID: 6319
	[HideInInspector]
	public bool m_bSetMeshCollider = true;

	// Token: 0x040018B0 RID: 6320
	[SerializeField]
	[HideInInspector]
	private bool m_bOverrideRootSettings;

	// Token: 0x040018B1 RID: 6321
	[SerializeField]
	[HideInInspector]
	private bool m_bUseEdgeLength = true;

	// Token: 0x040018B2 RID: 6322
	[SerializeField]
	[HideInInspector]
	private bool m_bUseCurvature = true;

	// Token: 0x040018B3 RID: 6323
	[SerializeField]
	[HideInInspector]
	private bool m_bProtectTexture = true;

	// Token: 0x040018B4 RID: 6324
	[SerializeField]
	[HideInInspector]
	private bool m_bLockBorder = true;

	// Token: 0x040018B5 RID: 6325
	[SerializeField]
	[HideInInspector]
	private bool m_bDataDirty = true;

	// Token: 0x040018B6 RID: 6326
	[SerializeField]
	[HideInInspector]
	private bool m_bExcludedFromTree;
}
