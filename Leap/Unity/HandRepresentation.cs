using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap.Unity
{
	// Token: 0x020006E0 RID: 1760
	public class HandRepresentation
	{
		// Token: 0x06002A68 RID: 10856 RVA: 0x000E5693 File Offset: 0x000E3A93
		public HandRepresentation(HandModelManager parent, Hand hand, Chirality repChirality, ModelType repType)
		{
			this.parent = parent;
			this.HandID = hand.Id;
			this.RepChirality = repChirality;
			this.RepType = repType;
			this.MostRecentHand = hand;
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06002A69 RID: 10857 RVA: 0x000E56C4 File Offset: 0x000E3AC4
		// (set) Token: 0x06002A6A RID: 10858 RVA: 0x000E56CC File Offset: 0x000E3ACC
		public int HandID
		{
			[CompilerGenerated]
			get
			{
				return this.<HandID>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<HandID>k__BackingField = value;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06002A6B RID: 10859 RVA: 0x000E56D5 File Offset: 0x000E3AD5
		// (set) Token: 0x06002A6C RID: 10860 RVA: 0x000E56DD File Offset: 0x000E3ADD
		public int LastUpdatedTime
		{
			[CompilerGenerated]
			get
			{
				return this.<LastUpdatedTime>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LastUpdatedTime>k__BackingField = value;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06002A6D RID: 10861 RVA: 0x000E56E6 File Offset: 0x000E3AE6
		// (set) Token: 0x06002A6E RID: 10862 RVA: 0x000E56EE File Offset: 0x000E3AEE
		public bool IsMarked
		{
			[CompilerGenerated]
			get
			{
				return this.<IsMarked>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsMarked>k__BackingField = value;
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06002A6F RID: 10863 RVA: 0x000E56F7 File Offset: 0x000E3AF7
		// (set) Token: 0x06002A70 RID: 10864 RVA: 0x000E56FF File Offset: 0x000E3AFF
		public Chirality RepChirality
		{
			[CompilerGenerated]
			get
			{
				return this.<RepChirality>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<RepChirality>k__BackingField = value;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06002A71 RID: 10865 RVA: 0x000E5708 File Offset: 0x000E3B08
		// (set) Token: 0x06002A72 RID: 10866 RVA: 0x000E5710 File Offset: 0x000E3B10
		public ModelType RepType
		{
			[CompilerGenerated]
			get
			{
				return this.<RepType>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<RepType>k__BackingField = value;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06002A73 RID: 10867 RVA: 0x000E5719 File Offset: 0x000E3B19
		// (set) Token: 0x06002A74 RID: 10868 RVA: 0x000E5721 File Offset: 0x000E3B21
		public Hand MostRecentHand
		{
			[CompilerGenerated]
			get
			{
				return this.<MostRecentHand>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<MostRecentHand>k__BackingField = value;
			}
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x000E572C File Offset: 0x000E3B2C
		public void Finish()
		{
			if (this.handModels != null)
			{
				for (int i = 0; i < this.handModels.Count; i++)
				{
					this.handModels[i].FinishHand();
					this.parent.ReturnToPool(this.handModels[i]);
					this.handModels[i] = null;
				}
			}
			this.parent.RemoveHandRepresentation(this);
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x000E57A4 File Offset: 0x000E3BA4
		public void AddModel(HandModelBase model)
		{
			if (this.handModels == null)
			{
				this.handModels = new List<HandModelBase>();
			}
			this.handModels.Add(model);
			if (model.GetLeapHand() == null)
			{
				model.SetLeapHand(this.MostRecentHand);
				model.InitHand();
				model.BeginHand();
				model.UpdateHand();
			}
			else
			{
				model.SetLeapHand(this.MostRecentHand);
				model.BeginHand();
			}
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x000E5813 File Offset: 0x000E3C13
		public void RemoveModel(HandModelBase model)
		{
			if (this.handModels != null)
			{
				model.FinishHand();
				this.handModels.Remove(model);
			}
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x000E5834 File Offset: 0x000E3C34
		public void UpdateRepresentation(Hand hand)
		{
			this.MostRecentHand = hand;
			if (this.handModels != null)
			{
				for (int i = 0; i < this.handModels.Count; i++)
				{
					this.handModels[i].SetLeapHand(hand);
					this.handModels[i].UpdateHand();
				}
			}
		}

		// Token: 0x04002287 RID: 8839
		private HandModelManager parent;

		// Token: 0x04002288 RID: 8840
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <HandID>k__BackingField;

		// Token: 0x04002289 RID: 8841
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <LastUpdatedTime>k__BackingField;

		// Token: 0x0400228A RID: 8842
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <IsMarked>k__BackingField;

		// Token: 0x0400228B RID: 8843
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Chirality <RepChirality>k__BackingField;

		// Token: 0x0400228C RID: 8844
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ModelType <RepType>k__BackingField;

		// Token: 0x0400228D RID: 8845
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Hand <MostRecentHand>k__BackingField;

		// Token: 0x0400228E RID: 8846
		public List<HandModelBase> handModels;
	}
}
