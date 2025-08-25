using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace PrefabEvolution
{
	// Token: 0x02000411 RID: 1041
	[SelectionBase]
	[AddComponentMenu("")]
	public class PEPrefabScript : MonoBehaviour, ISerializationCallbackReceiver
	{
		// Token: 0x06001A4F RID: 6735 RVA: 0x00091F08 File Offset: 0x00090308
		public PEPrefabScript()
		{
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06001A50 RID: 6736 RVA: 0x00091F26 File Offset: 0x00090326
		// (set) Token: 0x06001A51 RID: 6737 RVA: 0x00091F38 File Offset: 0x00090338
		public GameObject ParentPrefab
		{
			get
			{
				return PEPrefabScript.EditorBridge.GetAssetByGuid(this.ParentPrefabGUID);
			}
			set
			{
				string text = PEPrefabScript.EditorBridge.GetAssetGuid(value);
				if (!string.IsNullOrEmpty(text))
				{
					this.ParentPrefabGUID = text;
				}
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001A52 RID: 6738 RVA: 0x00091F63 File Offset: 0x00090363
		// (set) Token: 0x06001A53 RID: 6739 RVA: 0x00091F78 File Offset: 0x00090378
		public GameObject Prefab
		{
			get
			{
				return PEPrefabScript.EditorBridge.GetAssetByGuid(this.PrefabGUID);
			}
			set
			{
				string text = PEPrefabScript.EditorBridge.GetAssetGuid(value);
				if (!string.IsNullOrEmpty(text))
				{
					this.PrefabGUID = text;
				}
			}
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x00091FA3 File Offset: 0x000903A3
		private void OnValidate()
		{
			if (PEPrefabScript.EditorBridge.OnValidate != null)
			{
				PEPrefabScript.EditorBridge.OnValidate(this);
			}
		}

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06001A55 RID: 6741 RVA: 0x00091FBC File Offset: 0x000903BC
		// (remove) Token: 0x06001A56 RID: 6742 RVA: 0x00091FF4 File Offset: 0x000903F4
		public event Action OnBuildModifications
		{
			add
			{
				Action action = this.OnBuildModifications;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.OnBuildModifications, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = this.OnBuildModifications;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.OnBuildModifications, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x0009202A File Offset: 0x0009042A
		public void InvokeOnBuildModifications()
		{
			if (this.OnBuildModifications != null)
			{
				this.OnBuildModifications();
			}
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x00092042 File Offset: 0x00090442
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00092044 File Offset: 0x00090444
		public void OnAfterDeserialize()
		{
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x00092046 File Offset: 0x00090446
		private void ClearInternalData()
		{
			this.Properties = null;
			this.Links = null;
			this.Modifications = null;
			this.ParentPrefabGUID = null;
			this.PrefabGUID = "STRIPPED";
		}

		// Token: 0x0400154C RID: 5452
		[HideInInspector]
		public PEExposedProperties Properties = Utils.Create<PEExposedProperties>();

		// Token: 0x0400154D RID: 5453
		[HideInInspector]
		public PELinkage Links = Utils.Create<PELinkage>();

		// Token: 0x0400154E RID: 5454
		[HideInInspector]
		public PEModifications Modifications;

		// Token: 0x0400154F RID: 5455
		public string ParentPrefabGUID;

		// Token: 0x04001550 RID: 5456
		public string PrefabGUID;

		// Token: 0x04001551 RID: 5457
		private PEPrefabScript.PrefabInternalData _prefabInternalData;

		// Token: 0x04001552 RID: 5458
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBuildModifications;

		// Token: 0x02000412 RID: 1042
		public static class EditorBridge
		{
			// Token: 0x04001553 RID: 5459
			public static Action<PEPrefabScript> OnValidate;

			// Token: 0x04001554 RID: 5460
			public static Func<GameObject, string> GetAssetGuid;

			// Token: 0x04001555 RID: 5461
			public static Func<string, GameObject> GetAssetByGuid;
		}

		// Token: 0x02000413 RID: 1043
		private class PrefabInternalData
		{
			// Token: 0x06001A5B RID: 6747 RVA: 0x00092070 File Offset: 0x00090470
			public PrefabInternalData(PEPrefabScript script)
			{
				this.Properties = script.Properties;
				this.Links = script.Links;
				this.Modifications = script.Modifications;
				this.ParentPrefabGUID = script.ParentPrefabGUID;
				this.PrefabGUID = script.PrefabGUID;
			}

			// Token: 0x06001A5C RID: 6748 RVA: 0x000920BF File Offset: 0x000904BF
			public void Fill(PEPrefabScript script)
			{
				script.Properties = this.Properties;
				script.Links = this.Links;
				script.Modifications = this.Modifications;
				script.ParentPrefabGUID = this.ParentPrefabGUID;
				script.PrefabGUID = this.PrefabGUID;
			}

			// Token: 0x04001556 RID: 5462
			private readonly PEExposedProperties Properties;

			// Token: 0x04001557 RID: 5463
			private readonly PELinkage Links;

			// Token: 0x04001558 RID: 5464
			private readonly PEModifications Modifications;

			// Token: 0x04001559 RID: 5465
			private readonly string ParentPrefabGUID;

			// Token: 0x0400155A RID: 5466
			private readonly string PrefabGUID;
		}
	}
}
