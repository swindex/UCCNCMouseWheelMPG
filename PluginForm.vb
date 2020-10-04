Imports System.Threading
Imports System.Windows.Forms

Friend Class PluginForm
    Private UC As Plugininterface.Entry
    Dim PluginMain As UCCNCplugin
    Dim MustClose As Boolean = False
    Dim WheelDelta As Integer = 120
    Dim zpos As Double = 0
    Dim stp As Double = 0.001
    Dim direction As Direction = Direction.X

    Dim deftbcolor As Drawing.Color

    Public Sub New(CallerPluginMain As UCCNCplugin)
        Me.UC = CallerPluginMain.UC
        Me.PluginMain = CallerPluginMain
        InitializeComponent()
    End Sub

    Public Shared Sub Main()

    End Sub

    Private Sub PluginForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        WheelDelta = SystemInformation.MouseWheelScrollDelta
        deftbcolor = BTN_X.BackColor
        updateLabels()
    End Sub

    Private Sub PluginForm_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        ' Do not close the form when the red X button is pressed
        ' But start a Thread which will stop the Loop call from the UCCNC
        ' to prevent the form closing while there is a GUI update in the Loop event
        If Not MustClose Then
            e.Cancel = True
            Dim thrClose As New Thread(Sub() Closeform())
            thrClose.CurrentCulture = Thread.CurrentThread.CurrentCulture ' Preserve regional settings
            thrClose.Start()
        Else
            ' Form is closing here...
        End If
    End Sub

    Public Sub Closeform()
        ' Stop the Loop event to update the GUI
        PluginMain.LoopStop = True
        ' Wait until the loop exited
        While (PluginMain.LoopWorking)
            Thread.Sleep(10)
        End While
        ' Set the mustclose variable to true and call the .Close() function to close the Form
        MustClose = True
        Me.Close()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub PictureBox1_MouseWheel(sender As Object, e As MouseEventArgs)

    End Sub

    Private Sub PluginForm_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel


    End Sub

    Private Sub jog(steps As Integer)
        If UC.IsMoving Or UC.Getfield(False, 54) Then
            Return
        End If

        Dim dist As Double = steps * stp
        zpos = zpos + dist
        If direction = Direction.X Then
            Dim v = UC.GetXmachpos()
            UC.Code("G53 X" & v + dist)
        End If
        If direction = Direction.Y Then
            Dim v = UC.GetYmachpos()
            UC.Code("G53 Y" & v + dist)
        End If
        If direction = Direction.Z Then
            Dim v = UC.GetZmachpos()
            UC.Code("G53 Z" & v + dist)
        End If
        updateLabels()
    End Sub

    Private Sub updateLabels()
        BTN_0001.BackColor = deftbcolor
        BTN_001.BackColor = deftbcolor
        BTN_01.BackColor = deftbcolor
        BTN_1.BackColor = deftbcolor


        BTN_X.BackColor = deftbcolor
        BTN_Y.BackColor = deftbcolor
        BTN_Z.BackColor = deftbcolor

        If stp = 0.001 Then
            BTN_0001.BackColor = Drawing.Color.Green
        End If
        If stp = 0.01 Then
            BTN_001.BackColor = Drawing.Color.Green
        End If
        If stp = 0.1 Then
            BTN_01.BackColor = Drawing.Color.Green
        End If
        If stp = 1 Then
            BTN_1.BackColor = Drawing.Color.Green
        End If

        If direction = Direction.X Then
            BTN_X.BackColor = Drawing.Color.Green
        End If

        If direction = Direction.Y Then
            BTN_Y.BackColor = Drawing.Color.Green
        End If

        If direction = Direction.Z Then
            BTN_Z.BackColor = Drawing.Color.Green
        End If


    End Sub

    Private Sub BTN_X_Click(sender As Object, e As EventArgs) Handles BTN_X.Click
        direction = Direction.X
        updateLabels()
    End Sub
    Private Sub BTN_y_Click(sender As Object, e As EventArgs) Handles BTN_Y.Click
        direction = Direction.Y
        updateLabels()
    End Sub
    Private Sub BTN_z_Click(sender As Object, e As EventArgs) Handles BTN_Z.Click
        direction = Direction.Z
        updateLabels()
    End Sub

    Private Sub BTN_0001_Click(sender As Object, e As EventArgs) Handles BTN_0001.Click
        stp = 0.001
        updateLabels()
    End Sub

    Private Sub BTN_001_Click(sender As Object, e As EventArgs) Handles BTN_001.Click
        stp = 0.01
        updateLabels()
    End Sub

    Private Sub BTN_01_Click(sender As Object, e As EventArgs) Handles BTN_01.Click
        stp = 0.1
        updateLabels()
    End Sub
    Private Sub BTN_1_Click(sender As Object, e As EventArgs) Handles BTN_1.Click
        stp = 1
        updateLabels()
    End Sub

    Private Sub MPG_BOX_Click(sender As Object, e As EventArgs) Handles MPG_BOX.Click

    End Sub

    Private Sub MPG_BOX_MouseWheel(sender As Object, e As MouseEventArgs) Handles MPG_BOX.MouseWheel
        Dim steps As Integer = e.Delta / WheelDelta
        jog(steps)
    End Sub
End Class

Public Enum Direction
    X
    Y
    Z
End Enum