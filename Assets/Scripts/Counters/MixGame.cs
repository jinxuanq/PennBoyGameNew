using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class MixGame : MonoBehaviour
{
    private int timer = 0;
    [SerializeField] private int time = 5;
    private bool left;
    private bool right;
    private int currY;
    private int score;
    private Drink currD;
    [SerializeField] private GameObject buttonLeft;
    [SerializeField] private GameObject buttonRight;
    [SerializeField] private Image leftIG;
    [SerializeField] private Image rightIG;
    [SerializeField] private GameObject shaker;

    private void Start()
    {
        currY = (int) buttonLeft.transform.position.y;
    }
    public void Begin(Drink d)
    {
        currD = d;
        score = 0;
        StartCoroutine(Game());
    }
    private IEnumerator Game() 
    {
        timer = 0;
        leftIG.color = Color.white;
        rightIG.color = Color.white;

        buttonLeft.transform.position = new Vector3(buttonLeft.transform.position.x, currY, buttonLeft.transform.position.z);
        buttonRight.transform.position = new Vector3(buttonRight.transform.position.x, currY, buttonRight.transform.position.z);

        while (timer<time)
        {
            left = true; right = false;
            leftIG.color = Color.red;
            rightIG.color = Color.white;
            buttonLeft.transform.position = new Vector3(buttonLeft.transform.position.x,40*Random.Range(-10,10)+600,buttonLeft.transform.position.z);

            Vector3 relativePos = buttonLeft.transform.position - shaker.transform.position;
            float angle = Mathf.Atan2(relativePos.y, relativePos.x);
            shaker.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);

            yield return new WaitForSeconds(1);

            left = false; right = true;
            leftIG.color = Color.white;
            rightIG.color = Color.red;
            buttonRight.transform.position = new Vector3(buttonRight.transform.position.x, 40 * Random.Range(-10, 10)+600, buttonRight.transform.position.z);

            relativePos = buttonRight.transform.position - shaker.transform.position;
            angle = Mathf.Atan2(relativePos.y, relativePos.x);
            shaker.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg); 

            yield return new WaitForSeconds(1);

            timer++;
            Debug.Log("Mix game: "+ score);
        }
        left = false; right = false;
        currD.AssignMix(score*10);
    }

    public void LeftClick()
    {
        if (left) { score++; }
    }
    public void RightClick()
    {
        if (right) { score++; }
    }
}
