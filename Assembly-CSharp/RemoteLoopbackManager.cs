using System;
using System.Collections.Generic;
using System.IO;
using Oculus.Avatar;
using UnityEngine;

// Token: 0x02000771 RID: 1905
public class RemoteLoopbackManager : MonoBehaviour
{
	// Token: 0x06003126 RID: 12582 RVA: 0x000FF847 File Offset: 0x000FDC47
	public RemoteLoopbackManager()
	{
	}

	// Token: 0x06003127 RID: 12583 RVA: 0x000FF868 File Offset: 0x000FDC68
	private void Start()
	{
		this.LocalAvatar.RecordPackets = true;
		OvrAvatar localAvatar = this.LocalAvatar;
		localAvatar.PacketRecorded = (EventHandler<OvrAvatar.PacketEventArgs>)Delegate.Combine(localAvatar.PacketRecorded, new EventHandler<OvrAvatar.PacketEventArgs>(this.OnLocalAvatarPacketRecorded));
		float num = UnityEngine.Random.Range(this.LatencySettings.FakeLatencyMin, this.LatencySettings.FakeLatencyMax);
		this.LatencySettings.LatencyValues.AddFirst(num);
		this.LatencySettings.LatencySum += num;
	}

	// Token: 0x06003128 RID: 12584 RVA: 0x000FF8EC File Offset: 0x000FDCEC
	private void OnLocalAvatarPacketRecorded(object sender, OvrAvatar.PacketEventArgs args)
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			if (this.LocalAvatar.UseSDKPackets)
			{
				uint num = CAPI.ovrAvatarPacket_GetSize(args.Packet.ovrNativePacket);
				byte[] buffer = new byte[num];
				CAPI.ovrAvatarPacket_Write(args.Packet.ovrNativePacket, num, buffer);
				binaryWriter.Write(this.PacketSequence++);
				binaryWriter.Write(num);
				binaryWriter.Write(buffer);
			}
			else
			{
				binaryWriter.Write(this.PacketSequence++);
				args.Packet.Write(memoryStream);
			}
			this.SendPacketData(memoryStream.ToArray());
		}
	}

	// Token: 0x06003129 RID: 12585 RVA: 0x000FF9C4 File Offset: 0x000FDDC4
	private void Update()
	{
		if (this.packetQueue.Count > 0)
		{
			List<RemoteLoopbackManager.PacketLatencyPair> list = new List<RemoteLoopbackManager.PacketLatencyPair>();
			foreach (RemoteLoopbackManager.PacketLatencyPair packetLatencyPair in this.packetQueue)
			{
				packetLatencyPair.FakeLatency -= Time.deltaTime;
				if (packetLatencyPair.FakeLatency < 0f)
				{
					this.ReceivePacketData(packetLatencyPair.PacketData);
					list.Add(packetLatencyPair);
				}
			}
			foreach (RemoteLoopbackManager.PacketLatencyPair value in list)
			{
				this.packetQueue.Remove(value);
			}
		}
	}

	// Token: 0x0600312A RID: 12586 RVA: 0x000FFAB4 File Offset: 0x000FDEB4
	private void SendPacketData(byte[] data)
	{
		RemoteLoopbackManager.PacketLatencyPair packetLatencyPair = new RemoteLoopbackManager.PacketLatencyPair();
		packetLatencyPair.PacketData = data;
		packetLatencyPair.FakeLatency = this.LatencySettings.NextValue();
		this.packetQueue.AddLast(packetLatencyPair);
	}

	// Token: 0x0600312B RID: 12587 RVA: 0x000FFAEC File Offset: 0x000FDEEC
	private void ReceivePacketData(byte[] data)
	{
		using (MemoryStream memoryStream = new MemoryStream(data))
		{
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			int sequence = binaryReader.ReadInt32();
			OvrAvatarPacket packet;
			if (this.LoopbackAvatar.UseSDKPackets)
			{
				int count = binaryReader.ReadInt32();
				byte[] buffer = binaryReader.ReadBytes(count);
				IntPtr ovrNativePacket = CAPI.ovrAvatarPacket_Read((uint)data.Length, buffer);
				packet = new OvrAvatarPacket
				{
					ovrNativePacket = ovrNativePacket
				};
			}
			else
			{
				packet = OvrAvatarPacket.Read(memoryStream);
			}
			this.LoopbackAvatar.GetComponent<OvrAvatarRemoteDriver>().QueuePacket(sequence, packet);
		}
	}

	// Token: 0x040024E5 RID: 9445
	public OvrAvatar LocalAvatar;

	// Token: 0x040024E6 RID: 9446
	public OvrAvatar LoopbackAvatar;

	// Token: 0x040024E7 RID: 9447
	public RemoteLoopbackManager.SimulatedLatencySettings LatencySettings = new RemoteLoopbackManager.SimulatedLatencySettings();

	// Token: 0x040024E8 RID: 9448
	private int PacketSequence;

	// Token: 0x040024E9 RID: 9449
	private LinkedList<RemoteLoopbackManager.PacketLatencyPair> packetQueue = new LinkedList<RemoteLoopbackManager.PacketLatencyPair>();

	// Token: 0x02000772 RID: 1906
	private class PacketLatencyPair
	{
		// Token: 0x0600312C RID: 12588 RVA: 0x000FFB90 File Offset: 0x000FDF90
		public PacketLatencyPair()
		{
		}

		// Token: 0x040024EA RID: 9450
		public byte[] PacketData;

		// Token: 0x040024EB RID: 9451
		public float FakeLatency;
	}

	// Token: 0x02000773 RID: 1907
	[Serializable]
	public class SimulatedLatencySettings
	{
		// Token: 0x0600312D RID: 12589 RVA: 0x000FFB98 File Offset: 0x000FDF98
		public SimulatedLatencySettings()
		{
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x000FFBD4 File Offset: 0x000FDFD4
		public float NextValue()
		{
			this.AverageWindow = this.LatencySum / (float)this.LatencyValues.Count;
			float num = UnityEngine.Random.Range(this.FakeLatencyMin, this.FakeLatencyMax);
			float num2 = this.AverageWindow * (1f - this.LatencyWeight) + this.LatencyWeight * num;
			if (this.LatencyValues.Count >= this.MaxSamples)
			{
				this.LatencySum -= this.LatencyValues.First.Value;
				this.LatencyValues.RemoveFirst();
			}
			this.LatencySum += num2;
			this.LatencyValues.AddLast(num2);
			return num2;
		}

		// Token: 0x040024EC RID: 9452
		[Range(0f, 0.5f)]
		public float FakeLatencyMax = 0.25f;

		// Token: 0x040024ED RID: 9453
		[Range(0f, 0.5f)]
		public float FakeLatencyMin = 0.002f;

		// Token: 0x040024EE RID: 9454
		[Range(0f, 1f)]
		public float LatencyWeight = 0.25f;

		// Token: 0x040024EF RID: 9455
		[Range(0f, 10f)]
		public int MaxSamples = 4;

		// Token: 0x040024F0 RID: 9456
		internal float AverageWindow;

		// Token: 0x040024F1 RID: 9457
		internal float LatencySum;

		// Token: 0x040024F2 RID: 9458
		internal LinkedList<float> LatencyValues = new LinkedList<float>();
	}
}
