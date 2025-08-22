using System;

namespace Leap.Unity.Animation.Internal
{
	// Token: 0x02000641 RID: 1601
	public abstract class InterpolatorBase<ValueType, ObjType> : IInterpolator, IPoolable, IDisposable
	{
		// Token: 0x06002723 RID: 10019 RVA: 0x000DB166 File Offset: 0x000D9566
		protected InterpolatorBase()
		{
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000DB16E File Offset: 0x000D956E
		public InterpolatorBase<ValueType, ObjType> Init(ValueType a, ValueType b, ObjType target)
		{
			this._a = a;
			this._b = b;
			this._target = target;
			return this;
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06002725 RID: 10021
		public abstract float length { get; }

		// Token: 0x06002726 RID: 10022
		public abstract void Interpolate(float percent);

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06002727 RID: 10023
		public abstract bool isValid { get; }

		// Token: 0x06002728 RID: 10024 RVA: 0x000DB186 File Offset: 0x000D9586
		public void OnSpawn()
		{
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000DB188 File Offset: 0x000D9588
		public void OnRecycle()
		{
		}

		// Token: 0x0600272A RID: 10026
		public abstract void Dispose();

		// Token: 0x0400212F RID: 8495
		protected ValueType _a;

		// Token: 0x04002130 RID: 8496
		protected ValueType _b;

		// Token: 0x04002131 RID: 8497
		protected ObjType _target;
	}
}
