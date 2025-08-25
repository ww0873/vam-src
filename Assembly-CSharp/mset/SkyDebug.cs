using System;
using UnityEngine;

namespace mset
{
	// Token: 0x02000336 RID: 822
	public class SkyDebug : MonoBehaviour
	{
		// Token: 0x060013D3 RID: 5075 RVA: 0x000716C3 File Offset: 0x0006FAC3
		public SkyDebug()
		{
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x000716EB File Offset: 0x0006FAEB
		private void Start()
		{
			this.debugID = SkyDebug.debugCounter;
			SkyDebug.debugCounter++;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00071704 File Offset: 0x0006FB04
		private void LateUpdate()
		{
			bool flag = this.printOnce || this.printConstantly;
			if (base.GetComponent<Renderer>() && flag)
			{
				this.printOnce = false;
				this.debugString = this.GetDebugString();
				if (this.printToConsole)
				{
					Debug.Log(this.debugString);
				}
			}
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00071768 File Offset: 0x0006FB68
		public string GetDebugString()
		{
			string text = "<b>SkyDebug Info - " + base.name + "</b>\n";
			Material material;
			if (Application.isPlaying)
			{
				material = base.GetComponent<Renderer>().material;
			}
			else
			{
				material = base.GetComponent<Renderer>().sharedMaterial;
			}
			text = text + material.shader.name + "\n";
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"is supported: ",
				material.shader.isSupported,
				"\n"
			});
			ShaderIDs[] array = new ShaderIDs[]
			{
				new ShaderIDs(),
				new ShaderIDs()
			};
			array[0].Link();
			array[1].Link("1");
			text += "\n<b>Anchor</b>\n";
			SkyAnchor component = base.GetComponent<SkyAnchor>();
			if (component != null)
			{
				text = text + "Curr. sky: " + component.CurrentSky.name + "\n";
				text = text + "Prev. sky: " + component.PreviousSky.name + "\n";
			}
			else
			{
				text += "none\n";
			}
			text += "\n<b>Property Block</b>\n";
			if (this.block == null)
			{
				this.block = new MaterialPropertyBlock();
			}
			this.block.Clear();
			base.GetComponent<Renderer>().GetPropertyBlock(this.block);
			for (int i = 0; i < 2; i++)
			{
				text = text + "Renderer Property block - blend ID " + i;
				if (this.printDetails)
				{
					text = text + "\nexposureIBL  " + this.block.GetVector(array[i].exposureIBL);
					text = text + "\nexposureLM   " + this.block.GetVector(array[i].exposureLM);
					text = text + "\nskyMin       " + this.block.GetVector(array[i].skyMin);
					text = text + "\nskyMax       " + this.block.GetVector(array[i].skyMax);
					text += "\ndiffuse SH\n";
					for (int j = 0; j < 4; j++)
					{
						text = text + this.block.GetVector(array[i].SH[j]) + "\n";
					}
					text += "...\n";
				}
				Texture texture = this.block.GetTexture(array[i].specCubeIBL);
				Texture texture2 = this.block.GetTexture(array[i].skyCubeIBL);
				text += "\nspecCubeIBL  ";
				if (texture)
				{
					text += texture.name;
				}
				else
				{
					text += "none";
				}
				text += "\nskyCubeIBL   ";
				if (texture2)
				{
					text += texture2.name;
				}
				else
				{
					text += "none";
				}
				if (this.printDetails)
				{
					text = text + "\nskyMatrix\n" + this.block.GetMatrix(array[i].skyMatrix);
					text = text + "\ninvSkyMatrix\n" + this.block.GetMatrix(array[i].invSkyMatrix);
				}
				if (i == 0)
				{
					text = text + "\nblendWeightIBL " + this.block.GetFloat(array[i].blendWeightIBL);
				}
				text += "\n\n";
			}
			return text;
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00071B1C File Offset: 0x0006FF1C
		private void OnDrawGizmosSelected()
		{
			bool flag = this.printOnce || this.printConstantly;
			if (base.GetComponent<Renderer>() && this.printInEditor && this.printToConsole && flag)
			{
				this.printOnce = false;
				string message = this.GetDebugString();
				Debug.Log(message);
			}
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00071B80 File Offset: 0x0006FF80
		private void OnGUI()
		{
			if (this.printToGUI)
			{
				Rect position = Rect.MinMaxRect(3f, 3f, 360f, 1024f);
				if (Camera.main)
				{
					position.yMax = (float)Camera.main.pixelHeight;
				}
				position.xMin += (float)this.debugID * position.width;
				GUI.color = Color.white;
				if (this.debugStyle == null)
				{
					this.debugStyle = new GUIStyle();
					this.debugStyle.richText = true;
				}
				string str = "<color=\"#000\">";
				string str2 = "</color>";
				GUI.TextArea(position, str + this.debugString + str2, this.debugStyle);
				str = "<color=\"#FFF\">";
				position.xMin -= 1f;
				position.yMin -= 2f;
				GUI.TextArea(position, str + this.debugString + str2, this.debugStyle);
			}
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00071C88 File Offset: 0x00070088
		// Note: this type is marked as 'beforefieldinit'.
		static SkyDebug()
		{
		}

		// Token: 0x04001123 RID: 4387
		public bool printConstantly = true;

		// Token: 0x04001124 RID: 4388
		public bool printOnce;

		// Token: 0x04001125 RID: 4389
		public bool printToGUI = true;

		// Token: 0x04001126 RID: 4390
		public bool printToConsole;

		// Token: 0x04001127 RID: 4391
		public bool printInEditor = true;

		// Token: 0x04001128 RID: 4392
		public bool printDetails;

		// Token: 0x04001129 RID: 4393
		public string debugString = string.Empty;

		// Token: 0x0400112A RID: 4394
		private MaterialPropertyBlock block;

		// Token: 0x0400112B RID: 4395
		private GUIStyle debugStyle;

		// Token: 0x0400112C RID: 4396
		private static int debugCounter;

		// Token: 0x0400112D RID: 4397
		private int debugID;
	}
}
