using System;
using System.Collections;
using System.Data.Common;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{


    public TextMeshProUGUI CatNameUI;
    public TextMeshProUGUI CatTalkUI;
    public GameObject OptionPanel;
    public GameObject DialogueArea;
    public Button[] buttons = new Button[3];
    
    [Header("Cat Dialogue")]
    public string[] lines;
    public int index;
    public bool OptionsClicked; 
    public bool Accepted;
    

    [Header("Cat details")]
    [SerializeField] GameObject MeowPrefab;
    [SerializeField] string CatName = "Cat";
    public bool PlayerInside;
    public bool NPCTalking = false;
    [SerializeField] PlayerController PC;
    void Start()
    {
        PC = FindAnyObjectByType<PlayerController>();
        CatNameUI = GameObject.Find("CatNameUI").GetComponent<TextMeshProUGUI>();
        CatTalkUI = GameObject.Find("CatDialogueUI").GetComponent<TextMeshProUGUI>();
        DialogueArea = GameObject.Find("DialogueArea");
        OptionPanel = GameObject.Find("OptionPanel");

        for (int i = 0; i < OptionPanel.transform.childCount; i++)
        {
            buttons[i] = OptionPanel.transform.GetChild(i).GetComponent<Button>();
        }
        OptionPanel.SetActive(false);
        DialogueArea.SetActive(false);
    }

    private void Update()
    {
        if (!PlayerInside) return;//not inside
        CheckText();
        if (Accepted == true) return;

         if (Mouse.current.leftButton.wasPressedThisFrame && !OptionPanel.activeSelf) // option panel is off
         {
            if (CatTalkUI.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                CatTalkUI.text = lines[index];
            }
        }


    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // player is inside area
        {
            if (PC != null)
            {
                PlayerInside = true;
                collision.SendMessage("SetCATNPC",this, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // player is inside area
        {
            if (PC != null)
            {
                PlayerInside = false;
                collision.SendMessage("RemoveCATNPC", SendMessageOptions.DontRequireReceiver);
                OptionPanel.SetActive(false);
                DialogueArea.SetActive(false);
                GetComponent<Animator>().Play("BlackCatIDLE");
            }
        }
    }


    public void StartAction()
    {
        
    }


    public void StartDialouge()
    {
        if (Accepted == true) { DialogueArea.SetActive(true); OptionPanel.SetActive(false); CatTalkUI.text = "GO ON MEOWBORN";  return;};


        if (NPCTalking == true && !OptionPanel.activeSelf)
        {
            if (CatTalkUI.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                CatTalkUI.text = lines[index];
            }
            return;
        }

        NPCTalking = true;
        OptionsClicked = false;
        index = 0;
        DialogueArea.SetActive(true);
        CatNameUI.text = CatName;
        //CatTalkUI.text = "Meowwww";
        StartCoroutine(TypeLine());
    }

    public void CheckText()
    {
        if(OptionsClicked) 
            return;

        switch (index)
        {
            case 3:
                
                OptionPanel.SetActive(true);
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "yes";
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "no";
                buttons[2].gameObject.SetActive(false); 

                buttons[0].onClick.AddListener(() => OnButtonClicked(0));
                buttons[1].onClick.AddListener(() => OnButtonClicked(1));
             break;
        }
    }

    IEnumerator TypeLine()
    {
        CatTalkUI.text = "";
        foreach (char c in lines[index].ToCharArray())
        {
            CatTalkUI.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void FollowLoaf()
    {
        StartCoroutine(Loafing());
    }

    IEnumerator Loafing()
    {
        yield return new WaitForSeconds(1.0f);
        GetComponent<Animator>().Play("BlackCatLoaf");
    }

    void OnButtonClicked(int clickedIndex)
    {

        switch (clickedIndex)
        {
            case 0:
                CatTalkUI.text = String.Empty;
                CatTalkUI.text = "You Gained MeowPower!";
                Accepted = true;
                OptionPanel.SetActive(false);

                PC.unlockSkill();
                OptionsClicked = true;
                break;
            case 1:
                CatTalkUI.text = String.Empty;
                CatTalkUI.text = "Alright then Meow";
                OptionPanel.SetActive(false);
                OptionsClicked = true;
                break;
        }
    }


    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            CatTalkUI.text = string.Empty;
            StartCoroutine(TypeLine());
        }
     
    }
}
