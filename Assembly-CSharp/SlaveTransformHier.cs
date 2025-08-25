using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BC8 RID: 3016
public class SlaveTransformHier : MonoBehaviour
{
	// Token: 0x060055C1 RID: 21953 RVA: 0x001F5C24 File Offset: 0x001F4024
	public SlaveTransformHier()
	{
	}

	// Token: 0x060055C2 RID: 21954 RVA: 0x001F5C2C File Offset: 0x001F402C
	private Transform findTransform(string tname)
	{
		foreach (Transform transform in this.sourceTree.GetComponentsInChildren<Transform>())
		{
			if (transform.name == tname)
			{
				return transform;
			}
		}
		return null;
	}

	// Token: 0x060055C3 RID: 21955 RVA: 0x001F5C74 File Offset: 0x001F4074
	private void init()
	{
		if (this.sourceTree)
		{
			this.sourceTreeMap = new Dictionary<string, Transform>();
			foreach (Transform transform in this.sourceTree.GetComponentsInChildren<Transform>())
			{
				if (!this.sourceTreeMap.ContainsKey(transform.name))
				{
					this.sourceTreeMap.Add(transform.name, transform);
				}
			}
			this.transformMap = new Dictionary<Transform, Transform>();
			foreach (Transform transform2 in base.GetComponentsInChildren<Transform>())
			{
				Transform key;
				if (this.sourceTreeMap.TryGetValue(transform2.name, out key))
				{
					this.transformMap.Add(key, transform2);
				}
			}
		}
	}

	// Token: 0x060055C4 RID: 21956 RVA: 0x001F5D40 File Offset: 0x001F4140
	private void Start()
	{
		this.init();
	}

	// Token: 0x060055C5 RID: 21957 RVA: 0x001F5D48 File Offset: 0x001F4148
	private void Update()
	{
		if (this.transformMap != null)
		{
			foreach (Transform transform in this.transformMap.Keys)
			{
				Transform transform2;
				if (this.transformMap.TryGetValue(transform, out transform2))
				{
					transform2.position = transform.position;
					transform2.rotation = transform.rotation;
				}
			}
		}
	}

	// Token: 0x040046E4 RID: 18148
	public Transform sourceTree;

	// Token: 0x040046E5 RID: 18149
	private Dictionary<string, Transform> sourceTreeMap;

	// Token: 0x040046E6 RID: 18150
	private Dictionary<Transform, Transform> transformMap;
}
