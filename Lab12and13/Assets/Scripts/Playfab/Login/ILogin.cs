using PlayFab.ClientModels;
using PlayFab;
public interface ILogin 
{
    void Login(System.Action<LoginResult> onSucces, System.Action<PlayFabError> onFailure);
}
