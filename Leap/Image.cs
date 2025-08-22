using System;
using LeapInternal;

namespace Leap
{
	// Token: 0x020005DF RID: 1503
	public class Image
	{
		// Token: 0x060025FC RID: 9724 RVA: 0x000D7364 File Offset: 0x000D5764
		public Image(long frameId, long timestamp, ImageData leftImage, ImageData rightImage)
		{
			if (leftImage == null || rightImage == null)
			{
				throw new ArgumentNullException("images");
			}
			if (leftImage.type != rightImage.type || leftImage.format != rightImage.format || leftImage.width != rightImage.width || leftImage.height != rightImage.height || leftImage.bpp != rightImage.bpp || leftImage.DistortionSize != rightImage.DistortionSize)
			{
				throw new ArgumentException("image mismatch");
			}
			this.frameId = frameId;
			this.timestamp = timestamp;
			this.leftImage = leftImage;
			this.rightImage = rightImage;
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000D7423 File Offset: 0x000D5823
		private ImageData imageData(Image.CameraType camera)
		{
			return (camera != Image.CameraType.LEFT) ? this.rightImage : this.leftImage;
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000D743C File Offset: 0x000D583C
		public byte[] Data(Image.CameraType camera)
		{
			if (camera != Image.CameraType.LEFT && camera != Image.CameraType.RIGHT)
			{
				return null;
			}
			return this.imageData(camera).AsByteArray;
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000D7459 File Offset: 0x000D5859
		public uint ByteOffset(Image.CameraType camera)
		{
			if (camera != Image.CameraType.LEFT && camera != Image.CameraType.RIGHT)
			{
				return 0U;
			}
			return this.imageData(camera).byteOffset;
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06002600 RID: 9728 RVA: 0x000D7476 File Offset: 0x000D5876
		public uint NumBytes
		{
			get
			{
				return this.leftImage.width * this.leftImage.height * this.leftImage.bpp;
			}
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000D749B File Offset: 0x000D589B
		public float[] Distortion(Image.CameraType camera)
		{
			if (camera != Image.CameraType.LEFT && camera != Image.CameraType.RIGHT)
			{
				return null;
			}
			return this.imageData(camera).DistortionData.Data;
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x000D74BD File Offset: 0x000D58BD
		public Vector PixelToRectilinear(Image.CameraType camera, Vector pixel)
		{
			return Connection.GetConnection(0).PixelToRectilinear(camera, pixel);
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x000D74CC File Offset: 0x000D58CC
		public Vector RectilinearToPixel(Image.CameraType camera, Vector ray)
		{
			return Connection.GetConnection(0).RectilinearToPixel(camera, ray);
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x000D74DB File Offset: 0x000D58DB
		public bool Equals(Image other)
		{
			return this.frameId == other.frameId && this.Type == other.Type && this.Timestamp == other.Timestamp;
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000D7510 File Offset: 0x000D5910
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Image sequence",
				this.frameId,
				", format: ",
				this.Format,
				", type: ",
				this.Type
			});
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06002606 RID: 9734 RVA: 0x000D756A File Offset: 0x000D596A
		public long SequenceId
		{
			get
			{
				return this.frameId;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06002607 RID: 9735 RVA: 0x000D7572 File Offset: 0x000D5972
		public int Width
		{
			get
			{
				return (int)this.leftImage.width;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06002608 RID: 9736 RVA: 0x000D757F File Offset: 0x000D597F
		public int Height
		{
			get
			{
				return (int)this.leftImage.height;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06002609 RID: 9737 RVA: 0x000D758C File Offset: 0x000D598C
		public int BytesPerPixel
		{
			get
			{
				return (int)this.leftImage.bpp;
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x0600260A RID: 9738 RVA: 0x000D759C File Offset: 0x000D599C
		public Image.FormatType Format
		{
			get
			{
				eLeapImageFormat format = this.leftImage.format;
				if (format == eLeapImageFormat.eLeapImageType_IR)
				{
					return Image.FormatType.INFRARED;
				}
				if (format != eLeapImageFormat.eLeapImageType_RGBIr_Bayer)
				{
					return Image.FormatType.INFRARED;
				}
				return Image.FormatType.IBRG;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x0600260B RID: 9739 RVA: 0x000D75D8 File Offset: 0x000D59D8
		public Image.ImageType Type
		{
			get
			{
				eLeapImageType type = this.leftImage.type;
				if (type == eLeapImageType.eLeapImageType_Default)
				{
					return Image.ImageType.DEFAULT;
				}
				if (type != eLeapImageType.eLeapImageType_Raw)
				{
					return Image.ImageType.DEFAULT;
				}
				return Image.ImageType.RAW;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x0600260C RID: 9740 RVA: 0x000D7609 File Offset: 0x000D5A09
		public int DistortionWidth
		{
			get
			{
				return this.leftImage.DistortionSize * 2;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x0600260D RID: 9741 RVA: 0x000D7618 File Offset: 0x000D5A18
		public int DistortionHeight
		{
			get
			{
				return this.leftImage.DistortionSize;
			}
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x000D7625 File Offset: 0x000D5A25
		public float RayOffsetX(Image.CameraType camera)
		{
			if (camera != Image.CameraType.LEFT && camera != Image.CameraType.RIGHT)
			{
				return 0f;
			}
			return this.imageData(camera).RayOffsetX;
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000D7646 File Offset: 0x000D5A46
		public float RayOffsetY(Image.CameraType camera)
		{
			if (camera != Image.CameraType.LEFT && camera != Image.CameraType.RIGHT)
			{
				return 0f;
			}
			return this.imageData(camera).RayOffsetY;
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000D7667 File Offset: 0x000D5A67
		public float RayScaleX(Image.CameraType camera)
		{
			if (camera != Image.CameraType.LEFT && camera != Image.CameraType.RIGHT)
			{
				return 0f;
			}
			return this.imageData(camera).RayScaleX;
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x000D7688 File Offset: 0x000D5A88
		public float RayScaleY(Image.CameraType camera)
		{
			if (camera != Image.CameraType.LEFT && camera != Image.CameraType.RIGHT)
			{
				return 0f;
			}
			return this.imageData(camera).RayScaleY;
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06002612 RID: 9746 RVA: 0x000D76A9 File Offset: 0x000D5AA9
		public long Timestamp
		{
			get
			{
				return this.timestamp;
			}
		}

		// Token: 0x04001F95 RID: 8085
		private ImageData leftImage;

		// Token: 0x04001F96 RID: 8086
		private ImageData rightImage;

		// Token: 0x04001F97 RID: 8087
		private long frameId;

		// Token: 0x04001F98 RID: 8088
		private long timestamp;

		// Token: 0x020005E0 RID: 1504
		public enum FormatType
		{
			// Token: 0x04001F9A RID: 8090
			INFRARED,
			// Token: 0x04001F9B RID: 8091
			IBRG
		}

		// Token: 0x020005E1 RID: 1505
		public enum ImageType
		{
			// Token: 0x04001F9D RID: 8093
			DEFAULT,
			// Token: 0x04001F9E RID: 8094
			RAW
		}

		// Token: 0x020005E2 RID: 1506
		public enum CameraType
		{
			// Token: 0x04001FA0 RID: 8096
			LEFT,
			// Token: 0x04001FA1 RID: 8097
			RIGHT
		}
	}
}
