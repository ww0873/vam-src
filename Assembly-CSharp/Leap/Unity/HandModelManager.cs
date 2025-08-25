using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Leap.Unity.Attributes;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006DE RID: 1758
	public class HandModelManager : MonoBehaviour
	{
		// Token: 0x06002A47 RID: 10823 RVA: 0x000E465C File Offset: 0x000E2A5C
		public HandModelManager()
		{
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x000E46BF File Offset: 0x000E2ABF
		// (set) Token: 0x06002A49 RID: 10825 RVA: 0x000E46C7 File Offset: 0x000E2AC7
		public bool GraphicsEnabled
		{
			get
			{
				return this.graphicsEnabled;
			}
			set
			{
				this.graphicsEnabled = value;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06002A4A RID: 10826 RVA: 0x000E46D0 File Offset: 0x000E2AD0
		// (set) Token: 0x06002A4B RID: 10827 RVA: 0x000E46D8 File Offset: 0x000E2AD8
		public bool PhysicsEnabled
		{
			get
			{
				return this.physicsEnabled;
			}
			set
			{
				this.physicsEnabled = value;
			}
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000E46E1 File Offset: 0x000E2AE1
		protected virtual void OnUpdateFrame(Frame frame)
		{
			if (frame != null && this.graphicsEnabled)
			{
				this.UpdateHandRepresentations(this.graphicsHandReps, ModelType.Graphics, frame);
			}
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x000E4702 File Offset: 0x000E2B02
		protected virtual void OnFixedFrame(Frame frame)
		{
			if (frame != null && this.physicsEnabled)
			{
				this.UpdateHandRepresentations(this.physicsHandReps, ModelType.Physics, frame);
			}
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x000E4724 File Offset: 0x000E2B24
		protected virtual void UpdateHandRepresentations(Dictionary<int, HandRepresentation> all_hand_reps, ModelType modelType, Frame frame)
		{
			for (int i = 0; i < frame.Hands.Count; i++)
			{
				Hand hand = frame.Hands[i];
				HandRepresentation handRepresentation;
				if (!all_hand_reps.TryGetValue(hand.Id, out handRepresentation))
				{
					handRepresentation = this.MakeHandRepresentation(hand, modelType);
					all_hand_reps.Add(hand.Id, handRepresentation);
				}
				if (handRepresentation != null)
				{
					handRepresentation.IsMarked = true;
					handRepresentation.UpdateRepresentation(hand);
					handRepresentation.LastUpdatedTime = (int)frame.Timestamp;
				}
			}
			HandRepresentation handRepresentation2 = null;
			foreach (KeyValuePair<int, HandRepresentation> keyValuePair in all_hand_reps)
			{
				if (keyValuePair.Value != null)
				{
					if (keyValuePair.Value.IsMarked)
					{
						keyValuePair.Value.IsMarked = false;
					}
					else
					{
						handRepresentation2 = keyValuePair.Value;
					}
				}
			}
			if (handRepresentation2 != null)
			{
				all_hand_reps.Remove(handRepresentation2.HandID);
				handRepresentation2.Finish();
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06002A4F RID: 10831 RVA: 0x000E4818 File Offset: 0x000E2C18
		// (set) Token: 0x06002A50 RID: 10832 RVA: 0x000E4820 File Offset: 0x000E2C20
		public LeapProvider leapProvider
		{
			get
			{
				return this._leapProvider;
			}
			set
			{
				if (this._leapProvider != null)
				{
					this._leapProvider.OnFixedFrame -= this.OnFixedFrame;
					this._leapProvider.OnUpdateFrame -= this.OnUpdateFrame;
				}
				this._leapProvider = value;
				if (this._leapProvider != null)
				{
					this._leapProvider.OnFixedFrame += this.OnFixedFrame;
					this._leapProvider.OnUpdateFrame += this.OnUpdateFrame;
				}
			}
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x000E48B8 File Offset: 0x000E2CB8
		public void ReturnToPool(HandModelBase model)
		{
			HandModelManager.ModelGroup modelGroup;
			bool flag = this.modelGroupMapping.TryGetValue(model, out modelGroup);
			for (int i = 0; i < this.activeHandReps.Count; i++)
			{
				HandRepresentation handRepresentation = this.activeHandReps[i];
				if (handRepresentation.RepChirality == model.Handedness && handRepresentation.RepType == model.HandModelType)
				{
					bool flag2 = false;
					if (handRepresentation.handModels != null)
					{
						for (int j = 0; j < modelGroup.modelsCheckedOut.Count; j++)
						{
							HandModelBase y = modelGroup.modelsCheckedOut[j];
							for (int k = 0; k < handRepresentation.handModels.Count; k++)
							{
								if (handRepresentation.handModels[k] == y)
								{
									flag2 = true;
								}
							}
						}
					}
					if (!flag2)
					{
						handRepresentation.AddModel(model);
						this.modelToHandRepMapping[model] = handRepresentation;
						return;
					}
				}
			}
			modelGroup.ReturnToGroup(model);
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x000E49BC File Offset: 0x000E2DBC
		public HandRepresentation MakeHandRepresentation(Hand hand, ModelType modelType)
		{
			Chirality chirality = (!hand.IsRight) ? Chirality.Left : Chirality.Right;
			HandRepresentation handRepresentation = new HandRepresentation(this, hand, chirality, modelType);
			for (int i = 0; i < this.ModelPool.Count; i++)
			{
				HandModelManager.ModelGroup modelGroup = this.ModelPool[i];
				if (modelGroup.IsEnabled)
				{
					HandModelBase handModelBase = modelGroup.TryGetModel(chirality, modelType);
					if (handModelBase != null)
					{
						handRepresentation.AddModel(handModelBase);
						if (!this.modelToHandRepMapping.ContainsKey(handModelBase))
						{
							handModelBase.group = modelGroup;
							this.modelToHandRepMapping.Add(handModelBase, handRepresentation);
						}
					}
				}
			}
			this.activeHandReps.Add(handRepresentation);
			return handRepresentation;
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x000E4A6E File Offset: 0x000E2E6E
		public void RemoveHandRepresentation(HandRepresentation handRepresentation)
		{
			this.activeHandReps.Remove(handRepresentation);
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x000E4A80 File Offset: 0x000E2E80
		protected virtual void OnEnable()
		{
			if (this._leapProvider == null)
			{
				this._leapProvider = Hands.Provider;
			}
			this._leapProvider.OnUpdateFrame -= this.OnUpdateFrame;
			this._leapProvider.OnUpdateFrame += this.OnUpdateFrame;
			this._leapProvider.OnFixedFrame -= this.OnFixedFrame;
			this._leapProvider.OnFixedFrame += this.OnFixedFrame;
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x000E4B09 File Offset: 0x000E2F09
		protected virtual void OnDisable()
		{
			this._leapProvider.OnUpdateFrame -= this.OnUpdateFrame;
			this._leapProvider.OnFixedFrame -= this.OnFixedFrame;
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x000E4B3C File Offset: 0x000E2F3C
		private void Start()
		{
			for (int i = 0; i < this.ModelPool.Count; i++)
			{
				this.InitializeModelGroup(this.ModelPool[i]);
			}
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x000E4B78 File Offset: 0x000E2F78
		private void InitializeModelGroup(HandModelManager.ModelGroup collectionGroup)
		{
			if (this.modelGroupMapping.ContainsValue(collectionGroup))
			{
				return;
			}
			collectionGroup._handModelManager = this;
			HandModelBase handModelBase;
			if (collectionGroup.IsLeftToBeSpawned)
			{
				HandModelBase leftModel = collectionGroup.LeftModel;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(leftModel.gameObject);
				handModelBase = gameObject.GetComponent<HandModelBase>();
				handModelBase.transform.parent = base.transform;
			}
			else
			{
				handModelBase = collectionGroup.LeftModel;
			}
			if (handModelBase != null)
			{
				collectionGroup.modelList.Add(handModelBase);
				this.modelGroupMapping.Add(handModelBase, collectionGroup);
			}
			HandModelBase handModelBase2;
			if (collectionGroup.IsRightToBeSpawned)
			{
				HandModelBase rightModel = collectionGroup.RightModel;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(rightModel.gameObject);
				handModelBase2 = gameObject2.GetComponent<HandModelBase>();
				handModelBase2.transform.parent = base.transform;
			}
			else
			{
				handModelBase2 = collectionGroup.RightModel;
			}
			if (handModelBase2 != null)
			{
				collectionGroup.modelList.Add(handModelBase2);
				this.modelGroupMapping.Add(handModelBase2, collectionGroup);
			}
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x000E4C70 File Offset: 0x000E3070
		public void EnableGroup(string groupName)
		{
			base.StartCoroutine(this.enableGroup(groupName));
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x000E4C80 File Offset: 0x000E3080
		private IEnumerator enableGroup(string groupName)
		{
			yield return new WaitForEndOfFrame();
			HandModelManager.ModelGroup group = null;
			for (int i = 0; i < this.ModelPool.Count; i++)
			{
				if (this.ModelPool[i].GroupName == groupName)
				{
					group = this.ModelPool[i];
					for (int j = 0; j < this.activeHandReps.Count; j++)
					{
						HandRepresentation handRepresentation = this.activeHandReps[j];
						HandModelBase handModelBase = group.TryGetModel(handRepresentation.RepChirality, handRepresentation.RepType);
						if (handModelBase != null)
						{
							handRepresentation.AddModel(handModelBase);
							this.modelToHandRepMapping.Add(handModelBase, handRepresentation);
						}
					}
					group.IsEnabled = true;
				}
			}
			if (group == null)
			{
				UnityEngine.Debug.LogWarning("A group matching that name does not exisit in the modelPool");
			}
			yield break;
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x000E4CA2 File Offset: 0x000E30A2
		public void DisableGroup(string groupName)
		{
			base.StartCoroutine(this.disableGroup(groupName));
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x000E4CB4 File Offset: 0x000E30B4
		private IEnumerator disableGroup(string groupName)
		{
			yield return new WaitForEndOfFrame();
			HandModelManager.ModelGroup group = null;
			for (int i = 0; i < this.ModelPool.Count; i++)
			{
				if (this.ModelPool[i].GroupName == groupName)
				{
					group = this.ModelPool[i];
					for (int j = 0; j < group.modelsCheckedOut.Count; j++)
					{
						HandModelBase handModelBase = group.modelsCheckedOut[j];
						HandRepresentation handRepresentation;
						if (this.modelToHandRepMapping.TryGetValue(handModelBase, out handRepresentation))
						{
							handRepresentation.RemoveModel(handModelBase);
							group.ReturnToGroup(handModelBase);
							j--;
						}
					}
					group.IsEnabled = false;
				}
			}
			if (group == null)
			{
				UnityEngine.Debug.LogWarning("A group matching that name does not exisit in the modelPool");
			}
			yield break;
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x000E4CD6 File Offset: 0x000E30D6
		public void ToggleGroup(string groupName)
		{
			base.StartCoroutine(this.toggleGroup(groupName));
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x000E4CE8 File Offset: 0x000E30E8
		private IEnumerator toggleGroup(string groupName)
		{
			HandModelManager.<toggleGroup>c__Iterator2.<toggleGroup>c__AnonStorey5 <toggleGroup>c__AnonStorey = new HandModelManager.<toggleGroup>c__Iterator2.<toggleGroup>c__AnonStorey5();
			<toggleGroup>c__AnonStorey.<>f__ref$2 = this;
			<toggleGroup>c__AnonStorey.groupName = groupName;
			yield return new WaitForEndOfFrame();
			HandModelManager.ModelGroup modelGroup = this.ModelPool.Find(new Predicate<HandModelManager.ModelGroup>(<toggleGroup>c__AnonStorey.<>m__0));
			if (modelGroup != null)
			{
				if (modelGroup.IsEnabled)
				{
					this.DisableGroup(<toggleGroup>c__AnonStorey.groupName);
					modelGroup.IsEnabled = false;
				}
				else
				{
					this.EnableGroup(<toggleGroup>c__AnonStorey.groupName);
					modelGroup.IsEnabled = true;
				}
			}
			else
			{
				UnityEngine.Debug.LogWarning("A group matching that name does not exisit in the modelPool");
			}
			yield break;
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000E4D0C File Offset: 0x000E310C
		public void AddNewGroup(string groupName, HandModelBase leftModel, HandModelBase rightModel)
		{
			HandModelManager.ModelGroup modelGroup = new HandModelManager.ModelGroup();
			modelGroup.LeftModel = leftModel;
			modelGroup.RightModel = rightModel;
			modelGroup.GroupName = groupName;
			modelGroup.CanDuplicate = false;
			modelGroup.IsEnabled = true;
			this.ModelPool.Add(modelGroup);
			this.InitializeModelGroup(modelGroup);
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x000E4D58 File Offset: 0x000E3158
		private IEnumerator addNewGroupWait(string groupName, HandModelBase leftModel, HandModelBase rightModel)
		{
			yield return new WaitForEndOfFrame();
			this.AddNewGroup(groupName, leftModel, rightModel);
			yield break;
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000E4D88 File Offset: 0x000E3188
		public void AddNewGroupWait(string groupName, HandModelBase leftModel, HandModelBase rightModel)
		{
			base.StartCoroutine(this.addNewGroupWait(groupName, leftModel, rightModel));
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000E4D9C File Offset: 0x000E319C
		public void RemoveGroup(string groupName)
		{
			HandModelManager.<RemoveGroup>c__AnonStorey6 <RemoveGroup>c__AnonStorey = new HandModelManager.<RemoveGroup>c__AnonStorey6();
			<RemoveGroup>c__AnonStorey.groupName = groupName;
			while (this.ModelPool.Find(new Predicate<HandModelManager.ModelGroup>(<RemoveGroup>c__AnonStorey.<>m__1)) != null)
			{
				HandModelManager.ModelGroup modelGroup = this.ModelPool.Find(new Predicate<HandModelManager.ModelGroup>(<RemoveGroup>c__AnonStorey.<>m__0));
				if (modelGroup != null)
				{
					this.ModelPool.Remove(modelGroup);
					foreach (HandModelBase key in modelGroup.modelList)
					{
						this.modelGroupMapping.Remove(key);
					}
				}
			}
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000E4E58 File Offset: 0x000E3258
		private IEnumerator removeGroupWait(string groupName)
		{
			yield return new WaitForEndOfFrame();
			this.RemoveGroup(groupName);
			yield break;
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000E4E7A File Offset: 0x000E327A
		public void RemoveGroupWait(string groupName)
		{
			base.StartCoroutine(this.removeGroupWait(groupName));
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000E4E8C File Offset: 0x000E328C
		public T GetHandModel<T>(int handId) where T : HandModelBase
		{
			foreach (HandModelManager.ModelGroup modelGroup in this.ModelPool)
			{
				foreach (HandModelBase handModelBase in modelGroup.modelsCheckedOut)
				{
					if (handModelBase.GetLeapHand().Id == handId && handModelBase is T)
					{
						return handModelBase as T;
					}
				}
			}
			return (T)((object)null);
		}

		// Token: 0x04002274 RID: 8820
		protected Dictionary<int, HandRepresentation> graphicsHandReps = new Dictionary<int, HandRepresentation>();

		// Token: 0x04002275 RID: 8821
		protected Dictionary<int, HandRepresentation> physicsHandReps = new Dictionary<int, HandRepresentation>();

		// Token: 0x04002276 RID: 8822
		protected bool graphicsEnabled = true;

		// Token: 0x04002277 RID: 8823
		protected bool physicsEnabled = true;

		// Token: 0x04002278 RID: 8824
		[Tooltip("The LeapProvider to use to drive hand representations in the defined model pool groups.")]
		[SerializeField]
		[OnEditorChange("leapProvider")]
		private LeapProvider _leapProvider;

		// Token: 0x04002279 RID: 8825
		[SerializeField]
		[Tooltip("To add a new set of Hand Models, first add the Left and Right objects as children of this object. Then create a new Model Pool entry referencing the new Hand Models and configure it as desired. Once configured, the Hand Model Manager object pipes Leap tracking data to the Hand Models as hands are tracked, and spawns duplicates as needed if \"Can Duplicate\" is enabled.")]
		private List<HandModelManager.ModelGroup> ModelPool = new List<HandModelManager.ModelGroup>();

		// Token: 0x0400227A RID: 8826
		private List<HandRepresentation> activeHandReps = new List<HandRepresentation>();

		// Token: 0x0400227B RID: 8827
		private Dictionary<HandModelBase, HandModelManager.ModelGroup> modelGroupMapping = new Dictionary<HandModelBase, HandModelManager.ModelGroup>();

		// Token: 0x0400227C RID: 8828
		private Dictionary<HandModelBase, HandRepresentation> modelToHandRepMapping = new Dictionary<HandModelBase, HandRepresentation>();

		// Token: 0x020006DF RID: 1759
		[Serializable]
		public class ModelGroup
		{
			// Token: 0x06002A65 RID: 10853 RVA: 0x000E4F5C File Offset: 0x000E335C
			public ModelGroup()
			{
			}

			// Token: 0x06002A66 RID: 10854 RVA: 0x000E4F84 File Offset: 0x000E3384
			public HandModelBase TryGetModel(Chirality chirality, ModelType modelType)
			{
				for (int i = 0; i < this.modelList.Count; i++)
				{
					if (this.modelList[i].HandModelType == modelType && this.modelList[i].Handedness == chirality)
					{
						HandModelBase handModelBase = this.modelList[i];
						this.modelList.RemoveAt(i);
						this.modelsCheckedOut.Add(handModelBase);
						return handModelBase;
					}
				}
				if (this.CanDuplicate)
				{
					for (int j = 0; j < this.modelsCheckedOut.Count; j++)
					{
						if (this.modelsCheckedOut[j].HandModelType == modelType && this.modelsCheckedOut[j].Handedness == chirality)
						{
							HandModelBase original = this.modelsCheckedOut[j];
							HandModelBase handModelBase2 = UnityEngine.Object.Instantiate<HandModelBase>(original);
							handModelBase2.transform.parent = this._handModelManager.transform;
							this._handModelManager.modelGroupMapping.Add(handModelBase2, this);
							this.modelsCheckedOut.Add(handModelBase2);
							return handModelBase2;
						}
					}
				}
				return null;
			}

			// Token: 0x06002A67 RID: 10855 RVA: 0x000E50A7 File Offset: 0x000E34A7
			public void ReturnToGroup(HandModelBase model)
			{
				this.modelsCheckedOut.Remove(model);
				this.modelList.Add(model);
				this._handModelManager.modelToHandRepMapping.Remove(model);
			}

			// Token: 0x0400227D RID: 8829
			public string GroupName;

			// Token: 0x0400227E RID: 8830
			[HideInInspector]
			public HandModelManager _handModelManager;

			// Token: 0x0400227F RID: 8831
			public HandModelBase LeftModel;

			// Token: 0x04002280 RID: 8832
			[HideInInspector]
			public bool IsLeftToBeSpawned;

			// Token: 0x04002281 RID: 8833
			public HandModelBase RightModel;

			// Token: 0x04002282 RID: 8834
			[HideInInspector]
			public bool IsRightToBeSpawned;

			// Token: 0x04002283 RID: 8835
			[NonSerialized]
			public List<HandModelBase> modelList = new List<HandModelBase>();

			// Token: 0x04002284 RID: 8836
			[NonSerialized]
			public List<HandModelBase> modelsCheckedOut = new List<HandModelBase>();

			// Token: 0x04002285 RID: 8837
			public bool IsEnabled = true;

			// Token: 0x04002286 RID: 8838
			public bool CanDuplicate;
		}

		// Token: 0x02000FA0 RID: 4000
		[CompilerGenerated]
		private sealed class <enableGroup>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600748B RID: 29835 RVA: 0x000E50D4 File Offset: 0x000E34D4
			[DebuggerHidden]
			public <enableGroup>c__Iterator0()
			{
			}

			// Token: 0x0600748C RID: 29836 RVA: 0x000E50DC File Offset: 0x000E34DC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = new WaitForEndOfFrame();
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					group = null;
					for (int i = 0; i < this.ModelPool.Count; i++)
					{
						if (this.ModelPool[i].GroupName == groupName)
						{
							group = this.ModelPool[i];
							for (int j = 0; j < this.activeHandReps.Count; j++)
							{
								HandRepresentation handRepresentation = this.activeHandReps[j];
								HandModelBase handModelBase = group.TryGetModel(handRepresentation.RepChirality, handRepresentation.RepType);
								if (handModelBase != null)
								{
									handRepresentation.AddModel(handModelBase);
									this.modelToHandRepMapping.Add(handModelBase, handRepresentation);
								}
							}
							group.IsEnabled = true;
						}
					}
					if (group == null)
					{
						UnityEngine.Debug.LogWarning("A group matching that name does not exisit in the modelPool");
					}
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x17001129 RID: 4393
			// (get) Token: 0x0600748D RID: 29837 RVA: 0x000E522D File Offset: 0x000E362D
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700112A RID: 4394
			// (get) Token: 0x0600748E RID: 29838 RVA: 0x000E5235 File Offset: 0x000E3635
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600748F RID: 29839 RVA: 0x000E523D File Offset: 0x000E363D
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007490 RID: 29840 RVA: 0x000E524D File Offset: 0x000E364D
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040068A3 RID: 26787
			internal HandModelManager.ModelGroup <group>__0;

			// Token: 0x040068A4 RID: 26788
			internal string groupName;

			// Token: 0x040068A5 RID: 26789
			internal HandModelManager $this;

			// Token: 0x040068A6 RID: 26790
			internal object $current;

			// Token: 0x040068A7 RID: 26791
			internal bool $disposing;

			// Token: 0x040068A8 RID: 26792
			internal int $PC;
		}

		// Token: 0x02000FA1 RID: 4001
		[CompilerGenerated]
		private sealed class <disableGroup>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007491 RID: 29841 RVA: 0x000E5254 File Offset: 0x000E3654
			[DebuggerHidden]
			public <disableGroup>c__Iterator1()
			{
			}

			// Token: 0x06007492 RID: 29842 RVA: 0x000E525C File Offset: 0x000E365C
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = new WaitForEndOfFrame();
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					group = null;
					for (int i = 0; i < this.ModelPool.Count; i++)
					{
						if (this.ModelPool[i].GroupName == groupName)
						{
							group = this.ModelPool[i];
							for (int j = 0; j < group.modelsCheckedOut.Count; j++)
							{
								HandModelBase handModelBase = group.modelsCheckedOut[j];
								HandRepresentation handRepresentation;
								if (this.modelToHandRepMapping.TryGetValue(handModelBase, out handRepresentation))
								{
									handRepresentation.RemoveModel(handModelBase);
									group.ReturnToGroup(handModelBase);
									j--;
								}
							}
							group.IsEnabled = false;
						}
					}
					if (group == null)
					{
						UnityEngine.Debug.LogWarning("A group matching that name does not exisit in the modelPool");
					}
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x1700112B RID: 4395
			// (get) Token: 0x06007493 RID: 29843 RVA: 0x000E539C File Offset: 0x000E379C
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700112C RID: 4396
			// (get) Token: 0x06007494 RID: 29844 RVA: 0x000E53A4 File Offset: 0x000E37A4
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007495 RID: 29845 RVA: 0x000E53AC File Offset: 0x000E37AC
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007496 RID: 29846 RVA: 0x000E53BC File Offset: 0x000E37BC
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040068A9 RID: 26793
			internal HandModelManager.ModelGroup <group>__0;

			// Token: 0x040068AA RID: 26794
			internal string groupName;

			// Token: 0x040068AB RID: 26795
			internal HandModelManager $this;

			// Token: 0x040068AC RID: 26796
			internal object $current;

			// Token: 0x040068AD RID: 26797
			internal bool $disposing;

			// Token: 0x040068AE RID: 26798
			internal int $PC;
		}

		// Token: 0x02000FA2 RID: 4002
		[CompilerGenerated]
		private sealed class <toggleGroup>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007497 RID: 29847 RVA: 0x000E53C3 File Offset: 0x000E37C3
			[DebuggerHidden]
			public <toggleGroup>c__Iterator2()
			{
			}

			// Token: 0x06007498 RID: 29848 RVA: 0x000E53CC File Offset: 0x000E37CC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = new WaitForEndOfFrame();
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					modelGroup = this.ModelPool.Find(new Predicate<HandModelManager.ModelGroup>(<toggleGroup>c__AnonStorey.<>m__0));
					if (modelGroup != null)
					{
						if (modelGroup.IsEnabled)
						{
							base.DisableGroup(<toggleGroup>c__AnonStorey.groupName);
							modelGroup.IsEnabled = false;
						}
						else
						{
							base.EnableGroup(<toggleGroup>c__AnonStorey.groupName);
							modelGroup.IsEnabled = true;
						}
					}
					else
					{
						UnityEngine.Debug.LogWarning("A group matching that name does not exisit in the modelPool");
					}
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x1700112D RID: 4397
			// (get) Token: 0x06007499 RID: 29849 RVA: 0x000E54E5 File Offset: 0x000E38E5
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700112E RID: 4398
			// (get) Token: 0x0600749A RID: 29850 RVA: 0x000E54ED File Offset: 0x000E38ED
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600749B RID: 29851 RVA: 0x000E54F5 File Offset: 0x000E38F5
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600749C RID: 29852 RVA: 0x000E5505 File Offset: 0x000E3905
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040068AF RID: 26799
			internal string groupName;

			// Token: 0x040068B0 RID: 26800
			internal HandModelManager.ModelGroup <modelGroup>__0;

			// Token: 0x040068B1 RID: 26801
			internal HandModelManager $this;

			// Token: 0x040068B2 RID: 26802
			internal object $current;

			// Token: 0x040068B3 RID: 26803
			internal bool $disposing;

			// Token: 0x040068B4 RID: 26804
			internal int $PC;

			// Token: 0x040068B5 RID: 26805
			private HandModelManager.<toggleGroup>c__Iterator2.<toggleGroup>c__AnonStorey5 $locvar0;

			// Token: 0x02000FA6 RID: 4006
			private sealed class <toggleGroup>c__AnonStorey5
			{
				// Token: 0x060074AC RID: 29868 RVA: 0x000E550C File Offset: 0x000E390C
				public <toggleGroup>c__AnonStorey5()
				{
				}

				// Token: 0x060074AD RID: 29869 RVA: 0x000E5514 File Offset: 0x000E3914
				internal bool <>m__0(HandModelManager.ModelGroup i)
				{
					return i.GroupName == this.groupName;
				}

				// Token: 0x040068C3 RID: 26819
				internal string groupName;

				// Token: 0x040068C4 RID: 26820
				internal HandModelManager.<toggleGroup>c__Iterator2 <>f__ref$2;
			}
		}

		// Token: 0x02000FA3 RID: 4003
		[CompilerGenerated]
		private sealed class <addNewGroupWait>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600749D RID: 29853 RVA: 0x000E5527 File Offset: 0x000E3927
			[DebuggerHidden]
			public <addNewGroupWait>c__Iterator3()
			{
			}

			// Token: 0x0600749E RID: 29854 RVA: 0x000E5530 File Offset: 0x000E3930
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = new WaitForEndOfFrame();
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					base.AddNewGroup(groupName, leftModel, rightModel);
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x1700112F RID: 4399
			// (get) Token: 0x0600749F RID: 29855 RVA: 0x000E55A4 File Offset: 0x000E39A4
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001130 RID: 4400
			// (get) Token: 0x060074A0 RID: 29856 RVA: 0x000E55AC File Offset: 0x000E39AC
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060074A1 RID: 29857 RVA: 0x000E55B4 File Offset: 0x000E39B4
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060074A2 RID: 29858 RVA: 0x000E55C4 File Offset: 0x000E39C4
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040068B6 RID: 26806
			internal string groupName;

			// Token: 0x040068B7 RID: 26807
			internal HandModelBase leftModel;

			// Token: 0x040068B8 RID: 26808
			internal HandModelBase rightModel;

			// Token: 0x040068B9 RID: 26809
			internal HandModelManager $this;

			// Token: 0x040068BA RID: 26810
			internal object $current;

			// Token: 0x040068BB RID: 26811
			internal bool $disposing;

			// Token: 0x040068BC RID: 26812
			internal int $PC;
		}

		// Token: 0x02000FA4 RID: 4004
		[CompilerGenerated]
		private sealed class <RemoveGroup>c__AnonStorey6
		{
			// Token: 0x060074A3 RID: 29859 RVA: 0x000E55CB File Offset: 0x000E39CB
			public <RemoveGroup>c__AnonStorey6()
			{
			}

			// Token: 0x060074A4 RID: 29860 RVA: 0x000E55D3 File Offset: 0x000E39D3
			internal bool <>m__0(HandModelManager.ModelGroup i)
			{
				return i.GroupName == this.groupName;
			}

			// Token: 0x060074A5 RID: 29861 RVA: 0x000E55E6 File Offset: 0x000E39E6
			internal bool <>m__1(HandModelManager.ModelGroup i)
			{
				return i.GroupName == this.groupName;
			}

			// Token: 0x040068BD RID: 26813
			internal string groupName;
		}

		// Token: 0x02000FA5 RID: 4005
		[CompilerGenerated]
		private sealed class <removeGroupWait>c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060074A6 RID: 29862 RVA: 0x000E55F9 File Offset: 0x000E39F9
			[DebuggerHidden]
			public <removeGroupWait>c__Iterator4()
			{
			}

			// Token: 0x060074A7 RID: 29863 RVA: 0x000E5604 File Offset: 0x000E3A04
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = new WaitForEndOfFrame();
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					base.RemoveGroup(groupName);
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x17001131 RID: 4401
			// (get) Token: 0x060074A8 RID: 29864 RVA: 0x000E566C File Offset: 0x000E3A6C
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001132 RID: 4402
			// (get) Token: 0x060074A9 RID: 29865 RVA: 0x000E5674 File Offset: 0x000E3A74
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060074AA RID: 29866 RVA: 0x000E567C File Offset: 0x000E3A7C
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060074AB RID: 29867 RVA: 0x000E568C File Offset: 0x000E3A8C
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040068BE RID: 26814
			internal string groupName;

			// Token: 0x040068BF RID: 26815
			internal HandModelManager $this;

			// Token: 0x040068C0 RID: 26816
			internal object $current;

			// Token: 0x040068C1 RID: 26817
			internal bool $disposing;

			// Token: 0x040068C2 RID: 26818
			internal int $PC;
		}
	}
}
