#!/usr/bin/env bash
#TODO replace with MAKE

pushd `dirname $0` > /dev/null
SCRIPTROOT=`pwd`
popd > /dev/null

MONO_BIN=/opt/mono/bin

XBUILD=$MONO_BIN/xbuild
MONO=$MONO_BIN/mono

cp --verbose $SCRIPTROOT/../libkernel32/bin/libkernel32.so bin/
$XBUILD $SCRIPTROOT/ProofOfConcept.csproj

#MONO_LOG_LEVEL=debug 
MONO_REGISTRY_PATH=$SCRIPTROOT/../Tests/registry $MONO --runtime=v4.0 $SCRIPTROOT/bin/ProofOfConcept.exe