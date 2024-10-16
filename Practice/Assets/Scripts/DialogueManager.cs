using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject[] dialogues; // 4개의 대사 오브젝트

    private void Update()
    {
        // 각 숫자 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleDialogue(0); // 첫 번째 대사
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToggleDialogue(1); // 두 번째 대사
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToggleDialogue(2); // 세 번째 대사
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToggleDialogue(3); // 네 번째 대사
        }
    }

    private void ToggleDialogue(int index)
    {
        if (index >= 0 && index < dialogues.Length)
        {
            // 오브젝트가 활성화되어 있다면 비활성화, 비활성화 상태라면 활성화
            dialogues[index].SetActive(!dialogues[index].activeSelf);
        }
    }
}
