Option Explicit On
Option Strict On

Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Windows.Forms

Public Class TreeViewMS
    Inherits System.Windows.Forms.TreeView

    Protected mobjColl As ArrayList
    Protected mobjLastNode As TreeNode
    Protected mobjFirstNode As TreeNode

    Public Sub New()
        mobjColl = New ArrayList()
    End Sub

    Protected Overloads Overrides Sub OnPaint(ByVal pe As PaintEventArgs)
        ' Calling the base class OnPaint 
        MyBase.OnPaint(pe)
    End Sub

    Public Property SelectedNodes() As ArrayList
        Get
            Return mobjColl
        End Get
        Set(ByVal value As ArrayList)
            removePaintFromNodes()
            mobjColl.Clear()
            mobjColl = value
            PaintSelectedNodes()
        End Set
    End Property

    Protected Overloads Overrides Sub OnBeforeSelect(ByVal e As TreeViewCancelEventArgs)
        MyBase.OnBeforeSelect(e)

        Dim llogControl As Boolean = (ModifierKeys = Keys.Control)
        Dim llogShift As Boolean = (ModifierKeys = Keys.Shift)

        ' selecting twice the node while pressing CTRL ? 
        If llogControl AndAlso mobjColl.Contains(e.Node) Then
            ' unselect it (let framework know we don't want selection this time) 
            e.Cancel = True

            ' update nodes 
            removePaintFromNodes()
            mobjColl.Remove(e.Node)
            PaintSelectedNodes()
            Exit Sub
        End If

        mobjLastNode = e.Node
        If Not llogShift Then
            mobjFirstNode = e.Node
        End If
        ' store begin of shift sequence 
    End Sub

    Protected Overloads Overrides Sub OnAfterSelect(ByVal e As TreeViewEventArgs)
        MyBase.OnAfterSelect(e)

        Dim llogControl As Boolean = (ModifierKeys = Keys.Control)
        Dim llogShift As Boolean = (ModifierKeys = Keys.Shift)

        If llogControl Then
            If Not mobjColl.Contains(e.Node) Then
                ' new node ? 
                mobjColl.Add(e.Node)
            Else
                ' not new, remove it from the collection 
                removePaintFromNodes()
                mobjColl.Remove(e.Node)
            End If
            PaintSelectedNodes()
        Else
            ' SHIFT is pressed 
            If llogShift Then
                Dim lobjQueue As New Queue()
                Dim lobjUpperNode As TreeNode = mobjFirstNode
                Dim lobjBottomNode As TreeNode = e.Node
                ' case 1 : begin and end nodes are parent 
                Dim llogParent As Boolean = IsParent(mobjFirstNode, e.Node)
                ' is m_firstNode parent (direct or not) of e.Node 
                If Not llogParent Then
                    llogParent = IsParent(lobjBottomNode, lobjUpperNode)
                    If llogParent Then
                        ' swap nodes 
                        Dim lobjNode As TreeNode = lobjUpperNode
                        lobjUpperNode = lobjBottomNode
                        lobjBottomNode = lobjNode
                    End If
                End If
                If llogParent Then
                    Dim lobjNode As TreeNode = lobjBottomNode
                    'While lobjNode <> lobjUpperNode.Parent
                    While lobjNode IsNot lobjUpperNode.Parent
                        If Not mobjColl.Contains(lobjNode) Then
                            ' new node ? 
                            lobjQueue.Enqueue(lobjNode)
                        End If

                        lobjNode = lobjNode.Parent
                    End While
                Else
                    ' case 2 : nor the begin nor the end node are descendant one another 
                    If (lobjUpperNode.Parent Is Nothing AndAlso lobjBottomNode.Parent Is Nothing) OrElse (lobjUpperNode.Parent IsNot Nothing AndAlso lobjUpperNode.Parent.Nodes.Contains(lobjBottomNode)) Then
                        ' are they siblings ? 
                        Dim lintIndexUpper As Integer = lobjUpperNode.Index
                        Dim lintIndexBottom As Integer = lobjBottomNode.Index
                        If lintIndexBottom < lintIndexUpper Then
                            ' reversed? 
                            Dim lobjNode As TreeNode = lobjUpperNode
                            lobjUpperNode = lobjBottomNode
                            lobjBottomNode = lobjNode
                            lintIndexUpper = lobjUpperNode.Index
                            lintIndexBottom = lobjBottomNode.Index
                        End If

                        Dim lobjTmpNode As TreeNode = lobjUpperNode
                        While lintIndexUpper <= lintIndexBottom
                            If Not mobjColl.Contains(lobjTmpNode) Then
                                ' new node ? 
                                lobjQueue.Enqueue(lobjTmpNode)
                            End If

                            lobjTmpNode = lobjTmpNode.NextNode

                            lintIndexUpper += 1
                        End While
                        ' end while 
                    Else
                        If Not mobjColl.Contains(lobjUpperNode) Then
                            lobjQueue.Enqueue(lobjUpperNode)
                        End If
                        If Not mobjColl.Contains(lobjBottomNode) Then
                            lobjQueue.Enqueue(lobjBottomNode)
                        End If
                    End If
                End If

                mobjColl.AddRange(lobjQueue)

                PaintSelectedNodes()
                mobjFirstNode = e.Node
                ' let us chain several SHIFTs if we like it 
            Else
                ' end if m_bShift 
                ' in the case of a simple click, just add this item 
                If mobjColl IsNot Nothing AndAlso mobjColl.Count > 0 Then
                    removePaintFromNodes()
                    mobjColl.Clear()
                End If
                mobjColl.Add(e.Node)
            End If
        End If
    End Sub

    Protected Function IsParent(ByVal tobjParentNode As TreeNode, ByVal tobjChildNode As TreeNode) As Boolean
        'If tobjParentNode = tobjChildNode Then
        If tobjParentNode Is tobjChildNode Then
            Return True
        End If

        Dim lobjNode As TreeNode = tobjChildNode
        Dim llogFound As Boolean = False
        While Not llogFound AndAlso lobjNode IsNot Nothing
            lobjNode = lobjNode.Parent
            'llogFound = (lobjNode = tobjParentNode)
            llogFound = (lobjNode Is tobjParentNode)
        End While
        Return llogFound
    End Function

    Protected Sub PaintSelectedNodes()
        For Each lobjNode As TreeNode In mobjColl
            lobjNode.BackColor = SystemColors.Highlight
            lobjNode.ForeColor = SystemColors.HighlightText
        Next
    End Sub

    Protected Sub removePaintFromNodes()
        If mobjColl.Count = 0 Then
            Exit Sub
        End If

        Dim lobjNode0 As TreeNode = DirectCast(mobjColl(0), TreeNode)
        Dim lstcBackColor As Color = lobjNode0.TreeView.BackColor
        Dim lstcForeColor As Color = lobjNode0.TreeView.ForeColor

        For Each lobjNode As TreeNode In mobjColl
            lobjNode.BackColor = lstcBackColor
            lobjNode.ForeColor = lstcForeColor
        Next
    End Sub

End Class
