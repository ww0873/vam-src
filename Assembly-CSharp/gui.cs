using System;
using UnityEngine;

// Token: 0x0200041D RID: 1053
public class gui : MonoBehaviour
{
	// Token: 0x06001A7E RID: 6782 RVA: 0x00094586 File Offset: 0x00092986
	public gui()
	{
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x00094599 File Offset: 0x00092999
	private void Start()
	{
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x0009459B File Offset: 0x0009299B
	private void Update()
	{
	}

	// Token: 0x06001A81 RID: 6785 RVA: 0x000945A0 File Offset: 0x000929A0
	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 10f, 150f, 100f), base.name))
		{
			if (this.tname == "DAY")
			{
				RenderSettings.skybox = new Material(this.mat);
				this.tname = "NIGHT";
				GameObject.Find("sun").GetComponent<Light>().intensity = 0.1f;
			}
			else
			{
				RenderSettings.skybox = new Material(this.mat1);
				this.tname = "DAY";
				GameObject.Find("sun").GetComponent<Light>().intensity = 0f;
			}
			MonoBehaviour.print(this.mat);
		}
	}

	// Token: 0x04001598 RID: 5528
	public Material mat;

	// Token: 0x04001599 RID: 5529
	public Material mat1;

	// Token: 0x0400159A RID: 5530
	private string tname = "DAY";
}
