using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerManager : MonoBehaviour {
    public RawMapManager rmManager;
    public CharacterManager charManager;
    public TurnManager tManager;
    public AiManager aiManager;
    public CameraManager camManager;
    public UiManager uiManager;

    public GameObject sampleCharacter;

    public void AddCharacter(Character newCharacter) {
        MapPosition generatedPosition = RawMapManager.FindEmptyFloorPosition();
        newCharacter.Position = generatedPosition;
        newCharacter.Parent.transform.position = new Vector3(generatedPosition.X, 0, generatedPosition.Z);
        RawMapManager.Map[generatedPosition.Z][generatedPosition.X].CharacterId = newCharacter.Id;
        
        CharacterManager.AddCharacter(newCharacter);
        TurnManager.AddToProgressQueue(new ProgressQueueUnit(newCharacter.Id, 0));
    }

    IEnumerator ProgressAndCompleteNextTurn() {
        while (true) {
            TurnManager.CycleToNextTurn();

            // Set camera to next character
            ProgressQueueUnit now = TurnManager.ProgressQueue.Peek();
            Character character = CharacterManager.ActiveCharacters[now.CharacterId];
            CameraManager.FollowNewCharacter(now.CharacterId);
            Debug.Log(character.Name + "'s turn.");

            if (character.IsControllable) {
                uiManager.BeginPlayerTurn();

                // Wait for player to finish turn
                yield return new WaitUntil(() => UiManager.hasFinishedControlling);
            }
            else {
                AiManager.CompleteTurn(now.CharacterId);
            }

            TurnManager.ResetCurrentCharacterProgress();

            yield return new WaitForSeconds(0.1f);
        }
    }

    // Start is called before the first frame update
    void Start() {
        rmManager.GenerateMap();
        //rmManager.PrintMap(0);
        AddCharacter(new Character("A", 1, 0, 20, 5, 0, 0, 0, Instantiate(sampleCharacter), true));
        AddCharacter(new Character("B", 1, 1, 0, 1, 0, 0, 0, Instantiate(sampleCharacter), false));
        AddCharacter(new Character("C", 1, 2, 0, 1, 0, 0, 0, Instantiate(sampleCharacter), false));
        AddCharacter(new Character("D", 1, 3, 0, 1, 0, 0, 0, Instantiate(sampleCharacter), false));
        AddCharacter(new Character("E", 1, 4, 0, 1, 0, 0, 0, Instantiate(sampleCharacter), false));
        AddCharacter(new Character("F", 1, 5, 0, 1, 0, 0, 0, Instantiate(sampleCharacter), false));
        AddCharacter(new Character("Z", 1, 6, 20, 5, 0, 0, 0, Instantiate(sampleCharacter), true));
        //rmManager.PrintMap(1);
        StartCoroutine(ProgressAndCompleteNextTurn());
    }
}
