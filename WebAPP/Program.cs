using Infrastructure.Search.DependencyInjection;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<WebAPP.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddProjections();
await builder.Build().RunAsync();