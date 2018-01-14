using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	// Use this for initialization
    private Action recyleAction;
	void OnEnable ()
	{
	    StartCoroutine("RecyleObject");
	    this.recyleAction = Recyle;
	}

    IEnumerator RecyleObject()
    {
        yield return new WaitForSeconds(2);

        MyObjectPool.Recyle(this.gameObject,this.Recyle);
        
    }

    void OnDisable()
    {
        StopCoroutine("RecyleObject");
    }

    void Recyle()
    {
        Debug.Log("开始回收了");
        this.GetComponent<Rigidbody>().velocity=Vector3.one;
        this.GetComponent<Rigidbody>().angularVelocity=Vector3.zero;
        this.gameObject.SetActive(false);
    }
}
