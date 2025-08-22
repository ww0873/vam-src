using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Battlehub.RTSaveLoad.PersistentObjects;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000217 RID: 535
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentScript : PersistentObject
	{
		// Token: 0x06000AC5 RID: 2757 RVA: 0x0004229D File Offset: 0x0004069D
		public PersistentScript()
		{
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x000422B0 File Offset: 0x000406B0
		public PersistentScript(PersistentData baseObjData)
		{
			this.baseObjectData = baseObjData;
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x000422CC File Offset: 0x000406CC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (this.baseObjectData != null)
			{
				this.baseObjectData.ReadFrom(obj);
			}
			Type type = obj.GetType();
			if (!type.IsScript())
			{
				throw new ArgumentException(string.Format("obj of type {0} is not subclass of {1}", type, typeof(MonoBehaviour)), "obj");
			}
			this.TypeName = type.AssemblyQualifiedName;
			do
			{
				foreach (FieldInfo fieldInfo in type.GetSerializableFields())
				{
					if (this.fields.ContainsKey(fieldInfo.Name))
					{
						Debug.LogErrorFormat("Fields with same names are not supported. Field name {0}", new object[]
						{
							fieldInfo.Name
						});
					}
					else
					{
						bool isArray = fieldInfo.FieldType.IsArray;
						Type type2 = fieldInfo.FieldType;
						if (isArray)
						{
							type2 = type2.GetElementType();
						}
						if (type2.IsSubclassOf(typeof(UnityEngine.Object)) || type2 == typeof(UnityEngine.Object))
						{
							if (isArray)
							{
								UnityEngine.Object[] array = (UnityEngine.Object[])fieldInfo.GetValue(obj);
								if (array != null)
								{
									long[] array2 = new long[array.Length];
									for (int j = 0; j < array2.Length; j++)
									{
										array2[j] = array[j].GetMappedInstanceID();
									}
									this.fields.Add(fieldInfo.Name, new DataContract(PrimitiveContract.Create<long[]>(array2)));
								}
								else
								{
									this.fields.Add(fieldInfo.Name, new DataContract(PrimitiveContract.Create<long[]>(new long[0])));
								}
							}
							else
							{
								UnityEngine.Object obj2 = (UnityEngine.Object)fieldInfo.GetValue(obj);
								this.fields.Add(fieldInfo.Name, new DataContract(PrimitiveContract.Create<long>(obj2.GetMappedInstanceID())));
							}
						}
						else if (type2.IsSubclassOf(typeof(UnityEventBase)))
						{
							UnityEventBase unityEventBase = (UnityEventBase)fieldInfo.GetValue(obj);
							if (unityEventBase != null)
							{
								PersistentUnityEventBase persistentUnityEventBase = new PersistentUnityEventBase();
								persistentUnityEventBase.ReadFrom(unityEventBase);
								this.fields.Add(fieldInfo.Name, new DataContract(persistentUnityEventBase));
							}
						}
						else if (!fieldInfo.FieldType.IsEnum())
						{
							object value = fieldInfo.GetValue(obj);
							if (typeof(IEnumerable).IsAssignableFrom(fieldInfo.FieldType))
							{
								IEnumerable enumerable = (IEnumerable)value;
								IEnumerator enumerator = enumerable.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										object obj3 = enumerator.Current;
										if (obj3 is IRTSerializable)
										{
											IRTSerializable irtserializable = (IRTSerializable)obj3;
											irtserializable.Serialize();
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
							}
							else if (value is IRTSerializable)
							{
								IRTSerializable irtserializable2 = (IRTSerializable)value;
								irtserializable2.Serialize();
							}
							if (fieldInfo.FieldType.IsPrimitive() || fieldInfo.FieldType.IsArray())
							{
								PrimitiveContract primitiveContract = PrimitiveContract.Create(fieldInfo.FieldType);
								primitiveContract.ValueBase = value;
								this.fields.Add(fieldInfo.Name, new DataContract(primitiveContract));
							}
							else
							{
								this.fields.Add(fieldInfo.Name, new DataContract
								{
									Data = value
								});
							}
						}
						else
						{
							PrimitiveContract primitiveContract2 = PrimitiveContract.Create(typeof(uint));
							primitiveContract2.ValueBase = (uint)Convert.ChangeType(fieldInfo.GetValue(obj), typeof(uint));
							this.fields.Add(fieldInfo.Name, new DataContract(primitiveContract2));
						}
					}
				}
				type = type.BaseType();
			}
			while (type.IsScript());
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x000426A0 File Offset: 0x00040AA0
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			base.ReadFrom(obj);
			if (this.baseObjectData != null)
			{
				this.baseObjectData.GetDependencies(obj, dependencies);
			}
			Type type = obj.GetType();
			if (!type.IsScript())
			{
				throw new ArgumentException(string.Format("obj of type {0} is not subclass of {1}", type, typeof(MonoBehaviour)), "obj");
			}
			do
			{
				foreach (FieldInfo fieldInfo in type.GetSerializableFields())
				{
					bool isArray = fieldInfo.FieldType.IsArray;
					Type type2 = fieldInfo.FieldType;
					if (isArray)
					{
						type2 = type2.GetElementType();
					}
					if (type2.IsSubclassOf(typeof(UnityEngine.Object)) || type2 == typeof(UnityEngine.Object))
					{
						if (isArray)
						{
							UnityEngine.Object[] array = (UnityEngine.Object[])fieldInfo.GetValue(obj);
							if (array != null)
							{
								base.AddDependencies(array, dependencies);
							}
						}
						else
						{
							UnityEngine.Object obj2 = (UnityEngine.Object)fieldInfo.GetValue(obj);
							base.AddDependency(obj2, dependencies);
						}
					}
					else if (type2.IsSubclassOf(typeof(UnityEventBase)))
					{
						UnityEventBase unityEventBase = (UnityEventBase)fieldInfo.GetValue(obj);
						if (unityEventBase != null)
						{
							PersistentUnityEventBase persistentUnityEventBase = new PersistentUnityEventBase();
							persistentUnityEventBase.GetDependencies(unityEventBase, dependencies);
						}
					}
					else if (!fieldInfo.FieldType.IsEnum())
					{
						object value = fieldInfo.GetValue(obj);
						if (typeof(IEnumerable).IsAssignableFrom(fieldInfo.FieldType))
						{
							IEnumerable enumerable = (IEnumerable)value;
							IEnumerator enumerator = enumerable.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									object obj3 = enumerator.Current;
									if (obj3 is IRTSerializable)
									{
										IRTSerializable irtserializable = (IRTSerializable)obj3;
										irtserializable.GetDependencies(dependencies);
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
						}
						else if (value is IRTSerializable)
						{
							IRTSerializable irtserializable2 = (IRTSerializable)value;
							irtserializable2.GetDependencies(dependencies);
						}
					}
				}
				type = type.BaseType();
			}
			while (type.IsScript());
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x000428D4 File Offset: 0x00040CD4
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			if (this.baseObjectData != null)
			{
				this.baseObjectData.FindDependencies<T>(dependencies, objects, allowNulls);
			}
			Type type = Type.GetType(this.TypeName);
			if (type == null)
			{
				Debug.LogWarning(this.TypeName + " is not found");
				return;
			}
			if (!type.IsScript())
			{
				throw new ArgumentException(string.Format("obj of type {0} is not subclass of {1}", type, typeof(MonoBehaviour)), "obj");
			}
			do
			{
				FieldInfo[] serializableFields = type.GetSerializableFields();
				int i = 0;
				while (i < serializableFields.Length)
				{
					FieldInfo fieldInfo = serializableFields[i];
					string key = fieldInfo.Name;
					if (this.fields.ContainsKey(fieldInfo.Name))
					{
						goto IL_EE;
					}
					FormerlySerializedAsAttribute formerlySerializedAsAttribute = fieldInfo.GetCustomAttributes(typeof(FormerlySerializedAsAttribute), false).FirstOrDefault<object>() as FormerlySerializedAsAttribute;
					if (formerlySerializedAsAttribute != null)
					{
						key = formerlySerializedAsAttribute.oldName;
						if (this.fields.ContainsKey(key))
						{
							goto IL_EE;
						}
					}
					IL_341:
					i++;
					continue;
					IL_EE:
					bool isArray = fieldInfo.FieldType.IsArray;
					Type type2 = fieldInfo.FieldType;
					if (isArray)
					{
						type2 = type2.GetElementType();
					}
					DataContract dataContract = this.fields[key];
					if (type2.IsSubclassOf(typeof(UnityEngine.Object)) || type2 == typeof(UnityEngine.Object))
					{
						if (isArray)
						{
							if (dataContract.AsPrimitive.ValueBase is long[])
							{
								long[] ids = (long[])dataContract.AsPrimitive.ValueBase;
								base.AddDependencies<T>(ids, dependencies, objects, allowNulls);
							}
						}
						else if (dataContract.AsPrimitive.ValueBase is long)
						{
							long id = (long)dataContract.AsPrimitive.ValueBase;
							base.AddDependency<T>(id, dependencies, objects, allowNulls);
						}
						goto IL_341;
					}
					if (dataContract.Data != null)
					{
						if (fieldInfo.FieldType.IsSubclassOf(typeof(UnityEventBase)))
						{
							PersistentUnityEventBase asUnityEvent = dataContract.AsUnityEvent;
							asUnityEvent.FindDependencies<T>(dependencies, objects, allowNulls);
						}
						goto IL_341;
					}
					if (fieldInfo.FieldType.IsEnum())
					{
						goto IL_341;
					}
					object obj;
					if (fieldInfo.FieldType.IsPrimitive() || fieldInfo.FieldType.IsArray())
					{
						PrimitiveContract asPrimitive = dataContract.AsPrimitive;
						if (asPrimitive == null || (asPrimitive.ValueBase == null && fieldInfo.FieldType.IsValueType()) || (asPrimitive.ValueBase != null && !fieldInfo.FieldType.IsAssignableFrom(asPrimitive.ValueBase.GetType())))
						{
							goto IL_341;
						}
						obj = asPrimitive.ValueBase;
					}
					else
					{
						obj = dataContract.Data;
					}
					if (obj is IEnumerable)
					{
						IEnumerable enumerable = (IEnumerable)obj;
						IEnumerator enumerator = enumerable.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								object obj2 = enumerator.Current;
								if (obj2 is IRTSerializable)
								{
									IRTSerializable irtserializable = (IRTSerializable)obj2;
									irtserializable.FindDependencies<T>(dependencies, objects, allowNulls);
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
						goto IL_341;
					}
					if (obj is IRTSerializable)
					{
						IRTSerializable irtserializable2 = (IRTSerializable)obj;
						irtserializable2.FindDependencies<T>(dependencies, objects, allowNulls);
						goto IL_341;
					}
					goto IL_341;
				}
				type = type.BaseType();
			}
			while (type.IsScript());
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00042C54 File Offset: 0x00041054
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (this.baseObjectData != null)
			{
				PersistentObject asPersistentObject = this.baseObjectData.AsPersistentObject;
				if (asPersistentObject != null)
				{
					asPersistentObject.name = this.name;
				}
				this.baseObjectData.WriteTo(obj, objects);
			}
			Type type = obj.GetType();
			if (!type.IsScript())
			{
				throw new ArgumentException(string.Format("obj of type {0} is not subclass of {1}", type, typeof(MonoBehaviour)), "obj");
			}
			do
			{
				FieldInfo[] serializableFields = type.GetSerializableFields();
				int i = 0;
				while (i < serializableFields.Length)
				{
					FieldInfo fieldInfo = serializableFields[i];
					string key = fieldInfo.Name;
					if (this.fields.ContainsKey(fieldInfo.Name))
					{
						goto IL_F0;
					}
					FormerlySerializedAsAttribute formerlySerializedAsAttribute = fieldInfo.GetCustomAttributes(typeof(FormerlySerializedAsAttribute), false).FirstOrDefault<object>() as FormerlySerializedAsAttribute;
					if (formerlySerializedAsAttribute != null)
					{
						key = formerlySerializedAsAttribute.oldName;
						if (this.fields.ContainsKey(key))
						{
							goto IL_F0;
						}
					}
					IL_4AA:
					i++;
					continue;
					IL_F0:
					bool isArray = fieldInfo.FieldType.IsArray;
					Type type2 = fieldInfo.FieldType;
					if (isArray)
					{
						type2 = type2.GetElementType();
					}
					DataContract dataContract = this.fields[key];
					if (type2.IsSubclassOf(typeof(UnityEngine.Object)) || type2 == typeof(UnityEngine.Object))
					{
						if (isArray)
						{
							if (dataContract.AsPrimitive != null)
							{
								if (dataContract.AsPrimitive.ValueBase is long[])
								{
									long[] array = (long[])dataContract.AsPrimitive.ValueBase;
									Array array2 = Array.CreateInstance(type2, array.Length);
									for (int j = 0; j < array.Length; j++)
									{
										object value = objects.Get(array[j]);
										array2.SetValue(value, j);
									}
									fieldInfo.SetValue(obj, array2);
								}
							}
						}
						else if (dataContract.AsPrimitive != null)
						{
							if (dataContract.AsPrimitive.ValueBase is long)
							{
								long num = (long)dataContract.AsPrimitive.ValueBase;
								if (objects.ContainsKey(num))
								{
									object value2 = objects[num];
									try
									{
										fieldInfo.SetValue(obj, value2);
									}
									catch
									{
										Debug.LogError(num);
										throw;
									}
								}
							}
						}
						goto IL_4AA;
					}
					if (dataContract == null)
					{
						if (fieldInfo.FieldType.IsValueType())
						{
							goto IL_4AA;
						}
						fieldInfo.SetValue(obj, dataContract);
						goto IL_4AA;
					}
					else if (fieldInfo.FieldType.IsSubclassOf(typeof(UnityEventBase)))
					{
						if (dataContract.AsUnityEvent == null)
						{
							goto IL_4AA;
						}
						PersistentUnityEventBase asUnityEvent = dataContract.AsUnityEvent;
						UnityEventBase unityEventBase = (UnityEventBase)Activator.CreateInstance(fieldInfo.FieldType);
						asUnityEvent.WriteTo(unityEventBase, objects);
						fieldInfo.SetValue(obj, unityEventBase);
						goto IL_4AA;
					}
					else
					{
						if (!fieldInfo.FieldType.IsEnum())
						{
							object obj2;
							if (fieldInfo.FieldType.IsPrimitive() || fieldInfo.FieldType.IsArray())
							{
								PrimitiveContract asPrimitive = dataContract.AsPrimitive;
								if (asPrimitive == null || (asPrimitive.ValueBase == null && fieldInfo.FieldType.IsValueType()) || (asPrimitive.ValueBase != null && !fieldInfo.FieldType.IsAssignableFrom(asPrimitive.ValueBase.GetType())))
								{
									goto IL_4AA;
								}
								obj2 = asPrimitive.ValueBase;
							}
							else
							{
								obj2 = dataContract.Data;
							}
							fieldInfo.SetValue(obj, obj2);
							if (obj2 is IEnumerable)
							{
								IEnumerable enumerable = (IEnumerable)obj2;
								IEnumerator enumerator = enumerable.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										object obj3 = enumerator.Current;
										if (obj3 is IRTSerializable)
										{
											IRTSerializable irtserializable = (IRTSerializable)obj3;
											irtserializable.Deserialize(objects);
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
							}
							else if (obj2 is IRTSerializable)
							{
								IRTSerializable irtserializable2 = (IRTSerializable)obj2;
								irtserializable2.Deserialize(objects);
							}
							goto IL_4AA;
						}
						PrimitiveContract asPrimitive2 = dataContract.AsPrimitive;
						if (asPrimitive2 == null || (asPrimitive2.ValueBase == null && fieldInfo.FieldType.IsValueType()) || (asPrimitive2.ValueBase != null && asPrimitive2.ValueBase.GetType() != typeof(uint)))
						{
							goto IL_4AA;
						}
						object value3 = Enum.ToObject(fieldInfo.FieldType, asPrimitive2.ValueBase);
						fieldInfo.SetValue(obj, value3);
						goto IL_4AA;
					}
				}
				type = type.BaseType();
			}
			while (type.IsScript());
			return obj;
		}

		// Token: 0x04000BF9 RID: 3065
		public PersistentData baseObjectData;

		// Token: 0x04000BFA RID: 3066
		public Dictionary<string, DataContract> fields = new Dictionary<string, DataContract>();

		// Token: 0x04000BFB RID: 3067
		public string TypeName;
	}
}
