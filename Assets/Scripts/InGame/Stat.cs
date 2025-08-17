using System;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType { Flat, Percent }

public class StatModifier
{
    public readonly float value;
    public readonly ModifierType isPercentage;
    public readonly object source;

    public StatModifier(float value, ModifierType isPercentage, object source)
    {
        this.value = value;
        this.isPercentage = isPercentage;
        this.source = source;
    }
};

public class Stat
{
    readonly public float baseValue;
    private List<StatModifier> modifiers;

    public event Action OnStatChanged;

    public Stat(float baseValue)
    {
        this.baseValue = baseValue;
        modifiers = new List<StatModifier>();
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
            if (mod.isPercentage == ModifierType.Flat)
                _flat += mod.value;
        }
        return _flat;
    }

    public float GetAllPercentModifierSum()
    {
        float _percent = 0;
        foreach (StatModifier mod in modifiers)
        {
            if (mod.isPercentage == ModifierType.Flat)
                _percent += mod.value;
        }
        return _percent;
    }

    public float GetFinalValue()
    {
        float flat = 0;
        float percent = 0;
        foreach (StatModifier mod in modifiers)
        {
            if (mod.isPercentage == ModifierType.Flat)
                flat += mod.value;
            else
                percent += mod.value;
        }
        float finalValue = baseValue + flat;
        finalValue *= 1 + percent;

        return finalValue;
    }

    public int GetRoundedInt() => Mathf.RoundToInt(GetFinalValue());

    public float GetRoundedFloat(int decimalPlaces = 1)
    {
        float multiplier = Mathf.Pow(10f, decimalPlaces);
        return Mathf.Round(GetFinalValue() * multiplier) / multiplier;
    }

}
