using System;
using UnityEngine;

// Token: 0x02000DBC RID: 3516
public class DebugHUD : MonoBehaviour
{
	// Token: 0x06006CFA RID: 27898 RVA: 0x002912BD File Offset: 0x0028F6BD
	public DebugHUD()
	{
	}

	// Token: 0x06006CFB RID: 27899 RVA: 0x002912D0 File Offset: 0x0028F6D0
	public static void Msg(string msg)
	{
		if (DebugHUD.DebugT != null)
		{
			DebugHUD.DebugT.AppendTextLine(msg);
			DebugHUD.DebugT.Splash();
		}
	}

	// Token: 0x06006CFC RID: 27900 RVA: 0x002912F8 File Offset: 0x0028F6F8
	public static void Alert1()
	{
		if (DebugHUD.singleton != null)
		{
			DebugHUD.singleton.alert1Timer = DebugHUD.singleton.alertDisplayTime;
			if (DebugHUD.singleton.alert1Object != null)
			{
				DebugHUD.singleton.alert1Object.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06006CFD RID: 27901 RVA: 0x00291354 File Offset: 0x0028F754
	public static void Alert2()
	{
		if (DebugHUD.singleton != null)
		{
			DebugHUD.singleton.alert2Timer = DebugHUD.singleton.alertDisplayTime;
			if (DebugHUD.singleton.alert2Object != null)
			{
				DebugHUD.singleton.alert2Object.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06006CFE RID: 27902 RVA: 0x002913B0 File Offset: 0x0028F7B0
	public static void Alert3()
	{
		if (DebugHUD.singleton != null)
		{
			DebugHUD.singleton.alert3Timer = DebugHUD.singleton.alertDisplayTime;
			if (DebugHUD.singleton.alert3Object != null)
			{
				DebugHUD.singleton.alert3Object.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06006CFF RID: 27903 RVA: 0x0029140B File Offset: 0x0028F80B
	private void Start()
	{
		DebugHUD.singleton = this;
		if (this.DebugText != null)
		{
			DebugHUD.DebugT = this.DebugText.GetComponent<HUDText>();
		}
	}

	// Token: 0x06006D00 RID: 27904 RVA: 0x00291434 File Offset: 0x0028F834
	private void Update()
	{
		if (this.alert1Timer >= 0f)
		{
			this.alert1Timer -= Time.deltaTime;
		}
		else if (this.alert1Timer < 0f && this.alert1Object != null && this.alert1Object.gameObject.activeSelf)
		{
			this.alert1Object.gameObject.SetActive(false);
		}
		if (this.alert2Timer >= 0f)
		{
			this.alert2Timer -= Time.deltaTime;
		}
		else if (this.alert2Timer < 0f && this.alert2Object != null && this.alert2Object.gameObject.activeSelf)
		{
			this.alert2Object.gameObject.SetActive(false);
		}
		if (this.alert3Timer >= 0f)
		{
			this.alert3Timer -= Time.deltaTime;
		}
		else if (this.alert3Timer < 0f && this.alert3Object != null && this.alert3Object.gameObject.activeSelf)
		{
			this.alert3Object.gameObject.SetActive(false);
		}
	}

	// Token: 0x04005E76 RID: 24182
	public static DebugHUD singleton;

	// Token: 0x04005E77 RID: 24183
	public Transform DebugText;

	// Token: 0x04005E78 RID: 24184
	public float alertDisplayTime = 0.1f;

	// Token: 0x04005E79 RID: 24185
	public Transform alert1Object;

	// Token: 0x04005E7A RID: 24186
	public Transform alert2Object;

	// Token: 0x04005E7B RID: 24187
	public Transform alert3Object;

	// Token: 0x04005E7C RID: 24188
	protected float alert1Timer;

	// Token: 0x04005E7D RID: 24189
	protected float alert2Timer;

	// Token: 0x04005E7E RID: 24190
	protected float alert3Timer;

	// Token: 0x04005E7F RID: 24191
	private static HUDText DebugT;
}
