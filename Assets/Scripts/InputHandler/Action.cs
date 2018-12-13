using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base of an action
/// </summary>
[System.Serializable]
public class Action
{
	public string command; //Unity
	public List<KeyCode> keyCodes = new List<KeyCode>(); //Unity

	public List<string> axisList = new List<string>(); //Unity
	public triggerDir triggerDirection; //Unity

	private bool active;
	private float strenght;

	public enum triggerDir
	{
		positive = 1, negative = -1
	}

	//this method is intended to be called by InputHandler script
	//not from unity itself, so it's lower case
	public void update()
	{
		//for keys
		foreach (var key in keyCodes)
		{
			active = Input.GetKey(key);
			//breaks loop when if finds a key that is pressed,
			//so that the next does not override the last
			if (active) {
				testForController(key);
				return;
			}
		}
		//for axis
		Func<float, bool> isTilted = setUpDirection(triggerDirection);
		foreach (var axis in axisList)
		{
			strenght = Input.GetAxis(axis);

			active = isTilted(strenght);
			if (active) {
				InputHandler.isController = true;
				return;
			}
		}
	}

	private void testForController(KeyCode key)
	{
		InputHandler.isController = (int)key >= (int)KeyCode.JoystickButton0;
	}

	private Func<float, bool> setUpDirection(triggerDir triggerDirection)
	{
		if (triggerDirection == triggerDir.positive)
		{
			return (i) => i > 0;
		}
		return (i) => i < 0;
	}

	public bool isActive()
	{
		return active;
	}
	/// <summary>
	/// Name of action/Command to perform
	/// </summary>
	public string Command {
		get { return command.ToLower(); }
	}

	/// <summary>
	/// How much the player tilted the analog
	/// </summary>
	public float Strenght {
		get { return strenght; }
	}
}
