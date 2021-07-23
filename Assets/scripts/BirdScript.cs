using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Birds;

public class BirdScript : MonoBehaviour
{
    public Birds TypeOfBird;

    private Bird bird;
    
    void Start()
    {
        bird = Bird.GetBird(TypeOfBird);
    }

    
    void Update()
    {
        
    }
}
