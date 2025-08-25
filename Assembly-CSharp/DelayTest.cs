using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BD3 RID: 3027
public class DelayTest : MonoBehaviour
{
	// Token: 0x060055E9 RID: 21993 RVA: 0x001F6A1F File Offset: 0x001F4E1F
	public DelayTest()
	{
	}

	// Token: 0x17000C82 RID: 3202
	// (get) Token: 0x060055EA RID: 21994 RVA: 0x001F6A32 File Offset: 0x001F4E32
	// (set) Token: 0x060055EB RID: 21995 RVA: 0x001F6A3A File Offset: 0x001F4E3A
	public float delayTime
	{
		get
		{
			return this._delayTime;
		}
		set
		{
			this._delayTime = value;
		}
	}

	// Token: 0x060055EC RID: 21996 RVA: 0x001F6A43 File Offset: 0x001F4E43
	private void Start()
	{
		this.stopwatch = new Stopwatch();
		this.f = 1000f / (float)Stopwatch.Frequency;
		this.stopwatch.Start();
	}

	// Token: 0x060055ED RID: 21997 RVA: 0x001F6A70 File Offset: 0x001F4E70
	private void Update()
	{
		if (this.fpstext != null)
		{
			this.timer += Time.deltaTime;
			this.frameCount++;
			if (this.timer >= this.fpsUpdateInterval)
			{
				float num = (float)this.frameCount / this.timer;
				this.fpstext.text = "FPS: " + num.ToString("F2");
				this.frameCount = 0;
				this.timer = 0f;
			}
		}
		if (this.delayTime > 0f)
		{
			float num2 = (float)this.stopwatch.ElapsedTicks * this.f;
			float num4;
			do
			{
				float num3 = (float)this.stopwatch.ElapsedTicks * this.f;
				num4 = num3 - num2;
			}
			while (num4 <= this.delayTime);
		}
		if (this.uitext != null)
		{
			this.uitext.text = this.delayTime.ToString("F2");
		}
		if (this.controlAxis != string.Empty)
		{
			float axis = Input.GetAxis(this.controlAxis);
			if (axis <= 0.01f || axis >= 0.01f)
			{
				this.delayTime += axis * 0.01f;
			}
		}
	}

	// Token: 0x04004717 RID: 18199
	[SerializeField]
	private float _delayTime;

	// Token: 0x04004718 RID: 18200
	public Text uitext;

	// Token: 0x04004719 RID: 18201
	public Text fpstext;

	// Token: 0x0400471A RID: 18202
	public float fpsUpdateInterval = 0.5f;

	// Token: 0x0400471B RID: 18203
	public string controlAxis;

	// Token: 0x0400471C RID: 18204
	private Stopwatch stopwatch;

	// Token: 0x0400471D RID: 18205
	private float f;

	// Token: 0x0400471E RID: 18206
	private float timer;

	// Token: 0x0400471F RID: 18207
	private int frameCount;
}
