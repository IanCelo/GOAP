using UnityEngine;
using UnityEngine.AI;


namespace GOAP_Planner
{

public abstract class Action : ScriptableObject
{
	internal NavMeshAgent Agent;
	internal Character Character;
	
	public States Preconditions;

	public States Result;

	internal Transform Target;
	internal Storage   Storage = null;

	public int  Cost;
	public bool IsMoveAction;

	public  float Duration;
	private float m_RemainingTime;

	public virtual bool IsValid(States _WorldState)
	{
		return Preconditions.Compare(_WorldState);
	}

	public virtual int GetCost()
	{
		return Cost;
	}
	
	public void ApplyWorldChanges(ref States _WorldState)
	{
		foreach (State resultState in Result.StateList)
		{
			_WorldState.ApplyWorldChanges(resultState);
		}
	}

	public void Act()
	{		
		if (IsMoveAction && Target != null)
			Agent.SetDestination(Target.position);

		m_RemainingTime = 0;
	}

	public virtual bool FinishActing(ref States _WorldStates)
	{
		if (IsMoveAction)
		{
			if (!(Agent.remainingDistance <= Agent.stoppingDistance)) return false;
			return true;
		}

		m_RemainingTime += Time.deltaTime;

		return !(m_RemainingTime < Duration);
	}
	
	protected void PutInStorage(Inventory _Storage)
	{
		int amount      = Character.CarryCapacity;
		int totalAmount = _Storage.PutIn(amount);
		int remaining   = Character.SetInventory(_Storage.Type, -(amount - totalAmount), out bool setState);
		_Storage.Take(remaining);
	}

	protected void TakeFromStorage(Inventory _Storage)
	{
		int amount      = Character.CarryCapacity;
		int totalAmount = _Storage.Take(amount);
		int remaining   = Character.SetInventory(_Storage.Type, amount - totalAmount, out bool setState);
		_Storage.PutIn(remaining);
	}
	
	private void OnValidate()
	{
		Preconditions.Update();
		Result.Update();
	}

	public virtual void SetStorage()
	{
	}
}

}

