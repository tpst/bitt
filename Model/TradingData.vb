Public Class TradingData
    Public Sub New()
    End Sub

    Public Property [Date] As Date
    Public Property Price As Double
    Public Property Open As Double
    Public Property Close As Double
    Public Property High As Double
    Public Property Low As Double
    Public Property Volume As Double
End Class

Public Class TransactionData
    Public Sub New()
    End Sub

    Public Property Ask As Double
    Public Property Bid As Double
    Public Property Volume As Double
    Public Property Last As Double
    Public Property TransactionType As String
End Class

