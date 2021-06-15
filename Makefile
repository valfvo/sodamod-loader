SHELL := powershell.exe
.SHELLFLAGS := -NoProfile -Command

NET_VERSION = net48
SD2_PATH = C:/Program Files (x86)/Steam/steamapps/common/Soda Dungeon 2

PROJECT_DIR = SodaMod
PROJECT_NAME = SodaModLoader
PROJECT_FILE = ./$(PROJECT_DIR)/$(PROJECT_NAME).csproj

DLL_NAME = $(PROJECT_NAME).dll
DLL_SRC_PATH = ./$(PROJECT_DIR)/bin/Release/$(NET_VERSION)/publish/$(DLL_NAME)
DLL_DEST_PATH = $(SD2_PATH)/SodaModLoader/$(DLL_NAME)

all: release deploy run

build:
	dotnet build $(PROJECT_FILE)

release:
	dotnet publish $(PROJECT_FILE) -c Release

deploy:
	cp "$(DLL_SRC_PATH)" "$(DLL_DEST_PATH)"

run:
	Start-Process "$(SD2_PATH)/SodaDungeon2.exe"

PRETTY_PIPE_DIR = $(PROJECT_DIR)/IO
PRETTY_PIPE_BIN_DIR = $(PRETTY_PIPE_DIR)/bin

pretty-pipe:
	mkdir -Force $(PRETTY_PIPE_BIN_DIR) | Out-Null
	gcc $(PRETTY_PIPE_DIR)/pretty_pipe.c -o $(PRETTY_PIPE_BIN_DIR)/pretty_pipe

run-pipe:
	./$(PRETTY_PIPE_BIN_DIR)/pretty_pipe sodamod-pipe

start-pipe:
	Start-Process -NoNewWindow ./$(PRETTY_PIPE_BIN_DIR)/pretty_pipe -ArgumentList sodamod-pipe

stop-pipe:
	Stop-Process -Name "pretty_pipe"

CLEAN_ITEMS = $(PROJECT_DIR)/bin, $(PROJECT_DIR)/obj, $(PRETTY_PIPE_BIN_DIR)
CLEAN_FLAGS = -Recurse -Force -ErrorAction Ignore

clean:
	rm $(CLEAN_ITEMS) $(CLEAN_FLAGS)
