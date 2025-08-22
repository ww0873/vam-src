using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrefabEvolution
{
	// Token: 0x0200040A RID: 1034
	[Serializable]
	public class PELinkage
	{
		// Token: 0x06001A44 RID: 6724 RVA: 0x00093277 File Offset: 0x00091677
		public PELinkage()
		{
		}

		// Token: 0x170002EE RID: 750
		public PELinkage.Link this[int liif]
		{
			get
			{
				for (int i = 0; i < this.Links.Count; i++)
				{
					PELinkage.Link link = this.Links[i];
					if (link.LIIF == liif)
					{
						return link;
					}
				}
				return null;
			}
		}

		// Token: 0x170002EF RID: 751
		public PELinkage.Link this[PELinkage.Link link]
		{
			get
			{
				if (link == null)
				{
					return null;
				}
				return this[link.LIIF];
			}
		}

		// Token: 0x170002F0 RID: 752
		public PELinkage.Link this[UnityEngine.Object obj]
		{
			get
			{
				for (int i = 0; i < this.Links.Count; i++)
				{
					PELinkage.Link link = this.Links[i];
					if (link.InstanceTarget == obj)
					{
						return link;
					}
				}
				return null;
			}
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x00093334 File Offset: 0x00091734
		public UnityEngine.Object GetPrefabObject(GameObject prefab, UnityEngine.Object instanceObject)
		{
			PEPrefabScript component = prefab.GetComponent<PEPrefabScript>();
			PELinkage.Link link = this[instanceObject];
			if (link == null)
			{
				return null;
			}
			PELinkage.Link link2 = component.Links[link.LIIF];
			if (link2 == null)
			{
				return null;
			}
			return link2.InstanceTarget;
		}

		// Token: 0x04001537 RID: 5431
		public List<PELinkage.Link> Links = Utils.Create<List<PELinkage.Link>>();

		// Token: 0x0200040B RID: 1035
		[Serializable]
		public class Link
		{
			// Token: 0x06001A49 RID: 6729 RVA: 0x00093378 File Offset: 0x00091778
			public Link()
			{
			}

			// Token: 0x06001A4A RID: 6730 RVA: 0x00093380 File Offset: 0x00091780
			public override string ToString()
			{
				return string.Format("[Link]{0}:{1}", this.LIIF, this.InstanceTarget);
			}

			// Token: 0x04001538 RID: 5432
			public int LIIF;

			// Token: 0x04001539 RID: 5433
			public UnityEngine.Object InstanceTarget;
		}
	}
}
