#!/usr/bin/env bash
#TODO replace with MAKE

pushd `dirname $0` > /dev/null
SCRIPTROOT=`pwd`
popd > /dev/null

$SCRIPTROOT/libkernel32/build.sh || exit 1

#TODO nuget restore

$SCRIPTROOT/Tests/build.sh
