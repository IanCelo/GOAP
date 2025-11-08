using UnityEngine;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Actions/FillIronStock", fileName = "FillIronStockScriptable")]
public class FillIronStock : Action
{
	public override bool FinishActing(ref States _WorldStates)
	{
		if (!base.FinishActing(ref _WorldStates)) return false;
		
		PutInStorage(Storage);

		return true;
	}

	public override void SetStorage()
	{
		Storage = WorldManager.Instance.IronStock;

	}
}

}

