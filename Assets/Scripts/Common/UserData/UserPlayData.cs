using UnityEngine;

public enum CharacterType
{
    None = 0,
    Warrior = 1,
    Mage = 2,
    Archer = 3
}

public class UserPlayData : IUserData
{
    public CharacterType SelectedCharacter { get; set; } = CharacterType.None;

    public void SetDefaultData()
    {
        SelectedCharacter = CharacterType.Warrior;
    }

    public bool LoadData()
    {
        bool result = false;

        try
        {
            var lastSelectedCharacter = (CharacterType)PlayerPrefs.GetInt("LastSelectedCharacter");
            if(SelectedCharacter != CharacterType.None)
            {
                SelectedCharacter = lastSelectedCharacter;
                result = true;
            }
        }
        catch (System.Exception)
        {
            Logger.LogError("Failed to load user play data.");
        }
        return result;
    }

    public bool SaveData()
    {
        bool result = false;

        try
        {
            if(SelectedCharacter != CharacterType.None)
            {
                PlayerPrefs.SetInt("LastSelectedCharacter", (int)SelectedCharacter);
                PlayerPrefs.Save();
                result = true;
            }
            
        }
        catch (System.Exception)
        {
            Logger.LogError("Failed to load user play data.");
        }
        return result;
    }

    
}
