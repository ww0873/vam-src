using System;
using UnityEngine;

// Token: 0x02000D56 RID: 3414
public class CollisionDebugs : MonoBehaviour
{
	// Token: 0x060068F5 RID: 26869 RVA: 0x0027435F File Offset: 0x0027275F
	public CollisionDebugs()
	{
	}

	// Token: 0x17000F7E RID: 3966
	// (get) Token: 0x060068F6 RID: 26870 RVA: 0x00274367 File Offset: 0x00272767
	// (set) Token: 0x060068F7 RID: 26871 RVA: 0x00274370 File Offset: 0x00272770
	public bool allOn
	{
		get
		{
			return this._allOn;
		}
		set
		{
			if (this._allOn != value)
			{
				this._allOn = value;
				CollisionDebug[] componentsInChildren = base.GetComponentsInChildren<CollisionDebug>(true);
				foreach (CollisionDebug collisionDebug in componentsInChildren)
				{
					collisionDebug.on = this._allOn;
				}
			}
		}
	}

	// Token: 0x040059C2 RID: 22978
	[SerializeField]
	private bool _allOn;
}
