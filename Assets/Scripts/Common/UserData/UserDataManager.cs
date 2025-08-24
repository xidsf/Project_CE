using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    private List<IUserData> userDataList = new();

    public bool IsExistSavedData { get; private set; }

    protected override void Init()
    {
        base.Init();

        userDataList.Add(new UserSettingsData());
        userDataList.Add(new UserGoodsData());
        userDataList.Add(new UserInventoryData());
    }

    public void SetDefaultData()
    {
        IsExistSavedData = PlayerPrefs.GetInt("ExistsSavedData") == 1 ? true : false;
        if (!IsExistSavedData)
        {
            foreach(var data in userDataList)
            {
                data.SetDefaultData();
            }
            IsExistSavedData = true;
            PlayerPrefs.SetInt("ExistsSavedData", 1);
            SaveUserData();
        }
    }

    public void LoadUserData()
    {
        IsExistSavedData = PlayerPrefs.GetInt("ExistsSavedData") == 1 ? true : false;

        if (IsExistSavedData)
        {
            for (int i = 0; i < userDataList.Count; i++)
            {
                userDataList[i].LoadData();
            }
        }
    }

    public void SaveUserData()
    {
        bool hasSaveError = false;

        for (int i = 0; i < userDataList.Count; i++)
        {
            bool isSaveSuccess = userDataList[i].SaveData();
            if (!isSaveSuccess)
            {
                hasSaveError = true;
            }
        }

        if (!hasSaveError)
        {
            IsExistSavedData = true;
            PlayerPrefs.SetInt("ExistsSavedData", 1);
            PlayerPrefs.Save();
        }
    }

    public T GetUserData<T>()
    {
        return userDataList.OfType<T>().FirstOrDefault();
    }
}
