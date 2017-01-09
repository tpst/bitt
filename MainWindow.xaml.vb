Imports System.Security.Cryptography
Imports System.Text
Imports Newtonsoft.Json

'https://apiv2.bitcoinaverage.com/

Class MainWindow

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Me.Content = New MarketView()

        'Main()
    End Sub


    Private Sub Main()
        ' Not sure why this hash rubbish is necessary yet. 
        Dim signature As String

        Dim publicKey As String = "ZTExZTE0NTVjZjYxNDhhNzlhMzdkYmQ2ZTZiZGI5MGQ"
        Dim secretKey As String = "NWFmZmUyYmViZWM0NGU3N2JiOTEyYzljMzdlMmNlNjM5MDhkZWYwY2IwNDM0MTMyYWFkZmEzMTljMzUxYjhmOQ"

        Dim currentUnixTimeStamp As Int32 = DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalSeconds

        Using hasher As New HMACSHA256(Encoding.UTF8.GetBytes(secretKey))

            Dim buffer As String = currentUnixTimeStamp.ToString + "." + publicKey

            Dim digest As Byte() = hasher.ComputeHash(Encoding.UTF8.GetBytes(buffer))

            ' signature = timestamp.public_key.digest_value  where digest value is a hexadecimal string
            signature = buffer + "." + (BitConverter.ToString(digest).Replace("-", ""))
        End Using

    End Sub


End Class

