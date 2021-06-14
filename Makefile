NET_VERSION = net48
SD2_PATH = "C:/Program Files (x86)/Steam/steamapps/common/Soda Dungeon 2"

PROJECT_FOLDER = SodaMod
PROJECT_NAME = SodaModLoader
PROJECT_FILE = $(PROJECT_FOLDER)/$(PROJECT_NAME).csproj

DLL_NAME = SodaModLoader.dll
DLL_SRC_PATH = "./$(PROJECT_FOLDER)/bin/Release/$(NET_VERSION)/publish/$(DLL_NAME)"
DLL_DEST_PATH = $(SD2_PATH)"/SodaModLoader/$(DLL_NAME)"

all: publish deploy run

build:
	dotnet build $(PROJECT_FILE)

publish:
	dotnet publish $(PROJECT_FILE) -c Release

deploy:
	cp $(DLL_SRC_PATH) $(DLL_DEST_PATH)

run:
	$(SD2_PATH)/SodaDungeon2.exe

clean:
	powershell rm -Recurse -Force $(PROJECT_FOLDER)/bin
	powershell rm -Recurse -Force $(PROJECT_FOLDER)/obj
