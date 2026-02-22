using UnityEngine;
using TMPro; 
using Unity.Netcode; 

public class TextUpdater : NetworkBehaviour
{
    public TextMeshProUGUI uiTextElement; 
    


    public void UpdateText(string newText)
    {
        Debug.Log($"[TextUpdater] UpdateText called with: {newText}");
        if (uiTextElement != null)
        {
            uiTextElement.text = newText; 
        }
        else
        {
            Debug.LogError("UI Text Element reference not set in the Inspector!");
        }
    }
}