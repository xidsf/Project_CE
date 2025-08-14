using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

interface IStatReinforce
{
    void ReinforceStat(Stat stat, StatModifier mod);
}

interface ISkillReinfoce
{
    void ReinforceSkill(object skill);
}

public class ReinforcePlate
{
    List<ReinforcePlate> neighbors = new List<ReinforcePlate>();

    IStatReinforce statReinforce;
    ISkillReinfoce skillReinforce;



    ReinforcePlate(IStatReinforce stat)
    {
        statReinforce = stat;
    }

}
