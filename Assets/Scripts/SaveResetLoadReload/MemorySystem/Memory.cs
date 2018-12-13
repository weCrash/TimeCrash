using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Memory : Dictionary<string, ObjectState> {
	private CheckpointInfo checkpoint = new CheckpointInfo();
	private int orbs;
	private float health;
	private long playtime;

	[SerializeField]
	private RuntimeAnimatorController animatorController;
	[SerializeField]
	private bool withGun;
	[SerializeField]
	private bool withHook;

	public int Orbs {
		get { return orbs; } 
		set { this.orbs = value; }
	}
	public long Playtime {
		get { return playtime; } 
		set { this.playtime = value; }
	}
	public float Health {
		get { return health; } 
		set { this.health = value; }
	}
	public RuntimeAnimatorController AnimatorController {
		get { return animatorController; } 
		set { this.animatorController = value; }
	}
	public bool HasGun {
		get { return withGun; }
		set { withGun = value; }
	}
	public bool HasHook {
		get { return withHook; }
		set { withHook = value; }
	}
	public CheckpointInfo Checkpoint {
		get { return checkpoint; } 
		set { this.checkpoint = value; }
	}

	//
	// Methods
	//

	internal Memory Clone() {
		Memory image = new Memory() {
			Orbs = this.Orbs,
			Playtime = this.Playtime,
			Health = this.Health,
			AnimatorController = this.AnimatorController,
			HasGun = this.HasGun,
			HasHook = this.HasHook,
			Checkpoint = this.Checkpoint,
		};

		foreach(var item in this) {
			image.Add(
				item.Key,
				new ObjectState(item.Value));
		}
		return image;
	}

	internal ObjectState Get(string id) {
		try {
			return this[id];
		}
		catch(KeyNotFoundException e) {
			Debug.LogError("Tentativa de acessar um objeto que não esta no dicionario... noob.\n"+e.StackTrace);
			return new ObjectState();
		}		
	}
	internal ObjectState Get(Collectable gameobject) {
		var id = gameobject.GetComponent<Collectable>().ID;
		return this[id];
	}
}
