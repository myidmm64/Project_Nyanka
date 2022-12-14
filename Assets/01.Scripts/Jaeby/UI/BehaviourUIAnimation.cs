using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BehaviourUIAnimation : MonoBehaviour
{
    private Animator _animator = null;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void GoOnMouse()
    {
        _animator.SetBool("OnMouse", true);
    }

    public void GoExitMouse()
    {
        _animator.SetBool("ExitMouse", true);
    }

    public void GoIdle()
    {
        _animator.SetBool("OnMouse", false);
        _animator.SetBool("ExitMouse", false);
    }

    public void ViewPlayerSkill()
    {
        ClickManager.Instance.CurrentPlayer?.ViewSkillRange();
    }

    public void ViewEnd()
    {
        ClickManager.Instance.CurrentPlayer?.ViewEndSkillRange();
    }
}
