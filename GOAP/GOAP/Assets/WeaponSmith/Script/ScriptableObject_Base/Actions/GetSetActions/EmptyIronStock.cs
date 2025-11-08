using UnityEngine;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Actions/EmptyIronStock", fileName = "EmptyIronStockScriptable")]
public class EmptyIronStock : Action
{
	public override int GetCost()
	{
		Storage smelter = WorldManager.Instance.Smelter;

		int theoreticalAmount = smelter.Amount + Storage.Amount;

		if (theoreticalAmount * 2 < smelter.MaxAmount)
			return (5 + (smelter.MaxAmount - theoreticalAmount)) * base.GetCost();
		
		return base.GetCost();
	}

	public override bool FinishActing(ref States _WorldStates)
	{
		if (!base.FinishActing(ref _WorldStates)) return false;

		TakeFromStorage(Storage);

		return true;
	}

	public override void SetStorage()
	{
		Storage = WorldManager.Instance.IronStock;
	}
}

}

