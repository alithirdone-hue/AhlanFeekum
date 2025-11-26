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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AhlanFeekum.Blazor;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        //// Firebase configuration variables
        //var firebaseType = "service_account";
        //var projectId = "ahlanfeekum-de666";
        //var privateKeyId = "8cac693764b6461235aa5e2dda1747c5d3c8060e";
        //var privateKey = "-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDDGmUP64ioCg+t\nDTl2uA3r1BpUlwM9C3nYbUcmqIx8c8hjfP4D7f/FHJwAqoo7gIH1d9T+beJ1rJHO\n0WIA5/WF+Oqf6TCkVtX/DVRL1DA0THSp0lC+yx7bAkgth2fyUPw13keFVZZw9AxD\nBK5/sFllZbpbOl9mUNwTYFjB6cveUoaYCxrkqNmcgMXE6udH18f89eKLbabCTMFT\ndyNwbTZxJnbr2W3fIpVk1ohMk6aKxTLNhZoKqNlDhYwNJyLZW+pNBQvhfkoukRkM\nvHPyM6vc2Pc6R67y3WC/8wsL2ys65DexuFLVTOPmQy/KIVRgOb7i8ivChJ/Ycj6D\nOEVRC7tfAgMBAAECggEAR4yMd3QnH4jhCtOIuVLyOAQGy/K9i0uEGayppTG2o4VK\nfcG+3e4QGFdPlqb0HrIX9s9FWzEtFX590lzTFgX68nDAinhYEEyQGgLnYQ+lxegr\n+INafT28SRDjK1827ZRpic/EZs9mZnH5dTJoVAd2IUtXNlGH3tvUeX6hzitRSmxR\nPtVJETDvw2SVVw09UOygZLVUHQTU+8CW2QYlM161d3jR0JkSXGuiMo4EFncQkSyz\nmDAADWqeaqiz8t/Ft66p3xO6BlAeCofNUT/8ys1XCDXe2YTfbOOy6YkEK6ZuPd9A\nAJL9GDr6h5GIzKGDcZcTL7X1liutdG5VCumgLpRPMQKBgQDprj9amg/sAD6r+BKQ\nSQKAdGibEC4M+0i37p60N4TgVS5F906Tc+aFqOMh9r0GKeYl2VyEg1CrDVy4tkEJ\nSn7YqhmWa5tjpG/TJ0rWkPhDNcWqH977Plj8yrhmUqsC0zWZxT+hi2hHKQSp73zF\nqjnqC88ZKwwh4CGLg0t3N5IuawKBgQDVvOIZZbzQVhptL0Js9Q464uw9bDLGsxSg\nHiwcYSZyjArWznbW3d3wAwQNTea98dNju7uVVSAylh5GJYraJPYlv9UZn0bfWn9v\nQXsYHsPLGWvN83231ypYGaiBx3Rvgv+HETrquvK09IEBj0pLkqxWCR+QqZCceZXF\nB7x04TU73QKBgQDaQ0ufd3jZqRzgiTPlmpwAmTo/Y2xv6aFxUyrfD2BIHKe86BJ3\nfXDadPW5ennxsN7riUt15iVJr5BkYq76W5+BBdgifF3GQwfDxNaM9Rk9xZLbpSsj\npod9jmeQAzUBzX8qImedntWGadSWWT9EbQvtBJyqWF/boDoU0kyBjOE4AwKBgQCc\nHrGQF44JJzt6UTDV4VGZlbM1UljsZlZEdoWF3Th2JvCr4ndPjxPTgoBtL1/Bkmfz\ny2bDN0Cjcp9+YWHA4YqiHDMN2hesga/flhbRkXc2XMUv1BEaPaICZAt/cC8OXMNE\n50L/vboWjxnEB+Qeu24CEfvqcNDeWrkZCAOR5AY8SQKBgE9u3fYgI5NXdPiBPAqc\nsMRyDJ15HVH4//hlkrI8GQWJ6zQpxAmPXaHxJ1FGI3rKy4MMS68M3SFzG0MNNK3y\nMM8gh1yumSJVhXeujO0wOaykDREjjL9V52FM7iPwB2pNkMe6TTLYl9AYKj43xuj+\niSQ6US3Jebhpr8E9oxzybQ+Z\n-----END PRIVATE KEY-----\n";
        //var clientEmail = "firebase-adminsdk-fbsvc@ahlanfeekum-de666.iam.gserviceaccount.com";
        //var clientId = "110527382662515187673";
        //var authUri = "https://accounts.google.com/o/oauth2/auth";
        //var tokenUri = "https://oauth2.googleapis.com/token";
        //var authProviderX509CertUrl = "https://www.googleapis.com/oauth2/v1/certs";
        //var clientX509CertUrl = "https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-fbsvc%40ahlanfeekum-de666.iam.gserviceaccount.com";
        //var universeDomain = "googleapis.com";

        //// Create JSON string from variables
        //var firebaseJson = $@"{{
        //    ""type"": ""{firebaseType}"",
        //    ""project_id"": ""{projectId}"",
        //    ""private_key_id"": ""{privateKeyId}"",
        //    ""private_key"": ""{privateKey}"",
        //    ""client_email"": ""{clientEmail}"",
        //    ""client_id"": ""{clientId}"",
        //    ""auth_uri"": ""{authUri}"",
        //    ""token_uri"": ""{tokenUri}"",
        //    ""auth_provider_x509_cert_url"": ""{authProviderX509CertUrl}"",
        //    ""client_x509_cert_url"": ""{clientX509CertUrl}"",
        //    ""universe_domain"": ""{universeDomain}""
        //}}";

        //FirebaseApp.Create(new AppOptions()
        //{
        //    Credential = GoogleCredential.FromJson(firebaseJson),
        //});

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

            //// Initialize Firebase with better error handling
            //try
            //{
            //    var baseDir = AppContext.BaseDirectory;
            //    var currentDir = Directory.GetCurrentDirectory();
            //    var firebaseCredPath = Path.Combine(baseDir, "ahlanfeekum-3b824-firebase-adminsdk-fbsvc-fee901ea51.json");

            //    Log.Information("AppContext.BaseDirectory: {BaseDir}", baseDir);
            //    Log.Information("Current Directory: {CurrentDir}", currentDir);
            //    Log.Information("Looking for Firebase credentials at: {Path}", firebaseCredPath);

            //    if (File.Exists(firebaseCredPath))
            //    {
            //        Log.Information("Firebase credentials file found. Initializing...");
            //        FirebaseApp.Create(new AppOptions()
            //        {
            //            Credential = GoogleCredential.FromFile(firebaseCredPath),
            //        });
            //        Log.Information("Firebase initialized successfully.");
            //    }
            //    else
            //    {
            //        Log.Warning("Firebase credentials file NOT found at: {Path}", firebaseCredPath);

            //        // Check if file exists in current directory
            //        var altPath = Path.Combine(currentDir, "ahlanfeekum-3b824-firebase-adminsdk-fbsvc-fee901ea51.json");
            //        if (File.Exists(altPath))
            //        {
            //            Log.Information("Found Firebase credentials in current directory, using: {Path}", altPath);
            //            FirebaseApp.Create(new AppOptions()
            //            {
            //                Credential = GoogleCredential.FromFile(altPath),
            //            });
            //            Log.Information("Firebase initialized successfully from current directory.");
            //        }
            //        else
            //        {
            //            Log.Warning("Firebase credentials not found in current directory either: {Path}", altPath);

            //            // List files to help debug
            //            try
            //            {
            //                var filesInBase = Directory.GetFiles(baseDir).Take(20).Select(Path.GetFileName);
            //                Log.Information("Files in base directory (first 20): {Files}", string.Join(", ", filesInBase));
            //            }
            //            catch { }
            //        }
            //    }
            //}
            //catch (Exception firebaseEx)
            //{
            //    Log.Error(firebaseEx, "Failed to initialize Firebase. Application will continue without Firebase.");
            //}





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


            //// Firebase configuration variables
            //var firebaseType = "service_account";
            //var projectId = "ahlanfeekum-de666";
            //var privateKeyId = "8cac693764b6461235aa5e2dda1747c5d3c8060e";
            //var privateKey = "-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDDGmUP64ioCg+t\nDTl2uA3r1BpUlwM9C3nYbUcmqIx8c8hjfP4D7f/FHJwAqoo7gIH1d9T+beJ1rJHO\n0WIA5/WF+Oqf6TCkVtX/DVRL1DA0THSp0lC+yx7bAkgth2fyUPw13keFVZZw9AxD\nBK5/sFllZbpbOl9mUNwTYFjB6cveUoaYCxrkqNmcgMXE6udH18f89eKLbabCTMFT\ndyNwbTZxJnbr2W3fIpVk1ohMk6aKxTLNhZoKqNlDhYwNJyLZW+pNBQvhfkoukRkM\nvHPyM6vc2Pc6R67y3WC/8wsL2ys65DexuFLVTOPmQy/KIVRgOb7i8ivChJ/Ycj6D\nOEVRC7tfAgMBAAECggEAR4yMd3QnH4jhCtOIuVLyOAQGy/K9i0uEGayppTG2o4VK\nfcG+3e4QGFdPlqb0HrIX9s9FWzEtFX590lzTFgX68nDAinhYEEyQGgLnYQ+lxegr\n+INafT28SRDjK1827ZRpic/EZs9mZnH5dTJoVAd2IUtXNlGH3tvUeX6hzitRSmxR\nPtVJETDvw2SVVw09UOygZLVUHQTU+8CW2QYlM161d3jR0JkSXGuiMo4EFncQkSyz\nmDAADWqeaqiz8t/Ft66p3xO6BlAeCofNUT/8ys1XCDXe2YTfbOOy6YkEK6ZuPd9A\nAJL9GDr6h5GIzKGDcZcTL7X1liutdG5VCumgLpRPMQKBgQDprj9amg/sAD6r+BKQ\nSQKAdGibEC4M+0i37p60N4TgVS5F906Tc+aFqOMh9r0GKeYl2VyEg1CrDVy4tkEJ\nSn7YqhmWa5tjpG/TJ0rWkPhDNcWqH977Plj8yrhmUqsC0zWZxT+hi2hHKQSp73zF\nqjnqC88ZKwwh4CGLg0t3N5IuawKBgQDVvOIZZbzQVhptL0Js9Q464uw9bDLGsxSg\nHiwcYSZyjArWznbW3d3wAwQNTea98dNju7uVVSAylh5GJYraJPYlv9UZn0bfWn9v\nQXsYHsPLGWvN83231ypYGaiBx3Rvgv+HETrquvK09IEBj0pLkqxWCR+QqZCceZXF\nB7x04TU73QKBgQDaQ0ufd3jZqRzgiTPlmpwAmTo/Y2xv6aFxUyrfD2BIHKe86BJ3\nfXDadPW5ennxsN7riUt15iVJr5BkYq76W5+BBdgifF3GQwfDxNaM9Rk9xZLbpSsj\npod9jmeQAzUBzX8qImedntWGadSWWT9EbQvtBJyqWF/boDoU0kyBjOE4AwKBgQCc\nHrGQF44JJzt6UTDV4VGZlbM1UljsZlZEdoWF3Th2JvCr4ndPjxPTgoBtL1/Bkmfz\ny2bDN0Cjcp9+YWHA4YqiHDMN2hesga/flhbRkXc2XMUv1BEaPaICZAt/cC8OXMNE\n50L/vboWjxnEB+Qeu24CEfvqcNDeWrkZCAOR5AY8SQKBgE9u3fYgI5NXdPiBPAqc\nsMRyDJ15HVH4//hlkrI8GQWJ6zQpxAmPXaHxJ1FGI3rKy4MMS68M3SFzG0MNNK3y\nMM8gh1yumSJVhXeujO0wOaykDREjjL9V52FM7iPwB2pNkMe6TTLYl9AYKj43xuj+\niSQ6US3Jebhpr8E9oxzybQ+Z\n-----END PRIVATE KEY-----\n";
            //var clientEmail = "firebase-adminsdk-fbsvc@ahlanfeekum-de666.iam.gserviceaccount.com";
            //var clientId = "110527382662515187673";
            //var authUri = "https://accounts.google.com/o/oauth2/auth";
            //var tokenUri = "https://oauth2.googleapis.com/token";
            //var authProviderX509CertUrl = "https://www.googleapis.com/oauth2/v1/certs";
            //var clientX509CertUrl = "https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-fbsvc%40ahlanfeekum-de666.iam.gserviceaccount.com";
            //var universeDomain = "googleapis.com";


            //var firebaseType = "service_account";
            //var projectId = "ahlanfeekum-de666";
            //var privateKeyId = "9c58fa73ee676706cb36279b9e178e56c16f5d34";
            //var privateKey = "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCldrr3Y03Ya+DJ\nJjMbV/sqXqi8G0uBKkXxrgFsRREa6fajHO7jmatM534gXOAoP5d75qoaBu+ASRps\n923G2wxzfW2ex/PcbyPWM/910FqQeQdNI9qDpUDqmDSun+pxD7C9rhKEZTOwwNZN\nacVIcEdZC0pfnAKQp+Mle5vLArtgwr+G9gJ//khwjlNIe1sXhS4Z1XBBIvDLKcY3\n/6vrBVclkxi4hKpJ6+qMT87Jgl4JnIc6RIh2w1RH24LyqTmcDwhtxjbvglRDfF4B\nmVrGl73xBTsvXzy8Tb99S5kHqs1OOQ+hPdFV9ZIPSiNlX/8V0nM2Ps/LbCrDQe2T\nZ+ZsQCHxAgMBAAECggEACTFMd9wLgh+UMHiv1DASIyt5ImI90qI/moq5R4OazKz8\n59j+qKRr8OzXhALE7VeDGf4ZqzKBcdYd4rnoXmjjCzOHuI+RS8jc69Sdt7Tooygm\nMLHyPOgccCwemSkHRFrDzfXyz2oz8fj0rv5WWIzsWdUXv7ISLmk1ZGq2oBO5x0lT\njVOdiTAL0EifEcF2KxqrQDYU0JH58+az3aayidl/RXFm6ing0nRD8ubKQ8bGXilq\nvu3FMC9L3rMY1qCMFLMgeVHrwQF9gqHU7i/zQ5uFYma5p32HXjSo0bqKO/84Z+KJ\n14sbv51qtWV5z6RQ70DejbHWlTtUUSjGF3cOcb7y8QKBgQDk2MRLh5vVAjkHzGCc\ny5dcQLxoZgNF+DlEF0AuvyUwUlFG7vu1TPyZtaLbd4407dxAzWdR/Q6yvCzbBUQN\nR3AacJpO3KMxtuysbb49SppT8ZAebpRh22rnJXbui0CAYZqhh90Q7MlyuL4Hsp0U\nb+8sSA09FipSKWRY7dVOtGt9LQKBgQC5GLPebZ8O7y51bv1J5KeOAci8A/0brkL/\nrWZAdIDxZMCQW92KlciL4zWNQo2RUBO9b7XmrSOBsgDzmcTi1sAsmqORzt1FRhYW\nZrmdnvMCOaqvVcRlepUmgDviWFBl4IeHMbV0fr76XjK6qWay/hIeClxNYIoWKJbU\nrsXa8t0aVQKBgQCiElrC2yGj89UMDtSFdzKVJUvJ7CV5UljMPmM4OCtQU5w4Tlx5\nhDKJcbgLElrmZWbNhiscR8o3D0n9O3d8qBFeyEygeWYieViYrYhDxCgUN0pIIyx3\nOXw2g7P9MSXXRkjLCXuo1um8k9YRgY/5v2R1yfmGP8JtOd9Vk+qzuNc/aQKBgH/f\njQPxf511qAcBMoNGMGtu2BrsNoRVE3xHcwi8dAhQrqgMjzUa3X2m7mw0ulDVnY2W\nC3jdzFXhf77LULXV3tXxz2cAHuWo0cGQHlPCZ+f1pvGSsyfYVApRNQ3eLUz0nSzN\nLqrYlV+qjClgts6ZsDKIvdPHCh7c07cDNzVX62AxAoGAa29zGXTJTfG4Tbm3g/bz\nUR1c94Jil4467Wj5nesf7VF0Hk5nWzUhi1ZrauX9q3KFVV7pn56JEQwSc7lrwne1\n1rmA96fQqZI037BYIXmhzMasSqqUIUxGKPbAYLsaCk/mq4AGCDb/yQh3JW7YOIsn\nzsiM4VVO0fdeqCQN8ZKTWL8=\n-----END PRIVATE KEY-----\n";
            //var clientEmail = "firebase-adminsdk-fbsvc@ahlanfeekum-de666.iam.gserviceaccount.com";
            //var clientId = "110527382662515187673";
            //var authUri = "https://accounts.google.com/o/oauth2/auth";
            //var tokenUri = "https://oauth2.googleapis.com/token";
            //var authProviderX509CertUrl = "https://www.googleapis.com/oauth2/v1/certs";
            //var clientX509CertUrl = "https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-fbsvc%40ahlanfeekum-de666.iam.gserviceaccount.com";
            //var universeDomain = "googleapis.com";

            //// Create JSON string from variables
            //var firebaseJson = $@"{{
            //    ""type"": ""{firebaseType}"",
            //    ""project_id"": ""{projectId}"",
            //    ""private_key_id"": ""{privateKeyId}"",
            //    ""private_key"": ""{privateKey}"",
            //    ""client_email"": ""{clientEmail}"",
            //    ""client_id"": ""{clientId}"",
            //    ""auth_uri"": ""{authUri}"",
            //    ""token_uri"": ""{tokenUri}"",
            //    ""auth_provider_x509_cert_url"": ""{authProviderX509CertUrl}"",
            //    ""client_x509_cert_url"": ""{clientX509CertUrl}"",
            //    ""universe_domain"": ""{universeDomain}""
            //}}";

            //FirebaseApp.Create(new AppOptions()
            //{
            //    Credential = GoogleCredential.FromJson(firebaseJson),
            //});


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
