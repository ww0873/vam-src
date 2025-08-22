using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000206 RID: 518
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentDescriptor
	{
		// Token: 0x06000A5B RID: 2651 RVA: 0x0003EF6A File Offset: 0x0003D36A
		public PersistentDescriptor()
		{
			this.Children = new PersistentDescriptor[0];
			this.Components = new PersistentDescriptor[0];
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0003EF8A File Offset: 0x0003D38A
		public PersistentDescriptor(UnityEngine.Object obj)
		{
			this.InstanceId = obj.GetMappedInstanceID();
			this.TypeName = obj.GetType().AssemblyQualifiedName;
			this.Children = new PersistentDescriptor[0];
			this.Components = new PersistentDescriptor[0];
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0003EFC8 File Offset: 0x0003D3C8
		public override string ToString()
		{
			string text = string.Empty;
			PersistentDescriptor persistentDescriptor = this;
			if (persistentDescriptor.Parent == null)
			{
				text += "/";
			}
			else
			{
				while (persistentDescriptor.Parent != null)
				{
					text = text + "/" + persistentDescriptor.Parent.InstanceId;
					persistentDescriptor = persistentDescriptor.Parent;
				}
			}
			return string.Format("Descriptor InstanceId = {0}, Type = {1}, Path = {2}, Children = {3} Components = {4}", new object[]
			{
				this.InstanceId,
				this.TypeName,
				text,
				(this.Children == null) ? 0 : this.Children.Length,
				(this.Components == null) ? 0 : this.Components.Length
			});
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0003F09C File Offset: 0x0003D49C
		public long[] GetInstanceIds()
		{
			if ((this.Children == null || this.Children.Length == 0) && (this.Components == null || this.Components.Length == 0))
			{
				return new long[]
				{
					this.InstanceId
				};
			}
			List<long> list = new List<long>();
			this.GetInstanceIds(this, list);
			return list.ToArray();
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0003F100 File Offset: 0x0003D500
		private void GetInstanceIds(PersistentDescriptor descriptor, List<long> instanceIds)
		{
			instanceIds.Add(descriptor.InstanceId);
			if (descriptor.Components != null)
			{
				for (int i = 0; i < descriptor.Components.Length; i++)
				{
					this.GetInstanceIds(descriptor.Components[i], instanceIds);
				}
			}
			if (descriptor.Children != null)
			{
				for (int j = 0; j < descriptor.Children.Length; j++)
				{
					this.GetInstanceIds(descriptor.Children[j], instanceIds);
				}
			}
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0003F17F File Offset: 0x0003D57F
		public void FindReferencedObjects(Dictionary<long, UnityEngine.Object> referredObjects, Dictionary<long, UnityEngine.Object> allObjects, bool allowNulls)
		{
			this.FindReferencedObjects(this, referredObjects, allObjects, allowNulls);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0003F18C File Offset: 0x0003D58C
		private void FindReferencedObjects(PersistentDescriptor descriptor, Dictionary<long, UnityEngine.Object> referencedObjects, Dictionary<long, UnityEngine.Object> allObjects, bool allowNulls)
		{
			UnityEngine.Object value;
			if (allObjects.TryGetValue(descriptor.InstanceId, out value))
			{
				if (!referencedObjects.ContainsKey(descriptor.InstanceId))
				{
					referencedObjects.Add(descriptor.InstanceId, value);
				}
			}
			else if (allowNulls && !referencedObjects.ContainsKey(descriptor.InstanceId))
			{
				referencedObjects.Add(descriptor.InstanceId, null);
			}
			if (descriptor.Components != null)
			{
				for (int i = 0; i < descriptor.Components.Length; i++)
				{
					PersistentDescriptor descriptor2 = descriptor.Components[i];
					this.FindReferencedObjects(descriptor2, referencedObjects, allObjects, allowNulls);
				}
			}
			if (descriptor.Children != null)
			{
				for (int j = 0; j < descriptor.Children.Length; j++)
				{
					PersistentDescriptor descriptor3 = descriptor.Children[j];
					this.FindReferencedObjects(descriptor3, referencedObjects, allObjects, allowNulls);
				}
			}
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0003F268 File Offset: 0x0003D668
		public PersistentDescriptor[] FlattenHierarchy()
		{
			List<PersistentDescriptor> list = new List<PersistentDescriptor>();
			this.FlattenHierarchy(this, list);
			return list.ToArray();
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0003F28C File Offset: 0x0003D68C
		private void FlattenHierarchy(PersistentDescriptor descriptor, List<PersistentDescriptor> descriptors)
		{
			descriptors.Add(descriptor);
			if (descriptor.Components != null)
			{
				for (int i = 0; i < descriptor.Components.Length; i++)
				{
					descriptors.Add(descriptor.Components[i]);
				}
			}
			if (descriptor.Children != null)
			{
				for (int j = 0; j < descriptor.Children.Length; j++)
				{
					this.FlattenHierarchy(descriptor.Children[j], descriptors);
				}
			}
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0003F308 File Offset: 0x0003D708
		public static UnityEngine.Object GetOrCreateObject(PersistentDescriptor descriptor, Dictionary<long, UnityEngine.Object> dependencies, Dictionary<long, UnityEngine.Object> decomposition = null)
		{
			Type type = Type.GetType(descriptor.TypeName);
			if (type == null)
			{
				Debug.LogError("Unable to find System.Type for " + descriptor.TypeName);
				return null;
			}
			if (type == typeof(GameObject))
			{
				GameObject[] orCreateGameObjects = PersistentDescriptor.GetOrCreateGameObjects(new PersistentDescriptor[]
				{
					descriptor
				}, dependencies, decomposition);
				return orCreateGameObjects[0];
			}
			UnityEngine.Object @object;
			if (!dependencies.TryGetValue(descriptor.InstanceId, out @object))
			{
				@object = PersistentDescriptor.CreateInstance(type);
			}
			if (@object == null)
			{
				Debug.LogError("Unable to instantiate object of type " + type.FullName);
				return null;
			}
			if (!dependencies.ContainsKey(descriptor.InstanceId))
			{
				dependencies.Add(descriptor.InstanceId, @object);
			}
			if (decomposition != null)
			{
				decomposition.Add(descriptor.InstanceId, @object);
			}
			return @object;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0003F3D4 File Offset: 0x0003D7D4
		private static UnityEngine.Object CreateInstance(Type type)
		{
			if (type == typeof(Material))
			{
				if (PersistentDescriptor.m_standard == null)
				{
					PersistentDescriptor.m_standard = Shader.Find("Standard");
				}
				return new Material(PersistentDescriptor.m_standard);
			}
			if (type == typeof(Texture2D))
			{
				return new Texture2D(1, 1, TextureFormat.ARGB32, true);
			}
			return (UnityEngine.Object)Activator.CreateInstance(type);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0003F448 File Offset: 0x0003D848
		public static GameObject[] GetOrCreateGameObjects(PersistentDescriptor[] descriptors, Dictionary<long, UnityEngine.Object> dependencies, Dictionary<long, UnityEngine.Object> decomposition = null)
		{
			List<GameObject> list = new List<GameObject>();
			foreach (PersistentDescriptor descriptor in descriptors)
			{
				PersistentDescriptor.CreateGameObjectWithComponents(descriptor, list, dependencies, decomposition);
			}
			return list.ToArray();
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0003F484 File Offset: 0x0003D884
		public static PersistentDescriptor[] CreatePersistentDescriptors(UnityEngine.Object[] objects)
		{
			List<PersistentDescriptor> list = new List<PersistentDescriptor>();
			foreach (UnityEngine.Object @object in objects)
			{
				if (!(@object == null))
				{
					PersistentDescriptor item = new PersistentDescriptor(@object);
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0003F4D4 File Offset: 0x0003D8D4
		private static void CreateGameObjectWithComponents(PersistentDescriptor descriptor, List<GameObject> createdGameObjects, Dictionary<long, UnityEngine.Object> objects, Dictionary<long, UnityEngine.Object> decomposition = null)
		{
			GameObject gameObject;
			if (objects.ContainsKey(descriptor.InstanceId))
			{
				UnityEngine.Object @object = objects[descriptor.InstanceId];
				if (@object != null && !(@object is GameObject))
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Invalid Type ",
						@object.name,
						" ",
						@object.GetType(),
						" ",
						@object.GetInstanceID(),
						" ",
						descriptor.TypeName
					}));
				}
				gameObject = (GameObject)@object;
			}
			else
			{
				gameObject = new GameObject();
				objects.Add(descriptor.InstanceId, gameObject);
			}
			if (decomposition != null && !decomposition.ContainsKey(descriptor.InstanceId))
			{
				decomposition.Add(descriptor.InstanceId, gameObject);
			}
			createdGameObjects.Add(gameObject);
			gameObject.SetActive(false);
			if (descriptor.Parent != null)
			{
				if (!objects.ContainsKey(descriptor.Parent.InstanceId))
				{
					throw new ArgumentException(string.Format("objects dictionary is supposed to have object with instance id {0} at this stage. Descriptor {1}", descriptor.Parent.InstanceId, descriptor, "descriptor"));
				}
				GameObject gameObject2 = objects[descriptor.Parent.InstanceId] as GameObject;
				if (gameObject2 == null)
				{
					throw new ArgumentException(string.Format("object with instance id {0} should have GameObject type. Descriptor {1}", descriptor.Parent.InstanceId, descriptor, "descriptor"));
				}
				gameObject.transform.SetParent(gameObject2.transform, false);
			}
			if (descriptor.Components != null)
			{
				HashSet<Type> requirements = new HashSet<Type>();
				for (int i = 0; i < descriptor.Components.Length; i++)
				{
					PersistentDescriptor persistentDescriptor = descriptor.Components[i];
					Type type = Type.GetType(persistentDescriptor.TypeName);
					if (type == null)
					{
						Debug.LogWarningFormat("Unknown type {0} associated with component Descriptor {1}", new object[]
						{
							persistentDescriptor.TypeName,
							persistentDescriptor.ToString()
						});
					}
					else if (!type.IsSubclassOf(typeof(Component)))
					{
						Debug.LogErrorFormat("{0} is not subclass of {1}", new object[]
						{
							type.FullName,
							typeof(Component).FullName
						});
					}
					else
					{
						UnityEngine.Object object2;
						if (objects.ContainsKey(persistentDescriptor.InstanceId))
						{
							object2 = objects[persistentDescriptor.InstanceId];
							if (object2 != null && !(object2 is Component))
							{
								Debug.LogError(string.Concat(new object[]
								{
									"Invalid Type. Component ",
									object2.name,
									" ",
									object2.GetType(),
									" ",
									object2.GetInstanceID(),
									" ",
									descriptor.TypeName,
									" ",
									persistentDescriptor.TypeName
								}));
							}
						}
						else
						{
							object2 = PersistentDescriptor.AddComponent(objects, gameObject, requirements, persistentDescriptor, type);
						}
						if (decomposition != null && !decomposition.ContainsKey(persistentDescriptor.InstanceId))
						{
							decomposition.Add(persistentDescriptor.InstanceId, object2);
						}
					}
				}
			}
			if (descriptor.Children != null)
			{
				for (int j = 0; j < descriptor.Children.Length; j++)
				{
					PersistentDescriptor descriptor2 = descriptor.Children[j];
					PersistentDescriptor.CreateGameObjectWithComponents(descriptor2, createdGameObjects, objects, decomposition);
				}
			}
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0003F844 File Offset: 0x0003DC44
		private static UnityEngine.Object AddComponent(Dictionary<long, UnityEngine.Object> objects, GameObject go, HashSet<Type> requirements, PersistentDescriptor componentDescriptor, Type componentType)
		{
			PersistentDescriptor.<AddComponent>c__AnonStorey0 <AddComponent>c__AnonStorey = new PersistentDescriptor.<AddComponent>c__AnonStorey0();
			<AddComponent>c__AnonStorey.go = go;
			bool flag = requirements.Contains(componentType) || componentType.IsSubclassOf(typeof(Transform)) || componentType == typeof(Transform) || componentType.IsDefined(typeof(DisallowMultipleComponent), true) || (PersistentDescriptor.m_dependencies.ContainsKey(componentType) && PersistentDescriptor.m_dependencies[componentType].Any(new Func<Type, bool>(<AddComponent>c__AnonStorey.<>m__0)));
			Component component;
			if (flag)
			{
				component = <AddComponent>c__AnonStorey.go.GetComponent(componentType);
				if (component == null)
				{
					component = <AddComponent>c__AnonStorey.go.AddComponent(componentType);
				}
			}
			else
			{
				component = <AddComponent>c__AnonStorey.go.AddComponent(componentType);
				if (component == null)
				{
					component = <AddComponent>c__AnonStorey.go.GetComponent(componentType);
				}
			}
			if (component == null)
			{
				Debug.LogErrorFormat("Unable to add or get component of type {0}", new object[]
				{
					componentType
				});
			}
			else
			{
				object[] customAttributes = component.GetType().GetCustomAttributes(typeof(RequireComponent), true);
				for (int i = 0; i < customAttributes.Length; i++)
				{
					RequireComponent requireComponent = customAttributes[i] as RequireComponent;
					if (requireComponent != null)
					{
						if (requireComponent.m_Type0 != null && !requirements.Contains(requireComponent.m_Type0))
						{
							requirements.Add(requireComponent.m_Type0);
						}
						if (requireComponent.m_Type1 != null && !requirements.Contains(requireComponent.m_Type1))
						{
							requirements.Add(requireComponent.m_Type1);
						}
						if (requireComponent.m_Type2 != null && !requirements.Contains(requireComponent.m_Type2))
						{
							requirements.Add(requireComponent.m_Type2);
						}
					}
				}
				objects.Add(componentDescriptor.InstanceId, component);
			}
			return component;
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0003FA34 File Offset: 0x0003DE34
		public static PersistentDescriptor CreateDescriptor(GameObject go, PersistentDescriptor parentDescriptor = null)
		{
			PersistentIgnore component = go.GetComponent<PersistentIgnore>();
			if (component != null)
			{
				return null;
			}
			PersistentDescriptor persistentDescriptor = new PersistentDescriptor(go);
			persistentDescriptor.Parent = parentDescriptor;
			Component[] array;
			if (component == null)
			{
				IEnumerable<Component> components = go.GetComponents<Component>();
				if (PersistentDescriptor.<>f__am$cache0 == null)
				{
					PersistentDescriptor.<>f__am$cache0 = new Func<Component, bool>(PersistentDescriptor.<CreateDescriptor>m__0);
				}
				array = components.Where(PersistentDescriptor.<>f__am$cache0).ToArray<Component>();
			}
			else
			{
				array = go.GetComponents<Transform>();
				Array.Resize<Component>(ref array, array.Length + 1);
				array[array.Length - 1] = component;
			}
			if (array.Length > 0)
			{
				persistentDescriptor.Components = new PersistentDescriptor[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					Component obj = array[i];
					PersistentDescriptor persistentDescriptor2 = new PersistentDescriptor(obj);
					persistentDescriptor2.Parent = persistentDescriptor;
					persistentDescriptor.Components[i] = persistentDescriptor2;
				}
			}
			Transform transform = go.transform;
			if (transform.childCount > 0)
			{
				List<PersistentDescriptor> list = new List<PersistentDescriptor>();
				IEnumerator enumerator = transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						Transform transform2 = (Transform)obj2;
						if (component == null)
						{
							PersistentDescriptor persistentDescriptor3 = PersistentDescriptor.CreateDescriptor(transform2.gameObject, persistentDescriptor);
							if (persistentDescriptor3 != null)
							{
								list.Add(persistentDescriptor3);
							}
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				persistentDescriptor.Children = list.ToArray();
			}
			return persistentDescriptor;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0003FBB4 File Offset: 0x0003DFB4
		[ProtoAfterDeserialization]
		public void OnDeserialized()
		{
			if (this.Components != null)
			{
				for (int i = 0; i < this.Components.Length; i++)
				{
					this.Components[i].Parent = this;
				}
			}
			if (this.Children != null)
			{
				for (int j = 0; j < this.Children.Length; j++)
				{
					this.Children[j].Parent = this;
				}
			}
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0003FC28 File Offset: 0x0003E028
		// Note: this type is marked as 'beforefieldinit'.
		static PersistentDescriptor()
		{
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x0003FC79 File Offset: 0x0003E079
		[CompilerGenerated]
		private static bool <CreateDescriptor>m__0(Component c)
		{
			return c != null && !PersistentDescriptor.IgnoreTypes.Contains(c.GetType());
		}

		// Token: 0x04000B69 RID: 2921
		public static readonly HashSet<Type> IgnoreTypes = new HashSet<Type>(new Type[0]);

		// Token: 0x04000B6A RID: 2922
		public static readonly Dictionary<Type, HashSet<Type>> m_dependencies = new Dictionary<Type, HashSet<Type>>
		{
			{
				typeof(ParticleSystemRenderer),
				new HashSet<Type>
				{
					typeof(ParticleSystem)
				}
			}
		};

		// Token: 0x04000B6B RID: 2923
		private static Shader m_standard;

		// Token: 0x04000B6C RID: 2924
		public long InstanceId;

		// Token: 0x04000B6D RID: 2925
		public string TypeName;

		// Token: 0x04000B6E RID: 2926
		[ProtoIgnore]
		public PersistentDescriptor Parent;

		// Token: 0x04000B6F RID: 2927
		public PersistentDescriptor[] Children;

		// Token: 0x04000B70 RID: 2928
		public PersistentDescriptor[] Components;

		// Token: 0x04000B71 RID: 2929
		[CompilerGenerated]
		private static Func<Component, bool> <>f__am$cache0;

		// Token: 0x02000EAC RID: 3756
		[CompilerGenerated]
		private sealed class <AddComponent>c__AnonStorey0
		{
			// Token: 0x06007179 RID: 29049 RVA: 0x0003FC9D File Offset: 0x0003E09D
			public <AddComponent>c__AnonStorey0()
			{
			}

			// Token: 0x0600717A RID: 29050 RVA: 0x0003FCA5 File Offset: 0x0003E0A5
			internal bool <>m__0(Type d)
			{
				return this.go.GetComponent(d) != null;
			}

			// Token: 0x04006548 RID: 25928
			internal GameObject go;
		}
	}
}
