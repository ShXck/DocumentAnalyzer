﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AuthLibrary.Configuration;
using AuthLibrary.Authorization;
using AuthLibrary.Token;

namespace AuthLibrary.Factory
{
    public class AuthServiceFactory : IAuthServiceFactory
    {
        // The configuration is obtained from the AuthServiceConfig
        private string secretKey;
        private string issuerToken;
        private int expirationTime;

        private IAuthorizationService _Authorization = null;
        private IAuthenticationService _Authentication = null;

        public AuthServiceFactory()
        {
            // The configuration is obtained from the AuthServiceConfig
            secretKey = AuthServiceConfig.Config.SecretKey;
            issuerToken = AuthServiceConfig.Config.IssuerToken;
            expirationTime = AuthServiceConfig.Config.ExpirationTime;
        }

        /// <summary>
        /// Method to obtain an Authorization Service
        /// </summary>
        /// <returns>Authorization Service</returns>
        public IAuthorizationService Authorization
        {
            get
            {
                return _Authorization ??= buildAuthorizationService();
            }
        }

        /// <summary>
        /// Method to build an IAuthorizationService
        /// </summary>
        /// <returns>IAuthorizationService</returns>
        private IAuthorizationService buildAuthorizationService()
        {
            // Creation of the validator
            ITokenValidator tokenValidator = new TokenValidator(secretKey, issuerToken);

            // Creation of the AuthorizationService
            return new AuthorizationService(tokenValidator);
        }

        /// <summary>
        /// Method to obtain an Authentication Service
        /// </summary>
        /// <returns>Authentication Service</returns>
        public IAuthenticationService Authentication
        {
            get
            {
                return _Authentication ??= buildAuthenticationService();
            }
        }

        /// <summary>
        /// Method to build an IAuthenticationService
        /// </summary>
        /// <returns>IAuthenticationService</returns>
        private IAuthenticationService buildAuthenticationService()
        {
            // Creation of the token generator
            ITokenGenerator tokenGenerator = new TokenGenerator(secretKey, issuerToken, expirationTime);

            // Creation of the Authentication Service
            return new AuthenticationService(tokenGenerator);
        }
    }
}
