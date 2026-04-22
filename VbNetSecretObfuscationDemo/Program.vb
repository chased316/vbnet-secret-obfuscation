Module Program

    Private Const DemoPlainSecret As String = "demo-local-secret-rotate-me"

    Sub Main(args As String())
        Console.WriteLine("VB.NET secret resolution demo (local DPAPI file, AWS, Azure).")
        Console.WriteLine()

        If args.Length > 0 Then
            Select Case args(0).ToLowerInvariant()
                Case "local-write"
                    LocalFileSecretObfuscation.WriteProtectedSecret(DemoPlainSecret)
                    Console.WriteLine($"Wrote DPAPI-protected payload to:{Environment.NewLine}{LocalFileSecretObfuscation.GetDefaultStorePath()}")
                    Return
                Case "local-read"
                    Console.WriteLine(LocalFileSecretObfuscation.ReadProtectedSecret())
                    Return
                Case "aws"
                    Dim secretId = RequireEnv("AWS_SECRET_ID")
                    Dim region = Environment.GetEnvironmentVariable("AWS_REGION")
                    Dim value = AwsSecretsManagerObfuscation.GetSecretStringAsync(secretId, region).GetAwaiter().GetResult()
                    Console.WriteLine(value)
                    Return
                Case "azure"
                    Dim vault = RequireEnv("KEYVAULT_URI")
                    Dim name = RequireEnv("SECRET_NAME")
                    Dim version = Environment.GetEnvironmentVariable("SECRET_VERSION")
                    Dim value = AzureKeyVaultObfuscation.GetSecretValueAsync(vault, name, version).GetAwaiter().GetResult()
                    Console.WriteLine(value)
                    Return
                Case "chase-example"
                    Dim chaseSecret = AwsSecretsManagerObfuscation.GetChaseExampleSecretAsync().GetAwaiter().GetResult()
                    Console.WriteLine(chaseSecret)
                    Return
            End Select
        End If

        Console.WriteLine("Usage:")
        Console.WriteLine("  VbNetSecretObfuscationDemo local-write")
        Console.WriteLine("  VbNetSecretObfuscationDemo local-read")
        Console.WriteLine("  VbNetSecretObfuscationDemo aws        (needs AWS_SECRET_ID, AWS_REGION, AWS creds)")
        Console.WriteLine("  VbNetSecretObfuscationDemo azure      (needs KEYVAULT_URI, SECRET_NAME, Azure auth)")
        Console.WriteLine("  VbNetSecretObfuscationDemo chase-example  (test/chaseexample in us-east-2; AWS creds)")
    End Sub

    Private Function RequireEnv(name As String) As String
        Dim value = Environment.GetEnvironmentVariable(name)
        If String.IsNullOrWhiteSpace(value) Then
            Throw New InvalidOperationException($"Environment variable '{name}' is not set.")
        End If
        Return value
    End Function

End Module
