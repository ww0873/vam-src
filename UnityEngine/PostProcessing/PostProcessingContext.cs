using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200008E RID: 142
	public class PostProcessingContext
	{
		// Token: 0x060001FF RID: 511 RVA: 0x0000FAEA File Offset: 0x0000DEEA
		public PostProcessingContext()
		{
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000FAF2 File Offset: 0x0000DEF2
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000FAFA File Offset: 0x0000DEFA
		public bool interrupted
		{
			[CompilerGenerated]
			get
			{
				return this.<interrupted>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<interrupted>k__BackingField = value;
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000FB03 File Offset: 0x0000DF03
		public void Interrupt()
		{
			this.interrupted = true;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000FB0C File Offset: 0x0000DF0C
		public PostProcessingContext Reset()
		{
			this.profile = null;
			this.camera = null;
			this.materialFactory = null;
			this.renderTextureFactory = null;
			this.interrupted = false;
			return this;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000FB32 File Offset: 0x0000DF32
		public bool isGBufferAvailable
		{
			get
			{
				return this.camera.actualRenderingPath == RenderingPath.DeferredShading;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000FB42 File Offset: 0x0000DF42
		public bool isHdr
		{
			get
			{
				return this.camera.allowHDR;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000FB4F File Offset: 0x0000DF4F
		public int width
		{
			get
			{
				return this.camera.pixelWidth;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000FB5C File Offset: 0x0000DF5C
		public int height
		{
			get
			{
				return this.camera.pixelHeight;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000FB69 File Offset: 0x0000DF69
		public Rect viewport
		{
			get
			{
				return this.camera.rect;
			}
		}

		// Token: 0x040002F6 RID: 758
		public PostProcessingProfile profile;

		// Token: 0x040002F7 RID: 759
		public Camera camera;

		// Token: 0x040002F8 RID: 760
		public MaterialFactory materialFactory;

		// Token: 0x040002F9 RID: 761
		public RenderTextureFactory renderTextureFactory;

		// Token: 0x040002FA RID: 762
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <interrupted>k__BackingField;
	}
}
