using Microsoft.IdentityModel.Tokens;
using Pepp.Web.Apps.Bingo.Infrastructure.Caches;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using JWTSecretTypes =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants.TokenSecrets.Types.JWT;

namespace Pepp.Web.Apps.Bingo.Managers
{
    /// <summary>
    /// Interface for working with JSON Web Tokens
    /// </summary>
    public interface ITokenManager
    {
        /// <summary>
        /// Generate a JWT for a specific user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="tokenTTL"></param>
        /// <returns></returns>
        string GenerateJWTForUser(int userID, DateTime tokenTTL);
        /// <summary>
        /// Validates that a JWT is valid
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        bool TokenIsValid(string jwtToken);
    }

    public class TokenManager : ITokenManager
    {
        private readonly ITokenCache _cache;

        public TokenManager(ITokenCache cache) => _cache = cache;

        public string GenerateJWTForUser(int userID, DateTime tokenTTL)
        {
            //Setup Header
            TokenHeader header = new();
            string jwtHeader = JsonSerializer.Serialize(header);
            byte[] headerBytes = Encoding.UTF8.GetBytes(jwtHeader);
            string encodedHeader = Base64UrlEncoder.Encode(headerBytes);

            //Setup data body
            TokenBody body = new(userID, tokenTTL);
            string jwtBody = JsonSerializer.Serialize(body);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(jwtBody);
            string encodedBody = Base64UrlEncoder.Encode(bodyBytes);

            string jwt = $"{encodedHeader}.{encodedBody}";

            string jwtSignature = GetTokenSignature(jwt);
            string encodedSignature = Base64UrlEncoder.Encode(jwtSignature);

            string signedJWT = $"{jwt}.{encodedSignature}";

            return Base64UrlEncoder.Encode(signedJWT);
        }

        public bool TokenIsValid(string jwtToken)
        {
            //Token anatomy is "encodedHeader.encodedBody.encodedSignature"
            string[] tokenParts = jwtToken.Split('.');
            string encodedToken = $"{tokenParts[0]}.{tokenParts[1]}";
            string encodedSignature = tokenParts[2];

            string decodedSignature = Base64UrlEncoder.Decode(encodedSignature);

            string recreatedSignature = GetTokenSignature(encodedToken);

            bool validToken = recreatedSignature.Equals(decodedSignature);

            return validToken;
        }

        public static bool TokenIsExpired(string jwtToken)
        {
            //Token anatomy is "encodedHeader.encodedBody.encodedSignature"
            string[] tokenParts = jwtToken.Split('.');
            string encodedTokenBody = tokenParts[1];

            string decodedTokenBody = Base64UrlEncoder.Decode(encodedTokenBody);
            TokenBody jwtTokenBody = JsonSerializer.Deserialize<TokenBody>(decodedTokenBody);

            bool isExpired = DateTime.UtcNow > jwtTokenBody.ExpirationDateTime;
            return isExpired;
        }

        private string GetTokenSignature(string token)
        {
            string signingSecret = _cache.GetTokenSecret(JWTSecretTypes.SigningSecret);
            string jwtSignature = string.Empty;
            foreach (string value in new[] { token, signingSecret })
            {
                jwtSignature += value;
                string hash = GetHash(value);
                jwtSignature = hash;
            }
            return jwtSignature;
        }

        private string GetHash(string value)
        {
            string hashingKey = _cache.GetTokenSecret(JWTSecretTypes.SigningSecret);
            byte[] hashingKeyBytes = Encoding.UTF8.GetBytes(hashingKey);
            HMACSHA256 encryptor = new(hashingKeyBytes);
            byte[] unencryptedBytes = Encoding.UTF8.GetBytes(value);
            byte[] encryptedBytes = encryptor.ComputeHash(unencryptedBytes);
            string hash = string.Empty;
            for (int i = 0; i < encryptedBytes.Length; i++)
                hash += encryptedBytes[i].ToString("X2");
            return hash;
        }

        #region PRIVATE CLASS RESOURCES

        private class TokenHeader
        {
            public string Algorithm { get; } = "SHA256";
            public string Type { get; } = "JWT";
        }

        private class TokenBody
        {
            public TokenBody(int userID, DateTime expirationDateTime) =>
                (UserID, ExpirationDateTime) = (userID, expirationDateTime);

            public int UserID { get; set; }
            public DateTime ExpirationDateTime { get; set; }
        }

        #endregion
    }
}
