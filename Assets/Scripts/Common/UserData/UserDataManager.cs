using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    private List<IUserData> userDataList = new();

    bool isExistSavedData;

    protected override void Init()
    {
        base.Init();

        userDataList.Add(new UserSettingsData());
        userDataList.Add(new UserGoodsData());
    }

    public void LoadUserData()
    {
        isExistSavedData = PlayerPrefs.GetInt("ExistsSavedData") == 1 ? true : false;

        if (isExistSavedData)
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
            isExistSavedData = true;
            PlayerPrefs.SetInt("ExistsSavedData", 1);
            PlayerPrefs.Save();
        }
    }

    public T GetUserData<T>()
    {
        return userDataList.OfType<T>().FirstOrDefault();
    }
}
