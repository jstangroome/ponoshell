#!/usr/bin/env bash
#TODO replace with MAKE

pushd `dirname $0` > /dev/null
SCRIPTROOT=`pwd`
popd > /dev/null

MONO_BIN=/opt/mono/bin

MCS=$MONO_BIN/mcs

$MCS -out:Tests.dll -platform:x64 -target:library -reference:$SCRIPTROOT/../packages/NUnit.2.6.3/lib/nunit.framework.dll $SCRIPTROOT/MyClass.cs

MONO=$MONO_BIN/mono

$MONO --runtime=v4.0 $SCRIPTROOT/../tools/NUnit-2.6.3/bin/nunit-console.exe Tests.dll