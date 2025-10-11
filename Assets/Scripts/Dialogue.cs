using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class Dialogue : MonoBehaviour
{

    public TextMeshProUGUI textComponent;
    public List<string> lines = new List<string>();
    public float textSpeed;

    private int index;

    public event System.Action<Dialogue> OnDialogueEnded;
    [SerializeField] private GameInput gameInput;


    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(textComponent.text);
            //if line written, move to next line in list, else skip ahead and complete line
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void AddText(String newText)
    {
        lines.Add(newText);
    }

    public void StartDialogue()
    {
        gameObject.SetActive(true);
        textComponent.text = string.Empty;
        index = 0;
        StartCoroutine(TypeLine());
        
    }

    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty;
        yield return null;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        Debug.Log(textComponent.text);
    }

    void NextLine()
    {
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        {
            //moves the list forward and deletes read line, if last line then finish dialogue
            if (lines.Count > 1)
            {
                lines.RemoveAt(0);
            }
            else
            {
                lines.RemoveAt(0);
                OnDialogueEnded?.Invoke(this);
                gameObject.SetActive(false);
            }
        }
    }
}
