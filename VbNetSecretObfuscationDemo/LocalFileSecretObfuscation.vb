Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

''' <summary>
''' Windows-oriented local secret handling: plaintext never lands on disk.
''' The on-disk payload is DPAPI-protected (machine/user bound), which is a common
''' pattern for "obfuscating" or binding secrets to an environment without a cloud vault.
''' </summary>
Public NotInheritable Class LocalFileSecretObfuscation

    Private Const PayloadMarker As Byte = &H7B

    Public Shared Function GetDefaultStorePath() As String
        Dim appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        Return Path.Combine(appData, "VbNetSecretObfuscationDemo", "local-secret.bin")
    End Function

    ''' <summary>
    ''' Encrypts UTF-8 bytes with DPAPI and writes a small binary envelope to disk.
    ''' </summary>
    Public Shared Sub WriteProtectedSecret(plainText As String, Optional filePath As String = Nothing)
        If plainText Is Nothing Then Throw New ArgumentNullException(NameOf(plainText))

        Dim storePath = If(String.IsNullOrWhiteSpace(filePath), GetDefaultStorePath(), filePath)
        Dim folderPath = Path.GetDirectoryName(storePath)
        If Not String.IsNullOrEmpty(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If

        Dim plainBytes = Encoding.UTF8.GetBytes(plainText)
        Dim scope = DataProtectionScope.CurrentUser
        Dim protectedBytes = ProtectedData.Protect(plainBytes, Nothing, scope)

        Using stream As FileStream = File.Create(storePath)
            stream.WriteByte(PayloadMarker)
            stream.Write(protectedBytes, 0, protectedBytes.Length)
        End Using
    End Sub

    ''' <summary>
    ''' Reads the envelope and decrypts with DPAPI for the same user/machine context.
    ''' </summary>
    Public Shared Function ReadProtectedSecret(Optional filePath As String = Nothing) As String
        Dim storePath = If(String.IsNullOrWhiteSpace(filePath), GetDefaultStorePath(), filePath)
        If Not File.Exists(storePath) Then
            Throw New FileNotFoundException("Protected secret file not found. Call WriteProtectedSecret first.", storePath)
        End If

        Dim allBytes = File.ReadAllBytes(storePath)
        If allBytes.Length < 2 OrElse allBytes(0) <> PayloadMarker Then
            Throw New InvalidDataException("Unexpected file format.")
        End If

        Dim cipher = New Byte(allBytes.Length - 2) {}
        Buffer.BlockCopy(allBytes, 1, cipher, 0, cipher.Length)

        Dim scope = DataProtectionScope.CurrentUser
        Dim plainBytes = ProtectedData.Unprotect(cipher, Nothing, scope)
        Return Encoding.UTF8.GetString(plainBytes)
    End Function

End Class
