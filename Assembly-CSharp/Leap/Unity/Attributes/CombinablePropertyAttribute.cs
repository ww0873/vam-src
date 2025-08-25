using System;
using System.Reflection;
using UnityEngine;

namespace Leap.Unity.Attributes
{
	// Token: 0x02000678 RID: 1656
	public abstract class CombinablePropertyAttribute : PropertyAttribute
	{
		// Token: 0x06002849 RID: 10313 RVA: 0x000DE84F File Offset: 0x000DCC4F
		protected CombinablePropertyAttribute()
		{
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x0600284A RID: 10314 RVA: 0x000DE857 File Offset: 0x000DCC57
		// (set) Token: 0x0600284B RID: 10315 RVA: 0x000DE874 File Offset: 0x000DCC74
		public FieldInfo fieldInfo
		{
			get
			{
				if (!this._isInitialized)
				{
					Debug.LogError("CombinablePropertyAttribute needed fieldInfo but was not initialized. Did you call Init()?");
				}
				return this._fieldInfo;
			}
			protected set
			{
				this._fieldInfo = value;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x0600284C RID: 10316 RVA: 0x000DE87D File Offset: 0x000DCC7D
		// (set) Token: 0x0600284D RID: 10317 RVA: 0x000DE89A File Offset: 0x000DCC9A
		public UnityEngine.Object[] targets
		{
			get
			{
				if (!this._isInitialized)
				{
					Debug.LogError("CombinablePropertyAttribute needed fieldInfo but was not initialized. Did you call Init()?");
				}
				return this._targets;
			}
			protected set
			{
				this._targets = value;
			}
		}

		// Token: 0x040021A3 RID: 8611
		private bool _isInitialized;

		// Token: 0x040021A4 RID: 8612
		private FieldInfo _fieldInfo;

		// Token: 0x040021A5 RID: 8613
		private UnityEngine.Object[] _targets;
	}
}
