using System;
using UnityEngine;

// Token: 0x0200033D RID: 829
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x0600141F RID: 5151 RVA: 0x00074409 File Offset: 0x00072809
	public Singleton()
	{
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06001420 RID: 5152 RVA: 0x00074414 File Offset: 0x00072814
	public static T Instance
	{
		get
		{
			if (Singleton<T>.applicationIsQuitting)
			{
				return (T)((object)null);
			}
			object @lock = Singleton<T>._lock;
			T instance;
			lock (@lock)
			{
				if (Singleton<T>._instance == null)
				{
					Singleton<T>._instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
					if (UnityEngine.Object.FindObjectsOfType(typeof(T)).Length > 1)
					{
						return Singleton<T>._instance;
					}
					if (Singleton<T>._instance == null)
					{
						GameObject gameObject = new GameObject();
						Singleton<T>._instance = gameObject.AddComponent<T>();
						gameObject.name = "(singleton) " + typeof(T).ToString();
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
				}
				instance = Singleton<T>._instance;
			}
			return instance;
		}
	}

	// Token: 0x06001421 RID: 5153 RVA: 0x000744FC File Offset: 0x000728FC
	public void OnDestroy()
	{
		Singleton<T>.applicationIsQuitting = true;
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x00074504 File Offset: 0x00072904
	// Note: this type is marked as 'beforefieldinit'.
	static Singleton()
	{
	}

	// Token: 0x0400116D RID: 4461
	private static T _instance;

	// Token: 0x0400116E RID: 4462
	private static object _lock = new object();

	// Token: 0x0400116F RID: 4463
	private static bool applicationIsQuitting = false;
}
