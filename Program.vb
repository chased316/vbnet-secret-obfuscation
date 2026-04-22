Imports Amazon
Imports Amazon.SecretsManager
Imports Amazon.SecretsManager.Model

Module Program
    Sub Main()
        Using c As New AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName("us-east-2"))
            Console.WriteLine(c.GetSecretValue(New GetSecretValueRequest With {.SecretId = "secret1"}).SecretString)
            Console.WriteLine(c.GetSecretValue(New GetSecretValueRequest With {.SecretId = "secret2"}).SecretString)
        End Using
    End Sub
End Module
