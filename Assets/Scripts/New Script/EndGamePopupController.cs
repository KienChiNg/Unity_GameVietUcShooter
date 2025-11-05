using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGamePopupController : MonoBehaviour
{
    public TMP_Text networkTypeText;
    public TMP_Text pingText;
    public TMP_Text downloadSpeedText;
    public TMP_Text uploadSpeedText;
    public Transform resultContainer;
    public GameObject compressionResultPrefab;
    public GameObject ambuResultPrefab;
    public void ShowResultPanel(GameResult result, int maxCompression, int maxAmbu, int phaseCount)
    {
        ClearOldResult();
        //
        for (int i = 0; i < phaseCount; i++)
        {
            //
            Transform chestCompress = Instantiate(compressionResultPrefab, resultContainer).transform;
            UpdateResultChild(chestCompress, result.compressionCount[i], result.wrongCompressionCount[i], maxCompression);
             
            //
            Transform ambuMask = Instantiate(ambuResultPrefab, resultContainer).transform;
            UpdateResultChild(ambuMask, result.ambuCount[i], result.wrongAmbuCount[i], maxAmbu);
            
        }

        gameObject.SetActive(true);
    }

    public void UpdateResultChild(Transform child, int correctCount, int wrongCount, int targetCount)
    {
        TMP_Text[] childComponents = child.GetComponentsInChildren<TMP_Text>();
        childComponents[2].text = $"{correctCount + wrongCount}/{targetCount}";
        if (correctCount + wrongCount != targetCount)
        {
            childComponents[2].color = new Color32(217, 6, 20, 255);
        }
        else
        {
            childComponents[2].color = new Color32(6, 134, 96, 255);
        }

        if (correctCount < targetCount)
        {
            childComponents[3].text = "Mistimed";
            childComponents[3].color = new Color32(217, 6, 20, 255);
        }
        else
        {
            childComponents[3].text = "In time";
            childComponents[3].color = new Color32(6, 134, 96, 255);
        }
    }

    public void HideResultPopup()
    {
        ClearOldResult();
        gameObject.SetActive(false);
    }

    public void ClearOldResult()
    {
        for(int i = 0; i < resultContainer.childCount; i++)
        {
            Destroy(resultContainer.GetChild(i).gameObject);
        }
    }
}
