using AhlanFeekum.Blazor.Components;
using AhlanFeekum.Blazor.Helpers;
using AhlanFeekum.Blazor.Menus;
using AhlanFeekum.EntityFrameworkCore;
using AhlanFeekum.Localization;
using AhlanFeekum.MultiTenancy;
using System.Linq;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazorise.RichTextEdit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme;
using Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Identity.Blazor.Server;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement.Blazor.Server;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Blazor.Server;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Microsoft.AspNetCore.Identity;

namespace AhlanFeekum.Blazor;

[DependsOn(
    typeof(AhlanFeekumApplicationModule),
    typeof(AhlanFeekumEntityFrameworkCoreModule),
    typeof(AhlanFeekumHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreComponentsServerLeptonXLiteThemeModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpIdentityBlazorServerModule),
    typeof(AbpTenantManagementBlazorServerModule),
    typeof(AbpSettingManagementBlazorServerModule)
   )]
public class AhlanFeekumBlazorModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(AhlanFeekumResource),
                typeof(AhlanFeekumDomainModule).Assembly,
                typeof(AhlanFeekumDomainSharedModule).Assembly,
                typeof(AhlanFeekumApplicationModule).Assembly,
                typeof(AhlanFeekumApplicationContractsModule).Assembly,
                typeof(AhlanFeekumBlazorModule).Assembly
            );
        });

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("AhlanFeekum");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });

        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
            {
                //   serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", "d6af8d31-b318-4eb3-97ef-84624926d5da");
                serverBuilder.AddProductionEncryptionAndSigningCertificate("/var/apps/AhlanFeekum/certs/openiddict.pfx", "d6af8d31-b318-4eb3-97ef-84624926d5da");
                serverBuilder.SetIssuer(new Uri(configuration["AuthServer:Authority"]));
                serverBuilder.SetAccessTokenLifetime(TimeSpan.FromDays(30));
                serverBuilder.UseAspNetCore().DisableTransportSecurityRequirement();
            });
        }

        PreConfigure<AbpAspNetCoreComponentsWebOptions>(options =>
        {
            options.IsBlazorWebApp = true;
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        // Add services to the container.
        context.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Add CORS configuration
        context.Services.AddCors(options =>
        {
            // Development policy - allows any origin for local development
            options.AddPolicy("AllowFlutterWeb", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
            
            // Production policy - specific origins only
            options.AddPolicy("AllowWebApp", builder =>
            {
                builder
                    .WithOrigins(
                        "http://srv954186.hstgr.cloud",  // Flutter web app (port 80)
                        "https://admin.srv954186.hstgr.cloud",  // Admin API
                        "http://ahlanfeekum.com",        // Production domain
                        "https://ahlanfeekum.com"        // Production domain with HTTPS
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
            
            // Alternative policy for development with specific origins
            options.AddPolicy("AllowSpecificOrigins", builder =>
            {
                builder
                    .WithOrigins(
                        "http://srv954186.hstgr.cloud",  // Production Flutter web
                        "https://admin.srv954186.hstgr.cloud",  // Admin API
                        "http://ahlanfeekum.com",        // Production domain
                        "https://ahlanfeekum.com",       // Production domain with HTTPS
                        "https://localhost:3000",        // Flutter web dev server
                        "http://localhost:3000"          // HTTP for development
                    )
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        ConfigureAuthentication(context);
        ConfigureUrls(configuration);
        ConfigureBundles();
        ConfigureAutoMapper();
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureSwaggerServices(context.Services, configuration);
        ConfigureAutoApiControllers();
        ConfigureBlazorise(context);
        ConfigureRouter(context);
        ConfigureMenu(context);

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = Path.Combine(hostingEnvironment.WebRootPath, "ahlanfeekumassets");
                });
            });
        });
        Configure<AbpAntiForgeryOptions>(options =>
        {
            options.AutoValidate = false;
        });

        ConfigureRichTextEdit(context);
        //Configure<AbpJsonOptions>(options =>
        //{
        //    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        //    options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        //});
        context.Services.AddSingleton<IActionResultExecutor<ObjectResult>, AhlanFeekumWrapResultExecutor>();

        // Configure password policy
        Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = false;              // Require at least one digit
            options.Password.RequireLowercase = false;          // Require at least one lowercase letter
            options.Password.RequireUppercase = false;          // Require at least one uppercase letter
            options.Password.RequireNonAlphanumeric = false;   // Don't require special characters
            options.Password.RequiredLength = 6;              // Minimum password length
            options.Password.RequiredUniqueChars = 0;          // Don't require unique characters
        });


    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());
        });


        // Ensure GetAttachmentPath() can read SelfUrl via env var when only appsettings is used
        var selfUrl = configuration["App:SelfUrl"];
        if (!string.IsNullOrWhiteSpace(selfUrl))
        {
            System.Environment.SetEnvironmentVariable("App__SelfUrl", selfUrl);
        }
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            // MVC UI
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );

            //BLAZOR UI
            options.StyleBundles.Configure(
                BlazorLeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/blazor-global-styles.css");
                    //You can remove the following line if you don't use Blazor CSS isolation for components
                    bundle.AddFiles(new BundleFile("/AhlanFeekum.Blazor.styles.css", true));
                }
            );
        });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<AhlanFeekumDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}AhlanFeekum.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<AhlanFeekumDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}AhlanFeekum.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<AhlanFeekumApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}AhlanFeekum.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<AhlanFeekumApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}AhlanFeekum.Application"));
                options.FileSets.ReplaceEmbeddedByPhysical<AhlanFeekumBlazorModule>(hostingEnvironment.ContentRootPath);
            });
        }
    }

    private void ConfigureSwaggerServices(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddAbpSwaggerGen(
        //    options =>
        //    {
        //        options.SwaggerDoc("v1", new OpenApiInfo { Title = "AhlanFeekum API", Version = "v1" });
        //        options.DocInclusionPredicate((docName, description) => true);
        //        options.CustomSchemaIds(type => type.FullName);
        //    }
        //);

        services.AddAbpSwaggerGenWithOAuth(
configuration["AuthServer:Authority"],
new Dictionary<string, string>
{
                    {"AhlanFeekum", "AhlanFeekum API"}
},
options =>
{
options.DocumentFilter<CustomSwaggerFilterHelper>();
options.OperationFilter<AddRequiredHeaderParameterHelper>();
options.SwaggerDoc("v1", new OpenApiInfo { Title = "AhlanFeekum API", Version = "v1" });
options.SwaggerDoc("v2", new OpenApiInfo { Title = "AhlanFeekum API For Mobile", Version = "v1" });
options.DocInclusionPredicate((docName, description) => true);
options.CustomSchemaIds(type => type.FullName);
});
    }

    private void ConfigureBlazorise(ServiceConfigurationContext context)
    {
        context.Services
            .AddBootstrap5Providers()
            .AddFontAwesomeIcons();
    }

    private void ConfigureMenu(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AhlanFeekumMenuContributor());
        });
    }

    private void ConfigureRouter(ServiceConfigurationContext context)
    {
        Configure<AbpRouterOptions>(options =>
        {
            options.AppAssembly = typeof(AhlanFeekumBlazorModule).Assembly;
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(AhlanFeekumApplicationModule).Assembly);
        });
    }

    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AhlanFeekumBlazorModule>();
        });
    }
    private void ConfigureRichTextEdit(ServiceConfigurationContext context)
    {
        context.Services
         .AddBlazoriseRichTextEdit(options =>
         {
             options.UseBubbleTheme = true;
         });
    }
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var env = context.GetEnvironment();
        var app = context.GetApplicationBuilder();

        app.UseAbpRequestLocalization();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseCorrelationId();
        
        // Add CORS middleware for static files - IMPORTANT: Add this before static files
        if (env.IsDevelopment())
        {
            app.UseCors("AllowFlutterWeb"); // Use permissive policy for development
        }
        else
        {
            app.UseCors("AllowWebApp"); // Use specific policy for production
        }
        
        app.MapAbpStaticAssets();
        
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }
        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAntiforgery();
        app.UseAuthorization();

        app.UseSwagger();
        //app.UseAbpSwaggerUI(options =>
        //{
        //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AhlanFeekum API");
        //});
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "AhlanFeekum API");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "AhlanFeekum API For Mobile");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            // options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
        });
        app.UseConfiguredEndpoints(builder =>
        {
            builder.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddAdditionalAssemblies(builder.ServiceProvider.GetRequiredService<IOptions<AbpRouterOptions>>().Value.AdditionalAssemblies.ToArray());
        });
    }
}
