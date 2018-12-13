using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions {
	public static Vector3 RoundOneDecimal(this Vector3 vector) {
		return new Vector3(
			(float)System.Math.Round(vector.x, 1),
			(float)System.Math.Round(vector.y, 1),
			(float)System.Math.Round(vector.z, 1)
			);
	}
}
