using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButtonHandler : MonoBehaviour {
    public int id;
    
    
    // Start is called before the first frame update
    void Start() {
        GetComponent<Button>().onClick.AddListener(SelectAbilityOption);
    }

    public void SelectAbilityOption() {
        if (UiManager.selectedAbilityId != -1) {
            UiManager.abilityButtons[UiManager.selectedAbilityId].GetComponent<Image>().color = Color.white;
        }
        
        UiManager.selectedAbilityId = id;
        
        UiManager.abilityButtons[UiManager.selectedAbilityId].GetComponent<Image>().color = Color.red;
    }
    
}