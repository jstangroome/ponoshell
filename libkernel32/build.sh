#!/usr/bin/env bash
#TODO replace with MAKE

pushd `dirname $0` > /dev/null
SCRIPTROOT=`pwd`
popd > /dev/null

mkdir $SCRIPTROOT/bin/ > /dev/null
gcc -o $SCRIPTROOT/bin/libkernel32.so -fPIC -shared $SCRIPTROOT/kernel32.c 