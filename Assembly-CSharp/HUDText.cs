using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DC7 RID: 3527
public class HUDText : MonoBehaviour
{
	// Token: 0x06006D63 RID: 28003 RVA: 0x00292E84 File Offset: 0x00291284
	public HUDText()
	{
	}

	// Token: 0x06006D64 RID: 28004 RVA: 0x00292EB4 File Offset: 0x002912B4
	private void Start()
	{
		this.lines = new List<string>();
		this.mr = base.GetComponent<MeshRenderer>();
		this.tm = base.GetComponent<TextMesh>();
		if (this.StartHidden)
		{
			this.Hide();
		}
		else
		{
			this.Unhide();
		}
	}

	// Token: 0x06006D65 RID: 28005 RVA: 0x00292F00 File Offset: 0x00291300
	public void Splash()
	{
		if (this.hidden)
		{
			this.timerOn = true;
			this.timer = 2f;
			if (this.mr != null)
			{
				this.mr.enabled = true;
			}
		}
	}

	// Token: 0x06006D66 RID: 28006 RVA: 0x00292F3C File Offset: 0x0029133C
	public void Hide()
	{
		if (this.mr != null)
		{
			this.mr.enabled = false;
			this.hidden = true;
		}
	}

	// Token: 0x06006D67 RID: 28007 RVA: 0x00292F62 File Offset: 0x00291362
	public void Unhide()
	{
		this.timerOn = false;
		if (this.mr != null)
		{
			this.mr.enabled = true;
			this.hidden = false;
		}
	}

	// Token: 0x06006D68 RID: 28008 RVA: 0x00292F8F File Offset: 0x0029138F
	public string GetText()
	{
		if (this.tm != null)
		{
			return this.tm.text;
		}
		return string.Empty;
	}

	// Token: 0x06006D69 RID: 28009 RVA: 0x00292FB3 File Offset: 0x002913B3
	public void SetText(string text)
	{
		if (this.tm != null)
		{
			this.lines.Clear();
			this.lines.Add(text);
			this.tm.text = text;
		}
	}

	// Token: 0x06006D6A RID: 28010 RVA: 0x00292FE9 File Offset: 0x002913E9
	public void AppendTextLine(string text)
	{
		if (this.tm != null)
		{
			this.lines.Add(text);
			this.textNeedsUpdate = true;
		}
	}

	// Token: 0x06006D6B RID: 28011 RVA: 0x00293010 File Offset: 0x00291410
	private void Update()
	{
		if (this.timerOn)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				this.Hide();
				this.timerOn = false;
			}
		}
		this.timer2 -= Time.deltaTime;
		if (this.timer2 < 0f)
		{
			this.timer2 = this.UpdateInterval;
			if (this.textNeedsUpdate)
			{
				int num = this.lines.Count - this.DisplayLines;
				int count = this.lines.Count;
				if (num < 0)
				{
					num = 0;
				}
				this.tm.text = string.Empty;
				for (int i = num; i < count; i++)
				{
					TextMesh textMesh = this.tm;
					textMesh.text = textMesh.text + "\n" + this.lines[i];
				}
			}
		}
	}

	// Token: 0x04005ECE RID: 24270
	public float SplashTime = 2f;

	// Token: 0x04005ECF RID: 24271
	public int DisplayLines = 10;

	// Token: 0x04005ED0 RID: 24272
	public bool StartHidden = true;

	// Token: 0x04005ED1 RID: 24273
	public float UpdateInterval = 1f;

	// Token: 0x04005ED2 RID: 24274
	private bool timerOn;

	// Token: 0x04005ED3 RID: 24275
	private float timer;

	// Token: 0x04005ED4 RID: 24276
	private float timer2;

	// Token: 0x04005ED5 RID: 24277
	private MeshRenderer mr;

	// Token: 0x04005ED6 RID: 24278
	private TextMesh tm;

	// Token: 0x04005ED7 RID: 24279
	private bool hidden;

	// Token: 0x04005ED8 RID: 24280
	private List<string> lines;

	// Token: 0x04005ED9 RID: 24281
	private bool textNeedsUpdate;
}
