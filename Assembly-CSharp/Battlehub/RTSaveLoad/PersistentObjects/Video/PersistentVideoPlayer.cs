using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Video;

namespace Battlehub.RTSaveLoad.PersistentObjects.Video
{
	// Token: 0x020001D5 RID: 469
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentVideoPlayer : PersistentBehaviour
	{
		// Token: 0x06000979 RID: 2425 RVA: 0x0003A864 File Offset: 0x00038C64
		public PersistentVideoPlayer()
		{
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0003A86C File Offset: 0x00038C6C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			VideoPlayer videoPlayer = (VideoPlayer)obj;
			videoPlayer.source = (VideoSource)this.source;
			videoPlayer.url = this.url;
			videoPlayer.clip = (VideoClip)objects.Get(this.clip);
			videoPlayer.renderMode = (VideoRenderMode)this.renderMode;
			videoPlayer.targetCamera = (Camera)objects.Get(this.targetCamera);
			videoPlayer.targetTexture = (RenderTexture)objects.Get(this.targetTexture);
			videoPlayer.targetMaterialRenderer = (Renderer)objects.Get(this.targetMaterialRenderer);
			videoPlayer.targetMaterialProperty = this.targetMaterialProperty;
			videoPlayer.aspectRatio = (VideoAspectRatio)this.aspectRatio;
			videoPlayer.targetCameraAlpha = this.targetCameraAlpha;
			videoPlayer.waitForFirstFrame = this.waitForFirstFrame;
			videoPlayer.playOnAwake = this.playOnAwake;
			videoPlayer.time = this.time;
			videoPlayer.frame = this.frame;
			videoPlayer.playbackSpeed = this.playbackSpeed;
			videoPlayer.isLooping = this.isLooping;
			videoPlayer.timeSource = (VideoTimeSource)this.timeSource;
			videoPlayer.skipOnDrop = this.skipOnDrop;
			videoPlayer.controlledAudioTrackCount = this.controlledAudioTrackCount;
			videoPlayer.audioOutputMode = (VideoAudioOutputMode)this.audioOutputMode;
			videoPlayer.sendFrameReadyEvents = this.sendFrameReadyEvents;
			return videoPlayer;
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0003A9BC File Offset: 0x00038DBC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			VideoPlayer videoPlayer = (VideoPlayer)obj;
			this.source = (uint)videoPlayer.source;
			this.url = videoPlayer.url;
			this.clip = videoPlayer.clip.GetMappedInstanceID();
			this.renderMode = (uint)videoPlayer.renderMode;
			this.targetCamera = videoPlayer.targetCamera.GetMappedInstanceID();
			this.targetTexture = videoPlayer.targetTexture.GetMappedInstanceID();
			this.targetMaterialRenderer = videoPlayer.targetMaterialRenderer.GetMappedInstanceID();
			this.targetMaterialProperty = videoPlayer.targetMaterialProperty;
			this.aspectRatio = (uint)videoPlayer.aspectRatio;
			this.targetCameraAlpha = videoPlayer.targetCameraAlpha;
			this.waitForFirstFrame = videoPlayer.waitForFirstFrame;
			this.playOnAwake = videoPlayer.playOnAwake;
			this.time = videoPlayer.time;
			this.frame = videoPlayer.frame;
			this.playbackSpeed = videoPlayer.playbackSpeed;
			this.isLooping = videoPlayer.isLooping;
			this.timeSource = (uint)videoPlayer.timeSource;
			this.skipOnDrop = videoPlayer.skipOnDrop;
			this.controlledAudioTrackCount = videoPlayer.controlledAudioTrackCount;
			this.audioOutputMode = (uint)videoPlayer.audioOutputMode;
			this.sendFrameReadyEvents = videoPlayer.sendFrameReadyEvents;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0003AAF0 File Offset: 0x00038EF0
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.clip, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.targetCamera, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.targetTexture, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.targetMaterialRenderer, dependencies, objects, allowNulls);
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0003AB44 File Offset: 0x00038F44
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			VideoPlayer videoPlayer = (VideoPlayer)obj;
			base.AddDependency(videoPlayer.clip, dependencies);
			base.AddDependency(videoPlayer.targetCamera, dependencies);
			base.AddDependency(videoPlayer.targetTexture, dependencies);
			base.AddDependency(videoPlayer.targetMaterialRenderer, dependencies);
		}

		// Token: 0x04000AA3 RID: 2723
		public uint source;

		// Token: 0x04000AA4 RID: 2724
		public string url;

		// Token: 0x04000AA5 RID: 2725
		public long clip;

		// Token: 0x04000AA6 RID: 2726
		public uint renderMode;

		// Token: 0x04000AA7 RID: 2727
		public long targetCamera;

		// Token: 0x04000AA8 RID: 2728
		public long targetTexture;

		// Token: 0x04000AA9 RID: 2729
		public long targetMaterialRenderer;

		// Token: 0x04000AAA RID: 2730
		public string targetMaterialProperty;

		// Token: 0x04000AAB RID: 2731
		public uint aspectRatio;

		// Token: 0x04000AAC RID: 2732
		public float targetCameraAlpha;

		// Token: 0x04000AAD RID: 2733
		public bool waitForFirstFrame;

		// Token: 0x04000AAE RID: 2734
		public bool playOnAwake;

		// Token: 0x04000AAF RID: 2735
		public double time;

		// Token: 0x04000AB0 RID: 2736
		public long frame;

		// Token: 0x04000AB1 RID: 2737
		public float playbackSpeed;

		// Token: 0x04000AB2 RID: 2738
		public bool isLooping;

		// Token: 0x04000AB3 RID: 2739
		public uint timeSource;

		// Token: 0x04000AB4 RID: 2740
		public bool skipOnDrop;

		// Token: 0x04000AB5 RID: 2741
		public ushort controlledAudioTrackCount;

		// Token: 0x04000AB6 RID: 2742
		public uint audioOutputMode;

		// Token: 0x04000AB7 RID: 2743
		public bool sendFrameReadyEvents;
	}
}
