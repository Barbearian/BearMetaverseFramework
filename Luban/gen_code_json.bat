set WORKSPACE="..\My Project"

set GEN_CLIENT=Luban.ClientServer\Luban.ClientServer.exe
set CONF_ROOT=Config

%GEN_CLIENT% -j cfg --^
 -d %CONF_ROOT%\Defines\__root__.xml ^
 --input_data_dir %CONF_ROOT%\Data ^
 --output_code_dir %WORKSPACE%/Assets/Gen ^
 --output_data_dir %WORKSPACE%/Assets/GenerateData/json ^
 --gen_types code_cs_unity_json,data_json ^
 -s all 

pause