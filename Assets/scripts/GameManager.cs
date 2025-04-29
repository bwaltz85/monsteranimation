using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs & Spawn Points")]
    public GameObject[] playerMonsters;
    public GameObject[] opponentMonsters;
    public Transform playerSpawnPoint;
    public Transform opponentSpawnPoint;

    [Header("Managers")]
    public BattleUIManager uiManager;
    public TurnManager turnManager;
    public AudioManager audioManager; // Still keep this for Inspector assignment, but we'll use AudioManager.Instance
    public BackgroundManager backgroundManager;
    public RewardManager rewardManager;

    [Header("Assigned Combatants")]
    public Player player;
    public Opponent opponent;

    private bool waitingForPlayerContinue = false;
    private bool waitingForRewardSelection = false;

    void Awake()
    {
        if (turnManager == null)
            turnManager = FindFirstObjectByType<TurnManager>();
        if (turnManager == null)
        {
            Debug.LogError("TurnManager not found in scene!");
        }

        if (audioManager == null)
            audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in scene!");
        }

        if (backgroundManager == null)
            backgroundManager = FindFirstObjectByType<BackgroundManager>();
        if (backgroundManager == null)
        {
            Debug.LogError("BackgroundManager not found in scene!");
        }

        if (rewardManager == null)
            rewardManager = FindFirstObjectByType<RewardManager>();
        if (rewardManager == null)
        {
            Debug.LogError("RewardManager not found in scene!");
        }
    }

    void Start()
    {
        turnManager.OnPlayerTurnStart += StartPlayerTurn;
        turnManager.OnOpponentTurnStart += StartOpponentTurn;

        SpawnRandomPlayerMonster();
        SpawnRandomOpponentMonster();

        uiManager.UpdateMoveButtons();
        uiManager.UpdateHPUI();

        if (AudioManager.Instance != null)
        {
            Debug.Log("Calling PlayRandomBattleTrack from GameManager.Start using AudioManager.Instance");
            AudioManager.Instance.PlayRandomBattleTrack();
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance is null in GameManager.Start");
        }

        backgroundManager?.SetInitialBackground();

        turnManager.StartBattle();
    }

    void Update()
    {
        if (waitingForPlayerContinue && Input.GetKeyDown(KeyCode.Return))
        {
            waitingForPlayerContinue = false;
            waitingForRewardSelection = true;
            rewardManager.ShowRewards();
        }
    }

    void SpawnRandomPlayerMonster()
    {
        if (playerMonsters == null || playerMonsters.Length == 0)
        {
            Debug.LogError("PlayerMonsters array is empty! Please assign player monster prefabs in the Inspector.");
            return;
        }

        int index = Random.Range(0, playerMonsters.Length);
        GameObject monster = Instantiate(playerMonsters[index], playerSpawnPoint.position, Quaternion.identity);
        player = monster.GetComponent<Player>();
        player.AssignMoves();
        player.health = 1000;
        player.attackPower = 10;
    }

    void SpawnRandomOpponentMonster()
    {
        if (opponentMonsters == null || opponentMonsters.Length == 0)
        {
            Debug.LogError("OpponentMonsters array is empty! Please assign opponent monster prefabs in the Inspector.");
            return;
        }

        int index = Random.Range(0, opponentMonsters.Length);
        GameObject monster = Instantiate(opponentMonsters[index], opponentSpawnPoint.position, Quaternion.identity);
        opponent = monster.GetComponent<Opponent>();
        opponent.AssignMoves();
        opponent.health = 1000;
        opponent.attackPower = 10;

        monster.transform.localScale = new Vector3(-1f * Mathf.Abs(monster.transform.localScale.x), monster.transform.localScale.y, monster.transform.localScale.z);
    }

    private void StartPlayerTurn()
    {
        uiManager.DisplayMessage("Battle Start!");
        uiManager.DisplayTurn("Your Turn");
    }

    private void StartOpponentTurn()
    {
        OpponentTurn();
    }

    public void OnPlayerMoveSelected(int moveIndex)
    {
        if (!turnManager.isPlayerTurn || !turnManager.BattleActive) return;

        MoveScriptableObject selectedMove = player.playerMoves[moveIndex];
        player.ExecuteMove(selectedMove, opponent);

        uiManager.UpdateHPUI();
        uiManager.DisplayMessage($"You used {selectedMove.moveName}!");

        if (opponent.health <= 0)
        {
            EndBattle(true);
            return;
        }

        player.UpdateEffects();
        turnManager.EndPlayerTurn();
    }

    private void OpponentTurn()
    {
        MoveScriptableObject opponentMove = opponent.SelectRandomMove();
        opponent.ExecuteMove(opponentMove, player);

        uiManager.UpdateHPUI();
        uiManager.DisplayMessage($"Opponent used {opponentMove.moveName}!");

        if (player.health <= 0)
        {
            EndBattle(false);
            return;
        }

        opponent.UpdateEffects();
        turnManager.EndOpponentTurn();
    }

    void EndBattle(bool playerWon)
    {
        turnManager.EndBattle();

        if (playerWon)
        {
            uiManager.DisplayMessage("You win! Press [Enter] to continue.");
            if (AudioManager.Instance != null)
            {
                Debug.Log("Calling PlayVictorySoundThenPostTrack from GameManager.EndBattle using AudioManager.Instance");
                AudioManager.Instance.PlayVictorySoundThenPostTrack();
            }
            else
            {
                Debug.LogWarning("AudioManager.Instance is null in GameManager.EndBattle");
            }
            waitingForPlayerContinue = true;
        }
        else
        {
            uiManager.DisplayMessage("You lose.");
            if (AudioManager.Instance != null)
            {
                Debug.Log("Calling PlayLossSound from GameManager.EndBattle using AudioManager.Instance");
                AudioManager.Instance.PlayLossSound();
            }
            else
            {
                Debug.LogWarning("AudioManager.Instance is null in GameManager.EndBattle");
            }
        }
    }

    void NextLevel()
    {
        Destroy(opponent.gameObject);

        SpawnRandomOpponentMonster();

        backgroundManager?.SetInitialBackground();

        if (AudioManager.Instance != null)
        {
            Debug.Log("Calling ResumePreviousBattleMusic from GameManager.NextLevel using AudioManager.Instance");
            AudioManager.Instance.ResumePreviousBattleMusic();
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance is null in GameManager.NextLevel");
        }

        uiManager.UpdateMoveButtons();
        uiManager.UpdateHPUI();
        uiManager.DisplayMessage("Next Battle!");
        uiManager.DisplayTurn("Your Turn");

        waitingForRewardSelection = false;
        turnManager.StartBattle();
    }

    public void OnRewardSelected()
    {
        if (waitingForRewardSelection)
        {
            NextLevel();
        }
    }
}