using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Radikool6.Classes
{
    public class BasicAuthenticationMiddleware
    {
        private readonly string _id = Environment.GetEnvironmentVariable("radikool_id");
        private readonly string _password = Environment.GetEnvironmentVariable("radikool_password");

        readonly RequestDelegate _next;

        public BasicAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Basic 認証のヘッダー
            // Authorization: Basic <userName:password を Base64 エンコードした文字列>
            // からユーザー名とパスワードを取り出してチェックする
            string header = context.Request.Headers["Authorization"];
            if (header != null && header.StartsWith("Basic"))
            {
                var encodedCredentials = header.Substring("Basic".Length).Trim();
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                var separatorIndex = credentials.IndexOf(':');
                var userName = credentials.Substring(0, separatorIndex);
                var password = credentials.Substring(separatorIndex + 1);

                if (userName == _id && password == _password)
                {
                    // コンテキストにユーザー情報をセットする
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, userName),
                        new Claim(ClaimTypes.Role, "User")
                    };
                    var identity = new ClaimsIdentity(claims, "Basic");
                    context.User = new ClaimsPrincipal(identity);

                    await _next(context);
                    return;
                }
            }

            // ブラウザの認証ダイアログを出すには、レスポンスヘッダーに
            // WWW-Authenticate: Baic が必要
            context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.StatusCode = 401;
        }
    }
}