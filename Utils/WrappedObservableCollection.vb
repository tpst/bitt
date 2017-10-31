Imports System.Collections.ObjectModel

Public Interface IWrapper(Of T)
    ReadOnly Property Source As T
End Interface

Public Class WrappedObservableCollection(Of TWrapper As IWrapper(Of TWrapped), TWrapped)
    Inherits ObservableCollectionEx(Of TWrapper)

    Private _source As IList(Of TWrapped)

    Sub New(src As IList(Of TWrapped))
        Debug.Assert(src.Count = 0)
        _source = src
    End Sub

    Sub New(src As IList(Of TWrapped), factoryMethod As Func(Of TWrapped, TWrapper))
        MyBase.New(src.Select(Of TWrapper)(Function(x) factoryMethod(x)).ToList())
        _source = src
    End Sub

    Protected Overrides Sub ClearItems()
        MyBase.ClearItems()
        If (_source IsNot Nothing) Then
            _source.Clear()
        End If
    End Sub

    Protected Overrides Sub InsertItem(index As Integer, item As TWrapper)
        MyBase.InsertItem(index, item)
        If (_source IsNot Nothing) Then
            _source.Insert(index, item.Source)
        End If
    End Sub

    Protected Overrides Sub MoveItem(oldIndex As Integer, newIndex As Integer)
        MyBase.MoveItem(oldIndex, newIndex)
        If (_source IsNot Nothing) Then
            If (TypeOf _source Is ObservableCollection(Of TWrapped)) Then
                DirectCast(_source, ObservableCollection(Of TWrapped)).Move(oldIndex, newIndex)
            Else
                _source(newIndex) = _source(oldIndex)
                ' Not entirely sure what the semantics are for the original position - assumed clear rather than a swap with the new position.
                _source(oldIndex) = Nothing
            End If
        End If
    End Sub

    Protected Overrides Sub RemoveItem(index As Integer)
        MyBase.RemoveItem(index)
        If (_source IsNot Nothing) Then
            _source.RemoveAt(index)
        End If
    End Sub

    Protected Overrides Sub SetItem(index As Integer, item As TWrapper)
        MyBase.SetItem(index, item)
        If (_source IsNot Nothing) Then
            _source(index) = item.Source
        End If
    End Sub

End Class

