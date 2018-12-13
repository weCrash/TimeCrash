using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectState
{
	private bool wasTaken = false;

	public ObjectState() { }
	public ObjectState(ObjectState copy) {
		this.wasTaken = copy.Taken;
	}

	public void TakeIt() {
		this.wasTaken = true;
	}

	public bool Taken { get { return wasTaken; } }
}