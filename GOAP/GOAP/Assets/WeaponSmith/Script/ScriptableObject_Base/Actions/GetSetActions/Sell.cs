using UnityEngine;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Actions/Sell", fileName = "SellScriptable")]
public class Sell : Action
{
	public override bool FinishActing(ref States _WorldStates)
	{
		if (!base.FinishActing(ref _WorldStates)) return false;

		Character.SetInventory(ResourceType.WEAPON, -Character.CarryCapacity, out bool setState);

		return true;
	}
}

}

