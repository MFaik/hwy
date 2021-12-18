using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BooleanToken {
    LogicAnd, LogicOr, LogicNot, ParOpen, ParClose, BoolTrue, BoolFalse
}

public static class BooleanParser
{
    static List<BooleanToken> s_tokens;

    static int s_pointer;
    public static bool Parse(string str) {
        Tokenize(str);

        s_pointer = 0;
        return ParseTerm(s_tokens.Count);
    }

    static void Tokenize(string str) {
        s_tokens = new List<BooleanToken>();
        str = str.Replace(" ","");
        foreach(char c in str){
            if(c == '!')
                s_tokens.Add(BooleanToken.LogicNot);
            else if(c == '(')
                s_tokens.Add(BooleanToken.ParOpen);
            else if(c == ')')
                s_tokens.Add(BooleanToken.ParClose);
            else if(c == '1')
                s_tokens.Add(BooleanToken.BoolTrue);
            else if(c == '0')
                s_tokens.Add(BooleanToken.BoolFalse);
            else if(c == '&')
                s_tokens.Add(BooleanToken.LogicAnd);
            else if(c == '|')
                s_tokens.Add(BooleanToken.LogicOr);
        }
    }

    //inclusive start - exclusive end
    static bool ParseTerm(int end) {
        bool not = false;
        while(s_tokens[s_pointer] == BooleanToken.LogicNot){
            s_pointer++;
            not = !not;
        }
        bool left = ParseFactor();
        if(not)
            left = !left;

        if(s_pointer >= end)
            return left;

        if(s_tokens[s_pointer] == BooleanToken.LogicAnd){
            s_pointer++;
            bool right = ParseTerm(end);
            return left && right;
        }
        if(s_tokens[s_pointer] == BooleanToken.LogicOr){
            s_pointer++;
            bool right = ParseTerm(end);
            return left || right;
        }
        Debug.LogError(s_tokens[s_pointer]);
        return false;
    }
    
    static bool ParseFactor() {
        if(s_tokens[s_pointer] == BooleanToken.ParOpen){
            int tokenCount = FindParanthesisTokenCount(s_pointer);
            s_pointer++;
            bool ret = ParseTerm(s_pointer+tokenCount);
            s_pointer++;
            return ret;
        }
        if(s_tokens[s_pointer] == BooleanToken.BoolTrue){
            s_pointer++;
            return true;
        }
        if(s_tokens[s_pointer] == BooleanToken.BoolFalse){
            s_pointer++;
            return false;
        }
        Debug.LogError("TermErr");
        return false;
    }

    static int FindParanthesisTokenCount(int start) {
        if(s_tokens[start] != BooleanToken.ParOpen)
            return 0;
        int pointer = start;
        int parCount = 1;

        while(parCount > 0){
            pointer++;
            if(pointer >= s_tokens.Count)
                return -1;
            if(s_tokens[pointer] == BooleanToken.ParOpen)
                parCount++;
            else if(s_tokens[pointer] == BooleanToken.ParClose)
                parCount--;
        }
        return pointer-start-1;
    }
}
