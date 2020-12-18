Imports System.ComponentModel
Imports System.Windows.Forms
Imports MouseMPG

Friend Class ConfigForm

    Private UC As Plugininterface.Entry
    Dim PluginMain As UCCNCplugin
    Private Settings As Settings

    Public Sub New(CallerPluginMain As UCCNCplugin)
        Me.UC = CallerPluginMain.UC
        Me.PluginMain = CallerPluginMain
        Me.Settings = Me.PluginMain.Settings
        InitializeComponent()
    End Sub

    Private Sub ConfigForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        radio_in.Checked = Me.Settings.imperial
        radio_mm.Checked = Not Me.Settings.imperial

        txt_feedrate.Text = Me.Settings.feedrate.ToString
    End Sub

    Private Sub txt_feedrate_TextChanged(sender As Object, e As EventArgs) Handles txt_feedrate.TextChanged

    End Sub

    Private Sub txt_feedrate_Validating(sender As Object, e As CancelEventArgs) Handles txt_feedrate.Validating
        Dim res As Double = 0
        If Not Double.TryParse(txt_feedrate.Text, res) Then
            e.Cancel = True
        Else
            Settings.feedrate = res

            txt_feedrate.Text = res.ToString

        End If
    End Sub

    Private Sub ConfigForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

    End Sub

    Private Sub radio_mm_Validating(sender As Object, e As CancelEventArgs) Handles radio_mm.Validating, radio_in.Validating

    End Sub

    Private Sub ConfigForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed

    End Sub

    Private Sub radio_mm_CheckedChanged(sender As Object, e As EventArgs) Handles radio_mm.CheckedChanged

    End Sub

    Private Sub ConfigForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim res As Double = 0
        If Double.TryParse(txt_feedrate.Text, res) Then
            Settings.feedrate = res
        End If

        Settings.imperial = radio_in.Checked
        Settings.Save()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("https://hsmadvisor.com")
    End Sub
End Class