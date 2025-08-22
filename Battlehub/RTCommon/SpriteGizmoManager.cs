using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000DA RID: 218
	public class SpriteGizmoManager : MonoBehaviour
	{
		// Token: 0x06000431 RID: 1073 RVA: 0x00017AA1 File Offset: 0x00015EA1
		public SpriteGizmoManager()
		{
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00017AA9 File Offset: 0x00015EA9
		private void Awake()
		{
			if (SpriteGizmoManager.m_instance != null)
			{
				Debug.LogWarning("Another instance of GizmoManager Exists");
			}
			SpriteGizmoManager.m_instance = this;
			SpriteGizmoManager.Cleanup();
			SpriteGizmoManager.Initialize();
			this.AwakeOverride();
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00017ADB File Offset: 0x00015EDB
		private void OnDestroy()
		{
			SpriteGizmoManager.Cleanup();
			if (SpriteGizmoManager.m_instance == this)
			{
				SpriteGizmoManager.m_instance = null;
				SpriteGizmoManager.m_typeToMaterial = null;
				SpriteGizmoManager.m_types = null;
			}
			this.OnDestroyOverride();
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00017B0A File Offset: 0x00015F0A
		protected virtual void AwakeOverride()
		{
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00017B0C File Offset: 0x00015F0C
		protected virtual void OnDestroyOverride()
		{
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00017B0E File Offset: 0x00015F0E
		protected virtual Type[] GetTypes(Type[] types)
		{
			return types;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00017B14 File Offset: 0x00015F14
		protected virtual void GreateGizmo(GameObject go, Type type)
		{
			Material material;
			if (SpriteGizmoManager.m_typeToMaterial.TryGetValue(type, out material))
			{
				SpriteGizmo spriteGizmo = go.GetComponent<SpriteGizmo>();
				if (!spriteGizmo)
				{
					spriteGizmo = go.AddComponent<SpriteGizmo>();
				}
				spriteGizmo.Material = material;
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00017B54 File Offset: 0x00015F54
		protected virtual void DestroyGizmo(GameObject go)
		{
			SpriteGizmo component = go.GetComponent<SpriteGizmo>();
			if (component)
			{
				UnityEngine.Object.Destroy(component);
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00017B7C File Offset: 0x00015F7C
		private static void Initialize()
		{
			if (SpriteGizmoManager.m_types != null)
			{
				Debug.LogWarning("Already initialized");
				return;
			}
			SpriteGizmoManager.m_typeToMaterial = new Dictionary<Type, Material>();
			foreach (KeyValuePair<Type, string> keyValuePair in SpriteGizmoManager.m_typeToMaterialName)
			{
				Material material = Resources.Load<Material>(keyValuePair.Value);
				if (material != null)
				{
					SpriteGizmoManager.m_typeToMaterial.Add(keyValuePair.Key, material);
				}
			}
			int num = 0;
			SpriteGizmoManager.m_types = new Type[SpriteGizmoManager.m_typeToMaterial.Count];
			foreach (Type type in SpriteGizmoManager.m_typeToMaterial.Keys)
			{
				SpriteGizmoManager.m_types[num] = type;
				num++;
			}
			SpriteGizmoManager.m_types = SpriteGizmoManager.m_instance.GetTypes(SpriteGizmoManager.m_types);
			SpriteGizmoManager.OnIsOpenedChanged();
			if (SpriteGizmoManager.<>f__mg$cache0 == null)
			{
				SpriteGizmoManager.<>f__mg$cache0 = new RuntimeEditorEvent(SpriteGizmoManager.OnIsOpenedChanged);
			}
			RuntimeEditorApplication.IsOpenedChanged += SpriteGizmoManager.<>f__mg$cache0;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00017CC4 File Offset: 0x000160C4
		private static void Cleanup()
		{
			SpriteGizmoManager.m_types = null;
			SpriteGizmoManager.m_typeToMaterial = null;
			if (SpriteGizmoManager.<>f__mg$cache1 == null)
			{
				SpriteGizmoManager.<>f__mg$cache1 = new RuntimeEditorEvent(SpriteGizmoManager.OnIsOpenedChanged);
			}
			RuntimeEditorApplication.IsOpenedChanged -= SpriteGizmoManager.<>f__mg$cache1;
			SpriteGizmoManager.UnsubscribeAndDestroy();
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00017CFC File Offset: 0x000160FC
		private static void UnsubscribeAndDestroy()
		{
			SpriteGizmoManager.Unsubscribe();
			foreach (SpriteGizmo spriteGizmo in Resources.FindObjectsOfTypeAll<SpriteGizmo>())
			{
				if (!spriteGizmo.gameObject.IsPrefab())
				{
					SpriteGizmoManager.m_instance.DestroyGizmo(spriteGizmo.gameObject);
				}
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00017D4C File Offset: 0x0001614C
		private static void OnIsOpenedChanged()
		{
			if (RuntimeEditorApplication.IsOpened)
			{
				for (int i = 0; i < SpriteGizmoManager.m_types.Length; i++)
				{
					UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(SpriteGizmoManager.m_types[i]);
					for (int j = 0; j < array.Length; j++)
					{
						Component component = array[j] as Component;
						if (component && !component.gameObject.IsPrefab())
						{
							ExposeToEditor component2 = component.gameObject.GetComponent<ExposeToEditor>();
							if (component2 != null)
							{
								SpriteGizmoManager.m_instance.GreateGizmo(component.gameObject, SpriteGizmoManager.m_types[i]);
							}
						}
					}
				}
				SpriteGizmoManager.Subscribe();
			}
			else
			{
				SpriteGizmoManager.UnsubscribeAndDestroy();
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00017E00 File Offset: 0x00016200
		private static void Subscribe()
		{
			if (SpriteGizmoManager.<>f__mg$cache2 == null)
			{
				SpriteGizmoManager.<>f__mg$cache2 = new ExposeToEditorEvent(SpriteGizmoManager.OnAwaked);
			}
			ExposeToEditor.Awaked += SpriteGizmoManager.<>f__mg$cache2;
			if (SpriteGizmoManager.<>f__mg$cache3 == null)
			{
				SpriteGizmoManager.<>f__mg$cache3 = new ExposeToEditorEvent(SpriteGizmoManager.OnDestroyed);
			}
			ExposeToEditor.Destroyed += SpriteGizmoManager.<>f__mg$cache3;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00017E54 File Offset: 0x00016254
		private static void Unsubscribe()
		{
			if (SpriteGizmoManager.<>f__mg$cache4 == null)
			{
				SpriteGizmoManager.<>f__mg$cache4 = new ExposeToEditorEvent(SpriteGizmoManager.OnAwaked);
			}
			ExposeToEditor.Awaked -= SpriteGizmoManager.<>f__mg$cache4;
			if (SpriteGizmoManager.<>f__mg$cache5 == null)
			{
				SpriteGizmoManager.<>f__mg$cache5 = new ExposeToEditorEvent(SpriteGizmoManager.OnDestroyed);
			}
			ExposeToEditor.Destroyed -= SpriteGizmoManager.<>f__mg$cache5;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00017EA8 File Offset: 0x000162A8
		private static void OnAwaked(ExposeToEditor obj)
		{
			for (int i = 0; i < SpriteGizmoManager.m_types.Length; i++)
			{
				Component component = obj.GetComponent(SpriteGizmoManager.m_types[i]);
				if (component != null)
				{
					SpriteGizmoManager.m_instance.GreateGizmo(obj.gameObject, SpriteGizmoManager.m_types[i]);
				}
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00017F00 File Offset: 0x00016300
		private static void OnDestroyed(ExposeToEditor obj)
		{
			for (int i = 0; i < SpriteGizmoManager.m_types.Length; i++)
			{
				Component component = obj.GetComponent(SpriteGizmoManager.m_types[i]);
				if (component != null)
				{
					SpriteGizmoManager.m_instance.DestroyGizmo(obj.gameObject);
				}
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00017F50 File Offset: 0x00016350
		// Note: this type is marked as 'beforefieldinit'.
		static SpriteGizmoManager()
		{
		}

		// Token: 0x04000452 RID: 1106
		private static readonly Dictionary<Type, string> m_typeToMaterialName = new Dictionary<Type, string>
		{
			{
				typeof(Light),
				"BattlehubLightGizmo"
			},
			{
				typeof(Camera),
				"BattlehubCameraGizmo"
			},
			{
				typeof(AudioSource),
				"BattlehubAudioSourceGizmo"
			}
		};

		// Token: 0x04000453 RID: 1107
		private static Dictionary<Type, Material> m_typeToMaterial;

		// Token: 0x04000454 RID: 1108
		private static Type[] m_types;

		// Token: 0x04000455 RID: 1109
		private static SpriteGizmoManager m_instance;

		// Token: 0x04000456 RID: 1110
		[CompilerGenerated]
		private static RuntimeEditorEvent <>f__mg$cache0;

		// Token: 0x04000457 RID: 1111
		[CompilerGenerated]
		private static RuntimeEditorEvent <>f__mg$cache1;

		// Token: 0x04000458 RID: 1112
		[CompilerGenerated]
		private static ExposeToEditorEvent <>f__mg$cache2;

		// Token: 0x04000459 RID: 1113
		[CompilerGenerated]
		private static ExposeToEditorEvent <>f__mg$cache3;

		// Token: 0x0400045A RID: 1114
		[CompilerGenerated]
		private static ExposeToEditorEvent <>f__mg$cache4;

		// Token: 0x0400045B RID: 1115
		[CompilerGenerated]
		private static ExposeToEditorEvent <>f__mg$cache5;
	}
}
