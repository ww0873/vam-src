using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Leap;

namespace LeapInternal
{
	// Token: 0x020005E3 RID: 1507
	public class ImageData
	{
		// Token: 0x06002613 RID: 9747 RVA: 0x000D76B4 File Offset: 0x000D5AB4
		public ImageData(Image.CameraType camera, LEAP_IMAGE image, DistortionData distortionData)
		{
			this.camera = camera;
			this._properties = image.properties;
			this.DistortionMatrixKey = image.matrix_version;
			this.DistortionData = distortionData;
			this._object = MemoryManager.GetPinnedObject(image.data);
			this.byteOffset = image.offset;
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06002614 RID: 9748 RVA: 0x000D770E File Offset: 0x000D5B0E
		// (set) Token: 0x06002615 RID: 9749 RVA: 0x000D7716 File Offset: 0x000D5B16
		public Image.CameraType camera
		{
			[CompilerGenerated]
			get
			{
				return this.<camera>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<camera>k__BackingField = value;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06002616 RID: 9750 RVA: 0x000D771F File Offset: 0x000D5B1F
		public eLeapImageType type
		{
			get
			{
				return this._properties.type;
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06002617 RID: 9751 RVA: 0x000D772C File Offset: 0x000D5B2C
		public eLeapImageFormat format
		{
			get
			{
				return this._properties.format;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06002618 RID: 9752 RVA: 0x000D7739 File Offset: 0x000D5B39
		public uint bpp
		{
			get
			{
				return this._properties.bpp;
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06002619 RID: 9753 RVA: 0x000D7746 File Offset: 0x000D5B46
		public uint width
		{
			get
			{
				return this._properties.width;
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x0600261A RID: 9754 RVA: 0x000D7753 File Offset: 0x000D5B53
		public uint height
		{
			get
			{
				return this._properties.height;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x0600261B RID: 9755 RVA: 0x000D7760 File Offset: 0x000D5B60
		public float RayScaleX
		{
			get
			{
				return this._properties.x_scale;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x0600261C RID: 9756 RVA: 0x000D776D File Offset: 0x000D5B6D
		public float RayScaleY
		{
			get
			{
				return this._properties.y_scale;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x0600261D RID: 9757 RVA: 0x000D777A File Offset: 0x000D5B7A
		public float RayOffsetX
		{
			get
			{
				return this._properties.x_offset;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x0600261E RID: 9758 RVA: 0x000D7787 File Offset: 0x000D5B87
		public float RayOffsetY
		{
			get
			{
				return this._properties.y_offset;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x0600261F RID: 9759 RVA: 0x000D7794 File Offset: 0x000D5B94
		public byte[] AsByteArray
		{
			get
			{
				return this._object as byte[];
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06002620 RID: 9760 RVA: 0x000D77A1 File Offset: 0x000D5BA1
		public float[] AsFloatArray
		{
			get
			{
				return this._object as float[];
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06002621 RID: 9761 RVA: 0x000D77AE File Offset: 0x000D5BAE
		// (set) Token: 0x06002622 RID: 9762 RVA: 0x000D77B6 File Offset: 0x000D5BB6
		public uint byteOffset
		{
			[CompilerGenerated]
			get
			{
				return this.<byteOffset>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<byteOffset>k__BackingField = value;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06002623 RID: 9763 RVA: 0x000D77BF File Offset: 0x000D5BBF
		public int DistortionSize
		{
			get
			{
				return LeapC.DistortionSize;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06002624 RID: 9764 RVA: 0x000D77C6 File Offset: 0x000D5BC6
		// (set) Token: 0x06002625 RID: 9765 RVA: 0x000D77CE File Offset: 0x000D5BCE
		public ulong DistortionMatrixKey
		{
			[CompilerGenerated]
			get
			{
				return this.<DistortionMatrixKey>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<DistortionMatrixKey>k__BackingField = value;
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06002626 RID: 9766 RVA: 0x000D77D7 File Offset: 0x000D5BD7
		// (set) Token: 0x06002627 RID: 9767 RVA: 0x000D77DF File Offset: 0x000D5BDF
		public DistortionData DistortionData
		{
			[CompilerGenerated]
			get
			{
				return this.<DistortionData>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<DistortionData>k__BackingField = value;
			}
		}

		// Token: 0x04001FA2 RID: 8098
		private LEAP_IMAGE_PROPERTIES _properties;

		// Token: 0x04001FA3 RID: 8099
		private object _object;

		// Token: 0x04001FA4 RID: 8100
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Image.CameraType <camera>k__BackingField;

		// Token: 0x04001FA5 RID: 8101
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private uint <byteOffset>k__BackingField;

		// Token: 0x04001FA6 RID: 8102
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ulong <DistortionMatrixKey>k__BackingField;

		// Token: 0x04001FA7 RID: 8103
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DistortionData <DistortionData>k__BackingField;
	}
}
