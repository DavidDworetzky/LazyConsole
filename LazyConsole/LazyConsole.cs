using LazyConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        private static ConsoleExceptionType ConsoleExceptionType = ConsoleExceptionType.Show;

        /// <summary>
        /// Helper method to generate our actions...
        /// </summary>
        /// <param name="cl"></param>
        /// <returns></returns>
        public static Dictionary<char, Tuple<string, Action>> GenerateActions(Type cl)
        {
            var actions = new Dictionary<char, Tuple<string, Action>>();
            var methods = cl.GetMethods(BindingFlags.Public | BindingFlags.Static);
            return GetActionForMethods(methods, Utilities.AvailableKeys.KeySelectionList);
        }

        private static Dictionary<char, Tuple<string, Action>> GetActionForMethods(MethodInfo[] methods, List<char> availableKeys)
        {
            var actions = new Dictionary<char, Tuple<string, Action>>();
            using (var enumerator = Utilities.AvailableKeys.KeySelectionList.GetEnumerator())
            {
                //enumerator.MoveNext() once 
                enumerator.MoveNext();
                foreach (var method in methods)
                {
                    if (method.ReturnType == typeof(void))
                    {
                        //create delegate action for static method
                        Action action = (Action)Delegate.CreateDelegate(typeof(Action), method);
                        //get enumerator for keys list 

                        actions.Add((char)enumerator.Current, Tuple.Create(method.Name, action));
                    }
                    else
                    {
                        //get delegate first
                        var dl = Delegate.CreateDelegate(Expression.GetDelegateType(method.ReturnType), method);
                        //non void, we need to convert this differently
                        Action action = new Action((() => dl.DynamicInvoke()));

                        actions.Add((char)enumerator.Current, Tuple.Create(method.Name, action));
                    }

                    enumerator.MoveNext();
                }

            }
            return actions;
        }

        /// <summary>
        /// Generates pages of potential actions for consumption by LazyConsole
        /// </summary>
        /// <param name="cl"></param>
        /// <returns></returns>
        public static Dictionary<char, Dictionary<char, Tuple<string, Action>>> GeneratePagedActions(Type cl)
        {
            int i = 0;
            var methods = cl.GetMethods(BindingFlags.Public | BindingFlags.Static);
            //calculate number of pages based off of methods
            var methodList = methods.ToList();
            int count = methodList.Count;

            int keyCount = Utilities.AvailableKeys.KeySelectionList.Count;

            int numberOfPages = (int)Math.Floor((double)(count / keyCount)) + 1;
            var pages = new Dictionary<char, Dictionary<char, Tuple<string, Action>>>();

            using (var enumerator = Utilities.AvailableKeys.KeySelectionList.GetEnumerator())
            {
                for (int j = 0; j < numberOfPages; j++)
                {
                    //get method boundaries
                    int lower = j * keyCount;
                    int upper = Math.Max((j + 1) * keyCount, methodList.Count);
                    var subsetMethods = methodList.GetRange(lower, upper - lower).ToList().ToArray();
                    //generate page
                    var page = GetActionForMethods(subsetMethods, Utilities.AvailableKeys.KeySelectionList);
                    //add page
                    pages.Add(enumerator.Current, page);
                    //get next key index
                    enumerator.MoveNext();
                }
            }
            return pages;
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
