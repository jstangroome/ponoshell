#!/usr/bin/env bash
#TODO replace with MAKE

pushd `dirname $0` > /dev/null
SCRIPTROOT=`pwd`
popd > /dev/null

MONO_BIN=/opt/mono/bin

XBUILD=$MONO_BIN/xbuild
MONO=$MONO_BIN/mono

cp --verbose $SCRIPTROOT/../libkernel32/bin/libkernel32.so bin/
$XBUILD $SCRIPTROOT/Tests.csproj || exit 1

NUNIT_CONSOLE=`readlink --canonicalize $SCRIPTROOT/../tools/NUnit-2.6.3/bin/nunit-console.exe`
MONO_REGISTRY_PATH=$SCRIPTROOT/../registry $MONO --runtime=v4.0 $NUNIT_CONSOLE $SCRIPTROOT/bin/Tests.dll