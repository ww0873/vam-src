using System;
using UnityEngine;

// Token: 0x02000B83 RID: 2947
public class RhythmController : MonoBehaviour
{
	// Token: 0x060052E5 RID: 21221 RVA: 0x001DF367 File Offset: 0x001DD767
	public RhythmController()
	{
	}

	// Token: 0x17000C0D RID: 3085
	// (get) Token: 0x060052E6 RID: 21222 RVA: 0x001DF36F File Offset: 0x001DD76F
	public bool IsPlaying
	{
		get
		{
			return this.rhythmTool != null && this.rhythmTool.IsPlaying;
		}
	}

	// Token: 0x060052E7 RID: 21223 RVA: 0x001DF389 File Offset: 0x001DD789
	protected void Init()
	{
		if (!this._wasInit)
		{
			this._wasInit = true;
			this.rhythmTool.Init(this, false);
		}
	}

	// Token: 0x060052E8 RID: 21224 RVA: 0x001DF3AB File Offset: 0x001DD7AB
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x060052E9 RID: 21225 RVA: 0x001DF3B4 File Offset: 0x001DD7B4
	public void StartSong(AudioClip ac)
	{
		this.Init();
		this.rhythmTool.NewSong(ac);
		this.rhythmTool.Play();
		this.low = this.rhythmTool.GetResults("Low");
		this.mid = this.rhythmTool.GetResults("Mid");
		this.high = this.rhythmTool.GetResults("High");
	}

	// Token: 0x060052EA RID: 21226 RVA: 0x001DF421 File Offset: 0x001DD821
	private void Update()
	{
		this.rhythmTool.Update();
	}

	// Token: 0x040042AD RID: 17069
	public Atom containingAtom;

	// Token: 0x040042AE RID: 17070
	public RhythmTool rhythmTool;

	// Token: 0x040042AF RID: 17071
	public Frame[] low;

	// Token: 0x040042B0 RID: 17072
	public Frame[] mid;

	// Token: 0x040042B1 RID: 17073
	public Frame[] high;

	// Token: 0x040042B2 RID: 17074
	private int index;

	// Token: 0x040042B3 RID: 17075
	protected bool _wasInit;
}
