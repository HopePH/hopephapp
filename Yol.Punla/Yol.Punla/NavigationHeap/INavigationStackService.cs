using System.Collections.Generic;

namespace Yol.Punla.NavigationHeap
{
    public interface INavigationStackService
    {
        string CurrentStack { get; }
        Stack<string> NavigationStack { get; }

        void UpdateStackState(string page);
        void ResetStackStateTo(string page);
        void RemovePageFromNavigationStack(string page);
    }
}
