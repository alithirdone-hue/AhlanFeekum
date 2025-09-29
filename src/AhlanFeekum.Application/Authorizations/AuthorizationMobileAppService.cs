using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Json;
using Volo.Abp.Uow;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Microsoft.Extensions.Logging;
using AhlanFeekum.Localization;
using System.Net.Http;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using static Volo.Abp.UI.Navigation.DefaultMenuNames.Application;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Linq;
using AhlanFeekum.MobileResponses;
using Newtonsoft.Json;
using System.Text;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System.Collections.Generic;

namespace AhlanFeekum.Authorizations
{
    [RemoteService(false)]
    [AllowAnonymous]

    public class AuthorizationMobileAppService : ApplicationService, IAuthorizationMobileAppService
    {
        private readonly UserManager _userManager;
        private readonly ILogger<AuthorizationMobileAppService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IHttpClientFactory _httpClientFactory;


        public AuthorizationMobileAppService(UserManager userManager,
            IHttpClientFactory httpClientFactory,
            IJsonSerializer jsonSerializer,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor,
            ILogger<AuthorizationMobileAppService> logger,
            IConfiguration configuration,
            IIdentityUserRepository identityUserRepository)
        {
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
            LocalizationResource = typeof(AhlanFeekumResource);
            _identityUserRepository = identityUserRepository;
            _httpClientFactory = httpClientFactory;
        }


        [UnitOfWork(isTransactional: false)]

        public async Task<bool> AuthorizeAsync(string uId, string userName, string email, string password)
        {




            var user = await _userManager.FindUserByClientId(uId);
            if(user != null)
            {
    

            }
         

            if (user == null)
            {
                IdentityUser identityUser = await _userManager.CreateUserAsync(
                                            id: Guid.Parse(uId),
                                            userName: userName,
                                            email: email,
                                            password: password,
                                            name: userName,
                                            surname: userName
                                            );
                if(identityUser != null)
                    return true;
                return false;


            }
            return false;
     

        }

        public async Task<MobileResponseDto> GetAbpLogInAsync(TokenRequest request)
        {
            //if(request.UserName == "admin" && request.Password == "As12345678!")
            //{
            //    var responseResult = new
            //    {
            //        Token = new
            //        {
            //            access_token = "eyJhbGciOiJ11SUzI1NiIsImtpZCI6IjYzMjRFRDVFMEZGM0MwOEM3NUFFRUU0QkQ5MDUyNzdDNDMzQjNFMzIiLCJ4NXQiOiJZeVR0WGdfendJeDFydTVMMlFVbmZFTTdQakkiLCJ0eXAiOiJhdCtqd3QifQ.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM1Mi8iLCJleHAiOjE3NDYxNDg3NzQsImlhdCI6MTc0NjE0NTE3NCwiYXVkIjoiSW1hYXIiLCJzY29wZSI6IkltYWFyIiwianRpIjoiMTMwYzU3OGItZTFjZS00YjlhLThjZDAtMmRmZTJhZTY3YWRiIiwic3ViIjoiNjc0MTZkMjYtYmJiYi0wMjViLTkxY2ItM2ExOWEwODZjNmRkIiwicHJlZmVycmVkX3VzZXJuYW1lIjoiYWRtaW4iLCJlbWFpbCI6ImFkbWluQGFicC5pbyIsInJvbGUiOiJhZG1pbiIsImdpdmVuX25hbWUiOiJhZG1pbiIsInBob25lX251bWJlcl92ZXJpZmllZCI6IkZhbHNlIiwiZW1haWxfdmVyaWZpZWQiOiJGYWxzZSIsInVuaXF1ZV9uYW1lIjoiYWRtaW4iLCJvaV9wcnN0IjoiSW1hYXJfU3dhZ2dlciIsImNsaWVudF9pZCI6IkltYWFyX1N3YWdnZXIiLCJvaV90a25faWQiOiJkNGU0ODZmOS1iN2M2LWZhMTUtOGRkZC0zYTE5YTA4OWJjYWYifQ.VrXv4uUSxkXc93sf3dG78KEk9R85OYsCJWUMomdTtzBZnXp6ooswtikVzAkZu1_dVd-bGnRGYmVO2sd4FPD_7jKrMlNCs6CNgJ9Yb-UjcPYChODyZ0TOa6NKsXiLw95QzkrB0kcrl_BVDkykdZfthbK2vY25u93Tlzw0SjJ2UrztrdZoqHJ-0DMSu6nefFZO-qzlP5omf4WsZikz3FTFpM71KDXwQjQTbNMgLNKu1LGaQikNI-9bx9Yznx-EhvoMM2HHxGuyOvCP0KKj0PQK9bRDHVl0wSFA13ceevfdIqA6aJsW_-hmYGgcOstfWb9Ct3m3CQvJhw1dbYycdcU-4g",
            //            token_type = "TokenType",
            //            expires_in = "3600",
            //            refresh_token = ""
            //        }

            //    };

            //    return responseResult;
            //}

            var users = await _identityUserRepository.GetListAsync();
            var user = users.FirstOrDefault(u => u.Email == request.PhoneOrEmail || u.PhoneNumber == request.PhoneOrEmail);
            MobileResponseDto mobileResponseDto = new MobileResponseDto();
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                mobileResponseDto.Code = 401;
                mobileResponseDto.Message = "Wrong Email or password";
                mobileResponseDto.Data = null;
                return mobileResponseDto;
                // return Unauthorized();
            }
            var data = await _userManager.GetTokenAsync(user, request.Password);

            if (data.IsError)
                throw new Exception(data.Error);

            //var responseResult = new
            //{
            //    Token = new
            //    {
            //        access_token = data.AccessToken,
            //        token_type = data.TokenType,
            //        expires_in = data.ExpiresIn,
            //        refresh_token = data.RefreshToken
            //    }

            //};
            TokenResponse tokenResponse = new TokenResponse();
            tokenResponse.AccessToken = data.AccessToken;
            tokenResponse.TokenType = data.TokenType;
            tokenResponse.ExpiresIn = data.ExpiresIn.ToString();
            tokenResponse.Email = user.Email;
            tokenResponse.Phone = user.PhoneNumber;
            tokenResponse.Name = user.Name; ;
            tokenResponse.UserId = user.Id.ToString();
            Guid guestUser = Guid.Parse("3edb2ac3-49d8-5734-727e-3a1bee2e61b7");
            Guid hostUser = Guid.Parse("3f314e03-898d-92cd-8b2c-3a1bee2e46b4");
            Guid admin = Guid.Parse("e1221a17-0636-1e3a-b1d9-3a1bdea75230");
            var roles = await _identityUserRepository.GetRolesAsync(user.Id);
            if (roles.Any(r => r.Id == guestUser))
                tokenResponse.RoleId = 2;
            else
            {
                if (roles.Any(r => r.Id == hostUser))
                    tokenResponse.RoleId = 1;
                else
                {
                    if (roles.Any(r => r.Id == admin))
                        tokenResponse.RoleId = 3;

                }
            }
            // tokenResponse.refresh_token = data.RefreshToken;

            mobileResponseDto.Code = 200;
            mobileResponseDto.Message = "SUCCESS";
            mobileResponseDto.Data = tokenResponse;
            return (mobileResponseDto);

            return null;
        }

        [UnitOfWork(isTransactional: false)]
        public async Task<MobileResponseDto> GoogleAuthAsync(GoogleAuthRequest request)
        {
            try
            {
                // Validate Google ID token and get user info
                var googleUserInfo = await ValidateGoogleTokenAsync(request.IdToken);
                
                if (googleUserInfo == null)
                {
                    return new MobileResponseDto
                    {
                        Code = 401,
                        Message = "Invalid Google token",
                        Data = null
                    };
                }

                // Check if user exists by email
                var existingUser = await _userManager.FindByEmailAsync(googleUserInfo.Email);
                
                if (existingUser == null)
                {
                    // Create new user
                    var newUser = new IdentityUser(
                        id: Guid.NewGuid(),
                        userName: googleUserInfo.Email,
                        email: googleUserInfo.Email
                    )
                    {
                        Name = googleUserInfo.GivenName,
                        Surname = googleUserInfo.FamilyName
                    };

                   // var result = await _userManager.CreateAsync(newUser);
                    var result = true;
                   // if (!result.Succeeded)
                    if (true)
                    {
                       // _logger.LogError("Failed to create user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                        _logger.LogError("Failed to create user: {Errors}", "ghghjgh");
                        return new MobileResponseDto
                        {
                            Code = 500,
                            Message = "Failed to create user account",
                            Data = null
                        };
                    }

                    existingUser = newUser;
                }

                // Generate token for the user
                var tokenData = await _userManager.GetTokenAsync(existingUser, "Google");
                
                if (tokenData.IsError)
                {
                    _logger.LogError("Failed to generate token: {Error}", tokenData.Error);
                    return new MobileResponseDto
                    {
                        Code = 500,
                        Message = "Failed to generate authentication token",
                        Data = null
                    };
                }

                // Create token response
                var tokenResponse = new TokenResponse
                {
                    AccessToken = tokenData.AccessToken,
                    TokenType = tokenData.TokenType,
                    ExpiresIn = tokenData.ExpiresIn.ToString(),
                    Email = existingUser.Email,
                    Phone = existingUser.PhoneNumber,
                    Name = existingUser.Name,
                    UserId = existingUser.Id.ToString()
                };

                // Set role (default to guest user)
                Guid guestUser = Guid.Parse("3edb2ac3-49d8-5734-727e-3a1bee2e61b7");
                Guid hostUser = Guid.Parse("3f314e03-898d-92cd-8b2c-3a1bee2e46b4");
                Guid admin = Guid.Parse("e1221a17-0636-1e3a-b1d9-3a1bdea75230");
                
                var roles = await _identityUserRepository.GetRolesAsync(existingUser.Id);
                if (roles.Any(r => r.Id == guestUser))
                    tokenResponse.RoleId = 2;
                else if (roles.Any(r => r.Id == hostUser))
                    tokenResponse.RoleId = 1;
                else if (roles.Any(r => r.Id == admin))
                    tokenResponse.RoleId = 3;
                else
                    tokenResponse.RoleId = 2; // Default to guest

                return new MobileResponseDto
                {
                    Code = 200,
                    Message = "SUCCESS",
                    Data = tokenResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google authentication");
                return new MobileResponseDto
                {
                    Code = 500,
                    Message = "Internal server error during authentication",
                    Data = null
                };
            }
        }

        private async Task<GoogleUserInfo> ValidateGoogleTokenAsync(string idToken)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                
                // Call Google's tokeninfo endpoint to validate the token
                var response = await httpClient.GetAsync($"https://oauth2.googleapis.com/tokeninfo?id_token={idToken}");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Google token validation failed with status: {StatusCode}", response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var tokenInfo = JsonConvert.DeserializeObject<GoogleTokenInfo>(content);

                if (tokenInfo == null || string.IsNullOrEmpty(tokenInfo.Email))
                {
                    _logger.LogWarning("Invalid token info received from Google");
                    return null;
                }

                // Verify the token is for our application (optional but recommended)
                var clientId = _configuration["Google:ClientId"];
                if (!string.IsNullOrEmpty(clientId) && tokenInfo.Audience != clientId)
                {
                    _logger.LogWarning("Token audience mismatch. Expected: {Expected}, Got: {Actual}", clientId, tokenInfo.Audience);
                    return null;
                }

                return new GoogleUserInfo
                {
                    Id = tokenInfo.Subject,
                    Email = tokenInfo.Email,
                    Name = tokenInfo.Name,
                    GivenName = tokenInfo.GivenName,
                    FamilyName = tokenInfo.FamilyName,
                    Picture = tokenInfo.Picture,
                    EmailVerified = tokenInfo.EmailVerified == "true"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating Google token");
                return null;
            }
        }

        private class GoogleTokenInfo
        {
            [JsonProperty("sub")]
            public string Subject { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("given_name")]
            public string GivenName { get; set; }

            [JsonProperty("family_name")]
            public string FamilyName { get; set; }

            [JsonProperty("picture")]
            public string Picture { get; set; }

            [JsonProperty("email_verified")]
            public string EmailVerified { get; set; }

            [JsonProperty("aud")]
            public string Audience { get; set; }
        }

        [UnitOfWork(isTransactional: false)]
        public async Task<MobileResponseDto> FirebaseAuthAsync(FirebaseAuthRequest request)
        {
            try
            {
                // Initialize Firebase Admin SDK if not already initialized
                await InitializeFirebaseAsync();

                // Verify the Firebase ID token
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(request.IdToken);
                var uid = decodedToken.Uid;
                var claims = decodedToken.Claims;

                // Extract user information from the token
                var email = claims.GetValueOrDefault("email")?.ToString();
                var name = claims.GetValueOrDefault("name")?.ToString();
                var picture = claims.GetValueOrDefault("picture")?.ToString();
                var emailVerified = claims.GetValueOrDefault("email_verified")?.ToString() == "True";

                if (string.IsNullOrEmpty(email))
                {
                    return new MobileResponseDto
                    {
                        Code = 400,
                        Message = "Email not found in Firebase token",
                        Data = null
                    };
                }

                //// Check if user exists by email
                //var existingUser = await _userManager.FindByEmailAsync(email);

                //if (existingUser == null)
                //{
                //    // Create new user
                //    var newUser = new IdentityUser(
                //        id: Guid.NewGuid(),
                //        userName: email,
                //        email: email
                //    )
                //    {
                //        Name = name ?? email.Split('@')[0],
                //        Surname = name?.Split(' ').LastOrDefault() ?? ""
                //    };

                //    newUser.SetEmailConfirmed(emailVerified);
                //    var result = await _userManager.CreateAsync(newUser);
                //    if (!result.Succeeded)
                //    {
                //        _logger.LogError("Failed to create user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                //        return new MobileResponseDto
                //        {
                //            Code = 500,
                //            Message = "Failed to create user account",
                //            Data = null
                //        };
                //    }

                //    existingUser = newUser;
                //}

                //// Generate token for the user
                //var tokenData = await _userManager.GetTokenAsync(existingUser, "Firebase");

                //if (tokenData.IsError)
                //{
                //    _logger.LogError("Failed to generate token: {Error}", tokenData.Error);
                //    return new MobileResponseDto
                //    {
                //        Code = 500,
                //        Message = "Failed to generate authentication token",
                //        Data = null
                //    };
                //}

                // Create token response
                //var tokenResponse = new TokenResponse
                //{
                //    AccessToken = tokenData.AccessToken,
                //    TokenType = tokenData.TokenType,
                //    ExpiresIn = tokenData.ExpiresIn.ToString(),
                //    Email = existingUser.Email,
                //    Phone = existingUser.PhoneNumber,
                //    Name = existingUser.Name,
                //    UserId = existingUser.Id.ToString()
                //};

                //// Set role (default to guest user)
                //Guid guestUser = Guid.Parse("3edb2ac3-49d8-5734-727e-3a1bee2e61b7");
                //Guid hostUser = Guid.Parse("3f314e03-898d-92cd-8b2c-3a1bee2e46b4");
                //Guid admin = Guid.Parse("e1221a17-0636-1e3a-b1d9-3a1bdea75230");

                //var roles = await _identityUserRepository.GetRolesAsync(existingUser.Id);
                //if (roles.Any(r => r.Id == guestUser))
                //    tokenResponse.RoleId = 2;
                //else if (roles.Any(r => r.Id == hostUser))
                //    tokenResponse.RoleId = 1;
                //else if (roles.Any(r => r.Id == admin))
                //    tokenResponse.RoleId = 3;
                //else
                //    tokenResponse.RoleId = 2; // Default to guest

                return new MobileResponseDto
                {
                    Code = 200,
                    Message = "SUCCESS",
                    Data = new
                    {
                        Email = email,
                        Name = name,
                    }
                };
            }
            catch (FirebaseAuthException ex)
            {
                _logger.LogError(ex, "Firebase authentication error: {ErrorCode}", ex.ErrorCode);
                return new MobileResponseDto
                {
                    Code = 401,
                    Message = "Invalid Firebase token",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Firebase authentication");
                return new MobileResponseDto
                {
                    Code = 500,
                    Message = "Internal server error during authentication",
                    Data = null
                };
            }
        }

        private async Task InitializeFirebaseAsync()
        {
            try
            {
                if (FirebaseApp.DefaultInstance == null)
                {
                    var firebaseConfig = _configuration.GetSection("Firebase");
                    var credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(new
                    {
                        type = firebaseConfig["type"],
                        project_id = firebaseConfig["project_id"],
                        private_key_id = firebaseConfig["private_key_id"],
                        private_key = firebaseConfig["private_key"],
                        client_email = firebaseConfig["client_email"],
                        client_id = firebaseConfig["client_id"],
                        auth_uri = firebaseConfig["auth_uri"],
                        token_uri = firebaseConfig["token_uri"],
                        auth_provider_x509_cert_url = firebaseConfig["auth_provider_x509_cert_url"],
                        client_x509_cert_url = firebaseConfig["client_x509_cert_url"],
                        universe_domain = firebaseConfig["universe_domain"]
                    }));

                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = credential,
                        ProjectId = firebaseConfig["project_id"]
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize Firebase Admin SDK");
                throw;
            }
        }
        }
}
