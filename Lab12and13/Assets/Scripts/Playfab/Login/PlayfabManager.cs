using PlayFab;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using UnityEngine;

public class PlayfabManager 
{
    private LoginManager loginManager;
    private string savedEmailKey = "SavedEmail";
    private string userEmail;
   private void Start()
    {
        loginManager = new LoginManager();
        //check if email is saved
        if(PlayerPrefs.HasKey(savedEmailKey))
        {
            string savedEmail = PlayerPrefs.GetString(savedEmailKey);
            // auto login with saved email
            EmailLoginButtonClicked(savedEmail, "SavedPassword");
        }
    }

    // Example method for triggering email login
    public void EmailLoginButtonClicked(string email, string password)
    {
        userEmail = email;
        loginManager.SetLoginMethod(new EmailLogin(email, password));
        loginManager.Login(OnLoginSuccess, OnloginFailure);


    }
    //example method for triggering device id login
    public void DeviceIDLoginbuttonClicked(string deviceID)
    {
        loginManager.SetLoginMethod(new DeviceLogin(deviceID));
        loginManager.Login(OnLoginSuccess, OnloginFailure);
    }
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login Successful");
        //handle success here, such as loading player data
        //save email for future auto-logins
        if (!string.IsNullOrEmpty(userEmail))
        {
            PlayerPrefs.SetString(savedEmailKey, userEmail);
            //Load player data
            LoadPlayerData(result.PlayFabId);
        }
    }
    private void OnloginFailure(PlayFabError error)
    {
        Debug.LogError("Login failed: " + error.ErrorMessage);
    }
    private void LoadPlayerData(string playfabId)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = playfabId
        };
        PlayFabClientAPI.GetUserData(request, OnDataSuccess, OnDataFailure);
    }
    private void OnDataSuccess(GetUserDataResult result)
    {
        // process player data here
        Debug.Log("Player data loaded successfully");
    }
    private void OnDataFailure(PlayFabError error)
    {
        Debug.LogError("Failed to load player data: " + error.ErrorMessage);
    }

}
