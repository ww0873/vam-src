using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C9B RID: 3227
public class RebindSMR : MonoBehaviour
{
	// Token: 0x06006133 RID: 24883 RVA: 0x002506EA File Offset: 0x0024EAEA
	public RebindSMR()
	{
	}

	// Token: 0x06006134 RID: 24884 RVA: 0x002506F4 File Offset: 0x0024EAF4
	private void Start()
	{
		this.smr = base.GetComponent<SkinnedMeshRenderer>();
		if (this.newRoot != null && this.smr != null)
		{
			Transform[] array = new Transform[this.smr.bones.Length];
			Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
			foreach (Transform transform in this.newRoot.GetComponentsInChildren<Transform>())
			{
				if (!dictionary.ContainsKey(transform.name))
				{
					dictionary.Add(transform.name, transform);
				}
			}
			for (int j = 0; j < this.smr.bones.Length; j++)
			{
				Transform transform2 = this.smr.bones[j];
				Transform transform3;
				if (dictionary.TryGetValue(transform2.name, out transform3))
				{
					array[j] = transform3;
				}
			}
			this.smr.bones = array;
			Transform rootBone;
			if (dictionary.TryGetValue(this.smr.rootBone.name, out rootBone))
			{
				this.smr.rootBone = rootBone;
			}
		}
	}

	// Token: 0x0400511C RID: 20764
	public Transform newRoot;

	// Token: 0x0400511D RID: 20765
	private SkinnedMeshRenderer smr;
}
