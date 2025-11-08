using UnityEngine;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Actions/PutDownPickaxe", fileName = "PutDownPickaxeScriptable")]
public class PutDownPickaxe : Action
{
	public override bool FinishActing(ref States _WorldStates)
	{
		if (!base.FinishActing(ref _WorldStates)) return false;
		
		Character.SetTool(0);
		
		return true;
	}
}

}

