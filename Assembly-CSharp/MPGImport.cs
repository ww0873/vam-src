using System;
using System.Runtime.InteropServices;

// Token: 0x02000428 RID: 1064
public class MPGImport
{
	// Token: 0x06001AA8 RID: 6824 RVA: 0x00095B28 File Offset: 0x00093F28
	public MPGImport()
	{
	}

	// Token: 0x06001AA9 RID: 6825
	[DllImport("libmpg123-0")]
	public static extern int mpg123_init();

	// Token: 0x06001AAA RID: 6826
	[DllImport("libmpg123-0")]
	public static extern void mpg123_exit();

	// Token: 0x06001AAB RID: 6827
	[DllImport("libmpg123-0", CharSet = CharSet.Ansi)]
	public static extern IntPtr mpg123_new(string decoder, IntPtr error);

	// Token: 0x06001AAC RID: 6828
	[DllImport("libmpg123-0")]
	public static extern void mpg123_delete(IntPtr mh);

	// Token: 0x06001AAD RID: 6829
	[DllImport("libmpg123-0")]
	public static extern int mpg123_param(IntPtr mh, MPGImport.mpg123_parms type, int value, double fvalue);

	// Token: 0x06001AAE RID: 6830
	[DllImport("libmpg123-0")]
	public static extern int mpg123_getparam(IntPtr mh, MPGImport.mpg123_parms type, IntPtr val, IntPtr fval);

	// Token: 0x06001AAF RID: 6831
	[DllImport("libmpg123-0")]
	public static extern int mpg123_feature(MPGImport.mpg123_feature_set key);

	// Token: 0x06001AB0 RID: 6832
	[DllImport("libmpg123-0", CharSet = CharSet.Ansi)]
	public static extern string mpg123_plain_strerror(int errcode);

	// Token: 0x06001AB1 RID: 6833
	[DllImport("libmpg123-0", CharSet = CharSet.Ansi)]
	public static extern string mpg123_strerror(IntPtr mh);

	// Token: 0x06001AB2 RID: 6834
	[DllImport("libmpg123-0")]
	public static extern int mpg123_errcode(IntPtr mh);

	// Token: 0x06001AB3 RID: 6835
	[DllImport("libmpg123-0", CharSet = CharSet.Ansi)]
	public static extern IntPtr mpg123_decoders();

	// Token: 0x06001AB4 RID: 6836
	[DllImport("libmpg123-0", CharSet = CharSet.Ansi)]
	public static extern IntPtr mpg123_supported_decoders();

	// Token: 0x06001AB5 RID: 6837
	[DllImport("libmpg123-0")]
	public static extern int mpg123_decoder(IntPtr mh, string decoder_name);

	// Token: 0x06001AB6 RID: 6838
	[DllImport("libmpg123-0")]
	public static extern string mpg123_current_decoder(IntPtr mh);

	// Token: 0x06001AB7 RID: 6839
	[DllImport("libmpg123-0")]
	public static extern void mpg123_rates(IntPtr list, IntPtr number);

	// Token: 0x06001AB8 RID: 6840
	[DllImport("libmpg123-0")]
	public static extern void mpg123_encodings(IntPtr list, IntPtr number);

	// Token: 0x06001AB9 RID: 6841
	[DllImport("libmpg123-0")]
	public static extern int mpg123_format_none(IntPtr mh);

	// Token: 0x06001ABA RID: 6842
	[DllImport("libmpg123-0")]
	public static extern int mpg123_format_all(IntPtr mh);

	// Token: 0x06001ABB RID: 6843
	[DllImport("libmpg123-0")]
	public static extern int mpg123_format(IntPtr mh, int rate, int channels, int encodings);

	// Token: 0x06001ABC RID: 6844
	[DllImport("libmpg123-0")]
	public static extern int mpg123_format_support(IntPtr mh, int rate, int encoding);

	// Token: 0x06001ABD RID: 6845
	[DllImport("libmpg123-0")]
	public static extern int mpg123_getformat(IntPtr mh, out IntPtr rate, out IntPtr channels, out IntPtr encoding);

	// Token: 0x06001ABE RID: 6846
	[DllImport("libmpg123-0", CharSet = CharSet.Ansi)]
	public static extern int mpg123_open(IntPtr mh, string path);

	// Token: 0x06001ABF RID: 6847
	[DllImport("libmpg123-0")]
	public static extern int mpg123_open_fd(IntPtr mh, int fd);

	// Token: 0x06001AC0 RID: 6848
	[DllImport("libmpg123-0")]
	public static extern int mpg123_open_handle(IntPtr mh, IntPtr iohandle);

	// Token: 0x06001AC1 RID: 6849
	[DllImport("libmpg123-0")]
	public static extern int mpg123_open_feed(IntPtr mh);

	// Token: 0x06001AC2 RID: 6850
	[DllImport("libmpg123-0")]
	public static extern int mpg123_close(IntPtr mh);

	// Token: 0x06001AC3 RID: 6851
	[DllImport("libmpg123-0")]
	public static extern int mpg123_read(IntPtr mh, byte[] outmemory, int outmemsize, out IntPtr done);

	// Token: 0x06001AC4 RID: 6852
	[DllImport("libmpg123-0")]
	public static extern int mpg123_feed(IntPtr mh, IntPtr input, int size);

	// Token: 0x06001AC5 RID: 6853
	[DllImport("libmpg123-0")]
	public static extern int mpg123_decode(IntPtr mh, IntPtr inmemory, int inmemsize, IntPtr outmemory, int outmemsize, IntPtr done);

	// Token: 0x06001AC6 RID: 6854
	[DllImport("libmpg123-0")]
	public static extern int mpg123_decode_frame(IntPtr mh, IntPtr num, IntPtr audio, IntPtr bytes);

	// Token: 0x06001AC7 RID: 6855
	[DllImport("libmpg123-0")]
	public static extern int mpg123_framebyframe_decode(IntPtr mh, IntPtr num, IntPtr audio, IntPtr bytes);

	// Token: 0x06001AC8 RID: 6856
	[DllImport("libmpg123-0")]
	public static extern int mpg123_framebyframe_next(IntPtr mh);

	// Token: 0x06001AC9 RID: 6857
	[DllImport("libmpg123-0")]
	public static extern int mpg123_tell(IntPtr mh);

	// Token: 0x06001ACA RID: 6858
	[DllImport("libmpg123-0")]
	public static extern int mpg123_tellframe(IntPtr mh);

	// Token: 0x06001ACB RID: 6859
	[DllImport("libmpg123-0")]
	public static extern int mpg123_tell_stream(IntPtr mh);

	// Token: 0x06001ACC RID: 6860
	[DllImport("libmpg123-0")]
	public static extern int mpg123_seek(IntPtr mh, int sampleoff, int whence);

	// Token: 0x06001ACD RID: 6861
	[DllImport("libmpg123-0")]
	public static extern int mpg123_feedseek(IntPtr mh, int sampleoff, int whence, IntPtr input_offset);

	// Token: 0x06001ACE RID: 6862
	[DllImport("libmpg123-0")]
	public static extern int mpg123_seek_frame(IntPtr mh, int frameoff, int whence);

	// Token: 0x06001ACF RID: 6863
	[DllImport("libmpg123-0")]
	public static extern int mpg123_timeframe(IntPtr mh, double sec);

	// Token: 0x06001AD0 RID: 6864
	[DllImport("libmpg123-0")]
	public static extern int mpg123_index(IntPtr mh, IntPtr offsets, IntPtr step, IntPtr fill);

	// Token: 0x06001AD1 RID: 6865
	[DllImport("libmpg123-0")]
	public static extern int mpg123_set_index(IntPtr mh, IntPtr offsets, int step, int fill);

	// Token: 0x06001AD2 RID: 6866
	[DllImport("libmpg123-0")]
	public static extern int mpg123_position(IntPtr mh, int frame_offset, int buffered_bytes, IntPtr current_frame, IntPtr frames_left, IntPtr current_seconds, IntPtr seconds_left);

	// Token: 0x06001AD3 RID: 6867
	[DllImport("libmpg123-0")]
	public static extern int mpg123_eq(IntPtr mh, MPGImport.mpg123_channels channel, int band, double val);

	// Token: 0x06001AD4 RID: 6868
	[DllImport("libmpg123-0")]
	public static extern double mpg123_geteq(IntPtr mh, MPGImport.mpg123_channels channel, int band);

	// Token: 0x06001AD5 RID: 6869
	[DllImport("libmpg123-0")]
	public static extern int mpg123_reset_eq(IntPtr mh);

	// Token: 0x06001AD6 RID: 6870
	[DllImport("libmpg123-0")]
	public static extern int mpg123_volume(IntPtr mh, double vol);

	// Token: 0x06001AD7 RID: 6871
	[DllImport("libmpg123-0")]
	public static extern int mpg123_volume_change(IntPtr mh, double change);

	// Token: 0x06001AD8 RID: 6872
	[DllImport("libmpg123-0")]
	public static extern int mpg123_getvolume(IntPtr mh, IntPtr _base, IntPtr really, IntPtr rva_db);

	// Token: 0x06001AD9 RID: 6873
	[DllImport("libmpg123-0")]
	public static extern int mpg123_info(IntPtr mh, IntPtr mi);

	// Token: 0x06001ADA RID: 6874
	[DllImport("libmpg123-0")]
	public static extern int mpg123_safe_buffer();

	// Token: 0x06001ADB RID: 6875
	[DllImport("libmpg123-0")]
	public static extern int mpg123_scan(IntPtr mh);

	// Token: 0x06001ADC RID: 6876
	[DllImport("libmpg123-0")]
	public static extern int mpg123_length(IntPtr mh);

	// Token: 0x06001ADD RID: 6877
	[DllImport("libmpg123-0")]
	public static extern int mpg123_set_filesize(IntPtr mh, int size);

	// Token: 0x06001ADE RID: 6878
	[DllImport("libmpg123-0")]
	public static extern double mpg123_tpf(IntPtr mh);

	// Token: 0x06001ADF RID: 6879
	[DllImport("libmpg123-0")]
	public static extern int mpg123_clip(IntPtr mh);

	// Token: 0x06001AE0 RID: 6880
	[DllImport("libmpg123-0")]
	public static extern int mpg123_getstate(IntPtr mh, MPGImport.mpg123_state key, IntPtr val, IntPtr fval);

	// Token: 0x06001AE1 RID: 6881
	[DllImport("libmpg123-0")]
	public static extern void mpg123_init_string(IntPtr sb);

	// Token: 0x06001AE2 RID: 6882
	[DllImport("libmpg123-0")]
	public static extern void mpg123_free_string(IntPtr sb);

	// Token: 0x06001AE3 RID: 6883
	[DllImport("libmpg123-0")]
	public static extern int mpg123_resize_string(IntPtr sb, int news);

	// Token: 0x06001AE4 RID: 6884
	[DllImport("libmpg123-0")]
	public static extern int mpg123_grow_string(IntPtr sb, int news);

	// Token: 0x06001AE5 RID: 6885
	[DllImport("libmpg123-0")]
	public static extern int mpg123_copy_string(IntPtr from, IntPtr to);

	// Token: 0x06001AE6 RID: 6886
	[DllImport("libmpg123-0")]
	public static extern int mpg123_add_string(IntPtr sb, string stuff);

	// Token: 0x06001AE7 RID: 6887
	[DllImport("libmpg123-0")]
	public static extern int mpg123_add_substring(IntPtr sb, string stuff, int from, int count);

	// Token: 0x06001AE8 RID: 6888
	[DllImport("libmpg123-0")]
	public static extern int mpg123_set_string(IntPtr sb, string stuff);

	// Token: 0x06001AE9 RID: 6889
	[DllImport("libmpg123-0")]
	public static extern int mpg123_set_substring(IntPtr sb, string stuff, int from, int count);

	// Token: 0x06001AEA RID: 6890
	[DllImport("libmpg123-0")]
	public static extern MPGImport.mpg123_text_encoding mpg123_enc_from_id3(byte id3_enc_byte);

	// Token: 0x06001AEB RID: 6891
	[DllImport("libmpg123-0")]
	public static extern int mpg123_store_utf8(IntPtr sb, MPGImport.mpg123_text_encoding enc, string source, int source_size);

	// Token: 0x06001AEC RID: 6892
	[DllImport("libmpg123-0")]
	public static extern int mpg123_meta_check(IntPtr mh);

	// Token: 0x06001AED RID: 6893
	[DllImport("libmpg123-0")]
	public static extern int mpg123_id3(IntPtr mh, out IntPtr v1, out IntPtr v2);

	// Token: 0x06001AEE RID: 6894
	[DllImport("libmpg123-0")]
	public static extern int mpg123_icy(IntPtr mh, IntPtr icy_meta);

	// Token: 0x06001AEF RID: 6895
	[DllImport("libmpg123-0")]
	public static extern string mpg123_icy2utf8(string icy_text);

	// Token: 0x06001AF0 RID: 6896
	[DllImport("libmpg123-0")]
	public static extern IntPtr mpg123_parnew(IntPtr mp, string decoder, IntPtr error);

	// Token: 0x06001AF1 RID: 6897
	[DllImport("libmpg123-0")]
	public static extern IntPtr mpg123_new_pars(IntPtr error);

	// Token: 0x06001AF2 RID: 6898
	[DllImport("libmpg123-0")]
	public static extern void mpg123_delete_pars(IntPtr mp);

	// Token: 0x06001AF3 RID: 6899
	[DllImport("libmpg123-0")]
	public static extern int mpg123_fmt_none(IntPtr mp);

	// Token: 0x06001AF4 RID: 6900
	[DllImport("libmpg123-0")]
	public static extern int mpg123_fmt_all(IntPtr mp);

	// Token: 0x06001AF5 RID: 6901
	[DllImport("libmpg123-0")]
	public static extern int mpg123_fmt(IntPtr mh, int rate, int channels, int encodings);

	// Token: 0x06001AF6 RID: 6902
	[DllImport("libmpg123-0")]
	public static extern int mpg123_fmt_support(IntPtr mh, int rate, int encoding);

	// Token: 0x06001AF7 RID: 6903
	[DllImport("libmpg123-0")]
	public static extern int mpg123_par(IntPtr mp, MPGImport.mpg123_parms type, int value, double fvalue);

	// Token: 0x06001AF8 RID: 6904
	[DllImport("libmpg123-0")]
	public static extern int mpg123_getpar(IntPtr mp, MPGImport.mpg123_parms type, IntPtr val, IntPtr fval);

	// Token: 0x06001AF9 RID: 6905
	[DllImport("libmpg123-0")]
	public static extern int mpg123_replace_buffer(IntPtr mh, string data, int size);

	// Token: 0x06001AFA RID: 6906
	[DllImport("libmpg123-0")]
	public static extern int mpg123_outblock(IntPtr mh);

	// Token: 0x040015CF RID: 5583
	private const string Mpg123Dll = "libmpg123-0";

	// Token: 0x02000429 RID: 1065
	public enum mpg123_parms
	{
		// Token: 0x040015D1 RID: 5585
		MPG123_VERBOSE,
		// Token: 0x040015D2 RID: 5586
		MPG123_FLAGS,
		// Token: 0x040015D3 RID: 5587
		MPG123_ADD_FLAGS,
		// Token: 0x040015D4 RID: 5588
		MPG123_FORCE_RATE,
		// Token: 0x040015D5 RID: 5589
		MPG123_DOWN_SAMPLE,
		// Token: 0x040015D6 RID: 5590
		MPG123_RVA,
		// Token: 0x040015D7 RID: 5591
		MPG123_DOWNSPEED,
		// Token: 0x040015D8 RID: 5592
		MPG123_UPSPEED,
		// Token: 0x040015D9 RID: 5593
		MPG123_START_FRAME,
		// Token: 0x040015DA RID: 5594
		MPG123_DECODE_FRAMES,
		// Token: 0x040015DB RID: 5595
		MPG123_ICY_INTERVAL,
		// Token: 0x040015DC RID: 5596
		MPG123_OUTSCALE,
		// Token: 0x040015DD RID: 5597
		MPG123_TIMEOUT,
		// Token: 0x040015DE RID: 5598
		MPG123_REMOVE_FLAGS,
		// Token: 0x040015DF RID: 5599
		MPG123_RESYNC_LIMIT,
		// Token: 0x040015E0 RID: 5600
		MPG123_INDEX_SIZE,
		// Token: 0x040015E1 RID: 5601
		MPG123_PREFRAMES
	}

	// Token: 0x0200042A RID: 1066
	public enum mpg123_param_flags
	{
		// Token: 0x040015E3 RID: 5603
		MPG123_FORCE_MONO = 7,
		// Token: 0x040015E4 RID: 5604
		MPG123_MONO_LEFT = 1,
		// Token: 0x040015E5 RID: 5605
		MPG123_MONO_RIGHT,
		// Token: 0x040015E6 RID: 5606
		MPG123_MONO_MIX = 4,
		// Token: 0x040015E7 RID: 5607
		MPG123_FORCE_STEREO = 8,
		// Token: 0x040015E8 RID: 5608
		MPG123_FORCE_8BIT = 16,
		// Token: 0x040015E9 RID: 5609
		MPG123_QUIET = 32,
		// Token: 0x040015EA RID: 5610
		MPG123_GAPLESS = 64,
		// Token: 0x040015EB RID: 5611
		MPG123_NO_RESYNC = 128,
		// Token: 0x040015EC RID: 5612
		MPG123_SEEKBUFFER = 256,
		// Token: 0x040015ED RID: 5613
		MPG123_FUZZY = 512,
		// Token: 0x040015EE RID: 5614
		MPG123_FORCE_FLOAT = 1024,
		// Token: 0x040015EF RID: 5615
		MPG123_PLAIN_ID3TEXT = 2048,
		// Token: 0x040015F0 RID: 5616
		MPG123_IGNORE_STREAMLENGTH = 4096
	}

	// Token: 0x0200042B RID: 1067
	public enum mpg123_param_rva
	{
		// Token: 0x040015F2 RID: 5618
		MPG123_RVA_OFF,
		// Token: 0x040015F3 RID: 5619
		MPG123_RVA_MIX,
		// Token: 0x040015F4 RID: 5620
		MPG123_RVA_ALBUM,
		// Token: 0x040015F5 RID: 5621
		MPG123_RVA_MAX = 2
	}

	// Token: 0x0200042C RID: 1068
	public enum mpg123_feature_set
	{
		// Token: 0x040015F7 RID: 5623
		MPG123_FEATURE_ABI_UTF8OPEN,
		// Token: 0x040015F8 RID: 5624
		MPG123_FEATURE_OUTPUT_8BIT,
		// Token: 0x040015F9 RID: 5625
		MPG123_FEATURE_OUTPUT_16BIT,
		// Token: 0x040015FA RID: 5626
		MPG123_FEATURE_OUTPUT_32BIT,
		// Token: 0x040015FB RID: 5627
		MPG123_FEATURE_INDEX,
		// Token: 0x040015FC RID: 5628
		MPG123_FEATURE_PARSE_ID3V2,
		// Token: 0x040015FD RID: 5629
		MPG123_FEATURE_DECODE_LAYER1,
		// Token: 0x040015FE RID: 5630
		MPG123_FEATURE_DECODE_LAYER2,
		// Token: 0x040015FF RID: 5631
		MPG123_FEATURE_DECODE_LAYER3,
		// Token: 0x04001600 RID: 5632
		MPG123_FEATURE_DECODE_ACCURATE,
		// Token: 0x04001601 RID: 5633
		MPG123_FEATURE_DECODE_DOWNSAMPLE,
		// Token: 0x04001602 RID: 5634
		MPG123_FEATURE_DECODE_NTOM,
		// Token: 0x04001603 RID: 5635
		MPG123_FEATURE_PARSE_ICY,
		// Token: 0x04001604 RID: 5636
		MPG123_FEATURE_TIMEOUT_READ
	}

	// Token: 0x0200042D RID: 1069
	public enum mpg123_errors
	{
		// Token: 0x04001606 RID: 5638
		MPG123_DONE = -12,
		// Token: 0x04001607 RID: 5639
		MPG123_NEW_FORMAT,
		// Token: 0x04001608 RID: 5640
		MPG123_NEED_MORE,
		// Token: 0x04001609 RID: 5641
		MPG123_ERR = -1,
		// Token: 0x0400160A RID: 5642
		MPG123_OK,
		// Token: 0x0400160B RID: 5643
		MPG123_BAD_OUTFORMAT,
		// Token: 0x0400160C RID: 5644
		MPG123_BAD_CHANNEL,
		// Token: 0x0400160D RID: 5645
		MPG123_BAD_RATE,
		// Token: 0x0400160E RID: 5646
		MPG123_ERR_16TO8TABLE,
		// Token: 0x0400160F RID: 5647
		MPG123_BAD_PARAM,
		// Token: 0x04001610 RID: 5648
		MPG123_BAD_BUFFER,
		// Token: 0x04001611 RID: 5649
		MPG123_OUT_OF_MEM,
		// Token: 0x04001612 RID: 5650
		MPG123_NOT_INITIALIZED,
		// Token: 0x04001613 RID: 5651
		MPG123_BAD_DECODER,
		// Token: 0x04001614 RID: 5652
		MPG123_BAD_HANDLE,
		// Token: 0x04001615 RID: 5653
		MPG123_NO_BUFFERS,
		// Token: 0x04001616 RID: 5654
		MPG123_BAD_RVA,
		// Token: 0x04001617 RID: 5655
		MPG123_NO_GAPLESS,
		// Token: 0x04001618 RID: 5656
		MPG123_NO_SPACE,
		// Token: 0x04001619 RID: 5657
		MPG123_BAD_TYPES,
		// Token: 0x0400161A RID: 5658
		MPG123_BAD_BAND,
		// Token: 0x0400161B RID: 5659
		MPG123_ERR_NULL,
		// Token: 0x0400161C RID: 5660
		MPG123_ERR_READER,
		// Token: 0x0400161D RID: 5661
		MPG123_NO_SEEK_FROM_END,
		// Token: 0x0400161E RID: 5662
		MPG123_BAD_WHENCE,
		// Token: 0x0400161F RID: 5663
		MPG123_NO_TIMEOUT,
		// Token: 0x04001620 RID: 5664
		MPG123_BAD_FILE,
		// Token: 0x04001621 RID: 5665
		MPG123_NO_SEEK,
		// Token: 0x04001622 RID: 5666
		MPG123_NO_READER,
		// Token: 0x04001623 RID: 5667
		MPG123_BAD_PARS,
		// Token: 0x04001624 RID: 5668
		MPG123_BAD_INDEX_PAR,
		// Token: 0x04001625 RID: 5669
		MPG123_OUT_OF_SYNC,
		// Token: 0x04001626 RID: 5670
		MPG123_RESYNC_FAIL,
		// Token: 0x04001627 RID: 5671
		MPG123_NO_8BIT,
		// Token: 0x04001628 RID: 5672
		MPG123_BAD_ALIGN,
		// Token: 0x04001629 RID: 5673
		MPG123_NULL_BUFFER,
		// Token: 0x0400162A RID: 5674
		MPG123_NO_RELSEEK,
		// Token: 0x0400162B RID: 5675
		MPG123_NULL_POINTER,
		// Token: 0x0400162C RID: 5676
		MPG123_BAD_KEY,
		// Token: 0x0400162D RID: 5677
		MPG123_NO_INDEX,
		// Token: 0x0400162E RID: 5678
		MPG123_INDEX_FAIL,
		// Token: 0x0400162F RID: 5679
		MPG123_BAD_DECODER_SETUP,
		// Token: 0x04001630 RID: 5680
		MPG123_MISSING_FEATURE,
		// Token: 0x04001631 RID: 5681
		MPG123_BAD_VALUE,
		// Token: 0x04001632 RID: 5682
		MPG123_LSEEK_FAILED,
		// Token: 0x04001633 RID: 5683
		MPG123_BAD_CUSTOM_IO,
		// Token: 0x04001634 RID: 5684
		MPG123_LFS_OVERFLOW
	}

	// Token: 0x0200042E RID: 1070
	public enum mpg123_enc_enum
	{
		// Token: 0x04001636 RID: 5686
		MPG123_ENC_8 = 15,
		// Token: 0x04001637 RID: 5687
		MPG123_ENC_16 = 64,
		// Token: 0x04001638 RID: 5688
		MPG123_ENC_32 = 256,
		// Token: 0x04001639 RID: 5689
		MPG123_ENC_SIGNED = 128,
		// Token: 0x0400163A RID: 5690
		MPG123_ENC_FLOAT = 3584,
		// Token: 0x0400163B RID: 5691
		MPG123_ENC_SIGNED_16 = 208,
		// Token: 0x0400163C RID: 5692
		MPG123_ENC_UNSIGNED_16 = 96,
		// Token: 0x0400163D RID: 5693
		MPG123_ENC_UNSIGNED_8 = 1,
		// Token: 0x0400163E RID: 5694
		MPG123_ENC_SIGNED_8 = 130,
		// Token: 0x0400163F RID: 5695
		MPG123_ENC_ULAW_8 = 4,
		// Token: 0x04001640 RID: 5696
		MPG123_ENC_ALAW_8 = 8,
		// Token: 0x04001641 RID: 5697
		MPG123_ENC_SIGNED_32 = 4480,
		// Token: 0x04001642 RID: 5698
		MPG123_ENC_UNSIGNED_32 = 8448,
		// Token: 0x04001643 RID: 5699
		MPG123_ENC_FLOAT_32 = 512,
		// Token: 0x04001644 RID: 5700
		MPG123_ENC_FLOAT_64 = 1024,
		// Token: 0x04001645 RID: 5701
		MPG123_ENC_ANY = 14335
	}

	// Token: 0x0200042F RID: 1071
	public enum mpg123_channelcount
	{
		// Token: 0x04001647 RID: 5703
		MPG123_MONO = 1,
		// Token: 0x04001648 RID: 5704
		MPG123_STEREO
	}

	// Token: 0x02000430 RID: 1072
	public enum mpg123_channels
	{
		// Token: 0x0400164A RID: 5706
		MPG123_LEFT = 1,
		// Token: 0x0400164B RID: 5707
		MPG123_RIGHT,
		// Token: 0x0400164C RID: 5708
		MPG123_LR
	}

	// Token: 0x02000431 RID: 1073
	public enum mpg123_vbr
	{
		// Token: 0x0400164E RID: 5710
		MPG123_CBR,
		// Token: 0x0400164F RID: 5711
		MPG123_VBR,
		// Token: 0x04001650 RID: 5712
		MPG123_ABR
	}

	// Token: 0x02000432 RID: 1074
	public enum mpg123_version
	{
		// Token: 0x04001652 RID: 5714
		MPG123_1_0,
		// Token: 0x04001653 RID: 5715
		MPG123_2_0,
		// Token: 0x04001654 RID: 5716
		MPG123_2_5
	}

	// Token: 0x02000433 RID: 1075
	public enum mpg123_mode
	{
		// Token: 0x04001656 RID: 5718
		MPG123_M_STEREO,
		// Token: 0x04001657 RID: 5719
		MPG123_M_JOINT,
		// Token: 0x04001658 RID: 5720
		MPG123_M_DUAL,
		// Token: 0x04001659 RID: 5721
		MPG123_M_MONO
	}

	// Token: 0x02000434 RID: 1076
	public enum mpg123_flags
	{
		// Token: 0x0400165B RID: 5723
		MPG123_CRC = 1,
		// Token: 0x0400165C RID: 5724
		MPG123_COPYRIGHT,
		// Token: 0x0400165D RID: 5725
		MPG123_PRIVATE = 4,
		// Token: 0x0400165E RID: 5726
		MPG123_ORIGINAL = 8
	}

	// Token: 0x02000435 RID: 1077
	public enum mpg123_state
	{
		// Token: 0x04001660 RID: 5728
		MPG123_ACCURATE = 1
	}

	// Token: 0x02000436 RID: 1078
	public enum mpg123_text_encoding
	{
		// Token: 0x04001662 RID: 5730
		mpg123_text_unknown,
		// Token: 0x04001663 RID: 5731
		mpg123_text_utf8,
		// Token: 0x04001664 RID: 5732
		mpg123_text_latin1,
		// Token: 0x04001665 RID: 5733
		mpg123_text_icy,
		// Token: 0x04001666 RID: 5734
		mpg123_text_cp1252,
		// Token: 0x04001667 RID: 5735
		mpg123_text_utf16,
		// Token: 0x04001668 RID: 5736
		mpg123_text_utf16bom,
		// Token: 0x04001669 RID: 5737
		mpg123_text_utf16be,
		// Token: 0x0400166A RID: 5738
		mpg123_text_max = 7
	}

	// Token: 0x02000437 RID: 1079
	public enum mpg123_id3_enc
	{
		// Token: 0x0400166C RID: 5740
		mpg123_id3_latin1,
		// Token: 0x0400166D RID: 5741
		mpg123_id3_utf16bom,
		// Token: 0x0400166E RID: 5742
		mpg123_id3_utf16be,
		// Token: 0x0400166F RID: 5743
		mpg123_id3_utf8,
		// Token: 0x04001670 RID: 5744
		mpg123_id3_enc_max = 3
	}

	// Token: 0x02000438 RID: 1080
	public struct mpg123_frameinfo
	{
		// Token: 0x04001671 RID: 5745
		private MPGImport.mpg123_version version;

		// Token: 0x04001672 RID: 5746
		private int layer;

		// Token: 0x04001673 RID: 5747
		private int rate;

		// Token: 0x04001674 RID: 5748
		private MPGImport.mpg123_mode mode;

		// Token: 0x04001675 RID: 5749
		private int mode_ext;

		// Token: 0x04001676 RID: 5750
		private int framesize;

		// Token: 0x04001677 RID: 5751
		private MPGImport.mpg123_flags flags;

		// Token: 0x04001678 RID: 5752
		private int emphasis;

		// Token: 0x04001679 RID: 5753
		private int bitrate;

		// Token: 0x0400167A RID: 5754
		private int abr_rate;

		// Token: 0x0400167B RID: 5755
		private MPGImport.mpg123_vbr vbr;
	}

	// Token: 0x02000439 RID: 1081
	public struct mpg123_string
	{
		// Token: 0x0400167C RID: 5756
		private string p;

		// Token: 0x0400167D RID: 5757
		private int size;

		// Token: 0x0400167E RID: 5758
		private int fill;
	}

	// Token: 0x0200043A RID: 1082
	public struct mpg123_text
	{
		// Token: 0x0400167F RID: 5759
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		private char[] lang;

		// Token: 0x04001680 RID: 5760
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private char[] id;

		// Token: 0x04001681 RID: 5761
		private MPGImport.mpg123_string description;

		// Token: 0x04001682 RID: 5762
		private MPGImport.mpg123_string text;
	}

	// Token: 0x0200043B RID: 1083
	public struct mpg123_id3v2
	{
		// Token: 0x04001683 RID: 5763
		private byte version;

		// Token: 0x04001684 RID: 5764
		private IntPtr title;

		// Token: 0x04001685 RID: 5765
		private IntPtr artist;

		// Token: 0x04001686 RID: 5766
		private IntPtr album;

		// Token: 0x04001687 RID: 5767
		private IntPtr year;

		// Token: 0x04001688 RID: 5768
		private IntPtr genre;

		// Token: 0x04001689 RID: 5769
		private IntPtr comment;

		// Token: 0x0400168A RID: 5770
		private IntPtr comment_list;

		// Token: 0x0400168B RID: 5771
		private int comments;

		// Token: 0x0400168C RID: 5772
		private IntPtr text;

		// Token: 0x0400168D RID: 5773
		private int texts;

		// Token: 0x0400168E RID: 5774
		private IntPtr extra;

		// Token: 0x0400168F RID: 5775
		private int extras;
	}

	// Token: 0x0200043C RID: 1084
	public struct mpg123_id3v1
	{
		// Token: 0x04001690 RID: 5776
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public char[] tag;

		// Token: 0x04001691 RID: 5777
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
		public char[] title;

		// Token: 0x04001692 RID: 5778
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
		public char[] artist;

		// Token: 0x04001693 RID: 5779
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
		public char[] album;

		// Token: 0x04001694 RID: 5780
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public char[] year;

		// Token: 0x04001695 RID: 5781
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
		public char[] comment;

		// Token: 0x04001696 RID: 5782
		public byte genre;
	}
}
