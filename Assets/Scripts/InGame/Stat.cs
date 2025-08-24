using System;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType { Flat, Percent }

public class StatModifier
{
    public readonly float value;
    public readonly ModifierType modifierType;
    public readonly object source;

    public StatModifier(float value, ModifierType isPercentage, object source)
    {
        this.value = value;
        this.modifierType = isPercentage;
        this.source = source;
    }
};

public class Stat
{
    readonly public float baseValue;
    private List<StatModifier> modifiers;
    public bool IsFixed => fixedValue != -1;
    private float fixedValue;

    private readonly float minValue;
    private readonly float maxValue;

    public event Action OnStatChanged;

    public Stat(float baseValue, float minValue = 0, float maxValue = 10000)
    {
        this.baseValue = baseValue;
        modifiers = new List<StatModifier>();
        fixedValue = -1;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public void AddModifier(StatModifier mod)
    {
        modifiers.Add(mod);
        OnStatChanged?.Invoke();
    }
    public void RemoveModifier(object source)
    {
        modifiers.RemoveAll(mod => mod.source == source);
        OnStatChanged?.Invoke();
    }

    public float GetAllFlatModifierSum()
    {
        float _flat = 0;
        foreach (StatModifier mod in modifiers)
        {
            if (mod.modifierType == ModifierType.Flat)
                _flat += mod.value;
        }
        return _flat;
    }

    public float GetAllPercentModifierSum()
    {
        float _percent = 0;
        foreach (StatModifier mod in modifiers)
        {
            if (mod.modifierType == ModifierType.Percent)
                _percent += mod.value;
        }
        return _percent;
    }

    public float GetFinalValue()
    {
        if (IsFixed)
        {
            return fixedValue;
        }
        float flat = 0;
        float percent = 0;
        foreach (StatModifier mod in modifiers)
        {
            if (mod.modifierType == ModifierType.Flat)
                flat += mod.value;
            else
                percent += mod.value;
        }
        float finalValue = baseValue + flat;
        finalValue *= 1 + percent;

        if(finalValue < minValue)
        {
            finalValue = minValue;
        }
        else if (finalValue > maxValue)
        {
            finalValue = maxValue;
        }

        return finalValue;
    }

    public int GetRoundedInt() => Mathf.RoundToInt(GetFinalValue());

    public float GetRoundedFloat(int decimalPlaces = 1)
    {
        float multiplier = Mathf.Pow(10f, decimalPlaces);
        return Mathf.Round(GetFinalValue() * multiplier) / multiplier;
    }

    public void FixStat(float value)
    {
        if (value < minValue)
        {
            Logger.LogError ($"Fixed value must be larger than {minValue}");
            value = minValue;
        }
        fixedValue = value;
    }

    public void UnfixStat()
    {
        fixedValue = -1;
    }
}
