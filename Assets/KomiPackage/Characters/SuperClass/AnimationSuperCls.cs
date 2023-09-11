using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationSuperCls : MonoBehaviour
{
    [SerializeField]
    private AnimatorController CharacterAnimatorController;

    protected Animator CharacterAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void SetAnimator()
    {
        if (GetComponent<Animator>() != null)
        {
            this.CharacterAnimator = GetComponent<Animator>();
            Debug.Log("CharacterにAnimatorコンポーネントが付与されているため、ユーザー設定で使用します。");
        }
        else
        {
            this.CharacterAnimator = gameObject.AddComponent<Animator>() as Animator;
            this.CharacterAnimator.runtimeAnimatorController = (RuntimeAnimatorController)this.CharacterAnimatorController;
            Debug.Log("CharacterにAnimatorコンポーネントが付与されていなかったので\nCharacterインスタンスに動的にAnimatorを追加しました。");
        }

       

    }

}
