using System;
using UnityEngine;

namespace Leap.Unity.Space
{
	// Token: 0x0200071B RID: 1819
	[DisallowMultipleComponent]
	public class LeapSpaceAnchor : MonoBehaviour
	{
		// Token: 0x06002C55 RID: 11349 RVA: 0x000ECD64 File Offset: 0x000EB164
		public LeapSpaceAnchor()
		{
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x000ECD6C File Offset: 0x000EB16C
		protected virtual void OnEnable()
		{
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x000ECD6E File Offset: 0x000EB16E
		protected virtual void OnDisable()
		{
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x000ECD70 File Offset: 0x000EB170
		public void RecalculateParentAnchor()
		{
			if (this is LeapSpace)
			{
				this.parent = this;
			}
			else
			{
				this.parent = LeapSpaceAnchor.GetAnchor(base.transform.parent);
			}
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x000ECDA0 File Offset: 0x000EB1A0
		public static LeapSpaceAnchor GetAnchor(Transform root)
		{
			while (!(root == null))
			{
				LeapSpaceAnchor component = root.GetComponent<LeapSpaceAnchor>();
				if (component != null && component.enabled)
				{
					return component;
				}
				root = root.parent;
			}
			return null;
		}

		// Token: 0x04002370 RID: 9072
		[HideInInspector]
		public LeapSpaceAnchor parent;

		// Token: 0x04002371 RID: 9073
		[HideInInspector]
		public LeapSpace space;

		// Token: 0x04002372 RID: 9074
		public ITransformer transformer;
	}
}
