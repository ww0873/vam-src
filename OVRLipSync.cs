using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020007D0 RID: 2000
public class OVRLipSync : MonoBehaviour
{
	// Token: 0x060032B5 RID: 12981 RVA: 0x00107C02 File Offset: 0x00106002
	public OVRLipSync()
	{
	}

	// Token: 0x060032B6 RID: 12982
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_Initialize(int samplerate, int buffersize);

	// Token: 0x060032B7 RID: 12983
	[DllImport("OVRLipSync")]
	private static extern void ovrLipSyncDll_Shutdown();

	// Token: 0x060032B8 RID: 12984
	[DllImport("OVRLipSync")]
	private static extern IntPtr ovrLipSyncDll_GetVersion(ref int Major, ref int Minor, ref int Patch);

	// Token: 0x060032B9 RID: 12985
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_CreateContext(ref uint context, OVRLipSync.ContextProviders provider);

	// Token: 0x060032BA RID: 12986
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_DestroyContext(uint context);

	// Token: 0x060032BB RID: 12987
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_ResetContext(uint context);

	// Token: 0x060032BC RID: 12988
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_SendSignal(uint context, OVRLipSync.Signals signal, int arg1, int arg2);

	// Token: 0x060032BD RID: 12989
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_ProcessFrame(uint context, float[] audioBuffer, OVRLipSync.Flags flags, ref int frameNumber, ref int frameDelay, float[] visemes, int visemeCount);

	// Token: 0x060032BE RID: 12990
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_ProcessFrameInterleaved(uint context, float[] audioBuffer, OVRLipSync.Flags flags, ref int frameNumber, ref int frameDelay, float[] visemes, int visemeCount);

	// Token: 0x060032BF RID: 12991 RVA: 0x00107C0C File Offset: 0x0010600C
	private void Awake()
	{
		if (OVRLipSync.sInstance == null)
		{
			OVRLipSync.sInstance = this;
			int outputSampleRate = AudioSettings.outputSampleRate;
			int num;
			int num2;
			AudioSettings.GetDSPBufferSize(out num, out num2);
			string message = string.Format("OvrLipSync Awake: Queried SampleRate: {0:F0} BufferSize: {1:F0}", outputSampleRate, num);
			Debug.LogWarning(message);
			OVRLipSync.sInitialized = OVRLipSync.Initialize(outputSampleRate, num);
			if (OVRLipSync.sInitialized != OVRLipSync.Result.Success)
			{
				Debug.LogWarning(string.Format("OvrLipSync Awake: Failed to init Speech Rec library", new object[0]));
			}
			OVRTouchpad.Create();
			return;
		}
		Debug.LogWarning(string.Format("OVRLipSync Awake: Only one instance of OVRPLipSync can exist in the scene.", new object[0]));
	}

	// Token: 0x060032C0 RID: 12992 RVA: 0x00107CA6 File Offset: 0x001060A6
	private void OnDestroy()
	{
		if (OVRLipSync.sInstance != this)
		{
			Debug.LogWarning("OVRLipSync OnDestroy: This is not the correct OVRLipSync instance.");
			return;
		}
	}

	// Token: 0x060032C1 RID: 12993 RVA: 0x00107CC3 File Offset: 0x001060C3
	public static OVRLipSync.Result Initialize(int sampleRate, int bufferSize)
	{
		OVRLipSync.sInitialized = (OVRLipSync.Result)OVRLipSync.ovrLipSyncDll_Initialize(sampleRate, bufferSize);
		return OVRLipSync.sInitialized;
	}

	// Token: 0x060032C2 RID: 12994 RVA: 0x00107CD6 File Offset: 0x001060D6
	public static void Shutdown()
	{
		OVRLipSync.ovrLipSyncDll_Shutdown();
		OVRLipSync.sInitialized = OVRLipSync.Result.Unknown;
	}

	// Token: 0x060032C3 RID: 12995 RVA: 0x00107CE7 File Offset: 0x001060E7
	public static OVRLipSync.Result IsInitialized()
	{
		return OVRLipSync.sInitialized;
	}

	// Token: 0x060032C4 RID: 12996 RVA: 0x00107CEE File Offset: 0x001060EE
	public static OVRLipSync.Result CreateContext(ref uint context, OVRLipSync.ContextProviders provider)
	{
		if (OVRLipSync.IsInitialized() != OVRLipSync.Result.Success)
		{
			return OVRLipSync.Result.CannotCreateContext;
		}
		return (OVRLipSync.Result)OVRLipSync.ovrLipSyncDll_CreateContext(ref context, provider);
	}

	// Token: 0x060032C5 RID: 12997 RVA: 0x00107D07 File Offset: 0x00106107
	public static OVRLipSync.Result DestroyContext(uint context)
	{
		if (OVRLipSync.IsInitialized() != OVRLipSync.Result.Success)
		{
			return OVRLipSync.Result.Unknown;
		}
		return (OVRLipSync.Result)OVRLipSync.ovrLipSyncDll_DestroyContext(context);
	}

	// Token: 0x060032C6 RID: 12998 RVA: 0x00107D1F File Offset: 0x0010611F
	public static OVRLipSync.Result ResetContext(uint context)
	{
		if (OVRLipSync.IsInitialized() != OVRLipSync.Result.Success)
		{
			return OVRLipSync.Result.Unknown;
		}
		return (OVRLipSync.Result)OVRLipSync.ovrLipSyncDll_ResetContext(context);
	}

	// Token: 0x060032C7 RID: 12999 RVA: 0x00107D37 File Offset: 0x00106137
	public static OVRLipSync.Result SendSignal(uint context, OVRLipSync.Signals signal, int arg1, int arg2)
	{
		if (OVRLipSync.IsInitialized() != OVRLipSync.Result.Success)
		{
			return OVRLipSync.Result.Unknown;
		}
		return (OVRLipSync.Result)OVRLipSync.ovrLipSyncDll_SendSignal(context, signal, arg1, arg2);
	}

	// Token: 0x060032C8 RID: 13000 RVA: 0x00107D52 File Offset: 0x00106152
	public static OVRLipSync.Result ProcessFrame(uint context, float[] audioBuffer, OVRLipSync.Flags flags, OVRLipSync.Frame frame)
	{
		if (OVRLipSync.IsInitialized() != OVRLipSync.Result.Success)
		{
			return OVRLipSync.Result.Unknown;
		}
		return (OVRLipSync.Result)OVRLipSync.ovrLipSyncDll_ProcessFrame(context, audioBuffer, flags, ref frame.frameNumber, ref frame.frameDelay, frame.Visemes, frame.Visemes.Length);
	}

	// Token: 0x060032C9 RID: 13001 RVA: 0x00107D86 File Offset: 0x00106186
	public static OVRLipSync.Result ProcessFrameInterleaved(uint context, float[] audioBuffer, OVRLipSync.Flags flags, OVRLipSync.Frame frame)
	{
		if (OVRLipSync.IsInitialized() != OVRLipSync.Result.Success)
		{
			return OVRLipSync.Result.Unknown;
		}
		return (OVRLipSync.Result)OVRLipSync.ovrLipSyncDll_ProcessFrameInterleaved(context, audioBuffer, flags, ref frame.frameNumber, ref frame.frameDelay, frame.Visemes, frame.Visemes.Length);
	}

	// Token: 0x060032CA RID: 13002 RVA: 0x00107DBA File Offset: 0x001061BA
	// Note: this type is marked as 'beforefieldinit'.
	static OVRLipSync()
	{
	}

	// Token: 0x040026A3 RID: 9891
	public static readonly int VisemeCount = Enum.GetNames(typeof(OVRLipSync.Viseme)).Length;

	// Token: 0x040026A4 RID: 9892
	public static readonly int SignalCount = Enum.GetNames(typeof(OVRLipSync.Signals)).Length;

	// Token: 0x040026A5 RID: 9893
	public const string strOVRLS = "OVRLipSync";

	// Token: 0x040026A6 RID: 9894
	private static OVRLipSync.Result sInitialized = OVRLipSync.Result.Unknown;

	// Token: 0x040026A7 RID: 9895
	public static OVRLipSync sInstance = null;

	// Token: 0x020007D1 RID: 2001
	public enum Result
	{
		// Token: 0x040026A9 RID: 9897
		Success,
		// Token: 0x040026AA RID: 9898
		Unknown = -2200,
		// Token: 0x040026AB RID: 9899
		CannotCreateContext = -2201,
		// Token: 0x040026AC RID: 9900
		InvalidParam = -2202,
		// Token: 0x040026AD RID: 9901
		BadSampleRate = -2203,
		// Token: 0x040026AE RID: 9902
		MissingDLL = -2204,
		// Token: 0x040026AF RID: 9903
		BadVersion = -2205,
		// Token: 0x040026B0 RID: 9904
		UndefinedFunction = -2206
	}

	// Token: 0x020007D2 RID: 2002
	public enum Viseme
	{
		// Token: 0x040026B2 RID: 9906
		sil,
		// Token: 0x040026B3 RID: 9907
		PP,
		// Token: 0x040026B4 RID: 9908
		FF,
		// Token: 0x040026B5 RID: 9909
		TH,
		// Token: 0x040026B6 RID: 9910
		DD,
		// Token: 0x040026B7 RID: 9911
		kk,
		// Token: 0x040026B8 RID: 9912
		CH,
		// Token: 0x040026B9 RID: 9913
		SS,
		// Token: 0x040026BA RID: 9914
		nn,
		// Token: 0x040026BB RID: 9915
		RR,
		// Token: 0x040026BC RID: 9916
		aa,
		// Token: 0x040026BD RID: 9917
		E,
		// Token: 0x040026BE RID: 9918
		ih,
		// Token: 0x040026BF RID: 9919
		oh,
		// Token: 0x040026C0 RID: 9920
		ou
	}

	// Token: 0x020007D3 RID: 2003
	public enum Flags
	{
		// Token: 0x040026C2 RID: 9922
		None,
		// Token: 0x040026C3 RID: 9923
		DelayCompensateAudio
	}

	// Token: 0x020007D4 RID: 2004
	public enum Signals
	{
		// Token: 0x040026C5 RID: 9925
		VisemeOn,
		// Token: 0x040026C6 RID: 9926
		VisemeOff,
		// Token: 0x040026C7 RID: 9927
		VisemeAmount,
		// Token: 0x040026C8 RID: 9928
		VisemeSmoothing
	}

	// Token: 0x020007D5 RID: 2005
	public enum ContextProviders
	{
		// Token: 0x040026CA RID: 9930
		Main,
		// Token: 0x040026CB RID: 9931
		Other
	}

	// Token: 0x020007D6 RID: 2006
	[Serializable]
	public class Frame
	{
		// Token: 0x060032CB RID: 13003 RVA: 0x00107DF8 File Offset: 0x001061F8
		public Frame()
		{
		}

		// Token: 0x060032CC RID: 13004 RVA: 0x00107E10 File Offset: 0x00106210
		public void CopyInput(OVRLipSync.Frame input)
		{
			this.frameNumber = input.frameNumber;
			this.frameDelay = input.frameDelay;
			input.Visemes.CopyTo(this.Visemes, 0);
		}

		// Token: 0x040026CC RID: 9932
		public int frameNumber;

		// Token: 0x040026CD RID: 9933
		public int frameDelay;

		// Token: 0x040026CE RID: 9934
		public float[] Visemes = new float[OVRLipSync.VisemeCount];
	}
}
