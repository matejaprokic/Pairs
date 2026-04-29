using UnityEngine;

// MemoryCard: Represents a playable card in the memory game scene.
// Handles visual state (face/back), user interaction, and communicates selection events to the SceneController.
public class MainCard : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private GameObject cardBack;
    private SpriteRenderer _spriteRenderer;
    private int _id;

    public int Id
    {
        get { return _id; }
    }


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        // If the card is face-down and revealing is allowed,
        // flip it face-up and notify the scene controller which card was revealed
        if (cardBack.activeSelf && sceneController.CanReveal)
        {
            cardBack.SetActive(false);
            sceneController.RevealCard(this);
        }
    }

    public void Unreveal()
    {
        cardBack.SetActive(true);
    }

    // Initializes the card with provided data by setting its ID and face sprite
    public void SetUpCard(CardData cardData)
    {
        _id = cardData.id;
        _spriteRenderer.sprite = cardData.face;
    }
}
