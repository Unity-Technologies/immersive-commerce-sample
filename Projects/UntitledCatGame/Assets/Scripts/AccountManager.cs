using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Walmart.Commerce.Sdk;

public class AccountManager : MonoBehaviour
{
    public struct LoginResult
    {
        public bool Success;
        public string Id;
        public string AccessToken;
    }

    public string UserId => AuthenticationService.Instance.PlayerId;
    
    private bool _isAuthServiceInitialized = false;
    private string _cachedAccessToken;

    public async Task InitializeAsync()
    {
        try
        {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += OnUserSignedIn;
            AuthenticationService.Instance.SignInFailed += OnUserSignInFailed;
            AuthenticationService.Instance.SignedOut += OnUserSignedOut;
            AuthenticationService.Instance.Expired += OnLoginSessionExpired;
            
            _isAuthServiceInitialized = true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void OnDestroy()
    {
        if (_isAuthServiceInitialized)
        {
            AuthenticationService.Instance.SignedIn -= OnUserSignedIn;
            AuthenticationService.Instance.SignInFailed -= OnUserSignInFailed;
            AuthenticationService.Instance.SignedOut -= OnUserSignedOut;
            AuthenticationService.Instance.Expired -= OnLoginSessionExpired;
        }
    }
    
    private void Update()
    {
        if (_isAuthServiceInitialized && string.CompareOrdinal(_cachedAccessToken,AuthenticationService.Instance.AccessToken) != 0)
        {
            WalmartSdk.Instance.SetAuthorizationHeader("Bearer", AuthenticationService.Instance.AccessToken);
            _cachedAccessToken = AuthenticationService.Instance.AccessToken;
            
        }
    }

    public async Task<LoginResult> SignInAnonymously()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            _cachedAccessToken = AuthenticationService.Instance.AccessToken;
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
