using System;
using UnityEngine;

namespace SpeechBlendEngine
{
	// Token: 0x02000448 RID: 1096
	public class SpeechUtil : MonoBehaviour
	{
		// Token: 0x06001B5B RID: 7003 RVA: 0x000997BD File Offset: 0x00097BBD
		public SpeechUtil()
		{
		}

		// Token: 0x02000449 RID: 1097
		public enum Mode
		{
			// Token: 0x0400174B RID: 5963
			jawOnly,
			// Token: 0x0400174C RID: 5964
			jawAndVisemes
		}

		// Token: 0x0200044A RID: 1098
		public enum Accuracy
		{
			// Token: 0x0400174E RID: 5966
			Low,
			// Token: 0x0400174F RID: 5967
			Medium,
			// Token: 0x04001750 RID: 5968
			High
		}

		// Token: 0x0200044B RID: 1099
		[Serializable]
		public class VisemeBlendshapeIndexes
		{
			// Token: 0x06001B5C RID: 7004 RVA: 0x000997C8 File Offset: 0x00097BC8
			public VisemeBlendshapeIndexes(VoiceProfile.Template tmplte)
			{
				this.template = tmplte;
				this.visemeIndex = new int[tmplte.Nvis];
				for (int i = 0; i < tmplte.Nvis; i++)
				{
					this.visemeIndex[i] = -1;
				}
				this.mouthOpenIndex = -1;
			}

			// Token: 0x06001B5D RID: 7005 RVA: 0x0009981A File Offset: 0x00097C1A
			public int[] ReturnArray()
			{
				return this.visemeIndex;
			}

			// Token: 0x06001B5E RID: 7006 RVA: 0x00099822 File Offset: 0x00097C22
			public void LoadFromArray(int[] array)
			{
				this.visemeIndex = array;
			}

			// Token: 0x06001B5F RID: 7007 RVA: 0x0009982B File Offset: 0x00097C2B
			public int GetByIndex(int index)
			{
				return this.visemeIndex[index];
			}

			// Token: 0x06001B60 RID: 7008 RVA: 0x00099835 File Offset: 0x00097C35
			public bool BlendshapeAssigned(int index)
			{
				return this.GetByIndex(index) > -1;
			}

			// Token: 0x06001B61 RID: 7009 RVA: 0x00099841 File Offset: 0x00097C41
			public bool MouthOpenBlendshapeAssigned()
			{
				return this.mouthOpenIndex > -1;
			}

			// Token: 0x06001B62 RID: 7010 RVA: 0x0009984C File Offset: 0x00097C4C
			public bool AnyAssigned()
			{
				for (int i = 0; i < this.template.Nvis; i++)
				{
					if (this.BlendshapeAssigned(i))
					{
						return true;
					}
				}
				return this.MouthOpenBlendshapeAssigned();
			}

			// Token: 0x06001B63 RID: 7011 RVA: 0x0009988C File Offset: 0x00097C8C
			public bool JawOnly()
			{
				int num = 0;
				while (num < this.template.Nvis && !this.BlendshapeAssigned(num))
				{
					num++;
				}
				return false;
			}

			// Token: 0x04001751 RID: 5969
			public VoiceProfile.Template template;

			// Token: 0x04001752 RID: 5970
			public int[] visemeIndex;

			// Token: 0x04001753 RID: 5971
			public int mouthOpenIndex;
		}

		// Token: 0x0200044C RID: 1100
		[Serializable]
		public class VisemeBlendshapeNames
		{
			// Token: 0x06001B64 RID: 7012 RVA: 0x000998C4 File Offset: 0x00097CC4
			public VisemeBlendshapeNames(VoiceProfile.Template tmplte)
			{
				this.template = tmplte;
				this.visemeNames = new string[tmplte.Nvis];
				for (int i = 0; i < tmplte.Nvis; i++)
				{
					this.visemeNames[i] = null;
				}
				this.mouthOpenName = null;
			}

			// Token: 0x06001B65 RID: 7013 RVA: 0x00099916 File Offset: 0x00097D16
			public string[] ReturnArray()
			{
				return this.visemeNames;
			}

			// Token: 0x06001B66 RID: 7014 RVA: 0x0009991E File Offset: 0x00097D1E
			public void LoadFromArray(string[] array)
			{
				this.visemeNames = array;
			}

			// Token: 0x06001B67 RID: 7015 RVA: 0x00099927 File Offset: 0x00097D27
			public string GetByIndex(int index)
			{
				return this.visemeNames[index];
			}

			// Token: 0x06001B68 RID: 7016 RVA: 0x00099931 File Offset: 0x00097D31
			public bool BlendshapeAssigned(int index)
			{
				return this.GetByIndex(index) != null;
			}

			// Token: 0x06001B69 RID: 7017 RVA: 0x00099940 File Offset: 0x00097D40
			public bool MouthOpenBlendshapeAssigned()
			{
				return this.mouthOpenName != null;
			}

			// Token: 0x06001B6A RID: 7018 RVA: 0x00099950 File Offset: 0x00097D50
			public bool AnyAssigned()
			{
				for (int i = 0; i < this.template.Nvis; i++)
				{
					if (this.BlendshapeAssigned(i))
					{
						return true;
					}
				}
				return this.MouthOpenBlendshapeAssigned();
			}

			// Token: 0x06001B6B RID: 7019 RVA: 0x00099990 File Offset: 0x00097D90
			public bool JawOnly()
			{
				int num = 0;
				while (num < this.template.Nvis && !this.BlendshapeAssigned(num))
				{
					num++;
				}
				return false;
			}

			// Token: 0x04001754 RID: 5972
			public VoiceProfile.Template template;

			// Token: 0x04001755 RID: 5973
			public string[] visemeNames;

			// Token: 0x04001756 RID: 5974
			public string mouthOpenName;
		}

		// Token: 0x0200044D RID: 1101
		[Serializable]
		public class VisemeWeight
		{
			// Token: 0x06001B6C RID: 7020 RVA: 0x000999C8 File Offset: 0x00097DC8
			public VisemeWeight(VoiceProfile.Template tmplte)
			{
				this.template = tmplte;
				this.weights = new float[this.template.Nvis];
				for (int i = 0; i < this.template.Nvis; i++)
				{
					this.weights[i] = 1f;
				}
			}

			// Token: 0x06001B6D RID: 7021 RVA: 0x00099A21 File Offset: 0x00097E21
			public void LoadFromArray(float[] array)
			{
				this.weights = array;
			}

			// Token: 0x06001B6E RID: 7022 RVA: 0x00099A2A File Offset: 0x00097E2A
			public float[] ReturnArray()
			{
				return this.weights;
			}

			// Token: 0x06001B6F RID: 7023 RVA: 0x00099A32 File Offset: 0x00097E32
			public float GetByIndex(int index)
			{
				return this.weights[index];
			}

			// Token: 0x04001757 RID: 5975
			public float[] weights;

			// Token: 0x04001758 RID: 5976
			public VoiceProfile.Template template;
		}
	}
}
