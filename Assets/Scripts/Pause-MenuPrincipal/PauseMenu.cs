using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

	public GameObject PauseUI;
	public Texture2D cursorTexture;
	public GameObject player;
	public string startSceneName = "L1.S1.o_beco";

	private InputHandler input;

	private bool paused = false;

	private void Start()
	{
		input = player.GetComponent<InputHandler>();
		PauseUI.SetActive(false);

		Vector2 cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
		Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
	}

	private void Update()
	{
		if (input.activityDown("pause"))
		{
			paused = !paused;
		}

		if (paused)
		{
			PauseUI.SetActive(true);
			Time.timeScale = 0;
			Cursor.visible = true;
		}

		if (!paused)
		{
			PauseUI.SetActive(false);
			Time.timeScale = 1;
			Cursor.visible = false;
		}
	}

	public void Resume()
	{
		paused = false;
	}

	public void Restart()
	{
		MemoryManager.Amnesia();

		SceneManager.LoadScene(this.startSceneName);
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
		paused = false;
	}

	public void LoadScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
		paused = false;
	}

	public void Quit()
	{
		Application.Quit();
	}
}
