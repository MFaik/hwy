using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextFileReader : MonoBehaviour
{
    public static List<Dictionary<string,string>> Texts;

    static int m_currentPosition = 0;
    
    void Awake() {
        if(Texts == null){
            Texts = new List<Dictionary<string,string>>();
            for(int i = 0;i < SceneManager.sceneCountInBuildSettings;i++){
                Texts.Add(new Dictionary<string, string>());
            }
        }
        
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if(Texts[currentScene].Count == 0){
            TextAsset text = Resources.Load("Texts/CH" + currentScene) as TextAsset;

            m_currentPosition = 0;
            while(m_currentPosition < text.text.Length){
                string characterText = GetTextUntilCharExcludeQuotes(text.text, '}');
                if(characterText == null)
                    return;

                string[] characterTextArr = characterText.Split('{');

                if(characterTextArr.Length != 2){
                    Debug.LogError("Text format is wrong " + characterTextArr.Length);
                    return;
                }
                
                Texts[currentScene][characterTextArr[0].Trim()] = characterTextArr[1].Replace("\\n","\n");
            }
        }
    }

    static string GetTextUntilCharExcludeQuotes(string str, char endChar) {
        bool insideQuote = false;
        int startPosition = m_currentPosition;
        for(;m_currentPosition < str.Length;m_currentPosition++){
            if(str[m_currentPosition] == '"'){
                insideQuote = !insideQuote;
            }
            if(insideQuote)
                continue;
            if(str[m_currentPosition] == endChar){
                m_currentPosition++;
                return str.Substring(startPosition,m_currentPosition-startPosition-1);
            }
        }
        return null;
    }
}