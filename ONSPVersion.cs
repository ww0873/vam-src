using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000982 RID: 2434
public class ONSPVersion : MonoBehaviour
{
	// Token: 0x06003CC9 RID: 15561 RVA: 0x00126780 File Offset: 0x00124B80
	public ONSPVersion()
	{
	}

	// Token: 0x06003CCA RID: 15562
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern void ONSP_GetVersion(ref int Major, ref int Minor, ref int Patch);

	// Token: 0x06003CCB RID: 15563 RVA: 0x00126788 File Offset: 0x00124B88
	private void Awake()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		ONSPVersion.ONSP_GetVersion(ref num, ref num2, ref num3);
		string message = string.Format("ONSP Version: {0:F0}.{1:F0}.{2:F0}", num, num2, num3);
		Debug.Log(message);
	}

	// Token: 0x06003CCC RID: 15564 RVA: 0x001267C9 File Offset: 0x00124BC9
	private void Start()
	{
	}

	// Token: 0x06003CCD RID: 15565 RVA: 0x001267CB File Offset: 0x00124BCB
	private void Update()
	{
	}

	// Token: 0x04002EA0 RID: 11936
	public const string strONSPS = "AudioPluginOculusSpatializer";
}
