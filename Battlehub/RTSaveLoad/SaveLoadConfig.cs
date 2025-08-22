using System;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000204 RID: 516
	public class SaveLoadConfig
	{
		// Token: 0x06000A57 RID: 2647 RVA: 0x0003EF10 File Offset: 0x0003D310
		static SaveLoadConfig()
		{
			SaveLoadConfig.m_disabledTypes = new Type[]
			{
				typeof(Button)
			};
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0003EF35 File Offset: 0x0003D335
		public SaveLoadConfig()
		{
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x0003EF3D File Offset: 0x0003D33D
		public static Type[] DisabledComponentTypes
		{
			get
			{
				return SaveLoadConfig.m_disabledTypes;
			}
		}

		// Token: 0x04000B68 RID: 2920
		private static Type[] m_disabledTypes = new Type[0];
	}
}
