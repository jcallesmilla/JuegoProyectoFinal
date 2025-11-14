using UnityEngine;
using UnityEngine.UI;

public class SelectingArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    private RectTransform rect;
    private int currentPosition;

    private void Awake()
    {
        rect = GetComponent<RectTransform>(); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            changePosition(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            changePosition(1);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void changePosition(int _change)
    {
        currentPosition += _change;

        if (currentPosition < 0)
            currentPosition = options.Length - 1;
        else if (currentPosition > options.Length - 1)
            currentPosition = 0;

        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }

    private void Interact()
    {
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
