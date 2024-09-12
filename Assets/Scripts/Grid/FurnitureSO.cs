using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Furniture", menuName = "GameData/ScriptableObjects/Grid", order = 1)]
public class FurnitureSO : ScriptableObject
{
    public List<Furniture> listFurniture;
}

[Serializable]
public class Furniture
{
    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public ArchitectureType ArchitectureType { get; set; }

    [field: SerializeField]
    public int Level { get; set; }

    [field: SerializeField]
    public float Hp { get; set; }

    [field: SerializeField]
    public bool CanPutItemOnSeft { get; private set; } = false;

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField]
    public GameObject Prefab { get; private set; }


}
