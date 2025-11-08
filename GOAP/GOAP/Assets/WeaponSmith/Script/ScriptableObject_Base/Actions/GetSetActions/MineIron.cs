using UnityEngine;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Actions/MineIron", fileName = "MineIronScriptable")]
public class MineIron : Action
{
	public override bool FinishActing(ref States _WorldStates)
	{
		if (!base.FinishActing(ref _WorldStates)) return false;

		Character.SetInventory(ResourceType.IRON, Character.CarryCapacity, out bool setState);

		return true;
	}
}

}

