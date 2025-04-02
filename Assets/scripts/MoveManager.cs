using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    public static MoveManager Instance { get; private set; }

    public List<MoveScriptableObject> allMoves = new List<MoveScriptableObject>();

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllMoves();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadAllMoves()
    {
        // Load all move assets in the Resources/Moves folder
        MoveScriptableObject[] loadedMoves = Resources.LoadAll<MoveScriptableObject>("Moves");
        allMoves.AddRange(loadedMoves);
    }

    public MoveScriptableObject GetRandomMove()
    {
        if (allMoves.Count == 0)
        {
            Debug.LogWarning("No moves available in MoveManager.");
            return null;
        }

        return allMoves[Random.Range(0, allMoves.Count)];
    }

    public MoveScriptableObject GetRandomMoveOfType(MoveType type)
    {
        List<MoveScriptableObject> filtered = allMoves.FindAll(m => m.moveType == type);
        if (filtered.Count == 0)
        {
            Debug.LogWarning("No moves of type: " + type);
            return null;
        }

        return filtered[Random.Range(0, filtered.Count)];
    }
}
