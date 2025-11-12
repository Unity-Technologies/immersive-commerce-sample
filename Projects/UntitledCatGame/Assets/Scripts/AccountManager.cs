using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    public struct LoginResult
    {
        public bool Success;
        public string Id;
        public string AccessToken;
    }

    public string UserId => AuthenticationService.Instance.PlayerId;

    public async Task InitializeAsync()
    {
        try
        {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += OnUserSignedIn;
            AuthenticationService.Instance.SignInFailed += OnUserSignInFailed;
            AuthenticationService.Instance.SignedOut += OnUserSignedOut;
            AuthenticationService.Instance.Expired += OnLoginSessionExpired;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void OnDestroy()
    {
        AuthenticationService.Instance.SignedIn -= OnUserSignedIn;
        AuthenticationService.Instance.SignInFailed -= OnUserSignInFailed;
        AuthenticationService.Instance.SignedOut -= OnUserSignedOut;
        AuthenticationService.Instance.Expired -= OnLoginSessionExpired;
    }

    public async Task<LoginResult> SignInAnonymously()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            return new LoginResult()
            {
                Success = true,
                Id = AuthenticationService.Instance.PlayerId,
                AccessToken = AuthenticationService.Instance.AccessToken
            };
        }
        catch (RequestFailedException e)
        {
            Debug.LogError($"Sign in anonymously failed with error code: {e.ErrorCode}");
        }

        // If we don't get a successful login attempt return a failed response here
        return new LoginResult()
        {
            Success = false,
            Id = default,
            AccessToken = default
        };
    }
    
    private void OnUserSignedIn()
    {
        string id = AuthenticationService.Instance.PlayerId;
        string token = AuthenticationService.Instance.AccessToken;
        Debug.Log($"Login Success -- UserId: {id} AccessToken: {token}");
    }

    private void OnUserSignInFailed(RequestFailedException obj)
    {
        Debug.LogError(obj.Message);
    }

    private void OnUserSignedOut()
    {
        Debug.Log($"User Signed Out");
    }

    private void OnLoginSessionExpired()
    {
        Debug.Log($"User Session Expired");
    }
}
