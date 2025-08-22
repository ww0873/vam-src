using System;
using Leap.Unity.Attributes;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x0200073C RID: 1852
	public abstract class PostProcessProvider : LeapProvider
	{
		// Token: 0x06002D3F RID: 11583 RVA: 0x000D3434 File Offset: 0x000D1834
		protected PostProcessProvider()
		{
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06002D40 RID: 11584 RVA: 0x000D3452 File Offset: 0x000D1852
		// (set) Token: 0x06002D41 RID: 11585 RVA: 0x000D345C File Offset: 0x000D185C
		public LeapProvider inputLeapProvider
		{
			get
			{
				return this._inputLeapProvider;
			}
			set
			{
				if (Application.isPlaying && this._inputLeapProvider != null)
				{
					this._inputLeapProvider.OnFixedFrame -= this.processFixedFrame;
					this._inputLeapProvider.OnUpdateFrame -= this.processUpdateFrame;
				}
				this._inputLeapProvider = value;
				this.validateInput();
				if (Application.isPlaying && this._inputLeapProvider != null)
				{
					this._inputLeapProvider.OnFixedFrame -= this.processFixedFrame;
					this._inputLeapProvider.OnFixedFrame += this.processFixedFrame;
					this._inputLeapProvider.OnUpdateFrame -= this.processUpdateFrame;
					this._inputLeapProvider.OnUpdateFrame += this.processUpdateFrame;
				}
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06002D42 RID: 11586 RVA: 0x000D3536 File Offset: 0x000D1936
		public override Frame CurrentFrame
		{
			get
			{
				return this._cachedUpdateFrame;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06002D43 RID: 11587 RVA: 0x000D353E File Offset: 0x000D193E
		public override Frame CurrentFixedFrame
		{
			get
			{
				return this._cachedFixedFrame;
			}
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000D3546 File Offset: 0x000D1946
		protected virtual void OnEnable()
		{
			this.inputLeapProvider = this._inputLeapProvider;
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000D3554 File Offset: 0x000D1954
		protected virtual void OnValidate()
		{
			this.validateInput();
		}

		// Token: 0x06002D46 RID: 11590
		public abstract void ProcessFrame(ref Frame inputFrame);

		// Token: 0x06002D47 RID: 11591 RVA: 0x000D355C File Offset: 0x000D195C
		private void validateInput()
		{
			if (this.detectCycle())
			{
				this._inputLeapProvider = null;
				Debug.LogError("The input to the post-process provider on " + base.gameObject.name + " causes an infinite cycle, so its input has been set to null.");
			}
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000D3590 File Offset: 0x000D1990
		private bool detectCycle()
		{
			LeapProvider inputLeapProvider = this._inputLeapProvider;
			LeapProvider inputLeapProvider2 = this._inputLeapProvider;
			while (inputLeapProvider is PostProcessProvider)
			{
				inputLeapProvider2 = (inputLeapProvider2 as PostProcessProvider).inputLeapProvider;
				if (inputLeapProvider == inputLeapProvider2)
				{
					return true;
				}
				if (!(inputLeapProvider2 is PostProcessProvider))
				{
					return false;
				}
				inputLeapProvider = (inputLeapProvider as PostProcessProvider).inputLeapProvider;
				inputLeapProvider2 = (inputLeapProvider2 as PostProcessProvider).inputLeapProvider;
				if (!(inputLeapProvider2 is PostProcessProvider))
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x000D3608 File Offset: 0x000D1A08
		private void processUpdateFrame(Frame inputFrame)
		{
			if (this.dataUpdateMode == PostProcessProvider.DataUpdateMode.FixedUpdateOnly)
			{
				return;
			}
			this._cachedUpdateFrame.CopyFrom(inputFrame);
			if (!this.passthroughOnly)
			{
				this.ProcessFrame(ref this._cachedUpdateFrame);
			}
			base.DispatchUpdateFrameEvent(this._cachedUpdateFrame);
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x000D3647 File Offset: 0x000D1A47
		private void processFixedFrame(Frame inputFrame)
		{
			if (this.dataUpdateMode == PostProcessProvider.DataUpdateMode.UpdateOnly)
			{
				return;
			}
			this._cachedFixedFrame.CopyFrom(inputFrame);
			if (!this.passthroughOnly)
			{
				this.ProcessFrame(ref this._cachedFixedFrame);
			}
			base.DispatchFixedFrameEvent(this._cachedFixedFrame);
		}

		// Token: 0x040023C3 RID: 9155
		[Tooltip("The LeapProvider whose output hand data will be copied, modified, and output by this post-processing provider.")]
		[SerializeField]
		[OnEditorChange("inputLeapProvider")]
		protected LeapProvider _inputLeapProvider;

		// Token: 0x040023C4 RID: 9156
		[Tooltip("Whether this post-processing provider should process data received from Update frames, FixedUpdate frames, or both. Processing both kinds of frames is only recommended if your post-process is stateless.")]
		public PostProcessProvider.DataUpdateMode dataUpdateMode;

		// Token: 0x040023C5 RID: 9157
		[Tooltip("When this setting is enabled, frame data is passed from this provider's input directly to its output without performing any post-processing.")]
		public bool passthroughOnly;

		// Token: 0x040023C6 RID: 9158
		private Frame _cachedUpdateFrame = new Frame();

		// Token: 0x040023C7 RID: 9159
		private Frame _cachedFixedFrame = new Frame();

		// Token: 0x0200073D RID: 1853
		public enum DataUpdateMode
		{
			// Token: 0x040023C9 RID: 9161
			UpdateOnly,
			// Token: 0x040023CA RID: 9162
			FixedUpdateOnly,
			// Token: 0x040023CB RID: 9163
			UpdateAndFixedUpdate
		}
	}
}
