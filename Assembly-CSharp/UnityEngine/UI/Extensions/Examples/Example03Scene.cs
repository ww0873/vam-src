using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000492 RID: 1170
	public class Example03Scene : MonoBehaviour
	{
		// Token: 0x06001DAB RID: 7595 RVA: 0x000AAC03 File Offset: 0x000A9003
		public Example03Scene()
		{
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x000AAC0C File Offset: 0x000A900C
		private void Start()
		{
			IEnumerable<int> source = Enumerable.Range(0, 20);
			if (Example03Scene.<>f__am$cache0 == null)
			{
				Example03Scene.<>f__am$cache0 = new Func<int, Example03CellDto>(Example03Scene.<Start>m__0);
			}
			List<Example03CellDto> data = source.Select(Example03Scene.<>f__am$cache0).ToList<Example03CellDto>();
			this.scrollView.UpdateData(data);
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x000AAC58 File Offset: 0x000A9058
		[CompilerGenerated]
		private static Example03CellDto <Start>m__0(int i)
		{
			return new Example03CellDto
			{
				Message = "Cell " + i
			};
		}

		// Token: 0x0400191A RID: 6426
		[SerializeField]
		private Example03ScrollView scrollView;

		// Token: 0x0400191B RID: 6427
		[CompilerGenerated]
		private static Func<int, Example03CellDto> <>f__am$cache0;
	}
}
