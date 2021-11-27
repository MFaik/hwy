using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RandomAnimation : MonoBehaviour
{
    [SerializeField] int AnimationCount = 2;

    void Start()
    {
        GetComponent<Animator>().SetInteger("Random",Random.Range(0,AnimationCount));       
    }
}
