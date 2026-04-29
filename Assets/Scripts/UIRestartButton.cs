using UnityEngine;

public class UIRestartButton : MonoBehaviour
{
    [Header("Restart")]
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string restartMethodName;

    [Header("Mouse Feedback")]
    private Color defaultColor;
    private Vector3 defaultButtonScale;
    [SerializeField] private Color highlightColor = Color.cyan;
    [SerializeField] private Vector3 buttonPressScale = new Vector3(0.2f, 0.2f, 1.0f);

    private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        
    }

    private void Start()
    {
        SpriteRenderer _spriteRenderer = GetComponent<SpriteRenderer>();

        defaultColor = _spriteRenderer.color;
        defaultButtonScale = transform.localScale;
    }

    //private void OnMouseOver()
    //{ 
    //    if (_spriteRenderer != null)
    //    {
    //        _spriteRenderer.color = highlightColor;
    //    }
    //}

    private void OnMouseEnter()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = highlightColor;
        }
    }

    private void OnMouseExit()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = defaultColor; // _spriteRenderer.color = Color.white;
        }
    }

    private void OnMouseDown()
    {
        transform.localScale = buttonPressScale;
    }

    private void OnMouseUp()
    {
        transform.localScale = defaultButtonScale;

        if (targetObject != null)
        {
            targetObject.SendMessage(restartMethodName);
        }
    }
}
