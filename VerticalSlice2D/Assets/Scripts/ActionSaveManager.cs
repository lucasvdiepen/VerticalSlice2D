using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSaveManager : MonoBehaviour
{
    public enum ActionType
    {
        Fight,
        Roar,
        SoftVoice,
        Spare,
        Defend,
        Nothing
    }

    public class Action
    {
        public string playerName;
        public GameObject enemyId;
        public ActionType actionType;

        public Action(string playerName, ActionType actionType)
        {
            this.playerName = playerName;
            this.actionType = actionType;
        }

        public void AddEnemyTarget(GameObject enemyId)
        {
            this.enemyId = enemyId;
        }
    }

    public List<Action> actions;
    private string currentPlayerName;

    private void Start()
    {
        actions = new List<Action>();
    }

    public void ResetActions()
    {
        actions.Clear();
    }

    public void AddAction(string playerName, ActionType actionType)
    {
        currentPlayerName = playerName;

        actions.Add(new Action(playerName, actionType));

        if(actionType == ActionType.SoftVoice)
        {
            actions.Add(new Action("Ralsei", ActionType.Nothing));
        }
    }

    public void AddEnemyTarget(GameObject enemyId)
    {
        for(int i = 0; i < actions.Count; i++)
        {
            if (actions[i].playerName == currentPlayerName) actions[i].AddEnemyTarget(enemyId);
            break;
        }
    }
}
