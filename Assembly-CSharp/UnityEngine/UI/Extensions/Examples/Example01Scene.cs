using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000489 RID: 1161
	public class Example01Scene : MonoBehaviour
	{
		// Token: 0x06001D90 RID: 7568 RVA: 0x000AA449 File Offset: 0x000A8849
		public Example01Scene()
		{
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x000AA454 File Offset: 0x000A8854
		private void Start()
		{
			IEnumerable<int> source = Enumerable.Range(0, 20);
			if (Example01Scene.<>f__am$cache0 == null)
			{
				Example01Scene.<>f__am$cache0 = new Func<int, Example01CellDto>(Example01Scene.<Start>m__0);
			}
			List<Example01CellDto> data = source.Select(Example01Scene.<>f__am$cache0).ToList<Example01CellDto>();
			this.scrollView.UpdateData(data);
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x000AA4A0 File Offset: 0x000A88A0
		[CompilerGenerated]
		private static Example01CellDto <Start>m__0(int i)
		{
			return new Example01CellDto
			{
				Message = "Cell " + i
			};
		}

		// Token: 0x04001907 RID: 6407
		[SerializeField]
		private Example01ScrollView scrollView;

		// Token: 0x04001908 RID: 6408
		[CompilerGenerated]
		private static Func<int, Example01CellDto> <>f__am$cache0;
	}
}
