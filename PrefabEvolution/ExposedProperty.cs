using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PrefabEvolution
{
	// Token: 0x02000404 RID: 1028
	[Serializable]
	public class ExposedProperty : BaseExposedData
	{
		// Token: 0x06001A2A RID: 6698 RVA: 0x00092AF8 File Offset: 0x00090EF8
		public ExposedProperty()
		{
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x00092B00 File Offset: 0x00090F00
		public override BaseExposedData Clone()
		{
			ExposedProperty exposedProperty = (ExposedProperty)base.Clone();
			exposedProperty.Target = this.Target;
			exposedProperty.PropertyPath = this.PropertyPath;
			return exposedProperty;
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x00092B32 File Offset: 0x00090F32
		public override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();
			this._invocationChain = null;
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06001A2D RID: 6701 RVA: 0x00092B44 File Offset: 0x00090F44
		private ExposedProperty.PropertyInvocationChain invocationChain
		{
			get
			{
				if (this._invocationChain == null)
				{
					this._invocationChain = new ExposedProperty.PropertyInvocationChain(this.Target, this.PropertyPath);
					try
					{
						this.Value = this.Value;
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
						this._invocationChain.members = null;
					}
				}
				return this._invocationChain;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001A2E RID: 6702 RVA: 0x00092BB4 File Offset: 0x00090FB4
		public bool IsValid
		{
			get
			{
				return this.invocationChain.isValid;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001A2F RID: 6703 RVA: 0x00092BC4 File Offset: 0x00090FC4
		// (set) Token: 0x06001A30 RID: 6704 RVA: 0x00092C1C File Offset: 0x0009101C
		public object Value
		{
			get
			{
				if (!this.IsValid)
				{
					Debug.LogWarning(string.Concat(new object[]
					{
						"Trying to get value from invalid prefab property. Target:",
						this.Target,
						" Property path:",
						this.PropertyPath
					}));
					return null;
				}
				return this.invocationChain.value;
			}
			set
			{
				if (!this.IsValid)
				{
					Debug.LogWarning(string.Concat(new object[]
					{
						"Trying to set value to invalid prefab property. [Target:",
						this.Target,
						", Property path:",
						this.PropertyPath,
						"]"
					}));
					return;
				}
				this.invocationChain.value = value;
			}
		}

		// Token: 0x0400152C RID: 5420
		public UnityEngine.Object Target;

		// Token: 0x0400152D RID: 5421
		public string PropertyPath;

		// Token: 0x0400152E RID: 5422
		private ExposedProperty.PropertyInvocationChain _invocationChain;

		// Token: 0x02000405 RID: 1029
		public class PropertyInvocationChain
		{
			// Token: 0x06001A31 RID: 6705 RVA: 0x00092C7B File Offset: 0x0009107B
			public PropertyInvocationChain(object root, string path)
			{
				this.root = root;
				this.path = path;
				ExposedProperty.PropertyInvocationChain.GetInstance(root, path, out this.members);
			}

			// Token: 0x170002EB RID: 747
			// (get) Token: 0x06001A32 RID: 6706 RVA: 0x00092CA0 File Offset: 0x000910A0
			// (set) Token: 0x06001A33 RID: 6707 RVA: 0x00092CEC File Offset: 0x000910EC
			public object value
			{
				get
				{
					object value = this.root;
					ExposedProperty.PropertyInvocationChain.GetValidFieldName(ref value, this.path);
					for (int i = 0; i < this.members.Length; i++)
					{
						value = this.members[i].GetValue(value);
					}
					return value;
				}
				set
				{
					object value2 = this.root;
					ExposedProperty.PropertyInvocationChain.GetValidFieldName(ref value2, this.path);
					for (int i = 0; i < this.members.Length - 1; i++)
					{
						value2 = this.members[i].GetValue(value2);
					}
					this.members[this.members.Length - 1].SetValue(value2, value);
					for (int j = this.members.Length - 2; j >= 0; j--)
					{
						ExposedProperty.PropertyInvocationChain.InvokeInfo invokeInfo = this.members[j];
						ExposedProperty.PropertyInvocationChain.InvokeInfo invokeInfo2 = this.members[j + 1];
						if (invokeInfo.member.MemberType == MemberTypes.Property || invokeInfo.valueType.IsValueType || invokeInfo2.valueType.IsValueType)
						{
							invokeInfo.SetValue(invokeInfo2.tempTarget);
						}
					}
				}
			}

			// Token: 0x170002EC RID: 748
			// (get) Token: 0x06001A34 RID: 6708 RVA: 0x00092DC0 File Offset: 0x000911C0
			public bool isValid
			{
				get
				{
					return this.members != null;
				}
			}

			// Token: 0x06001A35 RID: 6709 RVA: 0x00092DD0 File Offset: 0x000911D0
			internal static object GetInstance(object obj, string path, out ExposedProperty.PropertyInvocationChain.InvokeInfo[] members)
			{
				path = path.Replace(".Array.data", string.Empty);
				string[] array = path.Split(new char[]
				{
					'.'
				});
				string[] array2 = array;
				object obj2 = obj;
				members = new ExposedProperty.PropertyInvocationChain.InvokeInfo[array2.Length];
				try
				{
					int num = 0;
					foreach (string text in array2)
					{
						members[num] = new ExposedProperty.PropertyInvocationChain.InvokeInfo();
						if (text.Contains("["))
						{
							string[] array4 = text.Split(new char[]
							{
								'[',
								']'
							});
							int index = int.Parse(array4[1]);
							members[num].index = index;
							obj2 = ExposedProperty.PropertyInvocationChain.getField(obj2, array4[0], out members[num].member, index);
						}
						else
						{
							obj2 = ExposedProperty.PropertyInvocationChain.getField(obj2, text, out members[num].member, -1);
						}
						PropertyInfo propertyInfo = members[num].member as PropertyInfo;
						FieldInfo fieldInfo = members[num].member as FieldInfo;
						if (fieldInfo != null)
						{
							members[num].valueType = fieldInfo.FieldType;
						}
						else if (propertyInfo != null)
						{
							members[num].valueType = propertyInfo.PropertyType;
						}
						num++;
					}
				}
				catch (Exception exception)
				{
					members = null;
					Debug.LogException(exception);
					return null;
				}
				return obj2;
			}

			// Token: 0x06001A36 RID: 6710 RVA: 0x00092F30 File Offset: 0x00091330
			private static object GetMemberValue(object target, MemberInfo member, int index = -1)
			{
				object obj = null;
				FieldInfo fieldInfo = member as FieldInfo;
				if (fieldInfo != null)
				{
					obj = fieldInfo.GetValue(target);
				}
				PropertyInfo propertyInfo = member as PropertyInfo;
				if (propertyInfo != null)
				{
					obj = propertyInfo.GetValue(target, null);
				}
				return (obj == null) ? null : ((index != -1) ? (obj as IList)[index] : obj);
			}

			// Token: 0x06001A37 RID: 6711 RVA: 0x00092F90 File Offset: 0x00091390
			private static void setValue(object target, MemberInfo member, object value, int index = -1)
			{
				if (index != -1)
				{
					target = ExposedProperty.PropertyInvocationChain.GetMemberValue(target, member, -1);
					(target as IList)[index] = value;
					return;
				}
				FieldInfo fieldInfo = member as FieldInfo;
				if (fieldInfo != null)
				{
					fieldInfo.SetValue(target, value);
				}
				PropertyInfo propertyInfo = member as PropertyInfo;
				if (propertyInfo != null)
				{
					propertyInfo.SetValue(target, value, null);
				}
			}

			// Token: 0x06001A38 RID: 6712 RVA: 0x00092FE8 File Offset: 0x000913E8
			public static string GetValidFieldName(ref object obj, string fieldName)
			{
				if (obj is Renderer && fieldName == "m_Materials")
				{
					return "sharedMaterials";
				}
				if (obj is MeshFilter && fieldName == "m_Mesh")
				{
					return "sharedMesh";
				}
				if (obj is GameObject && fieldName == "m_IsActive")
				{
					obj = new GameObjectWrapper(obj as GameObject);
				}
				return fieldName;
			}

			// Token: 0x06001A39 RID: 6713 RVA: 0x00093064 File Offset: 0x00091464
			private static object getField(object obj, string field, out MemberInfo member, int index = -1)
			{
				ExposedProperty.PropertyInvocationChain.<getField>c__AnonStorey0 <getField>c__AnonStorey = new ExposedProperty.PropertyInvocationChain.<getField>c__AnonStorey0();
				<getField>c__AnonStorey.field = field;
				member = null;
				object result;
				try
				{
					BindingFlags bindingAttr = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
					if (obj == null)
					{
						result = null;
					}
					else
					{
						<getField>c__AnonStorey.field = ExposedProperty.PropertyInvocationChain.GetValidFieldName(ref obj, <getField>c__AnonStorey.field);
						Type type = obj.GetType();
						member = type.GetField(<getField>c__AnonStorey.field, bindingAttr);
						if (member == null && <getField>c__AnonStorey.field.StartsWith("m_"))
						{
							member = type.GetField(<getField>c__AnonStorey.field.Remove(0, 2), bindingAttr);
						}
						member = type.GetProperty(<getField>c__AnonStorey.field, bindingAttr);
						if (member == null && <getField>c__AnonStorey.field.StartsWith("m_"))
						{
							member = type.GetProperty(<getField>c__AnonStorey.field.Remove(0, 2), bindingAttr);
						}
						if (member == null)
						{
							member = type.GetMembers(bindingAttr).First(new Func<MemberInfo, bool>(<getField>c__AnonStorey.<>m__0));
						}
						if (member != null)
						{
							result = ExposedProperty.PropertyInvocationChain.GetMemberValue(obj, member, index);
						}
						else
						{
							member = null;
							result = null;
						}
					}
				}
				catch (Exception)
				{
					member = null;
					result = null;
				}
				return result;
			}

			// Token: 0x0400152F RID: 5423
			public object root;

			// Token: 0x04001530 RID: 5424
			public string path;

			// Token: 0x04001531 RID: 5425
			public ExposedProperty.PropertyInvocationChain.InvokeInfo[] members;

			// Token: 0x02000406 RID: 1030
			public class InvokeInfo
			{
				// Token: 0x06001A3A RID: 6714 RVA: 0x0009318C File Offset: 0x0009158C
				public InvokeInfo()
				{
				}

				// Token: 0x06001A3B RID: 6715 RVA: 0x0009319B File Offset: 0x0009159B
				public object GetValue(object target)
				{
					this.tempTarget = target;
					return ExposedProperty.PropertyInvocationChain.GetMemberValue(target, this.member, this.index);
				}

				// Token: 0x06001A3C RID: 6716 RVA: 0x000931B6 File Offset: 0x000915B6
				public void SetValue(object target, object value)
				{
					this.tempTarget = target;
					ExposedProperty.PropertyInvocationChain.setValue(target, this.member, value, this.index);
				}

				// Token: 0x06001A3D RID: 6717 RVA: 0x000931D2 File Offset: 0x000915D2
				public void SetValue(object value)
				{
					ExposedProperty.PropertyInvocationChain.setValue(this.tempTarget, this.member, value, this.index);
				}

				// Token: 0x04001532 RID: 5426
				public MemberInfo member;

				// Token: 0x04001533 RID: 5427
				public int index = -1;

				// Token: 0x04001534 RID: 5428
				public object tempTarget;

				// Token: 0x04001535 RID: 5429
				public Type valueType;
			}

			// Token: 0x02000F58 RID: 3928
			[CompilerGenerated]
			private sealed class <getField>c__AnonStorey0
			{
				// Token: 0x06007390 RID: 29584 RVA: 0x000931EC File Offset: 0x000915EC
				public <getField>c__AnonStorey0()
				{
				}

				// Token: 0x06007391 RID: 29585 RVA: 0x000931F4 File Offset: 0x000915F4
				internal bool <>m__0(MemberInfo m)
				{
					return m.Name.ToUpper() == this.field.ToUpper();
				}

				// Token: 0x04006777 RID: 26487
				internal string field;
			}
		}
	}
}
