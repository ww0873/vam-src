using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x0200074B RID: 1867
	public class TransformHistory
	{
		// Token: 0x06002F7D RID: 12157 RVA: 0x000F71EB File Offset: 0x000F55EB
		public TransformHistory(int capacity = 32)
		{
			this.history = new RingBuffer<TransformHistory.TransformData>(capacity);
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x000F7200 File Offset: 0x000F5600
		public void UpdateDelay(Pose curPose, long timestamp)
		{
			TransformHistory.TransformData t = new TransformHistory.TransformData
			{
				time = timestamp,
				position = curPose.position,
				rotation = curPose.rotation
			};
			this.history.Add(t);
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x000F7248 File Offset: 0x000F5648
		public void SampleTransform(long timestamp, out Vector3 delayedPos, out Quaternion delayedRot)
		{
			TransformHistory.TransformData transformAtTime = TransformHistory.TransformData.GetTransformAtTime(this.history, timestamp);
			delayedPos = transformAtTime.position;
			delayedRot = transformAtTime.rotation;
		}

		// Token: 0x04002408 RID: 9224
		public RingBuffer<TransformHistory.TransformData> history;

		// Token: 0x0200074C RID: 1868
		public struct TransformData
		{
			// Token: 0x06002F80 RID: 12160 RVA: 0x000F727C File Offset: 0x000F567C
			public static TransformHistory.TransformData Lerp(TransformHistory.TransformData from, TransformHistory.TransformData to, long time)
			{
				if (from.time == to.time)
				{
					return from;
				}
				float t = (float)((double)(time - from.time) / (double)(to.time - from.time));
				return new TransformHistory.TransformData
				{
					time = time,
					position = Vector3.Lerp(from.position, to.position, t),
					rotation = Quaternion.Slerp(from.rotation, to.rotation, t)
				};
			}

			// Token: 0x06002F81 RID: 12161 RVA: 0x000F7304 File Offset: 0x000F5704
			public static TransformHistory.TransformData GetTransformAtTime(RingBuffer<TransformHistory.TransformData> history, long desiredTime)
			{
				for (int i = history.Count - 1; i > 0; i--)
				{
					if (history.Get(i).time >= desiredTime && history.Get(i - 1).time < desiredTime)
					{
						return TransformHistory.TransformData.Lerp(history.Get(i - 1), history.Get(i), desiredTime);
					}
				}
				if (history.Count > 0)
				{
					return history.GetLatest();
				}
				return new TransformHistory.TransformData
				{
					time = desiredTime,
					position = Vector3.zero,
					rotation = Quaternion.identity
				};
			}

			// Token: 0x04002409 RID: 9225
			public long time;

			// Token: 0x0400240A RID: 9226
			public Vector3 position;

			// Token: 0x0400240B RID: 9227
			public Quaternion rotation;
		}
	}
}
