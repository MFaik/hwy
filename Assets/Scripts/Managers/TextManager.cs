using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;

public enum Emotion { neutral, happy, mad, sad, suprised };

public class TextMessage 
{
    public TextMessage(string text, string character) {
        Text = text;
        Character = character;
    }

    public string Text;
    public string Character;
}  

public class TextManager : MonoBehaviour
{
    public static UnityEvent OnTextStart = new UnityEvent();
    public static UnityEvent OnTextFinish = new UnityEvent();

    [SerializeField] GameObject TextBox;
    [SerializeField] Image PP;
    [SerializeField] TextMeshProUGUI TextMesh;

    static TextManager s_instance;

    static Queue<TextMessage> s_textMessages = new Queue<TextMessage>();
    static bool s_isTextActive = false;

    int m_characterCount = 0;
    float m_startTime = 0;    
    [SerializeField] float DefaultSpeed = 10;
    float m_speed;

    void Start() {
        if(s_instance)
            Destroy(gameObject);
        s_instance = this;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerInput>().actions["Interact"].started += AdvanceText;
    } 

    public static void SetText(TextMessage text) {
        s_textMessages.Enqueue(text);

        if(!s_isTextActive)
            s_instance.StartCoroutine(nameof(ReadText));
    }

    public static void SetText(List<TextMessage> textList) {
        foreach(TextMessage text in textList)
            s_textMessages.Enqueue(text);
        
        if(!s_isTextActive)
            s_instance.StartCoroutine(nameof(ReadText));
    }

    IEnumerator ReadText() {
        if(s_textMessages.Count <= 0){
            OnTextFinish.Invoke();
            s_isTextActive = false;
            TextBox.SetActive(false);
            yield break;
        }

        OnTextStart.Invoke();
        m_startTime = Time.time;
        s_isTextActive = true;
        TextBox.SetActive(true);

        TextMessage textMessage = s_textMessages.Dequeue();
        string[] parsedText = textMessage.Text.Split('<', '>');
        
        string text = "";
        m_characterCount = 0;
        for(int i = 0;i < parsedText.Length;i++){
            if(i % 2 == 1){
                parsedText[i] = parsedText[i].Replace(" ","");
                if(!IsCustomTag(parsedText[i])){
                    text += parsedText[i];
                }
            } else{
                text += parsedText[i];
                m_characterCount += parsedText[i].Length;
            }
        }

        TextMesh.text = text;
        TextMesh.maxVisibleCharacters = 0;

        m_speed = DefaultSpeed;
        for(int i = 0;i < parsedText.Length;i++){
            if(i % 2 == 1){
                EvaluateTag(parsedText[i]);
            } else{
                for(int characterCount = 0;characterCount < parsedText[i].Length;characterCount++){
                    TextMesh.maxVisibleCharacters++;
                    yield return new WaitForSeconds(1 / m_speed);
                }
            }
        }
    }

    bool IsCustomTag(string tag) {
        return tag.StartsWith("speed=") || tag.StartsWith("emotion=");
    }

    void EvaluateTag(string tag) {
        if (tag.StartsWith("speed="))
        {
            if(m_speed < 100)
                m_speed = float.Parse(tag.Split('=')[1]);
        }
        /*else if (tag.StartsWith("emotion="))
        {
            onEmotionChange.Invoke((Emotion)System.Enum.Parse(typeof(Emotion), tag.Split('=')[1]));
        }
        else if (tag.StartsWith("action="))
        {
            onAction.Invoke(tag.Split('=')[1]);
        }*/
    }

    public void AdvanceText(InputAction.CallbackContext value) {
        if(!s_isTextActive)
            return;

        if((Time.time - m_startTime) < 0.1f)//stop input from getting read in the same frame as text activation
            return;

        if(TextMesh.maxVisibleCharacters < m_characterCount){
            m_speed = 100;
        } else{
            StartCoroutine(nameof(ReadText));
        }
    }
}