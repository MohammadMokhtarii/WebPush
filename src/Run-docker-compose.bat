@echo off


set "host_file_path=%SystemRoot%\System32\drivers\etc\hosts"


echo 127.0.0.1 pushdb >> "%host_file_path%"

echo Hosts added to hosts file.

echo Starting Docker Compose...
docker-compose up --build