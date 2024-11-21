using PlayFab.ClientModels;
using PlayFab;
using UnityEngine.Rendering.VirtualTexturing;

public class DeviceLogin : ILogin
{
    private string deviceId;
    public DeviceLogin(string deviceId)
    {
        this.deviceId = deviceId;
    }
    public void Login(System.Action<LoginResult> onSucess,System.Action<PlayFabError> onFailure)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = deviceId,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, onSucess, onFailure);
    }
}
