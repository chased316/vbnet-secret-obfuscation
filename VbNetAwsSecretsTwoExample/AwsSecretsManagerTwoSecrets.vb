Imports System.Threading.Tasks
Imports Amazon
Imports Amazon.SecretsManager
Imports Amazon.SecretsManager.Model

''' <summary>
''' Minimal helper: fetch two named secrets from AWS Secrets Manager (same region/credentials).
''' </summary>
Public NotInheritable Class AwsSecretsManagerTwoSecrets

    Public Shared Async Function GetSecretStringAsync(
        secretId As String,
        regionEndpoint As RegionEndpoint
    ) As Task(Of String)
        If String.IsNullOrWhiteSpace(secretId) Then Throw New ArgumentException("secretId is required.", NameOf(secretId))

        Using client As New AmazonSecretsManagerClient(regionEndpoint)
            Dim request As New GetSecretValueRequest With {.SecretId = secretId}
            Dim response = Await client.GetSecretValueAsync(request).ConfigureAwait(False)
            If response.SecretString IsNot Nothing Then
                Return response.SecretString
            End If
            If response.SecretBinary IsNot Nothing Then
                Return Convert.ToBase64String(response.SecretBinary.ToArray())
            End If
            Throw New InvalidOperationException($"Secret '{secretId}' has no string or binary payload.")
        End Using
    End Function

    Public Shared Function ResolveRegion() As RegionEndpoint
        Dim fromEnv = Environment.GetEnvironmentVariable("AWS_REGION")
        If String.IsNullOrWhiteSpace(fromEnv) Then
            fromEnv = Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION")
        End If
        If String.IsNullOrWhiteSpace(fromEnv) Then
            Throw New InvalidOperationException("Set AWS_REGION or AWS_DEFAULT_REGION before running this example.")
        End If
        Return RegionEndpoint.GetBySystemName(fromEnv)
    End Function

End Class
