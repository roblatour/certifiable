' Certifiable

' Copyright Rob Latour, 2025
' License MIT

' github/roblatour/certifiable

Imports System.IO
Imports System.Net.Security
Imports System.Net.Sockets
Imports System.Reflection
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Threading
Imports TextCopy

Module Program

    Dim iHost As String = String.Empty
    Dim iPort As Integer = 443
    Dim iGenerateLanguage As String = String.Empty
    Dim iVariableName As String = String.Empty
    Dim iDisplay As Boolean = False
    Dim iCertNum As Integer = 1
    Dim iCopyToClipboard As Boolean = False
    Dim iWriteToFile As Boolean = False
    Dim iOutputFilename As String = String.Empty
    Dim iOverwriteOutputFile As Boolean = False
    Dim iFuture As Boolean = False
    Dim iHelp As Boolean = False
    Dim iCertNumSpecified As Boolean = False

    Dim gExpiredCertificates As Integer = 0
    Dim gCurrentCertificates As Integer = 0
    Dim gFutureCertificates As Integer = 0

    Dim gCurrentTimeUTC As DateTime

    Dim gStartingColour As ConsoleColor

    Dim AllCertifcates As New List(Of X509Certificate2)()

    Private Function ServerCertificateValidationCallback(sender As Object, certificate As X509Certificate2, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) As Boolean

        ' Accept all certificates  
        Return True

    End Function

    Function GetAllCertifations() As Boolean

        gCurrentTimeUTC = DateTime.UtcNow
        Dim timeout As Integer = 5000 ' Timeout in milliseconds

        Try

            Using client As New TcpClient()

                Dim connectTask As Task = client.ConnectAsync(iHost, iPort)
                If Not connectTask.Wait(timeout) Then
                    Throw New TimeoutException("The connection attempt timed out.")
                End If

                client.ReceiveTimeout = timeout
                client.SendTimeout = timeout

                Using sslStream As New SslStream(client.GetStream(), False, AddressOf ServerCertificateValidationCallback)
                    Dim cts As New CancellationTokenSource(timeout)
                    Dim authTask As Task = sslStream.AuthenticateAsClientAsync(iHost)
                    If Not authTask.Wait(timeout) Then
                        Throw New TimeoutException("The SSL authentication timed out.")
                    End If

                    Dim chain As New X509Chain()
                    chain.Build(sslStream.RemoteCertificate)

                    Dim certificateInfo As New StringBuilder()

                    For Each cert In chain.ChainElements
                        If gCurrentTimeUTC > cert.Certificate.NotAfter Then
                            gExpiredCertificates += 1
                        ElseIf (gCurrentTimeUTC >= cert.Certificate.NotBefore) AndAlso (gCurrentTimeUTC <= cert.Certificate.NotAfter) Then
                            AllCertifcates.Add(cert.Certificate)
                            gCurrentCertificates += 1
                        Else
                            gFutureCertificates += 1
                            If iFuture Then
                                AllCertifcates.Add(cert.Certificate)
                            End If
                        End If
                    Next
                End Using
            End Using
        Catch ex As TimeoutException

            Console_WriteLineInColour("Error: the connection attempt to " & iHost & ":" & iPort & " timed out", ConsoleColor.Red)

            Return False

        Catch ex As Exception

            Console_WriteLineInColour(" ", ConsoleColor.Red)
            Console_WriteLineInColour("Error:", ConsoleColor.Red)

            Console_WriteLineInColour(ex.Message, ConsoleColor.Red)

            If ex.Message.Contains("Cannot determine the frame size or a corrupted frame was received") Then
                Console_WriteLineInColour("It may be you are looking for a certificate where there is none. For example with a server that only supports http and not https connections.", ConsoleColor.Red)
            End If

            Return False

        End Try

        Return True

    End Function

    Private Sub DisplayCertificates()

        If AllCertifcates.Count > 0 Then

            Dim count As Integer = 0

            For Each cert As X509Certificate2 In AllCertifcates

                count += 1

                If (iCertNumSpecified AndAlso (count = iCertNum)) OrElse (Not iCertNumSpecified) Then

                    Console_WriteLineInColour(" ")
                    Console_WriteLineInColour("Certificate Number:  " & count, ConsoleColor.Cyan)
                    Console_WriteLineInColour("Friendly Name:       " & cert.FriendlyName)
                    Console_WriteLineInColour("Subject:             " & cert.Subject)
                    Console_WriteLineInColour("Issuer:              " & cert.Issuer)
                    If (gCurrentTimeUTC >= cert.NotBefore) AndAlso (gCurrentTimeUTC <= cert.NotAfter) Then
                        Console_WriteLineInColour("Not Before:          " & cert.NotBefore, ConsoleColor.Green)
                        Console_WriteLineInColour("Not After:           " & cert.NotAfter, ConsoleColor.Green)
                    Else
                        Console_WriteLineInColour("Not Before:          " & cert.NotBefore, ConsoleColor.Yellow)
                        Console_WriteLineInColour("Not After:           " & cert.NotAfter, ConsoleColor.Yellow)
                    End If
                    Console_WriteLineInColour("Signature Algorithm: " & cert.SignatureAlgorithm.ToString)
                    Console_WriteLineInColour("Serial Number:       " & cert.SerialNumber)
                    Console_WriteLineInColour("Thumbprint:          " & cert.Thumbprint)
                    Console_WriteLineInColour("PEM:                 " & cert.ExportCertificatePem)

                End If

            Next

        End If

        If gExpiredCertificates = 1 Then
            Console_WriteLineInColour(" ")
            Console_WriteLineInColour("One expired certificate was not shown", ConsoleColor.Yellow)
        ElseIf gExpiredCertificates = 1 Then
            Console_WriteLineInColour(" ")
            Console_WriteLineInColour(gExpiredCertificates & " expired certificate were not shown", ConsoleColor.Yellow)
        End If

        If iFuture AndAlso (gFutureCertificates = 0) Then
            Console_WriteLineInColour(" ")
            Console_WriteLineInColour("There are no future dated certificates", ConsoleColor.Yellow)
        End If

    End Sub

    Private Function CreateOutputFileForCPlusPlus(cert As X509Certificate2) As String

        Dim ReturnValue As String = String.Empty

        Try

            Dim outputString As String
            Dim startCommentString As String
            Dim endString As String
            Dim firstLine As String
            Dim indent As Integer
            Dim variableName As String = String.Empty

            startCommentString = "// "
            endString = "\n"";"

            If iVariableName = String.Empty Then
                variableName = iHost.ToUpper.Replace(".", "_")
                If IsNumeric(variableName.Substring(0, 1)) Then
                    variableName = "HOST_" & variableName
                End If
                variableName &= "_CERTIFICATE"
            Else
                variableName = iVariableName
            End If

            firstLine = "const char* " & variableName & " = "
            indent = firstLine.Length

            outputString = startCommentString & "The following certificate is for use with " & iHost.ToLower & ":" & iPort & vbCrLf
            outputString &= startCommentString & "It is valid between " & cert.NotBefore.ToString & " (UTC) and " & cert.NotAfter.ToString & " (UTC) inclusive." & vbCrLf

            outputString &= firstLine

            Using reader As New StringReader(cert.ExportCertificatePem)
                Dim line As String = reader.ReadLine()
                While line IsNot Nothing
                    If Not line.Contains("-----BEGIN CERTIFICATE-----") Then
                        outputString &= StrDup(indent, " ")
                    End If

                    outputString &= """" & line

                    If line.Contains("-----END CERTIFICATE-----") Then
                        outputString &= endString
                    Else
                        outputString &= "\n"" \"
                    End If

                    outputString &= vbCrLf

                    line = reader.ReadLine()
                End While
            End Using

            ReturnValue = outputString

        Catch ex As Exception

            Console_WriteLineInColour(" ", ConsoleColor.Red)
            Console_WriteLineInColour("Error:", ConsoleColor.Red)
            Console_WriteLineInColour(ex.Message, ConsoleColor.Red)

        End Try

        Return ReturnValue

    End Function


    Private Function CreateOutputFileForPython(cert As X509Certificate2) As String

        Dim ReturnValue As String = String.Empty

        Try

            Dim outputString As String
            Dim startCommentString As String
            Dim endString As String
            Dim firstLine As String
            Dim variableName As String
            Dim indent As Integer

            startCommentString = "# "
            endString = """"""""

            If iVariableName = String.Empty Then
                variableName = iHost.ToUpper.Replace(".", "_")
                If IsNumeric(variableName.Substring(0, 1)) Then
                    variableName = "HOST_" & variableName
                End If
                variableName &= "_CERTIFICATE"
            Else
                variableName = iVariableName
            End If

            firstLine = variableName & " = "
            indent = 0

            outputString = startCommentString & "The following certificate is for use with " & iHost.ToLower & ":" & iPort & vbCrLf
            outputString &= startCommentString & "It is valid between " & cert.NotBefore.ToString & " (UTC) and " & cert.NotAfter.ToString & " (UTC) inclusive." & vbCrLf

            outputString &= firstLine

            Using reader As New StringReader(cert.ExportCertificatePem)
                Dim line As String = reader.ReadLine()
                While line IsNot Nothing

                    If line.Contains("-----BEGIN CERTIFICATE-----") Then
                        outputString &= """"""""
                    End If

                    outputString &= line

                    If line.Contains("-----END CERTIFICATE-----") Then
                        outputString &= endString
                    End If

                    outputString &= vbCrLf

                    line = reader.ReadLine()
                End While
            End Using

            ReturnValue = outputString

        Catch ex As Exception

            Console_WriteLineInColour(" ", ConsoleColor.Red)
            Console_WriteLineInColour("Error:", ConsoleColor.Red)
            Console_WriteLineInColour(ex.Message, ConsoleColor.Red)

        End Try


        Return ReturnValue

    End Function

    Private Function CreateOutputFileForVBNet(cert As X509Certificate2) As String

        Dim ReturnValue As String = String.Empty

        Try

            Dim outputString As String
            Dim startCommentString As String
            Dim endString As String
            Dim firstLine As String
            Dim variableName As String
            Dim indent As Integer

            startCommentString = "' "
            endString = """"

            If iVariableName = String.Empty Then
                variableName = iHost.ToUpper.Replace(".", "_")
                If IsNumeric(variableName.Substring(0, 1)) Then
                    variableName = "HOST_" & variableName
                End If
                variableName &= "_CERTIFICATE"
            Else
                variableName = iVariableName
            End If

            firstLine = "Dim " & variableName & " as String = "
            indent = firstLine.Length

            outputString = startCommentString & "The following certificate is for use with " & iHost.ToLower & ":" & iPort & vbCrLf
            outputString &= startCommentString & "It is valid between " & cert.NotBefore.ToString & " (UTC) and " & cert.NotAfter.ToString & " (UTC) inclusive." & vbCrLf

            outputString &= firstLine

            Using reader As New StringReader(cert.ExportCertificatePem)
                Dim line As String = reader.ReadLine()
                While line IsNot Nothing
                    If Not line.Contains("-----BEGIN CERTIFICATE-----") Then
                        outputString &= StrDup(indent, " ")
                    End If

                    outputString &= """" & line

                    If line.Contains("-----END CERTIFICATE-----") Then
                        outputString &= endString
                    Else
                        outputString &= "\n"" \"
                    End If

                    outputString &= vbCrLf

                    line = reader.ReadLine()
                End While
            End Using

            ReturnValue = outputString

        Catch ex As Exception

            Console_WriteLineInColour(" ", ConsoleColor.Red)
            Console_WriteLineInColour("Error:", ConsoleColor.Red)
            Console_WriteLineInColour(ex.Message, ConsoleColor.Red)

        End Try


        Return ReturnValue

    End Function

    Private Sub Generate(cert As X509Certificate2)

        Dim outputString As String = String.Empty
        Dim codeType As String = String.Empty

        Select Case iGenerateLanguage

            Case "c++"
                codeType = "C++"
                outputString = CreateOutputFileForCPlusPlus(cert)

            Case "python"
                codeType = "Python"
                outputString = CreateOutputFileForPython(cert)

            Case "vb.net"
                codeType = "VB.Net"
                outputString = CreateOutputFileForVBNet(cert)

            Case Else
                Console_WriteLineInColour("Invalid code format " & iGenerateLanguage, ConsoleColor.Yellow)

        End Select

        ' Console_WriteLineInColour(outputString) 

        Try

            If iCopyToClipboard Then

                ClipboardService.SetText(outputString)
                Console_WriteLineInColour(" ")
                Console_WriteLineInColour(codeType & " certificate code for " & iHost & ":" & iPort & " copied to the clipboard", ConsoleColor.Green)

            End If

        Catch ex As Exception

            Console_WriteLineInColour("Error: Copy to clipboard failed", ConsoleColor.Red)
            Console_WriteLineInColour(ex.Message, ConsoleColor.Red)

        End Try

        Try

            If iWriteToFile Then

                If outputString.Length > 0 Then

                    Dim OKToWrite As Boolean = True

                    ' Check if the file exists

                    If iOverwriteOutputFile Then
                    Else

                        If File.Exists(iOutputFilename) Then

                            Console_WriteLineInColour(iOutputFilename & " already exists. Do you want to overwrite it? (y/n)", ConsoleColor.Yellow)

                            Dim response As String = Console.ReadLine().ToLower()

                            If response = "y" OrElse response = "yes" Then
                            Else
                                Console_WriteLineInColour(iOutputFilename & " will not be overwritten")
                                Console_WriteLineInColour(codeType & " certificate code for " & iHost & ":" & iPort & " was not exported to " & iOutputFilename, ConsoleColor.Yellow)
                                OKToWrite = False
                            End If

                        End If

                    End If

                    If OKToWrite Then

                        ' Write the output to the output file

                        Dim path As String = IO.Path.GetDirectoryName(iOutputFilename)

                        If (path IsNot Nothing) AndAlso (path.Length > 0) Then
                            If Not Directory.Exists(path) Then
                                Directory.CreateDirectory(path)
                            End If
                        End If

                        Using file As New StreamWriter(iOutputFilename, False)
                            file.WriteLine(outputString)
                        End Using

                        Console_WriteLineInColour(" ")
                        Console_WriteLineInColour(codeType & " certificate code for " & iHost & ":" & iPort & " exported to " & iOutputFilename, ConsoleColor.Green)

                    End If

                End If

            End If


        Catch ex As Exception

            Console_WriteLineInColour("Error: Failed to write to " & iOutputFilename, ConsoleColor.Red)
            Console_WriteLineInColour(ex.Message, ConsoleColor.Red)

        End Try

    End Sub

    Private Function ValidateCertNum() As Boolean

        Dim returnCode As Boolean

        If iFuture Then
            returnCode = (iCertNum <= gCurrentCertificates + gFutureCertificates)
        Else
            returnCode = (iCertNum <= gCurrentCertificates)
        End If

        If returnCode Then
        Else

            Console_WriteLineInColour("The -n option value was set to " & iCertNum, ConsoleColor.Red)
            Console_WriteLineInColour("However, for " & iHost & ":" & iPort & " the -n option value must be between 1 and " & (gCurrentCertificates).ToString & " (inclusive)", ConsoleColor.Red)

            If gFutureCertificates > 0 Then
                Console_WriteLineInColour("unless the -f option is used, in which case the -n option value must be between 1 and " & (gCurrentCertificates + gFutureCertificates).ToString & " (inclusive)", ConsoleColor.Red)
            End If
            Console_WriteLineInColour(" ")
            Console_WriteLineInColour("For more information, here is the information on all current and future dated certifications:")

            iCertNumSpecified = False
            iFuture = True
            DisplayCertificates()

        End If

        Return returnCode

    End Function
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub Console_WriteLineInColour(ByVal Message As String, Optional ByVal Colour As ConsoleColor = ConsoleColor.Gray, Optional PrintSpecialCharacters As Boolean = False)

        Dim OriginalTextEncoding As System.Text.Encoding = System.Console.OutputEncoding

        Try

            If PrintSpecialCharacters Then
                System.Console.OutputEncoding = System.Text.Encoding.Unicode
            End If

            If (Console.BackgroundColor = Colour) Then
                Colour = ConsoleColor.Black
            End If

            Console.ForegroundColor = Colour

            If Message.Length > 0 Then
                Console.WriteLine(Message)
            End If

            If PrintSpecialCharacters Then
                System.Console.OutputEncoding = OriginalTextEncoding
            End If

        Catch ex As Exception

            Console_WriteLineInColour(" ", ConsoleColor.Red)
            Console_WriteLineInColour("Error:", ConsoleColor.Red)
            Console_WriteLineInColour(ex.Message, ConsoleColor.Red)

        End Try

    End Sub

    Private Function ValidateArguments(args As String()) As Boolean

        If (args.Length = 0) OrElse (args(0) = "?") OrElse (args(0) = "-?") Then
            iHelp = True
            Return True
        End If

        ' List of valid arguments
        Dim validArguments As HashSet(Of String) = New HashSet(Of String) From {"-p", "-g", "-v", "-d", "-n", "-c", "-w", "-o", "-f"}

        Dim invalidArgumentFound As Boolean = False

        For Each arg As String In args
            If arg.StartsWith("-") AndAlso Not validArguments.Contains(arg) Then
                Console_WriteLineInColour("Invalid argument: " & arg, ConsoleColor.Red)
                invalidArgumentFound = True
            End If
        Next

        If invalidArgumentFound Then
            Console_WriteLineInColour("please correct", ConsoleColor.Red)
            Return False
        End If

        iHost = args(0)

        For i As Integer = 0 To args.Length - 1
            Select Case args(i).ToLower()
                Case "-p"
                    If i + 1 < args.Length Then
                        If (Integer.TryParse(args(i + 1), iPort)) Then
                        Else
                            Console_WriteLineInColour("the -p option, if used, must be followed an integer value from 1 to 65535 inclusive", ConsoleColor.Red)
                            Return False
                        End If
                    End If
                Case "-g"
                    If i + 1 < args.Length Then iGenerateLanguage = args(i + 1)
                Case "-v"
                    If i + 1 < args.Length Then iVariableName = args(i + 1)
                Case "-d"
                    iDisplay = True
                Case "-n"
                    If i + 1 < args.Length Then
                        If (Integer.TryParse(args(i + 1), iCertNum)) Then
                            iCertNumSpecified = True
                        Else
                            Console_WriteLineInColour("the -n option, if used, must be followed an integer value greater than 0", ConsoleColor.Red)
                            Return False
                        End If
                    End If
                Case "-c"
                    iCopyToClipboard = True
                Case "-w"
                    If i + 1 < args.Length Then
                        Try
                            iWriteToFile = True
                            iOutputFilename = args(i + 1)
                        Catch ex As Exception
                            Console_WriteLineInColour("the -w option, if used, must be followed by a filename which may include a path", ConsoleColor.Red)
                            Return False
                        End Try
                    End If
                Case "-o"
                    iOverwriteOutputFile = True
                Case "-f"
                    iFuture = True
                Case "?"
                    iHelp = True

            End Select
        Next

        If Not ((Uri.CheckHostName(iHost) = UriHostNameType.Dns) OrElse (Uri.CheckHostName(iHost) = UriHostNameType.IPv4) OrElse (Uri.CheckHostName(iHost) = UriHostNameType.IPv6)) Then
            Console_WriteLineInColour("the host must be a valid domain name or IP address", ConsoleColor.Red)
            Return False
        End If

        If IsNumeric(iPort) AndAlso ((iPort > 0) AndAlso (iPort < 65536)) Then
            ' OK
        Else
            Console_WriteLineInColour("the -p option, if used, must be followed an integer value from 1 to 65535 inclusive", ConsoleColor.Red)
            Return False
        End If

        If IsNumeric(iCertNum) AndAlso ((iCertNum > 0)) Then
            ' OK
        Else
            Console_WriteLineInColour("the -c option, if used, must be followed by an integer value greater than 0", ConsoleColor.Red)
            Return False
        End If

        If (iDisplay) OrElse (iWriteToFile) OrElse (iCopyToClipboard) Then
        Else
            iDisplay = True
        End If

        If iGenerateLanguage.Length > 0 Then

            iGenerateLanguage = iGenerateLanguage.ToLower()

            If (iGenerateLanguage = "c++") OrElse (iGenerateLanguage = "vb.net") OrElse (iGenerateLanguage = "python") Then
            Else
                Console_WriteLineInColour("the -f option, if used, must be followed by 'c++', 'python', or 'vb.net' (without the quotes)", ConsoleColor.Red)
                Return False
            End If

        Else

            iGenerateLanguage = "c++"

        End If

        Return True

    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Function GetVersionNumber() As String
        Dim assembly As Assembly = assembly.GetExecutingAssembly()
        Dim version As Version = assembly.GetName().Version
        Dim versionString As String = version.ToString()

        While versionString.EndsWith(".0")
            versionString = versionString.Substring(0, versionString.Length - 2)
        End While

        If versionString = String.Empty Then
            versionString = 0
        End If

        Return versionString
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub DisplayHelp()

        Dim StartingColour As ConsoleColor = Console.ForegroundColor

        Dim versionNumber As String = GetVersionNumber()

        Console_WriteLineInColour(" ")
        Console_WriteLineInColour("Certifiable v" & GetVersionNumber() & " Help", ConsoleColor.White)
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour("Given a host name or IP address, and some additional information as outlined below,")
        Console_WriteLineInColour("Certifiable will generate the code for assigning a variable a SSL certificate PEM")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour("Usage:")
        Console_WriteLineInColour("certifiable [ host (-p n) (-d) (-n n) (-g x) (-v x) (-c) (-w) (-o) (-f) ] | [ ] | [ ? ]")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour("host   the host name or IP address from which to get the certificate(s)")
        Console_WriteLineInColour("       for example: google.com, www.google.com, 142.251.41.14")
        Console_WriteLineInColour("       a host value is required in all cases other than displaying this help information")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour("Options:", ConsoleColor.White)
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour(" -p n  the host's port number")
        Console_WriteLineInColour("       for example: -p 8096")
        Console_WriteLineInColour("       if not used a port number of 443 will be assumed")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour(" -d    display the host's available certificates")
        Console_WriteLineInColour("       if neither -c or -o options are used, the -d option will be assumed")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour(" -n n  in many cases more than one certificate may be available for a host")
        Console_WriteLineInColour("       when the -d option is used each certificate will be displayed with a unique certification number")
        Console_WriteLineInColour("       if not otherwise specified the first certificate will be used in code generation")
        Console_WriteLineInColour("       to specify a certificate other than the first certificate")
        Console_WriteLineInColour("       use -n n where the second n is the unique certificate number displayed with the -d option")
        Console_WriteLineInColour("       for example, -n 3")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour(" -g x  used to specifying the language in which the code will be generated")
        Console_WriteLineInColour("       supported languages are: c++, python, and vb.net")
        Console_WriteLineInColour("       for example: -g vb.net")
        Console_WriteLineInColour("       if not used, the c++ will be assumed")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour(" -v x  the variable name to be used in the output file")
        Console_WriteLineInColour("       for example: -v server_root_cert")
        Console_WriteLineInColour("       if not used, a variable name based on the host name / IP address will be generated")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour(" -c    the generated code will be copied to the clipboard")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour(" -w x  the generated code will be written to the specified (path and) file name")
        Console_WriteLineInColour("       for example, -w c:\temp\certificate.h")
        Console_WriteLineInColour("       the output path name is optional, if omitted the output file will be written to the current working directory")
        Console_WriteLineInColour("       for example, -w certificate.h")
        Console_WriteLineInColour("       if the path is specified but does not exist, it will be created")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour(" -o    if the -w option is used and the specified file already exists")
        Console_WriteLineInColour("       the user will be prompted to overwrite the file unless the -o options is used")
        Console_WriteLineInColour("       in which case the existing file will be automatically overwritten")
        Console_WriteLineInColour("       for example, -w c:\temp\certificate.h -o")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour(" -f    by default future dated certificates are ignored")
        Console_WriteLineInColour("       if -f is used, then future dated certificates won't be ignored")
        Console_WriteLineInColour("       note: expired certificates are always ignored")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour(" Certifiable used with no arguments, or with '?' displays this help")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour("Final notes on usage:")
        Console_WriteLineInColour(" the -d option does not generate code, rather simply displays available information")
        Console_WriteLineInColour(" code will only be generated if the -c or -o options are used")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour("Some common examples:")
        Console_WriteLineInColour(" certifiable ?")
        Console_WriteLineInColour(" certifiable www.google.com")
        Console_WriteLineInColour(" certifiable 142.251.41.14 -d")
        Console_WriteLineInColour(" certifiable github.com -d")
        Console_WriteLineInColour(" certifiable github.com -n 2 -c")
        Console_WriteLineInColour(" certifiable github.com -n 2 -g python -c")
        Console_WriteLineInColour(" certifiable github.com -w certificate.h")
        Console_WriteLineInColour(" certifiable github.com -w certificate.h -o")
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour("Certifiable v" & versionNumber, ConsoleColor.Yellow)
        Console_WriteLineInColour("Copyright © 2025, Rob Latour", ConsoleColor.Yellow, True)
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour("Certifiable is open source", ConsoleColor.Cyan)
        Console_WriteLineInColour("https://github.com/roblatour/certifiable", ConsoleColor.Cyan)
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour("Certifiable is licensed under the MIT License", ConsoleColor.Cyan)
        Console_WriteLineInColour("https://github.com/roblatour/setvol/blob/main/LICENSE", ConsoleColor.Cyan)
        Console_WriteLineInColour(" ")
        Console_WriteLineInColour("Certifiable makes use of TextCopy which is licensed under the MIT License", ConsoleColor.Cyan)
        Console_WriteLineInColour("https://github.com/CopyText/TextCopy", ConsoleColor.Cyan)
        Console_WriteLineInColour(" ")

        Console.ForegroundColor = StartingColour

    End Sub

    Sub Main(args As String())

        gStartingColour = Console.ForegroundColor

        If ValidateArguments(args) Then

            If -iHelp Then

                DisplayHelp()

            ElseIf GetAllCertifations() Then

                If (ValidateCertNum()) Then

                    Dim OKToGenerate As Boolean = (iCopyToClipboard OrElse iWriteToFile)

                    If iDisplay Then DisplayCertificates()

                    If iFuture Then

                        If (gCurrentCertificates = 0) AndAlso (gFutureCertificates = 0) Then

                            Console_WriteLineInColour("No current or future certificates were found for " & iHost & "on port " & iPort)
                            OKToGenerate = False

                        End If

                    Else

                        If (gCurrentCertificates = 0) Then

                            Console_WriteLineInColour("No current certificates were found for " & iHost & "on port " & iPort)
                            OKToGenerate = False

                        End If

                    End If

                    If iFuture Then
                    Else
                        If gFutureCertificates = 1 Then
                            Console_WriteLineInColour(" ")
                            Console_WriteLineInColour("Note: there is one future dated certificate, to see it use the -future option")
                        ElseIf gFutureCertificates > 1 Then
                            Console_WriteLineInColour(" ")
                            Console_WriteLineInColour("Note: there are " & gFutureCertificates & " future dated certificates, to see them use the -future option")
                        End If
                    End If

                    If OKToGenerate Then
                        Generate(AllCertifcates(iCertNum - 1))
                    End If

                End If

            End If

        End If

        Console.ForegroundColor = gStartingColour

        Environment.Exit(0)

    End Sub

End Module
