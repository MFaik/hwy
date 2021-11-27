using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeManager : MonoBehaviour
{
    public static void EditTime(float multiplier, float duration){
        Time.timeScale *= multiplier;
        DOVirtual.DelayedCall(duration,()=>{Time.timeScale /= multiplier;},true);
    }
}
