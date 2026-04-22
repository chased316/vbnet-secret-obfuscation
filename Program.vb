Imports Amazon.SecretsManager
Imports Amazon.SecretsManager.Model

Module Program

    Sub Main()
        Using client As New AmazonSecretsManagerClient()
            Print(client, "secret1")
            Print(client, "secret2")
        End Using
    End Sub

    Private Sub Print(client As IAmazonSecretsManager, secretId As String)
        Dim r = client.GetSecretValue(New GetSecretValueRequest With {.SecretId = secretId, .VersionStage = "AWSCURRENT"})
        Console.WriteLine(secretId & ": " & r.SecretString)
    End Sub

End Module
