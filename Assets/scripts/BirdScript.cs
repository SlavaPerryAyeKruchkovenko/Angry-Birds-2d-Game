using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public BirdScript(Bird bird)
	{
        this.BirdType = bird;
	}
    public Bird BirdType { get; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
