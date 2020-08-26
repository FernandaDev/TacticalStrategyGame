﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    static Queue<ICommand> commandQueue = new Queue<ICommand>();

    bool isBusy = false; // TODO set up the busy thing

    public static void AddCommand(ICommand newCommand) => commandQueue.Enqueue(newCommand);

    private void Update()
    {
        if(commandQueue.Count > 0)
        {
            if(!isBusy)
            {
                commandQueue.Dequeue().Execute();
            }
        }
    }
} 