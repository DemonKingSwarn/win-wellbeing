# win-wellbeing

## Installation

### Scoop

Make sure [scoop](https://scoop.sh) is installed in your system

```sh
scoop bucket add demon https://github.com/DemonKingSwarn/flix-cli-bucket.git
scoop install win-wellbeing
```

### Github Releases

You can download it from [Releases](https://github.com/DemonKingSwarn/win-wellbeing/releases)

## Usage

### Monitor Mode

```sh
Start-Process win-wellbeing -ArgumentList "-d" -WindowStyle Hidden
```

### Show Stats

```sh
win-wellbeing --show
```


## Autostart with Windows

- Press `Windows+R`
- type `shell:startup` and press enter
- Make a new file called `win-wellbeing-startup.ps1` and put the following in it:

```sh
powershell.exe -NoProfile -ExecutionPolicy Bypass -Command "Start-Process win-wellbeing -ArgumentList '-d' -WindowStyle Hidden"
```

save and exit
