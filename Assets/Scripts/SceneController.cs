using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// GameController: Manages core game setup and flow: creates and positions cards, shuffles them,
// tracks revealed cards, resolves matches/mismatches, and controls turn progression
public class SceneController : MonoBehaviour
{
    [Header("Cards Layout")]
    private const int gridRows = 2;
    private const int gridColumns = 4;
    private const float offsetX = 4f;
    private const float offsetY = 5f;

    [Header("Cards")]
    [SerializeField] private MainCard originalCard;
    [SerializeField] private Sprite[] cardFaceImages;
    private MainCard _firstRevealedCard;
    private MainCard _secondRevealedCard;

    [Header("Gameplay Settings")]
    [Range(0.5f, 5f)]
    [SerializeField] private float mismatchRevealDurationInSeconds = 1f;

    [Header("Score")]
    [SerializeField] private TextMesh scoreLabel;
    private int _score = 0;

    // Returns true if the player is allowed to reveal another card.
    // This is only possible when no second card is currently revealed (i.e., turn not yet completed).
    public bool CanReveal
    {
        get { return _secondRevealedCard == null; }
    }

    private void Start()
    {
        Debug.Assert(cardFaceImages.Length * 2 == gridRows * gridColumns,
            "Card images count doesn't match grid size!");
        
        SetCardStartLayout();
    }

    private void SetCardStartLayout()
    {
        List<CardData> deck = BuildDeck();
        ShuffleDeck(deck);
        PlaceCards(deck);
    }

    private List<CardData> BuildDeck()
    {
        // Step 1: Build a flat list of CardData pairs
        List<CardData> deck = new List<CardData>();

        for (int id = 0; id < cardFaceImages.Length; id++)
        {
            CardData cardData = new CardData { id = id, face = cardFaceImages[id] };
            deck.Add(cardData);
            deck.Add(cardData);
        }

        return deck;
    }

    // Fisher-Yates shuffle: iterate from the end, and for each position pick
    // a random card from the undecided portion (0 to i) and swap it into place.
    // The array is divided into two sides:
    //   left  [0..i] -> undecided, we pick randomly from here
    //   right [i+1..n] -> already decided, we don't touch this
    // We stop at i=1 (not i=0) because a single remaining card has nowhere to swap to.
    // More efficient than picking from the full array each time (naive approach),
    // which can revisit already-decided positions and produce a biased shuffle.
    private static void ShuffleDeck(List<CardData> deck)
    {
        // Step 2: Shuffle the list (Fisher-Yates)
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            // Basic approach for swapping card positions
            //CardData temp = deck[i];
            //deck[i] = deck[randomIndex];
            //deck[randomIndex] = temp;

            // Alternative approach: tuple swap — a C# shorthand for swapping two values without a temporary variable
            (deck[i], deck[randomIndex]) = (deck[randomIndex], deck[i]);
        }
    }

    private void PlaceCards(List<CardData> deck)
    {
        // Step 3: Place cards on the grid
        Vector3 startPos = originalCard.transform.position;
        int cardIndex = 0;

        for (int col = 0; col < gridColumns; col++)
        {
            for (int row = 0; row < gridRows; row++)
            {
                // Use the original card for the first instance; instantiate a clone for all subsequent cards
                MainCard card = (cardIndex == 0)
                    ? originalCard
                    : Instantiate(originalCard);

                card.SetUpCard(deck[cardIndex]);

                // starting point plus how many steps taken multiplied by the step size
                float x = startPos.x + col * offsetX;
                float y = startPos.y + row * offsetY;
                card.transform.position = new Vector3(x, y, startPos.z);

                cardIndex++;
            }
        }
    }

    public void RevealCard(MainCard card)
    {
        if (_firstRevealedCard == null)
        {
            _firstRevealedCard = card;
        }
        else
        {
            _secondRevealedCard = card;
            StartCoroutine(CheckCardMatchCoroutine());
        }
    }

    private IEnumerator CheckCardMatchCoroutine()
    {
        if (_firstRevealedCard.Id == _secondRevealedCard.Id)
        {
            _score++;
            scoreLabel.text = "Score: " + _score;
        }
        else
        {
            // On mismatch: wait for a short delay so player can see the cards, then hide both
            yield return new WaitForSeconds(mismatchRevealDurationInSeconds);
            _firstRevealedCard.Unreveal();
            _secondRevealedCard.Unreveal();
        }

        // Reset selected cards to end the current turn and allow the next turn to start
        _firstRevealedCard = null;
        _secondRevealedCard = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
