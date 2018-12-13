using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReload : MonoBehaviour {
    InputHandler input;
	GameObject player;

	void Update () {
        if (input.activityDown("Reset")) {
			int currentScrene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScrene);
        }
    }

    void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player");
        input = player.GetComponent<InputHandler>();

		//Reseta a memoria
		MemoryManager.Remember();
		reload();
		if(MemoryManager.Memory.Checkpoint.ID != -1) {
			softReload();
			return;
        }
		hardReload();
	}

	private void reload() {
		//reload gunState
		player.GetComponentInChildren<newGun>().Has_gun = MemoryManager.Memory.HasGun;
		//reload hookState
		player.GetComponentInChildren<newHook>().Has_hook = MemoryManager.Memory.HasHook;
		//reload Animation
		player.GetComponent<Animator>().runtimeAnimatorController = MemoryManager.Memory.AnimatorController;
	}

	//only if on the first checkpoint of one scene
	private void hardReload() {
	}

	//if not in the begining of the scene
	void softReload() {
		//reload position
		player.transform.position = MemoryManager.Memory.Checkpoint.SavedPosition;
		//reload life
		player.GetComponentInChildren<PlayerLife>().health = MemoryManager.Memory.Health;

		//Orbs são setadas diretamente pelo MemoryManager então não precisa recarregar eles aqui
	}
}
