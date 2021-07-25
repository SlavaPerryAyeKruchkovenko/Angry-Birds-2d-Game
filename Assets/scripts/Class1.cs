using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class Class1: MonoBehaviour
{
    Quaternion Quaternion;
    public async void Start()
    {
        Quaternion = gameObject.transform.rotation;
        while ((int)gameObject.transform.rotation.eulerAngles.z != (int)((Quaternion.eulerAngles.z + 180) % 360))
        {
            var angle = (int)gameObject.transform.rotation.eulerAngles.z;
            gameObject.transform.Rotate(-Vector3.forward);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), ForceMode2D.Impulse);
            await Task.Delay(10);
        }
    }
    public async void Update()
    {
        
    }
}

