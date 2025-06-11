# A linux script to build the mod.
MODNAME=KomachiMod_linux
DIRNAME=$MODNAME
DEST=~/.steam/steam/steamapps/common/LBoL/BepInEx/plugins/$DIRNAME
DLL=$MODNAME.dll

# Build the mod
dotnet build KomachiMod_linux.csproj
 
# Copy the files in DirResources to the destination
mkdir -p $DEST
cp -R DirResources/* $DEST
cp README.md $DEST
cp manifest.json $DEST
cp bin/Debug/netstandard2.1/$DLL $DEST

# Compress the archive (to upload to thunderstore)
cd $DEST
zip -r $DIRNAME.zip *

