namespace BoardifyApp.Services;

public interface INavigationService
{
    void NavigateTo(object viewModel);
    void NavigateBack();
}