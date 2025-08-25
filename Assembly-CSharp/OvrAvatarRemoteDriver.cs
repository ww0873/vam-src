using System;
using System.Collections.Generic;
using Oculus.Avatar;
using UnityEngine;

// Token: 0x02000793 RID: 1939
public class OvrAvatarRemoteDriver : OvrAvatarDriver
{
	// Token: 0x060031D6 RID: 12758 RVA: 0x00105649 File Offset: 0x00103A49
	public OvrAvatarRemoteDriver()
	{
	}

	// Token: 0x060031D7 RID: 12759 RVA: 0x0010566E File Offset: 0x00103A6E
	public void QueuePacket(int sequence, OvrAvatarPacket packet)
	{
		if (sequence > this.CurrentSequence)
		{
			this.CurrentSequence = sequence;
			this.packetQueue.Enqueue(packet);
		}
	}

	// Token: 0x060031D8 RID: 12760 RVA: 0x00105690 File Offset: 0x00103A90
	public override void UpdateTransforms(IntPtr sdkAvatar)
	{
		OvrAvatarDriver.PacketMode mode = this.Mode;
		if (mode != OvrAvatarDriver.PacketMode.SDK)
		{
			if (mode == OvrAvatarDriver.PacketMode.Unity)
			{
				this.UpdateFromUnityPacket(sdkAvatar);
			}
		}
		else
		{
			this.UpdateFromSDKPacket(sdkAvatar);
		}
	}

	// Token: 0x060031D9 RID: 12761 RVA: 0x001056D4 File Offset: 0x00103AD4
	private void UpdateFromSDKPacket(IntPtr sdkAvatar)
	{
		if (this.CurrentSDKPacket == IntPtr.Zero && this.packetQueue.Count >= 1)
		{
			this.CurrentSDKPacket = this.packetQueue.Dequeue().ovrNativePacket;
		}
		if (this.CurrentSDKPacket != IntPtr.Zero)
		{
			float num = CAPI.ovrAvatarPacket_GetDurationSeconds(this.CurrentSDKPacket);
			CAPI.ovrAvatar_UpdatePoseFromPacket(sdkAvatar, this.CurrentSDKPacket, Mathf.Min(num, this.CurrentPacketTime));
			this.CurrentPacketTime += Time.deltaTime;
			if (this.CurrentPacketTime > num)
			{
				CAPI.ovrAvatarPacket_Free(this.CurrentSDKPacket);
				this.CurrentSDKPacket = IntPtr.Zero;
				this.CurrentPacketTime -= num;
				while (this.packetQueue.Count > 4)
				{
					this.packetQueue.Dequeue();
				}
			}
		}
	}

	// Token: 0x060031DA RID: 12762 RVA: 0x001057BC File Offset: 0x00103BBC
	private void UpdateFromUnityPacket(IntPtr sdkAvatar)
	{
		if (!this.isStreaming && this.packetQueue.Count > 1)
		{
			this.currentPacket = this.packetQueue.Dequeue();
			this.isStreaming = true;
		}
		if (this.isStreaming)
		{
			this.CurrentPacketTime += Time.deltaTime;
			while (this.CurrentPacketTime > this.currentPacket.Duration)
			{
				if (this.packetQueue.Count == 0)
				{
					this.CurrentPose = this.currentPacket.FinalFrame;
					this.CurrentPacketTime = 0f;
					this.currentPacket = null;
					this.isStreaming = false;
					return;
				}
				while (this.packetQueue.Count > 4)
				{
					this.packetQueue.Dequeue();
				}
				this.CurrentPacketTime -= this.currentPacket.Duration;
				this.currentPacket = this.packetQueue.Dequeue();
			}
			this.CurrentPose = this.currentPacket.GetPoseFrame(this.CurrentPacketTime);
			base.UpdateTransformsFromPose(sdkAvatar);
		}
	}

	// Token: 0x040025AF RID: 9647
	private Queue<OvrAvatarPacket> packetQueue = new Queue<OvrAvatarPacket>();

	// Token: 0x040025B0 RID: 9648
	private IntPtr CurrentSDKPacket = IntPtr.Zero;

	// Token: 0x040025B1 RID: 9649
	private float CurrentPacketTime;

	// Token: 0x040025B2 RID: 9650
	private const int MinPacketQueue = 1;

	// Token: 0x040025B3 RID: 9651
	private const int MaxPacketQueue = 4;

	// Token: 0x040025B4 RID: 9652
	private int CurrentSequence = -1;

	// Token: 0x040025B5 RID: 9653
	private bool isStreaming;

	// Token: 0x040025B6 RID: 9654
	private OvrAvatarPacket currentPacket;
}
