using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	public GameObject prefab;
	// Start is called before the first frame update
    void Start()
    {
	    Instantiate(prefab,transform);
	    var obj = Instantiate(prefab,transform.position + prefab.transform.position,transform.rotation * prefab.transform.rotation,transform);
    	
    }

}
