using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D15 RID: 3349
public class HierarchicalAssignUIMaterial : MonoBehaviour
{
	// Token: 0x06006642 RID: 26178 RVA: 0x0026A3D5 File Offset: 0x002687D5
	public HierarchicalAssignUIMaterial()
	{
	}

	// Token: 0x06006643 RID: 26179 RVA: 0x0026A3E0 File Offset: 0x002687E0
	private void Start()
	{
		if (this.UIMaterial != null)
		{
			Image[] componentsInChildren = base.GetComponentsInChildren<Image>(true);
			foreach (Image image in componentsInChildren)
			{
				image.material = this.UIMaterial;
			}
			Text[] componentsInChildren2 = base.GetComponentsInChildren<Text>(true);
			foreach (Text text in componentsInChildren2)
			{
				text.material = this.UIMaterial;
			}
		}
	}

	// Token: 0x040055D5 RID: 21973
	public Material UIMaterial;
}
