using System;
using System.Collections;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000B20 RID: 2848
public class DAZUVMap
{
	// Token: 0x06004DB1 RID: 19889 RVA: 0x001B4900 File Offset: 0x001B2D00
	public DAZUVMap()
	{
	}

	// Token: 0x06004DB2 RID: 19890 RVA: 0x001B4908 File Offset: 0x001B2D08
	public void Import(JSONNode jsonUV)
	{
		string text = jsonUV["id"];
		this.id = text;
		int num = 0;
		int asInt = jsonUV["uvs"]["count"].AsInt;
		this.uvs = new Vector2[asInt];
		IEnumerator enumerator = jsonUV["uvs"]["values"].AsArray.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				JSONNode jsonnode = (JSONNode)obj;
				float asFloat = jsonnode[0].AsFloat;
				float asFloat2 = jsonnode[1].AsFloat;
				this.uvs[num].x = asFloat;
				this.uvs[num].y = asFloat2;
				num++;
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
		num = 0;
		JSONArray asArray = jsonUV["polygon_vertex_indices"].AsArray;
		int count = asArray.Count;
		this.vertexMap = new DAZVertexMap[count];
		IEnumerator enumerator2 = asArray.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				JSONNode jsonnode2 = (JSONNode)obj2;
				this.vertexMap[num] = new DAZVertexMap();
				int asInt2 = jsonnode2[0].AsInt;
				int asInt3 = jsonnode2[1].AsInt;
				int asInt4 = jsonnode2[2].AsInt;
				this.vertexMap[num].polyindex = asInt2;
				this.vertexMap[num].fromvert = asInt3;
				this.vertexMap[num].tovert = asInt4;
				num++;
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
	}

	// Token: 0x04003D6B RID: 15723
	public string id;

	// Token: 0x04003D6C RID: 15724
	public Vector2[] uvs;

	// Token: 0x04003D6D RID: 15725
	public DAZVertexMap[] vertexMap;
}
