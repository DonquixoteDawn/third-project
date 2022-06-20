using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] ItemSO spade;
    [SerializeField] ItemSO diamond;
    [SerializeField] ItemSO cloba;
    [SerializeField] ItemSO heart;
    [SerializeField] List<Card> myCards;
    [SerializeField] Transform cardSpawn;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;

    [SerializeField] GameObject card;

    List<Item> deck; //will pop


    void SetupItemBuffer()
    {
        deck = new List<Item>();

        for (int i = 0; i < spade.items.Length; i++) // setup all card
        {
            deck.Add(spade.items[i]);
            deck.Add(diamond.items[i]);
            deck.Add(cloba.items[i]);
            deck.Add(heart.items[i]);
        }

        deck = Shuffle(deck);
    }

    List<Item> Shuffle(List<Item> cardDeck)
    {
        List<Item> newDeck = new List<Item>();
        for (int i = 0; i < cardDeck.Count; i++)
        {
            int rand = Random.Range(i, cardDeck.Count);
            Item tmp = cardDeck[i];
            cardDeck[i] = cardDeck[rand];
            cardDeck[rand] = tmp;
            newDeck.Add(cardDeck[i]);
        }
       

        return newDeck; //i considered deep copy 
    }

    Item PopCard()
    {
        Item item = deck[0];
        deck.RemoveAt(0);
        return item;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupItemBuffer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            AddCard(true);            
        }
    }

    void AddCard(bool isMine)
    {
        GameObject cardObj = Instantiate(card, cardSpawn.position, Quaternion.identity);
        Card cardScipt = cardObj.GetComponent<Card>();
        cardScipt.Setup(PopCard(), true);
        myCards.Add(cardScipt);

        SetOriginOrder(true);
        CardAlignment(true);
    }

    void SetOriginOrder(bool isMine)
    {
        int count = myCards.Count;
        for(int i = 0; i < count; i++)
        {
            var targetCard = myCards[i];
            if (targetCard != null)
                targetCard.GetComponent<Order>().SetOrigin(i);
        }
    }

    void CardAlignment(bool isMine)
    {
        List<PRS> originPRSs = new List<PRS>();

        if (isMine)
            originPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 2);
        else return;

        var targetCards = myCards;

        for (int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];

            targetCard.originPRS = originPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
        }
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch(objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for(int i = 0; i < objCount; i++)
                {
                    objLerps[i] = interval * i;
                }
                break;
        }

        for(int i = 0; i<objCount; i++)
        {
            var targetPos = Vector3.Lerp(rightTr.position, leftTr.position, objLerps[i]);
            var targetRot = Quaternion.identity;
            if(objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(rightTr.rotation, leftTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }

        return results;
    }
}
