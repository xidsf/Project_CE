using UnityEngine;

public class UserGoodsData : IUserData
{
    public long Gold { get; private set; } = 0;

    public bool LoadData()
    {
        bool result = false;

        try
        {
            Gold = long.Parse(PlayerPrefs.GetString("Gold"));
            result = true;
        }
        catch (System.Exception e)
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
            PlayerPrefs.SetString("Gold", Gold.ToString());
            PlayerPrefs.Save();
            result = true;
        }
        catch (System.Exception e)
        {
            Logger.LogError($"{GetType()}::err msg: {e.Message}");
        }
        return result;
    }

    public void SetDefaultData()
    {
        Gold = 0;
    }
}
