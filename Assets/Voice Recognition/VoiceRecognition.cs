using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour
{

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> phrases = new Dictionary<string, Action>();

    [Header("Testing")]
    public GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        phrases.Add("test", Test);
        phrases.Add("show me a sign", ShowMeASign);
        phrases.Add("are you here", IsHere);

        keywordRecognizer = new KeywordRecognizer(phrases.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecogniseSpeech;
        keywordRecognizer.Start();
    }

    private void RecogniseSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        phrases[speech.text]?.Invoke();
    }

    void Test()
    {
        Debug.Log("Calling test");
    }

    void ShowMeASign()
    {
        if(UnityEngine.Random.Range(0, 3) == 0)
        {
            Debug.Log("Showing you a sign");
            Instantiate(ball, new Vector3(0, 10, 0), Quaternion.identity);
        } else
        {
            Debug.Log("I will not show you a sign");
        }
    }

    void IsHere()
    {
        if(UnityEngine.Random.Range(0, 3) == 0)
        {
            Debug.Log("I am here");
        } else
        {
            Debug.Log("I am not here");
        }
    }
}
