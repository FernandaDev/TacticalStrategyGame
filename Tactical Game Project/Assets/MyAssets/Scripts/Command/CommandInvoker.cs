using System;
using System.Collections.Generic;
using UnityEngine;

namespace FernandaDev
{
    public class CommandInvoker : MonoBehaviour
    {
        static List<ICommand> currentPlayerCommandList = new List<ICommand>();

        public static event Action OnCommandConfirmationRequest;
        static ICommand currentCommand;

        public static void AddCommand(ICommand newCommand)
        {
            // We only need to know what the player does in their turn, this way we can create a new list every time a turn changes.
            currentCommand = newCommand;
            ICommandController currentCommandController = currentCommand.commandController;

            // After the command's animation(movement for now) ends, we need make the confirmation panel show up and wait for the response.
            currentCommandController.OnCommandAnimationEnd += WaitConfirmationResponse;

            currentCommand.Execute();
        }

        // TODO the way we set up, it's only alowed to confirm AFTER the command has been completed. This works for the movement, but NOT for the attack!!!
        // Perhaps we need to make a function to dictate how the command's confirmation should work. 
        static void WaitConfirmationResponse()
        {
            OnCommandConfirmationRequest?.Invoke();
        }

        public static void OnCommandConfirmationResponse(bool confirm)
        {
            if (!confirm)
                currentCommand.Undo();
            else
                currentPlayerCommandList.Add(currentCommand);

            OnCommandConfirmationRequest = null;
            currentCommand = null;
            CommandInputReader.Instance.EndInputRead();
        }

        void OnTurnChanged()
        {
            currentPlayerCommandList.Clear();
        }
    }
}