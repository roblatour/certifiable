# Certifiable (v1.4 January 21, 2025)

Certifiable is an open source command line program for Windows which generates code to assign a SSL Certificate PEM to a variable.

The following languages are supported for code generation: C++, Python, and VB.net.  If you would like another programming language added, please just open an issue including an example of what a statement would look like in that language.

## Download 

You are welcome to download and use Certifiable on as many computers as you would like.

A downloadable signed executable portable copy of the program for Windows (x64) is available from [here](https://github.com/roblatour/certifiable/releases/download/v1.4.0.0/certifiable.exe).

## License
Certifiable is licensed under a [MIT license](https://github.com/roblatour/certifiable/blob/main/LICENSE)

## Setup

Certifiable does not need to be installed, rather download file (above) and run it.


## Example output

From the Windows command line enter:

```cpp
certifiable api.pushbullet.com -n 2 -c
```
the following response is returned:

<!-- { color: green } -->
`C++ certificate code for api.pushbullet.com:443 copied to the clipboard`

and here is what will be copied to the clipboard:
```cpp
// The following code is for use with api.pushbullet.com:443
// 
// It has been generated by Certifiable v1.4 ( https://github.com/roblatour/certifiable ) using the following command:
// "C:\Program Files\certifiable\certifiable.exe" api.pushbullet.com -n 2 -c
// 
// The certificate below is valid between 2024-03-12 20:00:00 (UTC) and 2027-03-12 18:59:59 (UTC) inclusive
// 
static const char *API_PUSHBULLET_COM_CERTIFICATE = "-----BEGIN CERTIFICATE-----\n" \
                                                    "MIIEVzCCAj+gAwIBAgIRALBXPpFzlydw27SHyzpFKzgwDQYJKoZIhvcNAQELBQAw\n" \
                                                    "TzELMAkGA1UEBhMCVVMxKTAnBgNVBAoTIEludGVybmV0IFNlY3VyaXR5IFJlc2Vh\n" \
                                                    "cmNoIEdyb3VwMRUwEwYDVQQDEwxJU1JHIFJvb3QgWDEwHhcNMjQwMzEzMDAwMDAw\n" \
                                                    "WhcNMjcwMzEyMjM1OTU5WjAyMQswCQYDVQQGEwJVUzEWMBQGA1UEChMNTGV0J3Mg\n" \
                                                    "RW5jcnlwdDELMAkGA1UEAxMCRTYwdjAQBgcqhkjOPQIBBgUrgQQAIgNiAATZ8Z5G\n" \
                                                    "h/ghcWCoJuuj+rnq2h25EqfUJtlRFLFhfHWWvyILOR/VvtEKRqotPEoJhC6+QJVV\n" \
                                                    "6RlAN2Z17TJOdwRJ+HB7wxjnzvdxEP6sdNgA1O1tHHMWMxCcOrLqbGL0vbijgfgw\n" \
                                                    "gfUwDgYDVR0PAQH/BAQDAgGGMB0GA1UdJQQWMBQGCCsGAQUFBwMCBggrBgEFBQcD\n" \
                                                    "ATASBgNVHRMBAf8ECDAGAQH/AgEAMB0GA1UdDgQWBBSTJ0aYA6lRaI6Y1sRCSNsj\n" \
                                                    "v1iU0jAfBgNVHSMEGDAWgBR5tFnme7bl5AFzgAiIyBpY9umbbjAyBggrBgEFBQcB\n" \
                                                    "AQQmMCQwIgYIKwYBBQUHMAKGFmh0dHA6Ly94MS5pLmxlbmNyLm9yZy8wEwYDVR0g\n" \
                                                    "BAwwCjAIBgZngQwBAgEwJwYDVR0fBCAwHjAcoBqgGIYWaHR0cDovL3gxLmMubGVu\n" \
                                                    "Y3Iub3JnLzANBgkqhkiG9w0BAQsFAAOCAgEAfYt7SiA1sgWGCIpunk46r4AExIRc\n" \
                                                    "MxkKgUhNlrrv1B21hOaXN/5miE+LOTbrcmU/M9yvC6MVY730GNFoL8IhJ8j8vrOL\n" \
                                                    "pMY22OP6baS1k9YMrtDTlwJHoGby04ThTUeBDksS9RiuHvicZqBedQdIF65pZuhp\n" \
                                                    "eDcGBcLiYasQr/EO5gxxtLyTmgsHSOVSBcFOn9lgv7LECPq9i7mfH3mpxgrRKSxH\n" \
                                                    "pOoZ0KXMcB+hHuvlklHntvcI0mMMQ0mhYj6qtMFStkF1RpCG3IPdIwpVCQqu8GV7\n" \
                                                    "s8ubknRzs+3C/Bm19RFOoiPpDkwvyNfvmQ14XkyqqKK5oZ8zhD32kFRQkxa8uZSu\n" \
                                                    "h4aTImFxknu39waBxIRXE4jKxlAmQc4QjFZoq1KmQqQg0J/1JF8RlFvJas1VcjLv\n" \
                                                    "YlvUB2t6npO6oQjB3l+PNf0DpQH7iUx3Wz5AjQCi6L25FjyE06q6BZ/QlmtYdl/8\n" \
                                                    "ZYao4SRqPEs/6cAiF+Qf5zg2UkaWtDphl1LKMuTNLotvsX99HP69V2faNyegodQ0\n" \
                                                    "LyTApr/vT01YPE46vNsDLgK+4cL6TrzC/a4WcmF5SRJ938zrv/duJHLXQIku5v0+\n" \
                                                    "EwOy59Hdm0PT/Er/84dDV0CSjdR/2XuZM3kpysSKLgD1cKiDA+IRguODCxfO9cyY\n" \
                                                    "Ig46v9mFmBvyH04=\n" \
                                                    "-----END CERTIFICATE-----\n";


```


## Using Certifiable

From the command prompt just type "certifiable ?" (without the quotes) to see the help as mostly reproduced below:<br>

certifiable [ host (-p n) (-d) (-n n) (-g x) (-v x) (-c) (-w) (-o) (-f) ] | [ ] | [ ? ]<br>

host&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;the host name or IP address from which to get the certificate(s)<br>
       for example: google.com, www.google.com, 142.251.41.14<br>
       a host value is required in all cases other than displaying this help information<br>

Options:<br>

 -p n &nbsp; &nbsp; &nbsp; &nbsp;the host's port number<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;for example: -p 8096<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;if not used a port number of 443 will be assumed<br>

 -d &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp; display the host's certificate(s) with unique number for each<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;if neither -c or -o options are used, the -d option will be assumed<br>

 -f &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp;  &nbsp;by default future dated certificates are excluded<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;if -f is used, then future dated certificates will be included<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;note: expired certificates are always excluded<br>

 -n n&nbsp; &nbsp; &nbsp; &nbsp;in many cases a host will have multiple certificates<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;-n indicates a specific certificate should be used for code generation, the second n specifies which one<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;for example, -n 3<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;indicates only the 3rd certificate should be used for code generation<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;if not used, code for all certificates will be generation<br>

 -g x &nbsp; &nbsp; &nbsp; &nbsp; used to specifying the language in which the code will be generated<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;supported languages are: c++, python, and vb.net<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;for example: -g vb.net<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Additionally, c++ may be followed by the option progmem<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;if c++ is followed by progmem the generated code will store the certificate in program memory<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;(flash memory) rather than in RAM (SRAM)<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;for example: -g c++ progmem<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;if not used, the c++ will be assumed<br>

 -v x &nbsp; &nbsp; &nbsp; &nbsp; the variable name to be used in the output file<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;for example: -v server_root_cert<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;if not used, a variable name based on the host name / IP address will be generated<br>

 -c &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp; the generated code will be copied to the clipboard<br>

 -w x &nbsp; &nbsp; &nbsp; &nbsp;the generated code will be written to the specified (path and) file name<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;for example, -w c:\temp\certificate.h<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;the output path name is optional, if omitted the output file will be written to the current working directory<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;for example, -w certificate.h<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;if the path is specified but does not exist, it will be created<br>

 -o &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp; if the -w option is used and the specified file already exists<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;the user will be prompted to overwrite the file unless the -o options is used<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;in which case the existing file will be automatically overwritten<br>
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;for example, -w c:\temp\certificate.h -o<br>

 Certifiable used with no arguments, or with '?' displays this help<br>

Final notes on usage:<br>
&nbsp; the -d option does not generate code, rather simply displays available information<br>
&nbsp; code will only be generated if the -c or -w options are used<br>

<br>
Some common examples:<br>
&nbsp; certifiable ?<br>
&nbsp; certifiable www.google.com<br>
&nbsp; certifiable 142.251.41.14 -d<br>
&nbsp; certifiable github.com -d<br>
&nbsp; certifiable github.com -c<br>
&nbsp; certifiable github.com -n 2 -g c++ progmem -c<br>
&nbsp; certifiable github.com -n 2 -g c++ -v github_cert -c<br>
&nbsp; certifiable github.com -n 2 -g python -c<br>
&nbsp; certifiable github.com -n 2 -g vb.net -w certificate.h<br>
&nbsp; certifiable github.com -w certificate.h -o<br>


## Components

Certifiable is built upon the .net framework 8

Certifiable makes use of TextCopy which is licensed under the MIT License
https://github.com/CopyText/TextCopy

* * *
 ## Support Certifiable

 To help support Certifiable, or to just say thanks, you're welcome to 'buy me a coffee'<br><br>
[<img alt="buy me  a coffee" width="200px" src="https://cdn.buymeacoffee.com/buttons/v2/default-blue.png" />](https://www.buymeacoffee.com/roblatour)
* * *
Copyright © 2025, Rob Latour
* * *
