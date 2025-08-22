using System;
using Oculus.Avatar;
using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine;

// Token: 0x02000774 RID: 1908
public class P2PManager
{
	// Token: 0x0600312F RID: 12591 RVA: 0x000FFC83 File Offset: 0x000FE083
	public P2PManager()
	{
		Net.SetPeerConnectRequestCallback(new Message<NetworkingPeer>.Callback(this.PeerConnectRequestCallback));
		Net.SetConnectionStateChangedCallback(new Message<NetworkingPeer>.Callback(this.ConnectionStateChangedCallback));
	}

	// Token: 0x06003130 RID: 12592 RVA: 0x000FFCAD File Offset: 0x000FE0AD
	public void ConnectTo(ulong userID)
	{
		if (PlatformManager.MyID < userID)
		{
			Net.Connect(userID);
			PlatformManager.LogOutput("P2P connect to " + userID);
		}
	}

	// Token: 0x06003131 RID: 12593 RVA: 0x000FFCD8 File Offset: 0x000FE0D8
	public void Disconnect(ulong userID)
	{
		if (userID != 0UL)
		{
			Net.Close(userID);
			RemotePlayer remoteUser = PlatformManager.GetRemoteUser(userID);
			if (remoteUser != null)
			{
				remoteUser.p2pConnectionState = PeerConnectionState.Unknown;
			}
		}
	}

	// Token: 0x06003132 RID: 12594 RVA: 0x000FFD08 File Offset: 0x000FE108
	private void PeerConnectRequestCallback(Message<NetworkingPeer> msg)
	{
		PlatformManager.LogOutput("P2P request from " + msg.Data.ID);
		RemotePlayer remoteUser = PlatformManager.GetRemoteUser(msg.Data.ID);
		if (remoteUser != null)
		{
			PlatformManager.LogOutput("P2P request accepted from " + msg.Data.ID);
			Net.Accept(msg.Data.ID);
		}
	}

	// Token: 0x06003133 RID: 12595 RVA: 0x000FFD7C File Offset: 0x000FE17C
	private void ConnectionStateChangedCallback(Message<NetworkingPeer> msg)
	{
		PlatformManager.LogOutput(string.Concat(new object[]
		{
			"P2P state to ",
			msg.Data.ID,
			" changed to  ",
			msg.Data.State
		}));
		RemotePlayer remoteUser = PlatformManager.GetRemoteUser(msg.Data.ID);
		if (remoteUser != null)
		{
			remoteUser.p2pConnectionState = msg.Data.State;
			if (msg.Data.State == PeerConnectionState.Timeout && PlatformManager.MyID < msg.Data.ID)
			{
				Net.Connect(msg.Data.ID);
				PlatformManager.LogOutput("P2P re-connect to " + msg.Data.ID);
			}
		}
	}

	// Token: 0x06003134 RID: 12596 RVA: 0x000FFE4C File Offset: 0x000FE24C
	public void SendAvatarUpdate(ulong userID, Transform bodyTransform, uint sequence, byte[] avatarPacket)
	{
		byte[] array = new byte[avatarPacket.Length + P2PManager.POSITION_DATA_LENGTH];
		array[0] = P2PManager.UPDATE_PACKET;
		int dstOffset = 1;
		this.PackULong(PlatformManager.MyID, array, ref dstOffset);
		this.PackFloat(bodyTransform.localPosition.x, array, ref dstOffset);
		this.PackFloat(bodyTransform.localPosition.y, array, ref dstOffset);
		this.PackFloat(bodyTransform.localPosition.z, array, ref dstOffset);
		this.PackFloat(bodyTransform.localRotation.x, array, ref dstOffset);
		this.PackFloat(bodyTransform.localRotation.y, array, ref dstOffset);
		this.PackFloat(bodyTransform.localRotation.z, array, ref dstOffset);
		this.PackFloat(bodyTransform.localRotation.w, array, ref dstOffset);
		this.PackUInt32(sequence, array, ref dstOffset);
		Buffer.BlockCopy(avatarPacket, 0, array, dstOffset, avatarPacket.Length);
		Net.SendPacket(userID, array, SendPolicy.Unreliable);
	}

	// Token: 0x06003135 RID: 12597 RVA: 0x000FFF48 File Offset: 0x000FE348
	private void PackFloat(float f, byte[] buf, ref int offset)
	{
		Buffer.BlockCopy(BitConverter.GetBytes(f), 0, buf, offset, 4);
		offset += 4;
	}

	// Token: 0x06003136 RID: 12598 RVA: 0x000FFF60 File Offset: 0x000FE360
	private void PackULong(ulong u, byte[] buf, ref int offset)
	{
		Buffer.BlockCopy(BitConverter.GetBytes(u), 0, buf, offset, 8);
		offset += 8;
	}

	// Token: 0x06003137 RID: 12599 RVA: 0x000FFF78 File Offset: 0x000FE378
	private void PackUInt32(uint u, byte[] buf, ref int offset)
	{
		Buffer.BlockCopy(BitConverter.GetBytes(u), 0, buf, offset, 4);
		offset += 4;
	}

	// Token: 0x06003138 RID: 12600 RVA: 0x000FFF90 File Offset: 0x000FE390
	public void GetRemotePackets()
	{
		Packet packet;
		while ((packet = Net.ReadPacket()) != null)
		{
			byte[] array = new byte[packet.Size];
			packet.ReadBytes(array);
			if (array[0] != P2PManager.UPDATE_PACKET)
			{
				PlatformManager.LogOutput("Invalid packet type: " + packet.Size);
			}
			else
			{
				this.processAvatarPacket(ref array);
			}
		}
	}

	// Token: 0x06003139 RID: 12601 RVA: 0x000FFFF8 File Offset: 0x000FE3F8
	public void processAvatarPacket(ref byte[] packet)
	{
		ulong userID = BitConverter.ToUInt64(packet, 1);
		RemotePlayer remoteUser = PlatformManager.GetRemoteUser(userID);
		if (remoteUser != null)
		{
			remoteUser.receivedBodyPositionPrior = remoteUser.receivedBodyPosition;
			remoteUser.receivedBodyPosition.x = BitConverter.ToSingle(packet, 9);
			remoteUser.receivedBodyPosition.y = BitConverter.ToSingle(packet, 13) + P2PManager.HEIGHT_OFFSET;
			remoteUser.receivedBodyPosition.z = BitConverter.ToSingle(packet, 17);
			remoteUser.receivedBodyRotationPrior = remoteUser.receivedBodyRotation;
			remoteUser.receivedBodyRotation.x = BitConverter.ToSingle(packet, 21);
			remoteUser.receivedBodyRotation.y = BitConverter.ToSingle(packet, 25);
			remoteUser.receivedBodyRotation.z = BitConverter.ToSingle(packet, 29);
			remoteUser.receivedBodyRotation.w = BitConverter.ToSingle(packet, 33);
			int sequence = BitConverter.ToInt32(packet, 37);
			byte[] array = new byte[packet.Length - P2PManager.POSITION_DATA_LENGTH];
			Buffer.BlockCopy(packet, P2PManager.POSITION_DATA_LENGTH, array, 0, array.Length);
			IntPtr ovrNativePacket = Oculus.Avatar.CAPI.ovrAvatarPacket_Read((uint)array.Length, array);
			remoteUser.RemoteAvatar.GetComponent<OvrAvatarRemoteDriver>().QueuePacket(sequence, new OvrAvatarPacket
			{
				ovrNativePacket = ovrNativePacket
			});
			remoteUser.RemoteAvatar.transform.localPosition = remoteUser.receivedBodyPosition;
			remoteUser.RemoteAvatar.transform.localRotation = remoteUser.receivedBodyRotation;
		}
	}

	// Token: 0x0600313A RID: 12602 RVA: 0x0010014D File Offset: 0x000FE54D
	// Note: this type is marked as 'beforefieldinit'.
	static P2PManager()
	{
	}

	// Token: 0x040024F3 RID: 9459
	private static readonly byte UPDATE_PACKET = 1;

	// Token: 0x040024F4 RID: 9460
	private static readonly int POSITION_DATA_LENGTH = 41;

	// Token: 0x040024F5 RID: 9461
	private static readonly float HEIGHT_OFFSET = 0.65f;
}
