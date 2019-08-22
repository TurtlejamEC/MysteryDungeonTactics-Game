using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    public Camera mainCamera;

    public float cameraHeight;
    public float angleOfDepression;

    // Character id of character to show on screen
    public int followingCharacterId;

    void Start() {
        mainCamera.transform.rotation = Quaternion.Euler(angleOfDepression, 0, 0);
    }

    void Update() {
        Character characterInfo = CharacterManager.ActiveCharacters[followingCharacterId];
        mainCamera.transform.rotation = Quaternion.Euler(angleOfDepression, 0, 0);
        mainCamera.transform.position = new Vector3(characterInfo.Position.X, cameraHeight, characterInfo.Position.Z - ((cameraHeight / Mathf.Tan(angleOfDepression * Mathf.Deg2Rad))));
    }

    public void FollowNewCharacter(int characterId) {
        followingCharacterId = characterId;
    }
}