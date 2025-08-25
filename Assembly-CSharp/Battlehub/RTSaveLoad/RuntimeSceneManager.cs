using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200022E RID: 558
	public class RuntimeSceneManager : MonoBehaviour, ISceneManager
	{
		// Token: 0x06000B99 RID: 2969 RVA: 0x000444DA File Offset: 0x000428DA
		public RuntimeSceneManager()
		{
		}

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06000B9A RID: 2970 RVA: 0x000444E4 File Offset: 0x000428E4
		// (remove) Token: 0x06000B9B RID: 2971 RVA: 0x0004451C File Offset: 0x0004291C
		public event EventHandler<ProjectManagerEventArgs> SceneCreated
		{
			add
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.SceneCreated;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.SceneCreated, (EventHandler<ProjectManagerEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.SceneCreated;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.SceneCreated, (EventHandler<ProjectManagerEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06000B9C RID: 2972 RVA: 0x00044554 File Offset: 0x00042954
		// (remove) Token: 0x06000B9D RID: 2973 RVA: 0x0004458C File Offset: 0x0004298C
		public event EventHandler<ProjectManagerEventArgs> SceneSaving
		{
			add
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.SceneSaving;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.SceneSaving, (EventHandler<ProjectManagerEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.SceneSaving;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.SceneSaving, (EventHandler<ProjectManagerEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06000B9E RID: 2974 RVA: 0x000445C4 File Offset: 0x000429C4
		// (remove) Token: 0x06000B9F RID: 2975 RVA: 0x000445FC File Offset: 0x000429FC
		public event EventHandler<ProjectManagerEventArgs> SceneSaved
		{
			add
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.SceneSaved;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.SceneSaved, (EventHandler<ProjectManagerEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.SceneSaved;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.SceneSaved, (EventHandler<ProjectManagerEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06000BA0 RID: 2976 RVA: 0x00044634 File Offset: 0x00042A34
		// (remove) Token: 0x06000BA1 RID: 2977 RVA: 0x0004466C File Offset: 0x00042A6C
		public event EventHandler<ProjectManagerEventArgs> SceneLoading
		{
			add
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.SceneLoading;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.SceneLoading, (EventHandler<ProjectManagerEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.SceneLoading;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.SceneLoading, (EventHandler<ProjectManagerEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06000BA2 RID: 2978 RVA: 0x000446A4 File Offset: 0x00042AA4
		// (remove) Token: 0x06000BA3 RID: 2979 RVA: 0x000446DC File Offset: 0x00042ADC
		public event EventHandler<ProjectManagerEventArgs> SceneLoaded
		{
			add
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.SceneLoaded;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.SceneLoaded, (EventHandler<ProjectManagerEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.SceneLoaded;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.SceneLoaded, (EventHandler<ProjectManagerEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x00044712 File Offset: 0x00042B12
		public ProjectItem ActiveScene
		{
			get
			{
				return this.m_activeScene;
			}
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0004471A File Offset: 0x00042B1A
		private void Awake()
		{
			Dependencies.Serializer.DeepClone<int>(1);
			this.m_project = Dependencies.Project;
			this.m_serializer = Dependencies.Serializer;
			this.AwakeOverride();
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00044744 File Offset: 0x00042B44
		private void Start()
		{
			this.StartOverride();
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0004474C File Offset: 0x00042B4C
		private void OnDestroy()
		{
			this.OnDestroyOverride();
			IdentifiersMap.Instance = null;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0004475A File Offset: 0x00042B5A
		protected virtual void AwakeOverride()
		{
			this.m_activeScene = ProjectItem.CreateScene("New Scene");
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0004476C File Offset: 0x00042B6C
		protected virtual void StartOverride()
		{
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0004476E File Offset: 0x00042B6E
		protected virtual void OnDestroyOverride()
		{
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x00044770 File Offset: 0x00042B70
		public void Exists(ProjectItem item, ProjectManagerCallback<bool> callback)
		{
			RuntimeSceneManager.<Exists>c__AnonStorey0 <Exists>c__AnonStorey = new RuntimeSceneManager.<Exists>c__AnonStorey0();
			<Exists>c__AnonStorey.callback = callback;
			this.m_project.Exists(item, new ProjectEventHandler<bool>(<Exists>c__AnonStorey.<>m__0));
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x000447A4 File Offset: 0x00042BA4
		public virtual void SaveScene(ProjectItem scene, ProjectManagerCallback callback)
		{
			RuntimeSceneManager.<SaveScene>c__AnonStorey1 <SaveScene>c__AnonStorey = new RuntimeSceneManager.<SaveScene>c__AnonStorey1();
			<SaveScene>c__AnonStorey.scene = scene;
			<SaveScene>c__AnonStorey.callback = callback;
			<SaveScene>c__AnonStorey.$this = this;
			if (this.SceneSaving != null)
			{
				this.SceneSaving(this, new ProjectManagerEventArgs(<SaveScene>c__AnonStorey.scene));
			}
			GameObject gameObject = new GameObject();
			ExtraSceneData extraSceneData = gameObject.AddComponent<ExtraSceneData>();
			extraSceneData.Selection = RuntimeSelection.objects;
			PersistentScene data = PersistentScene.CreatePersistentScene(new Type[0]);
			if (<SaveScene>c__AnonStorey.scene.Internal_Data == null)
			{
				<SaveScene>c__AnonStorey.scene.Internal_Data = new ProjectItemData();
			}
			<SaveScene>c__AnonStorey.scene.Internal_Data.RawData = this.m_serializer.Serialize<PersistentScene>(data);
			UnityEngine.Object.Destroy(gameObject);
			this.m_project.Save(<SaveScene>c__AnonStorey.scene, false, new ProjectEventHandler(<SaveScene>c__AnonStorey.<>m__0));
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00044874 File Offset: 0x00042C74
		public virtual void LoadScene(ProjectItem scene, ProjectManagerCallback callback)
		{
			RuntimeSceneManager.<LoadScene>c__AnonStorey2 <LoadScene>c__AnonStorey = new RuntimeSceneManager.<LoadScene>c__AnonStorey2();
			<LoadScene>c__AnonStorey.scene = scene;
			<LoadScene>c__AnonStorey.callback = callback;
			<LoadScene>c__AnonStorey.$this = this;
			this.RaiseSceneLoading(<LoadScene>c__AnonStorey.scene);
			<LoadScene>c__AnonStorey.isEnabled = RuntimeUndo.Enabled;
			RuntimeUndo.Enabled = false;
			RuntimeSelection.objects = null;
			RuntimeUndo.Enabled = <LoadScene>c__AnonStorey.isEnabled;
			this.m_project.LoadData(new ProjectItem[]
			{
				<LoadScene>c__AnonStorey.scene
			}, new ProjectEventHandler<ProjectItem[]>(<LoadScene>c__AnonStorey.<>m__0), new int[0]);
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x000448F6 File Offset: 0x00042CF6
		protected void RaiseSceneLoading(ProjectItem scene)
		{
			if (this.SceneLoading != null)
			{
				this.SceneLoading(this, new ProjectManagerEventArgs(scene));
			}
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x00044915 File Offset: 0x00042D15
		protected void RaiseSceneLoaded(ProjectItem scene)
		{
			if (this.SceneLoaded != null)
			{
				this.SceneLoaded(this, new ProjectManagerEventArgs(scene));
			}
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00044934 File Offset: 0x00042D34
		protected void CompleteSceneLoading(ProjectItem scene, ProjectManagerCallback callback, bool isEnabled, PersistentScene persistentScene)
		{
			PersistentScene.InstantiateGameObjects(persistentScene);
			this.m_project.UnloadData(scene);
			ExtraSceneData extraSceneData = UnityEngine.Object.FindObjectOfType<ExtraSceneData>();
			RuntimeUndo.Enabled = false;
			RuntimeSelection.objects = extraSceneData.Selection;
			RuntimeUndo.Enabled = isEnabled;
			UnityEngine.Object.Destroy(extraSceneData.gameObject);
			this.m_activeScene = scene;
			if (callback != null)
			{
				callback();
			}
			this.RaiseSceneLoaded(scene);
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00044998 File Offset: 0x00042D98
		public virtual void CreateScene()
		{
			RuntimeSelection.objects = null;
			RuntimeUndo.Purge();
			IEnumerable<GameObject> source = ExposeToEditor.FindAll(ExposeToEditorObjectType.EditorMode, false);
			if (RuntimeSceneManager.<>f__am$cache0 == null)
			{
				RuntimeSceneManager.<>f__am$cache0 = new Func<GameObject, ExposeToEditor>(RuntimeSceneManager.<CreateScene>m__0);
			}
			foreach (ExposeToEditor exposeToEditor in source.Select(RuntimeSceneManager.<>f__am$cache0).ToArray<ExposeToEditor>())
			{
				if (exposeToEditor != null)
				{
					UnityEngine.Object.DestroyImmediate(exposeToEditor.gameObject);
				}
			}
			GameObject gameObject = new GameObject();
			gameObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
			gameObject.transform.position = new Vector3(0f, 10f, 0f);
			Light light = gameObject.AddComponent<Light>();
			light.type = LightType.Directional;
			gameObject.name = "Directional Light";
			gameObject.AddComponent<ExposeToEditor>();
			GameObject gameObject2 = new GameObject();
			gameObject2.name = "Main Camera";
			gameObject2.transform.position = new Vector3(0f, 0f, -10f);
			gameObject2.AddComponent<Camera>();
			gameObject2.tag = "MainCamera";
			gameObject2.AddComponent<ExposeToEditor>();
			this.m_activeScene = ProjectItem.CreateScene("New Scene");
			if (this.SceneCreated != null)
			{
				this.SceneCreated(this, new ProjectManagerEventArgs(this.ActiveScene));
			}
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00044AF5 File Offset: 0x00042EF5
		[CompilerGenerated]
		private static ExposeToEditor <CreateScene>m__0(GameObject go)
		{
			return go.GetComponent<ExposeToEditor>();
		}

		// Token: 0x04000C9D RID: 3229
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ProjectManagerEventArgs> SceneCreated;

		// Token: 0x04000C9E RID: 3230
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ProjectManagerEventArgs> SceneSaving;

		// Token: 0x04000C9F RID: 3231
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ProjectManagerEventArgs> SceneSaved;

		// Token: 0x04000CA0 RID: 3232
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ProjectManagerEventArgs> SceneLoading;

		// Token: 0x04000CA1 RID: 3233
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ProjectManagerEventArgs> SceneLoaded;

		// Token: 0x04000CA2 RID: 3234
		protected IProject m_project;

		// Token: 0x04000CA3 RID: 3235
		protected ISerializer m_serializer;

		// Token: 0x04000CA4 RID: 3236
		[NonSerialized]
		private ProjectItem m_activeScene;

		// Token: 0x04000CA5 RID: 3237
		[CompilerGenerated]
		private static Func<GameObject, ExposeToEditor> <>f__am$cache0;

		// Token: 0x02000EC6 RID: 3782
		[CompilerGenerated]
		private sealed class <Exists>c__AnonStorey0
		{
			// Token: 0x060071B6 RID: 29110 RVA: 0x00044AFD File Offset: 0x00042EFD
			public <Exists>c__AnonStorey0()
			{
			}

			// Token: 0x060071B7 RID: 29111 RVA: 0x00044B05 File Offset: 0x00042F05
			internal void <>m__0(ProjectPayload<bool> result)
			{
				if (this.callback != null)
				{
					this.callback(result.Data);
				}
			}

			// Token: 0x04006594 RID: 26004
			internal ProjectManagerCallback<bool> callback;
		}

		// Token: 0x02000EC7 RID: 3783
		[CompilerGenerated]
		private sealed class <SaveScene>c__AnonStorey1
		{
			// Token: 0x060071B8 RID: 29112 RVA: 0x00044B23 File Offset: 0x00042F23
			public <SaveScene>c__AnonStorey1()
			{
			}

			// Token: 0x060071B9 RID: 29113 RVA: 0x00044B2C File Offset: 0x00042F2C
			internal void <>m__0(ProjectPayload saveCompleted)
			{
				this.$this.m_project.UnloadData(this.scene);
				this.$this.m_activeScene = this.scene;
				if (this.callback != null)
				{
					this.callback();
				}
				if (this.$this.SceneSaved != null)
				{
					this.$this.SceneSaved(this.$this, new ProjectManagerEventArgs(this.scene));
				}
			}

			// Token: 0x04006595 RID: 26005
			internal ProjectItem scene;

			// Token: 0x04006596 RID: 26006
			internal ProjectManagerCallback callback;

			// Token: 0x04006597 RID: 26007
			internal RuntimeSceneManager $this;
		}

		// Token: 0x02000EC8 RID: 3784
		[CompilerGenerated]
		private sealed class <LoadScene>c__AnonStorey2
		{
			// Token: 0x060071BA RID: 29114 RVA: 0x00044BA7 File Offset: 0x00042FA7
			public <LoadScene>c__AnonStorey2()
			{
			}

			// Token: 0x060071BB RID: 29115 RVA: 0x00044BB0 File Offset: 0x00042FB0
			internal void <>m__0(ProjectPayload<ProjectItem[]> loadDataCompleted)
			{
				this.scene = loadDataCompleted.Data[0];
				PersistentScene persistentScene = this.$this.m_serializer.Deserialize<PersistentScene>(this.scene.Internal_Data.RawData);
				this.$this.CompleteSceneLoading(this.scene, this.callback, this.isEnabled, persistentScene);
			}

			// Token: 0x04006598 RID: 26008
			internal ProjectItem scene;

			// Token: 0x04006599 RID: 26009
			internal ProjectManagerCallback callback;

			// Token: 0x0400659A RID: 26010
			internal bool isEnabled;

			// Token: 0x0400659B RID: 26011
			internal RuntimeSceneManager $this;
		}
	}
}
