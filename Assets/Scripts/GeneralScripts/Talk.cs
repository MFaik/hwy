using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Talk : MonoBehaviour
{
    void Start()
    {
        GetComponent<Interactable>().OnInteract.AddListener(TalkPlaceHolder);
    }

    void TalkPlaceHolder() {
        TextManager.SetText(new List<TextMessage>{
            new TextMessage("bir","oof"),
            new TextMessage("iki ve bekle <speed=1>. . .<speed=10> uc","oof"),
            new TextMessage("dort","oof"),
            new TextMessage("<speed=1> how long is thiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiis","oof")
        });
    }
}
