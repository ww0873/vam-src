using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001C0 RID: 448
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSpriteRenderer : PersistentRenderer
	{
		// Token: 0x06000930 RID: 2352 RVA: 0x000391FD File Offset: 0x000375FD
		public PersistentSpriteRenderer()
		{
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00039208 File Offset: 0x00037608
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			SpriteRenderer spriteRenderer = (SpriteRenderer)obj;
			spriteRenderer.sprite = (Sprite)objects.Get(this.sprite);
			spriteRenderer.drawMode = (SpriteDrawMode)this.drawMode;
			spriteRenderer.size = this.size;
			spriteRenderer.adaptiveModeThreshold = this.adaptiveModeThreshold;
			spriteRenderer.tileMode = (SpriteTileMode)this.tileMode;
			spriteRenderer.color = this.color;
			spriteRenderer.flipX = this.flipX;
			spriteRenderer.flipY = this.flipY;
			return spriteRenderer;
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0003929C File Offset: 0x0003769C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			SpriteRenderer spriteRenderer = (SpriteRenderer)obj;
			this.sprite = spriteRenderer.sprite.GetMappedInstanceID();
			this.drawMode = (uint)spriteRenderer.drawMode;
			this.size = spriteRenderer.size;
			this.adaptiveModeThreshold = spriteRenderer.adaptiveModeThreshold;
			this.tileMode = (uint)spriteRenderer.tileMode;
			this.color = spriteRenderer.color;
			this.flipX = spriteRenderer.flipX;
			this.flipY = spriteRenderer.flipY;
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00039323 File Offset: 0x00037723
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.sprite, dependencies, objects, allowNulls);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x00039340 File Offset: 0x00037740
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			SpriteRenderer spriteRenderer = (SpriteRenderer)obj;
			base.AddDependency(spriteRenderer.sprite, dependencies);
		}

		// Token: 0x04000A34 RID: 2612
		public long sprite;

		// Token: 0x04000A35 RID: 2613
		public uint drawMode;

		// Token: 0x04000A36 RID: 2614
		public Vector2 size;

		// Token: 0x04000A37 RID: 2615
		public float adaptiveModeThreshold;

		// Token: 0x04000A38 RID: 2616
		public uint tileMode;

		// Token: 0x04000A39 RID: 2617
		public Color color;

		// Token: 0x04000A3A RID: 2618
		public bool flipX;

		// Token: 0x04000A3B RID: 2619
		public bool flipY;
	}
}
