using System.Collections.Generic;
using UnityEngine;

public static class MoveAssigner
{
    public static MoveScriptableObject[] AssignUniqueTypeMoves(MoveScriptableObject[] availableMoves)
    {
        Dictionary<MoveType, List<MoveScriptableObject>> categorizedMoves = new Dictionary<MoveType, List<MoveScriptableObject>>();

        foreach (MoveScriptableObject move in availableMoves)
        {
            if (!categorizedMoves.ContainsKey(move.moveType))
                categorizedMoves[move.moveType] = new List<MoveScriptableObject>();

            categorizedMoves[move.moveType].Add(move);
        }

        List<MoveScriptableObject> finalMoves = new List<MoveScriptableObject>();

        foreach (MoveType type in System.Enum.GetValues(typeof(MoveType)))
        {
            if (categorizedMoves.ContainsKey(type) && categorizedMoves[type].Count > 0)
            {
                var moveList = categorizedMoves[type];
                finalMoves.Add(moveList[Random.Range(0, moveList.Count)]);
            }
            else
            {
                Debug.LogWarning($"No moves available for move type: {type}");
            }
        }

        return finalMoves.ToArray();
    }
}
