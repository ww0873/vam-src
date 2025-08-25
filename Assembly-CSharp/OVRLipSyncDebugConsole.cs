using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007C3 RID: 1987
public class OVRLipSyncDebugConsole : MonoBehaviour
{
	// Token: 0x0600326F RID: 12911 RVA: 0x001070D0 File Offset: 0x001054D0
	public OVRLipSyncDebugConsole()
	{
	}

	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x06003270 RID: 12912 RVA: 0x001070EC File Offset: 0x001054EC
	public static OVRLipSyncDebugConsole instance
	{
		get
		{
			if (OVRLipSyncDebugConsole.s_Instance == null)
			{
				OVRLipSyncDebugConsole.s_Instance = (UnityEngine.Object.FindObjectOfType(typeof(OVRLipSyncDebugConsole)) as OVRLipSyncDebugConsole);
				if (OVRLipSyncDebugConsole.s_Instance == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.AddComponent<OVRLipSyncDebugConsole>();
					gameObject.name = "OVRLipSyncDebugConsole";
					OVRLipSyncDebugConsole.s_Instance = (UnityEngine.Object.FindObjectOfType(typeof(OVRLipSyncDebugConsole)) as OVRLipSyncDebugConsole);
				}
			}
			return OVRLipSyncDebugConsole.s_Instance;
		}
	}

	// Token: 0x06003271 RID: 12913 RVA: 0x00107168 File Offset: 0x00105568
	private void Awake()
	{
		OVRLipSyncDebugConsole.s_Instance = this;
		this.Init();
	}

	// Token: 0x06003272 RID: 12914 RVA: 0x00107178 File Offset: 0x00105578
	private void Update()
	{
		if (this.clearTimeoutOn)
		{
			this.clearTimeout -= Time.deltaTime;
			if (this.clearTimeout < 0f)
			{
				OVRLipSyncDebugConsole.Clear();
				this.clearTimeout = 0f;
				this.clearTimeoutOn = false;
			}
		}
	}

	// Token: 0x06003273 RID: 12915 RVA: 0x001071C9 File Offset: 0x001055C9
	public void Init()
	{
		if (this.textMsg == null)
		{
			Debug.LogWarning("DebugConsole Init WARNING::UI text not set. Will not be able to display anything.");
		}
		OVRLipSyncDebugConsole.Clear();
	}

	// Token: 0x06003274 RID: 12916 RVA: 0x001071EB File Offset: 0x001055EB
	public static void Log(string message)
	{
		OVRLipSyncDebugConsole.instance.AddMessage(message, Color.white);
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x001071FD File Offset: 0x001055FD
	public static void Log(string message, Color color)
	{
		OVRLipSyncDebugConsole.instance.AddMessage(message, color);
	}

	// Token: 0x06003276 RID: 12918 RVA: 0x0010720B File Offset: 0x0010560B
	public static void Clear()
	{
		OVRLipSyncDebugConsole.instance.ClearMessages();
	}

	// Token: 0x06003277 RID: 12919 RVA: 0x00107217 File Offset: 0x00105617
	public static void ClearTimeout(float timeToClear)
	{
		OVRLipSyncDebugConsole.instance.SetClearTimeout(timeToClear);
	}

	// Token: 0x06003278 RID: 12920 RVA: 0x00107224 File Offset: 0x00105624
	public void AddMessage(string message, Color color)
	{
		this.messages.Add(message);
		if (this.textMsg != null)
		{
			this.textMsg.color = color;
		}
		this.Display();
	}

	// Token: 0x06003279 RID: 12921 RVA: 0x00107256 File Offset: 0x00105656
	public void ClearMessages()
	{
		this.messages.Clear();
		this.Display();
	}

	// Token: 0x0600327A RID: 12922 RVA: 0x00107269 File Offset: 0x00105669
	public void SetClearTimeout(float timeout)
	{
		this.clearTimeout = timeout;
		this.clearTimeoutOn = true;
	}

	// Token: 0x0600327B RID: 12923 RVA: 0x0010727C File Offset: 0x0010567C
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

	// Token: 0x0600327C RID: 12924 RVA: 0x001072D8 File Offset: 0x001056D8
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

	// Token: 0x0600327D RID: 12925 RVA: 0x00107382 File Offset: 0x00105782
	// Note: this type is marked as 'beforefieldinit'.
	static OVRLipSyncDebugConsole()
	{
	}

	// Token: 0x0400268F RID: 9871
	public ArrayList messages = new ArrayList();

	// Token: 0x04002690 RID: 9872
	public int maxMessages = 15;

	// Token: 0x04002691 RID: 9873
	public Text textMsg;

	// Token: 0x04002692 RID: 9874
	private static OVRLipSyncDebugConsole s_Instance;

	// Token: 0x04002693 RID: 9875
	private bool clearTimeoutOn;

	// Token: 0x04002694 RID: 9876
	private float clearTimeout;
}
