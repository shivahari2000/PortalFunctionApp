// See https://aka.ms/new-console-template for more information
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.Resources;

Console.WriteLine("Hello, World!");

Console.WriteLine("Navigates to browser. please login...");

ArmClient client = new ArmClient(new InteractiveBrowserCredential());

SubscriptionResource subscription = await client.GetDefaultSubscriptionAsync();

Console.WriteLine("Enter the Resource Group Name:");

string resourceGroupName = Console.ReadLine() ?? string.Empty;

var resourceGroupResponse =  await subscription.GetResourceGroupAsync(resourceGroupName);

ResourceGroupResource resourceGroupResource = resourceGroupResponse.Value;

var websiteCollection = resourceGroupResource.GetWebSites();

await foreach (var site in websiteCollection.GetAllAsync())
{
    var app = (await site.GetApplicationSettingsAsync()).Value;
    var props = app.Properties;
    string instrumentationId = props.Keys?.Where(x => x.Contains("client"))?.FirstOrDefault();
    string clientID = string.Empty;
    if (!string.IsNullOrWhiteSpace(instrumentationId))
        clientID = props[instrumentationId];
    Console.WriteLine($"{site.Data.Name} - {instrumentationId} - {clientID}");
}

//await foreach (var site in websiteCollection.GetAllAsync())
//{
//    var app = (await site.GetApplicationSettingsAsync()).Value;
//    var props = app.Properties;
//    string instrumentationId = props.ContainsKey("APPINSIGHTS_INSTRUMENTATIONKEY") ? props["APPINSIGHTS_INSTRUMENTATIONKEY"] : string.Empty;
//    Console.WriteLine($"{site.Data.Name} - {instrumentationId}");
//}

Console.ReadLine();
    