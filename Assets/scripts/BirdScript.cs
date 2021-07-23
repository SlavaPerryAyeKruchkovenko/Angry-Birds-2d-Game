using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Birds;

public class BirdScript : MonoBehaviour
{
<<<<<<< HEAD
    public Birds TypeOfBird;

    private Bird bird;
    
    void Start()
    {
        bird = Bird.GetBird(TypeOfBird);
=======
    public BirdScript(Bird bird)
	{
        this.BirdType = bird;
	}
    public Bird BirdType { get; }
    // Start is called before the first frame update
    void Start()
    {
        BirdType.StartLocation = transform.position;
>>>>>>> main
    }

    
    void Update()
    {
        
    }
}
