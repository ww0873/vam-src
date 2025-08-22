using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battlehub.Utils
{
	// Token: 0x020002A4 RID: 676
	[ExecuteInEditMode]
	public class Run : MonoBehaviour
	{
		// Token: 0x06000FED RID: 4077 RVA: 0x0005B144 File Offset: 0x00059544
		public Run()
		{
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000FEE RID: 4078 RVA: 0x0005B14C File Offset: 0x0005954C
		public static Run Instance
		{
			get
			{
				return Run.m_instance;
			}
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0005B153 File Offset: 0x00059553
		public void Animation(IAnimationInfo animation)
		{
			if (this.m_animations.Contains(animation))
			{
				return;
			}
			this.m_animations.Add(animation);
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0005B173 File Offset: 0x00059573
		public void Remove(IAnimationInfo animation)
		{
			this.m_animations.Remove(animation);
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0005B182 File Offset: 0x00059582
		private void Awake()
		{
			if (Run.m_instance != null)
			{
				Debug.LogWarning("Another instance of Animation already exist");
			}
			Run.m_instance = this;
			this.m_animations = new List<IAnimationInfo>();
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0005B1B0 File Offset: 0x000595B0
		private void Update()
		{
			for (int i = 0; i < this.m_animations.Count; i++)
			{
				IAnimationInfo animationInfo = this.m_animations[i];
				animationInfo.T += Time.deltaTime;
				if (animationInfo.T >= animationInfo.Duration)
				{
					this.m_animations.Remove(animationInfo);
				}
			}
		}

		// Token: 0x04000E56 RID: 3670
		private static Run m_instance;

		// Token: 0x04000E57 RID: 3671
		private List<IAnimationInfo> m_animations;
	}
}
