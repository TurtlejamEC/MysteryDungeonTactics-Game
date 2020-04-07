using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    enum UiState {
        Nothing,
        Settings,
        Action,
        ActionChoices,
        AbilityGraph,
        Map
    };

    enum ActionChoices {
        Move,
        Ability,
        Rest
    }
    
    public GameObject mainUi;
    public Canvas worldCanvas;
    public GameObject tileButton;

    public GameObject moveButton;
    public GameObject confirmButton;
    public GameObject restButton;
    public GameObject attackButton;
    public GameObject actionButton;
    public GameObject actionReturnButton;
    public GameObject[] actionChoiceButtons;

    public GameObject actionUiState;
    public GameObject actionChoicesUiState;
    public GameObject abilityGraphUiState;
    public GameObject mapUiState;

    public static bool hasFinishedControlling = false;

    public static List<GameObject> positionButtons;
    public static bool hasSelectedPosition = false;
    public static int selectedPositionId = -1;
    
    public static List<GameObject> abilityButtons;
    public static bool hasSelectedAbility = false;
    public static int selectedAbilityId = -1;
    
    public static int currentUiState = (int)UiState.Nothing;
    public static bool[] actionsDone;

    public bool returnFromAbilityGraphToActionChoices;
    public bool returnFromMapToAbilityGraph;
    public bool returnFromMapToActionChoices;

    private void Awake() {
        mainUi.SetActive(false);
    }

    public void AbilityGraphConfirmButtonPress() {
        if (selectedAbilityId == -1) return;

        hasSelectedAbility = true;
    }

    public void AbilityGraphToActionChoices() {
        abilityGraphUiState.SetActive(false);
        currentUiState = (int)UiState.ActionChoices;
        actionChoicesUiState.SetActive(true);
    }

    public void AbilityGraphToMap() {
        abilityGraphUiState.SetActive(false);
        currentUiState = (int)UiState.Map;
        mapUiState.SetActive(true);
        StartCoroutine(DisplayMapUiState());
    }

    public void AbilityGraphReturnButtonPress() {
        AbilityGraphToActionChoices();
    }

    public void ActionButtonPress() {
        ActionToActionChoices();
    }

    public void AbilityButtonPress() {
        ActionChoicesToAbilityGraph();
    }

    public void ActionChoicesToAbilityGraph() {
        actionChoicesUiState.SetActive(false);
        abilityGraphUiState.SetActive(true);
        currentUiState = (int)UiState.AbilityGraph;
        StartCoroutine(DisplayAbilityGraphUiState());
    }

    public void ActionChoicesToAction() {
        actionChoicesUiState.SetActive(false);
        actionUiState.SetActive(true);
        currentUiState = (int)UiState.Action;
    }

    public void ActionChoicesToMap() {
        actionChoicesUiState.SetActive(false);
        currentUiState = (int)UiState.Map;
        mapUiState.SetActive(true);
        StartCoroutine(DisplayMapUiState());
    }

    public void ActionReturnButtonPress() {
        ActionChoicesToAction();
    }

    public void ActionToActionChoices() {
        actionUiState.SetActive(false);
        actionChoicesUiState.SetActive(true);
        currentUiState = (int)UiState.ActionChoices;

        foreach (ActionChoices done in Enum.GetValues(typeof(ActionChoices)))
        {
            int index = (int)done;

            actionChoiceButtons[index].SetActive(false);
            if (!actionsDone[index]) {
                actionChoiceButtons[index].SetActive(true);
            }
        }

        if (CharacterManager.ActiveCharacters[TurnManager.ProgressQueue.Peek().CharacterId].CurrentEnergy <= 0) {
            actionChoiceButtons[(int)ActionChoices.Move].SetActive(false);
            actionChoiceButtons[(int)ActionChoices.Ability].SetActive(false);
        }
    }

    public void BeginPlayerTurn() {
        hasFinishedControlling = false;
        mainUi.SetActive(true);
        actionUiState.SetActive(true);
        currentUiState = (int)UiState.Action;
        actionsDone = new bool[Enum.GetNames(typeof(ActionChoices)).Length+1];
    }

    IEnumerator DisplayAbilityGraphUiState() {
        Transform abilityButtonsParent = abilityGraphUiState.transform.GetChild(0).GetChild(0).GetChild(0);
        abilityButtons = new List<GameObject>();
        foreach (Transform button in abilityButtonsParent) {
            button.gameObject.GetComponent<Image>().color = Color.white;
            abilityButtons.Add(button.gameObject);
        }

        hasSelectedAbility = returnFromAbilityGraphToActionChoices = false;
        selectedAbilityId = -1;
        yield return new WaitUntil(() => hasSelectedAbility || returnFromAbilityGraphToActionChoices);
        if (hasSelectedAbility) {
            AbilityGraphToMap();
        } else {
            AbilityGraphToActionChoices();
        }
    }

    IEnumerator DisplayMapUiState() {
        int currentId = TurnManager.ProgressQueue.Peek().CharacterId;
        Character current = CharacterManager.ActiveCharacters[currentId];

        List<AlgorithmTile> possiblePositions;

        possiblePositions = ActionManager.GetPossiblePositions(currentId);

        positionButtons = new List<GameObject>();

        for (int i = 0; i < possiblePositions.Count; i++) {
            GameObject newButton = Instantiate(tileButton);
            newButton.transform.SetParent(worldCanvas.transform, false);
            RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
            buttonRectTransform.anchorMax = buttonRectTransform.anchorMin = buttonRectTransform.pivot = Vector2.zero;
            buttonRectTransform.anchoredPosition = new Vector2(possiblePositions[i].Position.X, possiblePositions[i].Position.Z);

            newButton.GetComponent<ButtonHandler>().id = i;
            positionButtons.Add(newButton);
        }

        hasSelectedPosition = false;
        selectedPositionId = -1;
        returnFromMapToActionChoices = returnFromMapToAbilityGraph = false;
        yield return new WaitUntil(() => hasSelectedPosition || returnFromMapToActionChoices || returnFromMapToAbilityGraph);

        if (hasSelectedPosition) {
            if (selectedAbilityId == -1) {
                actionsDone[(int)ActionChoices.Move] = true;
            }
            else {
                actionsDone[(int)ActionChoices.Ability] = true;
            }

            selectedAbilityId = -1;

            MapToAction();
        } else {
            for (int i = 0; i < positionButtons.Count; i++) {
                Destroy(positionButtons[i]);
            }

            positionButtons.Clear();

            if (returnFromMapToAbilityGraph) {
                MapToAbilityGraph();
            } else {
                MapToActionChoices();
            }
        }
    }

    public void MapConfirmButtonPress() {
        if (selectedPositionId == -1) return;

        MapPosition selectedPosition = new MapPosition(-1, -1);
        for (int i = 0; i < positionButtons.Count; i++) {
            if (i == selectedPositionId) {
                selectedPosition.X = (int)(positionButtons[i].transform.position.x + 0.5f);
                selectedPosition.Z = (int)(positionButtons[i].transform.position.z + 0.5f);
            }

            Destroy(positionButtons[i]);
        }

        positionButtons.Clear();

        int currentId = TurnManager.ProgressQueue.Peek().CharacterId;

        if (selectedAbilityId >= 0) {
            int otherId = RawMapManager.Map[selectedPosition.Z][selectedPosition.X].CharacterId;
            if (otherId != -1) {
                ActionManager.DealDamage(currentId, otherId);
                print(CharacterManager.ActiveCharacters[otherId]);
            }
        } else {
            ActionManager.MoveCharacter(currentId, selectedPosition);
        }

        hasSelectedPosition = true;
    }

    public void MapReturnButtonPress() {
        if (selectedAbilityId == -1) {
            returnFromMapToActionChoices = true;
        } else {
            returnFromMapToAbilityGraph = true;
        }
    }

    public void MapToAction() {
        mapUiState.SetActive(false);
        actionUiState.SetActive(true);
        currentUiState = (int)UiState.Action;
        CameraManager.FollowNewCharacter(TurnManager.ProgressQueue.Peek().CharacterId);
    }

    public void MapToAbilityGraph() {
        mapUiState.SetActive(false);
        abilityGraphUiState.SetActive(true);
        currentUiState = (int)UiState.AbilityGraph;
        StartCoroutine(DisplayAbilityGraphUiState());
    }

    public void MapToActionChoices() {
        mapUiState.SetActive(false);
        actionUiState.SetActive(true);
        currentUiState = (int)UiState.ActionChoices;
    }

    public void MoveButtonPress() {
        ActionChoicesToMap();
    }

    public void RestButtonPress() {
        actionChoicesUiState.SetActive(false);
        int currentId = TurnManager.ProgressQueue.Peek().CharacterId;
        CharacterManager.ChangeEnergy(currentId, (int) (CharacterManager.ActiveCharacters[currentId].MaxEnergy * 0.5f));
        hasFinishedControlling = true;
    }
   
}
