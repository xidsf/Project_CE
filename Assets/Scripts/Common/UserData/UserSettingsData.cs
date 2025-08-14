using System.Collections.Generic;
using UnityEngine;

public class UserSettingsData : IUserData
{
    public float BGMVolume;
    public float SFXVolume;

    public void SetDefaultData()
    {
        BGMVolume = 0.5f;
        SFXVolume = 0.5f;
    }

    public bool LoadData()
    {
        bool result = false;

        try
        {
            BGMVolume = PlayerPrefs.GetFloat("BGMVolume");
            SFXVolume = PlayerPrefs.GetFloat("SFXVolume");

            result = true;
        }
        catch(System.Exception e)
        {
            Logger.LogError($"{GetType()}::err msg: {e.Message}");
        }
        return result;
    }

    public bool SaveData()
    {
        bool result = false;

        try
        {
            PlayerPrefs.SetFloat("BGMVolume", BGMVolume);
            PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
            PlayerPrefs.Save();

            result = true;
        }
        catch (System.Exception e)
        {
            Logger.LogError($"{GetType()}::err msg: {e.Message}");
        }
        return result;
    }
}
