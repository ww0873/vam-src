using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200023C RID: 572
	[ExecuteInEditMode]
	public class FolderTemplate : MonoBehaviour
	{
		// Token: 0x06000BDF RID: 3039 RVA: 0x0004A7BF File Offset: 0x00048BBF
		public FolderTemplate()
		{
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x0004A7CF File Offset: 0x00048BCF
		private void Awake()
		{
			if (Application.isPlaying)
			{
				base.enabled = false;
			}
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0004A7E2 File Offset: 0x00048BE2
		private void OnTransformParentChanged()
		{
			this.FixName();
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x0004A7EA File Offset: 0x00048BEA
		private void Update()
		{
			if (this.m_name != base.name)
			{
				this.FixName();
				this.m_name = base.name;
			}
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0004A814 File Offset: 0x00048C14
		private void FixName()
		{
			FolderTemplate[] siblings = FolderTemplate.GetSiblings(this);
			base.name = PathHelper.RemoveInvalidFineNameCharacters(base.name);
			string name = base.name;
			IEnumerable<FolderTemplate> source = siblings;
			if (FolderTemplate.<>f__am$cache0 == null)
			{
				FolderTemplate.<>f__am$cache0 = new Func<FolderTemplate, string>(FolderTemplate.<FixName>m__0);
			}
			base.name = PathHelper.GetUniqueName(name, source.Select(FolderTemplate.<>f__am$cache0).ToArray<string>());
			this.m_name = base.name;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0004A880 File Offset: 0x00048C80
		private static FolderTemplate[] GetSiblings(FolderTemplate template)
		{
			if (template.transform.parent == null)
			{
				return new FolderTemplate[0];
			}
			FolderTemplate component = template.transform.parent.GetComponent<FolderTemplate>();
			if (component == null)
			{
				return new FolderTemplate[0];
			}
			List<FolderTemplate> list = new List<FolderTemplate>();
			IEnumerator enumerator = component.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					FolderTemplate component2 = transform.GetComponent<FolderTemplate>();
					if (component2 != null && component2 != template)
					{
						list.Add(component2);
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
			return list.ToArray();
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0004A958 File Offset: 0x00048D58
		[CompilerGenerated]
		private static string <FixName>m__0(FolderTemplate s)
		{
			return s.name;
		}

		// Token: 0x04000CB2 RID: 3250
		[EnumFlags]
		public AssetTypeHint TypeHint = AssetTypeHint.All;

		// Token: 0x04000CB3 RID: 3251
		public UnityEngine.Object[] Objects;

		// Token: 0x04000CB4 RID: 3252
		private string m_name;

		// Token: 0x04000CB5 RID: 3253
		[CompilerGenerated]
		private static Func<FolderTemplate, string> <>f__am$cache0;
	}
}
