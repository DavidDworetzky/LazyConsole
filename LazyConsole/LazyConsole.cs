using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LazyConsole
{
    public static class LazyConsole
    {
        /// <summary>
        /// Starts up a Lazy Console using reflections to populate the list of actions. No muss no fuss.
        /// </summary>
        /// <param name="t"></param>
        public static void StartConsole(Type t)
        {
            InitializActionsFromClass(t);
            MenuLoop();
        }

        private static Dictionary<char, Tuple<string, Action>> Actions = new Dictionary<char, Tuple<string, Action>>();

        /// <summary>
        /// Helper method to generate our actions...
        /// </summary>
        /// <param name="cl"></param>
        /// <returns></returns>
        public static Dictionary<char, Tuple<string, Action>> GenerateActions(Type cl)
        {
            var actions = new Dictionary<char, Tuple<string, Action>>();

            using (var enumerator = Utilities.AvailableKeys.KeySelectionList.GetEnumerator())
            {
                enumerator.MoveNext();
                var methods = cl.GetMethods(BindingFlags.Public | BindingFlags.Static);
                //for each public method in target type
                foreach (var method in methods)
                {
                    //create delegate action for static method
                    Action action = (Action)Delegate.CreateDelegate(typeof(Action), method);
                    //get enumerator for keys list 

                    actions.Add((char)enumerator.Current, Tuple.Create(method.Name, action));

                    enumerator.MoveNext();
                }
            }
            return actions;
        }

        /// <summary>
        /// We use reflection to populate our list of actions
        /// </summary>
        private static void InitializActionsFromClass(Type cl)
        {
            Actions = GenerateActions(cl);
        }
        private static void MenuLoop()
        {
            var selection = (char)ConsoleKey.Enter;
            while (selection != (char)ConsoleKey.Escape)
            {
                //Display menu
                DisplayMenu();

                //Get the user input selection
                selection = WaitSelection();

                //Execute our action
                if (!Actions.Keys.Contains(selection)) continue;

                try
                {
                    Actions[selection].Item2();
                    System.Console.WriteLine();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine($"Exception: {e.Message}");
                    System.Console.WriteLine($"Inner Exception: {e.InnerException?.Message}");
                    throw;
                }
            }
        }
        private static void DisplayMenu()
        {
            var displayActions = Actions.ToList();

            //sort our actions in place (by char)
            displayActions.Sort(Utilities.OptionExtensions.CompareOptions);

            foreach (var selection in displayActions)
            {
                System.Console.WriteLine($"{selection.Key}:{selection.Value.Item1}");
            }
        }

        private static char WaitSelection()
        {
            var selection = (char)ConsoleKey.Enter;
            do
            {
                var keyInfo = System.Console.ReadKey(true);
                selection = char.ToUpper(keyInfo.KeyChar);
            }

            while (!Actions.ContainsKey(selection) && selection != (char)ConsoleKey.Escape);
            return selection;
        }
    }
}
