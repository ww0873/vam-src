using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x0200063A RID: 1594
	public abstract class AutoValueProxy : MonoBehaviour, IValueProxy
	{
		// Token: 0x06002700 RID: 9984 RVA: 0x000DAB11 File Offset: 0x000D8F11
		protected AutoValueProxy()
		{
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06002701 RID: 9985 RVA: 0x000DAB19 File Offset: 0x000D8F19
		// (set) Token: 0x06002702 RID: 9986 RVA: 0x000DAB21 File Offset: 0x000D8F21
		public bool autoPushingEnabled
		{
			get
			{
				return this._autoPushingEnabled;
			}
			set
			{
				this._autoPushingEnabled = value;
			}
		}

		// Token: 0x06002703 RID: 9987
		public abstract void OnPullValue();

		// Token: 0x06002704 RID: 9988
		public abstract void OnPushValue();

		// Token: 0x06002705 RID: 9989 RVA: 0x000DAB2A File Offset: 0x000D8F2A
		private void LateUpdate()
		{
			if (this._autoPushingEnabled)
			{
				this.OnPushValue();
			}
		}

		// Token: 0x0400210A RID: 8458
		[SerializeField]
		[HideInInspector]
		private bool _autoPushingEnabled;
	}
}
