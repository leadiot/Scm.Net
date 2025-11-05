using Com.Scm.Otp;
using Com.Scm.Otp.Hotp;
using Com.Scm.Otp.Totp;
using Com.Scm.Utils;
using static System.Net.WebRequestMethods;

public class Program
{
    public static void Main1(string[] args)
    {
        Console.WriteLine("=== HOTP实现示例 ===\n");

        HotpAuth hotp = new HotpAuth();

        // 1. 验证RFC 4226测试向量
        Console.WriteLine("正在验证RFC 4226测试向量...");
        bool rfcTestPassed = hotp.VerifyRfcTestVectors();
        Console.WriteLine();

        // 2. 生成随机密钥
        var secretKey = HotpAuth.GenerateRandomKey();
        string secret = TextUtils.Base32Encode(secretKey);
        Console.WriteLine($"生成的随机密钥: {secret}");
        Console.WriteLine($"密钥长度: {secret.Length} 字符\n");

        // 3. 创建HOTP实例
        Console.WriteLine($"使用参数:");
        Console.WriteLine($"  密码长度: {hotp.CodeLength} 位");
        Console.WriteLine($"  哈希算法: {hotp.HashAlgorithm}");
        Console.WriteLine($"  重新同步窗口: {hotp.ResyncWindow}\n");

        // 4. 生成多个HOTP密码示例
        long counter = 0;
        Console.WriteLine("生成HOTP密码序列:");
        for (int i = 0; i < 5; i++)
        {
            string code = hotp.GenerateCode(secretKey, counter);
            Console.WriteLine($"  计数器 {counter}: {code}");
            counter++;
        }
        Console.WriteLine();

        // 5. 验证密码示例
        hotp.ChangeCounter();
        string codeToVerify = hotp.GenerateCode(secretKey);

        Console.WriteLine($"验证测试:");
        Console.WriteLine($"  待验证密码: {codeToVerify}");
        Console.WriteLine($"  当前计数器: {hotp.GetCounter()}");

        bool isValid = hotp.VerifyCode(secretKey, codeToVerify);

        Console.WriteLine($"  验证结果: {(isValid ? "成功" : "失败")}");
        if (isValid)
        {
            Console.WriteLine($"  新计数器值: {hotp.GetCounter()}");
        }
        Console.WriteLine();

        // 6. 测试重新同步功能
        //hotp.ChangeCounter();
        //long expectedCounter = currentCounter + 1; // 模拟客户端超前1个计数
        //string resyncCode = hotp.GenerateCode(secretKey, expectedCounter);

        //Console.WriteLine("重新同步测试:");
        //Console.WriteLine($"  待验证密码: {resyncCode}");
        //Console.WriteLine($"  当前计数器: {currentCounter}");
        //Console.WriteLine($"  实际生成密码时的计数器: {expectedCounter}");

        //bool resyncSuccess = hotp.VerifyCode(secretKey, resyncCode);

        //Console.WriteLine($"  重新同步结果: {(resyncSuccess ? "成功" : "失败")}");
        //if (resyncSuccess)
        //{
        //    Console.WriteLine($"  同步后的计数器值: {hotp.GetCounter()}");
        //}
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("=== TOTP实现示例 ===\n");

        TotpAuth totp = new TotpAuth();

        // 1. 验证RFC 4226测试向量
        Console.WriteLine("正在验证RFC 6238测试向量...");
        bool rfcTestPassed = totp.VerifyRfcTestVectors();
        Console.WriteLine();

        // 1. 生成随机密钥
        string secretKey = TextUtils.Base32Encode(TotpAuth.GenerateRandomKey());
        Console.WriteLine($"生成的随机密钥: {secretKey}");
        Console.WriteLine($"密钥长度: {secretKey.Length} 字符\n");

        // 2. 创建TOTP实例
        Console.WriteLine($"使用参数:");
        Console.WriteLine($"  时间步长: {totp.TimeStep} 秒");
        Console.WriteLine($"  密码长度: {totp.CodeLength} 位");
        Console.WriteLine($"  哈希算法: {totp.HashAlgorithm}");
        Console.WriteLine($"  验证窗口: ±{totp.ValidationWindow} 个窗口\n");

        // 3. 生成当前TOTP密码
        string currentCode = totp.GenerateCode(secretKey);
        DateTime expirationTime = totp.GetExpirationTime(DateTime.UtcNow);

        Console.WriteLine($"当前TOTP密码: {currentCode}");
        Console.WriteLine($"密码过期时间: {expirationTime:yyyy-MM-dd HH:mm:ss UTC}");
        Console.WriteLine($"剩余时间: {(expirationTime - DateTime.UtcNow).Seconds} 秒\n");

        // 4. 验证密码
        bool isValid = totp.VerifyCode(secretKey, currentCode);
        Console.WriteLine($"验证当前密码: {(isValid ? "成功" : "失败")}");

        // 5. 验证错误密码
        bool isInvalid = totp.VerifyCode(secretKey, "123456");
        Console.WriteLine($"验证错误密码: {(isInvalid ? "成功" : "失败")}\n");

        // 6. 使用不同参数创建TOTP实例
        TotpAuth customTotp = new TotpAuth(60, 8, OtpHashAlgorithm.SHA256);
        customTotp.ValidationWindow = 2; // 验证前后2个窗口

        string customCode = customTotp.GenerateCode(secretKey);
        Console.WriteLine("自定义参数示例:");
        Console.WriteLine($"  时间步长: 60秒, 密码长度: 8位, 算法: SHA256");
        Console.WriteLine($"  当前密码: {customCode}");
        Console.WriteLine($"  验证窗口: ±2个窗口\n");

        // 7. 生成URL用于二维码（可导入到Google Authenticator等应用）
        string account = "user@example.com";
        string issuer = "MyApp";
        string totpUrl = GenerateTotpUrl(secretKey, account, issuer);
        Console.WriteLine("TOTP二维码URL (可导入到认证应用):");
        Console.WriteLine(totpUrl);
    }

    /// <summary>
    /// 生成TOTP URL（用于生成二维码）
    /// </summary>
    public static string GenerateTotpUrl(string secretKey, string account, string issuer)
    {
        string encodedAccount = Uri.EscapeDataString(account);
        string encodedIssuer = Uri.EscapeDataString(issuer);
        string encodedSecret = Uri.EscapeDataString(secretKey);

        return $"otpauth://totp/{encodedIssuer}:{encodedAccount}?secret={encodedSecret}&issuer={encodedIssuer}&algorithm=SHA1&digits=6&period=30";
    }
}
