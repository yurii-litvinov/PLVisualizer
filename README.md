# Pedagogical Load Visualizer

## Это временный readme.md

### Что нужно сделать сейчас, чтобы все заработало:
- Поставить в переменную окружения `GOOGLE_APPLICATION_CREDENTIALS` путь к скачанным `CREDENTIALS` в формате `.json` из [Google Cloud Console](https://console.cloud.google.com/)
- Убедиться, что установлен [Docker](https://www.docker.com/)
```console
git clone --recurse-submodules https://github.com/oveeernight/PLVisualizer
cd PLVisualizer
```
Ставим сертификат
```console
mkdir cert
dotnet dev-certs https --clean
dotnet dev-certs https -ep ./certs/https/cert.pfx -p aspnet
```
Запускаем приложение
```console
docker compose up
```
