using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;
using EvilFarmingGame.Items;
using Godot;

namespace EvilFarmingGame.Player
{
	public class Inventory
	{
		public List<Slot> Slots;
		//public List<Item> Items;
		public Item HeldItem;

		public int HeldSlot = 0;

		public Inventory(int Capacity)
		{
			Slots = new List<Slot>(Capacity);
		}
		
		public Slot this[int index]
		{
			get { return Slots[index]; }
			set { Slots[index] = value; }
		}

		public void Gain(Item item, int amount=1)
		{
			if (Slots.Count < Slots.Capacity)
			{
				foreach (var slot in Slots)
				{
					if (slot.item == item && slot.Amount < slot.item.MaxStackAmount)
					{
						slot.add(amount);
						return;
					}
				}

				Slots.Add(new Slot(item, amount));
			}
		}

		public void Remove(Item item, int amount=1)
		{
			if (Slots.Count < Slots.Capacity)
			{
				for (int i=0; i<Slots.Count; i++)
				{
					var slot = Slots[i];
					if (slot.item == item)
					{
						slot.remove(amount);
						if (slot.Amount == 0) Slots.RemoveAt(i);
						return;
					}
				}
			}

			GD.Print( $"Item of ID: \"{item.ID}\" cannot be found in inventory.");
		}

		public bool CanBeAdded(Item item, int amount=1)
		{
			foreach (var slot in Slots)
			{
				if (slot.item == item && slot.Amount < slot.item.MaxStackAmount)
				{
					return true;
				}
			}
			
			return Slots.Count < Slots.Capacity;
		}
		
		public class Slot
		{
			public readonly Item item;
			private int _amount; 
			public int Amount => _amount;
			public Slot(Item item, int amount = 1)
			{
				this.item = item;
				_amount = amount;
			}

			public void add(int x)
			{
				_amount += x;
			}

			public void remove(int x)
			{
				_amount -= x;
			}

			public override string ToString() => $"Item: {item.ID}, Amount: {Amount}";
			public static implicit operator Item(Slot slot) => slot.item;
		}

	}
}
