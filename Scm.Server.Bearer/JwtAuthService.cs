using Com.Scm.Jwt.Model;
using Com.Scm.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Com.Scm.Api.Service;

public class JwtAuthService
{
    public static string IssueJwt(JwtToken token)
    {
        var jwtModel = AppUtils.GetConfig(JwtModel.Name).Get<JwtModel>();
        var claims = new List<Claim>();
        //每次登陆动态刷新
        //JwtConst.ValidAudience = token.Id + DateTime.Now.ToString(CultureInfo.InvariantCulture);
        claims.AddRange(new[] {
            new Claim (nameof (JwtToken.id), token.id.ToString()),
            new Claim (nameof (JwtToken.unit_id), token.unit_id.ToString()),
            new Claim (nameof (JwtToken.user_name), token.user_name),
            new Claim (nameof (JwtToken.RoleArray), token.RoleArray),
            new Claim (nameof (JwtToken.time), token.time.ToString (CultureInfo.InvariantCulture)),
            new Claim (ClaimTypes.Role, token.Role)
        });
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtModel.Security));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(
            issuer: jwtModel.Issuer,
            //audience: JwtConst.ValidAudience,
            audience: jwtModel.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtModel.WebExp),
            signingCredentials: cred);
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="jwtStr"></param>
    /// <returns></returns>
    public static JwtToken SerializeJwt(string jwtStr)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(jwtStr);
        object id;
        object userId;
        object userName;
        object unitId;
        object unitName;
        object time;
        object role;
        object roleArray;

        try
        {
            jwtToken.Payload.TryGetValue("id", out id);
            jwtToken.Payload.TryGetValue("user_id", out userId);
            jwtToken.Payload.TryGetValue("user_name", out userName);
            jwtToken.Payload.TryGetValue("unit_id", out unitId);
            jwtToken.Payload.TryGetValue("unit_name", out unitName);
            jwtToken.Payload.TryGetValue("time", out time);
            jwtToken.Payload.TryGetValue("RoleArray", out roleArray);
            jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return new JwtToken()
        {
            id = Convert.ToInt64(id),
            user_id = Convert.ToInt64(userId),
            user_name = userName?.ToString(),
            unit_id = Convert.ToInt64(unitId),
            unit_name = unitName?.ToString(),
            time = Convert.ToDateTime(time),
            RoleArray = roleArray?.ToString(),
            Role = role?.ToString()
        };
    }
}