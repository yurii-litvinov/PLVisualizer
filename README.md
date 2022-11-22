# Pedagogical Load Visualizer

## Это временный readme.md

### Что нужно сделать сейчас, чтобы все заработало:

Поставить в переменную окружения `GOOGLE_APPLICATION_CREDENTIALS` путь к скачанным `CREDENTIALS` в формате `.json` из [Google Cloud Console](https://console.cloud.google.com/)
```console
git clone --recurse-submodules https://github.com/oveeernight/PLVisualizer
```
```console
dotnet build
```
```console
npm install
```
```console
cd PLVisualizer.Api
cd bin
cd Debug
cd net6.0
startProduction
```
Из корня репозитория:
```console
cd frontend
npm start
```

