using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {
	// Update is called once per frame
    public GameObject cube;
	void Update () {
	    if (Input.GetMouseButtonDown(0))
	    {
	        MyObjectPool.Spawn(this.cube, this.transform, Vector3.zero, Quaternion.identity);
	    }
	}
}
