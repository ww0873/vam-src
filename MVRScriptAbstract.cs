using System;

// Token: 0x02000C39 RID: 3129
public abstract class MVRScriptAbstract : JSONStorable
{
	// Token: 0x06005B32 RID: 23346 RVA: 0x00217EF7 File Offset: 0x002162F7
	protected MVRScriptAbstract()
	{
	}

	// Token: 0x06005B33 RID: 23347
	public abstract FreeControllerV3 GetMainAtomController();
}
