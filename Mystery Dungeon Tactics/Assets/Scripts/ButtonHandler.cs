using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {
    public int id;
    
    
    // Start is called before the first frame update
    void Start() {
        GetComponent<Button>().onClick.AddListener(SelectMovementOption);
    }
    
    public void SelectMovementOption() {
        if (UiManager.selectedMovement != -1) {
            UiManager.positionButtons[UiManager.selectedMovement].GetComponent<Image>().color = Color.white;
        }
        
        UiManager.selectedMovement = id;
        
        UiManager.positionButtons[UiManager.selectedMovement].GetComponent<Image>().color = Color.red;
    }
    
}
