using UnityEngine;
using System;

[System.Serializable]
public class MHMonsterData {
    public string name = "Monstruo";
    public double lat = 37.192206, lon = -3.616657;
    public GameObject prefab;
    public bool isDefeated;
    public int maxHealth = 3;
}
public class MHMonsterManager : MonoBehaviour {

    public static MHMonsterManager Instance;
    public MHMonsterData[] monsters = new MHMonsterData[3];
    public int currentMonsterIndex;

    void Awake() { Instance = this; }

    void Start() {
        foreach (var m in monsters) m.isDefeated = false;
    }
    public MHMonsterData GetNextMonster() {

        while (currentMonsterIndex < monsters.Length && monsters[currentMonsterIndex].isDefeated)
            currentMonsterIndex++;
        return currentMonsterIndex < monsters.Length ? monsters[currentMonsterIndex] : null;
    }
}