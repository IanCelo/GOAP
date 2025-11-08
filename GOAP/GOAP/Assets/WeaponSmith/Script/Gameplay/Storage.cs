using GOAP_Planner;

public class Storage : Inventory
{
	public StateKey AffectedStorageState;

	public int FullPriority = 1;
	public int MoreHalfFullPriority = 1;
	public int LessHalfFullPriority = 1;
	public int EmptyPriority = 1;
	
	public int GetFillPriority(States _WorldStates)
	{
		// Storage has no limit, always as the same priority
		if (MaxAmount == -1)
		{
			_WorldStates.ApplyWorldChanges(new State(AffectedStorageState, Amount > 0));
			return Amount == 0 ? EmptyPriority : FullPriority;
		}

		// Storage full, don't need filling
		if (Amount == MaxAmount)
		{
			_WorldStates.ApplyWorldChanges(new State(AffectedStorageState, true));
			return FullPriority;
		}

		_WorldStates.ApplyWorldChanges(new State(AffectedStorageState, false));

		// Storage is empty -> Max priority
		if (Amount == 0)
			return EmptyPriority;

		// Storage is more than half full -> low priority
		if (Amount * 2 >= MaxAmount)
			return MoreHalfFullPriority;

		// Storage is less than half full -> high priority
		if (Amount * 2 < MaxAmount)
			return LessHalfFullPriority;
		
		return -1;
	}
}
