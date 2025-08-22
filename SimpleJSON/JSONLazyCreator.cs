using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000D02 RID: 3330
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x06006593 RID: 26003 RVA: 0x00265367 File Offset: 0x00263767
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06006594 RID: 26004 RVA: 0x0026537D File Offset: 0x0026377D
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x06006595 RID: 26005 RVA: 0x00265393 File Offset: 0x00263793
		private void Set(JSONNode aVal)
		{
			if (this.m_Key == null)
			{
				this.m_Node.Add(aVal);
			}
			else
			{
				this.m_Node.Add(this.m_Key, aVal);
			}
			this.m_Node = null;
		}

		// Token: 0x17000EFA RID: 3834
		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.Set(new JSONArray
				{
					value
				});
			}
		}

		// Token: 0x17000EFB RID: 3835
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				this.Set(new JSONClass
				{
					{
						aKey,
						value
					}
				});
			}
		}

		// Token: 0x0600659A RID: 26010 RVA: 0x00265424 File Offset: 0x00263824
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray
			{
				aItem
			});
		}

		// Token: 0x0600659B RID: 26011 RVA: 0x00265448 File Offset: 0x00263848
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONClass
			{
				{
					aKey,
					aItem
				}
			});
		}

		// Token: 0x0600659C RID: 26012 RVA: 0x0026546A File Offset: 0x0026386A
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || object.ReferenceEquals(a, b);
		}

		// Token: 0x0600659D RID: 26013 RVA: 0x0026547B File Offset: 0x0026387B
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x0600659E RID: 26014 RVA: 0x00265487 File Offset: 0x00263887
		public override bool Equals(object obj)
		{
			return obj == null || object.ReferenceEquals(this, obj);
		}

		// Token: 0x0600659F RID: 26015 RVA: 0x00265498 File Offset: 0x00263898
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060065A0 RID: 26016 RVA: 0x002654A0 File Offset: 0x002638A0
		public override string ToString()
		{
			return string.Empty;
		}

		// Token: 0x060065A1 RID: 26017 RVA: 0x002654A7 File Offset: 0x002638A7
		public override string ToString(string aPrefix)
		{
			return string.Empty;
		}

		// Token: 0x060065A2 RID: 26018 RVA: 0x002654AE File Offset: 0x002638AE
		public override void ToString(string aPrefix, StringBuilder sb)
		{
		}

		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x060065A3 RID: 26019 RVA: 0x002654B0 File Offset: 0x002638B0
		// (set) Token: 0x060065A4 RID: 26020 RVA: 0x002654CC File Offset: 0x002638CC
		public override int AsInt
		{
			get
			{
				JSONData aVal = new JSONData(0);
				this.Set(aVal);
				return 0;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x060065A5 RID: 26021 RVA: 0x002654E8 File Offset: 0x002638E8
		// (set) Token: 0x060065A6 RID: 26022 RVA: 0x0026550C File Offset: 0x0026390C
		public override float AsFloat
		{
			get
			{
				JSONData aVal = new JSONData(0f);
				this.Set(aVal);
				return 0f;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x060065A7 RID: 26023 RVA: 0x00265528 File Offset: 0x00263928
		// (set) Token: 0x060065A8 RID: 26024 RVA: 0x00265554 File Offset: 0x00263954
		public override double AsDouble
		{
			get
			{
				JSONData aVal = new JSONData(0.0);
				this.Set(aVal);
				return 0.0;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x060065A9 RID: 26025 RVA: 0x00265570 File Offset: 0x00263970
		// (set) Token: 0x060065AA RID: 26026 RVA: 0x0026558C File Offset: 0x0026398C
		public override bool AsBool
		{
			get
			{
				JSONData aVal = new JSONData(false);
				this.Set(aVal);
				return false;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x060065AB RID: 26027 RVA: 0x002655A8 File Offset: 0x002639A8
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x060065AC RID: 26028 RVA: 0x002655C4 File Offset: 0x002639C4
		public override JSONClass AsObject
		{
			get
			{
				JSONClass jsonclass = new JSONClass();
				this.Set(jsonclass);
				return jsonclass;
			}
		}

		// Token: 0x04005507 RID: 21767
		private JSONNode m_Node;

		// Token: 0x04005508 RID: 21768
		private string m_Key;
	}
}
