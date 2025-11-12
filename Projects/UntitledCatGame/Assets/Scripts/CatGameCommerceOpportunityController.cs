using UnityEngine;
using System;
using System.Threading.Tasks;
using Unity.Commerce.Backend;
using Unity.Commerce.CommerceOpportunity;
using Unity.Services.Authentication;
using Walmart.Commerce.Sdk;

public class CatGameCommerceOpportunityController : MonoBehaviour
{
    [SerializeField] private AccountManager _accountManager;
    
    private const string SAMPLE_COMMERCE_OPPORTUNITY_NAME = "Cat ComOp 01";
    
    private async void Awake()
    {
        if (!WalmartSdk.Instance.IsInitialized)
        {
            Debug.Log("WalmartSdk Initializing ...");
            await WalmartSdk.Instance.InitializeAsync();
            Debug.Log("<color=green>WalmartSdk Initialization complete!</color>");
            if (_accountManager != null)
            {
                await _accountManager.InitializeAsync();
                AccountManager.LoginResult result = await _accountManager.SignInAnonymously();
                bool loginSuccess = result.Success;
                if (loginSuccess)
                {
                    bool accountLinkStatus = await WalmartSdk.Instance.SetupAuthorizationHeaderAndCheckAccountLinkStatus("Bearer",
                        AuthenticationService.Instance.AccessToken);
                    Debug.Log("Account is " + (accountLinkStatus ? "linked" : "not linked"));
                }
                else
                {
                    Debug.LogError("Error logging into Cat Game Anonymously</color>");
                }
            }
        }
    }
    
    /// <summary>
    /// Call this method to show the Commerce Opportunity named in <see cref="SAMPLE_COMMERCE_OPPORTUNITY_NAME"/>.
    /// </summary>
    public void ShowCommerceOpportunity()
    {
        _ = ShowCommerceOpportunity(SAMPLE_COMMERCE_OPPORTUNITY_NAME);
    }

    private async Task ShowCommerceOpportunity(string commerceOpportunityName)
    {
        // Check if Commerce Opportunity with given name exists
        if (!WalmartSdk.Instance.TryGetCommerceOpportunity(commerceOpportunityName, out CommerceOpportunityInstance commerceOpportunity))
        {
            Debug.LogError($"Failed to find {nameof(CommerceOpportunityInstance)} with {nameof(CommerceOpportunityInstance.DisplayName)} '{commerceOpportunityName}'");
            return;
        }

        if (commerceOpportunity.ProductPackageId == Guid.Empty)
        {
            Debug.LogError($"{nameof(CommerceOpportunityInstance)} with {nameof(CommerceOpportunityInstance.DisplayName)} '{commerceOpportunityName}' has not been linked with a Product Package.");
            return;
        }

        BackendResponse response = await WalmartSdk.Instance.DownloadProductPackageDataAsync(commerceOpportunity.ProductPackageId);
        if (!response.IsSuccess)
        {
            Debug.LogError($"{response.Status.Code} ({response.Status.Description}): Failed to retrieve Product Package with ID '{commerceOpportunity.ProductPackageId}'.");
            return;
        }

        if (!WalmartSdk.Instance.TryShowCommerceOpportunity(commerceOpportunityName))
        {
            Debug.LogError($"Failed to show Commerce Opportunity: {commerceOpportunityName}");
        }
    }
}
