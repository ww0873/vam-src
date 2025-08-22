using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace SimpleJSON
{
	// Token: 0x02000CFE RID: 3326
	public class JSONNode
	{
		// Token: 0x06006534 RID: 25908 RVA: 0x00263282 File Offset: 0x00261682
		public JSONNode()
		{
		}

		// Token: 0x06006535 RID: 25909 RVA: 0x0026328A File Offset: 0x0026168A
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x17000EE4 RID: 3812
		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000EE5 RID: 3813
		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x0600653A RID: 25914 RVA: 0x00263296 File Offset: 0x00261696
		// (set) Token: 0x0600653B RID: 25915 RVA: 0x0026329D File Offset: 0x0026169D
		public virtual string Value
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x0600653C RID: 25916 RVA: 0x0026329F File Offset: 0x0026169F
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600653D RID: 25917 RVA: 0x002632A2 File Offset: 0x002616A2
		public virtual void Add(JSONNode aItem)
		{
			this.Add(string.Empty, aItem);
		}

		// Token: 0x0600653E RID: 25918 RVA: 0x002632B0 File Offset: 0x002616B0
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x0600653F RID: 25919 RVA: 0x002632B3 File Offset: 0x002616B3
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x06006540 RID: 25920 RVA: 0x002632B6 File Offset: 0x002616B6
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x06006541 RID: 25921 RVA: 0x002632BC File Offset: 0x002616BC
		public virtual IEnumerable<JSONNode> Childs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06006542 RID: 25922 RVA: 0x002632D8 File Offset: 0x002616D8
		public IEnumerable<JSONNode> DeepChilds
		{
			get
			{
				foreach (JSONNode C in this.Childs)
				{
					foreach (JSONNode D in C.DeepChilds)
					{
						yield return D;
					}
				}
				yield break;
			}
		}

		// Token: 0x06006543 RID: 25923 RVA: 0x002632FB File Offset: 0x002616FB
		public override string ToString()
		{
			return "JSONNode";
		}

		// Token: 0x06006544 RID: 25924 RVA: 0x00263302 File Offset: 0x00261702
		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}

		// Token: 0x06006545 RID: 25925 RVA: 0x00263309 File Offset: 0x00261709
		public virtual void ToString(string aPrefix, StringBuilder sb)
		{
		}

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06006546 RID: 25926 RVA: 0x0026330C File Offset: 0x0026170C
		// (set) Token: 0x06006547 RID: 25927 RVA: 0x00263330 File Offset: 0x00261730
		public virtual int AsInt
		{
			get
			{
				int result = 0;
				if (int.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x06006548 RID: 25928 RVA: 0x00263348 File Offset: 0x00261748
		// (set) Token: 0x06006549 RID: 25929 RVA: 0x00263374 File Offset: 0x00261774
		public virtual float AsFloat
		{
			get
			{
				float result = 0f;
				if (float.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0f;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x0600654A RID: 25930 RVA: 0x0026338C File Offset: 0x0026178C
		// (set) Token: 0x0600654B RID: 25931 RVA: 0x002633C0 File Offset: 0x002617C0
		public virtual double AsDouble
		{
			get
			{
				double result = 0.0;
				if (double.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x0600654C RID: 25932 RVA: 0x002633D8 File Offset: 0x002617D8
		// (set) Token: 0x0600654D RID: 25933 RVA: 0x00263409 File Offset: 0x00261809
		public virtual bool AsBool
		{
			get
			{
				bool result = false;
				if (bool.TryParse(this.Value, out result))
				{
					return result;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = ((!value) ? "false" : "true");
			}
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x0600654E RID: 25934 RVA: 0x00263426 File Offset: 0x00261826
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x0600654F RID: 25935 RVA: 0x0026342E File Offset: 0x0026182E
		public virtual JSONClass AsObject
		{
			get
			{
				return this as JSONClass;
			}
		}

		// Token: 0x06006550 RID: 25936 RVA: 0x00263436 File Offset: 0x00261836
		public static implicit operator JSONNode(string s)
		{
			return new JSONData(s);
		}

		// Token: 0x06006551 RID: 25937 RVA: 0x0026343E File Offset: 0x0026183E
		public static implicit operator string(JSONNode d)
		{
			return (!(d == null)) ? d.Value : null;
		}

		// Token: 0x06006552 RID: 25938 RVA: 0x00263458 File Offset: 0x00261858
		public static bool operator ==(JSONNode a, object b)
		{
			return (b == null && a is JSONLazyCreator) || object.ReferenceEquals(a, b);
		}

		// Token: 0x06006553 RID: 25939 RVA: 0x00263474 File Offset: 0x00261874
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06006554 RID: 25940 RVA: 0x00263480 File Offset: 0x00261880
		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj);
		}

		// Token: 0x06006555 RID: 25941 RVA: 0x00263489 File Offset: 0x00261889
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06006556 RID: 25942 RVA: 0x00263494 File Offset: 0x00261894
		internal static string Escape(string aText)
		{
			if (JSONNode.escapeStringBuilder == null)
			{
				JSONNode.escapeStringBuilder = new StringBuilder(4096);
			}
			else
			{
				JSONNode.escapeStringBuilder.Length = 0;
			}
			if (aText != null)
			{
				foreach (char c in aText)
				{
					switch (c)
					{
					case '\b':
						JSONNode.escapeStringBuilder.Append("\\b");
						break;
					case '\t':
						JSONNode.escapeStringBuilder.Append("\\t");
						break;
					case '\n':
						JSONNode.escapeStringBuilder.Append("\\n");
						break;
					default:
						if (c != '"')
						{
							if (c != '\\')
							{
								JSONNode.escapeStringBuilder.Append(c);
							}
							else
							{
								JSONNode.escapeStringBuilder.Append("\\\\");
							}
						}
						else
						{
							JSONNode.escapeStringBuilder.Append("\\\"");
						}
						break;
					case '\f':
						JSONNode.escapeStringBuilder.Append("\\f");
						break;
					case '\r':
						JSONNode.escapeStringBuilder.Append("\\r");
						break;
					}
				}
			}
			return JSONNode.escapeStringBuilder.ToString();
		}

		// Token: 0x06006557 RID: 25943 RVA: 0x002635D4 File Offset: 0x002619D4
		internal static void EscapeAppend(string aText, StringBuilder sb)
		{
			if (aText != null)
			{
				foreach (char c in aText)
				{
					switch (c)
					{
					case '\b':
						sb.Append("\\b");
						break;
					case '\t':
						sb.Append("\\t");
						break;
					case '\n':
						sb.Append("\\n");
						break;
					default:
						if (c != '"')
						{
							if (c != '\\')
							{
								sb.Append(c);
							}
							else
							{
								sb.Append("\\\\");
							}
						}
						else
						{
							sb.Append("\\\"");
						}
						break;
					case '\f':
						sb.Append("\\f");
						break;
					case '\r':
						sb.Append("\\r");
						break;
					}
				}
			}
		}

		// Token: 0x06006558 RID: 25944 RVA: 0x002636C4 File Offset: 0x00261AC4
		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			bool flag2 = false;
			while (i < aJSON.Length)
			{
				char c = aJSON[i];
				switch (c)
				{
				case '\t':
					goto IL_37A;
				case '\n':
				case '\r':
					break;
				default:
					switch (c)
					{
					case '[':
						if (flag2)
						{
							stringBuilder.Append(aJSON[i]);
							goto IL_481;
						}
						stack.Push(new JSONArray());
						if (jsonnode != null)
						{
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(stack.Peek());
							}
							else if (stringBuilder2.Length > 0)
							{
								string text = stringBuilder2.ToString().Trim();
								if (text.Length > 0)
								{
									jsonnode.Add(text, stack.Peek());
								}
							}
						}
						stringBuilder2.Length = 0;
						stringBuilder.Length = 0;
						flag = false;
						jsonnode = stack.Peek();
						goto IL_481;
					case '\\':
						i++;
						if (flag2)
						{
							char c2 = aJSON[i];
							switch (c2)
							{
							case 'r':
								stringBuilder.Append('\r');
								break;
							default:
								if (c2 != 'b')
								{
									if (c2 != 'f')
									{
										if (c2 != 'n')
										{
											stringBuilder.Append(c2);
										}
										else
										{
											stringBuilder.Append('\n');
										}
									}
									else
									{
										stringBuilder.Append('\f');
									}
								}
								else
								{
									stringBuilder.Append('\b');
								}
								break;
							case 't':
								stringBuilder.Append('\t');
								break;
							case 'u':
							{
								string s = aJSON.Substring(i + 1, 4);
								stringBuilder.Append((char)int.Parse(s, NumberStyles.AllowHexSpecifier));
								i += 4;
								break;
							}
							}
						}
						goto IL_481;
					case ']':
						break;
					default:
						switch (c)
						{
						case ' ':
							goto IL_37A;
						default:
							switch (c)
							{
							case '{':
								if (flag2)
								{
									stringBuilder.Append(aJSON[i]);
									goto IL_481;
								}
								stack.Push(new JSONClass());
								if (jsonnode != null)
								{
									if (jsonnode is JSONArray)
									{
										jsonnode.Add(stack.Peek());
									}
									else if (stringBuilder2.Length > 0)
									{
										string text2 = stringBuilder2.ToString().Trim();
										if (text2.Length > 0)
										{
											jsonnode.Add(text2, stack.Peek());
										}
									}
								}
								stringBuilder2.Length = 0;
								stringBuilder.Length = 0;
								flag = false;
								jsonnode = stack.Peek();
								goto IL_481;
							default:
								if (c != ',')
								{
									if (c != ':')
									{
										stringBuilder.Append(aJSON[i]);
										flag = true;
										goto IL_481;
									}
									if (flag2)
									{
										stringBuilder.Append(aJSON[i]);
										goto IL_481;
									}
									stringBuilder2.Length = 0;
									stringBuilder2.Append(stringBuilder);
									stringBuilder.Length = 0;
									flag = false;
									goto IL_481;
								}
								else
								{
									if (flag2)
									{
										stringBuilder.Append(aJSON[i]);
										goto IL_481;
									}
									if (flag)
									{
										if (jsonnode is JSONArray)
										{
											jsonnode.Add(stringBuilder.ToString());
										}
										else if (stringBuilder2.Length > 0)
										{
											jsonnode.Add(stringBuilder2.ToString(), stringBuilder.ToString());
										}
									}
									stringBuilder2.Length = 0;
									stringBuilder.Length = 0;
									flag = false;
									goto IL_481;
								}
								break;
							case '}':
								break;
							}
							break;
						case '"':
							flag2 ^= true;
							flag = true;
							goto IL_481;
						}
						break;
					}
					if (flag2)
					{
						stringBuilder.Append(aJSON[i]);
					}
					else
					{
						if (stack.Count == 0)
						{
							throw new Exception("JSON Parse: Too many closing brackets");
						}
						stack.Pop();
						if (flag)
						{
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(stringBuilder.ToString());
							}
							else if (stringBuilder2.Length > 0)
							{
								string text3 = stringBuilder2.ToString().Trim();
								if (text3.Length > 0)
								{
									jsonnode.Add(text3, stringBuilder.ToString());
								}
							}
						}
						stringBuilder2.Length = 0;
						stringBuilder.Length = 0;
						flag = false;
						if (stack.Count > 0)
						{
							jsonnode = stack.Peek();
						}
					}
					break;
				}
				IL_481:
				i++;
				continue;
				IL_37A:
				if (flag2)
				{
					stringBuilder.Append(aJSON[i]);
				}
				goto IL_481;
			}
			if (flag2)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		// Token: 0x06006559 RID: 25945 RVA: 0x00263B75 File Offset: 0x00261F75
		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		// Token: 0x0600655A RID: 25946 RVA: 0x00263B78 File Offset: 0x00261F78
		public void SaveToStream(Stream aData)
		{
			BinaryWriter aWriter = new BinaryWriter(aData);
			this.Serialize(aWriter);
		}

		// Token: 0x0600655B RID: 25947 RVA: 0x00263B93 File Offset: 0x00261F93
		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600655C RID: 25948 RVA: 0x00263B9F File Offset: 0x00261F9F
		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600655D RID: 25949 RVA: 0x00263BAB File Offset: 0x00261FAB
		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600655E RID: 25950 RVA: 0x00263BB8 File Offset: 0x00261FB8
		public void SaveToFile(string aFileName)
		{
			Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
			using (FileStream fileStream = File.OpenWrite(aFileName))
			{
				this.SaveToStream(fileStream);
			}
		}

		// Token: 0x0600655F RID: 25951 RVA: 0x00263C0C File Offset: 0x0026200C
		public string SaveToBase64()
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.SaveToStream(memoryStream);
				memoryStream.Position = 0L;
				result = Convert.ToBase64String(memoryStream.ToArray());
			}
			return result;
		}

		// Token: 0x06006560 RID: 25952 RVA: 0x00263C60 File Offset: 0x00262060
		public static JSONNode Deserialize(BinaryReader aReader)
		{
			JSONBinaryTag jsonbinaryTag = (JSONBinaryTag)aReader.ReadByte();
			switch (jsonbinaryTag)
			{
			case JSONBinaryTag.Array:
			{
				int num = aReader.ReadInt32();
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < num; i++)
				{
					jsonarray.Add(JSONNode.Deserialize(aReader));
				}
				return jsonarray;
			}
			case JSONBinaryTag.Class:
			{
				int num2 = aReader.ReadInt32();
				JSONClass jsonclass = new JSONClass();
				for (int j = 0; j < num2; j++)
				{
					string aKey = aReader.ReadString();
					JSONNode aItem = JSONNode.Deserialize(aReader);
					jsonclass.Add(aKey, aItem);
				}
				return jsonclass;
			}
			case JSONBinaryTag.Value:
				return new JSONData(aReader.ReadString());
			case JSONBinaryTag.IntValue:
				return new JSONData(aReader.ReadInt32());
			case JSONBinaryTag.DoubleValue:
				return new JSONData(aReader.ReadDouble());
			case JSONBinaryTag.BoolValue:
				return new JSONData(aReader.ReadBoolean());
			case JSONBinaryTag.FloatValue:
				return new JSONData(aReader.ReadSingle());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jsonbinaryTag);
			}
		}

		// Token: 0x06006561 RID: 25953 RVA: 0x00263D5F File Offset: 0x0026215F
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06006562 RID: 25954 RVA: 0x00263D6B File Offset: 0x0026216B
		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06006563 RID: 25955 RVA: 0x00263D77 File Offset: 0x00262177
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06006564 RID: 25956 RVA: 0x00263D84 File Offset: 0x00262184
		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode result;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				result = JSONNode.Deserialize(binaryReader);
			}
			return result;
		}

		// Token: 0x06006565 RID: 25957 RVA: 0x00263DC4 File Offset: 0x002621C4
		public static JSONNode LoadFromFile(string aFileName)
		{
			JSONNode result;
			using (FileStream fileStream = File.OpenRead(aFileName))
			{
				result = JSONNode.LoadFromStream(fileStream);
			}
			return result;
		}

		// Token: 0x06006566 RID: 25958 RVA: 0x00263E04 File Offset: 0x00262204
		public static JSONNode LoadFromBase64(string aBase64)
		{
			byte[] buffer = Convert.FromBase64String(aBase64);
			return JSONNode.LoadFromStream(new MemoryStream(buffer)
			{
				Position = 0L
			});
		}

		// Token: 0x04005503 RID: 21763
		internal static StringBuilder escapeStringBuilder;

		// Token: 0x0200102B RID: 4139
		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<JSONNode>, IEnumerator, IDisposable, IEnumerator<JSONNode>
		{
			// Token: 0x06007729 RID: 30505 RVA: 0x00263E2D File Offset: 0x0026222D
			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			// Token: 0x0600772A RID: 30506 RVA: 0x00263E35 File Offset: 0x00262235
			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
				}
				return false;
			}

			// Token: 0x170011AD RID: 4525
			// (get) Token: 0x0600772B RID: 30507 RVA: 0x00263E4F File Offset: 0x0026224F
			JSONNode IEnumerator<JSONNode>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011AE RID: 4526
			// (get) Token: 0x0600772C RID: 30508 RVA: 0x00263E57 File Offset: 0x00262257
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600772D RID: 30509 RVA: 0x00263E5F File Offset: 0x0026225F
			[DebuggerHidden]
			public void Dispose()
			{
			}

			// Token: 0x0600772E RID: 30510 RVA: 0x00263E61 File Offset: 0x00262261
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600772F RID: 30511 RVA: 0x00263E68 File Offset: 0x00262268
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<SimpleJSON.JSONNode>.GetEnumerator();
			}

			// Token: 0x06007730 RID: 30512 RVA: 0x00263E70 File Offset: 0x00262270
			[DebuggerHidden]
			IEnumerator<JSONNode> IEnumerable<JSONNode>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new JSONNode.<>c__Iterator0();
			}

			// Token: 0x04006B3A RID: 27450
			internal JSONNode $current;

			// Token: 0x04006B3B RID: 27451
			internal bool $disposing;

			// Token: 0x04006B3C RID: 27452
			internal int $PC;
		}

		// Token: 0x0200102C RID: 4140
		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<JSONNode>, IEnumerator, IDisposable, IEnumerator<JSONNode>
		{
			// Token: 0x06007731 RID: 30513 RVA: 0x00263E8B File Offset: 0x0026228B
			[DebuggerHidden]
			public <>c__Iterator1()
			{
			}

			// Token: 0x06007732 RID: 30514 RVA: 0x00263E94 File Offset: 0x00262294
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					enumerator = this.Childs.GetEnumerator();
					num = 4294967293U;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1U:
						Block_4:
						try
						{
							switch (num)
							{
							}
							if (enumerator2.MoveNext())
							{
								D = enumerator2.Current;
								this.$current = D;
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
								flag = true;
								return true;
							}
						}
						finally
						{
							if (!flag)
							{
								if (enumerator2 != null)
								{
									enumerator2.Dispose();
								}
							}
						}
						break;
					}
					if (enumerator.MoveNext())
					{
						C = enumerator.Current;
						enumerator2 = C.DeepChilds.GetEnumerator();
						num = 4294967293U;
						goto Block_4;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170011AF RID: 4527
			// (get) Token: 0x06007733 RID: 30515 RVA: 0x00263FE4 File Offset: 0x002623E4
			JSONNode IEnumerator<JSONNode>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011B0 RID: 4528
			// (get) Token: 0x06007734 RID: 30516 RVA: 0x00263FEC File Offset: 0x002623EC
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007735 RID: 30517 RVA: 0x00263FF4 File Offset: 0x002623F4
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1U:
					try
					{
						try
						{
						}
						finally
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			// Token: 0x06007736 RID: 30518 RVA: 0x00264080 File Offset: 0x00262480
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06007737 RID: 30519 RVA: 0x00264087 File Offset: 0x00262487
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<SimpleJSON.JSONNode>.GetEnumerator();
			}

			// Token: 0x06007738 RID: 30520 RVA: 0x00264090 File Offset: 0x00262490
			[DebuggerHidden]
			IEnumerator<JSONNode> IEnumerable<JSONNode>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JSONNode.<>c__Iterator1 <>c__Iterator = new JSONNode.<>c__Iterator1();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}

			// Token: 0x04006B3D RID: 27453
			internal IEnumerator<JSONNode> $locvar0;

			// Token: 0x04006B3E RID: 27454
			internal JSONNode <C>__1;

			// Token: 0x04006B3F RID: 27455
			internal IEnumerator<JSONNode> $locvar1;

			// Token: 0x04006B40 RID: 27456
			internal JSONNode <D>__2;

			// Token: 0x04006B41 RID: 27457
			internal JSONNode $this;

			// Token: 0x04006B42 RID: 27458
			internal JSONNode $current;

			// Token: 0x04006B43 RID: 27459
			internal bool $disposing;

			// Token: 0x04006B44 RID: 27460
			internal int $PC;
		}
	}
}
