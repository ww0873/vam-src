using System;
using Oculus.Platform.Models;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x02000887 RID: 2183
	public static class Achievements
	{
		// Token: 0x06003781 RID: 14209 RVA: 0x0010E0F7 File Offset: 0x0010C4F7
		public static Request<AchievementUpdate> AddCount(string name, ulong count)
		{
			if (Core.IsInitialized())
			{
				return new Request<AchievementUpdate>(CAPI.ovr_Achievements_AddCount(name, count));
			}
			return null;
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x0010E111 File Offset: 0x0010C511
		public static Request<AchievementUpdate> AddFields(string name, string fields)
		{
			if (Core.IsInitialized())
			{
				return new Request<AchievementUpdate>(CAPI.ovr_Achievements_AddFields(name, fields));
			}
			return null;
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x0010E12B File Offset: 0x0010C52B
		public static Request<AchievementDefinitionList> GetAllDefinitions()
		{
			if (Core.IsInitialized())
			{
				return new Request<AchievementDefinitionList>(CAPI.ovr_Achievements_GetAllDefinitions());
			}
			return null;
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x0010E143 File Offset: 0x0010C543
		public static Request<AchievementProgressList> GetAllProgress()
		{
			if (Core.IsInitialized())
			{
				return new Request<AchievementProgressList>(CAPI.ovr_Achievements_GetAllProgress());
			}
			return null;
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x0010E15B File Offset: 0x0010C55B
		public static Request<AchievementDefinitionList> GetDefinitionsByName(string[] names)
		{
			if (Core.IsInitialized())
			{
				return new Request<AchievementDefinitionList>(CAPI.ovr_Achievements_GetDefinitionsByName(names, (names == null) ? 0 : names.Length));
			}
			return null;
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x0010E183 File Offset: 0x0010C583
		public static Request<AchievementProgressList> GetProgressByName(string[] names)
		{
			if (Core.IsInitialized())
			{
				return new Request<AchievementProgressList>(CAPI.ovr_Achievements_GetProgressByName(names, (names == null) ? 0 : names.Length));
			}
			return null;
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x0010E1AB File Offset: 0x0010C5AB
		public static Request<AchievementUpdate> Unlock(string name)
		{
			if (Core.IsInitialized())
			{
				return new Request<AchievementUpdate>(CAPI.ovr_Achievements_Unlock(name));
			}
			return null;
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x0010E1C4 File Offset: 0x0010C5C4
		public static Request<AchievementDefinitionList> GetNextAchievementDefinitionListPage(AchievementDefinitionList list)
		{
			if (!list.HasNextPage)
			{
				Debug.LogWarning("Oculus.Platform.GetNextAchievementDefinitionListPage: List has no next page");
				return null;
			}
			if (Core.IsInitialized())
			{
				return new Request<AchievementDefinitionList>(CAPI.ovr_HTTP_GetWithMessageType(list.NextUrl, 712888917));
			}
			return null;
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x0010E1FE File Offset: 0x0010C5FE
		public static Request<AchievementProgressList> GetNextAchievementProgressListPage(AchievementProgressList list)
		{
			if (!list.HasNextPage)
			{
				Debug.LogWarning("Oculus.Platform.GetNextAchievementProgressListPage: List has no next page");
				return null;
			}
			if (Core.IsInitialized())
			{
				return new Request<AchievementProgressList>(CAPI.ovr_HTTP_GetWithMessageType(list.NextUrl, 792913703));
			}
			return null;
		}
	}
}
