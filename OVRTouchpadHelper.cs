using System;
using UnityEngine;

// Token: 0x020007CF RID: 1999
public sealed class OVRTouchpadHelper : MonoBehaviour
{
	// Token: 0x060032AF RID: 12975 RVA: 0x00107B79 File Offset: 0x00105F79
	public OVRTouchpadHelper()
	{
	}

	// Token: 0x060032B0 RID: 12976 RVA: 0x00107B81 File Offset: 0x00105F81
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060032B1 RID: 12977 RVA: 0x00107B8E File Offset: 0x00105F8E
	private void Start()
	{
		OVRMessenger.AddListener<OVRTouchpad.TouchEvent>("Touchpad", new OVRCallback<OVRTouchpad.TouchEvent>(this.LocalTouchEventCallback));
	}

	// Token: 0x060032B2 RID: 12978 RVA: 0x00107BA6 File Offset: 0x00105FA6
	private void Update()
	{
		OVRTouchpad.Update();
	}

	// Token: 0x060032B3 RID: 12979 RVA: 0x00107BAD File Offset: 0x00105FAD
	public void OnDisable()
	{
		OVRTouchpad.OnDisable();
	}

	// Token: 0x060032B4 RID: 12980 RVA: 0x00107BB4 File Offset: 0x00105FB4
	private void LocalTouchEventCallback(OVRTouchpad.TouchEvent touchEvent)
	{
		switch (touchEvent)
		{
		}
	}
}
