using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ToggleButtonscript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        var value = gameObject.GetComponent<Toggle>().value;
        ChangeConditional(value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeConditional(bool value)
	{
        var animator = GetComponent<Animator>();
        if(animator)
            animator.SetBool("IsPress", value);
	}
}
