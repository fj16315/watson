using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using NPC;
using System;

namespace Notebook
{
    public enum Page : int { CHARACTER, INVENTORY, NOTES, MAP, MENU };
    public enum Item : int { KEY, BOOK, POISON };
    public enum Prop : int { RATPOISON, NIGHTSHADE, BARBITAL, WILL, THREATLETER, HERBSCRAP, MILITARYNOTE, MEDICALBOOK, REDHERRING, NOTEPAD }

    public class NotebookController : MonoBehaviour
    {

        public List<GameObject> propPictures;
        public Interactable[] interactables = new Interactable[9];
        public bool inspect = false;
        public GameObject currentInspect = null;

        public GameObject container;
        public PlayerController player;
        public AudioSource pageFlip;

        //GameObject tabsLeft;
        public GameObject tabsRightChars;
        public GameObject tabsEmpty;
        public Page currentPageEnum = (Page)6;

        // Buttons
        public List<Button> leftButtons;
        public List<Button> rightButtons;

        // Notebook pages
        public GameObject charPage;
        public GameObject invtPage;
        public GameObject notePage;
        public GameObject mapPage;
        public GameObject menuPage;
        GameObject currentPage;

        // Character pages
        public GameObject charActress;
        public GameObject charButler;
        public GameObject charColonel;
        public GameObject charCountess;
        public GameObject charEarl;
        public GameObject charGangster;
        public GameObject charPolice;
        GameObject currentChar;
        Character currentCharEnum = Character.POLICE;

        // Character clues
        List<Tuple<string, string> > cluesActress = new List<Tuple<string, string> >();
        List<Tuple<string, string>> cluesButler = new List<Tuple<string, string>>();
        List<Tuple<string, string>> cluesColonel = new List<Tuple<string, string>>();
        List<Tuple<string, string>> cluesCountess = new List<Tuple<string, string>>();
        List<Tuple<string, string>> cluesEarl = new List<Tuple<string, string>>();
        List<Tuple<string, string>> cluesGangster = new List<Tuple<string, string>>();
        List<Tuple<string, string>> cluesPolice = new List<Tuple<string, string>>();
        List<List<Tuple<string, string>>> cluesDirectory = new List<List<Tuple<string, string>>>();

        // Player notes
        List<string> playerNotes = new List<string>();

        public Text actressClueBox;
        public Text butlerClueBox;
        public Text colonelClueBox;
        public Text countessClueBox;
        public Text earlClueBox;
        public Text gangsterClueBox;
        public Text policeClueBox;

        public Text inventoryText;
        public Text notesText;

        private Color pressDelta = new Color(0.2f, 0.2f, 0.2f);

        public bool showing = false;

        // Use this for initialization
        void Start()
        {
            currentPage = menuPage;
            //ChangePage((int)Page.MENU);

            // Character pages
            currentChar = charPolice;
            charActress.SetActive(false);
            charButler.SetActive(false);
            charColonel.SetActive(false);
            charCountess.SetActive(false);
            charEarl.SetActive(false);
            charGangster.SetActive(false);
            charPolice.SetActive(false);
            rightButtons[(int)currentCharEnum].image.color += pressDelta;

            tabsRightChars.SetActive(false);
            tabsEmpty.SetActive(true);
            // Notebook pages
            charPage.SetActive(false);
            invtPage.SetActive(false);
            notePage.SetActive(false);
            mapPage.SetActive(false);
            menuPage.SetActive(true);

            // Add clue lists
            cluesDirectory.Add(cluesActress);
            cluesDirectory.Add(cluesButler);
            cluesDirectory.Add(cluesColonel);
            cluesDirectory.Add(cluesCountess);
            cluesDirectory.Add(cluesEarl);
            cluesDirectory.Add(cluesGangster);
            cluesDirectory.Add(cluesPolice);

            for (int i = 0; i < player.ownedProps.Length; i++)
            {
                player.ownedProps[i] = false;
            }
            Debug.Log(player.ownedProps);

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void NewInspect(GameObject prop)
        {
            currentInspect = prop;
        }

        public void Activate(bool active)
        {
            if (!inspect)
            {
                container.SetActive(active);
                UpdateInventory();
                if (currentPageEnum == Page.CHARACTER)
                {
                    ChangeCharacter((int)currentCharEnum);
                }
                else if (active)
                {
                    pageFlip.Play();
                }
            }
            else
            {
                inspect = false;
            }

        }

        public void ChangePage(int target)
        {
            if ((target != (int)currentPageEnum) && (!inspect)) 
            {
                if (currentPageEnum == Page.CHARACTER)
                {
                    tabsRightChars.SetActive(false);
                    tabsEmpty.SetActive(true);
                }
                currentPage.SetActive(false);

                // Change colour of buttons
                leftButtons[target].image.color += pressDelta;
                if ((int)currentPageEnum < 5)
                {
                    leftButtons[(int)currentPageEnum].image.color -= pressDelta;
                }

                switch ((Page)target)
                {
                    case Page.CHARACTER:
                        tabsEmpty.SetActive(false);
                        charPage.SetActive(true);
                        tabsRightChars.SetActive(true);
                        currentPage = charPage;
                        ChangeCharacter((int)currentCharEnum);
                        break;
                    case Page.INVENTORY:
                        invtPage.SetActive(true);
                        currentPage = invtPage;
                        UpdateInventory();
                        break;
                    case Page.NOTES:
                        notePage.SetActive(true);
                        UpdateNotes();
                        currentPage = notePage;
                        break;
                    case Page.MAP:
                        mapPage.SetActive(true);
                        currentPage = mapPage;
                        break;
                    case Page.MENU:
                        menuPage.SetActive(true);
                        currentPage = menuPage;
                        break;
                }
                currentPageEnum = (Page)target;
                pageFlip.Play();
            }
        }

        public void ChangeCharacter(int target)
        {
            currentChar.SetActive(false);

            // Change button colour
            rightButtons[target].image.color += pressDelta;
            rightButtons[(int)currentCharEnum].image.color -= pressDelta;

            switch ((Character)target)
            {
                case Character.ACTRESS:
                    charActress.SetActive(true);
                    actressClueBox.text = UpdateClues(target);
                    currentChar = charActress;
                    currentCharEnum = Character.ACTRESS;
                    break;
                case Character.BUTLER:
                    charButler.SetActive(true);
                    butlerClueBox.text = UpdateClues(target);
                    currentChar = charButler;
                    currentCharEnum = Character.BUTLER;
                    break;
                case Character.COLONEL:
                    charColonel.SetActive(true);
                    colonelClueBox.text = UpdateClues(target);
                    currentChar = charColonel;
                    currentCharEnum = Character.COLONEL;
                    break;
                case Character.COUNTESS:
                    charCountess.SetActive(true);
                    countessClueBox.text = UpdateClues(target);
                    currentChar = charCountess;
                    currentCharEnum = Character.COUNTESS;
                    break;
                case Character.EARL:
                    charEarl.SetActive(true);
                    earlClueBox.text = UpdateClues(target);
                    currentChar = charEarl;
                    currentCharEnum = Character.EARL;
                    break;
                case Character.GANGSTER:
                    charGangster.SetActive(true);
                    gangsterClueBox.text = UpdateClues(target);
                    currentChar = charGangster;
                    currentCharEnum = Character.GANGSTER;
                    break;
                case Character.POLICE:
                    charPolice.SetActive(true);
                    policeClueBox.text = UpdateClues(target);
                    currentChar = charPolice;
                    currentCharEnum = Character.POLICE;
                    break;
            }
            pageFlip.Play();
        }

        public int CharToEnum(string name)
        {
            switch (name.ToLower())
            {
                case "actress":
                    return (int)Character.ACTRESS;
                case "butler":
                    return (int)Character.BUTLER;
                case "colonel":
                    return (int)Character.COLONEL;
                case "countess":
                    return (int)Character.COUNTESS;
                case "earl":
                    return (int)Character.EARL;
                case "gangster":
                    return (int)Character.GANGSTER;
                case "police":
                    return (int)Character.POLICE;
            }
            return (int)Character.POLICE; // Default
        }

        public void LogResponse(NPCController character, string question, string clue)
        {
            //Debug.Log(question);
            //Debug.Log(clue);
            cluesDirectory[(int)character.GetEnum()].Add(new Tuple<string, string>(question, clue));
        }

        public void MakeNote(string note)
        {
            // If notes is empty or the new note is not the same as the previous
            if(!playerNotes.Any() || playerNotes[playerNotes.Count-1] != note) playerNotes.Add(note);
        }

        public string UpdateClues(int character)
        {
            string result = "";
            foreach (Tuple<string, string> exchange in cluesDirectory[character])
            {
                Debug.Log(exchange.Item1);
                if (exchange.Item1 != "")
                {
                    result += "<i>\"" + exchange.Item1 + "</i>\"  ~  ";
                }
                result += "\"" + exchange.Item2 + "\"\n";
            }
            return result;
        }

        public void UpdateInventory()
        {
            for (int i = 0; i < (int)Prop.NOTEPAD; i++)
            {
                propPictures[i].SetActive(player.ownedProps[i]);
            }
        }

        public void InspectObject(int propEnum)
        {
            inspect = true;
            interactables[propEnum].InspectObject();
        }

        public void UpdateNotes()
        {
            string result = "";
            foreach(string note in playerNotes)
            {
                result += "<b> </b>";
                if(note != "")
                {
                    result += "<i>" + note + "</i>\n";
                }
            }

            notesText.text = result;
        }

    }
}
