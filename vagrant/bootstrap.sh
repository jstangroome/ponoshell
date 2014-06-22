#!/usr/bin/env bash

apt-get update

## BEGIN build mono b7adbe3
apt-get install mono-complete git build-essential autoconf automake libtool --assume-yes

mkdir --parents /home/vagrant/mono
pushd /home/vagrant/mono
git clone https://github.com/mono/mono.git . --no-checkout
git checkout b7adbe3140562c7ebe2d14b7ad51026c2af22dae
./autogen.sh --prefix=/opt/mono --enable-nls=no
make
make install
popd
#END build mono