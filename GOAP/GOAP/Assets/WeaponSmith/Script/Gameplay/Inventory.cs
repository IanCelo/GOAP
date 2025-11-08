using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


public enum ResourceType
{
	NONE,
	WOOD,
	IRON,
	WEAPON,
}

public class Inventory : MonoBehaviour
{
	[SerializeField] private ResourceType ResourceType;
	public                   ResourceType Type
	{
		get => ResourceType;

		set
		{
			if (!Enum.IsDefined(typeof(ResourceType), value)) throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(ResourceType));

			ResourceType = value;
			OnAmountUpdate.Invoke();
		}
	}
	
	public int          MaxAmount = -1;
	
	[SerializeField] private int StartAmount;
	
	public readonly UnityEvent OnAmountUpdate = new();

	public bool Empty => Amount <= 0;
	public bool Full  => Amount >= MaxAmount;
	
	public                   int  Amount { get; private set; }

	private void Start()
	{
		PutIn(StartAmount);
	}

	// Put the newAmount in the storage
	public int PutIn(int _NewAmount)
	{
		// Stock is unlimited
		if (MaxAmount == -1)
		{
			Amount += _NewAmount;
			OnAmountUpdate.Invoke();

			return 0;
		}

		int difference = _NewAmount - (MaxAmount - Amount);
		Amount += _NewAmount;

		if (difference <= 0)
		{
			OnAmountUpdate.Invoke();
			return 0;
		}

		// More to add than there is remaining space
		Amount = MaxAmount;

		OnAmountUpdate.Invoke();
		
		return difference;
	}

	// Remove the current Amount from the storage
	public int Take(int _CurrentAmount)
	{
		int difference = _CurrentAmount - Amount;
		
		Amount -= _CurrentAmount;

		if (difference <= 0)
		{
			OnAmountUpdate.Invoke();
			return 0;
		}

		// More to remove than there is in stock
		Amount = 0;
		
		OnAmountUpdate.Invoke();

		return difference;
	}
}
