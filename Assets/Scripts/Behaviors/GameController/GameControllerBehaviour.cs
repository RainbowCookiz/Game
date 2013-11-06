﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameControllerBehaviour : MonoBehaviour 
{
    public List<ActorBehavior> playerTeam = new List<ActorBehavior>();
    public List<ActorBehavior> enemyTeam = new List<ActorBehavior>();
    public List<ActorBehavior> nuetrals = new List<ActorBehavior>();
    public int playerTeamTotal;
    public int enemyTeamTotal;
    public int nuetralTotal;
    public int leftToMoveThis;
    private GUIStyle gUIStyle;
	public int numberOfTurns = 1;

    public enum UnitSide
    {
        player,
        enemy, 
        nuetral,
        NUMBER_OF_SIDES
    }

    public GameControllerBehaviour.UnitSide currentTurn = UnitSide.player;
	public HUDController controller;

	/// <summary>
	/// Sets the sides up, the end condition up, and the turn counter.
    /// 
    /// Alex Reiss
	/// </summary>
	void Start () 
    {
        for (int index = 0; index < playerTeam.Count; index++)
            playerTeam[index].theSide = GameControllerBehaviour.UnitSide.player;

        for (int index = 0; index < enemyTeam.Count; index++)
            enemyTeam[index].theSide = GameControllerBehaviour.UnitSide.enemy;

        for (int index = 0; index < nuetrals.Count; index++)
            nuetrals[index].theSide = GameControllerBehaviour.UnitSide.nuetral;

        playerTeamTotal = playerTeam.Count;
        enemyTeamTotal = enemyTeam.Count;
        nuetralTotal = nuetrals.Count;
        leftToMoveThis = playerTeamTotal;

        gUIStyle = new GUIStyle();
        gUIStyle.fontSize = 10;
        gUIStyle.normal.textColor = Color.white;
		
		controller = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDController>();
		controller.whoseTurn.text = "Players Turn";
		controller.turnCount.text = "Turn " + numberOfTurns.ToString();
	}

	void RemoveDeadUnits(List<ActorBehavior> list, ref int teamTotal)
	{
		for (int _i = 0; _i < list.Count; _i++)
		{
			if (list[_i] == null)
			{
				list.RemoveAt(_i);
				teamTotal--;
			}
		}
	}

    /// <summary>
    /// Checks end game conditions, and end turn conditions.
    /// 
    /// Alex Reiss
    /// </summary>
    void Update()
    {
		// Remove dead units.
		RemoveDeadUnits(playerTeam, ref playerTeamTotal);
		RemoveDeadUnits(enemyTeam, ref enemyTeamTotal);
		RemoveDeadUnits(nuetrals, ref nuetralTotal);

        EndGame();

        //if (enemyTeamTotal == 0)
        //{
        //    Application.LoadLevel("PlayerWins");
        //}

        //if (playerTeamTotal == 0)
        //{
        //    Application.LoadLevel("PlayerLosses");
        //}

        if (Input.GetKeyDown(KeyCode.Space) || leftToMoveThis == 0)
        {

            EndTurn();
    //        for (int index = 0; index < playerTeam.Count; index++)
    //            playerTeam[index].actorHasMovedThisTurn = false;

    //        for (int index = 0; index < enemyTeam.Count; index++)
    //            enemyTeam[index].actorHasMovedThisTurn = false;

    //        for (int index = 0; index < nuetrals.Count; index++)
    //            nuetrals[index].actorHasMovedThisTurn = false;

    //        if (currentTurn == UnitSide.player)
    //        {
    //            currentTurn = UnitSide.enemy;
    //            leftToMoveThis = enemyTeamTotal;
    //        }
    //        else
    //        {
    //            currentTurn = UnitSide.player;
    //            leftToMoveThis = playerTeamTotal;
    //        }
        }
    }

    public void EndGame()
    {
        if (enemyTeamTotal == 0)
        {
            Application.LoadLevel("PlayerWins");
        }

        if (playerTeamTotal == 0)
        {
            Application.LoadLevel("PlayerLosses");
        }
    }

    public void EndTurn()
    {
		GameObject gridObject = (GameObject)GameObject.FindGameObjectWithTag("Grid");
		GridBehavior grid = gridObject.GetComponent<GridBehavior>();
		if (grid != null)
			grid.disableCurrentActor();

        for (int index = 0; index < playerTeam.Count; index++)
            playerTeam[index].actorHasMovedThisTurn = false;

        for (int index = 0; index < enemyTeam.Count; index++)
            enemyTeam[index].actorHasMovedThisTurn = false;

        for (int index = 0; index < nuetrals.Count; index++)
            nuetrals[index].actorHasMovedThisTurn = false;

        if (currentTurn == UnitSide.player)
        {
            currentTurn = UnitSide.enemy;
            leftToMoveThis = enemyTeamTotal;
			controller.whoseTurn.text = "Enemy Turn";
        }
        else
        {
            currentTurn = UnitSide.player;
            leftToMoveThis = playerTeamTotal;
			controller.whoseTurn.text = "Player Turn";
			numberOfTurns++;
			controller.turnCount.text = "Turn " + numberOfTurns.ToString();
        }
    }
}
