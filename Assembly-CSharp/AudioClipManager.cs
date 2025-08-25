using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000B78 RID: 2936
public class AudioClipManager : JSONStorable
{
	// Token: 0x06005261 RID: 21089 RVA: 0x001DC03A File Offset: 0x001DA43A
	public AudioClipManager()
	{
	}

	// Token: 0x06005262 RID: 21090 RVA: 0x001DC044 File Offset: 0x001DA444
	public NamedAudioClip GetClip(string uid)
	{
		NamedAudioClip result;
		if (this.uidToClip.TryGetValue(uid, out result))
		{
			return result;
		}
		uid = Regex.Replace(uid, "\\\\", "/");
		uid = Regex.Replace(uid, ".*/", string.Empty);
		if (this.uidToClip.TryGetValue(uid, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06005263 RID: 21091 RVA: 0x001DC0A0 File Offset: 0x001DA4A0
	public List<string> GetCategories()
	{
		List<string> list = new List<string>(this.categoryToClipList.Keys);
		list.Sort();
		return list;
	}

	// Token: 0x06005264 RID: 21092 RVA: 0x001DC0C8 File Offset: 0x001DA4C8
	public List<NamedAudioClip> GetCategoryClips(string category)
	{
		List<NamedAudioClip> result;
		if (this.categoryToClipList.TryGetValue(category, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06005265 RID: 21093 RVA: 0x001DC0EC File Offset: 0x001DA4EC
	public virtual bool AddClip(NamedAudioClip nac)
	{
		NamedAudioClip namedAudioClip;
		if (this.uidToClip.TryGetValue(nac.uid, out namedAudioClip))
		{
			Debug.LogError("Found duplicate audio clip " + nac.uid);
			return false;
		}
		this.uidToClip.Add(nac.uid, nac);
		List<NamedAudioClip> list;
		if (!this.categoryToClipList.TryGetValue(nac.category, out list))
		{
			list = new List<NamedAudioClip>();
			this.categoryToClipList.Add(nac.category, list);
		}
		list.Add(nac);
		this.clips.Add(nac);
		nac.manager = this;
		return true;
	}

	// Token: 0x06005266 RID: 21094 RVA: 0x001DC188 File Offset: 0x001DA588
	public virtual bool RemoveClip(NamedAudioClip nac)
	{
		if (!this.clips.Remove(nac))
		{
			Debug.LogError("Tried to remove clip that is not registered " + nac.uid);
			return false;
		}
		nac.destroyed = true;
		if (!this.uidToClip.Remove(nac.uid))
		{
			Debug.LogError("Tried to remove clip that is not registered " + nac.uid);
			return false;
		}
		List<NamedAudioClip> list;
		if (!this.categoryToClipList.TryGetValue(nac.category, out list))
		{
			Debug.LogError("Could not find category list for " + nac.uid + " " + nac.category);
			return false;
		}
		if (!list.Remove(nac))
		{
			Debug.LogError("Could not find clip " + nac.uid + " in category list " + nac.category);
			return false;
		}
		if (list.Count == 0)
		{
			this.categoryToClipList.Remove(nac.category);
		}
		return true;
	}

	// Token: 0x06005267 RID: 21095 RVA: 0x001DC288 File Offset: 0x001DA688
	public virtual void RemoveAllClips()
	{
		this.categoryToClipList.Clear();
		this.uidToClip.Clear();
		foreach (NamedAudioClip namedAudioClip in this.clips)
		{
			namedAudioClip.destroyed = true;
		}
		this.clips.Clear();
		if (SuperController.singleton != null)
		{
			SuperController.singleton.ValidateAllAtoms();
		}
	}

	// Token: 0x17000BF3 RID: 3059
	// (get) Token: 0x06005268 RID: 21096 RVA: 0x001DC320 File Offset: 0x001DA720
	// (set) Token: 0x06005269 RID: 21097 RVA: 0x001DC328 File Offset: 0x001DA728
	protected bool isPlaying
	{
		get
		{
			return this._isPlaying;
		}
		set
		{
			if (this._isPlaying != value)
			{
				this._isPlaying = value;
				if (this._isPlaying)
				{
					foreach (NamedAudioClip namedAudioClip in this.clips)
					{
						if (namedAudioClip.testButtonText != null)
						{
							namedAudioClip.testButtonText.text = "Stop";
						}
					}
				}
				else
				{
					foreach (NamedAudioClip namedAudioClip2 in this.clips)
					{
						if (namedAudioClip2.testButtonText != null)
						{
							namedAudioClip2.testButtonText.text = "Test";
						}
					}
				}
			}
		}
	}

	// Token: 0x0600526A RID: 21098 RVA: 0x001DC428 File Offset: 0x001DA828
	public void TestClip(NamedAudioClip ac)
	{
		if (this.testAudioSource != null)
		{
			if (this.testAudioSource.isPlaying)
			{
				this.testAudioSource.Stop();
			}
			else
			{
				this.testAudioSource.clip = ac.sourceClip;
				this.testAudioSource.Play();
			}
		}
	}

	// Token: 0x0600526B RID: 21099 RVA: 0x001DC482 File Offset: 0x001DA882
	protected virtual void Init()
	{
		this.categoryToClipList = new Dictionary<string, List<NamedAudioClip>>();
		this.uidToClip = new Dictionary<string, NamedAudioClip>();
		this.clips = new List<NamedAudioClip>();
	}

	// Token: 0x0600526C RID: 21100 RVA: 0x001DC4A5 File Offset: 0x001DA8A5
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
		}
	}

	// Token: 0x0600526D RID: 21101 RVA: 0x001DC4BE File Offset: 0x001DA8BE
	protected virtual void Update()
	{
		if (this.testAudioSource != null)
		{
			this.isPlaying = this.testAudioSource.isPlaying;
		}
	}

	// Token: 0x0400423F RID: 16959
	protected Dictionary<string, List<NamedAudioClip>> categoryToClipList;

	// Token: 0x04004240 RID: 16960
	protected Dictionary<string, NamedAudioClip> uidToClip;

	// Token: 0x04004241 RID: 16961
	protected List<NamedAudioClip> clips;

	// Token: 0x04004242 RID: 16962
	public AudioSource testAudioSource;

	// Token: 0x04004243 RID: 16963
	protected bool _isPlaying;
}
