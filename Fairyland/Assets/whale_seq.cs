using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class whale_seq : MonoBehaviour
{
    public Transform whale;
    public GameObject whaleObject;

    public GameObject menuCanvus;
    public GameObject angryCanvas;
    public GameObject happyCanvas;
    public GameObject surpriseCanvas;
    public GameObject backgroundPlane;
    public GameObject correctCanvas;
    public GameObject failCanvas;
    public GameObject menuText;
    public Button lineButton;

    //public Transform newWhale;
    //public GameObject newWhaleObject;

    public float moveDuration = 1.0f;
    public float scaleDuration = 1.0f;
    public float rotationDuration = 1.0f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 scaleVelocity = Vector3.zero;
    private float rotationVelocity;

    void Start()
    {
        if (whale == null)
        {
            Debug.LogError("Whale transform is not assigned.");
        }
        //if (newWhale == null)
        //{
        //    Debug.LogError("New Whale transform is not assigned.");
        //}

        angryCanvas.SetActive(false);
        happyCanvas.SetActive(false);
        surpriseCanvas.SetActive(false);
        menuCanvus.SetActive(false);
        //newWhaleObject.SetActive(false);
        correctCanvas.SetActive(false);
        failCanvas.SetActive(false);
        menuText.SetActive(false);
        Debug.Log("set all canvas to false");

        //backgroundPlane.SetActive(false);
    }

    public void StartSequence()
    {

        //if (newWhale == null)
        //{
        //    Debug.LogError("Attempted to start sequence but New Whale Transform is not assigned.");
        //    return;
        //}
        //else
        //{
        //    Debug.Log("newWhale is not null!");
        //}
        lineButton.interactable = false;
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        //if (whale == null || newWhale == null || newWhaleObject == null)
        //{
        //    Debug.LogError("One or more required objects are null.");
        //    yield break;
        //}

        backgroundPlane.SetActive(false);

        Debug.Log(whale.position);

        Vector3 targetPosition = new Vector3(0.0f, -1.5f, 0.0f);
        Vector3 targetScale = new Vector3(438.0f, 438.0f, 438.0f);
        Quaternion originalRotation = whale.rotation;
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

        menuCanvus.SetActive(true);
        menuText.SetActive(true);

    }
}
