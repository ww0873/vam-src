using System;
using Oculus.Platform.Models;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x0200088B RID: 2187
	public static class CloudStorage
	{
		// Token: 0x0600378F RID: 14223 RVA: 0x0010E2BA File Offset: 0x0010C6BA
		public static Request<CloudStorageUpdateResponse> Delete(string bucket, string key)
		{
			if (Core.IsInitialized())
			{
				return new Request<CloudStorageUpdateResponse>(CAPI.ovr_CloudStorage_Delete(bucket, key));
			}
			return null;
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x0010E2D4 File Offset: 0x0010C6D4
		public static Request<CloudStorageData> Load(string bucket, string key)
		{
			if (Core.IsInitialized())
			{
				return new Request<CloudStorageData>(CAPI.ovr_CloudStorage_Load(bucket, key));
			}
			return null;
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x0010E2EE File Offset: 0x0010C6EE
		public static Request<CloudStorageMetadataList> LoadBucketMetadata(string bucket)
		{
			if (Core.IsInitialized())
			{
				return new Request<CloudStorageMetadataList>(CAPI.ovr_CloudStorage_LoadBucketMetadata(bucket));
			}
			return null;
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x0010E307 File Offset: 0x0010C707
		public static Request<CloudStorageConflictMetadata> LoadConflictMetadata(string bucket, string key)
		{
			if (Core.IsInitialized())
			{
				return new Request<CloudStorageConflictMetadata>(CAPI.ovr_CloudStorage_LoadConflictMetadata(bucket, key));
			}
			return null;
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x0010E321 File Offset: 0x0010C721
		public static Request<CloudStorageData> LoadHandle(string handle)
		{
			if (Core.IsInitialized())
			{
				return new Request<CloudStorageData>(CAPI.ovr_CloudStorage_LoadHandle(handle));
			}
			return null;
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x0010E33A File Offset: 0x0010C73A
		public static Request<CloudStorageMetadata> LoadMetadata(string bucket, string key)
		{
			if (Core.IsInitialized())
			{
				return new Request<CloudStorageMetadata>(CAPI.ovr_CloudStorage_LoadMetadata(bucket, key));
			}
			return null;
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x0010E354 File Offset: 0x0010C754
		public static Request<CloudStorageUpdateResponse> ResolveKeepLocal(string bucket, string key, string remoteHandle)
		{
			if (Core.IsInitialized())
			{
				return new Request<CloudStorageUpdateResponse>(CAPI.ovr_CloudStorage_ResolveKeepLocal(bucket, key, remoteHandle));
			}
			return null;
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x0010E36F File Offset: 0x0010C76F
		public static Request<CloudStorageUpdateResponse> ResolveKeepRemote(string bucket, string key, string remoteHandle)
		{
			if (Core.IsInitialized())
			{
				return new Request<CloudStorageUpdateResponse>(CAPI.ovr_CloudStorage_ResolveKeepRemote(bucket, key, remoteHandle));
			}
			return null;
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x0010E38A File Offset: 0x0010C78A
		public static Request<CloudStorageUpdateResponse> Save(string bucket, string key, byte[] data, long counter, string extraData)
		{
			if (Core.IsInitialized())
			{
				return new Request<CloudStorageUpdateResponse>(CAPI.ovr_CloudStorage_Save(bucket, key, data, (uint)((data == null) ? 0 : data.Length), counter, extraData));
			}
			return null;
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x0010E3B7 File Offset: 0x0010C7B7
		public static Request<CloudStorageMetadataList> GetNextCloudStorageMetadataListPage(CloudStorageMetadataList list)
		{
			if (!list.HasNextPage)
			{
				Debug.LogWarning("Oculus.Platform.GetNextCloudStorageMetadataListPage: List has no next page");
				return null;
			}
			if (Core.IsInitialized())
			{
				return new Request<CloudStorageMetadataList>(CAPI.ovr_HTTP_GetWithMessageType(list.NextUrl, 1544004335));
			}
			return null;
		}
	}
}
