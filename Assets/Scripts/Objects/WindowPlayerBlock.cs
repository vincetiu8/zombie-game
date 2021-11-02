using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowPlayerBlock : MonoBehaviour {
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag=="Enemy") {
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		}
	}
}
