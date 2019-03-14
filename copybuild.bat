xcopy /y /s /e "WatsonAI\WatsonAI\bin\Debug\netcoreapp2.1" "unity\Basic\Assets\DLLs"
del "unity\Basic\Assets\DLLs\OpenNLP.dll"
del "unity\Basic\Assets\DLLs\SharpEntropy.dll"
del "unity\Basic\Assets\DLLs\SharpWordNet.dll"

pause