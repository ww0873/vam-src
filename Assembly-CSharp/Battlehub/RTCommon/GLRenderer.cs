using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000B6 RID: 182
	[ExecuteInEditMode]
	public class GLRenderer : MonoBehaviour
	{
		// Token: 0x060002F0 RID: 752 RVA: 0x00013BA5 File Offset: 0x00011FA5
		public GLRenderer()
		{
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x00013BAD File Offset: 0x00011FAD
		public static GLRenderer Instance
		{
			get
			{
				return GLRenderer.m_instance;
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00013BB4 File Offset: 0x00011FB4
		public void Add(IGL gl)
		{
			if (this.m_renderObjects.Contains(gl))
			{
				return;
			}
			this.m_renderObjects.Add(gl);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00013BD4 File Offset: 0x00011FD4
		public void Remove(IGL gl)
		{
			this.m_renderObjects.Remove(gl);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00013BE3 File Offset: 0x00011FE3
		private void Awake()
		{
			if (GLRenderer.m_instance != null)
			{
				Debug.LogWarning("Another instance of GLLinesRenderer aleready exist");
			}
			GLRenderer.m_instance = this;
			this.m_renderObjects = new List<IGL>();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00013C10 File Offset: 0x00012010
		private void OnDestroy()
		{
			if (GLRenderer.m_instance == this)
			{
				GLRenderer.m_instance = null;
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00013C28 File Offset: 0x00012028
		public void Draw(int cullingMask)
		{
			if (this.m_renderObjects == null)
			{
				return;
			}
			GL.PushMatrix();
			try
			{
				for (int i = 0; i < this.m_renderObjects.Count; i++)
				{
					IGL igl = this.m_renderObjects[i];
					igl.Draw(cullingMask);
				}
			}
			finally
			{
				GL.PopMatrix();
			}
		}

		// Token: 0x040003A6 RID: 934
		private static GLRenderer m_instance;

		// Token: 0x040003A7 RID: 935
		private List<IGL> m_renderObjects;
	}
}
