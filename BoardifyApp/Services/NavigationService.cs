namespace BoardifyApp.Services;

public class NavigationService : INavigationService
{
    private readonly Action<object> _setViewModelAction;
    private readonly Stack<object> _navigationStack = [];

    public NavigationService(Action<object> setViewModelAction)
    {
        _setViewModelAction = setViewModelAction;
        if (_setViewModelAction.Target != null)
        {
            _navigationStack.Push(_setViewModelAction.Target);
        }
    }

    public void NavigateTo(object viewModel)
    {
        _navigationStack.Push(viewModel);
        _setViewModelAction(viewModel);
    }

    public void NavigateBack()
    {
        if (_navigationStack.Count <= 1)
        {
            return;
        }

        _navigationStack.Pop();
        _setViewModelAction(_navigationStack.Peek());
    }
}