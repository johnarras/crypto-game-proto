using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class AnimatorBehaviour : BaseBehaviour
{
    public Animator MainAnimator;


    public void SetAnimInt(string intName, int value, float delay, VoidDelegate callback)
    {
        if (MainAnimator != null)
        {
            MainAnimator.SetInteger(intName, value);
        }

        StartAnimDelay(delay, callback);
    }

    public void SetAnimBool(string boolName, bool value, float delay, VoidDelegate callback)
    {
        if (MainAnimator != null)
        {
            MainAnimator.SetBool(boolName, value);
        }

        StartAnimDelay(delay, callback);
    }

    public void TriggerAnimation(string triggerName, float delay, VoidDelegate callback)
    {
        if (MainAnimator != null)
        {
            MainAnimator.SetTrigger(triggerName);
        }

        StartAnimDelay(delay, callback);
    }

    public void SetAnimFloat(string floatName, float val, float delay, VoidDelegate callback)
    {
        if (MainAnimator != null)
        {
            MainAnimator.SetFloat(floatName, val);
        }

        StartAnimDelay(delay, callback);
    }

    private void StartAnimDelay(float delay, VoidDelegate callback)
    {
        StartCoroutine(InnerAnimDelay(delay, callback));
    }

    private IEnumerator InnerAnimDelay(float delay, VoidDelegate callback)
    {
        yield return new WaitForSeconds(delay);
        if (callback != null)
        {
            callback();
        }
    }


}
