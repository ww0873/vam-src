using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D3A RID: 3386
[ExecuteInEditMode]
public class ZeroMorphs : MonoBehaviour
{
	// Token: 0x0600677F RID: 26495 RVA: 0x0026EBB9 File Offset: 0x0026CFB9
	public ZeroMorphs()
	{
	}

	// Token: 0x06006780 RID: 26496 RVA: 0x0026EBC1 File Offset: 0x0026CFC1
	private void OnEnable()
	{
		if (this.skin)
		{
			this.zero = true;
		}
	}

	// Token: 0x06006781 RID: 26497 RVA: 0x0026EBDC File Offset: 0x0026CFDC
	private void OnDisable()
	{
		if (this.skin && this.startingVals != null)
		{
			this.zero = false;
			foreach (int num in this.startingVals.Keys)
			{
				float value;
				if (this.startingVals.TryGetValue(num, out value))
				{
					this.skin.SetBlendShapeWeight(num, value);
				}
			}
		}
	}

	// Token: 0x06006782 RID: 26498 RVA: 0x0026EC78 File Offset: 0x0026D078
	private void Init()
	{
		this.wasInit = true;
		this.startingVals = new Dictionary<int, float>();
		if (this.skin)
		{
			foreach (string blendShapeName in this.morphs)
			{
				int blendShapeIndex = this.skin.sharedMesh.GetBlendShapeIndex(blendShapeName);
				if (blendShapeIndex != -1)
				{
					this.startingVals.Add(blendShapeIndex, this.skin.GetBlendShapeWeight(blendShapeIndex));
				}
			}
		}
	}

	// Token: 0x06006783 RID: 26499 RVA: 0x0026ECF7 File Offset: 0x0026D0F7
	private void Start()
	{
		this.Init();
	}

	// Token: 0x06006784 RID: 26500 RVA: 0x0026ED00 File Offset: 0x0026D100
	private void zeroMorphs()
	{
		if (this.skin && this.zero && this.startingVals != null)
		{
			foreach (int index in this.startingVals.Keys)
			{
				this.skin.SetBlendShapeWeight(index, 0f);
			}
		}
	}

	// Token: 0x06006785 RID: 26501 RVA: 0x0026ED94 File Offset: 0x0026D194
	private void Update()
	{
		if (this.reinit)
		{
			this.reinit = false;
			this.wasInit = false;
		}
		if (!this.wasInit)
		{
			this.Init();
		}
		this.zeroMorphs();
	}

	// Token: 0x06006786 RID: 26502 RVA: 0x0026EDC6 File Offset: 0x0026D1C6
	private void LateUpdate()
	{
		this.zeroMorphs();
	}

	// Token: 0x04005895 RID: 22677
	public bool reinit;

	// Token: 0x04005896 RID: 22678
	public SkinnedMeshRenderer skin;

	// Token: 0x04005897 RID: 22679
	public string[] morphs;

	// Token: 0x04005898 RID: 22680
	private Dictionary<int, float> startingVals;

	// Token: 0x04005899 RID: 22681
	private bool zero;

	// Token: 0x0400589A RID: 22682
	private bool wasInit;
}
