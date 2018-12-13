using System;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

	public static bool isController;

	[Header("Config Actions")]
	public Action[] actions;
	private List<string> lastActivity;

	[Header("Debugs")]
	public bool controllerDebugMode = false; //Unity
	public bool actionDebugMode = false; //Unity
	public bool keyDebugMode = false; //Unity

	void Start(){
		lastActivity = new List<string>();
	}
	void Update () {
		//Save what was active in the last tick
		lastActivity = getActiveActions();

		foreach(var act in actions) {
			act.update();
		}
		//if debug mode is active, print out the current active actions
		if (actionDebugMode) debug();
		if (keyDebugMode) detectPressedKeyOrButton();
		if (controllerDebugMode) print("isController: " + isController + "\n");
	}

	/// <summary>
	/// print out the current active actions
	/// </summary>
	private void debug() {
		string debugStr = "";
		string[] stra = getActiveActions().ToArray();
		foreach (var str in stra) debugStr += str + ";";
        if(debugStr != "") print(debugStr);
	}

	int id = 0;
	public void detectPressedKeyOrButton()
	{
		foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
			if (Input.GetKeyDown(kcode) && !((int)kcode <= 509 && (int)kcode >= 350))
				Debug.Log((id++) + " " + kcode);
	}

	public bool isKeyBeingPressed(){
		return Input.anyKey;
	}

	/// <summary>
	/// return an array with the name of the actions that are active by the input
	/// </summary>
	/// <returns></returns>
	public List<string> getActiveActions() {
		List<string> activeActions = new List<string>();

		foreach(var action in actions) {
			if (action.isActive()) {
				string command = action.Command;
				activeActions.Add(command);
			}
		}

		return activeActions;
	}

	/// <summary>
	/// Similar to GetKey
	/// </summary>
	public bool activityOf(string command) {
		//if (the command passed is active)
		//    return true
		//else
		//    return false
		return getActiveActions().Contains(command.ToLower());
	}

	/// <summary>
	/// Similar to GetKeyDown
	/// </summary>
	public bool activityDown(string command) {
		bool wasItActive = lastActivity.Contains(command.ToLower());
		//if it wasn't active but it is now
		if (activityOf(command) && !wasItActive) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Similar to GetKeyUp
	/// </summary>
	public bool activityUp(string command) {
		bool wasItActive = lastActivity.Contains(command.ToLower());
		//if it was active but it isn't anymore
		if (!activityOf(command) && wasItActive) {
			return true;
		}
		return false;
	}
}
