using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCtrl : MonoBehaviour
{
	public int firstSceneIndex = 1;



#if UNITY_STANDALONE_WIN
	private void Awake() {
		//Securit Stuff
		if(!System.IO.File.Exists("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\TimeCrash_Data\\validated.dat")) {
            Application.Quit();
		}
	}
#endif


	public void NewGame() {
		if (FindObjectOfType<MemoryManager>() != null) {
			//se o objeto existir, deleta ele
			MemoryManager.Amnesia();
		}

		StartCoroutine(changeLevel(firstSceneIndex));
	}

	public void Continue() {
		if (FindObjectOfType<MemoryManager>() == null) {
			NewGame();
			return;
		}

		StartCoroutine(
			changeLevel(
				MemoryManager.Memory.Checkpoint.SceneIndex));
	}



    public void LoadScene (string sceneName) { 
        SceneManager.LoadScene(sceneName);
    }

	public void LoadScene(int sceneIndex) {
		StartCoroutine(changeLevel(sceneIndex));
    }

	private IEnumerator changeLevel(int sceneIndex) {
        float fadeTime = GameObject.Find("MENUCTRL").GetComponent<Fading>().BeginFade(-1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(sceneIndex);
	}
}
