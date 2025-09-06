using Gpm.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUIItemData : InfiniteScrollData
{
    public CharacterType CharacterType;
}

public class CharacterSelectUIItem : InfiniteScrollItem
{
    [SerializeField] GameObject StatArea;
    [SerializeField] RawImage CharImage;

    private CharacterSelectUIItemData characterSelectUIItemData;

    [Header("Ω∫≈» ≈ÿΩ∫∆Æ")]
    [SerializeField] TextMeshProUGUI moveSpeedText;
    [SerializeField] TextMeshProUGUI attackDamageText;
    [SerializeField] TextMeshProUGUI attackSpeedText;
    [SerializeField] TextMeshProUGUI attackRangeText;
    [SerializeField] TextMeshProUGUI critChanceText;
    [SerializeField] TextMeshProUGUI critDamageText;
    [SerializeField] TextMeshProUGUI healthPointText;


    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        characterSelectUIItemData = scrollData as CharacterSelectUIItemData;
        if(characterSelectUIItemData == null)
        {
            Logger.LogError("characterSelectUIItemData is invalid.");
            return;
        }

        CharImage.texture = CharacterUIManager.Instance.GetUIPlayerRT(characterSelectUIItemData.CharacterType);

        var charData = DataTableManager.Instance.GetCharacterData(characterSelectUIItemData.CharacterType);
        if(charData == null)
        {
            Logger.LogError($"Cannot load CharacterData: CharType:{characterSelectUIItemData.CharacterType}");
            return;
        }
        
        moveSpeedText.text = charData.MoveSpeed.ToString();
        attackDamageText.text = charData.AttackDamage.ToString();
        attackSpeedText.text = charData.AttackSpeed.ToString();
        attackRangeText.text = charData.AttackRange.ToString();
        critChanceText.text = charData.CritChance.ToString();
        critDamageText.text= charData.CritDamage.ToString();
        healthPointText.text = charData.HP.ToString();
    }


}
