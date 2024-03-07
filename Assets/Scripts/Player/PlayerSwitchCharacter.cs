using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerSwitchCharacter : Singleton<PlayerSwitchCharacter>
{
    [SerializeField] List<Avatar> CharacterAvatarList;
    [SerializeField] List<Transform> listModel;

    public Transform SwitchCharacter(int choice)
    {
        Transform temp = null;
        Avatar avatarSelected = CharacterAvatarList[choice];
        foreach (Transform model in listModel)
        {
            if (avatarSelected == model.GetComponent<Animator>().avatar)
            {
                model.gameObject.SetActive(true);
                temp =  model;
            }
            else
            {
                model.gameObject.SetActive(false);
            }
        }

        Debug.Log("Null Character Selected");
        return temp;
    }
}
