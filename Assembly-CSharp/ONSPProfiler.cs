using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000981 RID: 2433
public class ONSPProfiler : MonoBehaviour
{
	// Token: 0x06003CC4 RID: 15556 RVA: 0x00126701 File Offset: 0x00124B01
	public ONSPProfiler()
	{
	}

	// Token: 0x06003CC5 RID: 15557 RVA: 0x00126714 File Offset: 0x00124B14
	private void Start()
	{
		Application.runInBackground = true;
		if (this.profilerEnabled)
		{
			Debug.Log("Oculus Audio Profiler enabled.");
		}
	}

	// Token: 0x06003CC6 RID: 15558 RVA: 0x00126734 File Offset: 0x00124B34
	private void Update()
	{
		if (this.port < 0 || this.port > 65535)
		{
			this.port = 2121;
		}
		ONSPProfiler.ONSP_SetProfilerPort(this.port);
		ONSPProfiler.ONSP_SetProfilerEnabled(this.profilerEnabled);
	}

	// Token: 0x06003CC7 RID: 15559
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int ONSP_SetProfilerEnabled(bool enabled);

	// Token: 0x06003CC8 RID: 15560
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int ONSP_SetProfilerPort(int port);

	// Token: 0x04002E9C RID: 11932
	public bool profilerEnabled;

	// Token: 0x04002E9D RID: 11933
	private const int DEFAULT_PORT = 2121;

	// Token: 0x04002E9E RID: 11934
	public int port = 2121;

	// Token: 0x04002E9F RID: 11935
	public const string strONSPS = "AudioPluginOculusSpatializer";
}
