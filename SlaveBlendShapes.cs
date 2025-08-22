using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000D39 RID: 3385
[ExecuteInEditMode]
public class SlaveBlendShapes : MonoBehaviour
{
	// Token: 0x0600677A RID: 26490 RVA: 0x0026E988 File Offset: 0x0026CD88
	public SlaveBlendShapes()
	{
	}

	// Token: 0x0600677B RID: 26491 RVA: 0x0026E990 File Offset: 0x0026CD90
	private void Start()
	{
		this.init();
	}

	// Token: 0x0600677C RID: 26492 RVA: 0x0026E998 File Offset: 0x0026CD98
	private void init()
	{
		if (!this.wasInit)
		{
			this.wasInit = true;
			this.dest = base.GetComponent<SkinnedMeshRenderer>();
			this.shapeMap = new Dictionary<int, int>();
			this.excludeMap = new Dictionary<string, bool>();
			if (this.exclude != null)
			{
				foreach (string key in this.exclude)
				{
					this.excludeMap.Add(key, true);
				}
			}
			if (this.dest)
			{
				this.shapeCount = this.dest.sharedMesh.blendShapeCount;
				if (this.source)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>();
					for (int j = 0; j < this.source.sharedMesh.blendShapeCount; j++)
					{
						string blendShapeName = this.source.sharedMesh.GetBlendShapeName(j);
						string key2 = Regex.Replace(blendShapeName, "^.*\\.", string.Empty);
						dictionary.Add(key2, j);
					}
					for (int k = 0; k < this.shapeCount; k++)
					{
						string blendShapeName2 = this.dest.sharedMesh.GetBlendShapeName(k);
						string key3 = Regex.Replace(blendShapeName2, "^.*\\.", string.Empty);
						int value;
						bool flag;
						if (dictionary.TryGetValue(key3, out value) && !this.excludeMap.TryGetValue(key3, out flag))
						{
							this.shapeMap.Add(k, value);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600677D RID: 26493 RVA: 0x0026EB18 File Offset: 0x0026CF18
	private void setWeights()
	{
		if (this.source && this.dest && this.shapeMap != null)
		{
			for (int i = 0; i < this.shapeCount; i++)
			{
				int index;
				if (this.shapeMap.TryGetValue(i, out index))
				{
					this.dest.SetBlendShapeWeight(i, this.source.GetBlendShapeWeight(index));
				}
			}
		}
	}

	// Token: 0x0600677E RID: 26494 RVA: 0x0026EB92 File Offset: 0x0026CF92
	private void Update()
	{
		if (this.reinit)
		{
			this.wasInit = false;
			this.reinit = false;
		}
		this.init();
		this.setWeights();
	}

	// Token: 0x0400588D RID: 22669
	public SkinnedMeshRenderer source;

	// Token: 0x0400588E RID: 22670
	public string[] exclude;

	// Token: 0x0400588F RID: 22671
	public bool reinit;

	// Token: 0x04005890 RID: 22672
	private SkinnedMeshRenderer dest;

	// Token: 0x04005891 RID: 22673
	private Dictionary<string, bool> excludeMap;

	// Token: 0x04005892 RID: 22674
	private Dictionary<int, int> shapeMap;

	// Token: 0x04005893 RID: 22675
	private int shapeCount;

	// Token: 0x04005894 RID: 22676
	private bool wasInit;
}
