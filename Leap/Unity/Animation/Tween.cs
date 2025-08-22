using System;
using System.Runtime.CompilerServices;
using Leap.Unity.Animation.Internal;
using UnityEngine;

namespace Leap.Unity.Animation
{
	// Token: 0x02000649 RID: 1609
	public struct Tween
	{
		// Token: 0x06002745 RID: 10053 RVA: 0x000DB2ED File Offset: 0x000D96ED
		private Tween(bool isSingle)
		{
			this._instance = Pool<TweenInstance>.Spawn();
			this._id = this._instance.instanceId;
			this._instance.returnToPoolUponStop = isSingle;
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000DB317 File Offset: 0x000D9717
		public MaterialSelector Target(Material material)
		{
			return new MaterialSelector(material, this);
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x000DB325 File Offset: 0x000D9725
		public TransformSelector Target(Transform transform)
		{
			return new TransformSelector(transform, this);
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x000DB333 File Offset: 0x000D9733
		public Tween Value(float a, float b, Action<float> onValue)
		{
			this.AddInterpolator(Pool<Tween.FloatInterpolator>.Spawn().Init(a, b, onValue));
			return this;
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x000DB34F File Offset: 0x000D974F
		public Tween Value(Vector2 a, Vector2 b, Action<Vector2> onValue)
		{
			this.AddInterpolator(Pool<Tween.Vector2Interpolator>.Spawn().Init(a, b, onValue));
			return this;
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x000DB36B File Offset: 0x000D976B
		public Tween Value(Vector3 a, Vector3 b, Action<Vector3> onValue)
		{
			this.AddInterpolator(Pool<Tween.Vector3Interpolator>.Spawn().Init(a, b, onValue));
			return this;
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x000DB387 File Offset: 0x000D9787
		public Tween Value(Quaternion a, Quaternion b, Action<Quaternion> onValue)
		{
			this.AddInterpolator(Pool<Tween.QuaternionInterpolator>.Spawn().Init(a, b, onValue));
			return this;
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x000DB3A3 File Offset: 0x000D97A3
		public Tween Value(Color a, Color b, Action<Color> onValue)
		{
			this.AddInterpolator(Pool<Tween.ColorInterpolator>.Spawn().Init(a, b, onValue));
			return this;
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x000DB3BF File Offset: 0x000D97BF
		public static Tween Single()
		{
			return new Tween(true);
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x000DB3C7 File Offset: 0x000D97C7
		public static Tween Persistent()
		{
			return new Tween(false);
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x000DB3D0 File Offset: 0x000D97D0
		public static Tween AfterDelay(float delay, Action onReachEnd)
		{
			Tween tween = Tween.Single();
			float a = 0f;
			float b = 1f;
			if (Tween.<>f__am$cache0 == null)
			{
				Tween.<>f__am$cache0 = new Action<float>(Tween.<AfterDelay>m__0);
			}
			return tween.Value(a, b, Tween.<>f__am$cache0).OverTime(delay).OnReachEnd(onReachEnd).Play();
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06002750 RID: 10064 RVA: 0x000DB42B File Offset: 0x000D982B
		public bool isValid
		{
			get
			{
				return this._instance != null && this._id == this._instance.instanceId;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06002751 RID: 10065 RVA: 0x000DB44E File Offset: 0x000D984E
		public bool isRunning
		{
			get
			{
				this.throwIfInvalid();
				return this._instance.runnerIndex != -1;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06002752 RID: 10066 RVA: 0x000DB467 File Offset: 0x000D9867
		// (set) Token: 0x06002753 RID: 10067 RVA: 0x000DB47A File Offset: 0x000D987A
		public Direction direction
		{
			get
			{
				this.throwIfInvalid();
				return this._instance.direction;
			}
			set
			{
				this.throwIfInvalid();
				this._instance.direction = value;
				this._instance.dstPercent = (float)((value != Direction.Backward) ? 1 : 0);
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06002754 RID: 10068 RVA: 0x000DB4A8 File Offset: 0x000D98A8
		public float timeLeft
		{
			get
			{
				this.throwIfInvalid();
				return Mathf.Abs((this._instance.curPercent - this._instance.dstPercent) / this._instance.velPercent);
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06002755 RID: 10069 RVA: 0x000DB4D8 File Offset: 0x000D98D8
		// (set) Token: 0x06002756 RID: 10070 RVA: 0x000DB4EC File Offset: 0x000D98EC
		public float progress
		{
			get
			{
				this.throwIfInvalid();
				return this._instance.curPercent;
			}
			set
			{
				this.throwIfInvalid();
				if (this._instance.curPercent == value)
				{
					return;
				}
				if (value < 0f || value > 1f)
				{
					throw new ArgumentException("Progress must be a value from 0 - 1");
				}
				if (this._instance.curPercent == 0f)
				{
					if (this._instance.OnLeaveStart != null)
					{
						this._instance.OnLeaveStart();
					}
				}
				else if (this._instance.curPercent == 1f && this._instance.OnLeaveEnd != null)
				{
					this._instance.OnLeaveEnd();
				}
				this._instance.curPercent = value;
				if (this._instance.curPercent == 0f)
				{
					if (this._instance.OnReachStart != null)
					{
						this._instance.OnReachStart();
					}
				}
				else if (this._instance.curPercent == 1f && this._instance.OnReachEnd != null)
				{
					this._instance.OnReachEnd();
				}
				if (this._instance.runnerIndex == -1)
				{
					this._instance.interpolatePercent();
				}
			}
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x000DB638 File Offset: 0x000D9A38
		public Tween AddInterpolator(IInterpolator interpolator)
		{
			this.throwIfInvalid();
			if (this._instance.interpolatorCount >= this._instance.interpolators.Length)
			{
				Utils.DoubleCapacity<IInterpolator>(ref this._instance.interpolators);
			}
			this._instance.interpolators[this._instance.interpolatorCount++] = interpolator;
			return this;
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x000DB6A1 File Offset: 0x000D9AA1
		public Tween OverTime(float seconds)
		{
			this.throwIfInvalid();
			this._instance.velPercent = 1f / seconds;
			return this;
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x000DB6C1 File Offset: 0x000D9AC1
		public Tween AtRate(float unitsPerSecond)
		{
			this.throwIfInvalid();
			this._instance.velPercent = unitsPerSecond / this._instance.interpolators[0].length;
			return this;
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000DB6EE File Offset: 0x000D9AEE
		public Tween Smooth(SmoothType type = SmoothType.Smooth)
		{
			this.throwIfInvalid();
			this._instance.smoothType = type;
			this._instance.smoothFunction = null;
			return this;
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000DB714 File Offset: 0x000D9B14
		public Tween Smooth(AnimationCurve curve)
		{
			this.throwIfInvalid();
			this._instance.smoothType = (SmoothType)0;
			this._instance.smoothFunction = new Func<float, float>(curve.Evaluate);
			return this;
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x000DB745 File Offset: 0x000D9B45
		public Tween Smooth(Func<float, float> smoothFunction)
		{
			this.throwIfInvalid();
			this._instance.smoothType = (SmoothType)0;
			this._instance.smoothFunction = smoothFunction;
			return this;
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x000DB76B File Offset: 0x000D9B6B
		public Tween OnProgress(Action<float> action)
		{
			this.throwIfInvalid();
			TweenInstance instance = this._instance;
			instance.OnProgress = (Action<float>)Delegate.Combine(instance.OnProgress, action);
			return this;
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x000DB795 File Offset: 0x000D9B95
		public Tween OnLeaveStart(Action action)
		{
			this.throwIfInvalid();
			TweenInstance instance = this._instance;
			instance.OnLeaveStart = (Action)Delegate.Combine(instance.OnLeaveStart, action);
			return this;
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x000DB7BF File Offset: 0x000D9BBF
		public Tween OnReachStart(Action action)
		{
			this.throwIfInvalid();
			TweenInstance instance = this._instance;
			instance.OnReachStart = (Action)Delegate.Combine(instance.OnReachStart, action);
			return this;
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x000DB7E9 File Offset: 0x000D9BE9
		public Tween OnLeaveEnd(Action action)
		{
			this.throwIfInvalid();
			TweenInstance instance = this._instance;
			instance.OnLeaveEnd = (Action)Delegate.Combine(instance.OnLeaveEnd, action);
			return this;
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x000DB813 File Offset: 0x000D9C13
		public Tween OnReachEnd(Action action)
		{
			this.throwIfInvalid();
			TweenInstance instance = this._instance;
			instance.OnReachEnd = (Action)Delegate.Combine(instance.OnReachEnd, action);
			return this;
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000DB840 File Offset: 0x000D9C40
		public Tween Play()
		{
			this.throwIfInvalid();
			if (this._instance.curPercent == this._instance.dstPercent)
			{
				return this;
			}
			if (this._instance.runnerIndex != -1)
			{
				return this;
			}
			TweenRunner.instance.AddTween(this._instance);
			return this;
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x000DB8A3 File Offset: 0x000D9CA3
		public Tween Play(Direction direction)
		{
			this.throwIfInvalid();
			this.direction = direction;
			this.Play();
			return this;
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000DB8C0 File Offset: 0x000D9CC0
		public Tween Play(float destinationPercent)
		{
			this.throwIfInvalid();
			if (destinationPercent < 0f || destinationPercent > 1f)
			{
				throw new ArgumentException("Destination percent must be within the range [0-1]");
			}
			this.direction = ((destinationPercent < this._instance.curPercent) ? Direction.Backward : Direction.Forward);
			this._instance.dstPercent = destinationPercent;
			this.Play();
			return this;
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x000DB92B File Offset: 0x000D9D2B
		public TweenInstance.TweenYieldInstruction Yield()
		{
			this.throwIfInvalid();
			return this._instance.yieldInstruction;
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x000DB93E File Offset: 0x000D9D3E
		public void Pause()
		{
			this.throwIfInvalid();
			if (this._instance.runnerIndex != -1)
			{
				TweenRunner.instance.RemoveTween(this._instance);
			}
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000DB968 File Offset: 0x000D9D68
		public void Stop()
		{
			this.throwIfInvalid();
			this.progress = 0f;
			this.direction = Direction.Forward;
			this.Pause();
			if (this.isValid && this._instance.returnToPoolUponStop)
			{
				this.Release();
			}
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000DB9B4 File Offset: 0x000D9DB4
		public void Release()
		{
			this.throwIfInvalid();
			this.Pause();
			TweenRunner.instance.ScheduleForRecycle(this._instance);
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000DB9D2 File Offset: 0x000D9DD2
		private void throwIfInvalid()
		{
			if (this.isValid)
			{
				return;
			}
			if (this._id == 0)
			{
				throw new InvalidOperationException("Tween is invalid.  Make sure you use Tween.Single or Tween.Persistant to create your Tween instead of the default constructor.");
			}
			throw new InvalidOperationException("Tween is invalid or was recycled.  Make sure to use Tween.Persistant if you want to keep a tween around after it finishes playing.");
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000DBA00 File Offset: 0x000D9E00
		[CompilerGenerated]
		private static void <AfterDelay>m__0(float x)
		{
		}

		// Token: 0x04002133 RID: 8499
		private int _id;

		// Token: 0x04002134 RID: 8500
		private TweenInstance _instance;

		// Token: 0x04002135 RID: 8501
		[CompilerGenerated]
		private static Action<float> <>f__am$cache0;

		// Token: 0x0200065E RID: 1630
		private class FloatInterpolator : FloatInterpolatorBase<Action<float>>
		{
			// Token: 0x060027E9 RID: 10217 RVA: 0x000DBA02 File Offset: 0x000D9E02
			public FloatInterpolator()
			{
			}

			// Token: 0x060027EA RID: 10218 RVA: 0x000DBA0A File Offset: 0x000D9E0A
			public override void Interpolate(float percent)
			{
				this._target(this._a + this._b * percent);
			}

			// Token: 0x060027EB RID: 10219 RVA: 0x000DBA26 File Offset: 0x000D9E26
			public override void Dispose()
			{
				this._target = null;
				Pool<Tween.FloatInterpolator>.Recycle(this);
			}

			// Token: 0x170004F0 RID: 1264
			// (get) Token: 0x060027EC RID: 10220 RVA: 0x000DBA35 File Offset: 0x000D9E35
			public override bool isValid
			{
				get
				{
					return true;
				}
			}
		}

		// Token: 0x0200065F RID: 1631
		private class Vector2Interpolator : Vector2InterpolatorBase<Action<Vector2>>
		{
			// Token: 0x060027ED RID: 10221 RVA: 0x000DBA38 File Offset: 0x000D9E38
			public Vector2Interpolator()
			{
			}

			// Token: 0x060027EE RID: 10222 RVA: 0x000DBA40 File Offset: 0x000D9E40
			public override void Interpolate(float percent)
			{
				this._target(this._a + this._b * percent);
			}

			// Token: 0x060027EF RID: 10223 RVA: 0x000DBA64 File Offset: 0x000D9E64
			public override void Dispose()
			{
				this._target = null;
				Pool<Tween.Vector2Interpolator>.Recycle(this);
			}

			// Token: 0x170004F1 RID: 1265
			// (get) Token: 0x060027F0 RID: 10224 RVA: 0x000DBA73 File Offset: 0x000D9E73
			public override bool isValid
			{
				get
				{
					return true;
				}
			}
		}

		// Token: 0x02000660 RID: 1632
		private class Vector3Interpolator : Vector3InterpolatorBase<Action<Vector3>>
		{
			// Token: 0x060027F1 RID: 10225 RVA: 0x000DBA76 File Offset: 0x000D9E76
			public Vector3Interpolator()
			{
			}

			// Token: 0x060027F2 RID: 10226 RVA: 0x000DBA7E File Offset: 0x000D9E7E
			public override void Interpolate(float percent)
			{
				this._target(this._a + this._b * percent);
			}

			// Token: 0x060027F3 RID: 10227 RVA: 0x000DBAA2 File Offset: 0x000D9EA2
			public override void Dispose()
			{
				this._target = null;
				Pool<Tween.Vector3Interpolator>.Recycle(this);
			}

			// Token: 0x170004F2 RID: 1266
			// (get) Token: 0x060027F4 RID: 10228 RVA: 0x000DBAB1 File Offset: 0x000D9EB1
			public override bool isValid
			{
				get
				{
					return true;
				}
			}
		}

		// Token: 0x02000661 RID: 1633
		private class QuaternionInterpolator : QuaternionInterpolatorBase<Action<Quaternion>>
		{
			// Token: 0x060027F5 RID: 10229 RVA: 0x000DBAB4 File Offset: 0x000D9EB4
			public QuaternionInterpolator()
			{
			}

			// Token: 0x060027F6 RID: 10230 RVA: 0x000DBABC File Offset: 0x000D9EBC
			public override void Interpolate(float percent)
			{
				this._target(Quaternion.Slerp(this._a, this._b, percent));
			}

			// Token: 0x060027F7 RID: 10231 RVA: 0x000DBADB File Offset: 0x000D9EDB
			public override void Dispose()
			{
				this._target = null;
				Pool<Tween.QuaternionInterpolator>.Recycle(this);
			}

			// Token: 0x170004F3 RID: 1267
			// (get) Token: 0x060027F8 RID: 10232 RVA: 0x000DBAEA File Offset: 0x000D9EEA
			public override bool isValid
			{
				get
				{
					return true;
				}
			}
		}

		// Token: 0x02000662 RID: 1634
		private class ColorInterpolator : ColorInterpolatorBase<Action<Color>>
		{
			// Token: 0x060027F9 RID: 10233 RVA: 0x000DBAED File Offset: 0x000D9EED
			public ColorInterpolator()
			{
			}

			// Token: 0x060027FA RID: 10234 RVA: 0x000DBAF5 File Offset: 0x000D9EF5
			public override void Interpolate(float percent)
			{
				this._target(this._a + this._b * percent);
			}

			// Token: 0x060027FB RID: 10235 RVA: 0x000DBB19 File Offset: 0x000D9F19
			public override void Dispose()
			{
				this._target = null;
				Pool<Tween.ColorInterpolator>.Recycle(this);
			}

			// Token: 0x170004F4 RID: 1268
			// (get) Token: 0x060027FC RID: 10236 RVA: 0x000DBB28 File Offset: 0x000D9F28
			public override bool isValid
			{
				get
				{
					return true;
				}
			}
		}
	}
}
