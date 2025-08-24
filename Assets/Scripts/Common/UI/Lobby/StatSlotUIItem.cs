using Gpm.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatSlotUIData : InfiniteScrollData
{
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

        var texture = Resources.Load<Texture2D>($"Textures/StatIcons/{statSlotData.StatName}");
        if(texture != null)
        {
            Icon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        float totalAmount = statSlotData.CharacterStatAmount + statSlotData.FlatIncreasementAmount + statSlotData.PercentIncreasementAmount;

        if (statSlotData.IsCriticalStat)
        {
            characterStatText.text = $"+{statSlotData.CharacterStatAmount * 100}%";
            if(statSlotData.FlatIncreasementAmount > 0)
            {
                flatIncreaseStatText.gameObject.SetActive(true);
                flatIncreaseStatText.text = $"+{statSlotData.FlatIncreasementAmount * 100}%";
            }
            else
            {
                flatIncreaseStatText.gameObject.SetActive(false);
            }
            percentIncreaseStatText.gameObject.SetActive(false);
            totalStatText.text = $"({totalAmount * 100}%)";
        }
        else
        {
            characterStatText.text = $"+{statSlotData.CharacterStatAmount.ToString("F2")}";
            if(statSlotData.FlatIncreasementAmount > 0)
            {
                flatIncreaseStatText.gameObject.SetActive(true);
                flatIncreaseStatText.text = $"+{statSlotData.FlatIncreasementAmount.ToString("F2")}";
            }
            else
            {
                flatIncreaseStatText.gameObject.SetActive(false);
            }

            if(statSlotData.PercentIncreasementAmount > 0)
            {
                percentIncreaseStatText.gameObject.SetActive(true);
                percentIncreaseStatText.text = $"+{statSlotData.PercentIncreasementAmount.ToString("F2")}";
            }
            else
            {
                percentIncreaseStatText.gameObject.SetActive(false);
            }
            totalStatText.text = $"({totalAmount.ToString("F2")})";
        }

    }
}
