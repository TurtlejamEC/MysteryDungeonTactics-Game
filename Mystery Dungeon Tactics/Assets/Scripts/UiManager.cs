using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {
    public GameObject mainUi;
    public Canvas worldCanvas;
    public GameObject tileButton;

    public GameObject moveButton;
    public GameObject confirmButton;

    public static bool hasFinishedControlling = false;

    public static List<GameObject> positionButtons;
    public static bool hasSelectedMovement = false;
    public static int selectedMovement = -1;

    private void Awake() {
        mainUi.SetActive(false);
    }

    public void BeginPlayerTurn() {
        UiManager.hasFinishedControlling = false;
        mainUi.SetActive(true);
        moveButton.SetActive(true);
    }

    public void MoveButtonPress() {
        moveButton.SetActive(false);
        StartCoroutine(DisplayMoveUi());
    }

    public void ConfirmButtonPress() {
        if (selectedMovement == -1) return;

        MapPosition destination = new MapPosition(-1, -1);
        for (int i = 0; i < positionButtons.Count; i++) {
            if (i == selectedMovement) {
                destination.X = (int)(positionButtons[i].transform.position.x + 0.5);
                destination.Z = (int)(positionButtons[i].transform.position.z + 0.5);
            }
            
            Destroy(positionButtons[i]);
        }
        
        positionButtons.Clear();
        
        int currentId = TurnManager.ProgressQueue.Peek().CharacterId;
        Character current = CharacterManager.ActiveCharacters[currentId];
        
        RawMapManager.Map[current.Position.Z][current.Position.X].CharacterId = -1;
        RawMapManager.Map[destination.Z][destination.X].CharacterId = currentId;
        CharacterManager.ActiveCharacters[currentId].Position = destination;
        CharacterManager.ActiveCharacters[currentId].Parent.transform.position = new Vector3(destination.X, 0, destination.Z);
        
        confirmButton.SetActive(false);
        hasSelectedMovement = true;
    }
    
    IEnumerator DisplayMoveUi() {
        int currentId = TurnManager.ProgressQueue.Peek().CharacterId;
        Character current = CharacterManager.ActiveCharacters[currentId];

        List<AlgorithmTile> possiblePositions = MapAlgorithms.ResultMapToAlgorithmTileList(
            MapAlgorithms.BfsReturnRange(RawMapManager.Map, current.Position, current.Movement, true, true)
        );

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

        hasSelectedMovement = false;
        selectedMovement = -1;
        confirmButton.SetActive(true);
        yield return new WaitUntil(() => hasSelectedMovement);
        hasFinishedControlling = true;
    }
}
