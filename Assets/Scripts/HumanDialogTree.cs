﻿using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class HumanDialogTree: HumanDialogue
{
    public DialogTreeNode node;
    public GameObject optionsParent;
    public TextMeshProUGUI options;

    protected override void Start()
    {
        base.Start();
        optionsParent.SetActive(false);
    }

    protected override void LateUpdate()
    {
        optionsParent.SetActive(false); //default state inactive, this will be set to active on every relevant frame

        if (!inDialog) return;
        if (revealLock) return; //make sure we are not revealing
        DisplayOptions();
        
        //move cursor
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            option = (option - 1 + node.children.Length) % node.children.Length;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            option = (option + 1) % node.children.Length;
        }
        
        //deal with space
        if (Input.GetKeyDown(KeyCode.Space)) //make sure we are not locked
        {
            if(node.children.Length == 0) { //end condition
                this.player.dialogLock = false; //unlock player
                //kill everything else
                canvas.gameObject.SetActive(false);
                inDialog = false;
                i = 0; //rest i
                return;
            }

            node = node.children[option];
            StartCoroutine(TextReveal(node.message, speakingSpeed));
        }
    }

    public override void TriggerDialog(Player player)
    {
        //dialog
        inDialog = true;
        canvas.gameObject.SetActive(true);
        
        // float expectedTime = textScrollSpeed * speakingSpeed * text.Length;
        StartCoroutine(TextReveal(node.message, speakingSpeed));

        this.player = player;
        this.player.dialogLock = true;
        
        //human dialog
        FacePlayer(player.transform.position);
    }

    private int option = 0;
    private void DisplayOptions()
    {
        if (node.children.Length == 0) return; //we dont do crap if no child
        optionsParent.SetActive(true);
        StringBuilder b = new StringBuilder();
        var opts = node.children.Select(o => o.description).ToArray();
        for(int i = 0; i < opts.Length; i++)
        {
            if (i == option) b.Append("> ");
            b.Append(opts[i]);
            b.Append("\n");
        }

        options.text = b.ToString();
    }
}