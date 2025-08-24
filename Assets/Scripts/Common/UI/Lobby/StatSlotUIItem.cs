using Gpm.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatSlotUIData : InfiniteScrollData
{
    public string StatImageName;
    public string StatName;
    public bool IsCriticalStat = false;
    public float CharacterStatAmount;
    public float FlatIncreasementAmount;
    public float PercentIncreasementAmount;
}

public class StatSlotUIItem : InfiniteScrollItem
{
    StatSlotUIData statSlotData;

    public Image Icon;
    public TextMeshProUGUI statNameText;
    public TextMeshProUGUI characterStatText;
    public TextMeshProUGUI flatIncreaseStatText;
    public TextMeshProUGUI percentIncreaseStatText;
    public TextMeshProUGUI totalStatText;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        statSlotData = scrollData as StatSlotUIData;
        if (statSlotData == null)
        {
            Debug.LogError("StatSlotUIData is null");
            return;
        }

        statNameText.text = statSlotData.StatName;

        var texture = Resources.Load<Texture2D>($"Textures/StatIcons/{statSlotData.StatImageName}");
        if(texture != null)
        {
            Icon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        float totalAmount = statSlotData.CharacterStatAmount * (statSlotData.PercentIncreasementAmount + 1) + statSlotData.FlatIncreasementAmount;

        if (statSlotData.IsCriticalStat)
        {
            characterStatText.text = $"+{statSlotData.CharacterStatAmount * 100}%";
            flatIncreaseStatText.text = $"+{statSlotData.FlatIncreasementAmount * 100}%";
            percentIncreaseStatText.text = $"¡¿{(statSlotData.PercentIncreasementAmount + 1) * 100}%";

            flatIncreaseStatText.gameObject.SetActive(statSlotData.FlatIncreasementAmount != 0 ? true : false);
            percentIncreaseStatText.gameObject.SetActive(statSlotData.PercentIncreasementAmount != 0 ? true : false);

            totalStatText.text = $"({totalAmount * 100}%)";
        }
        else
        {
            characterStatText.text = $"+{statSlotData.CharacterStatAmount.ToString("N2")}";
            flatIncreaseStatText.text = $"+{statSlotData.FlatIncreasementAmount.ToString("G2")}";
            percentIncreaseStatText.text = $"¡¿{((statSlotData.PercentIncreasementAmount + 1) * 100)}%";

            flatIncreaseStatText.gameObject.SetActive(statSlotData.FlatIncreasementAmount != 0 ? true : false);
            percentIncreaseStatText.gameObject.SetActive(statSlotData.PercentIncreasementAmount != 0 ? true : false);

            totalStatText.text = $"({totalAmount.ToString("N2")})";
        }

    }
}
