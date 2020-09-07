FROM mcr.microsoft.com/mssql/server:2017-latest-ubuntu

COPY ./db/start-up.sh /start-up.sh
COPY ./db/init-data /init-data

RUN chmod +x /start-up.sh
