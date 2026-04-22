Imports System.Threading.Tasks

''' <summary>
''' Standalone example: reads <c>secret1</c> and <c>secret2</c> from AWS Secrets Manager and prints them.
''' Credentials: usual AWS SDK resolution (env keys, shared credentials file, instance profile, etc.).
''' Region: AWS_REGION or AWS_DEFAULT_REGION (required).
''' Optional overrides: AWS_SECRET_NAME_1, AWS_SECRET_NAME_2 (defaults secret1 / secret2).
''' </summary>
Module Program

    Private Const DefaultSecret1Id As String = "secret1"
    Private Const DefaultSecret2Id As String = "secret2"

    Sub Main()
        RunAsync().GetAwaiter().GetResult()
    End Sub

    Private Async Function RunAsync() As Task
        Console.WriteLine("AWS Secrets Manager — fetch two secrets (default names: secret1, secret2).")
        Console.WriteLine()

        Dim region = AwsSecretsManagerTwoSecrets.ResolveRegion()
        Dim id1 = ResolveSecretName("AWS_SECRET_NAME_1", DefaultSecret1Id)
        Dim id2 = ResolveSecretName("AWS_SECRET_NAME_2", DefaultSecret2Id)

        Dim value1 = Await AwsSecretsManagerTwoSecrets.GetSecretStringAsync(id1, region).ConfigureAwait(True)
        Dim value2 = Await AwsSecretsManagerTwoSecrets.GetSecretStringAsync(id2, region).ConfigureAwait(True)

        Console.WriteLine(id1 & ":")
        Console.WriteLine(value1)
        Console.WriteLine()
        Console.WriteLine(id2 & ":")
        Console.WriteLine(value2)
    End Function

    Private Function ResolveSecretName(envVar As String, defaultName As String) As String
        Dim value = Environment.GetEnvironmentVariable(envVar)
        Return If(String.IsNullOrWhiteSpace(value), defaultName, value)
    End Function

End Module
