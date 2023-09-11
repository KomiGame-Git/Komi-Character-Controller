using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class HumanAnimationScript : AnimationSuperCls
{
    
    // Start is called before the first frame update
    void Start()
    {
        this.SetAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            this.CharacterAnimator.SetBool("isWalking", true);
        }
        else
        {
            this.CharacterAnimator.SetBool("isWalking", false);
        }

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && Input.GetKey(KeyCode.LeftControl))
        {
            this.CharacterAnimator.SetBool("isRunning", true);
        }
        else
        {
            this.CharacterAnimator.SetBool("isRunning", false);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.CharacterAnimator.SetTrigger("JumpTrigger");
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            this.CharacterAnimator.SetTrigger("SprintingForwardRollTrigger");
        }

    }
}
