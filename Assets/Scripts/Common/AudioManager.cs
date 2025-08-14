using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    COUNT
}

public enum SFX
{
    COUNT
}

public class AudioManager : Singleton<AudioManager>
{
    public Transform BGMTransform;
    public Transform SFXTransform;

    private const string AUDIO_PATH = "Audio";

    private Dictionary<BGM, AudioSource> m_BGMPlayer  = new Dictionary<BGM, AudioSource>();
    private AudioSource m_CurrentBGM;

    private Dictionary<SFX, AudioSource> m_SFXPlayer = new Dictionary<SFX, AudioSource>();

    protected override void Init()
    {
        base.Init();

        LoadBGMPLayer();
        LoadSFXPlayer();
    }

    private void LoadBGMPLayer()
    {
        for (int i = 0; i < (int)BGM.COUNT; i++)
        {
            var audioName = ((BGM)i).ToString();
            var pathstr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load<AudioClip>(pathstr);
            if(audioClip == null)
            {
                Logger.LogError($"LoadBGMPlayer :: {audioName} clip does not exist");
                continue;
            }

            GameObject newBGMObj = new GameObject(audioName);
            var newAudioSource = newBGMObj.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = true;
            newAudioSource.playOnAwake = false;

            //Instantiate(bgmObj, BGMTransform);
            newBGMObj.transform.parent = BGMTransform;

            m_BGMPlayer[(BGM)i] = newAudioSource;
        }
    }

    private void LoadSFXPlayer()
    {
        for (int i = 0; i < (int)SFX.COUNT; i++)
        {
            var audioName = ((SFX)i).ToString();
            var pathstr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load<AudioClip>(pathstr);
            if (audioClip == null)
            {
                Logger.LogError($"LoadSFXPlayer :: {audioName} clip does not exist");
                continue;
            }

            GameObject newSFXObj = new GameObject(audioName);
            var newAudioSource = newSFXObj.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = false;
            newAudioSource.playOnAwake = false;

            //Instantiate(bgmObj, SFXTransform);
            newSFXObj.transform.parent = SFXTransform;

            m_SFXPlayer[(SFX)i] = newAudioSource;
        }
    }

    //public void OnLoadUserData()
    //{
    //    var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();
    //    if(userSettingsData != null)
    //    {
    //        if(!userSettingsData.Sound)
    //        {
    //            Mute();
    //        }
    //    }
    //}

    public void PlayBGM(BGM bgm)
    {
        if(m_CurrentBGM != null)
        {
            m_CurrentBGM.Stop();
            m_CurrentBGM = null;
        }

        if (!m_BGMPlayer.ContainsKey(bgm))
        {
            Logger.LogError($"PlayBGM :: invalid Clip name {bgm}");
            return;
        }

        m_CurrentBGM = m_BGMPlayer[bgm];
        m_CurrentBGM.Play();
    }

    public void PauseBGM()
    {
        if (m_CurrentBGM) m_CurrentBGM.Pause();
    }

    public void ResumeBGM()
    {
        if (m_CurrentBGM) m_CurrentBGM.UnPause();
    }

    public void StopBGM()
    {
        if (m_CurrentBGM) m_CurrentBGM.Stop();
    }

    public void PlaySFX(SFX sfx)
    {
        if (!m_SFXPlayer.ContainsKey(sfx))
        {
            Logger.LogError($"PlaySFX :: invalid Clip name {sfx}");
            return;
        }

        m_SFXPlayer[sfx].Play();
    }

    public void Mute()
    {
        foreach(var bgmPlayer in m_BGMPlayer.Values)
        {
            bgmPlayer.mute = true;
        }
        foreach (var sfxPlayer in m_SFXPlayer.Values)
        {
            sfxPlayer.mute = true;
        }
    }

    public void Unmute()
    {
        foreach (var bgmPlayer in m_BGMPlayer.Values)
        {
            bgmPlayer.mute = false;
        }
        foreach (var sfxPlayer in m_SFXPlayer.Values)
        {
            sfxPlayer.mute = false;
        }
    }
}
