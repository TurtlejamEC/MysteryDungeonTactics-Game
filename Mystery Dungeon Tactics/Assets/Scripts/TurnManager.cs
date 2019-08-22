using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    public static Queue<ProgressQueueUnit> ProgressQueue { get; } = new Queue<ProgressQueueUnit>();
    
    public static void AddToProgressQueue(ProgressQueueUnit newCharacter) {
        ProgressQueue.Enqueue(newCharacter);
    }

    // Keep progressing everyone and leave the first person eligible for turn at front
    public static void CycleToNextTurn() {
        ProgressQueueUnit next = ProgressQueue.Peek();
        Character character = CharacterManager.ActiveCharacters[next.CharacterId];
        next.Progress += character.Speed;
        while (next.Progress < 100) {
            ProgressQueue.Enqueue(ProgressQueue.Dequeue());

            next = ProgressQueue.Peek();
            character = CharacterManager.ActiveCharacters[next.CharacterId];
            next.Progress += character.Speed;
        }
    }

    public static void ResetCurrentCharacterProgress() {
        ProgressQueue.Peek().Progress = 0;
        ProgressQueue.Enqueue(ProgressQueue.Dequeue());
    }
    
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        
    }
    
    
}
