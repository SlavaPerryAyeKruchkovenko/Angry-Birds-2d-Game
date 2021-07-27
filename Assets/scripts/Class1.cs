using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class Class1: MonoBehaviour
{
    public GameObject GameObject;

    public void Start()
    {
        
    }

    public void OnMouseDown()
    {
        GameObject.GetComponent<BirdScript>().ActivatePower();
    }

    public void Update()
    {
        
    }
}

