using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020008B6 RID: 2230
public class OVRVoiceMod : MonoBehaviour
{
	// Token: 0x06003822 RID: 14370 RVA: 0x0010FCE8 File Offset: 0x0010E0E8
	public OVRVoiceMod()
	{
	}

	// Token: 0x06003823 RID: 14371
	[DllImport("OVRVoiceMod")]
	private static extern int ovrVoiceModDll_Initialize(int SampleRate, int BufferSize);

	// Token: 0x06003824 RID: 14372
	[DllImport("OVRVoiceMod")]
	private static extern void ovrVoiceModDll_Shutdown();

	// Token: 0x06003825 RID: 14373
	[DllImport("OVRVoiceMod")]
	private static extern IntPtr ovrVoicemodDll_GetVersion(ref int Major, ref int Minor, ref int Patch);

	// Token: 0x06003826 RID: 14374
	[DllImport("OVRVoiceMod")]
	private static extern int ovrVoiceModDll_CreateContext(ref uint Context);

	// Token: 0x06003827 RID: 14375
	[DllImport("OVRVoiceMod")]
	private static extern int ovrVoiceModDll_DestroyContext(uint Context);

	// Token: 0x06003828 RID: 14376
	[DllImport("OVRVoiceMod")]
	private static extern int ovrVoiceModDll_SendParameter(uint Context, int Parameter, int Value);

	// Token: 0x06003829 RID: 14377
	[DllImport("OVRVoiceMod")]
	private static extern int ovrVoiceModDll_ProcessFrame(uint Context, uint Flags, float[] AudioBuffer);

	// Token: 0x0600382A RID: 14378
	[DllImport("OVRVoiceMod")]
	private static extern int ovrVoiceModDll_ProcessFrameInterleaved(uint Context, uint Flags, float[] AudioBuffer);

	// Token: 0x0600382B RID: 14379
	[DllImport("OVRVoiceMod")]
	private static extern int ovrVoiceModDll_GetAverageAbsVolume(uint Context, ref float Volume);

	// Token: 0x0600382C RID: 14380 RVA: 0x0010FCF0 File Offset: 0x0010E0F0
	private void Awake()
	{
		if (OVRVoiceMod.sInstance == null)
		{
			OVRVoiceMod.sInstance = this;
			int outputSampleRate = AudioSettings.outputSampleRate;
			int num;
			int num2;
			AudioSettings.GetDSPBufferSize(out num, out num2);
			string message = string.Format("OvrVoiceMod Awake: Queried SampleRate: {0:F0} BufferSize: {1:F0}", outputSampleRate, num);
			Debug.LogWarning(message);
			OVRVoiceMod.sOVRVoiceModInit = OVRVoiceMod.ovrVoiceModDll_Initialize(outputSampleRate, num);
			if (OVRVoiceMod.sOVRVoiceModInit != 0)
			{
				Debug.LogWarning(string.Format("OvrVoiceMod Awake: Failed to init VoiceMod library", new object[0]));
			}
			OVRTouchpad.Create();
			return;
		}
		Debug.LogWarning(string.Format("OVRVoiceMod Awake: Only one instance of OVRVoiceMod can exist in the scene.", new object[0]));
	}

	// Token: 0x0600382D RID: 14381 RVA: 0x0010FD8A File Offset: 0x0010E18A
	private void Start()
	{
	}

	// Token: 0x0600382E RID: 14382 RVA: 0x0010FD8C File Offset: 0x0010E18C
	private void Update()
	{
	}

	// Token: 0x0600382F RID: 14383 RVA: 0x0010FD8E File Offset: 0x0010E18E
	private void OnDestroy()
	{
		if (OVRVoiceMod.sInstance != this)
		{
			Debug.LogWarning("OVRVoiceMod OnDestroy: This is not the correct OVRVoiceMod instance.");
		}
		OVRVoiceMod.ovrVoiceModDll_Shutdown();
		OVRVoiceMod.sOVRVoiceModInit = -2250;
	}

	// Token: 0x06003830 RID: 14384 RVA: 0x0010FDB9 File Offset: 0x0010E1B9
	public static int IsInitialized()
	{
		return OVRVoiceMod.sOVRVoiceModInit;
	}

	// Token: 0x06003831 RID: 14385 RVA: 0x0010FDC0 File Offset: 0x0010E1C0
	public static int CreateContext(ref uint context)
	{
		if (OVRVoiceMod.IsInitialized() != 0)
		{
			return -2251;
		}
		return OVRVoiceMod.ovrVoiceModDll_CreateContext(ref context);
	}

	// Token: 0x06003832 RID: 14386 RVA: 0x0010FDD8 File Offset: 0x0010E1D8
	public static int DestroyContext(uint context)
	{
		if (OVRVoiceMod.IsInitialized() != 0)
		{
			return -2250;
		}
		return OVRVoiceMod.ovrVoiceModDll_DestroyContext(context);
	}

	// Token: 0x06003833 RID: 14387 RVA: 0x0010FDF0 File Offset: 0x0010E1F0
	public static int SendParameter(uint context, int parameter, int value)
	{
		if (OVRVoiceMod.IsInitialized() != 0)
		{
			return -2250;
		}
		return OVRVoiceMod.ovrVoiceModDll_SendParameter(context, parameter, value);
	}

	// Token: 0x06003834 RID: 14388 RVA: 0x0010FE0A File Offset: 0x0010E20A
	public static int ProcessFrame(uint context, float[] audioBuffer)
	{
		if (OVRVoiceMod.IsInitialized() != 0)
		{
			return -2250;
		}
		return OVRVoiceMod.ovrVoiceModDll_ProcessFrame(context, 0U, audioBuffer);
	}

	// Token: 0x06003835 RID: 14389 RVA: 0x0010FE24 File Offset: 0x0010E224
	public static int ProcessFrameInterleaved(uint context, float[] audioBuffer)
	{
		if (OVRVoiceMod.IsInitialized() != 0)
		{
			return -2250;
		}
		return OVRVoiceMod.ovrVoiceModDll_ProcessFrameInterleaved(context, 0U, audioBuffer);
	}

	// Token: 0x06003836 RID: 14390 RVA: 0x0010FE40 File Offset: 0x0010E240
	public static float GetAverageAbsVolume(uint context)
	{
		if (OVRVoiceMod.IsInitialized() != 0)
		{
			return 0f;
		}
		return 0f;
	}

	// Token: 0x06003837 RID: 14391 RVA: 0x0010FE64 File Offset: 0x0010E264
	// Note: this type is marked as 'beforefieldinit'.
	static OVRVoiceMod()
	{
	}

	// Token: 0x04002942 RID: 10562
	public const int ovrVoiceModSuccess = 0;

	// Token: 0x04002943 RID: 10563
	public const string strOVRLS = "OVRVoiceMod";

	// Token: 0x04002944 RID: 10564
	private static int sOVRVoiceModInit = -2250;

	// Token: 0x04002945 RID: 10565
	public static OVRVoiceMod sInstance;

	// Token: 0x020008B7 RID: 2231
	public enum ovrVoiceModError
	{
		// Token: 0x04002947 RID: 10567
		Unknown = -2250,
		// Token: 0x04002948 RID: 10568
		CannotCreateContext = -2251,
		// Token: 0x04002949 RID: 10569
		InvalidParam = -2252,
		// Token: 0x0400294A RID: 10570
		BadSampleRate = -2253,
		// Token: 0x0400294B RID: 10571
		MissingDLL = -2254,
		// Token: 0x0400294C RID: 10572
		BadVersion = -2255,
		// Token: 0x0400294D RID: 10573
		UndefinedFunction = -2256
	}

	// Token: 0x020008B8 RID: 2232
	public enum ovrViceModFlag
	{
		// Token: 0x0400294F RID: 10575
		None
	}
}
