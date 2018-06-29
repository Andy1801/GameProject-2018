using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ChooseAttack : PhaseParent {

    public GameObject attackPanel;
    public EventSystem attackEventSystem;


    private ChooseEnemy chooseEnemyPhase;
    private Button[] attackButtons;
    private Text[] attackText;

   
    private new void Start()
    {
        base.Start();
        chooseEnemyPhase = (ChooseEnemy)phaseSwap.PhaseUpdate((int)CurrentPhase.ChooseEnemy);
        attackButtons = new Button[gameManager.skillCount];
        attackText = new Text[gameManager.skillCount];

        int index = 0;

        foreach (Transform child in attackPanel.transform)
        {
            attackButtons[index] = child.GetComponent<Button>();

            attackText[index] = attackButtons[index].GetComponentInChildren<Text>();
            index++;
        }
    }

    public override void PhaseSetup()
    {
        stateManager.ChoosingAttack = true;

        attackPanel.SetActive(true);
        attackEventSystem.SetSelectedGameObject(attackPanel.transform.GetChild(0).gameObject);

        for(int index = 0; index < gameManager.skillCount; index++ )
        {
            //Set the text name for the button as well as the function the button will call.
            if (gameManager.ActiveUnit.skills[index] != null)
            {
                attackButtons[index].interactable = true;
                attackText[index].text = gameManager.ActiveUnit.skills[index].skillName;
            }
            else
            {
                attackButtons[index].interactable = false;
                attackText[index].text = "";
            }
        }

        gameManager.phaseReversal = PhaseReversal;
    }

    public override void PhaseReversal()
    {
        //Removes the attack menu shown to the player
        attackPanel.SetActive(false);

        // Bring back the field tracker
        stateManager.ChoosingAttack = false;

        gameManager.attackTile = null;
        attackEventSystem.SetSelectedGameObject(null);
        //Sends the phase back to the choose your enemy
        chooseEnemyPhase.PhaseSetup();
    }

    public void SetAttack(int skillIndex)
    {
        Debug.Log("The skill used was: " + Game_Manager.instance.ActiveUnit.skills[skillIndex].skillName);

        // Call the battle script


    }
}
