using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace OldMoatGames
{
	// Token: 0x02000007 RID: 7
	public class GifDecoder
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00002DEB File Offset: 0x000011EB
		public GifDecoder()
		{
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002E0A File Offset: 0x0000120A
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002E12 File Offset: 0x00001212
		public int NumberOfFrames
		{
			[CompilerGenerated]
			get
			{
				return this.<NumberOfFrames>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<NumberOfFrames>k__BackingField = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002E1B File Offset: 0x0000121B
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002E23 File Offset: 0x00001223
		public bool AllFramesRead
		{
			[CompilerGenerated]
			get
			{
				return this.<AllFramesRead>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<AllFramesRead>k__BackingField = value;
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002E2C File Offset: 0x0000122C
		public float GetDelayCurrentFrame()
		{
			return this._currentFrame.Delay;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002E39 File Offset: 0x00001239
		public int GetFrameCount()
		{
			return this._frameCount;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002E41 File Offset: 0x00001241
		public int GetLoopCount()
		{
			return this._loopCount;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002E49 File Offset: 0x00001249
		public GifDecoder.GifFrame GetCurrentFrame()
		{
			return this._currentFrame;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002E51 File Offset: 0x00001251
		public int GetFrameWidth()
		{
			return this._width;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002E59 File Offset: 0x00001259
		public int GetFrameHeight()
		{
			return this._height;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002E61 File Offset: 0x00001261
		public GifDecoder.Status Read(Stream inStream)
		{
			this.Init();
			if (inStream != null)
			{
				this._inStream = inStream;
				this.ReadHeader();
				if (this.Error())
				{
					this._status = GifDecoder.Status.StatusFormatError;
				}
			}
			else
			{
				this._status = GifDecoder.Status.StatusOpenError;
			}
			return this._status;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002EA0 File Offset: 0x000012A0
		public void Reset()
		{
			this._inStream.Position = 0L;
			this.Read(this._inStream);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002EBC File Offset: 0x000012BC
		public void Close()
		{
			this._inStream.Close();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002EC9 File Offset: 0x000012C9
		private bool Error()
		{
			return this._status != GifDecoder.Status.StatusOk;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002ED8 File Offset: 0x000012D8
		public void ReadNextFrame(bool loop)
		{
			while (!this.Error())
			{
				int num = this.Read();
				if (num != 0)
				{
					if (num != 33)
					{
						if (num == 44)
						{
							this.ReadImage();
							return;
						}
						if (num != 59)
						{
							this._status = GifDecoder.Status.StatusFormatError;
						}
						else
						{
							this.NumberOfFrames = this._frameCount;
							if (!loop)
							{
								this.AllFramesRead = true;
								return;
							}
							this.RewindReader();
						}
					}
					else
					{
						num = this.Read();
						if (num != 249)
						{
							if (num != 255)
							{
								this.Skip();
							}
							else
							{
								this.ReadBlock();
								string text = string.Empty;
								for (int i = 0; i < 11; i++)
								{
									text += (char)this._block[i];
								}
								if (text.Equals("NETSCAPE2.0"))
								{
									this.ReadNetscapeExt();
								}
								else
								{
									this.Skip();
								}
							}
						}
						else
						{
							this.ReadGraphicControlExt();
						}
					}
				}
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002FF4 File Offset: 0x000013F4
		private void RewindReader()
		{
			this._frameCount = 0;
			this.AllFramesRead = false;
			this._inStream.Position = this._imageDataOffset;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003018 File Offset: 0x00001418
		private void SetPixels()
		{
			if (this._lastDispose > 0 && this._lastDispose == 2)
			{
				int num = (!this._transparency) ? this._lastBgColor : 0;
				for (int i = 0; i < this._image.Length; i++)
				{
					this._image[i] = num;
				}
			}
			int num2 = 1;
			int num3 = 8;
			int num4 = 0;
			for (int j = 0; j < this._ih; j++)
			{
				int num5 = j;
				if (this._interlace)
				{
					if (num4 >= this._ih)
					{
						num2++;
						if (num2 != 2)
						{
							if (num2 != 3)
							{
								if (num2 == 4)
								{
									num4 = 1;
									num3 = 2;
								}
							}
							else
							{
								num4 = 2;
								num3 = 4;
							}
						}
						else
						{
							num4 = 4;
						}
					}
					num5 = num4;
					num4 += num3;
				}
				num5 += this._iy;
				if (num5 < this._height)
				{
					int num6 = j * this._iw;
					int num7 = this._height - num5 - 1;
					int k = num7 * this._width + this._ix;
					int num8 = k + this._iw;
					while (k < num8)
					{
						int num9 = this._act[(int)(this._pixels[num6++] & byte.MaxValue)];
						if (num9 != 0)
						{
							this._image[k] = num9;
						}
						k++;
					}
				}
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003190 File Offset: 0x00001590
		private void DecodeImageData()
		{
			int num = this._iw * this._ih;
			if (this._pixels == null || this._pixels.Length < num)
			{
				this._pixels = new byte[num];
			}
			if (this._prefix == null)
			{
				this._prefix = new short[4096];
			}
			if (this._suffix == null)
			{
				this._suffix = new byte[4096];
			}
			if (this._pixelStack == null)
			{
				this._pixelStack = new byte[4097];
			}
			int num2 = this.Read();
			int num3 = 1 << num2;
			int num4 = num3 + 1;
			int num5 = num3 + 2;
			int num6 = -1;
			int num7 = num2 + 1;
			int num8 = (1 << num7) - 1;
			for (int i = 0; i < num3; i++)
			{
				this._prefix[i] = 0;
				this._suffix[i] = (byte)i;
			}
			int num13;
			int num12;
			int num11;
			int num10;
			int num9;
			int j = num9 = (num10 = (num11 = (num12 = (num13 = 0))));
			int k = 0;
			while (k < num)
			{
				if (num12 == 0)
				{
					while (j < num7)
					{
						if (num10 == 0)
						{
							num10 = this.ReadBlock();
							num13 = 0;
						}
						num9 += (int)(this._block[num13++] & byte.MaxValue) << j;
						num10--;
						j += 8;
					}
					int i = num9 & num8;
					num9 >>= num7;
					j -= num7;
					if (i > num5 || i == num4)
					{
						break;
					}
					if (i == num3)
					{
						num7 = num2 + 1;
						num8 = (1 << num7) - 1;
						num5 = num3 + 2;
						num6 = -1;
						continue;
					}
					if (num6 == -1)
					{
						this._pixelStack[num12++] = this._suffix[i];
						num6 = i;
						num11 = i;
						continue;
					}
					int num14 = i;
					if (i == num5)
					{
						this._pixelStack[num12++] = (byte)num11;
						i = num6;
					}
					while (i > num3)
					{
						this._pixelStack[num12++] = this._suffix[i];
						i = (int)this._prefix[i];
					}
					num11 = (int)(this._suffix[i] & byte.MaxValue);
					if (num5 >= 4096)
					{
						break;
					}
					this._pixelStack[num12++] = (byte)num11;
					this._prefix[num5] = (short)num6;
					this._suffix[num5] = (byte)num11;
					num5++;
					if ((num5 & num8) == 0 && num5 < 4096)
					{
						num7++;
						num8 += num5;
					}
					num6 = num14;
				}
				num12--;
				this._pixels[k++] = this._pixelStack[num12];
			}
			while (k < num)
			{
				this._pixels[k] = 0;
				k++;
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003458 File Offset: 0x00001858
		private void Init()
		{
			this._status = GifDecoder.Status.StatusOk;
			this._frameCount = 0;
			this._currentFrame = null;
			this.AllFramesRead = false;
			this._gct = null;
			this._lct = null;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003484 File Offset: 0x00001884
		private int Read()
		{
			int result = 0;
			try
			{
				result = this._inStream.ReadByte();
			}
			catch (IOException)
			{
				this._status = GifDecoder.Status.StatusFormatError;
			}
			return result;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000034C4 File Offset: 0x000018C4
		private int ReadBlock()
		{
			this._blockSize = this.Read();
			int i = 0;
			if (this._blockSize <= 0)
			{
				return i;
			}
			try
			{
				while (i < this._blockSize)
				{
					int num = this._inStream.Read(this._block, i, this._blockSize - i);
					if (num == -1)
					{
						break;
					}
					i += num;
				}
			}
			catch (IOException)
			{
			}
			if (i < this._blockSize)
			{
				this._status = GifDecoder.Status.StatusFormatError;
			}
			return i;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003558 File Offset: 0x00001958
		private int[] ReadColorTable(int ncolors)
		{
			int num = 3 * ncolors;
			int[] array = null;
			byte[] array2 = new byte[num];
			int num2 = 0;
			try
			{
				num2 = this._inStream.Read(array2, 0, array2.Length);
			}
			catch (IOException)
			{
			}
			if (num2 < num)
			{
				this._status = GifDecoder.Status.StatusFormatError;
			}
			else
			{
				array = new int[256];
				int i = 0;
				int num3 = 0;
				while (i < ncolors)
				{
					uint num4 = (uint)array2[num3++];
					uint num5 = (uint)(array2[num3++] & byte.MaxValue);
					uint num6 = (uint)(array2[num3++] & byte.MaxValue);
					array[i++] = (int)(4278190080U | num6 << 16 | num5 << 8 | num4);
				}
			}
			return array;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000361C File Offset: 0x00001A1C
		private void ReadGraphicControlExt()
		{
			this.Read();
			int num = this.Read();
			this._dispose = (num & 28) >> 2;
			if (this._dispose == 0)
			{
				this._dispose = 1;
			}
			this._transparency = ((num & 1) != 0);
			this._delay = (float)this.ReadShort() / 100f;
			this._transIndex = this.Read();
			this.Read();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000368C File Offset: 0x00001A8C
		private void ReadHeader()
		{
			string text = string.Empty;
			for (int i = 0; i < 6; i++)
			{
				text += (char)this.Read();
			}
			if (!text.StartsWith("GIF"))
			{
				this._status = GifDecoder.Status.StatusFormatError;
				return;
			}
			this.ReadLsd();
			if (this._gctFlag && !this.Error())
			{
				this._gct = this.ReadColorTable(this._gctSize);
				this._bgColor = this._gct[this._bgIndex];
			}
			this._imageDataOffset = this._inStream.Position;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003730 File Offset: 0x00001B30
		private void ReadImage()
		{
			this._ix = this.ReadShort();
			this._iy = this.ReadShort();
			this._iw = this.ReadShort();
			this._ih = this.ReadShort();
			int num = this.Read();
			this._lctFlag = ((num & 128) != 0);
			this._interlace = ((num & 64) != 0);
			this._lctSize = 2 << (num & 7);
			if (this._lctFlag)
			{
				this._lct = this.ReadColorTable(this._lctSize);
				this._act = this._lct;
			}
			else
			{
				this._act = this._gct;
				if (this._bgIndex == this._transIndex)
				{
					this._bgColor = 0;
				}
			}
			int num2 = 0;
			if (this._transparency)
			{
				num2 = this._act[this._transIndex];
				this._act[this._transIndex] = 0;
			}
			if (this._act == null)
			{
				this._status = GifDecoder.Status.StatusFormatError;
			}
			if (this.Error())
			{
				return;
			}
			this.DecodeImageData();
			this.Skip();
			if (this.Error())
			{
				return;
			}
			if (this._image == null)
			{
				this._image = new int[this._width * this._height];
			}
			if (this._bimage == null)
			{
				this._bimage = new byte[this._width * this._height * 4];
			}
			this.SetPixels();
			Buffer.BlockCopy(this._image, 0, this._bimage, 0, this._bimage.Length);
			this._currentFrame = new GifDecoder.GifFrame(this._bimage, this._delay);
			this._frameCount++;
			if (this._transparency)
			{
				this._act[this._transIndex] = num2;
			}
			this.ResetFrame();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003904 File Offset: 0x00001D04
		private void ReadLsd()
		{
			this._width = this.ReadShort();
			this._height = this.ReadShort();
			int num = this.Read();
			this._gctFlag = ((num & 128) != 0);
			this._gctSize = 2 << (num & 7);
			this._bgIndex = this.Read();
			this.Read();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003964 File Offset: 0x00001D64
		private void ReadNetscapeExt()
		{
			do
			{
				this.ReadBlock();
				if (this._block[0] == 1)
				{
					int num = (int)(this._block[1] & byte.MaxValue);
					int num2 = (int)(this._block[2] & byte.MaxValue);
					this._loopCount = (num2 << 8 | num);
				}
			}
			while (this._blockSize > 0 && !this.Error());
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000039CB File Offset: 0x00001DCB
		private int ReadShort()
		{
			return this.Read() | this.Read() << 8;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000039DC File Offset: 0x00001DDC
		private void ResetFrame()
		{
			this._lastDispose = this._dispose;
			this._lastBgColor = this._bgColor;
			this._lct = null;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000039FD File Offset: 0x00001DFD
		private void Skip()
		{
			do
			{
				this.ReadBlock();
			}
			while (this._blockSize > 0 && !this.Error());
		}

		// Token: 0x0400002D RID: 45
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <NumberOfFrames>k__BackingField;

		// Token: 0x0400002E RID: 46
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <AllFramesRead>k__BackingField;

		// Token: 0x0400002F RID: 47
		private Stream _inStream;

		// Token: 0x04000030 RID: 48
		private GifDecoder.Status _status;

		// Token: 0x04000031 RID: 49
		private int _width;

		// Token: 0x04000032 RID: 50
		private int _height;

		// Token: 0x04000033 RID: 51
		private bool _gctFlag;

		// Token: 0x04000034 RID: 52
		private int _gctSize;

		// Token: 0x04000035 RID: 53
		private int _loopCount = 1;

		// Token: 0x04000036 RID: 54
		private int[] _gct;

		// Token: 0x04000037 RID: 55
		private int[] _lct;

		// Token: 0x04000038 RID: 56
		private int[] _act;

		// Token: 0x04000039 RID: 57
		private int _bgIndex;

		// Token: 0x0400003A RID: 58
		private int _bgColor;

		// Token: 0x0400003B RID: 59
		private int _lastBgColor;

		// Token: 0x0400003C RID: 60
		private bool _lctFlag;

		// Token: 0x0400003D RID: 61
		private bool _interlace;

		// Token: 0x0400003E RID: 62
		private int _lctSize;

		// Token: 0x0400003F RID: 63
		private int _ix;

		// Token: 0x04000040 RID: 64
		private int _iy;

		// Token: 0x04000041 RID: 65
		private int _iw;

		// Token: 0x04000042 RID: 66
		private int _ih;

		// Token: 0x04000043 RID: 67
		private int[] _image;

		// Token: 0x04000044 RID: 68
		private byte[] _bimage;

		// Token: 0x04000045 RID: 69
		private readonly byte[] _block = new byte[256];

		// Token: 0x04000046 RID: 70
		private int _blockSize;

		// Token: 0x04000047 RID: 71
		private int _dispose;

		// Token: 0x04000048 RID: 72
		private int _lastDispose;

		// Token: 0x04000049 RID: 73
		private bool _transparency;

		// Token: 0x0400004A RID: 74
		private float _delay;

		// Token: 0x0400004B RID: 75
		private int _transIndex;

		// Token: 0x0400004C RID: 76
		private long _imageDataOffset;

		// Token: 0x0400004D RID: 77
		private const int MaxStackSize = 4096;

		// Token: 0x0400004E RID: 78
		private short[] _prefix;

		// Token: 0x0400004F RID: 79
		private byte[] _suffix;

		// Token: 0x04000050 RID: 80
		private byte[] _pixelStack;

		// Token: 0x04000051 RID: 81
		private byte[] _pixels;

		// Token: 0x04000052 RID: 82
		private GifDecoder.GifFrame _currentFrame;

		// Token: 0x04000053 RID: 83
		private int _frameCount;

		// Token: 0x02000008 RID: 8
		public enum Status
		{
			// Token: 0x04000055 RID: 85
			StatusOk,
			// Token: 0x04000056 RID: 86
			StatusFormatError,
			// Token: 0x04000057 RID: 87
			StatusOpenError
		}

		// Token: 0x02000009 RID: 9
		public class GifFrame
		{
			// Token: 0x06000063 RID: 99 RVA: 0x00003A1D File Offset: 0x00001E1D
			public GifFrame(byte[] im, float del)
			{
				this.Image = im;
				this.Delay = del;
			}

			// Token: 0x04000058 RID: 88
			public byte[] Image;

			// Token: 0x04000059 RID: 89
			public float Delay;
		}
	}
}
