using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : MonoBehaviour {

	private static Memory shortTermMemory;
	private static Memory longTermMemory;
	public static Memory Memory { get { return shortTermMemory; } }


	//On enable roda antes dos Start, inicializa esse componente antes
	void OnEnable () {
		init();
		ForgetCollectable();
	}

	void init() {
		DontDestroyOnLoad(this);
		loadStartMemoryFromSpawner();
	}

	private void loadStartMemoryFromSpawner() {
		Memory startMemory = FindObjectOfType<MemoryManagerSpawner>().starterMemoryInThisScene;
		shortTermMemory = startMemory.Clone();
	}

	static void populateMemory() {
		//para cada gameobject que tem o script Collectable
		foreach(var item in FindObjectsOfType<Collectable>()) {
			tryAddToMemory(item);
		}
	}

	static void tryAddToMemory(Collectable item) {
		try {
			shortTermMemory.Add(item.ID, new ObjectState());
		}
		catch(System.ArgumentException) {
			Debug.LogError("A chave [ "+item.ID+" ] para ' "+item.name+" ' já existe no dicionario");
		}		
	}

	/// <summary>
	/// Reseta a lista de objetos da scene
	/// </summary>
	public static void ForgetCollectable() {
		shortTermMemory.Clear();
		populateMemory();
		ConsolidateMemory();
	}

	public static void Amnesia() {
		ResetMemory();

		UnityEngine.Object self = FindObjectOfType<MemoryManager>().gameObject;
		Destroy(self);
	}

	/// <summary>
	/// Recarrega a memoria
	/// </summary>
	public static void Remember() {
		shortTermMemory = longTermMemory.Clone();
	}

	/// <summary>
	/// Retorna o Estado do objeto pelo no script
	/// </summary>
	public static ObjectState Was(MonoBehaviour gameObject) {
		Collectable thisCollectable = gameObject.GetComponent<Collectable>();

		return longTermMemory.Get(thisCollectable);
	}

	/// <summary>
	/// Passa as coisas da memoria de curto prazo pra de longa, tornando possivel o recarregamento de partes feitas
	/// </summary>
	public static void ConsolidateMemory() {
		OnSavingProgress();

		longTermMemory = shortTermMemory.Clone();
	}

	/// <summary>
	/// Event to be exectuted before consolidating memory
	/// </summary>
	public static event EventHandler OnSave;
	protected static void OnSavingProgress() {
		if (OnSave == null) {
			return;
		}
		OnSave(null, null); //call event
	}

	/// <summary>
	/// Executes when deleting MemoryManager to reset all variables
	/// </summary>
	public static event EventHandler OnMemoryReset;
	private static void ResetMemory() {
		if (OnMemoryReset == null) {
			return;
		}
		OnMemoryReset(null, null);
	}
}