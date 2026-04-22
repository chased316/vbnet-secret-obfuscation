Imports Azure.Identity
Imports Azure.Security.KeyVault.Secrets

''' <summary>
''' Azure equivalent of AWS Secrets Manager for runtime secret resolution: Azure Key Vault.
''' The secret value is never compiled in; it is fetched using Azure AD credentials.
''' </summary>
''' <remarks>
''' Set KEYVAULT_URI to the vault URL, e.g. https://myvault.vault.azure.net/
''' Set SECRET_NAME to the secret name (not the version unless you pass a version explicitly).
''' Authentication: DefaultAzureCredential (managed identity on Azure, Azure CLI locally, etc.).
''' </remarks>
Public NotInheritable Class AzureKeyVaultObfuscation

    Public Shared Async Function GetSecretValueAsync(
        vaultUri As String,
        secretName As String,
        Optional secretVersion As String = Nothing
    ) As Task(Of String)
        If String.IsNullOrWhiteSpace(vaultUri) Then Throw New ArgumentException("vaultUri is required.", NameOf(vaultUri))
        If String.IsNullOrWhiteSpace(secretName) Then Throw New ArgumentException("secretName is required.", NameOf(secretName))

        Dim credential As New DefaultAzureCredential()
        Dim client As New SecretClient(New Uri(vaultUri.Trim()), credential)

        Dim response = If(
            String.IsNullOrWhiteSpace(secretVersion),
            Await client.GetSecretAsync(secretName).ConfigureAwait(False),
            Await client.GetSecretAsync(secretName, secretVersion).ConfigureAwait(False))

        Return response.Value.Value
    End Function

End Class
