using UnityEngine;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Actions/EmptyOutput", fileName = "EmptyOutputScriptable")]
public class EmptyOutput : Action
{
	public override bool FinishActing(ref States _WorldStates)
	{
		if (!base.FinishActing(ref _WorldStates)) return false;
		
		TakeFromStorage(Storage);

		return true;
	}

	public override void SetStorage()
	{
		Storage = WorldManager.Instance.Output;

	}
}

}

