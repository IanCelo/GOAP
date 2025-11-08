using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GOAP_PlannerExo
{

public abstract class Action : ScriptableObject
{
	internal GameObject Agent;
	
	public States Preconditions;

	public States Result;

	public int Cost; 

	public virtual bool IsValid(States _WorldState)
	{
		Dictionary<StateMemberKey, bool> asDictionary = _WorldState.GetAsDictionary();

		return Preconditions.Compare(_WorldState);
	}

	public virtual int GetCost(States _WorldState = null)
	{
		return Cost;
	}
	
	public void ApplyWorldChanges(ref States _WorldState)
	{
		foreach (StateMember resultState in Result.StateList)
		{
			foreach (StateMember state in _WorldState.StateList.Where(_State => _State.Key == resultState.Key))
			{
				state.Value = resultState.Value;
			}
		}

		_WorldState.GetAsDictionary(true);
	}

	public abstract bool Act(ref States _WorldState);
}

}
