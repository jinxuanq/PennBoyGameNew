using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class Dialogue : MonoBehaviour
{

    public TextMeshProUGUI textComponent;
    public IconSelector icon;
    public List<string> lines = new List<string>();
    public float textSpeed;
    private bool typing;

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
            Debug.Log(lines.Count);
            if(lines.Count > 0)
            {
                //if line written, move to next line in list, else skip ahead and complete line
                if (!typing)
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    typing = false;
                    textComponent.text = lines[index];
                    lines.RemoveAt(0);
                }
            }
            else
            {
                OnDialogueEnded?.Invoke(this);
            }

        }
    }

    public void AddText(String newText)
    {
        lines.Add(newText);
    }

    public void SetSprite(String sprite)
    {
        icon.Sprite(sprite);
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
        typing = true;
        textComponent.text = string.Empty;
        yield return null;
        foreach (char c in (lines[index].ToCharArray()))
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        Debug.Log(textComponent.text);
        lines.RemoveAt(0);
        typing = false;
    }

    void NextLine()
    {
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
    }
}
