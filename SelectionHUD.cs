using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DDF RID: 3551
public class SelectionHUD : MonoBehaviour
{
	// Token: 0x06006DDA RID: 28122 RVA: 0x00294404 File Offset: 0x00292804
	public SelectionHUD()
	{
	}

	// Token: 0x1700100D RID: 4109
	// (get) Token: 0x06006DDB RID: 28123 RVA: 0x0029440C File Offset: 0x0029280C
	public int numSlots
	{
		get
		{
			if (this.selectionTexts != null)
			{
				return this.selectionTexts.Length;
			}
			return 0;
		}
	}

	// Token: 0x06006DDC RID: 28124 RVA: 0x00294424 File Offset: 0x00292824
	public void ClearSelections()
	{
		for (int i = 0; i < this.selectionTexts.Length; i++)
		{
			this.SetSelection(i, null, string.Empty);
		}
	}

	// Token: 0x06006DDD RID: 28125 RVA: 0x00294458 File Offset: 0x00292858
	protected void Init()
	{
		if (!this._wasInit)
		{
			this._wasInit = true;
			this.selectionLineDrawers = new LineDrawer[this.selectionTexts.Length];
			this.selections = new Transform[this.selectionTexts.Length];
			for (int i = 0; i < this.selectionTexts.Length; i++)
			{
				if (i == 0 && this.firstSelectionLineMaterial != null)
				{
					this.selectionLineDrawers[i] = new LineDrawer(this.firstSelectionLineMaterial);
				}
				else if (this.selectionLineMaterial != null)
				{
					this.selectionLineDrawers[i] = new LineDrawer(this.selectionLineMaterial);
				}
			}
		}
	}

	// Token: 0x06006DDE RID: 28126 RVA: 0x00294509 File Offset: 0x00292909
	public void SetSelection(int index, Transform selection, string name)
	{
		this.Init();
		if (index < this.selectionTexts.Length)
		{
			this.selectionTexts[index].text = name;
			this.selections[index] = selection;
		}
	}

	// Token: 0x06006DDF RID: 28127 RVA: 0x00294536 File Offset: 0x00292936
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x06006DE0 RID: 28128 RVA: 0x00294540 File Offset: 0x00292940
	private void Update()
	{
		for (int i = 0; i < this.selectionTexts.Length; i++)
		{
			if (this.selections[i] != null && this.selectionLineDrawers[i] != null)
			{
				if (this.useDrawFromPosition)
				{
					this.selectionLineDrawers[i].SetLinePoints(this.drawFrom, this.selections[i].position);
				}
				else
				{
					this.selectionLineDrawers[i].SetLinePoints(this.selectionTexts[i].transform.position, this.selections[i].position);
				}
				this.selectionLineDrawers[i].Draw(base.gameObject.layer);
			}
		}
	}

	// Token: 0x04005F1D RID: 24349
	public Text headerText;

	// Token: 0x04005F1E RID: 24350
	public Text[] selectionTexts;

	// Token: 0x04005F1F RID: 24351
	public Material selectionLineMaterial;

	// Token: 0x04005F20 RID: 24352
	public Material firstSelectionLineMaterial;

	// Token: 0x04005F21 RID: 24353
	public Vector3 drawFrom;

	// Token: 0x04005F22 RID: 24354
	public bool useDrawFromPosition;

	// Token: 0x04005F23 RID: 24355
	protected LineDrawer[] selectionLineDrawers;

	// Token: 0x04005F24 RID: 24356
	protected Transform[] selections;

	// Token: 0x04005F25 RID: 24357
	protected bool _wasInit;
}
