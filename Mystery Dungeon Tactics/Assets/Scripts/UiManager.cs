using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    enum PosSubstate {
        Attack,
        Move
    };
    
    public GameObject mainUi;
    public Canvas worldCanvas;
    public GameObject tileButton;

    public GameObject moveButton;
    public GameObject confirmButton;
    public GameObject restButton;
    public GameObject attackButton;

    public static bool hasFinishedControlling = false;

    public static List<GameObject> positionButtons;
    public static bool hasSelectedPosition = false;
    public static int selectedPositionId = -1;
    public static int currentPosSubstate = -1;

    private void Awake() {
        mainUi.SetActive(false);
    }

    public void BeginPlayerTurn() {
        hasFinishedControlling = false;
        mainUi.SetActive(true);
        if (CharacterManager.ActiveCharacters[TurnManager.ProgressQueue.Peek().CharacterId].CurrentEnergy > 0) {
            moveButton.SetActive(true);
            attackButton.SetActive(true);
        }

        restButton.SetActive(true);
    }

    public void MoveButtonPress() {
        moveButton.SetActive(false);
        restButton.SetActive(false);
        attackButton.SetActive(false);
        currentPosSubstate = (int)PosSubstate.Move;
        StartCoroutine(DisplayPositionUi());
    }

    public void RestButtonPress() {
        moveButton.SetActive(false);
        restButton.SetActive(false);
        attackButton.SetActive(false);
        int currentId = TurnManager.ProgressQueue.Peek().CharacterId;
        CharacterManager.ChangeEnergy(currentId, (int) (CharacterManager.ActiveCharacters[currentId].MaxEnergy * 0.5f));
        hasFinishedControlling = true;
    }
    
    public void AttackButtonPress() {
        moveButton.SetActive(false);
        restButton.SetActive(false);
        attackButton.SetActive(false);
        currentPosSubstate = (int)PosSubstate.Attack;
        StartCoroutine(DisplayPositionUi());
    }

    public void ConfirmButtonPress() {
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
        
        if (currentPosSubstate == (int) PosSubstate.Attack) {
            int otherId = RawMapManager.Map[selectedPosition.Z][selectedPosition.X].CharacterId;
            if (otherId != -1) {
                ActionManager.DealDamage(currentId, otherId);
                print(CharacterManager.ActiveCharacters[otherId]);
            }
        } else {
            ActionManager.MoveCharacter(currentId, selectedPosition);
        }
        
        confirmButton.SetActive(false);
        hasSelectedPosition = true;
    }
    
    IEnumerator DisplayPositionUi() {
        int currentId = TurnManager.ProgressQueue.Peek().CharacterId;
        Character current = CharacterManager.ActiveCharacters[currentId];

        List<AlgorithmTile> possiblePositions;
        
        if (currentPosSubstate == (int) PosSubstate.Attack) {
            possiblePositions = MapAlgorithms.ResultMapToAlgorithmTileList(
                MapAlgorithms.BfsReturnRange(RawMapManager.Map, current.Position, 1, false, true)
            );
        } else {
            possiblePositions = MapAlgorithms.ResultMapToAlgorithmTileList(
                MapAlgorithms.BfsReturnRange(RawMapManager.Map, current.Position, Math.Min(current.Movement, current.CurrentEnergy), true, true)
            );
        }
        

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
        confirmButton.SetActive(true);
        yield return new WaitUntil(() => hasSelectedPosition);
        hasFinishedControlling = true;
    }
    
    
}
