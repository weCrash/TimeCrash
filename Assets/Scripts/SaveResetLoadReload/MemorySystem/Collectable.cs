using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	public bool autoSetInactive = false;
	public bool autoCollectOnTrigger = false;
	public bool autoDestroy = false;

	public event EventHandler OnCollect;

	private string id;

	void Awake() {
		//get a "unique" id (it's good enough, ok?)
		id = (transform.position.x * 10000 + transform.position.y).ToString();
	}

	private void Start() {
		if(!autoSetInactive) { return; }

		//Auto inactivate onject if this option was set 
		bool itWasTaken = MemoryManager.Was(this).Taken;
		if(itWasTaken) {
			gameObject.SetActive(false);
		}
	}

	public void OnTriggerEnter2D(Collider2D collision) {
		if(!autoCollectOnTrigger) { return; }
		//Comportamento padrão do script é ser coletado no trigger enter, se opção estiver ativa

		if (collision.tag != "Player") {
			return;
		}
		this.CollectIt();
	}

	public void CollectIt() {
		MemoryManager.Memory.Get(id).TakeIt();

		//trigger event
		if (OnCollect != null) OnCollect(null, null);

		if (!autoDestroy) { return; }
		//Auto destroy object after it got collected
		Destroy(this.gameObject);
	}

	public string ID {
		get { return id; }
		set { id = value; }
	}
}