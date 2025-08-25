using System;
using UnityEngine;

// Token: 0x02000D2A RID: 3370
[ExecuteInEditMode]
public class SyncMaterialParameters : MonoBehaviour
{
	// Token: 0x0600674A RID: 26442 RVA: 0x0026DB6D File Offset: 0x0026BF6D
	public SyncMaterialParameters()
	{
	}

	// Token: 0x0600674B RID: 26443 RVA: 0x0026DB75 File Offset: 0x0026BF75
	private void Start()
	{
	}

	// Token: 0x0600674C RID: 26444 RVA: 0x0026DB78 File Offset: 0x0026BF78
	private void Sync()
	{
		foreach (string name in this.syncParams)
		{
			if (this.master.HasProperty(name))
			{
				foreach (Material material in this.slaves)
				{
					if (material.HasProperty(name))
					{
						material.SetFloat(name, this.master.GetFloat(name));
					}
				}
			}
		}
		foreach (string name2 in this.syncColorParams)
		{
			if (this.master.HasProperty(name2))
			{
				foreach (Material material2 in this.slaves)
				{
					if (material2.HasProperty(name2))
					{
						material2.SetColor(name2, this.master.GetColor(name2));
					}
				}
			}
		}
	}

	// Token: 0x0600674D RID: 26445 RVA: 0x0026DC81 File Offset: 0x0026C081
	private void Update()
	{
		if (this.sync)
		{
			this.sync = false;
			this.Sync();
		}
	}

	// Token: 0x04005846 RID: 22598
	public bool sync;

	// Token: 0x04005847 RID: 22599
	public Material master;

	// Token: 0x04005848 RID: 22600
	public Material[] slaves;

	// Token: 0x04005849 RID: 22601
	public string[] syncParams;

	// Token: 0x0400584A RID: 22602
	public string[] syncColorParams;
}
