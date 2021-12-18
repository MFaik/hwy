using UnityEngine;
using DG.Tweening;

public class TimeManager
{
    public static void EditTime(float multiplier, float duration) {
        Time.timeScale *= multiplier;
        DOVirtual.DelayedCall(duration,()=>{Time.timeScale /= multiplier;},true);
    }
}
