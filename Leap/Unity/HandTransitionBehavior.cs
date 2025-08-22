using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006EA RID: 1770
	public abstract class HandTransitionBehavior : MonoBehaviour
	{
		// Token: 0x06002AD6 RID: 10966 RVA: 0x000E69B3 File Offset: 0x000E4DB3
		protected HandTransitionBehavior()
		{
		}

		// Token: 0x06002AD7 RID: 10967
		protected abstract void HandReset();

		// Token: 0x06002AD8 RID: 10968
		protected abstract void HandFinish();

		// Token: 0x06002AD9 RID: 10969 RVA: 0x000E69BC File Offset: 0x000E4DBC
		protected virtual void Awake()
		{
			this.handModelBase = base.GetComponent<HandModelBase>();
			if (this.handModelBase == null)
			{
				Debug.LogWarning("HandTransitionBehavior components require a HandModelBase component attached to the same GameObject. (Awake)");
				return;
			}
			this.handModelBase.OnBegin -= this.HandReset;
			this.handModelBase.OnBegin += this.HandReset;
			this.handModelBase.OnFinish -= this.HandFinish;
			this.handModelBase.OnFinish += this.HandFinish;
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x000E6A54 File Offset: 0x000E4E54
		protected virtual void OnDestroy()
		{
			if (this.handModelBase == null)
			{
				HandModelBase component = base.GetComponent<HandModelBase>();
				if (component == null)
				{
					Debug.LogWarning("HandTransitionBehavior components require a HandModelBase component attached to the same GameObject. (OnDestroy)");
					return;
				}
			}
			this.handModelBase.OnBegin -= this.HandReset;
			this.handModelBase.OnFinish -= this.HandFinish;
		}

		// Token: 0x040022C6 RID: 8902
		protected HandModelBase handModelBase;
	}
}
