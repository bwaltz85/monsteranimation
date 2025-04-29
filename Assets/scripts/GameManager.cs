using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject rewardPanel;
    [Header("Prefabs & Spawn Points")]
    public GameObject[] playerMonsters;
    public GameObject[] opponentMonsters;
    public Transform playerSpawnPoint;
    public Transform opponentSpawnPoint;

    [Header("Managers")]
    public BattleUIManager uiManager;
    public TurnManager turnManager;
    public AudioManager audioManager;
    public RewardManager rewardManager; // Added The Reward Manager, DS

    [Header("Assigned Combatants")]
    public Player player;
    public Opponent opponent;

    private bool isPlayerTurn = true;
    private bool battleEnded = false;
    private bool waitingForNextLevel = false;

    private GameObject currentOpponent;

    void Start()
    {
        SpawnRandomPlayerMonster();
        SpawnRandomOpponentMonster();

        uiManager.UpdateMoveButtons();
        uiManager.UpdateHPUI();

        uiManager.DisplayMessage("Battle Start!");
        uiManager.DisplayTurn("Your Turn");
        Debug.Log("Player's Turn");
    }

    void Update()
    {
        if (waitingForNextLevel && Input.GetKeyDown(KeyCode.Return))
        {
            waitingForNextLevel = false;
            StartNextBattle();
        }
    }

    void SpawnRandomPlayerMonster()
    {
        int index = Random.Range(0, playerMonsters.Length);
        GameObject prefab = playerMonsters[index];
        GameObject monster = Instantiate(prefab, playerSpawnPoint.position, Quaternion.identity);

        player = monster.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Spawned player monster is missing the Player script!");
            return;
        }

        player.AssignMoves();
        player.health = 1000;
        player.attackPower = 10;
    }

    void SpawnRandomOpponentMonster()
    {
        if (currentOpponent != null)
        {
            Destroy(currentOpponent);
        }

        int index = Random.Range(0, opponentMonsters.Length);
        GameObject prefab = opponentMonsters[index];
        currentOpponent = Instantiate(prefab, opponentSpawnPoint.position, Quaternion.identity);
        currentOpponent.transform.localScale = new Vector3(-1, 1, 1); // Face left

        opponent = currentOpponent.GetComponent<Opponent>();
        if (opponent == null)
        {
            Debug.LogError("Spawned opponent monster is missing the Opponent script!");
            return;
        }

        opponent.AssignMoves();
        opponent.health = 1000;
        opponent.attackPower = 10;
    }

    public void OnPlayerMoveSelected(int moveIndex)
    {
        if (!isPlayerTurn || battleEnded) return;

        MoveScriptableObject selectedMove = player.playerMoves[moveIndex];

        Debug.Log($"[BUTTON {moveIndex}] Triggered Move: {selectedMove.moveName} | Type: {selectedMove.moveType} | Power: {selectedMove.power}");

        player.ExecuteMove(selectedMove, opponent);
        uiManager.UpdateHPUI();
        uiManager.DisplayMessage($"You used {selectedMove.moveName}!");

        switch (selectedMove.moveType)
        {
            case MoveType.Damage:
                Debug.Log($"Player used {selectedMove.moveName} and dealt {selectedMove.power} damage!");
                break;
            case MoveType.Heal:
                Debug.Log($"Player used {selectedMove.moveName} and healed for {selectedMove.power} HP!");
                break;
            default:
                Debug.Log($"Player used {selectedMove.moveName} ({selectedMove.moveType})");
                break;
        }

        if (opponent.health <= 0)
        {
            EndBattle(true);
            return;
        }

        isPlayerTurn = false;
        uiManager.DisplayTurn("Enemy Turn");
        Debug.Log("Opponent's Turn");
        Invoke(nameof(OpponentTurn), 1.5f);

        turnManager?.EndPlayerTurn();
    }

    void OpponentTurn()
    {
        if (battleEnded || isPlayerTurn) return;

        MoveScriptableObject opponentMove = opponent.SelectRandomMove();
        opponent.ExecuteMove(opponentMove, player);
        uiManager.UpdateHPUI();

        uiManager.DisplayMessage($"Opponent used {opponentMove.moveName}!");

        switch (opponentMove.moveType)
        {
            case MoveType.Damage:
                Debug.Log($"Opponent used {opponentMove.moveName} and dealt {opponentMove.power} damage!");
                break;
            case MoveType.Heal:
                Debug.Log($"Opponent used {opponentMove.moveName} and healed for {opponentMove.power} HP!");
                break;
            default:
                Debug.Log($"Opponent used {opponentMove.moveName} ({opponentMove.moveType})");
                break;
        }

        if (player.health <= 0)
        {
            EndBattle(false);
            return;
        }

        isPlayerTurn = true;
        uiManager.DisplayTurn("Your Turn");
        Debug.Log("Player's Turn");
    }

    void EndBattle(bool playerWon)
    {
        battleEnded = true;

        if (playerWon)
        {
            uiManager.DisplayMessage("You win! Press ENTER to continue.");
            audioManager?.PlayVictorySoundThenPostTrack(); // ✅ Updated method name

            rewardManager.ShowRewards(); // Calls the ShowRewards method that activates the reward window, DS
            waitingForNextLevel = true;
        }
        else
        {
            uiManager.DisplayMessage("You lose.");
            audioManager?.PlayLossSound();
        }
    }

    void StartNextBattle()
    {
        battleEnded = false;
        isPlayerTurn = true;

        rewardPanel.SetActive(false);

        SpawnRandomOpponentMonster();
        player.AssignMoves();
        opponent.AssignMoves();

        uiManager.UpdateMoveButtons();
        uiManager.UpdateHPUI();

        uiManager.DisplayMessage("Next Battle Begins!");
        uiManager.DisplayTurn("Your Turn");

        Debug.Log("Next battle started.");
        audioManager?.ResumePreviousBattleMusic();
    }
}
