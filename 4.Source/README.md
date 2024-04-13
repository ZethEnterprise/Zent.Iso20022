This folder contains the actual Visual Studio project for creating the ISO20022 Data Model to include in your project.

# 2024-04-10
I FINALLY CRACKED IT! *"Cracked what?"*, you might say. I'll tell ya. My initial plan was to generate classes based on this ISO-model, which I've just made, and then use T4 templates for code generation. T4 templates are very handy for code generation and the normal usage of them is to execute them on the compile-time. In this case I didn't see the benefit to make it on the compile-time as the idea is to use it as a NuGet or a Tool for generating just the files, which you need. Therefore cloning this repo and generate the code from there was not the initial idea. Therefore I've looked into the runtime T4 template, which is something you can call on the fly and generate a set of files based on that. This is more to my liking as we can now utilize it as a tool like the famous xsd.exe tool or something similar if that works for you as well.

Anyway, my journey started a bit bumpy; copied my parser and internal model to a PoC solution; created a PoC runtime T4 template. This worked more or less out of the box, but now things got interesting. How do I feed this T4 template pieces of my model? That let me to a long time researching with examples and other peoples experiences. I have to feed the T4 template some parameters and I did that in multiple ways. I ran into a lot of errors, where I was missing either this DLL or that one (I will try to add the names at some point for the Google Webcrawler to pick them up and speed up your own research). There were various ways people had tried it and some also said that a normal .Net Core or above could not run with the T4 runtime template parameterized (and that is completely bonkers - because I actually got it to work). To keep it short: Import System.Codedom; create a T4 runtime Template file; build; create a U-SQL class file; make it partial to the T4 Template file and BOOM! now you have the parameterized T4 template for runtime transformation. *...sighing so ever lightly...* Yeah - I was beginning to loose any hope of using the T4s and just make it myself. But now the PoC works and I can begin to explore the model (and it is running with .Net 8!!!).

# 2024-04-13
So the first class has been sort of generated. So now there is some small details getting added to the thing. So let's see if we can make that first thingy serializable... HA! Lovely! Mr. Breaker is back in business. Just like LinQPad had issues due to the sheer size of the file '20220520_ISO20022_2013_eRepository.iso20022' (and rendering afterwards), so have I met my match in Visual Studio. The XML editor can only take in 10MB of a file, but luckily you can change the registry key and change it to something larger.

Execute this in a PowerShell window in order to make the setting for a larger size (in this case it is 100MB):

$vsWherePath = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
$installPath = &$vsWherePath -all -latest -property installationPath
$vsregedit = Join-Path $installPath 'Common7\IDE\vsregedit.exe'
& $VsRegEdit set "$installPath" "HKLM" "XmlEditor" "MaxFileSizeSupportedByLanguageService" string 100

 One thing that is a bit interesting is the identification of the schema namespace => How should we do that???