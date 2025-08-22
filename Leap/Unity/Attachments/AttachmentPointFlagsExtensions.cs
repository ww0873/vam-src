using System;

namespace Leap.Unity.Attachments
{
	// Token: 0x0200066C RID: 1644
	public static class AttachmentPointFlagsExtensions
	{
		// Token: 0x06002845 RID: 10309 RVA: 0x000DE800 File Offset: 0x000DCC00
		public static bool IsSinglePoint(this AttachmentPointFlags points)
		{
			return points != AttachmentPointFlags.None && points == (points & -points);
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x000DE821 File Offset: 0x000DCC21
		public static bool ContainsPoint(this AttachmentPointFlags points, AttachmentPointFlags singlePoint)
		{
			return points.Contains(singlePoint);
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x000DE82A File Offset: 0x000DCC2A
		public static bool Contains(this AttachmentPointFlags points, AttachmentPointFlags otherPoints)
		{
			return points != AttachmentPointFlags.None && otherPoints != AttachmentPointFlags.None && (points & otherPoints) == otherPoints;
		}
	}
}
