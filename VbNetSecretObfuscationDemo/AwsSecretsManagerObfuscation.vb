Imports Amazon
Imports Amazon.SecretsManager
Imports Amazon.SecretsManager.Model

''' <summary>
''' Retrieves a secret value from AWS Secrets Manager. Naming uses "Obfuscation" in the sense
''' that the application never embeds the secret; it resolves it at runtime from the control plane.
''' </summary>
''' <remarks>
''' Typical environment variables:
''' AWS_REGION (or AWS_DEFAULT_REGION), AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY
''' Optional: AWS_SESSION_TOKEN for temporary credentials.
''' </remarks>
Public NotInheritable Class AwsSecretsManagerObfuscation

    ''' <summary>
    ''' VB port of the AWS .NET getting-started snippet for a fixed secret in us-east-2.
    ''' </summary>
    ''' <seealso href="https://aws.amazon.com/developer/language/net/getting-started" />
    ''' <seealso href="https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html" />
    Public Shared Async Function GetChaseExampleSecretAsync() As Task(Of String)
        Const secretName As String = "test/chaseexample"
        Const region As String = "us-east-2"

        Using client As IAmazonSecretsManager = New AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region))
            Dim request As New GetSecretValueRequest With {
                .SecretId = secretName,
                .VersionStage = "AWSCURRENT"
            }

            ' Exceptions: https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
            Dim response = Await client.GetSecretValueAsync(request).ConfigureAwait(False)
            Return response.SecretString
        End Using
    End Function

    Public Shared Async Function GetSecretStringAsync(
        secretId As String,
        Optional regionName As String = Nothing
    ) As Task(Of String)
        If String.IsNullOrWhiteSpace(secretId) Then Throw New ArgumentException("secretId is required.", NameOf(secretId))

        Dim region = If(
            String.IsNullOrWhiteSpace(regionName),
            FallbackRegionFromEnvironment(),
            RegionEndpoint.GetBySystemName(regionName))

        Using client As New AmazonSecretsManagerClient(region)
            Dim request As New GetSecretValueRequest With {.SecretId = secretId}
            Dim response = Await client.GetSecretValueAsync(request).ConfigureAwait(False)
            Return response.SecretString
        End Using
    End Function

    Private Shared Function FallbackRegionFromEnvironment() As RegionEndpoint
        Dim fromEnv = Environment.GetEnvironmentVariable("AWS_REGION")
        If String.IsNullOrWhiteSpace(fromEnv) Then
            fromEnv = Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION")
        End If
        If String.IsNullOrWhiteSpace(fromEnv) Then
            Throw New InvalidOperationException("Set AWS_REGION (or pass regionName) before calling AWS Secrets Manager.")
        End If
        Return RegionEndpoint.GetBySystemName(fromEnv)
    End Function

End Class
