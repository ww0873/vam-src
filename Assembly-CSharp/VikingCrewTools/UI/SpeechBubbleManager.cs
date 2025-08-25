using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace VikingCrewTools.UI
{
	// Token: 0x0200056E RID: 1390
	public class SpeechBubbleManager : MonoBehaviour
	{
		// Token: 0x0600233D RID: 9021 RVA: 0x000C8BEC File Offset: 0x000C6FEC
		public SpeechBubbleManager()
		{
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x0600233E RID: 9022 RVA: 0x000C8C44 File Offset: 0x000C7044
		public static SpeechBubbleManager Instance
		{
			get
			{
				return SpeechBubbleManager._instance;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x0600233F RID: 9023 RVA: 0x000C8C4B File Offset: 0x000C704B
		// (set) Token: 0x06002340 RID: 9024 RVA: 0x000C8C54 File Offset: 0x000C7054
		public Camera Cam
		{
			get
			{
				return this._cam;
			}
			set
			{
				this._cam = value;
				foreach (Queue<SpeechBubbleBehaviour> queue in this._speechBubbleQueueDict.Values)
				{
					foreach (SpeechBubbleBehaviour speechBubbleBehaviour in queue)
					{
						speechBubbleBehaviour.Cam = this._cam;
					}
				}
			}
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x000C8D00 File Offset: 0x000C7100
		protected void Awake()
		{
			if (this._cam == null)
			{
				this._cam = Camera.main;
			}
			if (this._isSingleton)
			{
				SpeechBubbleManager._instance = this;
			}
			this._prefabsDict.Clear();
			this._speechBubbleQueueDict.Clear();
			foreach (SpeechBubbleManager.SpeechbubblePrefab speechbubblePrefab in this._prefabs)
			{
				this._prefabsDict.Add(speechbubblePrefab.type, speechbubblePrefab.prefab);
				this._speechBubbleQueueDict.Add(speechbubblePrefab.type, new Queue<SpeechBubbleBehaviour>());
			}
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x000C8DC8 File Offset: 0x000C71C8
		private IEnumerator DelaySpeechBubble(float delay, Transform objectToFollow, string text, SpeechBubbleManager.SpeechbubbleType type, float timeToLive, Color color, Vector3 offset)
		{
			yield return new WaitForSeconds(delay);
			if (objectToFollow)
			{
				this.AddSpeechBubble(objectToFollow, text, type, timeToLive, color, offset);
			}
			yield break;
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x000C8E18 File Offset: 0x000C7218
		public SpeechBubbleBehaviour AddSpeechBubble(Vector3 position, string text, SpeechBubbleManager.SpeechbubbleType type = SpeechBubbleManager.SpeechbubbleType.NORMAL, float timeToLive = 0f, Color color = default(Color))
		{
			if (timeToLive == 0f)
			{
				timeToLive = this._defaultTimeToLive;
			}
			if (color == default(Color))
			{
				color = this._defaultColor;
			}
			SpeechBubbleBehaviour bubble = this.GetBubble(type);
			bubble.Setup(position, text, timeToLive, color, this.Cam);
			this._speechBubbleQueueDict[type].Enqueue(bubble);
			return bubble;
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x000C8E84 File Offset: 0x000C7284
		public SpeechBubbleBehaviour AddSpeechBubble(Transform objectToFollow, string text, SpeechBubbleManager.SpeechbubbleType type = SpeechBubbleManager.SpeechbubbleType.NORMAL, float timeToLive = 0f, Color color = default(Color), Vector3 offset = default(Vector3))
		{
			if (timeToLive == 0f)
			{
				timeToLive = this._defaultTimeToLive;
			}
			if (color == default(Color))
			{
				color = this._defaultColor;
			}
			SpeechBubbleBehaviour bubble = this.GetBubble(type);
			bubble.Setup(objectToFollow, offset, text, timeToLive, color, this.Cam);
			this._speechBubbleQueueDict[type].Enqueue(bubble);
			return bubble;
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x000C8EF4 File Offset: 0x000C72F4
		public void AddDelayedSpeechBubble(float delay, Transform objectToFollow, string text, SpeechBubbleManager.SpeechbubbleType type = SpeechBubbleManager.SpeechbubbleType.NORMAL, float timeToLive = 0f, Color color = default(Color), Vector3 offset = default(Vector3))
		{
			base.StartCoroutine(this.DelaySpeechBubble(delay, objectToFollow, text, type, timeToLive, color, offset));
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x000C8F1C File Offset: 0x000C731C
		private SpeechBubbleBehaviour GetBubble(SpeechBubbleManager.SpeechbubbleType type = SpeechBubbleManager.SpeechbubbleType.NORMAL)
		{
			SpeechBubbleBehaviour speechBubbleBehaviour;
			if (this._speechBubbleQueueDict[type].Count == 0 || this._speechBubbleQueueDict[type].Peek().gameObject.activeInHierarchy)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.GetPrefab(type));
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.localScale = this._sizeMultiplier * this.GetPrefab(type).transform.localScale;
				speechBubbleBehaviour = gameObject.GetComponent<SpeechBubbleBehaviour>();
				if (!this._is2D)
				{
					Canvas canvas = gameObject.AddComponent<Canvas>();
					canvas.renderMode = RenderMode.WorldSpace;
					canvas.overrideSorting = true;
				}
			}
			else
			{
				speechBubbleBehaviour = this._speechBubbleQueueDict[type].Dequeue();
			}
			speechBubbleBehaviour.transform.SetAsLastSibling();
			return speechBubbleBehaviour;
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x000C8FEE File Offset: 0x000C73EE
		private GameObject GetPrefab(SpeechBubbleManager.SpeechbubbleType type)
		{
			return this._prefabsDict[type];
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x000C8FFC File Offset: 0x000C73FC
		public SpeechBubbleManager.SpeechbubbleType GetRandomSpeechbubbleType()
		{
			return this._prefabs[UnityEngine.Random.Range(0, this._prefabs.Count)].type;
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x000C9020 File Offset: 0x000C7420
		public void Clear()
		{
			foreach (KeyValuePair<SpeechBubbleManager.SpeechbubbleType, Queue<SpeechBubbleBehaviour>> keyValuePair in this._speechBubbleQueueDict)
			{
				foreach (SpeechBubbleBehaviour speechBubbleBehaviour in keyValuePair.Value)
				{
					speechBubbleBehaviour.Clear();
				}
			}
		}

		// Token: 0x04001D30 RID: 7472
		[Header("Default settings:")]
		[FormerlySerializedAs("defaultColor")]
		[SerializeField]
		private Color _defaultColor = Color.white;

		// Token: 0x04001D31 RID: 7473
		[FormerlySerializedAs("defaultTimeToLive")]
		[SerializeField]
		private float _defaultTimeToLive = 1f;

		// Token: 0x04001D32 RID: 7474
		[FormerlySerializedAs("is2D")]
		[SerializeField]
		private bool _is2D = true;

		// Token: 0x04001D33 RID: 7475
		[Tooltip("If you want to change the size of your speechbubbles in a scene without having to change the prefabs then change this value")]
		[FormerlySerializedAs("sizeMultiplier")]
		[SerializeField]
		private float _sizeMultiplier = 1f;

		// Token: 0x04001D34 RID: 7476
		[Tooltip("If you want to use different managers, for example if you want to have one manager for allies and one for enemies in order to style their speech bubbles differently set this to false. Note that you will need to keep track of a reference some other way in that case.")]
		[SerializeField]
		private bool _isSingleton = true;

		// Token: 0x04001D35 RID: 7477
		[Header("Prefabs mapping to each type:")]
		[FormerlySerializedAs("prefabs")]
		[SerializeField]
		private List<SpeechBubbleManager.SpeechbubblePrefab> _prefabs;

		// Token: 0x04001D36 RID: 7478
		private Dictionary<SpeechBubbleManager.SpeechbubbleType, GameObject> _prefabsDict = new Dictionary<SpeechBubbleManager.SpeechbubbleType, GameObject>();

		// Token: 0x04001D37 RID: 7479
		private Dictionary<SpeechBubbleManager.SpeechbubbleType, Queue<SpeechBubbleBehaviour>> _speechBubbleQueueDict = new Dictionary<SpeechBubbleManager.SpeechbubbleType, Queue<SpeechBubbleBehaviour>>();

		// Token: 0x04001D38 RID: 7480
		[SerializeField]
		[Tooltip("Will use main camera if left as null")]
		private Camera _cam;

		// Token: 0x04001D39 RID: 7481
		private static SpeechBubbleManager _instance;

		// Token: 0x0200056F RID: 1391
		public enum SpeechbubbleType
		{
			// Token: 0x04001D3B RID: 7483
			NORMAL,
			// Token: 0x04001D3C RID: 7484
			SERIOUS,
			// Token: 0x04001D3D RID: 7485
			ANGRY,
			// Token: 0x04001D3E RID: 7486
			THINKING
		}

		// Token: 0x02000570 RID: 1392
		[Serializable]
		public class SpeechbubblePrefab
		{
			// Token: 0x0600234A RID: 9034 RVA: 0x000C90C0 File Offset: 0x000C74C0
			public SpeechbubblePrefab()
			{
			}

			// Token: 0x04001D3F RID: 7487
			public SpeechBubbleManager.SpeechbubbleType type;

			// Token: 0x04001D40 RID: 7488
			public GameObject prefab;
		}

		// Token: 0x02000F7F RID: 3967
		[CompilerGenerated]
		private sealed class <DelaySpeechBubble>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007413 RID: 29715 RVA: 0x000C90C8 File Offset: 0x000C74C8
			[DebuggerHidden]
			public <DelaySpeechBubble>c__Iterator0()
			{
			}

			// Token: 0x06007414 RID: 29716 RVA: 0x000C90D0 File Offset: 0x000C74D0
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = new WaitForSeconds(delay);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					if (objectToFollow)
					{
						base.AddSpeechBubble(objectToFollow, text, type, timeToLive, color, offset);
					}
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x1700110F RID: 4367
			// (get) Token: 0x06007415 RID: 29717 RVA: 0x000C916D File Offset: 0x000C756D
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001110 RID: 4368
			// (get) Token: 0x06007416 RID: 29718 RVA: 0x000C9175 File Offset: 0x000C7575
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007417 RID: 29719 RVA: 0x000C917D File Offset: 0x000C757D
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007418 RID: 29720 RVA: 0x000C918D File Offset: 0x000C758D
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006828 RID: 26664
			internal float delay;

			// Token: 0x04006829 RID: 26665
			internal Transform objectToFollow;

			// Token: 0x0400682A RID: 26666
			internal string text;

			// Token: 0x0400682B RID: 26667
			internal SpeechBubbleManager.SpeechbubbleType type;

			// Token: 0x0400682C RID: 26668
			internal float timeToLive;

			// Token: 0x0400682D RID: 26669
			internal Color color;

			// Token: 0x0400682E RID: 26670
			internal Vector3 offset;

			// Token: 0x0400682F RID: 26671
			internal SpeechBubbleManager $this;

			// Token: 0x04006830 RID: 26672
			internal object $current;

			// Token: 0x04006831 RID: 26673
			internal bool $disposing;

			// Token: 0x04006832 RID: 26674
			internal int $PC;
		}
	}
}
