using System;
using System.Collections.Generic;
using System.Linq;


namespace GOAP_PlannerExo
{

public enum StateMemberKey
{
    IS_ENEMY_DEAD,
    IS_NEAR_ENEMY,
    IS_NEAR_WEAPON,
    HAS_WEAPON,
    IS_HURT,
}

[Serializable]
public class StateMember
{
    public StateMemberKey Key;
    public bool           Value;

    internal StateMember(StateMember _ToCopy)
    {
        Key   = _ToCopy.Key;
        Value = _ToCopy.Value;
    }
}

[Serializable]
public class States
{
    public  List<StateMember>                StateList = new();
    private Dictionary<StateMemberKey, bool> m_Dictionary;

    public States(States _ToCopy)
    {
        foreach (StateMember stateMember in _ToCopy.StateList)
        {
            StateList.Add(new StateMember(stateMember));
        }

        GetAsDictionary(true);
    }
    
    public Dictionary<StateMemberKey, bool> GetAsDictionary(bool _ForceCreation = false)
    {
        if (m_Dictionary != null && !_ForceCreation)
            return m_Dictionary;

        m_Dictionary = StateList.ToDictionary(_StateMember => _StateMember.Key, _StateMember => _StateMember.Value);

        return m_Dictionary;
    }

    private void OnValidate()
    {
        GetAsDictionary(true);
    }

    public bool Compare(States _ToCompare)
    {
        Dictionary<StateMemberKey, bool> asDictionary = _ToCompare.GetAsDictionary();

        return StateList.All(_StateMember => asDictionary.ContainsKey(_StateMember.Key) && asDictionary[_StateMember.Key] == _StateMember.Value);
    }
}

}
