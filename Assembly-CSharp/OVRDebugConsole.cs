using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008B3 RID: 2227
public class OVRDebugConsole : MonoBehaviour
{
	// Token: 0x06003800 RID: 14336 RVA: 0x0010F566 File Offset: 0x0010D966
	public OVRDebugConsole()
	{
	}

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x06003801 RID: 14337 RVA: 0x0010F584 File Offset: 0x0010D984
	public static OVRDebugConsole instance
	{
		get
		{
			if (OVRDebugConsole.s_Instance == null)
			{
				OVRDebugConsole.s_Instance = (UnityEngine.Object.FindObjectOfType(typeof(OVRDebugConsole)) as OVRDebugConsole);
				if (OVRDebugConsole.s_Instance == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.AddComponent<OVRDebugConsole>();
					gameObject.name = "OVRDebugConsole";
					OVRDebugConsole.s_Instance = (UnityEngine.Object.FindObjectOfType(typeof(OVRDebugConsole)) as OVRDebugConsole);
				}
			}
			return OVRDebugConsole.s_Instance;
		}
	}

	// Token: 0x06003802 RID: 14338 RVA: 0x0010F600 File Offset: 0x0010DA00
	private void Awake()
	{
		OVRDebugConsole.s_Instance = this;
		this.Init();
	}

	// Token: 0x06003803 RID: 14339 RVA: 0x0010F610 File Offset: 0x0010DA10
	private void Update()
	{
		if (this.clearTimeoutOn)
		{
			this.clearTimeout -= Time.deltaTime;
			if (this.clearTimeout < 0f)
			{
				OVRDebugConsole.Clear();
				this.clearTimeout = 0f;
				this.clearTimeoutOn = false;
			}
		}
	}

	// Token: 0x06003804 RID: 14340 RVA: 0x0010F661 File Offset: 0x0010DA61
	public void Init()
	{
		if (this.textMsg == null)
		{
			Debug.LogWarning("DebugConsole Init WARNING::UI text not set. Will not be able to display anything.");
		}
		OVRDebugConsole.Clear();
	}

	// Token: 0x06003805 RID: 14341 RVA: 0x0010F683 File Offset: 0x0010DA83
	public static void Log(string message)
	{
		OVRDebugConsole.instance.AddMessage(message, Color.white);
	}

	// Token: 0x06003806 RID: 14342 RVA: 0x0010F695 File Offset: 0x0010DA95
	public static void Log(string message, Color color)
	{
		OVRDebugConsole.instance.AddMessage(message, color);
	}

	// Token: 0x06003807 RID: 14343 RVA: 0x0010F6A3 File Offset: 0x0010DAA3
	public static void Clear()
	{
		OVRDebugConsole.instance.ClearMessages();
	}

	// Token: 0x06003808 RID: 14344 RVA: 0x0010F6AF File Offset: 0x0010DAAF
	public static void ClearTimeout(float timeToClear)
	{
		OVRDebugConsole.instance.SetClearTimeout(timeToClear);
	}

	// Token: 0x06003809 RID: 14345 RVA: 0x0010F6BC File Offset: 0x0010DABC
	public void AddMessage(string message, Color color)
	{
		this.messages.Add(message);
		if (this.textMsg != null)
		{
			this.textMsg.color = color;
		}
		this.Display();
	}

	// Token: 0x0600380A RID: 14346 RVA: 0x0010F6EE File Offset: 0x0010DAEE
	public void ClearMessages()
	{
		this.messages.Clear();
		this.Display();
	}

	// Token: 0x0600380B RID: 14347 RVA: 0x0010F701 File Offset: 0x0010DB01
	public void SetClearTimeout(float timeout)
	{
		this.clearTimeout = timeout;
		this.clearTimeoutOn = true;
	}

	// Token: 0x0600380C RID: 14348 RVA: 0x0010F714 File Offset: 0x0010DB14
	private void Prune()
	{
		if (this.messages.Count > this.maxMessages)
		{
			int count;
			if (this.messages.Count <= 0)
			{
				count = 0;
			}
			else
			{
				count = this.messages.Count - this.maxMessages;
			}
			this.messages.RemoveRange(0, count);
		}
	}

	// Token: 0x0600380D RID: 14349 RVA: 0x0010F770 File Offset: 0x0010DB70
	private void Display()
	{
		if (this.messages.Count > this.maxMessages)
		{
			this.Prune();
		}
		if (this.textMsg != null)
		{
			this.textMsg.text = string.Empty;
			for (int i = 0; i < this.messages.Count; i++)
			{
				Text text = this.textMsg;
				text.text += (string)this.messages[i];
				Text text2 = this.textMsg;
				text2.text += '\n';
			}
		}
	}

	// Token: 0x0600380E RID: 14350 RVA: 0x0010F81A File Offset: 0x0010DC1A
	// Note: this type is marked as 'beforefieldinit'.
	static OVRDebugConsole()
	{
	}

	// Token: 0x0400292C RID: 10540
	public ArrayList messages = new ArrayList();

	// Token: 0x0400292D RID: 10541
	public int maxMessages = 15;

	// Token: 0x0400292E RID: 10542
	public Text textMsg;

	// Token: 0x0400292F RID: 10543
	private static OVRDebugConsole s_Instance;

	// Token: 0x04002930 RID: 10544
	private bool clearTimeoutOn;

	// Token: 0x04002931 RID: 10545
	private float clearTimeout;
}
