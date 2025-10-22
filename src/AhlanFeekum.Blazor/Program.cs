using AhlanFeekum.Blazor.Helpers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace AhlanFeekum.Blazor;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        // Firebase configuration variables
        var firebaseType = "service_account";
        var projectId = "ahlanfeekum-de666";
        var privateKeyId = "8cac693764b6461235aa5e2dda1747c5d3c8060e";
        var privateKey = "-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDDGmUP64ioCg+t\nDTl2uA3r1BpUlwM9C3nYbUcmqIx8c8hjfP4D7f/FHJwAqoo7gIH1d9T+beJ1rJHO\n0WIA5/WF+Oqf6TCkVtX/DVRL1DA0THSp0lC+yx7bAkgth2fyUPw13keFVZZw9AxD\nBK5/sFllZbpbOl9mUNwTYFjB6cveUoaYCxrkqNmcgMXE6udH18f89eKLbabCTMFT\ndyNwbTZxJnbr2W3fIpVk1ohMk6aKxTLNhZoKqNlDhYwNJyLZW+pNBQvhfkoukRkM\nvHPyM6vc2Pc6R67y3WC/8wsL2ys65DexuFLVTOPmQy/KIVRgOb7i8ivChJ/Ycj6D\nOEVRC7tfAgMBAAECggEAR4yMd3QnH4jhCtOIuVLyOAQGy/K9i0uEGayppTG2o4VK\nfcG+3e4QGFdPlqb0HrIX9s9FWzEtFX590lzTFgX68nDAinhYEEyQGgLnYQ+lxegr\n+INafT28SRDjK1827ZRpic/EZs9mZnH5dTJoVAd2IUtXNlGH3tvUeX6hzitRSmxR\nPtVJETDvw2SVVw09UOygZLVUHQTU+8CW2QYlM161d3jR0JkSXGuiMo4EFncQkSyz\nmDAADWqeaqiz8t/Ft66p3xO6BlAeCofNUT/8ys1XCDXe2YTfbOOy6YkEK6ZuPd9A\nAJL9GDr6h5GIzKGDcZcTL7X1liutdG5VCumgLpRPMQKBgQDprj9amg/sAD6r+BKQ\nSQKAdGibEC4M+0i37p60N4TgVS5F906Tc+aFqOMh9r0GKeYl2VyEg1CrDVy4tkEJ\nSn7YqhmWa5tjpG/TJ0rWkPhDNcWqH977Plj8yrhmUqsC0zWZxT+hi2hHKQSp73zF\nqjnqC88ZKwwh4CGLg0t3N5IuawKBgQDVvOIZZbzQVhptL0Js9Q464uw9bDLGsxSg\nHiwcYSZyjArWznbW3d3wAwQNTea98dNju7uVVSAylh5GJYraJPYlv9UZn0bfWn9v\nQXsYHsPLGWvN83231ypYGaiBx3Rvgv+HETrquvK09IEBj0pLkqxWCR+QqZCceZXF\nB7x04TU73QKBgQDaQ0ufd3jZqRzgiTPlmpwAmTo/Y2xv6aFxUyrfD2BIHKe86BJ3\nfXDadPW5ennxsN7riUt15iVJr5BkYq76W5+BBdgifF3GQwfDxNaM9Rk9xZLbpSsj\npod9jmeQAzUBzX8qImedntWGadSWWT9EbQvtBJyqWF/boDoU0kyBjOE4AwKBgQCc\nHrGQF44JJzt6UTDV4VGZlbM1UljsZlZEdoWF3Th2JvCr4ndPjxPTgoBtL1/Bkmfz\ny2bDN0Cjcp9+YWHA4YqiHDMN2hesga/flhbRkXc2XMUv1BEaPaICZAt/cC8OXMNE\n50L/vboWjxnEB+Qeu24CEfvqcNDeWrkZCAOR5AY8SQKBgE9u3fYgI5NXdPiBPAqc\nsMRyDJ15HVH4//hlkrI8GQWJ6zQpxAmPXaHxJ1FGI3rKy4MMS68M3SFzG0MNNK3y\nMM8gh1yumSJVhXeujO0wOaykDREjjL9V52FM7iPwB2pNkMe6TTLYl9AYKj43xuj+\niSQ6US3Jebhpr8E9oxzybQ+Z\n-----END PRIVATE KEY-----\n";
        var clientEmail = "firebase-adminsdk-fbsvc@ahlanfeekum-de666.iam.gserviceaccount.com";
        var clientId = "110527382662515187673";
        var authUri = "https://accounts.google.com/o/oauth2/auth";
        var tokenUri = "https://oauth2.googleapis.com/token";
        var authProviderX509CertUrl = "https://www.googleapis.com/oauth2/v1/certs";
        var clientX509CertUrl = "https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-fbsvc%40ahlanfeekum-de666.iam.gserviceaccount.com";
        var universeDomain = "googleapis.com";

        // Create JSON string from variables
        var firebaseJson = $@"{{
            ""type"": ""{firebaseType}"",
            ""project_id"": ""{projectId}"",
            ""private_key_id"": ""{privateKeyId}"",
            ""private_key"": ""{privateKey}"",
            ""client_email"": ""{clientEmail}"",
            ""client_id"": ""{clientId}"",
            ""auth_uri"": ""{authUri}"",
            ""token_uri"": ""{tokenUri}"",
            ""auth_provider_x509_cert_url"": ""{authProviderX509CertUrl}"",
            ""client_x509_cert_url"": ""{clientX509CertUrl}"",
            ""universe_domain"": ""{universeDomain}""
        }}";

        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromJson(firebaseJson),
        });

        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
             .WriteTo.Async(c => c.File("Logs/logs.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: 10485760))//10 MB Size (10  * 1024 * 1024)
           // .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
            .CreateLogger();

        try
        {
            Log.Information("Starting web host.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
            await builder.AddApplicationAsync<AhlanFeekumBlazorModule>();
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                    options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
                });
            var app = builder.Build();
            
            // Add CORS middleware before static files
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/ahlanfeekumassets"))
                {
                    // Allow any origin for both development and production
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
                }
                await next();
            });
            
            app.UseStaticFiles();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
