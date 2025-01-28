using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    public List<int> modifiers;

    public int GetValue()
    {
        int finalValue = baseValue;
        foreach(int modifier in this.modifiers) {
            finalValue += modifier;
        }
        return finalValue;
    }
    public void SetDefaultValue(int _def) {
        this.baseValue = _def;
    }

    public void AddModifier(int _modifier)
    {
        this.modifiers.Add(_modifier);
    }
    public void RemoveModifier(int _modifier)
    {
        this.modifiers.Remove(_modifier);
    }
}
