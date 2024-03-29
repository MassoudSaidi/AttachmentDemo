﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Owin;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Jwt;
using AttachmentDemoWeb.App_Start;

namespace AttachmentDemoWeb
{
	public partial class Startup
	{
	public void ConfigureAuth(IAppBuilder app)
		{
		
			TokenValidationParameters tvps = new TokenValidationParameters
            {
				ValidAudience = ConfigurationManager.AppSettings["ida:Audience"],
				
                // Microsoft Accounts have an issuer GUID that is different from any organizational tenant GUID,
                // so to support both kinds of accounts, we do not validate the issuer.
                ValidateIssuer = false,
				SaveSigninToken = true
			};

            string[] endAuthoritySegments = { "oauth2/v2.0" };
            string[] parsedAuthority = ConfigurationManager.AppSettings["ida:Authority"].Split(endAuthoritySegments, System.StringSplitOptions.None);
            string wellKnownURL = parsedAuthority[0] + "v2.0/.well-known/openid-configuration";
			
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
			{
				AccessTokenFormat = new JwtFormat(tvps, new OpenIdConnectCachingSecurityTokenProvider(wellKnownURL))
			});
		}
	}

}