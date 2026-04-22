Imports System.Threading.Tasks
Imports Amazon.SecretsManager
Imports Amazon.SecretsManager.Model

''' <summary>
''' Minimal AWS Secrets Manager sample for .NET Framework 4.8.
''' Uses the AWS SDK default configuration: the same credential chain as the AWS CLI
''' (for example the default profile in ~/.aws/credentials on your machine), plus
''' region from AWS_REGION / AWS_DEFAULT_REGION or the shared config file when set there.
''' </summary>
Module Program

    Private Const DefaultSecret1Id As String = "secret1"
    Private Const DefaultSecret2Id As String = "secret2"

    Sub Main()
        RunAsync().GetAwaiter().GetResult()
    End Sub

    Private Async Function RunAsync() As Task
        Console.WriteLine("AWS Secrets Manager (default SDK credentials + region resolution).")
        Console.WriteLine()

        Dim id1 = ResolveName("AWS_SECRET_NAME_1", DefaultSecret1Id)
        Dim id2 = ResolveName("AWS_SECRET_NAME_2", DefaultSecret2Id)

        ' Parameterless client: FallbackCredentialsFactory + region from environment / shared config.
        Using client As New AmazonSecretsManagerClient()
            Dim value1 = Await GetSecretStringAsync(client, id1).ConfigureAwait(True)
            Dim value2 = Await GetSecretStringAsync(client, id2).ConfigureAwait(True)

            Console.WriteLine(id1 & ":")
            Console.WriteLine(value1)
            Console.WriteLine()
            Console.WriteLine(id2 & ":")
            Console.WriteLine(value2)
        End Using
    End Function

    Private Async Function GetSecretStringAsync(client As IAmazonSecretsManager, secretId As String) As Task(Of String)
        Dim request As New GetSecretValueRequest With {
            .SecretId = secretId,
            .VersionStage = "AWSCURRENT"
        }
        Dim response = Await client.GetSecretValueAsync(request).ConfigureAwait(False)
        If response.SecretString IsNot Nothing Then
            Return response.SecretString
        End If
        If response.SecretBinary IsNot Nothing Then
            Return Convert.ToBase64String(response.SecretBinary.ToArray())
        End If
        Throw New InvalidOperationException($"Secret '{secretId}' has no string or binary payload.")
    End Function

    Private Function ResolveName(envVar As String, defaultName As String) As String
        Dim value = Environment.GetEnvironmentVariable(envVar)
        Return If(String.IsNullOrWhiteSpace(value), defaultName, value)
    End Function

End Module
