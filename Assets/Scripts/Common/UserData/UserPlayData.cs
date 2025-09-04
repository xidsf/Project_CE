using UnityEngine;

public enum CharacterType
{
    None = -1,
    Warrior,
    Mage,
    Archer,
    Dummy,
    Count
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
                Logger.Log($"User play data loaded. SelectedCharacter: {SelectedCharacter}");
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
                Logger.Log($"User play data saved. SelectedCharacter: {SelectedCharacter}");
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
