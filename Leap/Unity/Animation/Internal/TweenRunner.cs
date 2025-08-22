using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity.Animation.Internal
{
	// Token: 0x02000665 RID: 1637
	public class TweenRunner : MonoBehaviour
	{
		// Token: 0x06002809 RID: 10249 RVA: 0x000DCB95 File Offset: 0x000DAF95
		public TweenRunner()
		{
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x0600280A RID: 10250 RVA: 0x000DCBB8 File Offset: 0x000DAFB8
		public static TweenRunner instance
		{
			get
			{
				if (TweenRunner._cachedInstance == null)
				{
					TweenRunner._cachedInstance = UnityEngine.Object.FindObjectOfType<TweenRunner>();
					if (TweenRunner._cachedInstance == null)
					{
						TweenRunner._cachedInstance = new GameObject("__Tween Runner__").AddComponent<TweenRunner>();
						TweenRunner._cachedInstance.gameObject.hideFlags = HideFlags.HideAndDontSave;
					}
				}
				return TweenRunner._cachedInstance;
			}
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x000DCC1C File Offset: 0x000DB01C
		private void Update()
		{
			int runningCount = this._runningCount;
			while (runningCount-- != 0)
			{
				TweenInstance tweenInstance = this._runningTweens[runningCount];
				try
				{
					tweenInstance.Step(this);
				}
				catch (Exception exception)
				{
					Debug.LogError("Error occured inside of tween!  Tween has been terminated");
					Debug.LogException(exception);
					if (tweenInstance.runnerIndex != -1)
					{
						this.RemoveTween(tweenInstance);
					}
				}
			}
			while (this._toRecycle.Count > 0)
			{
				TweenInstance tweenInstance2 = this._toRecycle.Dequeue();
				if (tweenInstance2.instanceId == -2)
				{
					tweenInstance2.Dispose();
				}
			}
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x000DCCC4 File Offset: 0x000DB0C4
		public void ScheduleForRecycle(TweenInstance instance)
		{
			instance.instanceId = -2;
			this._toRecycle.Enqueue(instance);
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x000DCCDC File Offset: 0x000DB0DC
		public void AddTween(TweenInstance instance)
		{
			if (this._runningCount >= this._runningTweens.Length)
			{
				Utils.DoubleCapacity<TweenInstance>(ref this._runningTweens);
			}
			instance.runnerIndex = this._runningCount;
			this._runningTweens[this._runningCount++] = instance;
			if (instance.curPercent == 0f)
			{
				if (instance.OnLeaveStart != null)
				{
					instance.OnLeaveStart();
				}
			}
			else if (instance.curPercent == 1f && instance.OnLeaveEnd != null)
			{
				instance.OnLeaveEnd();
			}
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x000DCD80 File Offset: 0x000DB180
		public void RemoveTween(TweenInstance instance)
		{
			if (instance.runnerIndex == -1)
			{
				return;
			}
			this._runningCount--;
			if (this._runningCount < 0)
			{
				throw new Exception("Removed more tweens than were started!");
			}
			int runnerIndex = instance.runnerIndex;
			this._runningTweens[this._runningCount].runnerIndex = runnerIndex;
			this._runningTweens[runnerIndex] = this._runningTweens[this._runningCount];
			instance.runnerIndex = -1;
			if (instance.curPercent == 1f)
			{
				if (instance.OnReachEnd != null)
				{
					instance.OnReachEnd();
				}
			}
			else if (instance.curPercent == 0f && instance.OnReachStart != null)
			{
				instance.OnReachStart();
			}
			if (instance.runnerIndex == -1 && instance.returnToPoolUponStop)
			{
				this.ScheduleForRecycle(instance);
			}
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x000DCE64 File Offset: 0x000DB264
		// Note: this type is marked as 'beforefieldinit'.
		static TweenRunner()
		{
		}

		// Token: 0x04002156 RID: 8534
		private TweenInstance[] _runningTweens = new TweenInstance[16];

		// Token: 0x04002157 RID: 8535
		private int _runningCount;

		// Token: 0x04002158 RID: 8536
		private Queue<TweenInstance> _toRecycle = new Queue<TweenInstance>();

		// Token: 0x04002159 RID: 8537
		private static TweenRunner _cachedInstance;
	}
}
