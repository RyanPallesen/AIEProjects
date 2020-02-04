using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private static Hand _instance;

    public static Hand Instance { get { return _instance; } }

    public int Movement;
    public int Turn;
    public int currentTier = 1;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    [HideInInspector] public List<CardDisplay> cards = new List<CardDisplay>();

    public int HandSize = 4;
    public GameObject prefabHandHolder;

    private List<GameObject> positions = new List<GameObject>();
    private Vector3 scale = Vector3.one;
    public int cardWidth;

    // Start is called before the first frame update
    private void Start()
    {
        UpdateHandSize(HandSize);
 
    }

    private void UpdateHandSize(int final)
    {
        HandSize = final;

        foreach (GameObject go in positions)
        {
            Destroy(go);
        }
        positions.Clear();
        RebuildHandPositions();
    }

    public void UpdateHandSize()
    {
        UpdateHandSize(HandSize);
    }

    public void RebuildHandPositions()
    {
        float translatePer = 1420 / HandSize;

        float currentTranslation = -(1420 / 2);

        if (translatePer < cardWidth)
        {
            scale.x = translatePer / cardWidth;
            scale.y = translatePer / cardWidth;
        }
        else
        {
            scale = Vector3.one;
        }



        currentTranslation += translatePer / 2;
        for (int i = 0; i < HandSize; i++)
        {

            GameObject newGameObject = null;

            if (positions.Count > i && positions[i])
            {
                newGameObject = positions[i];
            }
            else
            {
                newGameObject = Instantiate<GameObject>(prefabHandHolder, transform);
                positions.Add(newGameObject);

            }

            (newGameObject.transform as RectTransform).localPosition = (new Vector3(currentTranslation, 0, 0));
            currentTranslation += translatePer;
        }

       
        for(int i =0; i < cards.Count; i++)
        {
            if (positions.Count > i)
            {
                cards[i].transform.parent.SetParent(positions[i].transform,false);
                cards[i].transform.localPosition = Vector3.zero;
                cards[i].transform.localScale = scale;
            }
        }


    }

    public void DiscardCard(BaseCard.BaseCardType cardType)
    {
        foreach(CardDisplay cardDisplay in cards)
        {
            if (cardDisplay.Card.cardPrimary.baseCardType == cardType)
            {
                DiscardCard(cardDisplay);

                return;
            }
        }

    }

    public void DiscardCard(int card)
    {
        DiscardCard(cards[card]);
    }

    public void DiscardCard(CardDisplay card)
    {
        cards.Remove(card);
        Destroy(card.transform.parent.gameObject);
        RebuildHandPositions();
    }

    public void DrawCard()
    {
        if (cards.Count <= HandSize)
        {
            GameObject card = Deck.Instance.GetCard(Hand.Instance.currentTier);
            
            CardDisplay drawnCard = card.GetComponentInChildren<CardDisplay>();

            drawnCard.Init();

            cards.Add(drawnCard);
            RebuildHandPositions();


        }
        audioSource.Play();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DrawCard();
        }
    }

    public void MoveExtra()
    {
        Movement++;
        TetrisHandler.Instance.TakeDamage(TetrisPiece.ResourceType.Air);
        TetrisHandler.Instance.TakeDamage(TetrisPiece.ResourceType.Fuel);
        TetrisHandler.Instance.TakeDamage(TetrisPiece.ResourceType.Metal);
    }

    public void EndTurn()
    {
        if (TetrisHandler.Instance.resources.Count < 1)
        {

            List<CardDisplay> discards = new List<CardDisplay>();
            List<BaseCard> autoPlay = new List<BaseCard>();

            foreach (CardDisplay cardDisplay in cards)
            {
                if (cardDisplay.Card.cardPrimary.baseCardType == BaseCard.BaseCardType.Event)
                {
                    autoPlay.Add(cardDisplay.Card.cardPrimary);
                    discards.Add(cardDisplay);
                }
                if (cardDisplay.Card.cardSecondary.baseCardType == BaseCard.BaseCardType.Event)
                {
                    autoPlay.Add(cardDisplay.Card.cardSecondary);
                }

                foreach (BaseCard.DelayedEvent Devent in cardDisplay.Card.cardPrimary.delayedEvents)
                {
                    if (Devent.eventType == BaseCard.EventType.Junk)
                    {
                        discards.Add(cardDisplay);

                    }
                    if (cardDisplay.Card.cardPrimary.timeInHand == Devent.turnDelay)
                    {
                        autoPlay.Add(cardDisplay.Card.cardPrimary);
                        discards.Add(cardDisplay);

                    }

                }

                foreach (BaseCard.DelayedEvent Devent in cardDisplay.Card.cardSecondary.delayedEvents)
                {
                    if (Devent.eventType == BaseCard.EventType.Junk)
                    {
                        discards.Add(cardDisplay);
                    }

                    if (cardDisplay.Card.cardSecondary.timeInHand == Devent.turnDelay)
                    {
                        autoPlay.Add(cardDisplay.Card.cardSecondary);
                    }

                }

                cardDisplay.Card.cardPrimary.timeInHand++;
                cardDisplay.Card.cardSecondary.timeInHand++;
            }



            for (int i = 0; i < autoPlay.Count; i++)
            {
                autoPlay[i].DoCardBehaviour(null);
            }

            for (int i = 0; i < discards.Count; i++)
            {
                DiscardCard(discards[i]);
            }

            MoveExtra();
            Turn++;


            if (Movement > 0 && currentTier < 1)
            {
                currentTier = 1;
            }

            if (Movement > 5 && currentTier < 2)
            {
                currentTier = 2;
            }

            if (Movement > 15 && currentTier < 3)
            {
                currentTier = 2;
            }



            for (int i = cards.Count; i < HandSize; i++)
            {
                DrawCard();
            }
        }
    }
}