using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP_Planner
{

[Serializable]
public class State
{
    [Tooltip("This is just the display of the name for clarity when list is closed, can't be changed")]
    public string   Name;
    
    public StateKey KeyScriptable;
    public bool     Value;

    public int Key => KeyScriptable.Hash;

    internal State(StateKey _Key, bool _Value)
    {
        KeyScriptable = _Key;
        Value         = _Value;
    }
    
    internal State(State _ToCopy)
    {
        KeyScriptable = _ToCopy.KeyScriptable;
        Value         = _ToCopy.Value;
    }

    internal void SetName()
    {
        Name = KeyScriptable?.Key + (Value ? "_True" : "_False");
    }
}

[Serializable]
public class States
{
    public  List<State>          StateList = new();

    public States()
    {}
    
    public States(States _ToCopy)
    {
        foreach (State stateMember in _ToCopy.StateList)
        {
            StateList.Add(new State(stateMember));
        }
    }

    public void Update()
    {
        foreach (State stateMember in StateList)
        {
            stateMember.SetName();
        }
    }

    public void ApplyWorldChanges(State _Key)
    {
        foreach (State state in StateList.Where(_State => _State.Key == _Key.Key))
        {
            state.Value             = _Key.Value;
        }
    }

    public void SetState(int _Key, bool _NewValue)
    {
        foreach (State state in StateList.Where(_State => _State.Key == _Key))
        {
            state.Value = _NewValue;
        }
    }
    
    public bool Compare(States _ToCompare)
    {
        return StateList.All(_State => _ToCompare.StateList.Where(_OtherState => _State.Key == _OtherState.Key).All(_Variable => _State.Value == _Variable.Value));
    }

    public static States MergeStates(States _WorldStates, States _InstanceWorldStates)
    {
        States newState = new();
        newState.StateList.AddRange(_WorldStates.StateList);
        newState.StateList.AddRange(_InstanceWorldStates.StateList);

        return newState;
    }
}

}
