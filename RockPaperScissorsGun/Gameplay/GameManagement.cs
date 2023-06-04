using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    //Card Prefab Objects
    [SerializeField]
    private GameObject RockCardPrefab;
    [SerializeField]
    private GameObject PaperCardPrefab;
    [SerializeField]
    private GameObject ScissorsCardPrefab;
    [SerializeField]
    private GameObject WaterCardPrefab;
    [SerializeField]
    private GameObject LeafCardPrefab;
    [SerializeField]
    private GameObject FireCardPrefab;
    [SerializeField]
    private GameObject GunCardPrefab;

    //GameCard References
    [SerializeField]
    private GameObject CardToMove;
    [SerializeField]
    private GameObject CurrentGameCard;
    private GameObject SelectedCard;
    private GameObject Gun;

    //Lists
    private List<GameObject> TypesToSet = new List<GameObject>();
    private List<GameObject> TypesToPlay = new List<GameObject>();

    [SerializeField]
    private List<GameObject> GameCards = new List<GameObject>();

    [SerializeField]
    private List<GameObject> AICards = new List<GameObject>();
    [SerializeField]
    private List<GameObject> AIAvailableCards = new List<GameObject>();

    [SerializeField]
    private List<GameObject> PlayerCards = new List<GameObject>();

    public List<GameObject> PlayerAvailableCards = new List<GameObject>();

    //Deck Transforms
    [SerializeField]
    private GameObject PlayerDeck;
    [SerializeField]
    private GameObject GameDeck;
    [SerializeField]
    private GameObject AiDeck;
    [SerializeField]
    private GameObject CardSpawn;

    //Public Variables
    public int StartingCards;
    public float CardSpacing;
    public float MoveSpeed;
    public float WaitTime;
    public float AvailableCardSpacing;
    public float GameCardVerticalSpacing;
    public int GunCardChance;

    //Display
    public TMP_Text PlayerScoreUI;
    public TMP_Text Gametimer;

    public GameObject DisplayUI;
    public TMP_Text PLCnt;
    public TMP_Text AICnt;
    public GameObject GunMessageObj;
    public TMP_Text GunMessage;

    //Private variables
    private bool IsMoving;
    private bool PlayerTurnProcessing;
    public bool PlayerSelecting;
    private bool AITurnProcessing;
    private bool AISelecting;
    private bool checking;
    private bool RunTimer;
    private bool GunCardPlaying;
    private Vector3 CurrPos;
    private Vector3 TargetDeckPos;
    RaycastHit HitData;
    private float Rotate_FaceDown = 180;
    private float GameTimerInSeconds;
    private float DisplayMinutes;
    private float DisplaySeconds;

    // Start is called before the first frame update
    void Start()
    {
        int randNum = Random.Range(0, 3);

        Debug.Log("STOPPING TRACK");
        GameObject.Find("AudioHandler").gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>().Stop();

        if (randNum == 0)
        {
            AudioHandler.Instance.PlayTrack01();
        }
        else if (randNum == 1)
        {
            AudioHandler.Instance.PlayTrack02();
        }
        else
        {
            AudioHandler.Instance.PlayTrack03();
        }

        GameObject Rock = Instantiate(RockCardPrefab, new Vector3(CardSpawn.transform.position.x, CardSpawn.transform.position.y, CardSpawn.transform.position.z), Quaternion.Euler(0f, 0f, Rotate_FaceDown), CardSpawn.transform);
        TypesToSet.Add(Rock);
        TypesToPlay.Add(Rock);
        GameObject Paper = Instantiate(PaperCardPrefab, new Vector3(CardSpawn.transform.position.x, CardSpawn.transform.position.y, CardSpawn.transform.position.z), Quaternion.Euler(0f, 0f, Rotate_FaceDown), CardSpawn.transform);
        TypesToSet.Add(Paper);
        TypesToPlay.Add(Paper);
        GameObject Scissors = Instantiate(ScissorsCardPrefab, new Vector3(CardSpawn.transform.position.x, CardSpawn.transform.position.y, CardSpawn.transform.position.z), Quaternion.Euler(0f, 0f, Rotate_FaceDown), CardSpawn.transform);
        TypesToSet.Add(Scissors);
        TypesToPlay.Add(Scissors);
        GameObject Water = Instantiate(WaterCardPrefab, new Vector3(CardSpawn.transform.position.x, CardSpawn.transform.position.y, CardSpawn.transform.position.z), Quaternion.Euler(0f, 0f, Rotate_FaceDown), CardSpawn.transform);
        TypesToSet.Add(Water);
        TypesToPlay.Add(Water);
        GameObject Leaf = Instantiate(LeafCardPrefab, new Vector3(CardSpawn.transform.position.x, CardSpawn.transform.position.y, CardSpawn.transform.position.z), Quaternion.Euler(0f, 0f, Rotate_FaceDown), CardSpawn.transform);
        TypesToSet.Add(Leaf);
        TypesToPlay.Add(Leaf);
        GameObject Fire =  Instantiate(FireCardPrefab, new Vector3(CardSpawn.transform.position.x, CardSpawn.transform.position.y, CardSpawn.transform.position.z), Quaternion.Euler(0f, 0f, Rotate_FaceDown), CardSpawn.transform);
        TypesToSet.Add(Fire);
        TypesToPlay.Add(Fire);

        Gun = Instantiate(GunCardPrefab, new Vector3(CardSpawn.transform.position.x, CardSpawn.transform.position.y, CardSpawn.transform.position.z), Quaternion.Euler(0f, 0f, Rotate_FaceDown), CardSpawn.transform);
        TypesToPlay.Add(Gun);

        IsMoving = false;
        PlayerTurnProcessing = false;
        AITurnProcessing = false;
        PlayerSelecting = false;
        checking = false;
        RunTimer = false;
        GunCardPlaying = false;
        DisplayUI.SetActive(false);

        GameTimerInSeconds = GameObject.Find("GameTime").GetComponent<GameTime>().TimeToSet;

        //Call Gameloop
        StartCoroutine(GameLoop());
    }

    // Update is called once per frame
    void Update()
    {
        if (CardToMove && IsMoving && !CardToMove.transform.position.Equals(TargetDeckPos))
        {
            CurrPos = new Vector3(CardToMove.transform.position.x, CardToMove.transform.position.y, CardToMove.transform.position.z);
            CardToMove.transform.position = Vector3.MoveTowards(CurrPos, TargetDeckPos, (MoveSpeed * Time.deltaTime));
            //Debug.Log("MOVING" + CardToMove.transform.position);
        }
        else 
        { 
            if (TargetDeckPos == CardSpawn.transform.position)
            {
                //Debug.Log("DELETION COMPLETE");
                Destroy(CardToMove);
            }

            IsMoving = false; 
        }

        if (PlayerSelecting && PlayerTurnProcessing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit HitData))
                {
                    if (HitData.collider.tag == "Card")
                    {
                        for (int i = 0; i < PlayerAvailableCards.Count; i++)
                        {
                            if (HitData.collider.gameObject.GetComponent<Card>().CardType == PlayerAvailableCards[i].GetComponent<Card>().CardType)
                            {
                                SelectedCard = HitData.collider.gameObject;
                                PlayerSelecting = false;
                                break;
                            }
                            else
                            {
                                Debug.Log(HitData.collider.gameObject + " Is not an available card.");
                            }
                        }
                    }
                }
            }
        }

        if (GameCards.Count > 0)
        {
            CurrentGameCard = GameCards[GameCards.Count - 1];
        }
        if (GameCards.Count == 0)
        {
            CurrentGameCard = null;
        }

        if (GameTimerInSeconds > 0 && RunTimer && !GunCardPlaying)
        {
            GameTimerInSeconds -= Time.deltaTime;
            DisplayMinutes = Mathf.FloorToInt(GameTimerInSeconds / 60);
            DisplaySeconds = Mathf.FloorToInt(GameTimerInSeconds % 60);
            Gametimer.text = ("Time Left " + DisplayMinutes + ":" + DisplaySeconds);
        }
        else if (GameTimerInSeconds < 0)
        {
            RunTimer = false;
            //StopCoroutine(GameLoop());
            //Gametimer.text = ("Game Over!");
        }

        //PlayerScoreUI.text = ("Cards: " + PlayerCards.Count);
    }

   private IEnumerator GameLoop()
    {
        if(DisplayUI.activeSelf)
        {
            DisplayUI.SetActive(false);
        }

        //Draw and move 1 card to the center play area
        StartCoroutine(DrawCards(1, GameDeck, GameCards, TypesToSet));
        while (IsMoving)
        {
            yield return null;
        }

        //Draw and move 5 random cards to both players
        StartCoroutine(DrawCards(StartingCards, PlayerDeck, PlayerCards, TypesToPlay));
        while (IsMoving)
        {
            yield return null;
        }
        StartCoroutine(DrawCards(StartingCards, AiDeck, AICards, TypesToPlay));
        while (IsMoving)
        {
            yield return null;
        }

        Gametimer.text = ("READY...");
        yield return new WaitForSeconds(WaitTime);
        Gametimer.text = ("GO...");

        //Start game timer
        RunTimer = true;

        //Player goes first
        yield return new WaitForSeconds(0.1f);

        while(RunTimer)
        {
            StartCoroutine(PlayerTurn());
            while (PlayerTurnProcessing)
            {
                yield return null;
            }

            if (GameCards.Count == 0)
            {
                StartCoroutine(DrawCards(1, GameDeck, GameCards, TypesToSet));
                while (IsMoving)
                {
                    yield return null;
                }
            }

            //Ai turn
            StartCoroutine(AITurn());
            while (AITurnProcessing)
            {
                yield return null;
            }

            if(GameCards.Count == 0)
            {
                StartCoroutine(DrawCards(1, GameDeck, GameCards, TypesToSet));
                while (IsMoving)
                {
                    yield return null;
                }
            }
        }

        Gametimer.text = ("Finishing...");

        for(int i = 0; i < PlayerCards.Count; i++)
        {
            GameObject newCard = Instantiate(PlayerCards[i], GameObject.Find("ResultsObj").transform);
            newCard.gameObject.SetActive(false);
            DontDestroyOnLoad(newCard);
            GameObject.Find("ResultsObj").GetComponent<ResultsHandler>().FinalPlayerCards.Add(newCard);
        }

        for (int i = 0; i < AICards.Count; i++)
        {
            GameObject newCard = Instantiate(AICards[i], GameObject.Find("ResultsObj").transform);
            newCard.gameObject.SetActive(false);
            DontDestroyOnLoad(newCard);
            GameObject.Find("ResultsObj").GetComponent<ResultsHandler>().FinalCPUCards.Add(newCard);
        }

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Assets/Scenes/Results.unity");
    }

    private IEnumerator DrawCards(int Amount, GameObject Deck, List<GameObject> DeckList, List<GameObject> DeckToPull)
    {
        //Debug.Log("Drawing: " + Amount + " card(s)" + " TO: " + Deck);
        GameObject CardToReturn;
        CardToMove = null;
        int RandIndex = 0;

        //For the amount of cards called
        for (int i = 0; i < Amount; i++)
        {
            if (DeckToPull == TypesToPlay)
            {
                //Generate Random number
                RandIndex = Random.Range(0, 100);
                //Debug.Log("Number to beat for Gun Card: " + (100 - GunCardChance));

                if (RandIndex > (100 - GunCardChance))
                {
                    Debug.Log(RandIndex + " Gun Card");

                    CardToReturn = Gun;
                    GameObject newCard = Instantiate(CardToReturn, new Vector3(CardSpawn.transform.position.x, CardSpawn.transform.position.y, CardSpawn.transform.position.z), Quaternion.identity);

                    CardToMove = newCard;
                    MoveCards(CardToMove, Deck, DeckList);
                    yield return null;
                }

                else
                {
                    //Debug.Log(RandIndex + " No Gun Card");

                    RandIndex = Random.Range(0, 5);

                    CardToReturn = TypesToPlay[RandIndex];

                    GameObject newCard = Instantiate(CardToReturn, new Vector3(CardSpawn.transform.position.x, CardSpawn.transform.position.y, CardSpawn.transform.position.z), Quaternion.identity);

                    CardToMove = newCard;
                    MoveCards(CardToMove, Deck, DeckList);
                    yield return null;
                }
            }

            else if (DeckToPull == TypesToSet)
            {
                //Generate Random index
                RandIndex = Random.Range(0, TypesToSet.Count);
                CardToReturn = TypesToSet[RandIndex];

                GameObject newCard = Instantiate(CardToReturn, new Vector3(CardSpawn.transform.position.x, CardSpawn.transform.position.y, CardSpawn.transform.position.z), Quaternion.identity);
                CardToMove = newCard;
                MoveCards(CardToMove, Deck, DeckList);
                yield return null;
            }
           // Debug.Log("Drawn: " + CardToMove);
            while (IsMoving)
            {
                yield return null;
            }
        }
    }

    private void MoveCards(GameObject CardObj, GameObject PhysicalDeck, List<GameObject> DeckList)
    {
        if(DeckList != null)
        {
            DeckList.Add(CardToMove);
        }

        //AudioCall
        AudioHandler.Instance.PlayDraw();

        if (DeckList == GameCards)
        {
            for(int i = 0; i < DeckList.Count; i++)
            {
                Debug.Log(i + ": " + DeckList[i].name);
            }

            TargetDeckPos = new Vector3((GameDeck.transform.position.x + (GameCards.Count - 1 * CardSpacing)), GameDeck.transform.position.y + (GameCardVerticalSpacing * GameCards.Count), GameDeck.transform.position.z);
            CardToMove.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        else if (DeckList == PlayerCards)
        {
            for (int i = 0; i < PlayerCards.Count; i++)
            {
                if (PlayerCards.Count <= 6)
                {
                    TargetDeckPos = new Vector3((PlayerDeck.transform.position.x + (PlayerCards.Count * CardSpacing)), PlayerDeck.transform.position.y, PlayerDeck.transform.position.z);
                    CardToMove.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                else
                {
                    TargetDeckPos = new Vector3(PlayerDeck.transform.position.x, PlayerDeck.transform.position.y, PlayerDeck.transform.position.z);
                    CardToMove.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
            }

        }

        else if (DeckList == AICards)
        {
            for (int i = 0; i < PlayerCards.Count; i++)
            {
                if (AICards.Count <= 6)
                {
                    TargetDeckPos = new Vector3((AiDeck.transform.position.x + (AICards.Count * CardSpacing)), AiDeck.transform.position.y, AiDeck.transform.position.z);
                    CardToMove.transform.rotation = Quaternion.Euler(0f, 0f, Rotate_FaceDown);
                }
                else
                {
                    TargetDeckPos = new Vector3(AiDeck.transform.position.x, AiDeck.transform.position.y, AiDeck.transform.position.z);
                    CardToMove.transform.rotation = Quaternion.Euler(0f, 0f, Rotate_FaceDown);
                }
            }
        }

        else
        {
            TargetDeckPos = new Vector3((CardSpawn.transform.position.x), CardSpawn.transform.position.y, CardSpawn.transform.position.z);
            Debug.Log("MOVING TO DELETE: " + CardObj);
        }

        if (CurrPos != TargetDeckPos)
        {
            CardToMove = CardObj;
            CardToMove.transform.parent = PhysicalDeck.transform;

            IsMoving = true;
        }

    }

    private IEnumerator ReadjustDeck(List<GameObject> DeckList, GameObject PhysicalDeck)
    {
        for (int i = 0; i < DeckList.Count; i++)
        {
            if (i <= 6)
            {
                Vector3 NecessaryPos = new Vector3((PhysicalDeck.transform.position.x + ((i + 1) * CardSpacing)), PhysicalDeck.transform.position.y, PhysicalDeck.transform.position.z);
                Vector3 CardPos = DeckList[i].gameObject.transform.position;

                if (CardPos != NecessaryPos)
                {
                    CardToMove = DeckList[i].gameObject;
                    CurrPos = CardPos;
                    TargetDeckPos = NecessaryPos;
                    IsMoving = true;
                }

                while (IsMoving)
                {
                    yield return null;
                }
            }
            else
            {
                Vector3 NecessaryPos = new Vector3(PhysicalDeck.transform.position.x, PhysicalDeck.transform.position.y, PhysicalDeck.transform.position.z);
                Vector3 CardPos = DeckList[i].gameObject.transform.position;

                if (CardPos != NecessaryPos)
                {
                    CardToMove = DeckList[i].gameObject;
                    CurrPos = CardPos;
                    TargetDeckPos = NecessaryPos;
                    IsMoving = true;
                }

                while (IsMoving)
                {
                    yield return null;
                }
            }
        }
    }

    private IEnumerator PlayerTurn()
    {
        //Clear Variables and Lists

        for (int i = 0; i < PlayerAvailableCards.Count; i++)
        {
            if (PlayerAvailableCards[i].GetComponent<Card>().IsHighlighted == true)
            {
                PlayerAvailableCards[i].GetComponent<Card>().IsHighlighted = false;
            }
        }

        PlayerAvailableCards.Clear();
        SelectedCard = null;

        if (!PlayerTurnProcessing)
        {
            PlayerTurnProcessing = true;
        }

        //TurnLogic
        if(PlayerTurnProcessing)
        {
            Debug.Log("Player 1 Turn Start!");

            //Loop through deck to see if any held cards can be chosen
            CheckDeck(PlayerCards, PlayerTurnProcessing, PlayerAvailableCards);
            while (checking == true)
            {
                yield return null;
            }

            //If AvailableCards contains anything:
            if (PlayerAvailableCards.Count != 0)
            {
                this.gameObject.GetComponent<CardHighlight>().CurrentIndex = 0;

                for (int i = 0; i < PlayerAvailableCards.Count; i++)
                {
                    if(PlayerAvailableCards[i].GetComponent<Card>().Beats == CurrentGameCard.GetComponent<Card>().CardType)
                    {
                        PlayerAvailableCards[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                        PlayerAvailableCards[i].transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
                        PlayerAvailableCards[i].transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
                    }
                    else if(PlayerAvailableCards[i].GetComponent<Card>().Adds == CurrentGameCard.GetComponent<Card>().CardType)
                    {
                        PlayerAvailableCards[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                        PlayerAvailableCards[i].transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
                        PlayerAvailableCards[i].transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
                    }
                    else if(PlayerAvailableCards[i].GetComponent<Card>().CardType == "Gun")
                    {
                        PlayerAvailableCards[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                        PlayerAvailableCards[i].transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
                    }
                }

                //Debug.Log(PlayerAvailableCards.Count + "/" + PlayerCards.Count + " of Player 1's Cards can be played! " + "Selecting!");
                PlayerSelecting = true;
                while (PlayerSelecting)
                {
                    yield return null;
                }
                StartCoroutine(PlayCard(SelectedCard, PlayerCards, PlayerAvailableCards));

                for (int i = 0; i < PlayerAvailableCards.Count; i++)
                {
                    PlayerAvailableCards[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                }

                while (IsMoving)
                {
                    yield return null;
                }
            }
            else if (PlayerAvailableCards.Count == 0)
            {
                //Debug.Log("0 of " + PlayerCards.Count + " of Player 1's Cards are Playable! Draw!");
                StartCoroutine(DrawCards(1, PlayerDeck, PlayerCards, TypesToPlay));
                while (IsMoving)
                {
                    yield return null;
                }
            }
        }


        //Debug.Log("Readjusting!");
        StartCoroutine(ReadjustDeck(PlayerCards, PlayerDeck));
        while (IsMoving)
        {
            yield return null;
        }

        Debug.Log("Player 1 Turn Over!");
        PlayerTurnProcessing = false;
        yield return null;
    }

    private IEnumerator AITurn()
    {
        //Clear Variables and Lists
        AIAvailableCards.Clear();
        SelectedCard = null;

        if (!AITurnProcessing)
        {
            AITurnProcessing = true;
        }

        if (AITurnProcessing)
        {
            Debug.Log("Player 2 Turn Start!");

            //Loop through deck
            CheckDeck(AICards, AITurnProcessing, AIAvailableCards);
            while (checking == true)
            {
                yield return null;
            }

            //If AvailableCards contains anything:
            if (AIAvailableCards.Count != 0)
            {
                //Debug.Log(AIAvailableCards.Count + "/" + AICards.Count + " of Player 2's Cards can be played!");
                StartCoroutine(AIPlayCard());
                while (AISelecting)
                {
                    yield return null;
                }
            }

            else if (AIAvailableCards.Count == 0)
            {
                //Debug.Log("0 of " + AICards.Count + " of Player 2's Cards are Playable! Draw!");
                StartCoroutine(DrawCards(1, AiDeck, AICards, TypesToPlay));
                while (IsMoving)
                {
                    yield return null;
                }
            }
        }

        //Debug.Log("Readjusting!");
        StartCoroutine(ReadjustDeck(AICards, AiDeck));
        while (IsMoving)
        {
            yield return null;
        }

        Debug.Log("Player 2 Turn Over!");
        AITurnProcessing = false;
        yield return null;
    }

    private IEnumerator AIPlayCard()
    {
        AISelecting = true;

        if (AISelecting)
        {
            Debug.Log("Player 2 selecting!");

            //Generate Random index
            int RandTime = Random.Range(1, 3);
            yield return new WaitForSeconds(RandTime);

            //Generate Random index
            int RandIndex = Random.Range(0, AIAvailableCards.Count);

            Debug.Log("Player 2 Selected: " + AIAvailableCards[RandIndex].gameObject.GetComponent<Card>().CardType + " Playing now!");
            StartCoroutine(PlayCard(AIAvailableCards[RandIndex].gameObject, AICards, AIAvailableCards));

        }
        while (IsMoving)
        {
            yield return null;
        }
        AISelecting = false;
        yield return null;
    }

    private void CheckDeck(List<GameObject> DecktoCheck, bool Turn, List<GameObject> DecktoAdd)
    {
        checking = true;

        if (Turn && checking)
        {
            Debug.Log("Current Card: " + CurrentGameCard + " Searching Deck (" + DecktoCheck.Count + ")...");

            //Loop through deck to see what cards have an appropriate beats/add types
            for (int i = 0; i < DecktoCheck.Count; i++)
            {
                //Debug.Log("Card: " + i + "/" + DecktoCheck.Count + " " + DecktoCheck[i]);

                if(DecktoCheck[i] != null)
                {
                    if (DecktoCheck[i].GetComponent<Card>().Beats.Equals(CurrentGameCard.GetComponent<Card>().CardType) || DecktoCheck[i].GetComponent<Card>().Adds.Equals(CurrentGameCard.GetComponent<Card>().CardType) || DecktoCheck[i].GetComponent<Card>().CardType.Equals("Gun"))
                    {
                        Debug.Log(DecktoCheck[i].GetComponent<Card>().CardType + " is Playable! " + i + "/" + DecktoCheck.Count);
                        DecktoAdd.Add(DecktoCheck[i]);
                        if (DecktoCheck == PlayerCards)
                        {
                            if (i > 6)
                            {
                                DecktoCheck[i].gameObject.transform.position = new Vector3(DecktoCheck[i].gameObject.transform.position.x, (DecktoCheck[i].gameObject.transform.position.y + (GameCardVerticalSpacing * i)), DecktoCheck[i].gameObject.transform.position.z + ((AvailableCardSpacing) * (i - 6)));
                            }
                            else
                            {
                                DecktoCheck[i].gameObject.transform.position = new Vector3(DecktoCheck[i].gameObject.transform.position.x, DecktoCheck[i].gameObject.transform.position.y, DecktoCheck[i].gameObject.transform.position.z + AvailableCardSpacing);
                            }
                        }
                    }
                }
            }
        }

        checking = false;
    }

    private IEnumerator PlayCard(GameObject CardToPlay, List<GameObject> CardList, List<GameObject> AvailableCardList)
    {
        Debug.Log("Playing: " + CardToPlay.GetComponent<Card>().CardType + " now!");

        if(CardToPlay.GetComponent<Card>().IsHighlighted == true)
        {
            CardToPlay.GetComponent<Card>().IsHighlighted = false;
        }

        CardList.Remove(CardToPlay);
        AvailableCardList.Remove(CardToPlay);
        CardToPlay.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

        if (CardToPlay.GetComponent<Card>().Beats == CurrentGameCard.GetComponent<Card>().CardType)
        {
            Debug.Log(CardToPlay.GetComponent<Card>().CardType + " BEATS " + CurrentGameCard.GetComponent<Card>().CardType);

            //Audio Calls
            if (CardToPlay.GetComponent<Card>().CardType == "Water")
            {
                AudioHandler.Instance.PlayWater();
            }
            else if (CardToPlay.GetComponent<Card>().CardType == "Fire")
            {
                AudioHandler.Instance.PlayFire();
            }
            else if (CardToPlay.GetComponent<Card>().CardType == "Leaf")
            {
                AudioHandler.Instance.PlayLeaf();
            }
            else if (CardToPlay.GetComponent<Card>().CardType == "Rock")
            {
                AudioHandler.Instance.PlayRock();
            }
            else if (CardToPlay.GetComponent<Card>().CardType == "Paper")
            {
                AudioHandler.Instance.PlayPaper();
            }
            else if (CardToPlay.GetComponent<Card>().CardType == "Scissors")
            {
                AudioHandler.Instance.PlayScissors();
            }

            GameObject BeatCard = CurrentGameCard;
            GameCards.Remove(BeatCard);

            CardToMove = CardToPlay;
            CardToPlay.transform.position = new Vector3(CardToPlay.transform.position.x, (CardToPlay.transform.position.y + 0.5f), CardToPlay.transform.position.z);
            MoveCards(CardToPlay, GameDeck, GameCards);
            while (IsMoving)
            {
                yield return null;
            }

            CardToMove = BeatCard;
            MoveCards(CardToMove, CardSpawn, null);
            while (IsMoving)
            {
                yield return null;
            }
        }

        else if (CardToPlay.GetComponent<Card>().Adds == CurrentGameCard.GetComponent<Card>().CardType)
        {
            CardToMove = CardToPlay;
            MoveCards(CardToPlay, GameDeck, GameCards);
            while (IsMoving)
            {
                yield return null;
            }

            Debug.Log(CardToPlay.GetComponent<Card>().CardType + " ADDS TO " + CurrentGameCard.GetComponent<Card>().CardType);

            //Audio Call
            AudioHandler.Instance.PlayAdd();
        }

        else if (CardToPlay.GetComponent<Card>().CardType.Equals("Gun"))
        {
            Debug.Log("FULL CLEAR! " + GameCards.Count);
            GunCardPlaying = true;

            int RandIndex = Random.Range(0, 5);

            if (RandIndex == 0)
            {
                GunMessage.text = "HAMMERFANTASTIC!";
                GunMessage.fontSize = 150;
                AudioHandler.Instance.PlayM01();
            }
            if (RandIndex == 1)
            {
                GunMessage.text = "GUNBELIEVABLE!";
                GunMessage.fontSize = 150;
                AudioHandler.Instance.PlayM02();
            }
            if (RandIndex == 2)
            {
                GunMessage.text = "EXPERT CARDSMAN!";
                GunMessage.fontSize = 200;
                AudioHandler.Instance.PlayM03();
            }
            if (RandIndex == 3)
            {
                GunMessage.text = "PEWTIFUL!";
                GunMessage.fontSize = 250;
                AudioHandler.Instance.PlayM04();
            }
            if (RandIndex == 4)
            {
                GunMessage.text = "NICE SHOT!";
                GunMessage.fontSize = 250;
                AudioHandler.Instance.PlayM05();
            }
            if (RandIndex == 5)
            {
                GunMessage.text = "AMAZINGLY ACCURATE!";
                GunMessage.fontSize = 250;
                AudioHandler.Instance.PlayM06();
            }

            GunMessageObj.SetActive(true);

            if(PlayerSelecting)
            {
                PlayerCards.Remove(CardToPlay);
            }
            else
            {
                AICards.Remove(CardToPlay);
            }

            CardToMove = CardToPlay;
            MoveCards(CardToMove, CardSpawn, null);
            while (IsMoving)
            {
                yield return null;
            }

            for (int i = GameCards.Count; i != 0; i--)
            {
                CardToMove = CurrentGameCard;
                GameCards.Remove(CardToMove);

                //AudioCall
                AudioHandler.Instance.PlayShot();

                if (PlayerTurnProcessing)
                {
                    CardToMove = CurrentGameCard;
                    MoveCards(CardToMove, AiDeck, AICards);
                    GameCards.Remove(CardToMove);
                    while (IsMoving)
                    {
                        yield return null;
                    }
                }
                else if (AITurnProcessing && !PlayerTurnProcessing)
                {
                    CardToMove = CurrentGameCard;
                    MoveCards(CardToMove, PlayerDeck, PlayerCards);
                    GameCards.Remove(CardToMove);
                    while (IsMoving)
                    {
                        yield return null;
                    }
                }
            }

            GunCardPlaying = false;
            GunMessageObj.SetActive(false);
        }

        if (PlayerSelecting)
        {
            PlayerSelecting = false;
        }
        else if (AISelecting)
        {
            AISelecting = false;
        }
        while (IsMoving)
        {
            yield return null;
        }
    }
}
