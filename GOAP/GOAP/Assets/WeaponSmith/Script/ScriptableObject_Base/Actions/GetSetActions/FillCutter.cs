using UnityEngine;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Actions/FillCutter", fileName = "FillCutterScriptable")]
public class FillCutter : Action
{
	public override bool FinishActing(ref States _WorldStates)
	{
		if (!base.FinishActing(ref _WorldStates)) return false;

		PutInStorage(Storage);

		return true;
	}

	public override void SetStorage()
	{
		Storage = WorldManager.Instance.Cutter;
	}
}

}

