using System;
using System.IO;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000D01 RID: 3329
	public class JSONData : JSONNode
	{
		// Token: 0x06006588 RID: 25992 RVA: 0x002651A7 File Offset: 0x002635A7
		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06006589 RID: 25993 RVA: 0x002651B6 File Offset: 0x002635B6
		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		// Token: 0x0600658A RID: 25994 RVA: 0x002651C5 File Offset: 0x002635C5
		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		// Token: 0x0600658B RID: 25995 RVA: 0x002651D4 File Offset: 0x002635D4
		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		// Token: 0x0600658C RID: 25996 RVA: 0x002651E3 File Offset: 0x002635E3
		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x0600658D RID: 25997 RVA: 0x002651F2 File Offset: 0x002635F2
		// (set) Token: 0x0600658E RID: 25998 RVA: 0x002651FA File Offset: 0x002635FA
		public override string Value
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x0600658F RID: 25999 RVA: 0x00265203 File Offset: 0x00263603
		public override string ToString()
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06006590 RID: 26000 RVA: 0x0026521F File Offset: 0x0026361F
		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06006591 RID: 26001 RVA: 0x0026523B File Offset: 0x0026363B
		public override void ToString(string aPrefix, StringBuilder sb)
		{
			sb.Append("\"");
			JSONNode.EscapeAppend(this.m_Data, sb);
			sb.Append("\"");
		}

		// Token: 0x06006592 RID: 26002 RVA: 0x00265264 File Offset: 0x00263664
		public override void Serialize(BinaryWriter aWriter)
		{
			JSONData jsondata = new JSONData(string.Empty);
			jsondata.AsInt = this.AsInt;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(4);
				aWriter.Write(this.AsInt);
				return;
			}
			jsondata.AsFloat = this.AsFloat;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(7);
				aWriter.Write(this.AsFloat);
				return;
			}
			jsondata.AsDouble = this.AsDouble;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(5);
				aWriter.Write(this.AsDouble);
				return;
			}
			jsondata.AsBool = this.AsBool;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(6);
				aWriter.Write(this.AsBool);
				return;
			}
			aWriter.Write(3);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x04005506 RID: 21766
		private string m_Data;
	}
}
