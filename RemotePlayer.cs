using System;
using Oculus.Platform;
using UnityEngine;

// Token: 0x02000778 RID: 1912
public class RemotePlayer
{
	// Token: 0x0600315D RID: 12637 RVA: 0x00100F87 File Offset: 0x000FF387
	public RemotePlayer()
	{
	}

	// Token: 0x04002526 RID: 9510
	public ulong remoteUserID;

	// Token: 0x04002527 RID: 9511
	public bool stillInRoom;

	// Token: 0x04002528 RID: 9512
	public PeerConnectionState p2pConnectionState;

	// Token: 0x04002529 RID: 9513
	public PeerConnectionState voipConnectionState;

	// Token: 0x0400252A RID: 9514
	public OvrAvatar RemoteAvatar;

	// Token: 0x0400252B RID: 9515
	public GameObject remotePlayerBody;

	// Token: 0x0400252C RID: 9516
	public Vector3 receivedBodyPosition;

	// Token: 0x0400252D RID: 9517
	public Vector3 receivedBodyPositionPrior;

	// Token: 0x0400252E RID: 9518
	public Quaternion receivedBodyRotation;

	// Token: 0x0400252F RID: 9519
	public Quaternion receivedBodyRotationPrior;
}
