using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AF6 RID: 2806
public class DAZMorphSet : MonoBehaviour
{
	// Token: 0x06004B8E RID: 19342 RVA: 0x001A4E29 File Offset: 0x001A3229
	public DAZMorphSet()
	{
	}

	// Token: 0x06004B8F RID: 19343 RVA: 0x001A4E34 File Offset: 0x001A3234
	public void InitSet()
	{
		if (this.DAZMeshInitTransform != null)
		{
			DAZMesh[] componentsInChildren = this.DAZMeshInitTransform.GetComponentsInChildren<DAZMesh>(true);
			List<string> list = new List<string>();
			List<float> list2 = new List<float>();
			List<float> list3 = new List<float>();
			List<string> list4 = new List<string>();
			List<float> list5 = new List<float>();
			foreach (DAZMesh dazmesh in componentsInChildren)
			{
				if (dazmesh.morphBank != null)
				{
					foreach (DAZMorph dazmorph in dazmesh.morphBank.morphs)
					{
						if (dazmorph.morphValue != dazmorph.startValue)
						{
							if (dazmorph.visible)
							{
								list.Add(dazmorph.morphName);
								list2.Add(dazmorph.morphValue);
								list3.Add(dazmorph.startValue);
							}
							else
							{
								list4.Add(dazmorph.morphName);
								list5.Add(dazmorph.morphValue);
							}
						}
					}
				}
			}
			this.morphNames = list.ToArray();
			this.morphValues = list2.ToArray();
			this.morphStartValues = list3.ToArray();
			this.untrackedMorphNames = list4.ToArray();
			this.untrackedMorphValues = list5.ToArray();
		}
	}

	// Token: 0x06004B90 RID: 19344 RVA: 0x001A4FAC File Offset: 0x001A33AC
	public void ApplySet()
	{
		if (this.DAZMeshApplyTransform != null && this.morphNames != null)
		{
			DAZMesh[] components = this.DAZMeshApplyTransform.GetComponents<DAZMesh>();
			foreach (DAZMesh dazmesh in components)
			{
				if (dazmesh.morphBank != null)
				{
					foreach (DAZMorph dazmorph in dazmesh.morphBank.morphs)
					{
						if (dazmorph.visible)
						{
							dazmorph.morphValue = dazmorph.startValue;
						}
					}
				}
			}
			for (int j = 0; j < this.morphNames.Length; j++)
			{
				foreach (DAZMesh dazmesh2 in components)
				{
					if (dazmesh2.morphBank != null)
					{
						DAZMorph morph = dazmesh2.morphBank.GetMorph(this.morphNames[j]);
						if (morph != null)
						{
							morph.morphValue = this.morphValues[j];
						}
					}
				}
			}
			foreach (DAZMesh dazmesh3 in components)
			{
				if (dazmesh3.morphBank != null)
				{
					dazmesh3.morphBank.ApplyMorphsImmediate();
				}
			}
		}
	}

	// Token: 0x04003A58 RID: 14936
	public string displayName;

	// Token: 0x04003A59 RID: 14937
	public Transform DAZMeshInitTransform;

	// Token: 0x04003A5A RID: 14938
	public Transform DAZMeshApplyTransform;

	// Token: 0x04003A5B RID: 14939
	public string[] morphNames;

	// Token: 0x04003A5C RID: 14940
	public float[] morphValues;

	// Token: 0x04003A5D RID: 14941
	public float[] morphStartValues;

	// Token: 0x04003A5E RID: 14942
	public string[] untrackedMorphNames;

	// Token: 0x04003A5F RID: 14943
	public float[] untrackedMorphValues;
}
