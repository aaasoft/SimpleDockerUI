# SimpleDockerUI

说明
--------------------
配置文件是web.properties

服务端docker运行命令
--------------------
docker run\
 --name=SimpleDockerUI\
 -d\
 --log-driver=none\
 --restart=always\
 -p "127.0.0.1:8021:3000"\
 -v /etc/timezone:/etc/timezone\
 -v /etc/localtime:/etc/localtime\
 -e "LANG=zh_CN.UTF-8"\
 -v /data/SimpleDockerUI:/root\
 -v /var/run/docker.sock:/var/run/docker.sock\
 -v /var/log/docker-SimpleDockerUI:/var/log\
 -w /root\
 microsoft/dotnet:2.1-runtime\
 dotnet /root/Launcher.dll
