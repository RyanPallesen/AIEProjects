using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deck : MonoBehaviour, IPointerEnterHandler
{
    public GameObject cardPrefab;

    public Dictionary<int, List<BaseCard>> keyValuePairs = new Dictionary<int, List<BaseCard>>();


    public static Deck Instance { get { return _instance; } }

    private static Deck _instance;

    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;


            foreach (ScriptableObject @object in Resources.LoadAll("Cards/Resource"))
            {
                BaseCard card = @object as BaseCard;
                if (card)
                {
                    if (!keyValuePairs.ContainsKey(card.cardTier))
                    {
                        keyValuePairs[card.cardTier] = new List<BaseCard>();
                    }

                    for (int i = -1; i < card.frequency; i++)
                    {
                        keyValuePairs[card.cardTier].Add(card);
                    }
                }
            }

            foreach (ScriptableObject @object in Resources.LoadAll("Cards/Event"))
            {
                BaseCard card = @object as BaseCard;
                if (card)
                {
                    if (!keyValuePairs.ContainsKey(card.cardTier))
                    {
                        keyValuePairs[card.cardTier] = new List<BaseCard>();
                    }

                    for (int i = -1; i < card.frequency; i++)
                    {
                        keyValuePairs[card.cardTier].Add(card);
                    }
                }
            }

            foreach (ScriptableObject @object in Resources.LoadAll<ScriptableObject>("Cards/Action"))
            {
                BaseCard card = @object as BaseCard;
                if (card)
                {
                    if (!keyValuePairs.ContainsKey(card.cardTier))
                    {
                        keyValuePairs[card.cardTier] = new List<BaseCard>();
                    }

                    for (int i = -1; i < card.frequency; i++)
                    {
                        keyValuePairs[card.cardTier].Add(card);
                    }
                }
            }
        }
    }

    public GameObject GetCard(int tier)
    {
        Card card = new Card();

        card.cardPrimary = GetCardOfTier(tier);
        card.cardSecondary = GetCardOfTier(tier);
        
        if(card.cardPrimary.isFullCard)
        {
            card.cardSecondary = card.cardPrimary;
        }

        if (card.cardSecondary.isFullCard)
        {
            card.cardPrimary = card.cardSecondary;
        }

        
        card.cardTier = tier;

        GameObject go = Instantiate(cardPrefab, transform);
        go.GetComponentInChildren<CardDisplay>().Card = card;

        return go;
    }


    public BaseCard GetCardOfTier(int tier)
    {

        List<BaseCard> cards = keyValuePairs[tier];
        int random = Random.Range(0, cards.Count);

        return (cards[random]);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipHandler.Instance.textmesh.text = "The deck";
    }
}
