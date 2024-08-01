using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autodesk.Authentication;
using Autodesk.Authentication.Model;

namespace bucket.manager.APSModel
{
    public class Token
    {
        public string accessToken = string.Empty;
        public int expiresIn;
        public DateTime expiresAt;

        public Token(string token, int? expiresIn)
        {
            this.accessToken = token;
            this.expiresIn = expiresIn ?? 0;
            this.expiresAt = DateTime.Now.AddSeconds((double)expiresIn - 1.0);
        }
    }

    public partial class APS
    {
        private Token token;

        private async Task<Token> GetToken(List<Scopes> scopes)
        {
            var authenticationClient = new AuthenticationClient(_sdkManager);
            var auth = await authenticationClient.GetTwoLeggedTokenAsync(_clientId, _clientSecret, scopes);
            return new Token(auth.AccessToken, auth.ExpiresIn);
        }

        //public async Task<Token> GetPublicToken()
        //{
        //    if (token.accessToken == null || token.expiresAt < DateTime.UtcNow)
        //        token = await GetToken(new List<Scopes> { Scopes.ViewablesRead });
        //    return token;
        //}

        public async Task<Token> GetInternalToken()
        {
            if (token == null || token.expiresAt < DateTime.UtcNow)
                token = await GetToken(new List<Scopes> { Scopes.BucketCreate, Scopes.BucketRead, Scopes.DataRead, Scopes.DataWrite, Scopes.DataCreate });
            return token;
        }
    }
}
