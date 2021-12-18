using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;

public enum EmoteEnum { neutral, happy, mad, sad, suprised, tired };

public class TextMessage 
{
    public TextMessage(string character, string text) {
        Character = character;
        Text = text;
    }

    public string Character;
    public string Text;
}  

public class TextBoxManager : MonoBehaviour
{
    public static UnityEvent OnTextStart = new UnityEvent();
    public static UnityEvent OnTextFinish = new UnityEvent();
    public static UnityEvent<int> OnTextTrigger = new UnityEvent<int>();

    [SerializeField] GameObject TextBox;
    [SerializeField] Image PP;
    [SerializeField] TextMeshProUGUI TextMesh;

    static TextBoxManager s_instance;

    static Queue<TextMessage> s_textMessages = new Queue<TextMessage>();
    static bool s_isTextActive = false;

    static string s_textIndex;

    Dictionary<string,Sprite[]> m_emotes;
    Dictionary<string,EmoteEnum> m_emoteStringToEnum;

    TextMessage m_currentTextMessage;

    int m_characterCount = 0;
    float m_startTime = 0;    
    float m_defaultSpeed = 15;
    float m_speed;
    float m_textWaitTimer = 0;

    void Start() {
        if(s_instance)
            Destroy(gameObject);
        s_instance = this;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerInput>().actions["Interact"].started += AdvanceText;

        //optimization
        m_emoteStringToEnum = new Dictionary<string, EmoteEnum>();
        int counter = 0;
        foreach(string name in Enum.GetNames(typeof(EmoteEnum))) {  
            m_emoteStringToEnum[name] = (EmoteEnum)counter++;
        }  

        Texture2D[] textures = Resources.LoadAll<Texture2D>("Emotes");
        m_emotes = new Dictionary<string, Sprite[]>();
        foreach(Texture2D texture in textures){
            m_emotes[texture.name] = Resources.LoadAll<Sprite>("Emotes/" + texture.name);
        }
    } 

    public static void SetText(TextMessage text, string textIndex) {
        s_textIndex = textIndex; 
        s_textMessages.Enqueue(text);

        if(!s_isTextActive)
            s_instance.StartCoroutine(nameof(ReadText));
    }

    public static void SetText(List<TextMessage> textList, string textIndex) {
        s_textIndex = textIndex;
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

        m_currentTextMessage = s_textMessages.Dequeue();

        if(!m_emotes.ContainsKey(m_currentTextMessage.Character)){
            Debug.LogError("Emotes doesn't exist for character: " + m_currentTextMessage.Character);
            yield break;
        }

        PP.sprite = m_emotes[m_currentTextMessage.Character][(int)EmoteEnum.neutral];

        string[] parsedText = m_currentTextMessage.Text.Split('<', '>');
        
        string text = "";
        m_characterCount = 0;
        for(int i = 0;i < parsedText.Length;i++){
            if(i % 2 == 1){
                parsedText[i] = parsedText[i].Replace(" ","");
                if(!IsCustomTag(parsedText[i])){
                    text += $"<{parsedText[i]}>";
                }
            } else{
                text += parsedText[i];
                m_characterCount += parsedText[i].Length;
            }
        }

        TextMesh.SetText(text);
        TextMesh.maxVisibleCharacters = 0;

        m_speed = m_defaultSpeed;
        for(int i = 0;i < parsedText.Length;i++){
            if(i % 2 == 1){
                EvaluateTag(parsedText[i]);
            } else{
                for(int characterCount = 0;characterCount < parsedText[i].Length;characterCount++){
                    TextMesh.maxVisibleCharacters++;
                    yield return StoppableWaitForSeconds(1 / m_speed);
                }
            }
        }
    }

    IEnumerator StoppableWaitForSeconds(float seconds) {
        for(m_textWaitTimer = seconds;m_textWaitTimer >= 0;m_textWaitTimer -= Time.deltaTime){
            yield return 0;
        }
    }

    bool IsCustomTag(string tag) {
        return StringStartsWith(tag, "speed=") || StringStartsWith(tag, "emote=") || StringStartsWith(tag, "set=") || StringStartsWith(tag, "trigger=");
    }

    void EvaluateTag(string tag) {
        if (StringStartsWith(tag, "speed=")){
            if(m_speed < 100){
                m_speed = float.Parse(tag.Split('=')[1], CultureInfo.InvariantCulture);
            }
        } else if(StringStartsWith(tag, "set=")){
            SaveSystem.SaveData.dialogueFlags[s_textIndex + "_" + tag.Split('=')[1]] = true;
        } else if(StringStartsWith(tag, "trigger=")){
            OnTextTrigger.Invoke(int.Parse(tag.Split('=')[1]));
        }
        else if (tag.StartsWith("emote="))
        {
            if(!m_emotes.ContainsKey(m_currentTextMessage.Character)){
                Debug.LogError("Emotes doesn't exist for character");
                return;
            }
            PP.sprite = m_emotes[m_currentTextMessage.Character][ (int)m_emoteStringToEnum[tag.Split('=')[1]] ];
        }
    }

    public static bool StringStartsWith(string a, string b) {
            int aLen = a.Length;
            int bLen = b.Length;
            
            int ap = 0, bp = 0;
            
            while(ap < aLen && bp < bLen && a[ap] == b[bp]){
                ap++;
                bp++;
            }

            return bp == bLen;
        }

    public void AdvanceText(InputAction.CallbackContext value) {
        if(!s_isTextActive)
            return;

        if((Time.time - m_startTime) < 0.1f)//stop input from getting read in the same frame as text activation
            return;

        if(TextMesh.maxVisibleCharacters < m_characterCount){
            m_speed = 100;
            m_textWaitTimer =  0;
        } else{
            StartCoroutine(nameof(ReadText));
        }
    }
}