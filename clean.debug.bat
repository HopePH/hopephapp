REM This is the fix for debugging not hitting. Manually clean all the files in your project.
REM Just change the root folder and good to go

@ECHO OFF
for /d /r . %%d in (bin,obj) do @if exist "%%d" rd /s/q "%%d"
pause

REM If in case there is permission error for the above just delete the files inside instead
set root="C:\_Work.Dir.GitHub.HopePH.BDD\hopephapp"
del /F /Q /S "%root%\Yol.Punla.Common\bin\Debug\*"
del /F /Q /S "%root%\Yol.Punla.Business\bin\Debug\*"
del /F /Q /S "%root%\Yol.Punla\Yol.Punla\bin\Debug\*"
del /F /Q /S "%root%\Yol.Punla\Yol.Punla.Droid\bin\Debug\*"
del /F /Q /S "%root%\Yol.Punla\Yol.Punla.iOS\bin\iPhoneSimulator\Debug\*"

REM cross platformbase
del /F /Q /S "%root%\Yol.Punla.Platform\bin\Debug\*"
del /F /Q /S "%root%\Yol.Punla.Platform._Droid\bin\Debug\*"
del /F /Q /S "%root%\Yol.Punla.Platform._IOS\bin\Debug\*"
del /F /Q /S "%root%\Yol.Punla.Platform.Interface\bin\Debug\*"

REM clean release
del /F /Q /S "%root%\Yol.Punla.Common\bin\Release\*"
del /F /Q /S "%root%\Yol.Punla.Business\bin\Release\*"
del /F /Q /S "%root%\Yol.Punla\Yol.Punla\bin\Release\*"
del /F /Q /S "%root%\Yol.Punla\Yol.Punla.Droid\bin\Release\*"
del /F /Q /S "%root%\Yol.Punla\Yol.Punla.iOS\bin\iPhoneSimulator\Release\*"
pause