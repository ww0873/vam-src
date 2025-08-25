using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000723 RID: 1827
	public class Comment : MonoBehaviour
	{
		// Token: 0x06002C96 RID: 11414 RVA: 0x000EF3C8 File Offset: 0x000ED7C8
		public Comment()
		{
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06002C97 RID: 11415 RVA: 0x000EF3D7 File Offset: 0x000ED7D7
		// (set) Token: 0x06002C98 RID: 11416 RVA: 0x000EF3DF File Offset: 0x000ED7DF
		public string text
		{
			get
			{
				return this._comment;
			}
			set
			{
				this._comment = value;
			}
		}

		// Token: 0x04002388 RID: 9096
		[TextArea]
		[SerializeField]
		protected string _comment;

		// Token: 0x04002389 RID: 9097
		[SerializeField]
		[HideInInspector]
		protected bool _isEditing = true;
	}
}
