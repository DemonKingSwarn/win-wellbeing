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

