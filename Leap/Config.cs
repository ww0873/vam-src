using System;
using System.Collections.Generic;
using LeapInternal;

namespace Leap
{
	// Token: 0x020005B6 RID: 1462
	public class Config
	{
		// Token: 0x060024A6 RID: 9382 RVA: 0x000D3E1C File Offset: 0x000D221C
		public Config(int connectionKey)
		{
			this._connection = Connection.GetConnection(connectionKey);
			Connection connection = this._connection;
			connection.LeapConfigChange = (EventHandler<ConfigChangeEventArgs>)Delegate.Combine(connection.LeapConfigChange, new EventHandler<ConfigChangeEventArgs>(this.handleConfigChange));
			Connection connection2 = this._connection;
			connection2.LeapConfigResponse = (EventHandler<SetConfigResponseEventArgs>)Delegate.Combine(connection2.LeapConfigResponse, new EventHandler<SetConfigResponseEventArgs>(this.handleConfigResponse));
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x000D3E94 File Offset: 0x000D2294
		private void handleConfigChange(object sender, ConfigChangeEventArgs eventArgs)
		{
			object obj;
			if (this._transactions.TryGetValue(eventArgs.RequestId, out obj))
			{
				Action<bool> action = obj as Action<bool>;
				action(eventArgs.Succeeded);
				this._transactions.Remove(eventArgs.RequestId);
			}
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x000D3EE0 File Offset: 0x000D22E0
		private void handleConfigResponse(object sender, SetConfigResponseEventArgs eventArgs)
		{
			object obj = new object();
			if (this._transactions.TryGetValue(eventArgs.RequestId, out obj))
			{
				switch (eventArgs.DataType)
				{
				case Config.ValueType.TYPE_BOOLEAN:
				{
					Action<bool> action = obj as Action<bool>;
					action((int)eventArgs.Value != 0);
					break;
				}
				case Config.ValueType.TYPE_INT32:
				{
					Action<int> action2 = obj as Action<int>;
					action2((int)eventArgs.Value);
					break;
				}
				case Config.ValueType.TYPE_FLOAT:
				{
					Action<float> action3 = obj as Action<float>;
					action3((float)eventArgs.Value);
					break;
				}
				case Config.ValueType.TYPE_STRING:
				{
					Action<string> action4 = obj as Action<string>;
					action4((string)eventArgs.Value);
					break;
				}
				}
				this._transactions.Remove(eventArgs.RequestId);
			}
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x000D3FD4 File Offset: 0x000D23D4
		public bool Get<T>(string key, Action<T> onResult)
		{
			uint configValue = this._connection.GetConfigValue(key);
			if (configValue > 0U)
			{
				this._transactions.Add(configValue, onResult);
				return true;
			}
			return false;
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x000D4008 File Offset: 0x000D2408
		public bool Set<T>(string key, T value, Action<bool> onResult) where T : IConvertible
		{
			uint num = this._connection.SetConfigValue<T>(key, value);
			if (num > 0U)
			{
				this._transactions.Add(num, onResult);
				return true;
			}
			return false;
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000D403A File Offset: 0x000D243A
		[Obsolete("Use the generic Set<T> method instead.")]
		public Config.ValueType Type(string key)
		{
			return Config.ValueType.TYPE_UNKNOWN;
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x000D403D File Offset: 0x000D243D
		[Obsolete("Use the generic Get<T> method instead.")]
		public bool GetBool(string key)
		{
			return false;
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x000D4040 File Offset: 0x000D2440
		[Obsolete("Use the generic Set<T> method instead.")]
		public bool SetBool(string key, bool value)
		{
			return false;
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x000D4043 File Offset: 0x000D2443
		[Obsolete("Use the generic Get<T> method instead.")]
		public bool GetInt32(string key)
		{
			return false;
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x000D4046 File Offset: 0x000D2446
		[Obsolete("Use the generic Set<T> method instead.")]
		public bool SetInt32(string key, int value)
		{
			return false;
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x000D4049 File Offset: 0x000D2449
		[Obsolete("Use the generic Get<T> method instead.")]
		public bool GetFloat(string key)
		{
			return false;
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x000D404C File Offset: 0x000D244C
		[Obsolete("Use the generic Set<T> method instead.")]
		public bool SetFloat(string key, float value)
		{
			return false;
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x000D404F File Offset: 0x000D244F
		[Obsolete("Use the generic Get<T> method instead.")]
		public bool GetString(string key)
		{
			return false;
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000D4052 File Offset: 0x000D2452
		[Obsolete("Use the generic Set<T> method instead.")]
		public bool SetString(string key, string value)
		{
			return false;
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000D4055 File Offset: 0x000D2455
		[Obsolete]
		public bool Save()
		{
			return false;
		}

		// Token: 0x04001ED8 RID: 7896
		private Connection _connection;

		// Token: 0x04001ED9 RID: 7897
		private Dictionary<uint, object> _transactions = new Dictionary<uint, object>();

		// Token: 0x020005B7 RID: 1463
		public enum ValueType
		{
			// Token: 0x04001EDB RID: 7899
			TYPE_UNKNOWN,
			// Token: 0x04001EDC RID: 7900
			TYPE_BOOLEAN,
			// Token: 0x04001EDD RID: 7901
			TYPE_INT32,
			// Token: 0x04001EDE RID: 7902
			TYPE_FLOAT = 6,
			// Token: 0x04001EDF RID: 7903
			TYPE_STRING = 8
		}
	}
}
