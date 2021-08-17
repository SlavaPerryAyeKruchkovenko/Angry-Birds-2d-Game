using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void Awake()
    {
        var toggle = GetComponent<Toggle>();
        bool value = toggle.isOn;
        ChangeConditional(value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeConditional(bool value)
    {
        var animator = GetComponent<Animator>();
        if (animator)
            animator.SetBool("IsPress", value);
    }
}
