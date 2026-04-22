Imports System.Configuration
Imports System.IO
Imports Amazon
Imports Amazon.Runtime
Imports Amazon.SecretsManager
Imports Amazon.SecretsManager.Model

Module Program
    Sub Main()
        Dim map As New ExeConfigurationFileMap With {.ExeConfigFilename = ResolveRealConfigPath()}
        Dim cfg = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None)
        Dim settings = cfg.AppSettings.Settings

        Dim accessKey = settings("AWSAccessKey")?.Value
        Dim secretKey = settings("AWSSecretKey")?.Value
        Dim regionName = settings("AWSRegion")?.Value

        If String.IsNullOrWhiteSpace(accessKey) OrElse String.IsNullOrWhiteSpace(secretKey) OrElse String.IsNullOrWhiteSpace(regionName) Then
            Throw New InvalidOperationException("real.config must define AWSAccessKey, AWSSecretKey, and AWSRegion (copy App.config.example to real.config).")
        End If

        Dim creds As New BasicAWSCredentials(accessKey, secretKey)

        Using c As New AmazonSecretsManagerClient(creds, RegionEndpoint.GetBySystemName(regionName))
            Console.WriteLine(c.GetSecretValue(New GetSecretValueRequest With {.SecretId = "secret1"}).SecretString)
            Console.WriteLine(c.GetSecretValue(New GetSecretValueRequest With {.SecretId = "secret2"}).SecretString)
        End Using
    End Sub

    Private Function ResolveRealConfigPath() As String
        Dim baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
        Dim realConfigPath = Path.Combine(baseDir, "real.config")
        If Not File.Exists(realConfigPath) Then
            Throw New FileNotFoundException("Missing real.config next to the executable. Copy App.config.example to real.config and set your keys.", realConfigPath)
        End If
        Return realConfigPath
    End Function
End Module
