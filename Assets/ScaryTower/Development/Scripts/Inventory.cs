using System;
using System.Collections.Generic;

[Serializable]
public class Inventory
{
	public enum Powerup
	{
		Werewolf,
		Frankenstein,
		Ghost,
		Cat
	}

	public PlayerScript.Character character;
	public Elevator.Type elevator;

	public Dictionary<Powerup, int> powerup;
}