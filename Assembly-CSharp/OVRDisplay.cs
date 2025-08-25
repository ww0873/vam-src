using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020008D0 RID: 2256
public class OVRDisplay
{
	// Token: 0x060038C5 RID: 14533 RVA: 0x00114987 File Offset: 0x00112D87
	public OVRDisplay()
	{
		this.UpdateTextures();
	}

	// Token: 0x060038C6 RID: 14534 RVA: 0x001149AC File Offset: 0x00112DAC
	public void Update()
	{
		this.UpdateTextures();
		if (this.recenterRequested && Time.frameCount > this.recenterRequestedFrameCount)
		{
			if (this.RecenteredPose != null)
			{
				this.RecenteredPose();
			}
			this.recenterRequested = false;
			this.recenterRequestedFrameCount = int.MaxValue;
		}
	}

	// Token: 0x140000BB RID: 187
	// (add) Token: 0x060038C7 RID: 14535 RVA: 0x00114A04 File Offset: 0x00112E04
	// (remove) Token: 0x060038C8 RID: 14536 RVA: 0x00114A3C File Offset: 0x00112E3C
	public event Action RecenteredPose
	{
		add
		{
			Action action = this.RecenteredPose;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.RecenteredPose, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.RecenteredPose;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.RecenteredPose, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x060038C9 RID: 14537 RVA: 0x00114A72 File Offset: 0x00112E72
	public void RecenterPose()
	{
		InputTracking.Recenter();
		this.recenterRequested = true;
		this.recenterRequestedFrameCount = Time.frameCount;
		OVRMixedReality.RecenterPose();
	}

	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x060038CA RID: 14538 RVA: 0x00114A90 File Offset: 0x00112E90
	public Vector3 acceleration
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return Vector3.zero;
			}
			return OVRPlugin.GetNodeAcceleration(OVRPlugin.Node.None, OVRPlugin.Step.Render).FromFlippedZVector3f();
		}
	}

	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x060038CB RID: 14539 RVA: 0x00114AAE File Offset: 0x00112EAE
	public Vector3 angularAcceleration
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return Vector3.zero;
			}
			return OVRPlugin.GetNodeAngularAcceleration(OVRPlugin.Node.None, OVRPlugin.Step.Render).FromFlippedZVector3f() * 57.29578f;
		}
	}

	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x060038CC RID: 14540 RVA: 0x00114AD6 File Offset: 0x00112ED6
	public Vector3 velocity
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return Vector3.zero;
			}
			return OVRPlugin.GetNodeVelocity(OVRPlugin.Node.None, OVRPlugin.Step.Render).FromFlippedZVector3f();
		}
	}

	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x060038CD RID: 14541 RVA: 0x00114AF4 File Offset: 0x00112EF4
	public Vector3 angularVelocity
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return Vector3.zero;
			}
			return OVRPlugin.GetNodeAngularVelocity(OVRPlugin.Node.None, OVRPlugin.Step.Render).FromFlippedZVector3f() * 57.29578f;
		}
	}

	// Token: 0x060038CE RID: 14542 RVA: 0x00114B1C File Offset: 0x00112F1C
	public OVRDisplay.EyeRenderDesc GetEyeRenderDesc(XRNode eye)
	{
		return this.eyeDescs[(int)eye];
	}

	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x060038CF RID: 14543 RVA: 0x00114B30 File Offset: 0x00112F30
	public OVRDisplay.LatencyData latency
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return default(OVRDisplay.LatencyData);
			}
			string latency = OVRPlugin.latency;
			Regex regex = new Regex("Render: ([0-9]+[.][0-9]+)ms, TimeWarp: ([0-9]+[.][0-9]+)ms, PostPresent: ([0-9]+[.][0-9]+)ms", RegexOptions.None);
			OVRDisplay.LatencyData result = default(OVRDisplay.LatencyData);
			Match match = regex.Match(latency);
			if (match.Success)
			{
				result.render = float.Parse(match.Groups[1].Value);
				result.timeWarp = float.Parse(match.Groups[2].Value);
				result.postPresent = float.Parse(match.Groups[3].Value);
			}
			return result;
		}
	}

	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x060038D0 RID: 14544 RVA: 0x00114BDB File Offset: 0x00112FDB
	public float appFramerate
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return 0f;
			}
			return OVRPlugin.GetAppFramerate();
		}
	}

	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x060038D1 RID: 14545 RVA: 0x00114BF4 File Offset: 0x00112FF4
	public int recommendedMSAALevel
	{
		get
		{
			int num = OVRPlugin.recommendedMSAALevel;
			if (num == 1)
			{
				num = 0;
			}
			return num;
		}
	}

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x060038D2 RID: 14546 RVA: 0x00114C11 File Offset: 0x00113011
	public float[] displayFrequenciesAvailable
	{
		get
		{
			return OVRPlugin.systemDisplayFrequenciesAvailable;
		}
	}

	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x060038D3 RID: 14547 RVA: 0x00114C18 File Offset: 0x00113018
	// (set) Token: 0x060038D4 RID: 14548 RVA: 0x00114C1F File Offset: 0x0011301F
	public float displayFrequency
	{
		get
		{
			return OVRPlugin.systemDisplayFrequency;
		}
		set
		{
			OVRPlugin.systemDisplayFrequency = value;
		}
	}

	// Token: 0x060038D5 RID: 14549 RVA: 0x00114C27 File Offset: 0x00113027
	private void UpdateTextures()
	{
		this.ConfigureEyeDesc(XRNode.LeftEye);
		this.ConfigureEyeDesc(XRNode.RightEye);
	}

	// Token: 0x060038D6 RID: 14550 RVA: 0x00114C38 File Offset: 0x00113038
	private void ConfigureEyeDesc(XRNode eye)
	{
		if (!OVRManager.isHmdPresent)
		{
			return;
		}
		OVRPlugin.Sizei eyeTextureSize = OVRPlugin.GetEyeTextureSize((OVRPlugin.Eye)eye);
		OVRPlugin.Frustumf eyeFrustum = OVRPlugin.GetEyeFrustum((OVRPlugin.Eye)eye);
		this.eyeDescs[(int)eye] = new OVRDisplay.EyeRenderDesc
		{
			resolution = new Vector2((float)eyeTextureSize.w, (float)eyeTextureSize.h),
			fov = 57.29578f * new Vector2(eyeFrustum.fovX, eyeFrustum.fovY)
		};
	}

	// Token: 0x040029F0 RID: 10736
	private bool needsConfigureTexture;

	// Token: 0x040029F1 RID: 10737
	private OVRDisplay.EyeRenderDesc[] eyeDescs = new OVRDisplay.EyeRenderDesc[2];

	// Token: 0x040029F2 RID: 10738
	private bool recenterRequested;

	// Token: 0x040029F3 RID: 10739
	private int recenterRequestedFrameCount = int.MaxValue;

	// Token: 0x040029F4 RID: 10740
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action RecenteredPose;

	// Token: 0x020008D1 RID: 2257
	public struct EyeRenderDesc
	{
		// Token: 0x040029F5 RID: 10741
		public Vector2 resolution;

		// Token: 0x040029F6 RID: 10742
		public Vector2 fov;
	}

	// Token: 0x020008D2 RID: 2258
	public struct LatencyData
	{
		// Token: 0x040029F7 RID: 10743
		public float render;

		// Token: 0x040029F8 RID: 10744
		public float timeWarp;

		// Token: 0x040029F9 RID: 10745
		public float postPresent;

		// Token: 0x040029FA RID: 10746
		public float renderError;

		// Token: 0x040029FB RID: 10747
		public float timeWarpError;
	}
}
