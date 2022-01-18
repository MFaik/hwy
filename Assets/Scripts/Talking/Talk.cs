using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class Talk : MonoBehaviour
{
    [SerializeField] string Name;

    [SerializeField] UnityEvent[] OnTextTrigger;
 
    int m_currentScene;

    List<string> m_textMessageConditions;
    List<List<TextMessage>> m_textMessageList;

    void Start() {
        m_currentScene = SceneManager.GetActiveScene().buildIndex;
        GetTextMessages();
    }

    public void StartTalk() {//FIXME problematic remove listener
        for(int i = 0;i < m_textMessageConditions.Count;i++){
            if(BooleanParser.Parse(ReplaceFlagsWithBools(m_textMessageConditions[i]))){
                TextBoxManager.Instance.SetText(m_textMessageList[i], Name);
                TextBoxManager.Instance.OnTextTrigger.AddListener(InvokeTextTrigger);
                TextBoxManager.Instance.OnTextFinish.AddListener(RemoveTextTrigger);
                return;
            }
        }
    }

    void InvokeTextTrigger(int a) {
        if(OnTextTrigger.Length > a)
            OnTextTrigger[a].Invoke();
    }
    //HACK: problematic remove listener
    void RemoveTextTrigger() {
        TextBoxManager.Instance.OnTextTrigger.RemoveListener(InvokeTextTrigger);
        TextBoxManager.Instance.OnTextFinish.RemoveListener(RemoveTextTrigger);
    }

    void GetTextMessages() {
        m_textMessageConditions = new List<string>();
        m_textMessageList = new List<List<TextMessage>>();
        if(!TextFileReader.Texts[m_currentScene].ContainsKey(Name)){
            Debug.LogError("Name doesn't exist : " + Name);
            return;
        }
        string[] texts = TextFileReader.Texts[m_currentScene][Name].Split('[',']');
        for(int i = 1;i < texts.Length;i += 2){
            List<TextMessage> textMessageList = new List<TextMessage>();
            string condition = texts[i].Replace(" ","");
            string[] textMessages = texts[i+1].Split('"');
            for(int j = 0;j < textMessages.Length-1;j+=2){
                textMessageList.Add(new TextMessage(textMessages[j].Trim(),textMessages[j+1]));
            }
            m_textMessageConditions.Add(condition);
            m_textMessageList.Add(textMessageList);
        }
    }

    string ReplaceFlagsWithBools(string str) {
        StringBuilder strBuilder = new StringBuilder("",str.Length);
        int wordPointer = 0;
        string[] words = Regex.Matches(str,@"#?[\d\w]+").OfType<Match>().Select(m => m.Value).ToArray();

        for(int i = 0;i < str.Length;i++){
            if(str[i] == '#'){
                if(SaveSystem.GetProgress(words[wordPointer].Substring(1)))
                    strBuilder.Append('1');
                else
                    strBuilder.Append('0');
                
                wordPointer++;
                i++;
            } else if(Char.IsLetterOrDigit(str[i])){
                if(String.Equals(words[wordPointer], "1") || String.Equals(words[wordPointer], "true")){
                    strBuilder.Append('1');
                } else if(String.Equals(words[wordPointer], "0") || String.Equals(words[wordPointer], "false")){
                    strBuilder.Append('0');
                } else {
                    string wordWithName = Name + "_" + words[wordPointer];
                    
                    if(SaveSystem.SaveData.dialogueFlags.ContainsKey(wordWithName) && SaveSystem.SaveData.dialogueFlags[wordWithName])
                        strBuilder.Append('1');
                    else 
                        strBuilder.Append('0');
                }
                wordPointer++;
            }

            while(i < str.Length && (Char.IsLetterOrDigit(str[i]) || Char.IsWhiteSpace(str[i]))){
                i++;
            }
            if(i >= str.Length)
                break;
            
            strBuilder.Append(str[i]);
        }

        return strBuilder.ToString();
    }
}
