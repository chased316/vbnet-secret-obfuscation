Imports Amazon.SecretsManager
Imports Amazon.SecretsManager.Model

Module Program
    Sub Main()
        Using c As New AmazonSecretsManagerClient()
            Console.WriteLine(c.GetSecretValue(New GetSecretValueRequest With {.SecretId = "secret1"}).SecretString)
            Console.WriteLine(c.GetSecretValue(New GetSecretValueRequest With {.SecretId = "secret2"}).SecretString)
        End Using
    End Sub
End Module
