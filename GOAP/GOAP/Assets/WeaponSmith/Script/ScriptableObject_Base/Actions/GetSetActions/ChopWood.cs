using UnityEngine;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Actions/ChopWood", fileName = "ChopWoodScriptable")]
public class ChopWood : Action
{
	public override bool FinishActing(ref States _WorldStates)
	{
		if (!base.FinishActing(ref _WorldStates)) return false;

		Character.SetInventory(ResourceType.WOOD, Character.CarryCapacity, out bool setState);

		return true;
	}
}

}

