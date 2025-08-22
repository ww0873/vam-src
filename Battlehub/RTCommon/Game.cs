using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Battlehub.RTCommon
{
	// Token: 0x020000B0 RID: 176
	public class Game : MonoBehaviour
	{
		// Token: 0x060002CB RID: 715 RVA: 0x00013595 File Offset: 0x00011995
		public Game()
		{
		}

		// Token: 0x060002CC RID: 716 RVA: 0x000135A0 File Offset: 0x000119A0
		private void Awake()
		{
			if (this.BtnRestart != null)
			{
				this.BtnRestart.onClick.AddListener(new UnityAction(this.RestartGame));
			}
			RuntimeEditorApplication.ActiveWindowChanged += this.OnActiveWindowChanged;
			this.StartGame();
			this.AwakeOverride();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000135F8 File Offset: 0x000119F8
		private void Start()
		{
			this.StartOverride();
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00013600 File Offset: 0x00011A00
		private void OnDestroy()
		{
			if (this.m_applicationQuit)
			{
				return;
			}
			this.OnDestroyOverride();
			this.DestroyGame();
			if (this.BtnRestart != null)
			{
				this.BtnRestart.onClick.RemoveListener(new UnityAction(this.RestartGame));
			}
			RuntimeEditorApplication.ActiveWindowChanged -= this.OnActiveWindowChanged;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00013664 File Offset: 0x00011A64
		private void OnApplicationQuit()
		{
			this.m_applicationQuit = true;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0001366D File Offset: 0x00011A6D
		private void RestartGame()
		{
			RuntimeEditorApplication.IsPlaying = false;
			RuntimeEditorApplication.IsPlaying = true;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0001367C File Offset: 0x00011A7C
		private void StartGame()
		{
			this.DestroyGame();
			IEnumerable<GameObject> source = ExposeToEditor.FindAll(ExposeToEditorObjectType.EditorMode, true);
			if (Game.<>f__am$cache0 == null)
			{
				Game.<>f__am$cache0 = new Func<GameObject, ExposeToEditor>(Game.<StartGame>m__0);
			}
			IEnumerable<ExposeToEditor> source2 = source.Select(Game.<>f__am$cache0);
			if (Game.<>f__am$cache1 == null)
			{
				Game.<>f__am$cache1 = new Func<ExposeToEditor, int>(Game.<StartGame>m__1);
			}
			this.m_editorObjects = source2.OrderBy(Game.<>f__am$cache1).ToArray<ExposeToEditor>();
			IEnumerable<ExposeToEditor> editorObjects = this.m_editorObjects;
			if (Game.<>f__am$cache2 == null)
			{
				Game.<>f__am$cache2 = new Func<ExposeToEditor, bool>(Game.<StartGame>m__2);
			}
			this.m_enabledEditorObjects = editorObjects.Where(Game.<>f__am$cache2).ToArray<ExposeToEditor>();
			this.m_editorSelection = RuntimeSelection.objects;
			HashSet<GameObject> hashSet = new HashSet<GameObject>((RuntimeSelection.gameObjects == null) ? new GameObject[0] : RuntimeSelection.gameObjects);
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.m_editorObjects.Length; i++)
			{
				ExposeToEditor exposeToEditor = this.m_editorObjects[i];
				if (!(exposeToEditor.Parent != null))
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(exposeToEditor.gameObject, exposeToEditor.transform.position, exposeToEditor.transform.rotation);
					ExposeToEditor component = gameObject.GetComponent<ExposeToEditor>();
					component.ObjectType = ExposeToEditorObjectType.PlayMode;
					component.SetName(exposeToEditor.name);
					component.Init();
					ExposeToEditor[] componentsInChildren = exposeToEditor.GetComponentsInChildren<ExposeToEditor>(true);
					ExposeToEditor[] componentsInChildren2 = gameObject.GetComponentsInChildren<ExposeToEditor>(true);
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						if (hashSet.Contains(componentsInChildren[j].gameObject))
						{
							list.Add(componentsInChildren2[j].gameObject);
						}
					}
					exposeToEditor.gameObject.SetActive(false);
				}
			}
			bool enabled = RuntimeUndo.Enabled;
			RuntimeUndo.Enabled = false;
			RuntimeSelection.objects = list.ToArray();
			RuntimeUndo.Enabled = enabled;
			RuntimeUndo.Store();
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0001384C File Offset: 0x00011C4C
		private void DestroyGame()
		{
			if (this.m_editorObjects == null)
			{
				return;
			}
			this.OnDestoryGameOverride();
			IEnumerable<GameObject> source = ExposeToEditor.FindAll(ExposeToEditorObjectType.PlayMode, true);
			if (Game.<>f__am$cache3 == null)
			{
				Game.<>f__am$cache3 = new Func<GameObject, ExposeToEditor>(Game.<DestroyGame>m__3);
			}
			foreach (ExposeToEditor exposeToEditor in source.Select(Game.<>f__am$cache3).ToArray<ExposeToEditor>())
			{
				if (exposeToEditor != null)
				{
					UnityEngine.Object.DestroyImmediate(exposeToEditor.gameObject);
				}
			}
			for (int j = 0; j < this.m_enabledEditorObjects.Length; j++)
			{
				ExposeToEditor exposeToEditor2 = this.m_enabledEditorObjects[j];
				if (exposeToEditor2 != null)
				{
					exposeToEditor2.gameObject.SetActive(true);
				}
			}
			bool enabled = RuntimeUndo.Enabled;
			RuntimeUndo.Enabled = false;
			RuntimeSelection.objects = this.m_editorSelection;
			RuntimeUndo.Enabled = enabled;
			RuntimeUndo.Restore();
			this.m_editorObjects = null;
			this.m_enabledEditorObjects = null;
			this.m_editorSelection = null;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0001393F File Offset: 0x00011D3F
		protected virtual void OnActiveWindowChanged()
		{
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00013941 File Offset: 0x00011D41
		protected virtual void AwakeOverride()
		{
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00013943 File Offset: 0x00011D43
		protected virtual void StartOverride()
		{
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00013945 File Offset: 0x00011D45
		protected virtual void OnDestroyOverride()
		{
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00013947 File Offset: 0x00011D47
		protected virtual void OnDestoryGameOverride()
		{
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00013949 File Offset: 0x00011D49
		[CompilerGenerated]
		private static ExposeToEditor <StartGame>m__0(GameObject go)
		{
			return go.GetComponent<ExposeToEditor>();
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00013951 File Offset: 0x00011D51
		[CompilerGenerated]
		private static int <StartGame>m__1(ExposeToEditor exp)
		{
			return exp.transform.GetSiblingIndex();
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0001395E File Offset: 0x00011D5E
		[CompilerGenerated]
		private static bool <StartGame>m__2(ExposeToEditor eo)
		{
			return eo.gameObject.activeSelf;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0001396B File Offset: 0x00011D6B
		[CompilerGenerated]
		private static ExposeToEditor <DestroyGame>m__3(GameObject go)
		{
			return go.GetComponent<ExposeToEditor>();
		}

		// Token: 0x04000393 RID: 915
		public Button BtnRestart;

		// Token: 0x04000394 RID: 916
		private ExposeToEditor[] m_editorObjects;

		// Token: 0x04000395 RID: 917
		private ExposeToEditor[] m_enabledEditorObjects;

		// Token: 0x04000396 RID: 918
		private UnityEngine.Object[] m_editorSelection;

		// Token: 0x04000397 RID: 919
		private bool m_applicationQuit;

		// Token: 0x04000398 RID: 920
		[CompilerGenerated]
		private static Func<GameObject, ExposeToEditor> <>f__am$cache0;

		// Token: 0x04000399 RID: 921
		[CompilerGenerated]
		private static Func<ExposeToEditor, int> <>f__am$cache1;

		// Token: 0x0400039A RID: 922
		[CompilerGenerated]
		private static Func<ExposeToEditor, bool> <>f__am$cache2;

		// Token: 0x0400039B RID: 923
		[CompilerGenerated]
		private static Func<GameObject, ExposeToEditor> <>f__am$cache3;
	}
}
