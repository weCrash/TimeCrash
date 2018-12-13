using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerNameUI : MonoBehaviour {

	private string playerName = null;
	private Button btn;

	public string APIBaseURL = "https://us-central1-timecrash-183921.cloudfunctions.net/addPlays";
	public string orbsQuery = "o";
	public string playtimeQuery = "p";
	public string userQuery = "u";



	private void OnEnable() {
		if (FindObjectOfType<MemoryManager>() == null) {
			SceneManager.LoadScene(0);
		}
	}

	private void Start() {
		this.btn = FindObjectOfType<Button>();
		Cursor.visible = true;
	}

	//
	// UI functions
	//

	public void textChanged(string text) {
		if (text == "") {
			this.btn.interactable = false;
			return;
		}

		this.btn.interactable = true;
	}

	public void SavePlayerName(string text) {
		this.playerName = text;
	}
	
	public void OnClickSubmitPlay() {
		if (this.playerName == null) {
			Debug.LogError("WTF? no name!!");
			return;
		}
		btn.interactable = false;

		string url = APIBaseURL;
		setPoints(ref url);
		setPlaytime(ref url);
		setUser(ref url);

		//make the url call, no need for variable
		StartCoroutine(submitAndBackToMenu(url));
	}

	public void OnClickBackToMenu() {
		MemoryManager.Amnesia();
		SceneManager.LoadScene(0);
	}

	IEnumerator submitAndBackToMenu(string url) {
		yield return new WWW(url);
		OnClickBackToMenu();
	}

	//
	// API functions
	//

	private void setUser(ref string url)
	{
		prepareURLToQuery(ref url);

		url += userQuery + "=" + this.playerName;
	}

	private void setPlaytime(ref string url)
	{
		prepareURLToQuery(ref url);
		url += playtimeQuery + "=" + MemoryManager.Memory.Playtime;
	}

	private void setPoints(ref string url)
	{
		prepareURLToQuery(ref url);
		url += orbsQuery + "=" + MemoryManager.Memory.Orbs;
	}

	private void prepareURLToQuery(ref string url)
	{
		if (!url.Contains("?")) {
			url += "?";
			return;
		}
		url += "&";
	}
}
