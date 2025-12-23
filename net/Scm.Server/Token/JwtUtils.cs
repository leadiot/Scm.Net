using Com.Scm.Config;
using Com.Scm.Enums;
using Com.Scm.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Com.Scm.Token.Utils;

public class JwtUtils
{
    public static string IssueJwt(ScmToken token)
    {
        var jwtModel = AppUtils.GetConfig<JwtConfig>(JwtConfig.Name);

        var claims = new List<Claim>();
        //每次登陆动态刷新
        //JwtConst.ValidAudience = token.Id + DateTime.Now.ToString(CultureInfo.InvariantCulture);
        claims.AddRange(new[] {
            new Claim (nameof (ScmToken.id), token.id.ToString()),
            new Claim (nameof (ScmToken.user_id), token.user_id.ToString()),
            new Claim (nameof (ScmToken.user_codes), token.user_codes??""),
            new Claim (nameof (ScmToken.user_name), token.user_name??""),
            new Claim (nameof (ScmToken.time), token.time.ToString()),
            new Claim (nameof (ScmToken.data), ((int)token.data).ToString()),
            //new Claim (ClaimTypes.Role, token.Role??""),
            //new Claim (nameof (ScmToken.RoleArray), token.RoleArray),
        });
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtModel.Security));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(
            issuer: jwtModel.Issuer,
            audience: jwtModel.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtModel.Expires),
            signingCredentials: cred);
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="jwtStr"></param>
    /// <returns></returns>
    public static ScmToken SerializeJwt(string jwtStr)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(jwtStr);
        object id;
        object userId;
        object userCodes;
        object userName;
        object time;
        object data;
        //object role;
        //object roleArray;

        try
        {
            jwtToken.Payload.TryGetValue(nameof(ScmToken.id), out id);
            jwtToken.Payload.TryGetValue(nameof(ScmToken.user_id), out userId);
            jwtToken.Payload.TryGetValue(nameof(ScmToken.user_codes), out userCodes);
            jwtToken.Payload.TryGetValue(nameof(ScmToken.user_name), out userName);
            jwtToken.Payload.TryGetValue(nameof(ScmToken.time), out time);
            jwtToken.Payload.TryGetValue(nameof(ScmToken.data), out data);
            //jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            //jwtToken.Payload.TryGetValue("RoleArray", out roleArray);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return new ScmToken()
        {
            id = id?.ToString(),
            user_id = Convert.ToInt64(userId),
            user_codes = userCodes?.ToString(),
            user_name = userName?.ToString(),
            time = Convert.ToInt64(time),
            data = (ScmUserDataEnum)Convert.ToInt32(data),
            //RoleArray = roleArray?.ToString(),
            //Role = role?.ToString()
        };
    }
}