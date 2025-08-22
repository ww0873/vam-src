using System;
using UnityEngine;

// Token: 0x02000304 RID: 772
public class FPS : MonoBehaviour
{
	// Token: 0x0600123F RID: 4671 RVA: 0x0006542F File Offset: 0x0006382F
	public FPS()
	{
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x00065442 File Offset: 0x00063842
	private void Awake()
	{
		this.guiStyleHeader.fontSize = 14;
		this.guiStyleHeader.normal.textColor = new Color(1f, 0f, 0f);
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x00065475 File Offset: 0x00063875
	private void OnGUI()
	{
		GUI.Label(new Rect(0f, 0f, 30f, 30f), "FPS: " + (int)this.fps, this.guiStyleHeader);
	}

	// Token: 0x06001242 RID: 4674 RVA: 0x000654B4 File Offset: 0x000638B4
	private void Update()
	{
		this.timeleft -= Time.deltaTime;
		this.frames++;
		if ((double)this.timeleft <= 0.0)
		{
			this.fps = (float)this.frames;
			this.timeleft = 1f;
			this.frames = 0;
		}
	}

	// Token: 0x04000F9F RID: 3999
	private readonly GUIStyle guiStyleHeader = new GUIStyle();

	// Token: 0x04000FA0 RID: 4000
	private float timeleft;

	// Token: 0x04000FA1 RID: 4001
	private float fps;

	// Token: 0x04000FA2 RID: 4002
	private int frames;
}
