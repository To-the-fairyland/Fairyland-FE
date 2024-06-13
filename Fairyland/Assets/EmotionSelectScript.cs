using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmotionSelectScript : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;

    public Button GoToEmotionButton;
    public GameObject GoToEmotionCanvas;

    public GameObject EmotionSelectCanvas;
    public GameObject AskEmotionCanvas;

    public GameObject AngryCanvas;
    public GameObject HappyCanvas;
    public GameObject SurpriseCanvas;
    public GameObject SadCanvas;
    public GameObject FearCanvas;
    public GameObject CalmCanvas;

    public Animator whaleAnimator;
    public Material[] expressions;

    public GameObject CorrectCanvas;
    public GameObject WrongCanvas;

    public GameObject WhaleObject;
    public GameObject Bubble;
    public Transform whale;

    public Button AngryButton;
    public Button HappyButton;
    public Button SurpriseButton;
    public Button SadButton;
    public Button FearButton;
    public Button CalmButton;

    public GameObject AskLineGuessCanvas;
    public GameObject SpeakStartCanvas;
    public GameObject SpeakStopCanvas;

    public Button SpeakStartButton;

    private int prevSelection = 0;
    private bool isDescriptionShown = false;

    private float moveDuration = 1.0f;
    private float scaleDuration = 1.0f;
    private float rotationDuration = 1.0f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 scaleVelocity = Vector3.zero;
    private float rotationVelocity;

    public Vector3 originalPosition;
    public Quaternion originalRotation;

    private Book BookClass;
    private SpeakingScript speakScript;
    private NaverTTSManager TTSManager;

    private int answerEmotion = 6;

    public Camera uiCamera;

    private string[] emotionKoreanArray = { "평온함", "기쁨", "슬", "화남", "무서움", "놀라움", "Main" };
    string[] emotionArray = { "Calm", "Happy", "Sad", "Angry", "Fear", "Surprised", "Main" };


    void Start()
    {
        AngryCanvas.SetActive(false);
        HappyCanvas.SetActive(false);
        SurpriseCanvas.SetActive(false);
        SadCanvas.SetActive(false);
        FearCanvas.SetActive(false);
        CalmCanvas.SetActive(false);
        EmotionSelectCanvas.SetActive(false);
        //newWhaleObject.SetActive(false);
        CorrectCanvas.SetActive(false);
        WrongCanvas.SetActive(false);
        AskEmotionCanvas.SetActive(false);
        AskLineGuessCanvas.SetActive(false);
        GoToEmotionCanvas.SetActive(false);

        BookClass = GetComponent<Book>();
        speakScript = GetComponent<SpeakingScript>();
        TTSManager = GetComponent<NaverTTSManager>();

        originalPosition = whale.position;
        originalRotation = whale.rotation;

    }

    public void OnGotoEmotionButtonPress()
    {

        StartCoroutine(StartEmotionSequence());
    }

    public void ExpressionSelectButton(int expression)
    {
        StartCoroutine(selectionButton(expression));
    }

    IEnumerator StartEmotionSequence()
    {
        //if (whale == null || newWhale == null || newWhaleObject == null)
        //{
        //    Debug.LogError("One or more required objects are null.");
        //    yield break;
        //}

        Bubble.SetActive(false);
        AskLineGuessCanvas.SetActive(false);
        SpeakStartCanvas.SetActive(false);
        SpeakStopCanvas.SetActive(false);
        GoToEmotionCanvas.SetActive(false);

        Vector3 screenPosition = new Vector3(Screen.width / 2, Screen.height / 3 + 50.0f, -100.0f);
        Vector3 targetPosition = uiCamera.ScreenToWorldPoint(screenPosition);
        Vector3 targetScale = new Vector3(550.0f, 550.0f, 550.0f);
        //Quaternion originalRotation = whale.rotation;
        originalPosition = whale.position;
        originalRotation = whale.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 180, 0);

        float elapsedTime = 0;

        while (Vector3.Distance(whale.position, targetPosition) > 3.0f || Vector3.Distance(whale.localScale, targetScale) > 3.0f)
        {
            whale.position = Vector3.SmoothDamp(whale.position, targetPosition, ref velocity, moveDuration, Mathf.Infinity, Time.deltaTime);
            whale.localScale = Vector3.SmoothDamp(whale.localScale, targetScale, ref scaleVelocity, scaleDuration, Mathf.Infinity, Time.deltaTime);
            whale.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / rotationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        whale.position = targetPosition;
        whale.localScale = targetScale;
        whale.rotation = targetRotation;

        EmotionSelectCanvas.SetActive(true);
        AskEmotionCanvas.SetActive(true);
        AngryButton.interactable = true;
        HappyButton.interactable = true;
        SurpriseButton.interactable = true;
        SadButton.interactable = true;
        FearButton.interactable = true;
        CalmButton.interactable = true;

        TTSManager.GetAndPlaySpeech("vdain", "Neutral", "주인공이 어떠한 감정을 느낄까?", "AskLineGuess");

    }

    private IEnumerator selectionButton(int expression)
    {
        AngryButton.interactable = false;
        HappyButton.interactable = false;
        SurpriseButton.interactable = false;
        SadButton.interactable = false;
        FearButton.interactable = false;
        CalmButton.interactable = false;

        AskEmotionCanvas.SetActive(false);
        whaleAnimator.SetInteger("NextInt", expression);
        changeExpression(expression);
        if (!isDescriptionShown)
        {
            if (expression == 1) // angry
            {
                HappyCanvas.SetActive(false);
                AngryCanvas.SetActive(true);
                SurpriseCanvas.SetActive(false);
                SadCanvas.SetActive(false);
                FearCanvas.SetActive(false);
                CalmCanvas.SetActive(false);
                TTSManager.GetAndPlaySpeech("vdain", "Angry", "화났어요.", "Angry");
                prevSelection = 1;
            }
            else if (expression == 2) // happy
            {
                AngryCanvas.SetActive(false);
                HappyCanvas.SetActive(true);
                SurpriseCanvas.SetActive(false);
                SadCanvas.SetActive(false);
                FearCanvas.SetActive(false);
                CalmCanvas.SetActive(false);
                TTSManager.GetAndPlaySpeech("vdain", "Happy", "기뻐요.", "Happy");
                prevSelection = 2;
            }
            else if (expression == 3) // surprised
            {
                SurpriseCanvas.SetActive(true);
                HappyCanvas.SetActive(false);
                AngryCanvas.SetActive(false);
                SadCanvas.SetActive(false);
                FearCanvas.SetActive(false);
                CalmCanvas.SetActive(false);
                TTSManager.GetAndPlaySpeech("vdain", "Sad", "놀랐어요.", "Surprised");
                prevSelection = 3;
            }
            else if (expression == 4) // Sad
            {
                SadCanvas.SetActive(true);
                HappyCanvas.SetActive(false);
                AngryCanvas.SetActive(false);
                SurpriseCanvas.SetActive(false);
                FearCanvas.SetActive(false);
                CalmCanvas.SetActive(false);
                TTSManager.GetAndPlaySpeech("vdain", "Sad", "슬퍼요.", "Sad");
                prevSelection = 4;

            }
            else if (expression == 5) // Fear
            {
                FearCanvas.SetActive(true);
                HappyCanvas.SetActive(false);
                AngryCanvas.SetActive(false);
                SurpriseCanvas.SetActive(false);
                SadCanvas.SetActive(false);
                CalmCanvas.SetActive(false);
                TTSManager.GetAndPlaySpeech("vdain", "Sad", "무서워요.", "Fear");
                prevSelection = 5;
            }
            else if (expression == 6) // Calm
            {
                FearCanvas.SetActive(true);
                HappyCanvas.SetActive(false);
                AngryCanvas.SetActive(false);
                SurpriseCanvas.SetActive(false);
                SadCanvas.SetActive(false);
                CalmCanvas.SetActive(true);
                TTSManager.GetAndPlaySpeech("vdain", "Happy", "평온해요.", "Calm");
                prevSelection = 6;
            }

            isDescriptionShown = true;
            yield return new WaitForSeconds(1);
            AngryButton.interactable = true;
            HappyButton.interactable = true;
            SurpriseButton.interactable = true;
            SadButton.interactable = true;
            FearButton.interactable = true;
            CalmButton.interactable = true;
            whaleAnimator.SetInteger("NextInt", 0);
            changeExpression(0);

            //menuCanvas.SetActive(false);
        }
        else
        {
            if (expression == prevSelection)
            {
                StartCoroutine(ShowFeedbackAndHide());

            }
            else
            {
                whaleAnimator.SetInteger("NextInt", expression);
                changeExpression(expression);
                if (expression == 1)
                {
                    HappyCanvas.SetActive(false);
                    AngryCanvas.SetActive(true);
                    SurpriseCanvas.SetActive(false);
                    SadCanvas.SetActive(false);
                    FearCanvas.SetActive(false);
                    CalmCanvas.SetActive(false);
                    TTSManager.GetAndPlaySpeech("vdain", "Angry", "화났어요.", "Angry");
                    prevSelection = 1;
                }
                else if (expression == 2)
                {
                    AngryCanvas.SetActive(false);
                    HappyCanvas.SetActive(true);
                    SurpriseCanvas.SetActive(false);
                    SadCanvas.SetActive(false);
                    FearCanvas.SetActive(false);
                    CalmCanvas.SetActive(false);
                    TTSManager.GetAndPlaySpeech("vdain", "Happy", "기뻐요.", "Happy");
                    prevSelection = 2;
                }
                else if (expression == 3) // Surprised
                {
                    SurpriseCanvas.SetActive(true);
                    HappyCanvas.SetActive(false);
                    AngryCanvas.SetActive(false);
                    SadCanvas.SetActive(false);
                    FearCanvas.SetActive(false);
                    CalmCanvas.SetActive(false);
                    TTSManager.GetAndPlaySpeech("vdain", "Sad", "놀랐어요.", "Surprised");
                    prevSelection = 3;
                }
                else if (expression == 4) // Sad
                {
                    SadCanvas.SetActive(true);
                    HappyCanvas.SetActive(false);
                    AngryCanvas.SetActive(false);
                    SurpriseCanvas.SetActive(false);
                    FearCanvas.SetActive(false);
                    CalmCanvas.SetActive(false);
                    TTSManager.GetAndPlaySpeech("vdain", "Sad", "슬퍼요.", "Sad");
                    prevSelection = 4;

                }
                else if (expression == 5) // Fear
                {
                    FearCanvas.SetActive(true);
                    HappyCanvas.SetActive(false);
                    AngryCanvas.SetActive(false);
                    SurpriseCanvas.SetActive(false);
                    SadCanvas.SetActive(false);
                    CalmCanvas.SetActive(false);
                    TTSManager.GetAndPlaySpeech("vdain", "Sad", "무서워요.", "Fear");
                    prevSelection = 5;
                }
                else if (expression == 6) // Calm
                {
                    FearCanvas.SetActive(false);
                    HappyCanvas.SetActive(false);
                    AngryCanvas.SetActive(false);
                    SurpriseCanvas.SetActive(false);
                    SadCanvas.SetActive(false);
                    CalmCanvas.SetActive(true);
                    TTSManager.GetAndPlaySpeech("vdain", "Happy", "평온해요.", "Calm");
                    prevSelection = 6;
                }

                isDescriptionShown = true;
                yield return new WaitForSeconds(1);
                AngryButton.interactable = true;
                HappyButton.interactable = true;
                SurpriseButton.interactable = true;
                SadButton.interactable = true;
                FearButton.interactable = true;
                CalmButton.interactable = true;
                whaleAnimator.SetInteger("NextInt", 0);
                changeExpression(0);

            }

        }

    }

    public void changeExpression(int expression)
    {
        SkinnedMeshRenderer renderer = skinnedMeshRenderer;
        if (renderer != null)
        {
            renderer.material = expressions[expression];
        }
        else
        {
            Debug.LogError("SkinnedMeshRenderer not found on the game object.");
        }
    }



    private IEnumerator ShowFeedbackAndHide()
    {
        int[] fixedAnswer = { 6, 2, 4, 1, 5, 3, 7};
        int Answer = BookClass.emotionInteger;

        if (Answer == 6 || Answer == null)
        {
            prevSelection = 7;
        }

        if (prevSelection == fixedAnswer[Answer])
        {
            HappyCanvas.SetActive(false);
            AngryCanvas.SetActive(false);
            SurpriseCanvas.SetActive(false);
            SadCanvas.SetActive(false);
            FearCanvas.SetActive(false);
            CalmCanvas.SetActive(false);
            CorrectCanvas.SetActive(true);
            TTSManager.GetAndPlaySpeech("vdain", "Happy", "맞았어요!", "Correct");

        }
        else
        {
            HappyCanvas.SetActive(false);
            AngryCanvas.SetActive(false);
            SurpriseCanvas.SetActive(false);
            SadCanvas.SetActive(false);
            FearCanvas.SetActive(false);
            CalmCanvas.SetActive(false);
            WrongCanvas.SetActive(true);

            TTSManager.GetAndPlaySpeech("vdain", "Sad", "틀렸어요.", "Wrong");

        }

        yield return new WaitForSeconds(4);

        StartCoroutine(gotoWhaleOriginalPosition());

        SpeakStartCanvas.SetActive(true);
        SpeakStartButton.interactable = true;
        AskLineGuessCanvas.SetActive(true);

        string guideText = BookClass.guideText;

        if (!string.IsNullOrEmpty(guideText))
        {
            TTSManager.GetAndPlaySpeech("ndain", "Neutral", guideText, "Guide");
            Debug.Log("Guide Text: " + guideText);

        }
    }

    public IEnumerator gotoWhaleOriginalPosition()
    {
        CorrectCanvas.SetActive(false);
        WrongCanvas.SetActive(false);
        EmotionSelectCanvas.SetActive(false);

        //whaleObject.SetActive(false);

        isDescriptionShown = false;

        Bubble.SetActive(true);


        //Vector3 targetPosition = new Vector3(5.88f, 1.42f, 90.00f);
        //Vector3 targetPosition = new Vector3(5.46f, 1.35f, 89.99f);
        Vector3 targetPosition = originalPosition;
        //Vector3 targetScale = new Vector3(195.0f, 195.0f, 195.0f);
        Vector3 targetScale = new Vector3(250.0f, 250.0f, 250.0f);
        //Quaternion targetRotation = Quaternion.Euler(15, 200, 0);
        Quaternion targetRotation = originalRotation;

        float elapsedTime = 0;

        whaleAnimator.SetInteger("NextInt", 0);
        changeExpression(0);

        while (Vector3.Distance(whale.position, targetPosition) > 3.0f || Vector3.Distance(whale.localScale, targetScale) > 3.0f)
        {
            whale.position = Vector3.SmoothDamp(whale.position, targetPosition, ref velocity, moveDuration, Mathf.Infinity, Time.deltaTime);
            whale.localScale = Vector3.SmoothDamp(whale.localScale, targetScale, ref scaleVelocity, scaleDuration, Mathf.Infinity, Time.deltaTime);
            whale.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / rotationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        whale.position = targetPosition;
        whale.localScale = targetScale;
        whale.rotation = targetRotation;

        
        //BookClass.WhaleSpeak();

        //BookClass.StartGotoOriginalPos();
    }

    public void gotoOriginalAtOnce()
    {
        CorrectCanvas.SetActive(false);
        WrongCanvas.SetActive(false);
        EmotionSelectCanvas.SetActive(false);

        //whaleObject.SetActive(false);

        isDescriptionShown = false;

        Bubble.SetActive(true);

        Vector3 targetPosition = originalPosition;
        Vector3 targetScale = new Vector3(250.0f, 250.0f, 250.0f);
        Quaternion targetRotation = originalRotation;

        whaleAnimator.SetInteger("NextInt", 0);
        changeExpression(0);

        whale.position = targetPosition;
        whale.localScale = targetScale;
        whale.rotation = targetRotation;

       


    }

}


