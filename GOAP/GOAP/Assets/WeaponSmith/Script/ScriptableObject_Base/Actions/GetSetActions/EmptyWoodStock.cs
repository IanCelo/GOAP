using UnityEngine;
using UnityEngine.Serialization;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Actions/EmptyWoodStock", fileName = "EmptyWoodStockScriptable")]
public class EmptyWoodStock : Action
{
	public override int GetCost()
	{
		Storage cutter            = WorldManager.Instance.Cutter;
		
		int     theoreticalAmount = cutter.Amount + Storage.Amount;

		if (theoreticalAmount * 2 < cutter.MaxAmount)
			return (5 + (cutter.MaxAmount - theoreticalAmount)) * base.GetCost();
		
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
		Storage = WorldManager.Instance.WoodStock;

	}
}

}

